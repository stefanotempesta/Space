using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BiometricSecurity.Recognition
{
    public class VoiceRecognition : IRecognition, IDisposable
    {
        public VoiceRecognition()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("<API Endpoint>");
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "<Subscription Key>");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public double Recognize(string siteId, out string name)
        {
            ReadAudioStream(siteId, out Stream audioStream);

            Guid[] enrolledProfileIds = GetEnrolledProfilesAsync();
            string operationUri = IdentifyAsync(audioStream, enrolledProfileIds).Result;

            IdentificationOperation status = null;
            do
            {
                status = CheckIdentificationStatusAsync(operationUri).Result;
                Thread.Sleep(100);
            } while (status == null);

            Guid profileId = status.ProcessingResult.IdentifiedProfileId;

            VoiceVerificationResponse verification = VerifyAsync(profileId, audioStream).Result;
            if (verification == null)
            {
                name = string.Empty;
                return 0;
            }

            Profile profile = GetProfileAsync(profileId).Result;
            name = profile.Name;

            return ToConfidenceScore(verification.Confidence);
        }

        private Guid[] GetEnrolledProfilesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<VoiceVerificationResponse> VerifyAsync(Guid profileId, Stream audioStream)
        {
            string now = DateTime.Now.ToString("u");
            var payload = new MultipartFormDataContent("Upload----" + now)
            {
                { new StreamContent(audioStream), "verificationData", profileId.ToString("D") + "_" + now }
            };

            var requestUri = "/spid/v1.0/verify?verificationProfileId=" + profileId.ToString("D");
            HttpResponseMessage response = await _httpClient.PostAsync(requestUri, payload);

            string content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<VoiceVerificationResponse>(content);
        }

        public async Task<IdentificationOperation> CheckIdentificationStatusAsync(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<IdentificationOperation>(content);
        }

        public async Task<string> IdentifyAsync(Stream audioStream, Guid[] profileIds)
        {
            string now = DateTime.Now.ToString("u");
            var payload = new MultipartFormDataContent("Upload----" + now)
            {
                { new StreamContent(audioStream), "Data", "testFile_" + now }
            };

            string identificationProfileIds = string.Join(",", profileIds.Select(id => id.ToString("D")));
            string requestUri = "/spid/v1.0/identify?identificationProfileIds=" + identificationProfileIds;

            HttpResponseMessage response = await _httpClient.PostAsync(requestUri, payload);
            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                return string.Empty;
            }

            return response.Headers.GetValues("Operation-Location").First();
        }

        public async Task<Profile> GetProfileAsync(Guid profileId)
        {
            string requestUri = "/spid/v1.0/identificationProfiles/" + profileId.ToString("D");
            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            string content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Profile>(content);
        }

        private double ToConfidenceScore(Confidence confidence)
        {
            return (double)confidence / 100.0d;
        }

        private void ReadAudioStream(string siteId, out Stream audioStream)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private HttpClient _httpClient;
    }
}

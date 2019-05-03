using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace BiometricSecurity.Recognition
{
    public class FaceRecognition : IRecognition
    {
        public double Recognize(string siteId, out string name)
        {
            FaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials("<Subscription Key>"))
            {
                Endpoint = "<API Endpoint>"
            };

            ReadImageStream(siteId, out Stream imageStream);

            // Detect faces in the image
            IList<DetectedFace> detectedFaces = faceClient.Face.DetectWithStreamAsync(imageStream).Result;

            // Too many faces detected
            if (detectedFaces.Count > 1)
            {
                name = string.Empty;
                return 0;
            }

            IList<Guid> faceIds = detectedFaces.Select(f => f.FaceId.Value).ToList();

            // Identify faces
            IList<IdentifyResult> identifiedFaces = faceClient.Face.IdentifyAsync(faceIds, "<Person Group ID>").Result;

            // No faces identified
            if (identifiedFaces.Count == 0)
            {
                name = string.Empty;
                return 0;
            }

            // Get the first candidate (candidates are ranked by confidence)
            IdentifyCandidate candidate = identifiedFaces.Single().Candidates.FirstOrDefault();

            // Find the person
            Person person = faceClient.PersonGroupPerson.GetAsync("", candidate.PersonId).Result;
            name = person.Name;

            return candidate.Confidence;
        }

        private void ReadImageStream(string siteId, out Stream imageStream)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningServiceDisplay
{
    internal class AzureMLExperimentServiceClient
    {
        public static void Run()
        {
            InvokeRequestResponseService().Wait();
        }

        static readonly string serviceEndpoint = ConfigurationManager.AppSettings["ServiceEndpoint"];
        static readonly string apiKey = ConfigurationManager.AppSettings["ApiKey"];

        static async Task InvokeRequestResponseService()
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, StringTable>() {
                        {
                            "inputData",
                            new StringTable()
                            {
                                ColumnNames = new string[] {"Date", "Object", "Hits"},
                                Values = new string[,] {
                                    { "2017/02/17", "a7c0aa02-2049-4b66-a41e-9cd559ea739a", "9000" },
                                    { "2017/02/17", "c1c4582c-abd6-4846-9a49-f381e8897578", "6000" },
                                }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri(serviceEndpoint);

                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Result: {0}", result);
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
            }
        }
    }
}

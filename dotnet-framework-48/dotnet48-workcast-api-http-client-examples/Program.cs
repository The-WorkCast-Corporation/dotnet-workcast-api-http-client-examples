using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace dotnet48_workcast_api_http_client_examples
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Usage: dotnet run <baseUrl> <bearerToken> <skipValue> <limitValue>");
                return;
            }

            string baseUrl = args[0];
            string bearerToken = args[1];
            int skipValue = int.Parse(args[2]);
            int limitValue = int.Parse(args[3]);

            var httpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };

            var client = new HttpClient(httpClientHandler);
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}?Skip={skipValue}&Limit={limitValue}");
            request.Headers.Add("Authorization", $"Bearer {bearerToken}");

            var response = await client.SendAsync(request);

            // Check if response is a redirect
            if ((int)response.StatusCode >= 300 && (int)response.StatusCode < 400)
            {
                // Check if the Location header is present
                if (response.Headers.Location != null)
                {
                    // Extract the relative path of the redirected URL
                    string redirectedRelativePath = response.Headers.Location.OriginalString;

                    // Extract the host from the original request URL
                    Uri originalUri = new Uri(baseUrl);
                    string host = originalUri.GetLeftPart(UriPartial.Authority);

                    // Combine the host with the relative path to form the full redirected URL
                    string fullRedirectedUrl = $"{host}{redirectedRelativePath}";

                    // Create a new request with the full redirected URL
                    request = new HttpRequestMessage(HttpMethod.Get, fullRedirectedUrl);
                    request.Headers.Add("Authorization", $"Bearer {bearerToken}");

                    // Send the second request
                    response = await client.SendAsync(request);
                }
                else
                {
                    Console.WriteLine("Redirected response does not contain a Location header.");
                }
            }

            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Pretty-print the JSON response
                await PrettyPrintJsonResponse(response);
            }
            else
            {
                Console.WriteLine($"Failed to retrieve data. Status code: {response.StatusCode}");
            }
        }

        static async Task PrettyPrintJsonResponse(HttpResponseMessage response)
        {
            // Read the JSON response as a string
            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON string
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            object result = serializer.Deserialize(jsonResponse, typeof(object));

            // Serialize the JSON document back to a formatted JSON string
            string prettyJson = serializer.Serialize(result);

            // Print the formatted JSON string
            Console.WriteLine(prettyJson);
        }
    }
}
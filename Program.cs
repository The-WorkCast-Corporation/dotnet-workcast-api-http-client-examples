using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

class Program
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

        var httpClientHandler = new HttpClientHandler();
        httpClientHandler.AllowAutoRedirect = false;

        var client = new HttpClient(httpClientHandler);
        var request = new HttpRequestMessage(HttpMethod.Get, baseUrl);
        request.Headers.Add("Authorization", $"Bearer {bearerToken}");
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(skipValue.ToString()), "Skip");
        content.Add(new StringContent(limitValue.ToString()), "Limit");
        request.Content = content;

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
                var redirectedRequest = new HttpRequestMessage(HttpMethod.Get, fullRedirectedUrl);
                redirectedRequest.Headers.Add("Authorization", $"Bearer {bearerToken}");

                // Send the second request
                var redirectedResponse = await client.SendAsync(redirectedRequest);
                redirectedResponse.EnsureSuccessStatusCode();

                // Pretty-print the JSON response
                await PrettyPrintJsonResponse(redirectedResponse);
            }
            else
            {
                Console.WriteLine("Redirected response does not contain a Location header.");
            }
        }
        else
        {
            // Pretty-print the JSON response
            await PrettyPrintJsonResponse(response);
        }
    }

    static async Task PrettyPrintJsonResponse(HttpResponseMessage response)
    {
        // Read the JSON response as a stream
        using (var responseStream = await response.Content.ReadAsStreamAsync())
        {
            // Deserialize the JSON stream
            using (var jsonDocument = await JsonDocument.ParseAsync(responseStream))
            {
                // Serialize the JSON document back to a formatted JSON string
                string prettyJson = JsonSerializer.Serialize(jsonDocument.RootElement, new JsonSerializerOptions { WriteIndented = true });

                // Print the formatted JSON string
                Console.WriteLine(prettyJson);
            }
        }
    }
}

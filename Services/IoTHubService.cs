using Microsoft.Azure.Devices.Common.Security;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2.Services
{
    public class IoTHubService
    {
        private readonly string _iotHubUri = "laweit.azure-devices.net";
        private readonly string _deviceId = "ducklin";
        private readonly string _sas = "SharedAccessSignature sr=laweit.azure-devices.net&sig=vulpuQ6Fw9VL8Di9L0%2FhlrV5Zrti5Z2WXPe4l8sIQHk%3D&se=1704139826&skn=iothubowner";

        public IoTHubService()
        {
            // Additional initialization if needed
        }

        public async Task InvokeDirectMethodAsync(string methodName)
        {
            try
            {
                // Construct the URL
                string url = $"https://{_iotHubUri}/twins/{_deviceId}/methods?api-version=2020-09-30";

                // Prepare the request payload
                string payload = $"{{\"methodName\":\"{methodName}\",\"payload\":\"YourPayload\"}}";

                // Make the HTTP POST request
                string sasToken = await GetSasTokenAsync();
                await MakeHttpRequestAsync(url, sasToken, payload);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error invoking method: {ex.Message}");
                // Log the exception details for further analysis
                throw;
            }
        }
        public async Task<string> GetSasTokenAsync()
        {
            try
            {
                var sasBuilder = new SharedAccessSignatureBuilder
                {
                    Key = "kB4eT7Z3mywudplS0emKsNOtGcJjVaEBlAIoTJ8uK3Q=",
                    Target = "laweit.azure-devices.net", // Use only the IoT Hub URI, not the entire connection string
                    TimeToLive = TimeSpan.FromHours(1) // Adjust the expiration time as needed
                };

                return sasBuilder.ToSignature();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating SAS token: {ex.Message}");
                throw;
            }
        }

        private async Task MakeHttpRequestAsync(string url, string sasToken, string payload)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Set the authorization header with the SAS token
                    client.DefaultRequestHeaders.Add("Authorization", _sas);

                    // Create StringContent with the payload and set content type
                    var content = new StringContent(payload, Encoding.UTF8, "application/json");

                    // Make the HTTP POST request with the payload
                    var response = await client.PostAsync(url, content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the success response
                        var result = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Method invoked successfully. Response: {result}");
                    }
                    else
                    {
                        // Handle the error response
                        Console.WriteLine($"Error invoking method. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error making HTTP request: {ex.Message}");
                // Log the exception details for further analysis
                throw;
            }
        }

    }
}

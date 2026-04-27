using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    /// Service class to communicate with Python Gesture Recognition API
    /// </summary>
    public class GestureService
    {
        private readonly string _apiBaseUrl;
        private HttpClient _httpClient;
        private JsonSerializerOptions _jsonOptions;

        public GestureService(string apiUrl = "http://127.0.0.1:5000")
        {
            _apiBaseUrl = apiUrl.TrimEnd('/');
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        /// <summary>
        /// Start camera processing
        /// </summary>
        public async Task<bool> StartAsync(string comPort = "COM3", int baudrate = 9600)
        {
            try
            {
                var payload = new { com_port = comPort, baudrate = baudrate };
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/start", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Start error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Stop camera processing
        /// </summary>
        public async Task<bool> StopAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/stop", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Stop error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get current frame and gesture data
        /// </summary>
        public async Task<GestureFrame> GetFrameAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/frame");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<GestureFrame>(json, _jsonOptions);
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetFrame error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Get engine status
        /// </summary>
        public async Task<StatusResponse> GetStatusAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/status");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<StatusResponse>(json, _jsonOptions);
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetStatus error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Health check
        /// </summary>
        public async Task<bool> HealthCheckAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Response model for frame data
    /// </summary>
    public class GestureFrame
    {
        [JsonPropertyName("frame")]
        public string Frame { get; set; } // Base64 encoded JPEG image

        [JsonPropertyName("gesture")]
        public string Gesture { get; set; } // Gesture name

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; } // Confidence percentage (0-100)

        [JsonPropertyName("led_status")]
        public bool LedStatus { get; set; } // LED status

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } // ISO 8601 timestamp
    }

    /// <summary>
    /// Response model for status
    /// </summary>
    public class StatusResponse
    {
        [JsonPropertyName("running")]
        public bool Running { get; set; }

        [JsonPropertyName("gesture")]
        public string Gesture { get; set; }

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }

        [JsonPropertyName("led_status")]
        public bool LedStatus { get; set; }
    }
}

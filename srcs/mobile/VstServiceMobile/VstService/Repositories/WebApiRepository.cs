namespace VstService.Repositories
{
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using VstService.Interfaces;
    using VstService.Models;

    /// <summary>
    /// Web api repository
    /// </summary>
    public partial class WebApiRepository : IWebApiRepository
    {
        private const int TIMEOUT_API = 10;
        private readonly HttpClient _httpClient;
        private string? _baseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiRepository"/> class.
        /// </summary>
        public WebApiRepository()
        {
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback
                = (sender, cert, chain, sslPolicyErrors) => { return true; },
            };

            _httpClient = new HttpClient(clientHandler);
            _httpClient.Timeout = TimeSpan.FromMinutes(TIMEOUT_API);
        }

        /// <summary>
        /// Get base url
        /// </summary>
        /// <returns>Base Url string</returns>
        public string GetBaseUrl() => _baseUrl ?? string.Empty;

        /// <summary>
        /// Set base url
        /// </summary>
        /// <param name="baseUrl">string baseUrl</param>
        public void SetBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        /// <summary>
        /// clear header
        /// </summary>
        public void ClearHeader()
            => _httpClient.DefaultRequestHeaders.Clear();

        /// <summary>
        /// remove header
        /// </summary>
        /// <param name="key">header key</param>
        public void RemoveHeader(string key)
            => _httpClient.DefaultRequestHeaders.Remove(key);

        /// <summary>
        /// add header
        /// </summary>
        /// <param name="key">key header</param>
        /// <param name="value">value header</param>
        public void AddHeader(string key, string value)
            => _httpClient.DefaultRequestHeaders.Add(key, value);

        /// <summary>
        /// add bear token
        /// </summary>
        /// <param name="token">token</param>
        public void AddBearer(string token)
            => _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        /// <summary>
        /// add basic
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="password">user password</param>
        public void AddBasic(string username, string password)
        {
            string credentials = $"{username}:{password}";
            string encodedCredentials
                = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Basic", encodedCredentials);
        }

        /// <summary>
        /// add type
        /// </summary>
        /// <param name="type">type</param>
        public void AddType(string type = "application/json")
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue(type));
        }

        /// <summary>
        /// set timeout
        /// </summary>
        /// <param name="second">second time out</param>
        public void SetTimeout(int second) => _httpClient.Timeout = TimeSpan.FromSeconds(second);

        public void SetMessageServerDown(string message)
        {
            this._serverDown = message;
        }
    }

    /// <summary>
    /// implement method api http
    /// </summary>
    public partial class WebApiRepository
    {
        private string _serverDown = "Cannot connect to server. Please restart Application again";

        /// <summary>
        /// Get method
        /// </summary>
        /// <param name="endpoint">endpoint</param>
        /// <param name="token">token</param>
        /// <param name="mediaType">mediaType</param>
        /// <returns>BaseApiResponse</returns>
        public async Task<BaseApiResponse> Get(string endpoint, CancellationToken? token = null, string mediaType = "application/json")
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint, cancellationToken: token ?? CancellationToken.None);

                object value = mediaType == "application/zip"
                    ? (object)await response.Content.ReadAsByteArrayAsync()
                    : await response.Content.ReadAsStringAsync();

                var r = new BaseApiResponse
                {
                    Status = response.StatusCode,
                    Value = value,
                };

                return r;
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Request was cancelled.");
                return new BaseApiResponse
                {
                    Status = HttpStatusCode.GatewayTimeout,
                    Value = JsonConvert.SerializeObject(new BaseResponse
                    {
                        Status = HttpStatusCode.GatewayTimeout,
                        Error = "Request was cancelled.",
                    }),
                };
            }
            catch (Exception)
            {
                return new BaseApiResponse
                {
                    Status = HttpStatusCode.BadGateway,
                    Value = JsonConvert.SerializeObject(new BaseResponse
                    {
                        Status = HttpStatusCode.BadGateway,
                        Error = _serverDown,
                    }),
                };
            }
        }

        /// <summary>
        /// Post method
        /// </summary>
        /// <param name="endpoint">endpoint</param>
        /// <param name="payload">payload</param>
        /// <param name="mediaType">mediaType</param>
        /// <param name="token">token</param>
        /// <returns>BaseApiResponse</returns>
        public async Task<BaseApiResponse> Post(string endpoint, string payload, string mediaType = "application/json", CancellationToken? token = null)
        {
            HttpContent content = new StringContent(payload, Encoding.UTF8);
            if (mediaType is null)
                mediaType = "application/json";
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content, cancellationToken: token ?? CancellationToken.None);
                var value = await response.Content.ReadAsStringAsync();

                var r = new BaseApiResponse
                {
                    Status = response.StatusCode,
                    Value = value,
                };
                return r;
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Request was cancelled.");
                return new BaseApiResponse
                {
                    Status = HttpStatusCode.GatewayTimeout,
                    Value = JsonConvert.SerializeObject(new BaseResponse
                    {
                        Status = HttpStatusCode.GatewayTimeout,
                        Error = "Request was cancelled.",
                    }),
                };
            }
            catch (Exception)
            {
                return new BaseApiResponse
                {
                    Status = HttpStatusCode.BadGateway,
                    Value = JsonConvert.SerializeObject(new BaseResponse
                    {
                        Status = HttpStatusCode.BadGateway,
                        Error = _serverDown,
                    }),
                };
            }
        }

        /// <summary>
        /// Put method
        /// </summary>
        /// <param name="endpoint">endpoint</param>
        /// <param name="payload">payload</param>
        /// <param name="mediaType">mediaType</param>
        /// <param name="token">token</param>
        /// <returns>BaseApiResponse</returns>
        public async Task<BaseApiResponse> Put(string endpoint, string payload, string mediaType = "application/json", CancellationToken? token = null)
        {
            HttpContent content = new StringContent(payload, Encoding.UTF8);
            if (mediaType is null)
                mediaType = "application/json";
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            try
            {
                HttpResponseMessage response = await _httpClient.PutAsync(endpoint, content, cancellationToken: token ?? CancellationToken.None);
                var r = new BaseApiResponse
                {
                    Status = response.StatusCode,
                    Value = await response.Content.ReadAsStringAsync(),
                };
                return r;
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Request was cancelled.");
                return new BaseApiResponse
                {
                    Status = HttpStatusCode.GatewayTimeout,
                    Value = JsonConvert.SerializeObject(new BaseResponse
                    {
                        Status = HttpStatusCode.GatewayTimeout,
                        Error = "Request was cancelled.",
                    }),
                };
            }
            catch (Exception)
            {
                return new BaseApiResponse
                {
                    Status = HttpStatusCode.BadGateway,
                    Value = JsonConvert.SerializeObject(new BaseResponse
                    {
                        Status = HttpStatusCode.BadGateway,
                        Error = _serverDown,
                    }),
                };
            }
        }

        /// <summary>
        /// Delete method
        /// </summary>
        /// <param name="endpoint">endpoint</param>
        /// <param name="token">token</param>
        /// <returns>BaseApiResponse</returns>
        public async Task<BaseApiResponse> Delete(string endpoint, CancellationToken? token = null)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint, cancellationToken: token ?? CancellationToken.None);
                var r = new BaseApiResponse
                {
                    Status = response.StatusCode,
                    Value = await response.Content.ReadAsStringAsync(),
                };
                return r;
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Request was cancelled.");
                return new BaseApiResponse
                {
                    Status = HttpStatusCode.GatewayTimeout,
                    Value = JsonConvert.SerializeObject(new BaseResponse
                    {
                        Status = HttpStatusCode.GatewayTimeout,
                        Error = "Request was cancelled.",
                    }),
                };
            }
            catch (Exception)
            {
                return new BaseApiResponse
                {
                    Status = HttpStatusCode.BadGateway,
                    Value = JsonConvert.SerializeObject(new BaseResponse
                    {
                        Status = HttpStatusCode.BadGateway,
                        Error = _serverDown,
                    }),
                };
            }
        }

        /// <summary>
        /// Patch method
        /// </summary>
        /// <param name="endpoint">endpoint</param>
        /// <param name="payload">payload</param>
        /// <param name="mediaType">mediaType</param>
        /// <param name="token">token</param>
        /// <returns>BaseApiResponse</returns>
        public async Task<BaseApiResponse> Patch(string endpoint, string payload, string mediaType = "application/json", CancellationToken? token = null)
        {
            HttpContent content = new StringContent(payload, Encoding.UTF8);
            if (mediaType is null)
                mediaType = "application/json";
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            try
            {
                HttpResponseMessage response = await _httpClient.PatchAsync(endpoint, content, cancellationToken: token ?? CancellationToken.None);
                var r = new BaseApiResponse
                {
                    Status = response.StatusCode,
                    Value = await response.Content.ReadAsStringAsync(),
                };
                return r;
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Request was cancelled.");
                return new BaseApiResponse
                {
                    Status = HttpStatusCode.GatewayTimeout,
                    Value = JsonConvert.SerializeObject(new BaseResponse
                    {
                        Status = HttpStatusCode.GatewayTimeout,
                        Error = "Request was cancelled.",
                    }),
                };
            }
            catch (Exception)
            {
                return new BaseApiResponse
                {
                    Status = HttpStatusCode.BadGateway,
                    Value = JsonConvert.SerializeObject(new BaseResponse
                    {
                        Status = HttpStatusCode.BadGateway,
                        Error = _serverDown,
                    }),
                };
            }
        }
    }
}

using AquilaService.Models;

namespace AquilaService.Interfaces;


/// <summary>
/// interface WebApiRepository
/// </summary>
public interface IWebApiRepository
{
    /// <summary>
    /// Set timeout to call api
    /// </summary>
    /// <param name="second">second</param>
    public void SetTimeout(int second);

    /// <summary>
    /// get base url in httpClient
    /// </summary>
    /// <returns>string url</returns>
    public string GetBaseUrl();

    /// <summary>
    /// set base url in httpClient
    /// </summary>
    /// <param name="baseUrl">baseUrl</param>
    public void SetBaseUrl(string baseUrl);

    /// <summary>
    /// clear all header
    /// </summary>
    public void ClearHeader();

    /// <summary>
    /// remove header with key
    /// </summary>
    /// <param name="key">key</param>
    public void RemoveHeader(string key);

    /// <summary>
    /// add header with key and value
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="value">value</param>
    public void AddHeader(string key, string value);

    /// <summary>
    /// add bearer token
    /// </summary>
    /// <param name="token">token</param>
    public void AddBearer(string token);

    /// <summary>
    /// add basic with username and password
    /// </summary>
    /// <param name="username">username</param>
    /// <param name="password">password</param>
    public void AddBasic(string username, string password);

    /// <summary>
    /// add type header application/json, application/xml, text/plain, ...
    /// </summary>
    /// <param name="type">type</param>
    public void AddType(string type = "application/json");

    /// <summary>
    /// set message response when server down
    /// </summary>
    /// <param name="message">message</param>
    public void SetMessageServerDown(string message);

    /// <summary>
    /// call method get with endpoint
    /// </summary>
    /// <param name="endpoint">endpoint</param>
    /// <param name="token">token</param>
    /// <param name="mediaType">mediaType</param>
    /// <returns>BaseApiResponse</returns>
    public Task<BaseApiResponse> Get(string endpoint, CancellationToken? token = null, string mediaType = "application/json");

    /// <summary>
    /// call method post with endpoint and payload
    /// </summary>
    /// <param name="endpoint">endpoint</param>
    /// <param name="payload">payload</param>
    /// <param name="mediaType">mediaType</param>
    /// <param name="token">token</param>
    /// <returns>BaseApiResponse</returns>
    public Task<BaseApiResponse> Post(string endpoint, string payload, string mediaType = "application/json", CancellationToken? token = null);

    /// <summary>
    /// call method put with endpoint and payload
    /// </summary>
    /// <param name="endpoint">endpoint</param>
    /// <param name="payload">payload</param>
    /// <param name="mediaType">mediaType</param>
    /// <param name="token">token</param>
    /// <returns>BaseApiResponse</returns>
    public Task<BaseApiResponse> Put(string endpoint, string payload, string mediaType = "application/json", CancellationToken? token = null);

    /// <summary>
    /// call method delete with endpoint
    /// </summary>
    /// <param name="endpoint">endpoint</param>
    /// <param name="token">token</param>
    /// <returns>BaseApiResponse</returns>
    public Task<BaseApiResponse> Delete(string endpoint, CancellationToken? token = null);

    /// <summary>
    /// call method patch with endpoint and payload
    /// </summary>
    /// <param name="endpoint">endpoint</param>
    /// <param name="payload">payload</param>
    /// <param name="mediaType">mediaType</param>
    /// <param name="token">token</param>
    /// <returns>BaseApiResponse</returns>
    public Task<BaseApiResponse> Patch(string endpoint, string payload, string mediaType = "application/json", CancellationToken? token = null);
}


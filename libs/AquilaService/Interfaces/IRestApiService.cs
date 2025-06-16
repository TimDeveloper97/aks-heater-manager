using AquilaService.Models;

namespace AquilaService.Interfaces;

/// <summary>
/// interfaces rest api service
/// </summary>
public interface IRestApiService
{
    /// <summary>
    /// Rest api call to web api service and manager API
    /// </summary>
    /// <typeparam name="TResponse">mode response</typeparam>
    /// <typeparam name="TModel">model success</typeparam>
    /// <typeparam name="TResult">model return</typeparam>
    /// <param name="method">type method</param>
    /// <param name="requestData">data request to API</param>
    /// <param name="onSuccess">function on success</param>
    /// <param name="onFailure">function on failure</param>
    /// <param name="isShowPopup">is show pop up when fail</param>
    /// <returns>TResult</returns>
    Task<TResult> RequestAPI<TResponse, TModel, TResult>(
        ERMethod method,
        RData requestData,
        Func<ResponseSuccess<TModel>, Task<TResult>>? onSuccess = null,
        Func<ResponseFailure, Task>? onFailure = null,
        bool isShowPopup = true)
        where TResponse : BaseResponse;
}

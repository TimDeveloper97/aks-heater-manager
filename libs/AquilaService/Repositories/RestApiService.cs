namespace AquilaService.Repositories;

using AquilaService.Interfaces;
using AquilaService.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

/// <summary>
/// Rest api service
/// </summary>
public partial class RestApiService : IRestApiService
{
    private readonly IWebApiRepository _webApiRepository;
    private readonly ILogger<RestApiService> _logger;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="RestApiService"/> class.
    /// </summary>
    /// <param name="serviceProvider">serviceProvider</param>
    public RestApiService(IServiceProvider serviceProvider)
    {
        _webApiRepository = serviceProvider.GetRequiredService<IWebApiRepository>();
        _logger = serviceProvider.GetRequiredService<ILogger<RestApiService>>();
        _serviceProvider = serviceProvider;
    }
}

/// <summary>
/// Rest api service version 2
/// </summary>
public partial class RestApiService
{
    /// <inheritdoc/>
    public async Task<TResult> RequestAPI<TResponse, TModel, TResult>(
        ERMethod method,
        RData requestData,
        Func<ResponseSuccess<TModel>, Task<TResult>>? onSuccess = null,
        Func<ResponseFailure, Task>? onFailure = null,
        bool isShowPopup = true)
        where TResponse : BaseResponse
    {
        try
        {
            var responseAPI = await CallApi(method, requestData);

            var statusCode = (int)responseAPI.Status!;

            var baseResponse = JsonConvert.DeserializeObject<TResponse>(responseAPI.Value?.ToString() ?? string.Empty);
            var jsonResponseValue = baseResponse?.Value?.ToString();

            if (baseResponse is null
                || baseResponse.Status is null)
            {
                return default!;
            }

            _logger?.LogInformation(baseResponse.Message);

            var responseStatusCode = int.Parse(baseResponse.Status.ToString() ?? "-1");
            var message = baseResponse.Message;
            dynamic warning = baseResponse.Warning?.ToString() ?? string.Empty;
            var error = baseResponse.Error;
            string? buttonOk = null;

            if (responseStatusCode < 400)
            {
                var isString = typeof(TModel) == typeof(string);
                dynamic? responseValue = null;

                if (isString)
                {
                    responseValue = jsonResponseValue;
                }
                else if (jsonResponseValue is not null && AquilaService.TextHelper.IsJson(jsonResponseValue))
                {
                    responseValue = JsonConvert.DeserializeObject<TModel>(jsonResponseValue);
                }

                if (onSuccess is not null && responseStatusCode != (int)HttpStatusCode.NoContent)
                {
                    dynamic? result = null;

                    if (onSuccess != null)
                    {
                        result = await onSuccess(new ResponseSuccess<TModel>
                        {
                            Model = responseValue,
                            Message = message,
                            Warning = warning,
                        });
                    }

                    return result ?? default!;
                }
            }
            else
            {
                var title = "ERROR: " ?? string.Empty;

                //switch (statusCode)
                //{
                //    case (int)HttpStatusCode.Unauthorized:
                //        error = RestApiResponseMessage.UNAUTHORIZED;
                //        break;

                //    // MBTG-1930 update message not connet to server
                //    case (int)HttpStatusCode.BadGateway:
                //        error = RestApiResponseMessage.BADGATEWAY;
                //        title = RestApiResponseMessage.TITLE_BADGATEWAY;
                //        buttonOk = RestApiResponseMessage.BUTTON_BADGATEWAY;
                //        break;
                //}

                var failResp = new ResponseFailure
                {
                    Warning = warning,
                    Error = error,
                    StatusCode = statusCode,
                };

                if (onFailure != null)
                    await onFailure(failResp);

                var unauthorized = statusCode == (int)HttpStatusCode.Unauthorized;
                if (isShowPopup || unauthorized)
                {
                    //MainThread.BeginInvokeOnMainThread(async () =>
                    //{
                    //    await Shell.Current.ShowPopupAsync(new PopupGeneric(
                    //        serviceProvider: _serviceProvider,
                    //        Models.EPopupType.ConfirmAlertPopup,
                    //        title: title,
                    //        message: failResp.Errors?.ToString() ?? string.Empty,
                    //        buttonOk: buttonOk));

                    //    // MBTG-1916: close app when token expired
                    //    // notes: neu muon handle rieng cho "validate" va "generate" thi show popup khac cho cac api khac
                    //    if (unauthorized)
                    //    {
                    //        Application.Current?.Quit();
                    //    }
                    //});
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
#if DEBUG
            Debug.WriteLine(ex);
#endif
        }

        return default!;
    }
}

/// <summary>
/// Request call API
/// </summary>
public partial class RestApiService
{
    private async Task<BaseApiResponse> CallApi(ERMethod method, RData requestData)
    {
        if (requestData.Endpoint is null)
            throw new Exception("Endpoint is null.");

        switch (method)
        {
            case ERMethod.Get:
                return await _webApiRepository.Get(requestData.Endpoint, requestData.Token, requestData.MediaType);
            case ERMethod.Post:
                return await _webApiRepository.Post(requestData.Endpoint, requestData.Payload, requestData.MediaType, requestData.Token);
            case ERMethod.Put:
                return await _webApiRepository.Put(requestData.Endpoint, requestData.Payload, requestData.MediaType, requestData.Token);
            case ERMethod.Delete:
                return await _webApiRepository.Delete(requestData.Endpoint, requestData.Token);
            case ERMethod.Patch:
                return await _webApiRepository.Patch(requestData.Endpoint, requestData.Payload, requestData.MediaType, requestData.Token);
            default:
                return new BaseApiResponse();
        }
    }
}

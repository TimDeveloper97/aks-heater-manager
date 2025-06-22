namespace VstService.Models
{
    /// <summary>
    /// response success
    /// </summary>
    /// <typeparam name="TModel">Model</typeparam>
    public class ResponseSuccess<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Gets or sets model
        /// </summary>
        public TModel? Model { get; set; }

        /// <summary>
        /// Gets or sets message
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets warning
        /// </summary>
        public string? Warning { get; set; }
    }

    /// <summary>
    /// response fail
    /// </summary>
    public class ResponseFailure
    {
        /// <summary>
        /// Gets or sets warning
        /// </summary>
        public string? Warning { get; set; }

        /// <summary>
        /// Gets or sets errors
        /// </summary>
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public object? Error { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

        /// <summary>
        /// Gets or sets status Code
        /// </summary>
        public int StatusCode { get; set; } = 400;
    }
}
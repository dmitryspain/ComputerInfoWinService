using NLog;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace ComputerInfoWebApi.Filters
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public override void Handle(ExceptionHandlerContext context)
        {
            var result = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(context.Exception.Message),
                ReasonPhrase = "InternalServerError"
            };

            _logger.Error(context.Exception.Message);
            context.Result = new HttpResult(context.Request, result);
        }

        public class HttpResult : IHttpActionResult
        {
            private readonly HttpRequestMessage _request;
            private readonly HttpResponseMessage _httpResponseMessage;

            public HttpResult(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
            {
                _request = request;
                _httpResponseMessage = httpResponseMessage;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_httpResponseMessage);
            }
        }
    }
}
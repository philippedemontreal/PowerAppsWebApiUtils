using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PowerAppsWebApiUtils.Tests
{
    public class FakeHttpMessageHandler : DelegatingHandler
    {
        public string Content { get; private set; }
        public HttpRequestMessage Request { get; private set; }
        public HttpResponseMessage Response { get; set; }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            Content = await request.Content.ReadAsStringAsync();
            return await Task.FromResult(Response);
        }
    }

}
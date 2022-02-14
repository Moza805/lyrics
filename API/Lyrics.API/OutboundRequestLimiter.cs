namespace Lyrics.API
{
    public class OutboundRequestLimiter : DelegatingHandler
    {

        private SemaphoreSlim _limiter;

        public OutboundRequestLimiter(SemaphoreSlim limiter)
        {
            _limiter = limiter;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await _limiter.WaitAsync(cancellationToken);
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            finally
            {
                _limiter.Release();
            }
        }
    }
}
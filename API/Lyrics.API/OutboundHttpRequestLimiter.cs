namespace Lyrics.API
{
    /// <summary>
    /// Rate limit Http requests
    /// </summary>
    /// <seealso cref="https://stackoverflow.com/a/52053084" />
    public class OutboundHttpRequestLimiter : DelegatingHandler
    {

        private SemaphoreSlim _limiter;

        /// <summary>
        /// New up an instance of this request limiter class
        /// </summary>
        /// <param name="maxConcurrentRequests">Number of HTTP requests that can be made at the same time</param>
        public OutboundHttpRequestLimiter(int maxConcurrentRequests)
        {
            _limiter = new SemaphoreSlim(maxConcurrentRequests);
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
namespace Net_Beginner_web_app.Services
{
    public class ApiService :DelegatingHandler
    {
        private readonly IHttpContextAccessor ContextAccessor;

        public ApiService(IHttpContextAccessor contextAccessor) {
            ContextAccessor = contextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = ContextAccessor.HttpContext?.Session.GetString("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}

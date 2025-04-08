using System.Net.Http.Json;

namespace WebApi.Test
{
    public class ForumClassFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public ForumClassFixture(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        protected async Task<HttpResponseMessage> DoPost(string endpoint, object? request, string token = "", string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthorizeRequest(token);

            return await _httpClient.PostAsJsonAsync(endpoint, request);
        }

        protected async Task<HttpResponseMessage> DoPut(string endpoint, object request, string token = "", string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthorizeRequest(token);

            return await _httpClient.PutAsJsonAsync(endpoint, request);
        }

        protected async Task<HttpResponseMessage> DoDelete(string endpoint, string token = "", string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthorizeRequest(token);

            return await _httpClient.DeleteAsync(endpoint);
        }

        protected async Task<HttpResponseMessage> DoGet(string endpoint, string token, string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthorizeRequest(token);

            return await _httpClient.GetAsync(endpoint);
        }

        private void ChangeRequestCulture(string culture)
        {
            if (_httpClient.DefaultRequestHeaders.AcceptLanguage != null)
            {
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
            }

            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
        }

        private void AuthorizeRequest(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}

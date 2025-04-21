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

        protected async Task<HttpResponseMessage> DoPostFormData(string endpoint, object request, string culture = "en")
        {
            ChangeRequestCulture(culture);

            var formContent = BuildFormDataContent(request);

            return await _httpClient.PostAsync(endpoint, formContent);
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

        private static MultipartFormDataContent BuildFormDataContent(object request)
        {
            var multipartContent = new MultipartFormDataContent();

            var requestProperties = request.GetType().GetProperties().ToList();

            foreach (var property in requestProperties)
            {
                var propertyValue = property.GetValue(request);

                if (string.IsNullOrWhiteSpace(propertyValue?.ToString()))
                {
                    continue;
                }

                multipartContent.Add(new StringContent(propertyValue.ToString()!), property.Name);
            }

            return multipartContent;
        }
    }
}

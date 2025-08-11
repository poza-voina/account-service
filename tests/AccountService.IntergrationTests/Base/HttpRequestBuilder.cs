using System.Text;
using System.Text.Json;

namespace AccountService.IntegrationTests.Base;

public class HttpRequestBuilder(HttpMethod method, string url)
{
    private const string ContentType = "application/json";
    private readonly HttpRequestMessage _request = new(method, url);

    public HttpRequestBuilder WithContent(HttpContent content)
    {
        _request.Content = content;
        return this;
    }

    public HttpRequestBuilder WithJsonContent<T>(T obj)
    {
        var json = JsonSerializer.Serialize(obj);
        _request.Content = new StringContent(json, Encoding.UTF8, ContentType);
        return this;
    }

    public HttpRequestMessage Build() => _request;
}
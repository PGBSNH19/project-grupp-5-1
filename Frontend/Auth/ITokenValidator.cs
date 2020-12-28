using System.Net.Http;
using System.Threading.Tasks;

namespace Frontend.Auth
{
    public interface ITokenValidator
    {
        Task<HttpClient> CheckToken(HttpClient http);
    }
}

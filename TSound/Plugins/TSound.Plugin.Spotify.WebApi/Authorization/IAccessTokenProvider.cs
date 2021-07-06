using System.Threading.Tasks;

namespace TSound.Plugin.Spotify.WebApi.Authorization
{
    public interface IAccessTokenProvider
    {

        Task<string> GetAccessToken();
    }
}

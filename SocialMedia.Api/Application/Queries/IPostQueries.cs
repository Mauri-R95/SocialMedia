using SocialMedia.Api.Application.Model;
using System.Threading.Tasks;

namespace SocialMedia.Api.Application.Queries
{
    public interface IPostQueries
    {
        Task<PostDto> GetPostById(int id);
    }
}

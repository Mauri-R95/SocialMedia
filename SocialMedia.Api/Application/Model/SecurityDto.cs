using SocialMedia.Core.Enumerations;

namespace SocialMedia.Api.Application.Model
{
    public class SecurityDto
    {
        public string User { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public RoleType? Role { get; set; }
    }
}

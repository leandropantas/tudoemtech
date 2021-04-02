using SofTracerAPI.Models.Projects;

namespace SoftTracerAPI.Models
{
    public class ProjectUser
    {

        public string UserId { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }

    }
}
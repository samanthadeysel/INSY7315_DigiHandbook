using Digital_Handbook_Portal.Models;

namespace Digital_Handbook_Portal.ViewModels
{
    public class UserManagementViewModel
    {
        public IEnumerable<User> Users { get; set; } = new List<User>();
        public IEnumerable<UserSession> Sessions { get; set; } = new List<UserSession>();
    }
}

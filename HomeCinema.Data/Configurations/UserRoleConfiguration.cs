using HomeCinema.Entities;

namespace HomeCinema.Data.Configurations
{
    public class UserRoleConfiguration : EntityBaseConfiguration <UserRole>
    {
        public UserRoleConfiguration()
        {
            Property(ur => ur.RoleId).IsRequired();
            Property(ur => ur.UserId).IsRequired();
        }
    }
}

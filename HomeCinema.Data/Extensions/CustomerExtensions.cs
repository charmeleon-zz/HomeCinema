using HomeCinema.Data.Repositories;
using HomeCinema.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.Data.Extensions
{
    public static class CustomerExtensions
    {
        public static bool CustomerExists(this IEntityBaseRepository<Customer> customer, string email,
            string identityCard)
        {
            return customer.GetAll()
                .Where(
                    c => c.Email.ToLower() == email 
                    || c.IdentityCard.ToLower() == identityCard)
                .Count() > 0;
        }
    }
}

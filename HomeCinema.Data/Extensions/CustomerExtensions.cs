using HomeCinema.Data.Repositories;
using HomeCinema.Entities;
using System;
using System.Linq;

namespace HomeCinema.Data.Extensions
{
    public static class CustomerExtensions
    {
        public static bool CustomerExists(this IEntityBaseRepository<Customer> repository, string email,
            string identityCard)
        {
            var count = repository.GetAll()
                .Where(
                    c => c.Email.ToLower() == email 
                    || c.IdentityCard.ToLower() == identityCard)
                .Count();

            return count > 0;
        }

        public static string GetCustomerFullName(this IEntityBaseRepository<Customer> repository, 
            int customerId)
        {
            string fullName = string.Empty;
            var customer = repository.GetSingle(customerId);

            fullName = customer.FirstName + ' ' + customer.LastName;

            return fullName;
        }
    }
}

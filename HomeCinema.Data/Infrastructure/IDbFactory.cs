using System;
using HomeCinema.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        HomeCinemaContext Init();
    }
}

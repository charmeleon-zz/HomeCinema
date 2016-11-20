using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        HomeCinemaContext dbContext;
        public HomeCinemaContext Init()
        {
            return dbContext ?? (dbContext = new HomeCinemaContext());
        }
        protected override void DisposeCore()
        {
            //base.DisposeCore();
            if (null != dbContext)
            {
                dbContext.Dispose();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HomeCinema.Web.Infrastructure.Extensions;
using System.Web;
using System.Security.Principal;
using HomeCinema.Services;

namespace HomeCinema.Web.Infrastructure
{
    public class HomeCinemaAuthHandler : DelegatingHandler
    {
        IEnumerable<string> authHeaderValues = null;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            MembershipContext membershipContext;
            try
            {
                request.Headers.TryGetValues("Authorization", out authHeaderValues);
                if (null == authHeaderValues)
                {
                    return base.SendAsync(request, cancellationToken);
                }

                var tokens = authHeaderValues.FirstOrDefault();
                tokens = tokens.Replace("Basic", "").Trim();

                if (!String.IsNullOrEmpty(tokens))
                {
                    byte[] data = Convert.FromBase64String(tokens);
                    string decodedString = Encoding.UTF8.GetString(data);
                    string[] tokensValues = decodedString.Split(':');
                    var membershipService = request.GetMembershipService();

                    membershipContext = membershipService.ValidateUser(tokensValues[0], tokensValues[1]);
                    if (null != membershipContext.User)
                    {
                        IPrincipal principal = membershipContext.Principal;
                        Thread.CurrentPrincipal = principal;
                        HttpContext.Current.User = principal;
                        return base.SendAsync(request, cancellationToken);
                    }
                    else
                    {
                        // Wrong credentials
                        var response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                        var tsc = new TaskCompletionSource<HttpResponseMessage>();
                        tsc.SetResult(response);
                        return tsc.Task;
                    }
                }
                else
                {
                    var response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(response);
                    return tsc.Task;
                }
            }
            catch
            {
                var response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
            }
        }
    }
}
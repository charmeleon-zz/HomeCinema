using System.Web.Mvc;
using HomeCinema.Data.Infrastructure;
using HomeCinema.Data.Repositories;
using HomeCinema.Entities;
using System.Net.Http;
using HomeCinema.Data.Extensions;
using AutoMapper;
using HomeCinema.Web.Models;
using System.Collections.Generic;
using System.Net;

namespace HomeCinema.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("/api/stocks")]
    public class StocksController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Stock> stockRepository;
        public StocksController(IEntityBaseRepository<Stock> stockRepository,
            IEntityBaseRepository<Error> errorRepository, IUnitOfWork unitOfWork)
            : base(errorRepository, unitOfWork)
        {
            this.stockRepository = stockRepository;
        }

        [Route("movie/{id:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int movieId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var movie = stockRepository.GetAvailableItems(movieId);
                var stocksVm = Mapper.Map<IEnumerable<Stock>, 
                    IEnumerable<StockViewModel>>(movie);

                response = request.CreateResponse(HttpStatusCode.OK, stocksVm);

                return response;
            });
        }
    }
}
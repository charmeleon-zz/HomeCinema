using System.Web.Mvc;
using HomeCinema.Data.Infrastructure;
using HomeCinema.Data.Repositories;
using HomeCinema.Entities;
using System.Collections.Generic;
using HomeCinema.Web.Models;
using System.Net.Http;
using System.Net;
using System;

namespace HomeCinema.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/rentals")]
    public class RentalsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Rental> rentalRepository;
        private readonly IEntityBaseRepository<Customer> customerRepository;
        private readonly IEntityBaseRepository<Stock> stockRepository;
        private readonly IEntityBaseRepository<Movie> movieRepository;

        public RentalsController(
            IEntityBaseRepository<Rental> rentalRepository,
            IEntityBaseRepository<Customer> customerRepository,
            IEntityBaseRepository<Stock> stockRepository,
            IEntityBaseRepository<Movie> movieRepository,
            IEntityBaseRepository<Error> errorRepository, IUnitOfWork unitOfWork) 
            : base(errorRepository, unitOfWork)
        {
            this.rentalRepository = rentalRepository;
            this.customerRepository = customerRepository;
            this.stockRepository = stockRepository;
            this.movieRepository = movieRepository;
        }

        [HttpGet]
        [Route("{id:int}/rentalhistory")]
        public HttpResponseMessage RentalHistory(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
           {
               HttpResponseMessage response = null;
               var rentalHistory = GetMovieRentalHistory(id);

               response = request.CreateResponse
                (HttpStatusCode.OK, rentalHistory);
               
               return response;
           });

        }

        private List<RentalHistoryViewModel> GetMovieRentalHistory(int movieId)
        {
            var rentalHistory = new List<RentalHistoryViewModel>();
            var rentals = new List<Rental>();
            var movie = movieRepository.GetSingle(movieId);

            foreach (var stock in movie.Stocks)
            {
                rentals.AddRange(stock.Rentals);
            }

            foreach (var rental in rentals)
            {
                var historyItem = new RentalHistoryViewModel()
                {
                    ID = rental.ID,
                    StockId = rental.StockId,
                    RentalDate = rental.RentalDate,
                    ReturnedDate = rental.ReturnedDate.HasValue ? rental.ReturnedDate : null,
                    Status = rental.Status,
                    Customer = customerRepository.GetCustomerFullName(rental.CustomerId)
                };

                rentalHistory.Add(historyItem);
            }

            rentalHistory.Sort((r1, r2) => r2.RentalDate.CompareTo(r1.RentalDate));

            return rentalHistory;
        }

        [HttpPost]
        [Route("return/{rentalId:int}")]
        public HttpResponseMessage Return(HttpRequestMessage request, int rentalId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var rental = rentalRepository.GetSingle(rentalId);

                if (null == rental)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Invalid rental");
                }
                else
                {
                    rental.Status = "Returned";
                    rental.Stock.IsAvailable = true;
                    rental.ReturnedDate = DateTime.Now;

                    _unitOfWork.Commit();

                    response = request.CreateResponse(HttpStatusCode.OK);
                }

                return response;
            });
        }
    }
}
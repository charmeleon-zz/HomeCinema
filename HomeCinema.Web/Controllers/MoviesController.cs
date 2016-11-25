using AutoMapper;
using HomeCinema.Data.Infrastructure;
using HomeCinema.Data.Repositories;
using HomeCinema.Entities;
using HomeCinema.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HomeCinema.Web.Controllers
{
    public class MoviesController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Movie> movieRepository;

        public MoviesController(IEntityBaseRepository<Movie> movieRepo, 
            IEntityBaseRepository<Error> errorRepo, IUnitOfWork unitOfWork)
            : base(errorRepo, unitOfWork)
        {
            movieRepository = movieRepo;
        }

        [AllowAnonymous]
        [Route("latest")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var movies = movieRepository.GetAll().OrderByDescending(m => m.ReleaseDate).Take(6).ToList();
                var moviesVm = Mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(movies);

                response = request.CreateResponse<IEnumerable<MovieViewModel>>(HttpStatusCode.OK, moviesVm);

                return response;
            });
        }
    }
}

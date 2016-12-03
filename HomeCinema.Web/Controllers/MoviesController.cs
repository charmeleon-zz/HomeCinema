using AutoMapper;
using HomeCinema.Data.Infrastructure;
using HomeCinema.Data.Repositories;
using HomeCinema.Entities;
using HomeCinema.Web.Infrastructure.Core;
using HomeCinema.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HomeCinema.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/movies")]
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
        [Route("{page:int=0}/{pageSize=3}/{filter?}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int? page,
            int? pageSize, string filter = null)
        {
            int currentPage = page.Value;
            int currentPageSize = pageSize.Value;

            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                List<Movie> movies = null;

                if (string.IsNullOrEmpty(filter))
                {
                    movieRepository.GetAll().OrderByDescending(m => m.ReleaseDate).Take(6).ToList();
                }
                else
                {
                    movieRepository.GetAll()
                        .OrderBy(m => m.ID)
                        .Where(m => m.Title.ToLower()
                            .Contains(filter.ToLower().Trim()))
                        .ToList();
                }
                // Paginate
                int totalMovies = movies.Count();
                movies = movies.Skip(currentPage * currentPageSize)
                    .Take(currentPageSize).ToList();

                var moviesVm = Mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(movies);
                var pagedSet = new PaginationSet<MovieViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalMovies,
                    TotalPages = (int)Math.Ceiling((decimal)totalMovies / currentPageSize),
                    Items = moviesVm
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [Route("details/{id:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var movie = movieRepository.GetSingle(id);
                var movieVm = Mapper.Map<Movie, MovieViewModel>(movie);

                response = request.CreateResponse<MovieViewModel>
                    (HttpStatusCode.OK, movieVm);

                return response;
            });
        }
    }
}

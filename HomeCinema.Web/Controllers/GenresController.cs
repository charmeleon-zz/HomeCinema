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
using System.Web;
using System.Web.Http;

namespace HomeCinema.Web.Controllers
{
    [Authorize(Roles="Admin")]
    [RoutePrefix("api/genres")]
    public class GenresController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Genre> _genreRepository;
        public GenresController(IEntityBaseRepository<Genre> genreRepo,
            IEntityBaseRepository<Error> errorRepo, IUnitOfWork _unitOfWork)
            : base(errorRepo, _unitOfWork)
        {
            _genreRepository = genreRepo;
        }

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var genres = _genreRepository.GetAll().ToList();
                var genresVm = Mapper.Map<IEnumerable<Genre>, IEnumerable<GenreViewModel>>(genres);

                response = request.CreateResponse<IEnumerable<GenreViewModel>>(HttpStatusCode.OK, genresVm);

                return response;
            });
        }
    }
}
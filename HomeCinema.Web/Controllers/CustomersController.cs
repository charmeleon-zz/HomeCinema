using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeCinema.Data.Infrastructure;
using HomeCinema.Data.Repositories;
using HomeCinema.Entities;
using System.Web.Http;
using System.Net.Http;
using HomeCinema.Web.Models;
using AutoMapper;
using HomeCinema.Web.Infrastructure.Core;
using System.Net;

namespace HomeCinema.Web.Controllers
{
    [Authorize(Roles="Admin")]
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Customer> customerRepository;
        public CustomersController(IEntityBaseRepository<Customer> customerRepository,
            IEntityBaseRepository<Error> errorRepository, IUnitOfWork unitOfWork) :
            base(errorRepository, unitOfWork)
        {
            this.customerRepository = customerRepository;
        }
        [HttpGet]
        [Route("search/{page:int=0}/{pageSize=4}/{filter?}")]
        public HttpResponseMessage Search(HttpRequestMessage request, int? page, 
            int? pageSize, string filter = null)
        {
            int currentPage = page.Value;
            int currentPageSize = pageSize.Value;

            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Customer> customers = null;
                int totalMovies = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();

                    customers = customerRepository.GetAll()
                        .OrderBy(c => c.ID)
                        .Where(c => c.LastName.ToLower().Contains(filter) ||
                        c.FirstName.ToLower().Contains(filter) ||
                        c.IdentityCard.ToLower().Contains(filter)
                        ).ToList();
                }
                else
                {
                    customers = customerRepository.GetAll().ToList();
                }

                totalMovies = customers.Count();
                customers = customers.Skip(currentPage * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();

                IEnumerable<CustomerViewModel> customersVm =
                    Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>> (customers);

                PaginationSet<CustomerViewModel> pagedSet = new PaginationSet<CustomerViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalMovies,
                    TotalPages = (int)Math.Ceiling((decimal)totalMovies / currentPageSize),
                    Items = customersVm
                };

                response = request.CreateResponse<PaginationSet<CustomerViewModel>>(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, CustomerViewModel customer)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                        .Select(m => m.ErrorMessage).ToArray());
                }
                else
                {
                    Customer _customer = customerRepository.GetSingle(customer.ID);
                    // TODO: Implement UpdateCustomer
                    _customer.UpdateCustomer(customer);
                    _unitOfWork.Commit();

                    response = request.CreateResponse(HttpStatusCode.OK);
                }

                return response;
            });
        }

        [HttpPost]
        [Route("register")]
        public HttpResponseMessage Register(HttpRequestMessage request, CustomerViewModel customer)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                            .Select(m => m.ErrorMessage).ToArray());
                }
                else
                {
                    if (customerRepository.UserExists(customer.Email, customer.IdentityCard))
                    {
                        ModelState.AddModelError("Invalid user", "Email or identity card already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                            ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        Customer newCustomer = new Customer();
                        newCustomer.UpdateCustomer(customer);
                        customerRepository.Add(newCustomer);
                        _unitOfWork.Commit();

                        // Update view model by re-mapping the just-committed newCustomer
                        customer = Mapper.Map<Customer, CustomerViewModel>(newCustomer);
                        response = request.CreateResponse<CustomerViewModel>(HttpStatusCode.Created, customer);
                    }
                }

                return response;
            });
        }
    }
}
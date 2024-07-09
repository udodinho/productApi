using System.Net;
using Application.Helper;
using Application.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Product.Application.Dtos;
using Product.Application.Helpers;
using Product.Application.Services.Contracts;
using Product.Domain.Entities;
using Product.Infrastructure.Repository;

namespace Product.Application.Services.Implementation
{
    public class ProductService : IProductService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<PagedResponse<IEnumerable<ProductsResponse>>> Products(ResourceParameters parameter, string name, IUrlHelper urlHelper)
        {
            var products = new List<ProductItem>();

            var query = _unitOfWork.Products.QueryAll();

            if (!string.IsNullOrEmpty(parameter.Search))
            {
                var search = parameter.Search.Trim();
                query = query.Where(x =>
                      x.Name.ToLower().Contains(search)
                      || x.Description.ToLower().Contains(search));
            }

            if (parameter.StartDate != null)
            {
                query = query.Where(x => parameter.StartDate <= x.CreatedAt);
            }

            if (parameter.EndDate != null)
            {
                query = query.Where(x => parameter.EndDate >= x.CreatedAt);
            }

            if (parameter.UserId != null) {
                query = query.Where(x => x.UserId == parameter.UserId);
            }

            products = query.ToList();
            var res = _mapper.Map<List<ProductsResponse>>(products);
            var items = await PagedList<ProductsResponse>.Create(res, parameter.PageNumber, parameter.PageSize);
            var page = PageUtility<ProductsResponse>.CreateResourcePageUrl(parameter, name, items, urlHelper);

            var response = new PagedResponse<IEnumerable<ProductsResponse>>
            {
                Message = "Products retrieved successfully",
                Data = items,
                Meta = new Meta
                {
                    Pagination = page
                }
            };

            return response;
        }

        public async Task<SuccessResponse<ProductsResponse>> CreateProduct(string userId, CreateProductDto model)
        {
            var exists = _unitOfWork.Products.Exists(x => x.Name == model.Name);

            if (exists)
            {
                throw new RestException(HttpStatusCode.BadRequest, $"Product with same name exist");
            }

            var prod = _mapper.Map<ProductItem>(model);
            prod.Id = Guid.NewGuid().ToString();
            prod.UserId = userId;

            await _unitOfWork.Products.AddAsync(prod);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<ProductsResponse>(prod);


            var response = new SuccessResponse<ProductsResponse>
            {
                Message = "Product created successfully",
                Data = res,

            };

            return response;
        }

        public async Task<SuccessResponse<ProductsResponse>> Product(string userId, string productId)
        {

            var product = await _unitOfWork.Products.FirstOrDefaultNoTracking(x => x.UserId == userId && x.Id == productId);

            if (product is null)
            {
                throw new RestException(HttpStatusCode.BadRequest, $"Product does not exist");
            }

            var res = _mapper.Map<ProductsResponse>(product);
            var response = new SuccessResponse<ProductsResponse>
            {
                Message = "Product retrieved successfully",
                Data = res,

            };

            return response;
        }

        public async Task<SuccessResponse<ProductsResponse>> UpdateProduct(string userId, string productId, UpdateProductDto model)
        {
            var product = await _unitOfWork.Products.FirstOrDefaultNoTracking(x => x.UserId == userId && x.Id == productId);

            if (product is null)
            {
                throw new RestException(HttpStatusCode.BadRequest, $"Product does not exist");
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price; 

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<ProductsResponse>(product);


            var response = new SuccessResponse<ProductsResponse>
            {
                Message = "Product updated successfully",
                Data = res,

            };

            return response;
        }

        public async Task<SuccessResponse<string>> DeleteProduct(string userId, string productId)
        {

            var product = await _unitOfWork.Products.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == productId);
            if (product is null)
            {
                throw new RestException(HttpStatusCode.BadRequest, $"Product does not exist");
            }

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.SaveChangesAsync();
            var response = new SuccessResponse<string>
            {
                Message = "Product deleted successfully",

            };

            return response;
        }
    }
}

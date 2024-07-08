using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Dtos;
using Product.Application.Helpers;

namespace Product.Application.Services.Contracts
{
    public interface IProductService
    {
        Task<SuccessResponse<ProductsResponse>> CreateProduct(string userId, CreateProductDto model);
        Task<SuccessResponse<ProductsResponse>> UpdateProduct(string userId, string productId, UpdateProductDto model);
        Task<SuccessResponse<string>> DeleteProduct(string userId, string productId);
        Task<SuccessResponse<ProductsResponse>> Product(string userId, string productId);
        Task<PagedResponse<IEnumerable<ProductsResponse>>> Products(ResourceParameters parameter, string name, IUrlHelper urlHelper);
    }
}

﻿using System.Dynamic;
using System.Reflection;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Helpers;

namespace Application.Helpers
{
    public class PageUtility<T>
    {
        public static Pagination CreateResourcePageUrl(ResourceParameters parameters, string name, PagedList<T> pageData, IUrlHelper helper)
        {
            if (pageData is null) return new Pagination
            {
                Total = 0,
                PageSize = 10,
                TotalPages = 0
            };
            var prevLink = pageData.HasPrevious
                ? CreateResourceUri(parameters, name, ResourceUriType.PreviousPage, helper)
                : null;
            var nextLink = pageData.HasNext
                ? CreateResourceUri(parameters, name, ResourceUriType.NextPage, helper)
                : null;

            var pagination = new Pagination
            {
                NextPage = nextLink,
                PreviousPage = prevLink,
                Total = pageData.TotalCount,
                PageSize = pageData.PageSize,
                TotalPages = pageData.TotalPages
            };
            return pagination;
        }

        private static string CreateResourceUri(ResourceParameters parameter, string name, ResourceUriType type, IUrlHelper url)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return url.Link(name,
                        new
                        {
                            PageNumber = parameter.PageNumber - 1,
                            parameter.PageSize,
                            Search = parameter.Search
                        });

                case ResourceUriType.NextPage:
                    return url.Link(name,
                        new
                        {
                            PageNumber = parameter.PageNumber + 1,
                            parameter.PageSize,
                            Search = parameter.Search
                        });

                default:
                    return url.Link(name,
                        new
                        {
                            parameter.PageNumber,
                            parameter.PageSize,
                            Search = parameter.Search,
                        });
            }

        }

        public static Pagination CreateResourcePageUrl(IDictionary<string, object> parameters, string name, PagedList<T> pageData, IUrlHelper helper)
        {

            var prevLink = pageData.HasPrevious
                ? CreateResourceUri(parameters, name, ResourceUriType.PreviousPage, helper)
                : null;
            var nextLink = pageData.HasNext
                ? CreateResourceUri(parameters, name, ResourceUriType.NextPage, helper)
                : null;

            var pagination = new Pagination
            {
                NextPage = nextLink,
                PreviousPage = prevLink,
                Total = pageData.TotalCount,
                PageSize = pageData.PageSize,
                TotalPages = pageData.TotalPages
            };

            return pagination;
        }

        private static string CreateResourceUri(IDictionary<string, object> parameters, string name, ResourceUriType type, IUrlHelper url)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => url.Link(name, parameters),
                ResourceUriType.NextPage => url.Link(name, parameters),
                _ => url.Link(name, parameters),
            };
        }

        public static IDictionary<string, object> GenerateResourceParameters(dynamic parameter, PagedList<T> events)
        {
            var pageNumber = "PageNumber";
            var dynamicParameter = new ExpandoObject() as IDictionary<string, object>;

            foreach (PropertyInfo param in parameter.GetType().GetProperties())
            {
                if (events.HasNext && param.Name.Equals(pageNumber, StringComparison.OrdinalIgnoreCase))
                {
                    dynamicParameter.Add(param.Name, (int)param.GetValue(parameter, null) + 1);
                }
                else if (events.HasPrevious && param.Name.Equals(pageNumber, StringComparison.OrdinalIgnoreCase))
                {
                    dynamicParameter.Add(param.Name, (int)param.GetValue(parameter, null) - 1);
                }
                else
                {
                    dynamicParameter.Add(param.Name, param.GetValue(parameter, null));
                }
            }

            return dynamicParameter;
        }
    }
}

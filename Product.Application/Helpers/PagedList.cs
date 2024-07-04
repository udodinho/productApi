using Microsoft.EntityFrameworkCore;

namespace Application.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => (CurrentPage > 1);
        public bool HasNext => (CurrentPage < TotalPages);

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static async Task<PagedList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            if (pageSize == 0)
            {
                pageSize = count;
            }


            var items = await source.Skip(((pageNumber - 1) * pageSize)).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);

        }
        public static async Task<PagedList<T>> Create(IList<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            if (pageSize == 0)
            {
                pageSize = count;
            }

            var items = await Task.Run(() => source.Skip(((pageNumber - 1) * pageSize)).Take(pageSize).ToList());
            return new PagedList<T>(items, count, pageNumber, pageSize);

        }

        public static async Task<PagedList<T>> FillRange(PagedList<T> destination, IQueryable<T> source, int pageNumber, int pageSize)
        {
            int minimumRows = 1;
            var take = pageSize - Convert.ToInt32(destination?.TotalCount);
            if (take < minimumRows) return destination;


            var count = source.Count();
            var totalCount = count + destination.TotalCount;
            if (destination.PageSize == 0)
            {
                destination.PageSize = count;
            }


            var items = await Task.Run(() => source.Skip(((pageNumber - 1) * pageSize)).Take(take).ToList());
            destination.AddRange(items);
            return new PagedList<T>(destination, totalCount, pageNumber, pageSize);

        }

        public static async Task<PagedList<T>> FillRange(PagedList<T> destination, IList<T> source, int pageNumber, int pageSize)
        {
            int minimumRows = 1;
            var take = pageSize - Convert.ToInt32(destination?.TotalCount);
            if (take < minimumRows) return destination;

            var count = source.Count();
            var totalCount = count + destination.TotalCount;
            if (destination.PageSize == 0)
            {
                destination.PageSize = count;
            }


            var items = await Task.Run(() => source.Skip(((pageNumber - 1) * pageSize)).Take(take).ToList());
            destination.AddRange(items);
            return new PagedList<T>(destination, totalCount, pageNumber, pageSize);

        }

    }

    public enum ResourceUriType
    {
        PreviousPage,
        NextPage,
        CurrentPage
    }
}

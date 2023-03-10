
namespace Webhooks.ResultObject
{
    public class PagedDataResult<T> : Result, IPagedDataResult<T>
    {
        protected PagedDataResult() { }

        public T Data { get; protected set; }

        /// <summary>
        /// Total count of Items.
        /// </summary>
        public int TotalCount { get; set; }

        public static new IPagedDataResult<T> Fail()
        {
            return new PagedDataResult<T> { Failed = true, Succeeded = false };
        }

        public static new IPagedDataResult<T> Fail(string message)
        {
            return new PagedDataResult<T> { Failed = true, Succeeded = false, Message = message };
        }

        public static new IPagedDataResult<T> Success()
        {
            return new PagedDataResult<T> { Failed = false, Succeeded = true };
        }

        public static new IPagedDataResult<T> Success(string message)
        {
            return new PagedDataResult<T> { Failed = false, Succeeded = true, Message = message };
        }

        public static IPagedDataResult<T> Success(T data)
        {
            return new PagedDataResult<T> { Failed = false, Succeeded = true, Data = data };
        }

        public static IPagedDataResult<T> Success(T data, string message)
        {
            return new PagedDataResult<T> { Failed = false, Succeeded = true, Data = data, Message = message };
        }
    }
}

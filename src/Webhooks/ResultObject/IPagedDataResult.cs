
namespace Webhooks.ResultObject
{
    public interface IPagedDataResult<out T> : IResult
    {
        int TotalCount { get; set; }
        T Data { get; }
    }
}

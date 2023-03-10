namespace Webhooks.ResultObject
{
  public interface IResult
  {
    bool Failed { get; }

    string Message { get; }

    bool Succeeded { get; }
  }
}


namespace Webhooks.ResultObject
{
  [Serializable]
  public class Result : IResult
  {
    protected Result() { }

    public bool Failed { get; protected set; }

    public string Message { get; protected set; }

    public bool Succeeded { get; protected set; }

    public static IResult Fail()
    {
      return new Result { Failed = true, Succeeded = false };
    }

    public static IResult Fail(string message)
    {
      return new Result { Failed = true, Succeeded = false, Message = message };
    }

    public static IResult Success()
    {
      return new Result { Failed = false, Succeeded = true };
    }

    public static IResult Success(string message)
    {
      return new Result { Failed = false, Succeeded = true, Message = message };
    }
  }
}

using ROUMVP.Extensions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace ROUMVP.ResultsAndErrors;

public class ResultOrUnprocessable<TOnSuccess,TEnumError> : IResultOrUnprocessable where TEnumError : Enum
{
    public bool IsSuccess { get; private set; }
    
    public TOnSuccess? Result { get; private set; }
    public UnprocessableHttpRequestInfo? UnprocessableInfo { get; private set; }
    
    public object? ResultObject => Result;
    public object? UnprocessableInfoObject => UnprocessableInfo;

    public static implicit operator ResultOrUnprocessable<TOnSuccess, TEnumError>(TEnumError te)
    {
        var unprocessableInfo = new UnprocessableHttpRequestInfo()
        {
            Code = Convert.ToInt32(te),
            Description = te.GetDescription()
        };
        
        var roe = new ResultOrUnprocessable<TOnSuccess, TEnumError>
        {
            Result = default,
            IsSuccess = false,
            UnprocessableInfo = unprocessableInfo
        };

        return roe;
    }
    
    public static implicit operator ResultOrUnprocessable<TOnSuccess, TEnumError>(TOnSuccess tr)
    {
        var roe = new ResultOrUnprocessable<TOnSuccess, TEnumError>
        {
            Result = tr,
            IsSuccess = true,
        };

        return roe;
    }

    
}
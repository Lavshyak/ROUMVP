namespace ROUMVP.ResultsAndErrors;

#pragma warning disable CS1591
public interface ISuccessOrUnprocessable
{
    public bool IsSuccess { get; }
    
    public object? UnprocessableInfoObject { get; }
}
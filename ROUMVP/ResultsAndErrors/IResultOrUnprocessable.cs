namespace ROUMVP.ResultsAndErrors;

#pragma warning disable CS1591
public interface IResultOrUnprocessable : ISuccessOrUnprocessable
{
    public object? ResultObject { get; }
}
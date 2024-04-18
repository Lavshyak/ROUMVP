using System.ComponentModel;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ROUMVP.ResultsAndErrors;

namespace ROUMVP.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ExampleController : ControllerBase
{
    //OK. returns normal file. octet-stream in headers, raw bytes in body.
    [Produces<FileResult>]
    [HttpGet]
    public FileResult GetFileCommon()
    {
        return File([2, 3, 5, 6, 2], MediaTypeNames.Application.Octet);
    }
    
    public enum SomeErrors
    {
        [Description("A error")]
        A,
        B
    }
    
    //Incorrect. returns json. json in headers, json in body (serialized FileResult)
    [HttpGet]
    public ResultOrUnprocessable<FileResult, SomeErrors> GetFileCustom()
    {
        return File([2, 3, 5, 6, 2], MediaTypeNames.Application.Octet);
    }
    
    //OK. returns json. json in headers, json in body (serialized UnprocessableHttpRequestInfo)
    [HttpGet]
    public ResultOrUnprocessable<FileResult, SomeErrors> GetFileCustomError()
    {
        return SomeErrors.A;
    }
}
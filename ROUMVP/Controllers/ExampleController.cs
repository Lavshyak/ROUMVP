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
    
    //OK. returns json. json in headers, json in body (serialized int)
    [HttpGet]
    public ResultOrUnprocessable<int, SomeErrors> GetIntCustom()
    {
        return 4;
    }

    public class SomeModel
    {
        public int Id { get; set; }
        public string Str { get; set; }
    }
    
    //OK. returns json. json in headers, json in body (serialized SomeModel)
    [HttpGet]
    public ResultOrUnprocessable<SomeModel, SomeErrors> GetSomeModelCustom()
    {
        return new SomeModel(){Id = 5, Str = "xdstr"};
    }
}
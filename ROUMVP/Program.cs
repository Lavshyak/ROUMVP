using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OpenApi.Models;
using ROUMVP.ResultsAndErrors;

var builder = WebApplication.CreateBuilder(args);

//In my case, CWD by default points to the folder with the project source code
Directory.SetCurrentDirectory(AppContext.BaseDirectory);

var services = builder.Services;
var environment = builder.Environment;

services.AddControllers(options =>
{
    options.OutputFormatters.RemoveType<StringOutputFormatter>();
    options.OutputFormatters.Insert(0, new ResultOrUnprocessableOutputFormatter());

});

if (environment.IsDevelopment())
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API V1", Version = "v1" });

        var basePath = Directory.GetCurrentDirectory();

        options.IncludeXmlComments(Path.Combine(basePath, "ROUMVP.xml"), true);
        
        options.OperationFilter<ResultOrUnprocessableOperationFilter>();
    });
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
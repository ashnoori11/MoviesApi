using Application;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.ApiBehavior;
using MoviesApi.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOutputCache(opt =>
{
    opt.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(30);
    opt.AddPolicy("Paginating", builder => builder.Expire(TimeSpan.FromSeconds(10)));
    opt.AddPolicy("SingleRow", builder => builder.Expire(TimeSpan.FromSeconds(5)));
    //opt.AddPolicy("Reports", builder => builder.Expire(TimeSpan.FromSeconds(60)));
});
builder.Services.AddControllers(options =>
    options.Filters.Add(new ParseBadRequest())
    );

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
    BadRequestBehavior.Parse(options);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddCors(options =>
{
    var frontEndUrl = builder.Configuration.GetValue<string>("frontend_url");
    options.AddDefaultPolicy(corsBuilder =>
    {
        corsBuilder
        .WithOrigins(frontEndUrl)
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .WithMethods("PUT", "DELETE", "GET","POST")
        .WithExposedHeaders(new string[] {"totalAmountOfRecords"});
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();
app.UseOutputCache();
app.MapControllers();

app.Run();


public partial class Program { }
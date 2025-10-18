using Application.Common;
using Asp.Versioning;
using Domain.Entities.Mongo;
using FluentValidation;
using Infrastructure.Softiator;
using pizzapantry_backend.Application;

using pizzapantry_backend.Infrastructure;

using pizzapantry_backend.WebApi;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
builder.Services.ConfigureServices();



// Adding the configuration services
builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplication();

builder.Services.Configure<MongoDatabaseSettings>(builder.Configuration.GetSection(nameof(MongoDatabaseSettings)));
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
})
.AddMvc() // This is needed for controllers
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});


builder.Services.AddScoped<ISoftiator, Softiator>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(AppDomain.CurrentDomain.Load("pizzapantry_backend.Application"));

var assemblies = AppDomain.CurrentDomain.GetAssemblies();

foreach (var assembly in assemblies)
{
    var handlerTypes = assembly
        .GetTypes()
        .Where(type => !type.IsAbstract && !type.IsInterface)
        .SelectMany(type =>
            type.GetInterfaces()
                .Where(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
                )
                .Select(i => new { Interface = i, Implementation = type })
        );

    foreach (var handler in handlerTypes)
    {
        builder.Services.AddTransient(handler.Interface, handler.Implementation);
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Running migrations
    using var scope = app.Services.CreateScope();

}
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
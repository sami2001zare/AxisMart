using AxisMart.Application.Ecommerce;
using AxisMart.Infra.Ecommerce;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog((context, configuration) =>
//{
//    configuration.ReadFrom.Configuration(context.Configuration);
//});

builder.Services.AddControllers()
    .AddJsonOptions(option =>
    {
        option.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    })
    .AddNewtonsoftJson();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplicationService(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", cors =>
    {
        cors
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


var app = builder.Build();

app.ApplyMigration();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UserRequestContextLogging();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

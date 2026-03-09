using Template.Application.ApplicationModule;
using Template.Infrastructure.InfrastructureModule;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration); // обов'язково


builder.Services.AddControllers();
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Template API V1");
        c.RoutePrefix = ""; // робить UI доступним на корені http://localhost:5277/
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
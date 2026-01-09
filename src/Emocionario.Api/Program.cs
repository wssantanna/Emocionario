using Emocionario.Api.Endpoints;
using Emocionario.Api.Middleware;
using Emocionario.Application.Usuarios.Services;
using Emocionario.Infrastructure;
using FluentValidation;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Emocionario API")
            .WithTheme(ScalarTheme.Purple)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.UseCors();

app.MapUsuariosEndpoints();

app.MapGet("/status", () => Results.Ok(new
{
    Status = "Ok",
    Fuso = DateTime.UtcNow,
    Aplicacao = "API RESTful Emocionario",
    Versao = "1.0.0"
}))
.WithName("VerificarStatusApi")
.WithTags("Status")
.WithSummary("Verifica o status da API")
.Produces(StatusCodes.Status200OK);

app.Run();

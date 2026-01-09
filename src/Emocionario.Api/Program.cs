using Emocionario.Api.Endpoints;
using Emocionario.Api.Middleware;
using Emocionario.Application.Usuarios.Services;
using Emocionario.Infrastructure;
using FluentValidation;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao container
builder.Services.AddOpenApi();

// Configurar infraestrutura (DbContext, Repositories)
builder.Services.AddInfrastructure();

// Registrar serviços da camada Application
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Registrar validadores do FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Configurar CORS (permitir qualquer origem em desenvolvimento)
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

// Configurar o pipeline de requisições HTTP

// Middleware de tratamento de exceções global
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configurar OpenAPI/Swagger apenas em desenvolvimento
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

// HTTPS Redirection
app.UseHttpsRedirection();

// CORS
app.UseCors();

// Mapear endpoints de usuários
app.MapUsuariosEndpoints();

// Endpoint de health check
app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy",
    Timestamp = DateTime.UtcNow,
    Application = "Emocionario API",
    Version = "1.0.0"
}))
.WithName("HealthCheck")
.WithTags("Health")
.WithSummary("Verifica o status de saúde da API")
.Produces(StatusCodes.Status200OK);

app.Run();

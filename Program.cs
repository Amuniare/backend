using UserManagementAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware pipeline (order matters!)
// 1. Error handling first - catches all exceptions
app.UseMiddleware<ErrorHandlingMiddleware>();

// 2. Authentication second - validates tokens
app.UseMiddleware<AuthenticationMiddleware>();

// 3. Logging last - logs all requests/responses
app.UseMiddleware<LoggingMiddleware>();

// Enable Swagger for testing
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// Root endpoint
app.MapGet("/", () => new
{
    message = "User Management API",
    version = "1.0.0",
    endpoints = new
    {
        users = "/api/user",
        documentation = "/swagger"
    },
    authentication = "Use header: Authorization: Bearer techhive-api-key-2024"
});

app.Run();

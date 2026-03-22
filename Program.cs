using Microsoft.EntityFrameworkCore;
using Smart_Desk_AI.Agents;
using Smart_Desk_AI.Data;
using Smart_Desk_AI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS — allow React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Supabase PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration
        .GetConnectionString("DefaultConnection"))
    .UseSnakeCaseNamingConvention());

// Register services
builder.Services.AddScoped<OpenAIService>();
builder.Services.AddScoped<DocumentService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<AgentFactory>();

// Register agents
builder.Services.AddScoped<HRAgent>();
builder.Services.AddScoped<FinanceAgent>();
builder.Services.AddScoped<ITAgent>();
builder.Services.AddScoped<OrchestratorAgent>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// CORS must be before everything else!
app.UseCors("AllowReact");

app.UseAuthorization();
app.MapControllers();

app.Run();
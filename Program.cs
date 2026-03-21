using Microsoft.EntityFrameworkCore;
using Smart_Desk_AI.Agents;
using Smart_Desk_AI.Data;
using Smart_Desk_AI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Supabase PostgreSQL database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration
        .GetConnectionString("DefaultConnection")).UseSnakeCaseNamingConvention());

// Register services
builder.Services.AddScoped<OpenAIService>();
builder.Services.AddScoped<DocumentService>();

// Register agents
builder.Services.AddScoped<HRAgent>();
builder.Services.AddScoped<FinanceAgent>();
builder.Services.AddScoped<ITAgent>();
builder.Services.AddScoped<OrchestratorAgent>();
builder.Services.AddScoped<ReviewService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
using Microsoft.EntityFrameworkCore;
using PD.Services;
using PD.Data;
using PD.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3002")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();

    });
});
builder.Services.AddScoped<DialogflowService>(provider =>
{
    string projectId = builder.Configuration["Dialogflow:ProjectId"];
    string serviceAccountKeyPath = builder.Configuration["Dialogflow:ServiceAccountKeyPath"];
    return new DialogflowService(projectId, serviceAccountKeyPath);
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IOtpService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var smtpSettings = configuration.GetSection("EmailSettings");

  
    string smtpHost = smtpSettings["SmtpServer"];
    string smtpUsername = smtpSettings["FromEmail"];
    string smtpPassword = smtpSettings["Password"];
    string smtpPortString = smtpSettings["SmtpPort"];

    if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
    {
        throw new ApplicationException("Invalid SMTP configuration: Missing required fields.");
    }

    if (!int.TryParse(smtpPortString, out var smtpPort))
    {
        throw new ApplicationException("Invalid SMTP configuration: SmtpPort must be a valid integer.");
    }

    return new OtpService(smtpHost, smtpPort, smtpUsername, smtpPassword);
});

builder.Services.AddScoped<IFormDataRepository, FormDataRepository>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<ReviewService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


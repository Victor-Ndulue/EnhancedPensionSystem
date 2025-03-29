using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_WebAPP.Extensions;
using EnhancedPensionSystem_WebAPP.LoggerConfig;
using Hangfire;

LogConfigurator.ConfigureLogger();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.RegisterFluentValidation();
builder.Services.ConfigureUserIdentityManager();
builder.Services.RegisterDbContext(builder.Configuration);
builder.Services.RegisterUnitOfWork();
builder.Services.ConfigureHangfire();
builder.Services.RegisterGenericRepo();
builder.Services.ConfigureController();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapHangfireDashboard("/background-jobs", HangfireAuthorizationFilter.GetHangfireDashboardOptions(builder.Configuration));

app.MapControllers();
RecurringJob.AddOrUpdate<IBackgroundJobService>(x => x.ValidateContributionsAsync(), Cron.Daily);
RecurringJob.AddOrUpdate<IBackgroundJobService>(x => x.CheckBenefitEligibilityAsync(), Cron.Weekly);
RecurringJob.AddOrUpdate<IBackgroundJobService>(x => x.CalculateInterestAsync(), Cron.Monthly);
RecurringJob.AddOrUpdate<IBackgroundJobService>(x => x.SendNotificationsAsync(), Cron.MinuteInterval(30));

app.Run();

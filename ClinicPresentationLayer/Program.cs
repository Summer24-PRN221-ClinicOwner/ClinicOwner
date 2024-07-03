using BusinessObjects.Entities;
using ClinicRepositories;
using ClinicRepositories.Interfaces;
using ClinicServices;
using ClinicServices.EmailService;
using ClinicServices.Interfaces;
using ClinicServices.QuartzService;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quartz;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IRoomAvailabilityRepository, RoomAvailabilityRepository>();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddScoped<IClinicOwnerService, ClinicOwnerService>();
builder.Services.AddScoped<IClinicOwnerRepository, ClinicOwnerRepository>();

builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IClinicRepository, ClinicRepository>();

builder.Services.AddScoped<IDentistAvailabilityService, DentistAvailabilityService>();
builder.Services.AddScoped<IDentistAvailabilityRepository, DentistAvailabilityRepository>();

builder.Services.AddScoped<IDentistService, DentistService>();
builder.Services.AddScoped<IDentistRepository, DentistRepository>();

builder.Services.AddScoped<ILicenseService, LicenseService>();
builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<User>();
builder.Services.AddScoped<IJobExecutionLogService, JobExecutionLogService>();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

//begin of set up Quartz background task scheduler
builder.Services.AddQuartz(q =>
{
    // Configure job with cron schedule
    q.AddJob<AppointmentNotificationJob>(new JobKey("AppointmentNotificationJob"), j => j
    .WithDescription("Send appointment notifications daily."));

    q.AddTrigger(t => t
        .ForJob(new JobKey("AppointmentNotificationJob"))
        .WithIdentity("AppointmentNotificationJobTrigger")
        .WithCronSchedule("0 0 7 * * ?")); // run every day at 7 AM 
});

builder.Services.AddTransient<AppointmentNotificationJob>(); // Register the job as transient
builder.Services.AddQuartzHostedService(options =>
{
    // wait for the job to complete
    options.WaitForJobsToComplete = true;
});
//end of set up Quartz background task scheduler

//set default page to MainPage
builder.Services.Configure<RazorPagesOptions>(options =>
{
    options.Conventions.AddPageRoute("/MainPage", "");
});
IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
var emailConfig = configuration.GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
var app = builder.Build();

// run the job ìf the job has not been run for the day
using (var scope = app.Services.CreateScope())
{
    var jobExecutionLogService = scope.ServiceProvider.GetRequiredService<IJobExecutionLogService>();
    var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
    var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppointmentNotificationJob>>();

    var lastRun = await jobExecutionLogService.GetLastExecutionTimeAsync("AppointmentNotificationJob");
    var timeCheck = DateTime.UtcNow.AddHours(7).Date;
    if (lastRun == null || lastRun.Value.Date < timeCheck)
    {
        var job = new AppointmentNotificationJob(appointmentService, emailSender, logger, jobExecutionLogService);
        await job.Execute(null);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();

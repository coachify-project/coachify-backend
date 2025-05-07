using Coachify.BLL.Interfaces;
using Coachify.BLL.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Coachify.DAL;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") 
                      ?? "Data Source=coachify.db")); // Строка подключения из конфигурации или по умолчанию

// 2. Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// 3. Register Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICoachService, CoachService>();
builder.Services.AddScoped<ICoachApplicationService, CoachApplicationService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseStatusService, CourseStatusService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IFeedbackStatusService, FeedbackStatusService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ILessonStatusService, LessonStatusService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IModuleStatusService, ModuleStatusService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentStatusService, PaymentStatusService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAnswerOptionService, AnswerOptionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<ITestSubmissionService, TestSubmissionService>();
builder.Services.AddScoped<ITestSubmissionAnswerService, TestSubmissionAnswerService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<IUserCoachApplicationStatusService, UserCoachApplicationStatusService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// 4. Add Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Coachify API", Version = "v1" });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();  
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Coachify API V1"));
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

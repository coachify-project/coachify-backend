using System.Text;
using AutoMapper;
using Coachify.BLL.Interfaces;
using Coachify.BLL.Services;
using Coachify.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ==== 1. Configure Database Context ====
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
      .UseSqlite(
          builder.Configuration.GetConnectionString("DefaultConnection")
          ?? "Data Source=coachify.db"
      )
      // Игнорируем ошибку о pending model changes
      .ConfigureWarnings(w =>
          w.Ignore(RelationalEventId.PendingModelChangesWarning)
      )
);

// ==== 2. Configure AutoMapper ====
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ==== 3. Load JWT settings from configuration ====
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey     = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key is missing");
var issuer     = jwtSection["Issuer"];
var audience   = jwtSection["Audience"];
var expireMin  = int.Parse(jwtSection["ExpireMinutes"] ?? "60");
var keyBytes   = Encoding.UTF8.GetBytes(jwtKey);

// ==== 4. Configure Authentication / JWT Bearer ====
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata      = false;
        options.SaveToken                 = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidIssuer              = issuer,
            ValidateAudience         = true,
            ValidAudience            = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey         = new SymmetricSecurityKey(keyBytes),
            ValidateLifetime         = true,
            ClockSkew                = TimeSpan.Zero
        };
    });

// ==== 5. Register BLL services ====
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICoachService, CoachService>();
builder.Services.AddScoped<ICoachApplicationService, CoachApplicationService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseStatusService, CourseStatusService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IEnrollmentStatusService, EnrollmentStatusService>();
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

// ==== 6. Add Controllers, CORS & Swagger ====

// Добавляем контроллеры
builder.Services.AddControllers();

// Добавляем CORS с разрешением для всех источников
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Coachify API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In          = ParameterLocation.Header,
        Description = "Введите `Bearer {token}`",
        Name        = "Authorization",
        Type        = SecuritySchemeType.ApiKey,
        Scheme      = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ==== 7. Apply pending migrations on startup ====
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// ==== 8. Configure middleware pipeline ====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Coachify API V1"));
}

app.UseRouting();

// Включаем CORS (до аутентификации и авторизации)
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

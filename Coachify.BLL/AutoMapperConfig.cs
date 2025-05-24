using Coachify.BLL.Mappings;
using Microsoft.Extensions.DependencyInjection;


namespace Coachify.BLL;

public static class AutoMapperConfig
{
    public static void AddAutoMapperConfig(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(AnswerOptionProfile).Assembly,
            typeof(CategoryProfile).Assembly,
            typeof(CertificateProfile).Assembly,
            typeof(CoachApplicationProfile).Assembly,
            typeof(CoachProfile).Assembly,
            typeof(CourseProfile).Assembly,
            typeof(CourseStatusProfile).Assembly,
            typeof(EnrollmentProfile).Assembly,
            typeof(EnrollmentStatusProfile).Assembly,
            typeof(FeedbackProfile).Assembly,
            typeof(FeedbackStatusProfile).Assembly,
            typeof(LessonProfile).Assembly,
            typeof(ModuleProfile).Assembly,
            typeof(PaymentProfile).Assembly,
            typeof(PaymentStatusProfile).Assembly,
            typeof(QuestionProfile).Assembly,
            typeof(RoleProfile).Assembly,
            typeof(SkillProfile).Assembly,
            typeof(TestProfile).Assembly,
            typeof(TestSubmissionProfile).Assembly,
            typeof(TestSubmissionAnswerProfile).Assembly,
            typeof(UserProfile).Assembly
        );
    }
}
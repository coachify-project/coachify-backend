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
            typeof(FeedbackProfile).Assembly,
            typeof(FeedbackStatusProfile).Assembly,
            typeof(LessonProfile).Assembly,
            typeof(LessonStatusProfile).Assembly,
            typeof(ModuleProfile).Assembly,
            typeof(ModuleStatusProfile).Assembly,
            typeof(PaymentProfile).Assembly,
            typeof(PaymentStatusProfile).Assembly,
            typeof(QuestionProfile).Assembly,
            typeof(RoleProfile).Assembly,
            typeof(TestProfile).Assembly,
            typeof(TestSubmissionAnswerProfile).Assembly,
            typeof(TestSubmissionProfile).Assembly,
            typeof(UserCoachApplicationStatusProfile).Assembly,
            typeof(UserProfile).Assembly
        );
    }
}
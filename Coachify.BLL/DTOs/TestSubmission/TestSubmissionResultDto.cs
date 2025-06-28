namespace Coachify.BLL.DTOs.TestSubmission;

public class TestSubmissionResultDto
{
    public int Score { get; set; }
    public int scorePercentage { get; set; } // Для фронтенда
    public int CorrectAnswers { get; set; }
    public int correctAnswers { get; set; } // Для фронтенда
    public int TotalQuestions { get; set; }
    public int totalQuestions { get; set; } // Для фронтенда
    public int passingScore { get; set; } // Проходной балл для фронтенда
    public bool IsPassed { get; set; }
    public bool passed { get; set; } // Для фронтенда
    public DateTime SubmittedAt { get; set; }
}
namespace TriviaQuest.api.Dtos;
// we make dtos for only those req who belongs to post

public record AnswerDto(int QuestionId, int ChoiceId);
public record ScoreRequest(List<AnswerDto> Answers);

public class QuizResultDto
{
    public int QuestionId { get; set; }
    public int SelectedChoiceId { get; set; }
    public bool IsCorrect { get; set; }

    public int? CorrectChoiceId { get; set; }
    public string? CorrectChoiceText { get; set; }
    public string? Explanation { get; set; }
}

public class QuizScoreResponseDto
{
    public int Total { get; set; }
    public int Correct { get; set; }
    public int Percent { get; set; }

    public List<QuizResultDto> Results { get; set; } = new();
}

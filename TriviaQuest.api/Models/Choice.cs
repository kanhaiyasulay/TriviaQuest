namespace TriviaQuest.api.Models;

public class Choice
{
    public int Id { get; set; }
    public string Text { get; set; } = default!;
    public bool IsCorrect { get; set; }   // never expose to client directly
    public int QuestionId { get; set; }
    public Question? Question { get; set; }
}

namespace TriviaQuest.api.Models;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = default!;
    public string? Explanation { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<Choice> Choices { get; set; } = new List<Choice>();
}

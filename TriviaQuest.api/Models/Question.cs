namespace TriviaQuest.api.Models;

public enum Difficulty
{
    Easy = 1,
    Medium = 2,
    Hard = 3
}


public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = default!;

    public Difficulty Difficulty { get; set; }

    public string? Explanation { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<Choice> Choices { get; set; } = new List<Choice>();
}

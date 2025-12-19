namespace TriviaQuest.api.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}

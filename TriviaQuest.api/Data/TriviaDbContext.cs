using Microsoft.EntityFrameworkCore;
using TriviaQuest.api.Models;

namespace TriviaQuest.api.Data;

public class TriviaDbContext : DbContext
{
    public TriviaDbContext(DbContextOptions<TriviaDbContext> options) : base(options) {}

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Choice> Choices => Set<Choice>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // Simple seeding
        b.Entity<Category>().HasData(
            new Category { Id = 1, Name = "General Knowledge" },
            new Category { Id = 2, Name = "Movies" },
            new Category { Id = 3, Name = "Games" }
        );

        b.Entity<Question>().HasData(
            new Question { Id = 1, Text = "What is the capital of France?", Explanation = "Paris is capital of France", CategoryId = 1 },
            new Question { Id = 2, Text = "Who directed 'Inception'?", Explanation = "Christopher Nolan was director", CategoryId = 2 },
            new Question { Id = 3, Text = "Which company makes the PlayStation?", Explanation = "Sony makes since 1994", CategoryId = 3 }
        );

        b.Entity<Choice>().HasData(
            // Q1
            new Choice { Id = 1, Text = "Paris", IsCorrect = true,  QuestionId = 1 },
            new Choice { Id = 2, Text = "Berlin", IsCorrect = false, QuestionId = 1 },
            new Choice { Id = 3, Text = "Madrid", IsCorrect = false, QuestionId = 1 },
            new Choice { Id = 4, Text = "Rome",  IsCorrect = false, QuestionId = 1 },

            // Q2
            new Choice { Id = 5,  Text = "Christopher Nolan", IsCorrect = true,  QuestionId = 2 },
            new Choice { Id = 6,  Text = "Steven Spielberg",  IsCorrect = false, QuestionId = 2 },
            new Choice { Id = 7,  Text = "James Cameron",     IsCorrect = false, QuestionId = 2 },
            new Choice { Id = 8,  Text = "Ridley Scott",      IsCorrect = false, QuestionId = 2 },

            // Q3
            new Choice { Id = 9,  Text = "Sony",        IsCorrect = true,  QuestionId = 3 },
            new Choice { Id = 10, Text = "Nintendo",    IsCorrect = false, QuestionId = 3 },
            new Choice { Id = 11, Text = "Microsoft",   IsCorrect = false, QuestionId = 3 },
            new Choice { Id = 12, Text = "Sega",        IsCorrect = false, QuestionId = 3 }
        );
    }
}

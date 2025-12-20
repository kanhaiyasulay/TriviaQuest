using Microsoft.EntityFrameworkCore;
using TriviaQuest.api.Data;
using TriviaQuest.api.Dtos;

namespace TriviaQuest.api.Endpoints;

public static class TriviaEndpoints
{
    public static IEndpointRouteBuilder MapTriviaEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");
        // 1) Categories
        group.MapGet("/categories", async (TriviaDbContext db) =>
        {
            var cats = await db.Categories
                .OrderBy(c => c.Name)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            return Results.Ok(cats);
        });

        // 2) Random questions
        group.MapGet("/questions", async (
            int? categoryId,
            int? limit,
            TriviaDbContext db) =>
        {
            var q = db.Questions.AsQueryable();

            if (categoryId is not null)
                q = q.Where(x => x.CategoryId == categoryId);

            q = q.OrderBy(x => EF.Functions.Random());

            var take = Math.Clamp(limit ?? 5, 1, 20);

            var items = await q
                .Take(take)
                .Select(x => new
                {
                    x.Id,
                    x.Text,
                    x.CategoryId,
                    Choices = x.Choices
                        .OrderBy(_ => EF.Functions.Random())
                        .Select(c => new { c.Id, c.Text })
                })
                .ToListAsync();

            return Results.Ok(items);
        });

        // 3) Score endpoint
        app.MapPost("/api/quiz/score", async (ScoreRequest req, TriviaDbContext db) =>
        {
            if (req.Answers is null || req.Answers.Count == 0)
                return Results.BadRequest("No answers provided.");

            var qIds = req.Answers.Select(a => a.QuestionId).Distinct().ToList();

            var choices = await db.Choices
                .Where(c => qIds.Contains(c.QuestionId))
                .Select(c => new
                {
                    c.Id,
                    c.QuestionId,
                    c.Text,
                    c.IsCorrect
                })
                .ToListAsync();

            var results = new List<object>();
            var correctCount = 0;

            foreach (var a in req.Answers)
            {
                var selected = choices.FirstOrDefault(c =>
                    c.QuestionId == a.QuestionId &&
                    c.Id == a.ChoiceId);

                var correctChoice = choices.FirstOrDefault(c =>
                    c.QuestionId == a.QuestionId &&
                    c.IsCorrect);

                var isCorrect = selected != null && selected.IsCorrect;

                if (isCorrect)
                    correctCount++;

                results.Add(new
                {
                    questionId = a.QuestionId,
                    selectedChoiceId = a.ChoiceId,
                    isCorrect,
                    correctChoiceId = isCorrect ? null : correctChoice?.Id,
                    correctChoiceText = isCorrect ? null : correctChoice?.Text
                });
            }

            return Results.Ok(new
            {
                total = req.Answers.Count,
                correct = correctCount,
                percent = (int)Math.Round(100.0 * correctCount / req.Answers.Count),
                results
            });
        });


        return group;
    }
}

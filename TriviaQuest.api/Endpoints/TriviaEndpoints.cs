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
        group.MapPost("/quiz/score", async (ScoreRequest req, TriviaDbContext db) =>
        {
            if (req.Answers is null || req.Answers.Count == 0)
                return Results.BadRequest("No answers provided.");

            var qIds = req.Answers
                .Select(a => a.QuestionId)
                .Distinct()
                .ToList();

            var choices = await db.Choices
                .Where(c => qIds.Contains(c.QuestionId))
                .Select(c => new { c.Id, c.QuestionId, c.IsCorrect })
                .ToListAsync();

            var correct = 0;

            foreach (var a in req.Answers)
            {
                var match = choices.FirstOrDefault(c =>
                    c.QuestionId == a.QuestionId &&
                    c.Id == a.ChoiceId);

                if (match is not null && match.IsCorrect)
                    correct++;
            }

            return Results.Ok(new
            {
                total = req.Answers.Count,
                correct,
                percent = (int)Math.Round(100.0 * correct / req.Answers.Count)
            });
        });

        return group;
    }
}

namespace TriviaQuest.api.Dtos;
// we make dtos for only those req who belongs to post

public record AnswerDto(int QuestionId, int ChoiceId);
public record ScoreRequest(List<AnswerDto> Answers);
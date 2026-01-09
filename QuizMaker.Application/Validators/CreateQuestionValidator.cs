using FluentValidation;
using QuizMaker.Application.Dto.Requests;

namespace QuizMaker.Application.Validators;

public class CreateQuestionValidator : AbstractValidator<CreateQuestionRequest>
{
    public CreateQuestionValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty()
            .WithMessage("Question text is required.");

        RuleFor(x => x.AnswerText)
            .NotEmpty()
            .WithMessage("Answer text is required.");
    }
}

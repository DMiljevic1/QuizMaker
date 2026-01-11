using FluentValidation;
using QuizMaker.Application.Dto.Requests;

namespace QuizMaker.Application.Validators;

public class CreateQuestionValidator : AbstractValidator<CreateQuestionRequest>
{
    public CreateQuestionValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty()
            .WithMessage("Question text is required.")
            .MaximumLength(1000)
            .WithMessage("Question text must not exceed 1000 characters.");

        RuleFor(x => x.AnswerText)
            .NotEmpty()
            .WithMessage("Answer text is required.")
            .MaximumLength(500)
            .WithMessage("Answer text must not exceed 500 characters.");
    }
}

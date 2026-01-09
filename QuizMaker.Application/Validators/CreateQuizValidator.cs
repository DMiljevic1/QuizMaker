using FluentValidation;
using QuizMaker.Application.Dto.Requests;

namespace QuizMaker.Application.Validators;

public class CreateQuizValidator : AbstractValidator<CreateQuizRequest>
{
    public CreateQuizValidator()
    {
        RuleFor(x => x.QuizName)
            .NotEmpty().WithMessage("Quiz name is required.")
            .Length(3, 100).WithMessage("Quiz name must contain between 3 and 100 characters.");

        RuleFor(x => x.NewQuestions)
            .ForEach(x => x.SetValidator(new CreateQuestionValidator()));
    }
}

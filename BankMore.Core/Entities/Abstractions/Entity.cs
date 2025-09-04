using FluentValidation;
using FluentValidation.Results;

namespace BankMore.Core.Entities.Abstractions
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public bool IsValid => ValidationResult.IsValid;
        public bool IsInvalid => !IsValid;

        protected ValidationResult ValidationResult { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
            ValidationResult = new ValidationResult();
        }

        public abstract bool Validate();
    }
}

using FluentValidation;
using HomeApi.Contracts.Models.Devices;

namespace HomeApi.Contracts.Validation
{
    public class DeleteDeviceRequestValidator : AbstractValidator<DeleteDeviceRequest>
    {
        /// <summary>
        /// Метод, конструктор, устанавливающий правила
        /// </summary>
        public DeleteDeviceRequestValidator()
        {
            /* Зададим правила валидации */
            RuleFor(x => x.Name).NotEmpty(); // Проверим на null и на пустое свойство
            RuleFor(x => x.SerialNumber).NotEmpty();
            RuleFor(x => x.Room).NotEmpty().Must(BeSupported).WithMessage($"Please choose one of the following locations: {string.Join(", ", Values.ValidRooms)}");
        }

        /// <summary>
        ///  Метод кастомной валидации для свойства location
        /// </summary>
        private bool BeSupported(string location)
        {
            // Проверим, содержится ли значение в списке допустимых
            return Values.ValidRooms.Any(e => e == location);
        }
    }
}

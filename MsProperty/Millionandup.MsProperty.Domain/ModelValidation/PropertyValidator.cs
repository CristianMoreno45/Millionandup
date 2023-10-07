using FluentValidation;
using Millionandup.Framework.Domain.ModelValidation.Enums;
using Millionandup.MsProperty.Domain.AggregatesModel;
using Millionandup.MsProperty.Domain.Interfaces;

namespace Millionandup.MsProperty.Domain.ModelValidation
{

    /// <summary>
    /// Class in charge of validations for Property Business Model
    /// </summary>
    public class PropertyValidator : AbstractValidator<Property>
    {
        /// <summary>
        /// Interface that allows you to manage the Property model
        /// </summary>
        private readonly IPropertyModel _repository;


        /// <summary>
        /// Regular expression to accept letters (with accents), numbers and spaces, 
        /// more info in: <see cref="https://regexper.com/#%5E%5Ba-zA-Z0-9%20%C3%A1%C3%A9%C3%AD%C3%B3%C3%BA%C3%81%C3%89%C3%8D%C3%93%C3%9A%C3%B1%C3%91%5D%2B%24"/>
        /// </summary>
        private readonly string REG_LETTERS_NUMBERS_AND_SPACE = @"^[a-zA-Z0-9 áéíóúÁÉÍÓÚñÑ]+$";

        /// <summary>
        /// Regular expression to accept numbers, letters, spaces, comma, period, hashtag and hyphen,
        /// more info in: <see href="https://regexper.com/#%5E%5B%23.0-9a-zA-Z%5Cs%2C-%5D%2B%24"/>
        /// </summary>
        private readonly string REG_ADDRESS = @"^[#.0-9a-zA-Z\s,-]+$";

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="repository">Interface that allows you to manage the Property model</param>
        public PropertyValidator(IPropertyModel repository)
        {
            _repository = repository;

            // Rules applicable to the creation of the entity
            RuleSet(ValidationType.CREATE.ToString(), () =>
            {

                BasicRules();
                RuleFor(entity => entity.Name).Must((entity, name) => IsNameValid(entity.Name)).WithMessage(MessagesError.NAME_ALREADY_EXIST);
                RuleFor(entity => entity.CodeInternal).Must((entity, code) => IsCodeValid(entity.CodeInternal)).WithMessage(MessagesError.CODE_ALREADY_EXIST);
            });

            // Rules applicable to the updating of the entity
            RuleSet(ValidationType.UPDATE.ToString(), () =>
            {
                BasicRules();
                RuleFor(entity => entity.Name).Must((entity, name) => IsNameValid(entity.Name, entity.PropertyId, entity.CodeInternal)).WithMessage(MessagesError.NAME_ALREADY_EXIST);

            });

            // Rules applicable to the deleting of the entity
            RuleSet(ValidationType.DELETE.ToString(), () =>
            {
                // TODO: The validations would go here but it is not applied since they are not part of the test.
            });

            // Rules applicable to the updating the value of the property
            RuleSet(PropertyValidationType.UPDATE_VALUE.ToString(), () =>
            {
                RuleFor(p => p.Price).GreaterThan(0).WithMessage(MessagesError.PRICE_GREATER_THAN_ZERO);
                RuleFor(p => p.Price).LessThan(1000000000000).WithMessage(string.Format(MessagesError.MAX_LENGHT_MONEY, "Price", 1000000000000));
            });
        }

        /// <summary>
        /// Basic rules container
        /// </summary>
        private void BasicRules()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(p => p.Name).Must(name => IsNullOrEmpty(name)).WithMessage(MessagesError.NAME_IS_MANDATORY);
            RuleFor(p => p.Name).Matches(REG_LETTERS_NUMBERS_AND_SPACE).WithMessage(MessagesError.NAME_INCORRECT_FORMAT);
            RuleFor(p => p.Name.Length).LessThan(80).WithMessage(string.Format(MessagesError.MAX_LENGHT_STRING, "Name",80));
            RuleFor(p => p.Price).GreaterThan(0).WithMessage(MessagesError.PRICE_GREATER_THAN_ZERO);
            RuleFor(p => p.Price).LessThan(1000000000000).WithMessage(string.Format(MessagesError.MAX_LENGHT_MONEY, "Price", 1000000000000));
            RuleFor(p => p.Address).Must(address => IsNullOrEmpty(address)).WithMessage(MessagesError.ADDRESS_IS_MANDATORY);
            RuleFor(p => p.Address).Matches(REG_ADDRESS).WithMessage(MessagesError.ADDRESS_INCORRECT_FORMAT);
            RuleFor(p => p.Address.Length).LessThan(125).WithMessage(string.Format(MessagesError.MAX_LENGHT_STRING, "Address", 125));
            RuleFor(p => p.CodeInternal).Must(codeInternal => IsNullOrEmpty(codeInternal)).WithMessage(MessagesError.CODE_IS_MANDATORY);
            RuleFor(p => p.CodeInternal.Length).LessThan(50).WithMessage(string.Format(MessagesError.MAX_LENGHT_STRING, "CodeInternal", 50));
        }

        /// <summary>
        /// Method responsible for validating if the value is empty or null (validation of included spaces)
        /// </summary>
        /// <param name="value">Some Property</param>
        /// <returns>boolean indicating whether the property has value</returns>
        private static bool IsNullOrEmpty(string value) => !string.IsNullOrEmpty(value.Trim());

        /// <summary>
        /// Method responsible for validating if the name of a property that is already in the database
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="propertyId">Property ID</param>
        /// <returns>boolean indicating whether the property already exists with that name</returns>
        private bool IsNameValid(string name, Guid propertyId, string codeInternal)
        {
            return !_repository.GetByFilter(x => x.Name.ToUpper() == name.ToUpper()
                && (
                    !x.PropertyId.Equals(propertyId)
                    || !x.CodeInternal.Equals(codeInternal)
                )).Any();
        }

        /// <summary>
        /// Method responsible for validating if the name of a property that is already in the database
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>boolean indicating whether the property already exists with that name</returns>
        private bool IsNameValid(string name) => !_repository.GetByFilter(x => x.Name.ToUpper() == name.ToUpper()).Any();

        /// <summary>
        /// Method responsible for validating if the code internal of a property that is already in the database
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>boolean indicating whether the property already exists with that name</returns>
        private bool IsCodeValid(string code) => !_repository.GetByFilter(x => x.CodeInternal.ToUpper() == code.ToUpper()).Any();

        /// <summary>
        /// Enumerator, allows you to customize other types of validation that escape the basics (Creation, update and deletion)
        /// </summary>
        public enum PropertyValidationType
        {
            UPDATE_VALUE
        }

        /// <summary>
        /// Express Error Messages of this domain
        /// </summary>
        public static class MessagesError
        {
            public const string NAME_ALREADY_EXIST = "There is already a Property created with that name.";
            public const string NAME_IS_MANDATORY = "The 'Name' field is required.";
            public const string NAME_INCORRECT_FORMAT = "The 'Name' field is incorrectly formatted, only letters (with accents), numbers and spaces are accepted.";
            public const string PRICE_GREATER_THAN_ZERO = "The price of the property must be greater than zero.";
            public const string ADDRESS_IS_MANDATORY = "The 'Address' field is required.";
            public const string ADDRESS_INCORRECT_FORMAT = "The 'Address' field has an incorrect format, only letters (without accents), numbers, spaces and other characters (#.-,) are accepted.";
            public const string CODE_IS_MANDATORY = "The Property 'Code internal' field is required.";
            public const string CODE_ALREADY_EXIST = "There is already a Property created with that code.";
            public const string OWNERS_IS_MANDATORY = "The 'Owners' field is mandatory.";
            public const string OWNERS_NOT_EXIST = "Check the 'Owners' field, one or more do not exist in the database.";
            public const string PROPERTYID_IS_MANDATORY = "The 'PropertyId' field is required.";
            public const string MAX_LENGHT_STRING = "The '{0}' field must have a maximum of '{1}' characters";
            public const string MAX_LENGHT_MONEY = "The '{0}' field must have a maximum of $'{1}'.";
        }
    }
}

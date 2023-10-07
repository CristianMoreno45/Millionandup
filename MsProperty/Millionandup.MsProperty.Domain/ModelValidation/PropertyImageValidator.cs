using FluentValidation;
using Millionandup.Framework.Domain.ModelValidation.Enums;
using Millionandup.MsProperty.Domain.AggregatesModel;

namespace Millionandup.MsProperty.Domain.ModelValidation
{
    /// <summary>
    /// Class in charge of validations for Property Image Business Model
    /// </summary>
    public class PropertyImageValidator : AbstractValidator<PropertyImage>
    {

        /// <summary>
        /// Regular expression to accept url's, more detail in <see href="https://regexper.com/#%28%28ht%7Cf%29tp%28s%3F%29%29%28%3A%28%28%5C%2F%5C%2F%29%28%3F!%5C%2F%29%29%29%28%28%28w%29%7B3%7D%5C.%29%3F%29%28%5Ba-zA-Z0-9%5C-_%5D%2B%28%5C.%28com%7Cedu%7Cgov%7Cint%7Cmil%7Cnet%7Corg%7Cbiz%7Cinfo%7Cname%7Cpro%7Cmuseum%7Cco%5C.uk%29%29%29%28%5C%2F%28%3F!%5C%2F%29%29%28%28%5Ba-zA-Z0-9%5C-_%5C%2F%5D*%29%3F%29%28%5Ba-zA-Z0-9%5D%29%2B%5C.%28%28jpg%7Cjpeg%7Cgif%7Cpng%29%28%3F!%28%5Cw%7C%5CW%29%29%29"/>
        /// </summary>
        private readonly string REG_URL = @"((ht|f)tp(s?))(:((\/\/)(?!\/)))(((w){3}\.)?)([a-zA-Z0-9\-_]+(\.(com|edu|gov|int|mil|net|org|biz|info|name|pro|museum|co\.uk)))(\/(?!\/))(([a-zA-Z0-9\-_\/]*)?)([a-zA-Z0-9])+\.((jpg|jpeg|gif|png)(?!(\w|\W)))";

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="repository">Interface that allows you to manage the Property model</param>
        public PropertyImageValidator()
        {

            // Rules applicable to the creation of the entity
            RuleSet(ValidationType.CREATE.ToString(), () =>
            {
                BasicRules();
            });

            // Rules applicable to the updating of the entity
            RuleSet(ValidationType.UPDATE.ToString(), () =>
            {
                BasicRules();
            });

            // Rules applicable to the deleting of the entity
            RuleSet(ValidationType.DELETE.ToString(), () =>
            {
                // TODO: The validations would go here but it is not applied since they are not part of the test.
            });
        }

        /// <summary>
        /// Basic rules container
        /// </summary>
        private void BasicRules()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(p => p.File).Must(file => IsNullOrEmpty(file)).WithMessage(MessagesError.FILE_IS_MANDATORY);
            RuleFor(p => p.File).Matches(REG_URL).WithMessage(MessagesError.FILEURL_HAS_INCORRECT_FORMAT);
            RuleFor(p => p.File.Length).LessThan(2048).WithMessage(string.Format(MessagesError.MAX_LENGHT_STRING, "File", 2048));
        }

        /// <summary>
        /// Method responsible for validating if the value is empty or null (validation of included spaces)
        /// </summary>
        /// <param name="value">Some Property</param>
        /// <returns>boolean indicating whether the property has value</returns>
        private static bool IsNullOrEmpty(string value) => !string.IsNullOrEmpty(value.Trim());

        /// <summary>
        /// Express Error Messages of this domain
        /// </summary>
        public static class MessagesError
        {
            public const string FILE_IS_MANDATORY = "The 'Name' field is required.";
            public const string FILEURL_HAS_INCORRECT_FORMAT = "The 'File' field is incorrectly formatted, only URLs are accepted (https://something.com).";
            public const string PROPERTYID_DONT_EXIST = "Check the 'PropertyIdList' field, one or more do not exist in the database.";
            public const string CODEINTERNAL_DONT_EXIST = "Check the 'CodeInternalList' field, one or more do not exist in the database.";
            public const string PROPERTIES_DONT_EXIST = "One or more properties do not exist in the database.";
            public const string PROPERTY_DONT_EXIST = "This Property do not exist in the database.";
            public const string MAX_LENGHT_STRING = "The '{0}' field must have a maximum of '{1}' characters";
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Common.Lib.MVC.Attributes
{

    public class PhoneAttribute : ValidationAttribute
    {
        private Regex Regex { get; set; }

        public string Pattern
        {
            get
            {
                //Description: US Phone Number -- doesn't check to see if first digit is legal (not a 0 or 1).
                //Matches: (123) 456-7890 | 123-456-7890
                //Non-Matches: 1234567890
                //return @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}";

                return @"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$";
                //Description: This RegEx requires a US phone number WITH area code. It is written to all users to enter whatever delimiters they want or 
                //no delimiters at all (i.e. 111-222-3333, or 111.222.3333, or (111) 222-3333, or 1112223333, etc...).
                //Matches: (111) 222-3333 | 1112223333 | 111-222-3333
                //Non-Matches: 11122223333 | 11112223333 | 11122233333
            }
        }

        public PhoneAttribute()
        {
            Regex = new Regex(Pattern);
        }

        public override bool IsValid(object value)
        {
            // convert the value to a string
            var stringValue = Convert.ToString(value, CultureInfo.CurrentCulture);

            // automatically pass if value is null or empty. RequiredAttribute should be used to assert an empty value.
            if (string.IsNullOrWhiteSpace(stringValue)) return true;

            var m = Regex.Match(stringValue);

            // looking for an exact match, not just a search hit.
            return (m.Success && (m.Index == 0) && (m.Length == stringValue.Length));
        }
    }

    public class PhoneValidator : DataAnnotationsModelValidator<PhoneAttribute>
    {
        private readonly string _errorMessage;
        private readonly string _pattern;

        public PhoneValidator(ModelMetadata metadata, ControllerContext context, PhoneAttribute attribute)
            : base(metadata, context, attribute)
        {
            _errorMessage = attribute.ErrorMessage;
            _pattern = attribute.Pattern;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rule = new ModelClientValidationRegexRule(_errorMessage, _pattern);
            return new[] {rule};
        }
    }

    public class EmailAttribute : ValidationAttribute
    {
        private Regex Regex { get; set; }

        public string Pattern
        {
            get
            {
                return @"^(([^<>()[\]\\.,;:\s@\""]+"
                       + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                       + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                       + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                       + @"[a-zA-Z]{2,}))$";
            }
        }

        public EmailAttribute()
        {
            Regex = new Regex(Pattern);
        }

        public override bool IsValid(object value)
        {
            // convert the value to a string
            var stringValue = Convert.ToString(value, CultureInfo.CurrentCulture);

            // automatically pass if value is null or empty. RequiredAttribute should be used to assert an empty value.
            if (string.IsNullOrWhiteSpace(stringValue)) return true;

            var m = Regex.Match(stringValue);

            // looking for an exact match, not just a search hit.
            return (m.Success && (m.Index == 0) && (m.Length == stringValue.Length));
        }
    }

    public class EmailValidator : DataAnnotationsModelValidator<EmailAttribute>
    {
        private readonly string _errorMessage;
        private readonly string _pattern;

        public EmailValidator(ModelMetadata metadata, ControllerContext context, EmailAttribute attribute)
            : base(metadata, context, attribute)
        {
            _errorMessage = attribute.ErrorMessage;
            _pattern = attribute.Pattern;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rule = new ModelClientValidationRegexRule(_errorMessage, _pattern);
            return new[] {rule};
        }
    }
}

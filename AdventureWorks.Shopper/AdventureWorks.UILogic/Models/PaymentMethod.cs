// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AdventureWorks.UILogic.Services;

namespace AdventureWorks.UILogic.Models
{
    // Documentation on validating user input is at http://go.microsoft.com/fwlink/?LinkID=288817&clcid=0x409
    public class PaymentMethod : ValidatableBindableBase
    {
        // Regex rules for the fields.
        // Notice that you might need more complex rules in your app.

        // We allow all Unicode letter characters as well as internal spaces and hypens, as long as these do not occur in sequences.
        private const string NAMES_REGEX_PATTERN = @"\A\p{L}+([\p{Zs}\-][\p{L}]+)*\z";

        // We allow only numbers and hypens, as long as these do not occur in sequences.
        private const string NUMBERS_REGEX_PATTERN = @"\A\d+([\d\-][\d]+)*\z";

        private const string MONTHNUMBERS_REGEX_PATTERN = @"\A(0?[1-9]|1[0-2])\z";

        private const string YEARNUMBERS_REGEX_PATTERN = @"\A([2-9]\d\d\d)\z";

        private string _id;
        private string _cardNumber;
        private string _cardholderName;
        private string _expirationMonth;
        private string _expirationYear;
        private string _phone;
        private string _cardVerificationCode;

        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        ////[Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        ////[StringLength(20, MinimumLength = 4, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "CardNumberInvalidLengthErrorMessage")]
        public string CardNumber
        {
            get { return _cardNumber; }
            set { SetProperty(ref _cardNumber, value); }
        }

        ////[Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        ////[RegularExpression(NAMES_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexErrorMessage")]
        public string CardholderName
        {
            get { return _cardholderName; }
            set { SetProperty(ref _cardholderName, value); }
        }

        ////[Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        ////[RegularExpression(MONTHNUMBERS_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "CardMonthInvalid")]
        public string ExpirationMonth
        {
            get { return _expirationMonth; }
            set { SetProperty(ref _expirationMonth, value); }
        }

        ////[Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        ////[RegularExpression(YEARNUMBERS_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "CardYearInvalid")]
        public string ExpirationYear
        {
            get { return _expirationYear; }
            set { SetProperty(ref _expirationYear, value); }
        }

        ////[RegularExpression(NUMBERS_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexErrorMessage")]
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        ////[Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        ////[RegularExpression(NUMBERS_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexErrorMessage")]
        public string CardVerificationCode
        {
            get { return _cardVerificationCode; }
            set { SetProperty(ref _cardVerificationCode, value); }
        }

        public bool IsDefault { get; set; }
    }
}

using Prism.Windows.AppModel;
using Windows.ApplicationModel.Resources;

namespace AdventureWorks.UILogic.Services {
    public static class ErrorMessagesHelper
    {
        static ErrorMessagesHelper()
        {
            // TODO: 'ResourceLoader.ResourceLoader(string)' is obsolete: 'ResourceLoader may be altered or unavailable for releases after Windows 8.1. Instead, use GetForCurrentView.
            ResourceLoader = new ResourceLoaderAdapter(new ResourceLoader("AdventureWorks.UILogic/Resources"));
        }

        public static IResourceLoader ResourceLoader { get; set; }

        public static string RequiredErrorMessage
        {
            get { return ResourceLoader.GetString("ErrorRequired"); }
        }

        public static string RegexErrorMessage
        {
            get { return ResourceLoader.GetString("ErrorRegex"); }
        }

        public static string ZipCodeLengthInvalidErrorMessage
        {
            get { return ResourceLoader.GetString("ZipCodeLengthInvalidErrorMessage"); }
        }

        public static string ZipCodeRegexErrorMessage
        {
            get { return ResourceLoader.GetString("ZipCodeRegexErrorMessage"); }
        }

        public static string CardNumberInvalidLengthErrorMessage
        {
            get { return ResourceLoader.GetString("ErrorCardNumberInvalidLength"); }
        }

        public static string CardMonthInvalid
        {
            get { return ResourceLoader.GetString("ErrorCardMonthInvalid"); }
        }

        public static string CardYearInvalid
        {
            get { return ResourceLoader.GetString("ErrorCardYearInvalid"); }
        }

        public static string GeneralServiceErrorMessage
        {
            get { return ResourceLoader.GetString("GeneralServiceErrorMessage"); }
        }

        public static string ErrorProcessingOrder
        {
            get { return ResourceLoader.GetString("ErrorProcessingOrder"); }
        }
    }
}

using System;
namespace Ispm.Core.Helpers
{
    public class ErrorHelper
    {
        public static string GetErrorMessage(string methodeName, Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage = ex.InnerException.Message;
                if (ex.InnerException.InnerException != null)
                {
                    errMessage = ex.InnerException.InnerException.Message;
                    if (ex.InnerException.InnerException.InnerException != null)
                    {
                        errMessage = ex.InnerException.InnerException.InnerException.Message;
                    }
                }
            }
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = ex.StackTrace!.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
            }
            var isDevelopment = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development", StringComparison.InvariantCultureIgnoreCase);
            string err = methodeName + ", line: " + lineNumber + Environment.NewLine + "Error Message: " + errMessage;
            // if (!isDevelopment)
            // {
            //     err = "Internal Server Error";
            //     _ = TelegramBot.PushNotifAsync(methodeName, lineNumber, errMessage);
            // }
            return err;
        }
    }
}



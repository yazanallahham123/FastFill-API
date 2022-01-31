using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Utils.Messages
{
    public class LoginErrorMessages
    {
        public const String MOBILE_NUMBER_REQUIRED = "Mobile number is required.";
        public const String PASSWORD_REQUIRED = "Password is required.";
        public const String VERIFICATION_ID_REQUIRED = "OTP Verification Id is required.";
        public const String OLD_PASSWORD_REQUIRED = "Old password is required.";
        public const String INCORRECT_PASSWORD = "Password is incorrect.";
        public const String INCORRECT_OLD_PASSWORD = "Old password is incorrect.";

        public static List<String> ModelStateParser(ModelStateDictionary modelStateDictionary)
        {
            return modelStateDictionary.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)).ToList();
        }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Utils.Messages
{
    public class UserErrorMessages
    {
        public const String FIRST_NAME_REQUIRED = "First Name is required.";
        public const String LAST_NAME_REQUIRED = "Last Name is required.";
        public const String PASSWORD_REQUIRED = "Password is required.";
        public const String MOBILE_NUMBER_REQUIRED = "Mobile number is required.";
        public const String MOBILE_NUMBER_EXISTS = "The mobile number already exists.";
        public const String INVALID_MOBILE_NUMBER_VALUE = "Invalid mobile number value.";
        public const String INVALID_USER_ID_VALUE = "Invalid user id value.";
        public const String USER_ID_REQUIRED = "User id is required.";
        public const String NOT_OWNER = "Sorry, you are not the account owner";
        public const String INVALID_ROLE = "Role type value doesn't exist within enum should be one of [Admin, Support, User]";

        public static List<String> ModelStateParser(ModelStateDictionary modelStateDictionary)
        {
            return modelStateDictionary.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)).ToList();
        }
    }
}

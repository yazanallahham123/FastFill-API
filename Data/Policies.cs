using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Data
{
    public class Policies
    {
        public const string Admin = "1";
        public const string Support = "2";
        public const string User = "3";
        public const string Company = "4";
        public const string Sybertech = "100";
        public const string Bushrapay = "101";
        public const string CompanyAgent = "5";
        public const string Faisal = "102";

        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
        }

        public static AuthorizationPolicy SupportPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Support).Build();
        }

        public static AuthorizationPolicy UserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(User).Build();
        }
        public static AuthorizationPolicy CompanyPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Company).Build();
        }

        public static AuthorizationPolicy SyberTechPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Sybertech).Build();
        }

        public static AuthorizationPolicy BushraPayPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Bushrapay).Build();
        }

        public static AuthorizationPolicy CompanyAgentPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(CompanyAgent).Build();
        }

        public static AuthorizationPolicy FaisalPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Faisal).Build();
        }


    }
}

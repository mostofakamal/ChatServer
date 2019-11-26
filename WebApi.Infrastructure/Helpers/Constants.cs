using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WebApi.Infrastructure.Helpers
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }

            public static class CorsPolicy
            {
                public const string Name = "CorsPolicy";
            }

            public static string RemoveAllNonPrintableCharacters(string target)
            {
                return Regex.Replace(target, @"\p{C}+", string.Empty);
            }
        }
    }
}

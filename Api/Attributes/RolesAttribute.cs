using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Authorization;
using TechBodiaApi.Data.Definitions;

namespace TechBodiaApi.Api.Attributes
{
    /// <summary>
    /// Custom authorization attribute for role-based access control
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RolesAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Constructor for a single role
        /// </summary>
        /// <param name="role">Single Role</param>
        public RolesAttribute(Roles role)
        {
            Roles = GetEnumMemberValue(role);
        }

        /// <summary>
        /// Constructor for multiple roles
        /// </summary>
        /// <param name="roles">Multiple Roles</param>
        public RolesAttribute(params Roles[] roles)
        {
            Roles = string.Join(",", roles.Select(GetEnumMemberValue));
        }

        /// <summary>
        /// Retrieves the EnumMember value if available; otherwise, defaults to ToString()
        /// </summary>
        private static string GetEnumMemberValue(Roles role)
        {
            return role.GetType()
                       .GetMember(role.ToString())
                       .FirstOrDefault()?
                       .GetCustomAttribute<EnumMemberAttribute>()?
                       .Value ?? role.ToString();
        }
    }
}

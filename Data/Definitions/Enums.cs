using System.Runtime.Serialization;

namespace TechBodiaApi.Data.Definitions
{
    public enum Roles
    {
        [EnumMember(Value = "User")]
        User = 1,

        [EnumMember(Value = "Admin")]
        Admin = 9,
    }
}

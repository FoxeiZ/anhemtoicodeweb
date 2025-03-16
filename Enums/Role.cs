using Microsoft.Ajax.Utilities;
using System;

namespace anhemtoicodeweb.Enums
{
    public enum Role
    {
        Admin,
        Seller,
        SellerPending,
        Customer,
        Disabled,
        Empty
    }

    public static class RoleExtensions
    {
        public static bool EqualsTo(object obj, params Role[] roles)
        {
            var c_role = obj.IfNotNull(o => (Role)o, Role.Empty);
            if (c_role.Equals(Role.Admin))
                return true;
            return obj != null && Array.Exists(roles, r => r == c_role);
        }
    }
}
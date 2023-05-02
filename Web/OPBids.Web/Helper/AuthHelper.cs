using Microsoft.Owin;
using OPBids.Entities.View.Shared;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using OPBids.Common;

namespace OPBids.Web.Helper
{
    public static class AuthHelper
    {
        public static string GetClaims(IOwinContext ctx, string name) {                
            ClaimsPrincipal user = ctx.Authentication.User;
            IEnumerable<Claim> claims = user.Claims;

            string _claim = string.Empty;
            foreach (Claim c in claims)
            {
                if (c.Type == name)
                {
                    _claim = c.Value;
                }
            }
            return _claim;
        }
    }
}
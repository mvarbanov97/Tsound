using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models.SpotifyDomainModels
{
    public class UserAuth
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public bool Authorized { get; set; }
        public string Scopes { get; set; }
    }

    public static class UserAuthExtensions
    {
        public static void EnforceInvariants(this UserAuth userAuth)
        {
            if (string.IsNullOrEmpty(userAuth.UserId)) throw new InvalidOperationException("UserHash must not be null");
        }
    }
}

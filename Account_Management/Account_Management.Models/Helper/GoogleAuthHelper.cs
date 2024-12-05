using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.Models.Helper
{
    public class GoogleAuthHelper
    {
        public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string googleToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string>
            {
                "52674306659-2glgruq6dk2jnrol9pg6ip9ub8q3gtbj.apps.googleusercontent.com"
            }
            };

            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken, settings);
                return payload;
            }
            catch
            {
                return null;
            }
        }
    }
}

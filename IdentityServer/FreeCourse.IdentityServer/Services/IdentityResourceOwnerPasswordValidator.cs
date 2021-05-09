using FreeCourse.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existsUser = await _userManager.FindByEmailAsync(context.UserName);
            if (existsUser == null)
            {
                var error = new Dictionary<string, object>();
                error.Add("errors", new List<string> { "Email veya şifreniz hatalı" });

                context.Result.CustomResponse = error;
                return;
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(existsUser, context.Password);

            if (!passwordCheck)
            {
                var error = new Dictionary<string, object>();
                error.Add("errors", new List<string> { "Email veya şifreniz hatalı" });

                context.Result.CustomResponse = error;
                return;
            }


            context.Result = new GrantValidationResult(existsUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}

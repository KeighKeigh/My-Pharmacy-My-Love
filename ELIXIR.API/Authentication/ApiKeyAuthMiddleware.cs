using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;


namespace ELIXIR.API.Authentication
{
    public class ApiKeyAuthAttribute : Attribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

            if (configuration == null)
            {
                context.Result = new StatusCodeResult(500);
                return;
            }

            var apiKey = configuration.GetValue<string>(AuthConstants.ApiKeySectionName);

            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey) ||
                string.IsNullOrEmpty(apiKey) ||
                !apiKey.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("Invalid or missing API Key");
            }
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ToolkitBoilerplate.Infrastructure
{
    public class ValidateRecaptchaAttribute : ActionFilterAttribute
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;
        private const string verificationEndpoint = "https://www.google.com/recaptcha/api/siteverify";

        public ValidateRecaptchaAttribute(IConfiguration config, IHostingEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await ValidateReCaptcha(context);
            await base.OnActionExecutionAsync(context, next);
        }

        private async Task ValidateReCaptcha(ActionExecutingContext context)
        {
            // if (_env.IsDevelopment())
            //    return;

            var secret = _config["Recaptcha:Secret"];
            var gRecaptchaResponse = context.HttpContext.Request.Form["g-recaptcha-response"];

            if (string.IsNullOrWhiteSpace(secret))
                return;

            if (string.IsNullOrWhiteSpace(gRecaptchaResponse))
            {
                context.ModelState.AddModelError("reCAPTCHA", "reCAPTCHA not found.");
                // Might be using on a GET request. Don't do that.
            }
            else
            {
                using (var webClient = new HttpClient())
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("secret", secret),
                        new KeyValuePair<string, string>("response", gRecaptchaResponse)
                    });

                    HttpResponseMessage rawResponse = await webClient.PostAsync(verificationEndpoint, content);
                    string json = await rawResponse.Content.ReadAsStringAsync();
                    dynamic response = JObject.Parse(json);
                    if (response == null)
                    {
                        context.ModelState.AddModelError("reCAPTCHA", "No response recieved from reCAPTCHA.");
                    }
                    else if (!(bool)response.success)
                    {
                        context.ModelState.AddModelError("reCAPTCHA", "Invalid reCAPTCHA submitted.");
                    }
                }
            }
        }
    }
}

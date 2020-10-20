using System;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace AppSettingsCors.WebApi
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AppSettingsCorsAttribute : Attribute, ICorsPolicyProvider
    {
        private readonly CorsPolicy policy;

        public AppSettingsCorsAttribute(
            string allowedOriginsAppSettingName,
            string allowedHeadersAppSettingName,
            string allowedMethodsAppSettingName,
            string supportsCredentialsAppSettingName = null
        )
        {
            policy = new CorsPolicy();
            ConfigureOrigins(allowedOriginsAppSettingName);
            ConfigureHeaders(allowedHeadersAppSettingName);
            ConfigureMethods(allowedMethodsAppSettingName);
            ConfigureSupportsCredentials(supportsCredentialsAppSettingName);
        }

        private void ConfigureOrigins(string allowedOriginsAppSettingName)
        {
            if (string.IsNullOrEmpty(allowedOriginsAppSettingName))
            {
                throw new ArgumentNullException(nameof(allowedOriginsAppSettingName));
            }

            var origins = ConfigurationManager.AppSettings[allowedOriginsAppSettingName];

            if (string.IsNullOrEmpty(origins))
            {
                throw new Exception($"CORS Origins AppSetting is null or empty: {allowedOriginsAppSettingName}");
            }
            else if (origins == "*")
            {
                policy.AllowAnyOrigin = true;
            }
            else
            {
                foreach (var origin in origins.Split(','))
                {
                    policy.Origins.Add(origin.Trim());
                }
            }
        }

        private void ConfigureHeaders(string allowedHeadersAppSettingName)
        {

            if (string.IsNullOrEmpty(allowedHeadersAppSettingName))
            {
                throw new ArgumentNullException(nameof(allowedHeadersAppSettingName));
            }

            var headers = ConfigurationManager.AppSettings[allowedHeadersAppSettingName];

            if (string.IsNullOrEmpty(headers))
            {
                throw new Exception($"CORS Headers AppSetting is null or empty: {allowedHeadersAppSettingName}");
            }
            else if (headers == "*")
            {
                policy.AllowAnyHeader = true;
            }
            else
            {
                foreach (var header in headers.Split(','))
                {
                    policy.Headers.Add(header.Trim());
                }
            }
        }

        private void ConfigureMethods(string allowedMethodsAppSettingName)
        {
            if (string.IsNullOrEmpty(allowedMethodsAppSettingName))
            {
                throw new ArgumentNullException(nameof(allowedMethodsAppSettingName));
            }

            var methods = ConfigurationManager.AppSettings[allowedMethodsAppSettingName];

            if (string.IsNullOrEmpty(methods))
            {
                throw new Exception($"CORS Methods AppSetting is null or empty: {allowedMethodsAppSettingName}");
            }
            else if (methods == "*")
            {
                policy.AllowAnyMethod = true;
            }
            else
            {
                foreach (var verb in methods.Split(','))
                {
                    policy.Methods.Add(verb.Trim());
                }
            }
        }

        private void ConfigureSupportsCredentials(string supportsCredentialsAppSettingName)
        {
            if (string.IsNullOrEmpty(supportsCredentialsAppSettingName))
            {
                return;
            }

            var supportsCredentialsString = ConfigurationManager.AppSettings[supportsCredentialsAppSettingName];

            if (string.IsNullOrEmpty(supportsCredentialsString))
            {
                throw new Exception($"CORS SupportsCredentials AppSetting is null or empty: {supportsCredentialsAppSettingName}");
            }

            if (!bool.TryParse(supportsCredentialsString, out bool supportsCredentials))
            {
                throw new Exception($"CORS SupportsCredentials AppSetting is cannot be parsed as boolean: {supportsCredentialsString}");
            }

            policy.SupportsCredentials = supportsCredentials;
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(policy);
        }
    }
}
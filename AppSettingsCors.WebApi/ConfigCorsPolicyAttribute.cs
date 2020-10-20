using System;
using System.Linq;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace AppSettingsCors.WebApi
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ConfigCorsPolicyAttribute : Attribute, ICorsPolicyProvider
    {
        private readonly CorsPolicy policy;
        private readonly string policyKey;

        public ConfigCorsPolicyAttribute(string policyKey)
        {
            policy = new CorsPolicy();
            this.policyKey = policyKey;
            var policiesSection = (CorsPoliciesSection)ConfigurationManager.GetSection("CorsPolicies");
            var corsPolicyElement = policiesSection.CorsPolicies.OfType<CorsPolicyElement>().FirstOrDefault(e => e.Key == policyKey);
            ConfigureOrigins(corsPolicyElement.AllowedOrigins.TextContent);
            ConfigureHeaders(corsPolicyElement.AllowedHeaders.TextContent);
            ConfigureMethods(corsPolicyElement.AllowedMethods.TextContent);
            ConfigureSupportsCredentials(corsPolicyElement.SupportsCredentials.TextContent);
        }

        private void ConfigureOrigins(string origins)
        {
            if (string.IsNullOrEmpty(origins))
            {
                throw new ArgumentNullException(nameof(origins), $"CORS Origins is null or empty for policy {policyKey}");
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

        private void ConfigureHeaders(string headers)
        {
            if (string.IsNullOrEmpty(headers))
            {
                throw new ArgumentNullException(nameof(headers), $"CORS Headers is null or empty for policy {policyKey}");
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

        private void ConfigureMethods(string methods)
        {
            if (string.IsNullOrEmpty(methods))
            {
                throw new ArgumentNullException(nameof(methods), $"CORS Methods is null or empty for policy {policyKey}");
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

        private void ConfigureSupportsCredentials(string supportsCredentialsString)
        {
            if (string.IsNullOrEmpty(supportsCredentialsString))
            {
                throw new ArgumentNullException(nameof(supportsCredentialsString), $"CORS SupportsCredentials is null or empty for policy {policyKey}");
            }

            if (!bool.TryParse(supportsCredentialsString, out bool supportsCredentials))
            {
                throw new Exception($"CORS SupportsCredentials is cannot be parsed as boolean: {supportsCredentialsString}");
            }

            policy.SupportsCredentials = supportsCredentials;
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(policy);
        }
    }
}
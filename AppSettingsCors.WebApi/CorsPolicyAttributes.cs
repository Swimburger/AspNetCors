using System;

namespace AppSettingsCors.WebApi
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class CorsPolicyAAttribute : AppSettingsCorsAttribute
    {
        public CorsPolicyAAttribute() : base(
            "AllowedOriginsCors_A",
            "AllowedHeadersCors_A",
            "AllowedMethodsCors_A"
        )
        { }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class CorsPolicyBAttribute : AppSettingsCorsAttribute
    {
        public CorsPolicyBAttribute() : base(
            "AllowedOriginsCors_B",
            "AllowedHeadersCors_B",
            "AllowedMethodsCors_B",
            "SupportsCredentialsCors_B"
        )
        { }
    }
}
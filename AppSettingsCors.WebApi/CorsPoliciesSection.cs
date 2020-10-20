using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;

namespace AppSettingsCors.WebApi
{
    public class CorsPoliciesSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(CorsPolicyCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public CorsPolicyCollection CorsPolicies
        {
            get => (CorsPolicyCollection)base[""];
            set =>  this["Policies"] = value;
        }
    }

    public class CorsPolicyCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new CorsPolicyElement();

        protected override object GetElementKey(ConfigurationElement element) => (element as CorsPolicyElement).Key;
    }

    public class CorsPolicyElement : ConfigurationElement
    {
        [ConfigurationProperty("key", Options = ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey)]
        public string Key
        {
            get { return (string)base["key"]; }
            set { base["key"] = value; }
        }

        [ConfigurationProperty("AllowedOrigins", Options = ConfigurationPropertyOptions.IsRequired)]
        public TextContentConfigurationElement AllowedOrigins
        {
            get { return (TextContentConfigurationElement)base["AllowedOrigins"]; }
            set { base["AllowedOrigins"] = value; }
        }

        [ConfigurationProperty("AllowedHeaders", Options = ConfigurationPropertyOptions.IsRequired)]
        public TextContentConfigurationElement AllowedHeaders
        {
            get { return (TextContentConfigurationElement)base["AllowedHeaders"]; }
            set { base["AllowedHeaders"] = value; }
        }

        [ConfigurationProperty("AllowedMethods", Options = ConfigurationPropertyOptions.IsRequired)]
        public TextContentConfigurationElement AllowedMethods
        {
            get { return (TextContentConfigurationElement)base["AllowedMethods"]; }
            set { base["AllowedMethods"] = value; }
        }

        [ConfigurationProperty("SupportsCredentials", Options = ConfigurationPropertyOptions.IsRequired)]
        public TextContentConfigurationElement SupportsCredentials
        {
            get { return (TextContentConfigurationElement)base["SupportsCredentials"]; }
            set { base["SupportsCredentials"] = value; }
        }
    }

    public class TextContentConfigurationElement : ConfigurationElement
    {
        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            TextContent = reader.ReadElementContentAsString();
        }

        public string TextContent { get; private set; }
    }
}
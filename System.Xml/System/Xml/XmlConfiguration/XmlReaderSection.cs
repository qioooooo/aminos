using System;
using System.ComponentModel;
using System.Configuration;

namespace System.Xml.XmlConfiguration
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class XmlReaderSection : ConfigurationSection
	{
		[ConfigurationProperty("prohibitDefaultResolver", DefaultValue = "false")]
		internal string ProhibitDefaultResolverString
		{
			get
			{
				return (string)base["prohibitDefaultResolver"];
			}
			set
			{
				base["prohibitDefaultResolver"] = value;
			}
		}

		private bool _ProhibitDefaultResolver
		{
			get
			{
				string prohibitDefaultResolverString = this.ProhibitDefaultResolverString;
				bool flag;
				XmlConvert.TryToBoolean(prohibitDefaultResolverString, out flag);
				return flag;
			}
		}

		internal static bool ProhibitDefaultUrlResolver
		{
			get
			{
				XmlReaderSection xmlReaderSection = ConfigurationManager.GetSection(XmlConfigurationString.XmlReaderSectionPath) as XmlReaderSection;
				return xmlReaderSection != null && xmlReaderSection._ProhibitDefaultResolver;
			}
		}

		internal static XmlResolver CreateDefaultResolver()
		{
			if (XmlReaderSection.ProhibitDefaultUrlResolver)
			{
				return null;
			}
			return new XmlUrlResolver();
		}
	}
}

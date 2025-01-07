using System;
using System.Globalization;

namespace System.Xml.XmlConfiguration
{
	internal static class XmlConfigurationString
	{
		internal const string XmlReaderSectionName = "xmlReader";

		internal const string XmlTextReaderSectionName = "xmlTextReader";

		internal const string XsltSectionName = "xslt";

		internal const string ProhibitDefaultResolverName = "prohibitDefaultResolver";

		internal const string LimitCharactersFromEntitiesName = "limitCharactersFromEntities";

		internal const string LimitXPathComplexityName = "limitXPathComplexity";

		internal const string EnableMemberAccessForXslCompiledTransformName = "enableMemberAccessForXslCompiledTransform";

		internal const string XmlConfigurationSectionName = "system.xml";

		internal static string XmlReaderSectionPath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[] { "system.xml", "xmlReader" });

		internal static string XmlTextReaderSectionPath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[] { "system.xml", "xmlTextReader" });

		internal static string XsltSectionPath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[] { "system.xml", "xslt" });
	}
}

using System;
using System.ComponentModel;
using System.Configuration;

namespace System.Xml.XmlConfiguration
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class XmlTextReaderSection : ConfigurationSection
	{
		[ConfigurationProperty("limitCharactersFromEntities", DefaultValue = "true")]
		internal string LimitCharactersFromEntitiesString
		{
			get
			{
				return (string)base["limitCharactersFromEntities"];
			}
			set
			{
				base["limitCharactersFromEntities"] = value;
			}
		}

		private bool _LimitCharactersFromEntities
		{
			get
			{
				string limitCharactersFromEntitiesString = this.LimitCharactersFromEntitiesString;
				bool flag = true;
				XmlConvert.TryToBoolean(limitCharactersFromEntitiesString, out flag);
				return flag;
			}
		}

		internal static bool LimitCharactersFromEntities
		{
			get
			{
				XmlTextReaderSection xmlTextReaderSection = ConfigurationManager.GetSection(XmlConfigurationString.XmlTextReaderSectionPath) as XmlTextReaderSection;
				return xmlTextReaderSection == null || xmlTextReaderSection._LimitCharactersFromEntities;
			}
		}
	}
}

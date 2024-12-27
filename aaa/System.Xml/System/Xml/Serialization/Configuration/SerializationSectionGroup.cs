using System;
using System.Configuration;

namespace System.Xml.Serialization.Configuration
{
	// Token: 0x02000354 RID: 852
	public sealed class SerializationSectionGroup : ConfigurationSectionGroup
	{
		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x0600293D RID: 10557 RVA: 0x000D38FC File Offset: 0x000D28FC
		[ConfigurationProperty("schemaImporterExtensions")]
		public SchemaImporterExtensionsSection SchemaImporterExtensions
		{
			get
			{
				return (SchemaImporterExtensionsSection)base.Sections["schemaImporterExtensions"];
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x0600293E RID: 10558 RVA: 0x000D3913 File Offset: 0x000D2913
		[ConfigurationProperty("dateTimeSerialization")]
		public DateTimeSerializationSection DateTimeSerialization
		{
			get
			{
				return (DateTimeSerializationSection)base.Sections["dateTimeSerialization"];
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x0600293F RID: 10559 RVA: 0x000D392A File Offset: 0x000D292A
		public XmlSerializerSection XmlSerializer
		{
			get
			{
				return (XmlSerializerSection)base.Sections["xmlSerializer"];
			}
		}
	}
}

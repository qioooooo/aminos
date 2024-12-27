using System;
using System.Globalization;

namespace System.Xml.Serialization.Configuration
{
	// Token: 0x0200034C RID: 844
	internal static class ConfigurationStrings
	{
		// Token: 0x06002910 RID: 10512 RVA: 0x000D31D0 File Offset: 0x000D21D0
		private static string GetSectionPath(string sectionName)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[] { "system.xml.serialization", sectionName });
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06002911 RID: 10513 RVA: 0x000D3200 File Offset: 0x000D2200
		internal static string SchemaImporterExtensionsSectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("schemaImporterExtensions");
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06002912 RID: 10514 RVA: 0x000D320C File Offset: 0x000D220C
		internal static string DateTimeSerializationSectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("dateTimeSerialization");
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06002913 RID: 10515 RVA: 0x000D3218 File Offset: 0x000D2218
		internal static string XmlSerializerSectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("xmlSerializer");
			}
		}

		// Token: 0x040016B9 RID: 5817
		internal const string Name = "name";

		// Token: 0x040016BA RID: 5818
		internal const string SchemaImporterExtensionsSectionName = "schemaImporterExtensions";

		// Token: 0x040016BB RID: 5819
		internal const string DateTimeSerializationSectionName = "dateTimeSerialization";

		// Token: 0x040016BC RID: 5820
		internal const string XmlSerializerSectionName = "xmlSerializer";

		// Token: 0x040016BD RID: 5821
		internal const string SectionGroupName = "system.xml.serialization";

		// Token: 0x040016BE RID: 5822
		internal const string SqlTypesSchemaImporterChar = "SqlTypesSchemaImporterChar";

		// Token: 0x040016BF RID: 5823
		internal const string SqlTypesSchemaImporterNChar = "SqlTypesSchemaImporterNChar";

		// Token: 0x040016C0 RID: 5824
		internal const string SqlTypesSchemaImporterVarChar = "SqlTypesSchemaImporterVarChar";

		// Token: 0x040016C1 RID: 5825
		internal const string SqlTypesSchemaImporterNVarChar = "SqlTypesSchemaImporterNVarChar";

		// Token: 0x040016C2 RID: 5826
		internal const string SqlTypesSchemaImporterText = "SqlTypesSchemaImporterText";

		// Token: 0x040016C3 RID: 5827
		internal const string SqlTypesSchemaImporterNText = "SqlTypesSchemaImporterNText";

		// Token: 0x040016C4 RID: 5828
		internal const string SqlTypesSchemaImporterVarBinary = "SqlTypesSchemaImporterVarBinary";

		// Token: 0x040016C5 RID: 5829
		internal const string SqlTypesSchemaImporterBinary = "SqlTypesSchemaImporterBinary";

		// Token: 0x040016C6 RID: 5830
		internal const string SqlTypesSchemaImporterImage = "SqlTypesSchemaImporterImage";

		// Token: 0x040016C7 RID: 5831
		internal const string SqlTypesSchemaImporterDecimal = "SqlTypesSchemaImporterDecimal";

		// Token: 0x040016C8 RID: 5832
		internal const string SqlTypesSchemaImporterNumeric = "SqlTypesSchemaImporterNumeric";

		// Token: 0x040016C9 RID: 5833
		internal const string SqlTypesSchemaImporterBigInt = "SqlTypesSchemaImporterBigInt";

		// Token: 0x040016CA RID: 5834
		internal const string SqlTypesSchemaImporterInt = "SqlTypesSchemaImporterInt";

		// Token: 0x040016CB RID: 5835
		internal const string SqlTypesSchemaImporterSmallInt = "SqlTypesSchemaImporterSmallInt";

		// Token: 0x040016CC RID: 5836
		internal const string SqlTypesSchemaImporterTinyInt = "SqlTypesSchemaImporterTinyInt";

		// Token: 0x040016CD RID: 5837
		internal const string SqlTypesSchemaImporterBit = "SqlTypesSchemaImporterBit";

		// Token: 0x040016CE RID: 5838
		internal const string SqlTypesSchemaImporterFloat = "SqlTypesSchemaImporterFloat";

		// Token: 0x040016CF RID: 5839
		internal const string SqlTypesSchemaImporterReal = "SqlTypesSchemaImporterReal";

		// Token: 0x040016D0 RID: 5840
		internal const string SqlTypesSchemaImporterDateTime = "SqlTypesSchemaImporterDateTime";

		// Token: 0x040016D1 RID: 5841
		internal const string SqlTypesSchemaImporterSmallDateTime = "SqlTypesSchemaImporterSmallDateTime";

		// Token: 0x040016D2 RID: 5842
		internal const string SqlTypesSchemaImporterMoney = "SqlTypesSchemaImporterMoney";

		// Token: 0x040016D3 RID: 5843
		internal const string SqlTypesSchemaImporterSmallMoney = "SqlTypesSchemaImporterSmallMoney";

		// Token: 0x040016D4 RID: 5844
		internal const string SqlTypesSchemaImporterUniqueIdentifier = "SqlTypesSchemaImporterUniqueIdentifier";

		// Token: 0x040016D5 RID: 5845
		internal const string Type = "type";

		// Token: 0x040016D6 RID: 5846
		internal const string Mode = "mode";

		// Token: 0x040016D7 RID: 5847
		internal const string CheckDeserializeAdvances = "checkDeserializeAdvances";

		// Token: 0x040016D8 RID: 5848
		internal const string TempFilesLocation = "tempFilesLocation";
	}
}

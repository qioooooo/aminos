using System;
using System.Configuration;
using System.Xml.Serialization.Advanced;

namespace System.Xml.Serialization.Configuration
{
	// Token: 0x02000353 RID: 851
	public sealed class SchemaImporterExtensionsSection : ConfigurationSection
	{
		// Token: 0x06002936 RID: 10550 RVA: 0x000D3545 File Offset: 0x000D2545
		public SchemaImporterExtensionsSection()
		{
			this.properties.Add(this.schemaImporterExtensions);
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x000D3581 File Offset: 0x000D2581
		private static string GetSqlTypeSchemaImporter(string typeName)
		{
			return "System.Data.SqlTypes." + typeName + ", System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x000D3594 File Offset: 0x000D2594
		protected override void InitializeDefault()
		{
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterChar", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeCharSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterNChar", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeNCharSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterVarChar", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeVarCharSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterNVarChar", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeNVarCharSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterText", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeTextSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterNText", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeNTextSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterVarBinary", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeVarBinarySchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterBinary", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeBinarySchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterImage", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeVarImageSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterDecimal", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeDecimalSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterNumeric", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeNumericSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterBigInt", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeBigIntSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterInt", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeIntSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterSmallInt", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeSmallIntSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterTinyInt", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeTinyIntSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterBit", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeBitSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterFloat", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeFloatSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterReal", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeRealSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterDateTime", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeDateTimeSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterSmallDateTime", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeSmallDateTimeSchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterMoney", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeMoneySchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterSmallMoney", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeSmallMoneySchemaImporterExtension")));
			this.SchemaImporterExtensions.Add(new SchemaImporterExtensionElement("SqlTypesSchemaImporterUniqueIdentifier", SchemaImporterExtensionsSection.GetSqlTypeSchemaImporter("TypeUniqueIdentifierSchemaImporterExtension")));
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06002939 RID: 10553 RVA: 0x000D386A File Offset: 0x000D286A
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600293A RID: 10554 RVA: 0x000D3872 File Offset: 0x000D2872
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public SchemaImporterExtensionElementCollection SchemaImporterExtensions
		{
			get
			{
				return (SchemaImporterExtensionElementCollection)base[this.schemaImporterExtensions];
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x0600293B RID: 10555 RVA: 0x000D3888 File Offset: 0x000D2888
		internal SchemaImporterExtensionCollection SchemaImporterExtensionsInternal
		{
			get
			{
				SchemaImporterExtensionCollection schemaImporterExtensionCollection = new SchemaImporterExtensionCollection();
				foreach (object obj in this.SchemaImporterExtensions)
				{
					SchemaImporterExtensionElement schemaImporterExtensionElement = (SchemaImporterExtensionElement)obj;
					schemaImporterExtensionCollection.Add(schemaImporterExtensionElement.Name, schemaImporterExtensionElement.Type);
				}
				return schemaImporterExtensionCollection;
			}
		}

		// Token: 0x040016E4 RID: 5860
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x040016E5 RID: 5861
		private readonly ConfigurationProperty schemaImporterExtensions = new ConfigurationProperty(null, typeof(SchemaImporterExtensionElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}

using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.Design;
using System.Xml;

namespace System.Data.Design
{
	// Token: 0x02000092 RID: 146
	[DataSourceXmlClass("Connection")]
	internal class DesignConnection : DataSourceComponent, IDesignConnection, IDataSourceNamedObject, INamedObject, ICloneable, IDataSourceInitAfterLoading, IDataSourceXmlSpecialOwner, IDataSourceCollectionMember
	{
		// Token: 0x06000611 RID: 1553 RVA: 0x0000BBC8 File Offset: 0x0000ABC8
		public DesignConnection()
		{
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0000BBE6 File Offset: 0x0000ABE6
		public DesignConnection(string connectionName, ConnectionString cs, string provider)
		{
			this.name = connectionName;
			this.connectionStringObject = cs;
			this.provider = provider;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0000BC1C File Offset: 0x0000AC1C
		public DesignConnection(string connectionName, IDbConnection conn)
		{
			if (conn == null)
			{
				throw new ArgumentNullException("conn");
			}
			this.name = connectionName;
			DbProviderFactory factoryFromType = ProviderManager.GetFactoryFromType(conn.GetType(), ProviderManager.ProviderSupportedClasses.DbConnection);
			this.provider = ProviderManager.GetInvariantProviderName(factoryFromType);
			this.connectionStringObject = new ConnectionString(this.provider, conn.ConnectionString);
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x0000BC8A File Offset: 0x0000AC8A
		internal static string ConnectionNameRegex
		{
			get
			{
				return DesignConnection.regexIdentifier;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0000BC91 File Offset: 0x0000AC91
		// (set) Token: 0x06000616 RID: 1558 RVA: 0x0000BC99 File Offset: 0x0000AC99
		[DataSourceXmlAttribute]
		[DefaultValue(MemberAttributes.Assembly)]
		public MemberAttributes Modifier
		{
			get
			{
				return this.modifier;
			}
			set
			{
				this.modifier = value;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x0000BCA2 File Offset: 0x0000ACA2
		// (set) Token: 0x06000618 RID: 1560 RVA: 0x0000BCAA File Offset: 0x0000ACAA
		[DataSourceXmlAttribute]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					if (this.CollectionParent != null)
					{
						this.CollectionParent.ValidateUniqueName(this, value);
					}
					this.name = value;
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x0000BCD6 File Offset: 0x0000ACD6
		// (set) Token: 0x0600061A RID: 1562 RVA: 0x0000BCDE File Offset: 0x0000ACDE
		[DataSourceXmlAttribute(SpecialWay = true)]
		[Browsable(false)]
		public ConnectionString ConnectionStringObject
		{
			get
			{
				return this.connectionStringObject;
			}
			set
			{
				this.connectionStringObject = value;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x0000BCE7 File Offset: 0x0000ACE7
		// (set) Token: 0x0600061C RID: 1564 RVA: 0x0000BD02 File Offset: 0x0000AD02
		public string ConnectionString
		{
			get
			{
				if (this.ConnectionStringObject != null)
				{
					return this.ConnectionStringObject.ToString();
				}
				return string.Empty;
			}
			set
			{
				if (this.ConnectionStringObject != null)
				{
					this.ConnectionStringObject = new ConnectionString(this.provider, value);
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x0000BD1E File Offset: 0x0000AD1E
		// (set) Token: 0x0600061E RID: 1566 RVA: 0x0000BD26 File Offset: 0x0000AD26
		[Browsable(false)]
		[DataSourceXmlAttribute]
		public string Provider
		{
			get
			{
				return this.provider;
			}
			set
			{
				this.provider = value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x0000BD2F File Offset: 0x0000AD2F
		// (set) Token: 0x06000620 RID: 1568 RVA: 0x0000BD37 File Offset: 0x0000AD37
		[Browsable(false)]
		[DataSourceXmlAttribute]
		public bool IsAppSettingsProperty
		{
			get
			{
				return this.isAppSettingsProperty;
			}
			set
			{
				this.isAppSettingsProperty = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x0000BD40 File Offset: 0x0000AD40
		// (set) Token: 0x06000622 RID: 1570 RVA: 0x0000BD48 File Offset: 0x0000AD48
		[Browsable(false)]
		[DataSourceXmlAttribute]
		public string AppSettingsObjectName
		{
			get
			{
				return this.appSettingsObjectName;
			}
			set
			{
				this.appSettingsObjectName = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x0000BD51 File Offset: 0x0000AD51
		// (set) Token: 0x06000624 RID: 1572 RVA: 0x0000BD59 File Offset: 0x0000AD59
		[DataSourceXmlAttribute(SpecialWay = true)]
		[Browsable(false)]
		public CodePropertyReferenceExpression PropertyReference
		{
			get
			{
				return this.propertyReference;
			}
			set
			{
				this.propertyReference = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x0000BD62 File Offset: 0x0000AD62
		// (set) Token: 0x06000626 RID: 1574 RVA: 0x0000BD6A File Offset: 0x0000AD6A
		[DataSourceXmlAttribute]
		[Browsable(false)]
		public string ParameterPrefix
		{
			get
			{
				return this.parameterPrefix;
			}
			set
			{
				this.parameterPrefix = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0000BD73 File Offset: 0x0000AD73
		[Browsable(false)]
		public IDictionary Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x0000BD7B File Offset: 0x0000AD7B
		[Browsable(false)]
		public string PublicTypeName
		{
			get
			{
				return "Connection";
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0000BD84 File Offset: 0x0000AD84
		public IDbConnection CreateEmptyDbConnection()
		{
			DbProviderFactory factory = ProviderManager.GetFactory(this.provider);
			return factory.CreateConnection();
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0000BDA4 File Offset: 0x0000ADA4
		public object Clone()
		{
			DesignConnection designConnection = new DesignConnection();
			designConnection.Name = this.name;
			if (this.ConnectionStringObject != null)
			{
				designConnection.ConnectionStringObject = (ConnectionString)((ICloneable)this.ConnectionStringObject).Clone();
			}
			designConnection.provider = this.provider;
			designConnection.isAppSettingsProperty = this.isAppSettingsProperty;
			designConnection.propertyReference = this.propertyReference;
			designConnection.properties = (HybridDictionary)DesignUtil.CloneDictionary(this.properties);
			return designConnection;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0000BE24 File Offset: 0x0000AE24
		void IDataSourceInitAfterLoading.InitializeAfterLoading()
		{
			if (this.name == null || this.name.Length == 0)
			{
				throw new DataSourceSerializationException(SR.GetString("DTDS_NameIsRequired", new object[] { "Connection" }));
			}
			if (StringUtil.EmptyOrSpace(this.provider))
			{
				throw new DataSourceSerializationException(SR.GetString("DTDS_CouldNotDeserializeConnection"));
			}
			if (this.connectionStringValue != null)
			{
				this.ConnectionStringObject = new ConnectionString(this.provider, this.connectionStringValue);
			}
			this.properties.Clear();
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0000BEAD File Offset: 0x0000AEAD
		void IDataSourceXmlSpecialOwner.ReadSpecialItem(string propertyName, XmlNode xmlNode, DataSourceXmlSerializer serializer)
		{
			if (propertyName == "ConnectionStringObject")
			{
				this.connectionStringValue = xmlNode.InnerText;
				return;
			}
			if (propertyName == "PropertyReference")
			{
				this.propertyReference = PropertyReferenceSerializer.Deserialize(xmlNode.InnerText);
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0000BEE7 File Offset: 0x0000AEE7
		void IDataSourceXmlSpecialOwner.WriteSpecialItem(string propertyName, XmlWriter writer, DataSourceXmlSerializer serializer)
		{
			if (propertyName == "ConnectionStringObject")
			{
				writer.WriteString(this.ConnectionStringObject.ToFullString());
				return;
			}
			if (propertyName == "PropertyReference")
			{
				writer.WriteString(PropertyReferenceSerializer.Serialize(this.PropertyReference));
			}
		}

		// Token: 0x04000B1B RID: 2843
		private string name;

		// Token: 0x04000B1C RID: 2844
		private ConnectionString connectionStringObject;

		// Token: 0x04000B1D RID: 2845
		private string connectionStringValue;

		// Token: 0x04000B1E RID: 2846
		private string provider;

		// Token: 0x04000B1F RID: 2847
		private bool isAppSettingsProperty;

		// Token: 0x04000B20 RID: 2848
		private string appSettingsObjectName;

		// Token: 0x04000B21 RID: 2849
		private CodePropertyReferenceExpression propertyReference;

		// Token: 0x04000B22 RID: 2850
		private HybridDictionary properties = new HybridDictionary();

		// Token: 0x04000B23 RID: 2851
		private MemberAttributes modifier = MemberAttributes.Assembly;

		// Token: 0x04000B24 RID: 2852
		private static readonly string regexAlphaCharacter = "[\\p{L}\\p{Nl}]";

		// Token: 0x04000B25 RID: 2853
		private static readonly string regexUnderscoreCharacter = "\\p{Pc}";

		// Token: 0x04000B26 RID: 2854
		private static readonly string regexIdentifierCharacter = "[\\p{L}\\p{Nl}\\p{Nd}\\p{Mn}\\p{Mc}\\p{Cf}]";

		// Token: 0x04000B27 RID: 2855
		private static readonly string regexIdentifierStart = string.Concat(new string[]
		{
			"(",
			DesignConnection.regexAlphaCharacter,
			"|(",
			DesignConnection.regexUnderscoreCharacter,
			DesignConnection.regexIdentifierCharacter,
			"))"
		});

		// Token: 0x04000B28 RID: 2856
		private static readonly string regexIdentifier = DesignConnection.regexIdentifierStart + DesignConnection.regexIdentifierCharacter + "*";

		// Token: 0x04000B29 RID: 2857
		private string parameterPrefix;
	}
}

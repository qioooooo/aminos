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
	[DataSourceXmlClass("Connection")]
	internal class DesignConnection : DataSourceComponent, IDesignConnection, IDataSourceNamedObject, INamedObject, ICloneable, IDataSourceInitAfterLoading, IDataSourceXmlSpecialOwner, IDataSourceCollectionMember
	{
		public DesignConnection()
		{
		}

		public DesignConnection(string connectionName, ConnectionString cs, string provider)
		{
			this.name = connectionName;
			this.connectionStringObject = cs;
			this.provider = provider;
		}

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

		internal static string ConnectionNameRegex
		{
			get
			{
				return DesignConnection.regexIdentifier;
			}
		}

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

		[Browsable(false)]
		public IDictionary Properties
		{
			get
			{
				return this.properties;
			}
		}

		[Browsable(false)]
		public string PublicTypeName
		{
			get
			{
				return "Connection";
			}
		}

		public IDbConnection CreateEmptyDbConnection()
		{
			DbProviderFactory factory = ProviderManager.GetFactory(this.provider);
			return factory.CreateConnection();
		}

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

		private string name;

		private ConnectionString connectionStringObject;

		private string connectionStringValue;

		private string provider;

		private bool isAppSettingsProperty;

		private string appSettingsObjectName;

		private CodePropertyReferenceExpression propertyReference;

		private HybridDictionary properties = new HybridDictionary();

		private MemberAttributes modifier = MemberAttributes.Assembly;

		private static readonly string regexAlphaCharacter = "[\\p{L}\\p{Nl}]";

		private static readonly string regexUnderscoreCharacter = "\\p{Pc}";

		private static readonly string regexIdentifierCharacter = "[\\p{L}\\p{Nl}\\p{Nd}\\p{Mn}\\p{Mc}\\p{Cf}]";

		private static readonly string regexIdentifierStart = string.Concat(new string[]
		{
			"(",
			DesignConnection.regexAlphaCharacter,
			"|(",
			DesignConnection.regexUnderscoreCharacter,
			DesignConnection.regexIdentifierCharacter,
			"))"
		});

		private static readonly string regexIdentifier = DesignConnection.regexIdentifierStart + DesignConnection.regexIdentifierCharacter + "*";

		private string parameterPrefix;
	}
}

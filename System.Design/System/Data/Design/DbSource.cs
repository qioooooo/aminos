using System;
using System.CodeDom;
using System.ComponentModel;
using System.Xml;

namespace System.Data.Design
{
	[DataSourceXmlClass("DbSource")]
	internal class DbSource : Source, IDataSourceXmlSpecialOwner
	{
		protected internal override DataSourceCollectionBase CollectionParent
		{
			get
			{
				if (base.CollectionParent != null)
				{
					return base.CollectionParent;
				}
				if (this.owner != null && this.owner is DesignTable && ((DesignTable)this.owner).MainSource == this)
				{
					return ((DesignTable)this.owner).Sources;
				}
				return null;
			}
		}

		[DataSourceXmlAttribute]
		[Browsable(false)]
		public string ConnectionRef
		{
			get
			{
				if (this.connection != null)
				{
					return this.connection.Name;
				}
				return this.connectionRef;
			}
			set
			{
				this.connectionRef = value;
			}
		}

		[Browsable(false)]
		[DataSourceXmlAttribute(SpecialWay = true)]
		public Type ScalarCallRetval
		{
			get
			{
				return this.scalarCallRetval;
			}
		}

		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(null)]
		public IDesignConnection Connection
		{
			get
			{
				return this.connection;
			}
			set
			{
				this.connection = value;
			}
		}

		[DefaultValue(TypeEnum.CLR)]
		[DataSourceXmlAttribute]
		public TypeEnum MethodsParameterType
		{
			get
			{
				return this.parameterType;
			}
			set
			{
				this.parameterType = value;
			}
		}

		public CommandOperation CommandOperation
		{
			get
			{
				if (this.SelectCommand != null)
				{
					return CommandOperation.Select;
				}
				if (this.InsertCommand != null)
				{
					return CommandOperation.Insert;
				}
				if (this.UpdateCommand != null)
				{
					return CommandOperation.Update;
				}
				if (this.DeleteCommand != null)
				{
					return CommandOperation.Delete;
				}
				return CommandOperation.Unknown;
			}
		}

		[DefaultValue(MemberAttributes.Public)]
		[DataSourceXmlAttribute]
		public MemberAttributes FillMethodModifier
		{
			get
			{
				return base.Modifier;
			}
			set
			{
				base.Modifier = value;
			}
		}

		[DefaultValue(MemberAttributes.Public)]
		[DataSourceXmlAttribute]
		public MemberAttributes GetMethodModifier
		{
			get
			{
				return this.getMethodModifier;
			}
			set
			{
				this.getMethodModifier = value;
			}
		}

		[Browsable(false)]
		public override string Name
		{
			get
			{
				if (StringUtil.Empty(base.Name) && this.generateMethods == GenerateMethodTypes.Get)
				{
					return this.GetMethodName;
				}
				return base.Name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
					SourceCollection sourceCollection = this.CollectionParent as SourceCollection;
					if (sourceCollection != null)
					{
						sourceCollection.ValidateUniqueDbSourceName(this, value, true);
					}
				}
			}
		}

		[DefaultValue("Fill")]
		[DataSourceXmlAttribute]
		public string FillMethodName
		{
			get
			{
				return this.Name;
			}
			set
			{
				this.Name = value;
			}
		}

		[DataSourceXmlAttribute]
		[DefaultValue("GetData")]
		public string GetMethodName
		{
			get
			{
				if (StringUtil.EmptyOrSpace(this.getMethodName) && this.CollectionParent != null)
				{
					if (base.IsMainSource)
					{
						this.GetMethodName = "GetData";
					}
					else
					{
						this.GetMethodName = "GetDataBy";
					}
				}
				return this.getMethodName;
			}
			set
			{
				this.getMethodName = value;
			}
		}

		[DataSourceXmlAttribute]
		public string UserGetMethodName
		{
			get
			{
				return this.userGetMethodName;
			}
			set
			{
				this.userGetMethodName = value;
			}
		}

		[RefreshProperties(RefreshProperties.All)]
		[DataSourceXmlAttribute]
		[DefaultValue(GenerateMethodTypes.Both)]
		public GenerateMethodTypes GenerateMethods
		{
			get
			{
				return this.generateMethods;
			}
			set
			{
				this.generateMethods = value;
			}
		}

		[DataSourceXmlAttribute]
		[DefaultValue(true)]
		public bool GeneratePagingMethods
		{
			get
			{
				return this.generatePagingMethods;
			}
			set
			{
				this.generatePagingMethods = value;
			}
		}

		[Browsable(false)]
		public override object Parent
		{
			get
			{
				if (base.Parent != null)
				{
					return base.Parent;
				}
				return base.Owner;
			}
		}

		[Browsable(false)]
		public override string PublicTypeName
		{
			get
			{
				if (base.Owner is DesignTable)
				{
					return "Query";
				}
				return "Query";
			}
		}

		[DataSourceXmlAttribute]
		[Browsable(false)]
		public QueryType QueryType
		{
			get
			{
				return this.queryType;
			}
			set
			{
				this.queryType = value;
				if (this.queryType != QueryType.Rowset)
				{
					this.GenerateMethods = GenerateMethodTypes.Fill;
				}
			}
		}

		[DataSourceXmlSubItem(Name = "SelectCommand", ItemType = typeof(DbSourceCommand))]
		[Browsable(false)]
		public DbSourceCommand SelectCommand
		{
			get
			{
				return this.selectCommand;
			}
			set
			{
				if (this.selectCommand != null)
				{
					this.selectCommand.SetParent(null);
				}
				this.selectCommand = value;
				if (this.selectCommand != null)
				{
					this.selectCommand.SetParent(this);
					this.selectCommand.CommandOperation = CommandOperation.Select;
				}
			}
		}

		[Browsable(false)]
		[DataSourceXmlSubItem(Name = "UpdateCommand", ItemType = typeof(DbSourceCommand))]
		public DbSourceCommand UpdateCommand
		{
			get
			{
				return this.updateCommand;
			}
			set
			{
				if (this.updateCommand != null)
				{
					this.updateCommand.SetParent(null);
				}
				this.updateCommand = value;
				if (this.updateCommand != null)
				{
					this.updateCommand.SetParent(this);
					this.updateCommand.CommandOperation = CommandOperation.Update;
				}
			}
		}

		[Browsable(false)]
		[DataSourceXmlSubItem(Name = "DeleteCommand", ItemType = typeof(DbSourceCommand))]
		public DbSourceCommand DeleteCommand
		{
			get
			{
				return this.deleteCommand;
			}
			set
			{
				if (this.deleteCommand != null)
				{
					this.deleteCommand.SetParent(null);
				}
				this.deleteCommand = value;
				if (this.deleteCommand != null)
				{
					this.deleteCommand.SetParent(this);
					this.deleteCommand.CommandOperation = CommandOperation.Delete;
				}
			}
		}

		[Browsable(false)]
		[DataSourceXmlSubItem(Name = "InsertCommand", ItemType = typeof(DbSourceCommand))]
		public DbSourceCommand InsertCommand
		{
			get
			{
				return this.insertCommand;
			}
			set
			{
				if (this.insertCommand != null)
				{
					this.insertCommand.SetParent(null);
				}
				this.insertCommand = value;
				if (this.insertCommand != null)
				{
					this.insertCommand.SetParent(this);
					this.insertCommand.CommandOperation = CommandOperation.Insert;
				}
			}
		}

		[DataSourceXmlAttribute]
		public DbObjectType DbObjectType
		{
			get
			{
				return this.dbObjectType;
			}
			set
			{
				this.dbObjectType = value;
			}
		}

		[DataSourceXmlAttribute]
		public bool UseOptimisticConcurrency
		{
			get
			{
				return this.useOptimisticConcurrency;
			}
			set
			{
				this.useOptimisticConcurrency = value;
			}
		}

		internal override bool NameExist(string nameToCheck)
		{
			return StringUtil.EqualValue(this.FillMethodName, nameToCheck, true) || StringUtil.EqualValue(this.GetMethodName, nameToCheck, true);
		}

		public override object Clone()
		{
			DbSource dbSource = new DbSource();
			if (this.connection != null)
			{
				dbSource.connection = (DesignConnection)this.connection.Clone();
			}
			if (this.selectCommand != null)
			{
				dbSource.selectCommand = (DbSourceCommand)this.selectCommand.Clone();
				dbSource.selectCommand.SetParent(dbSource);
			}
			if (this.insertCommand != null)
			{
				dbSource.insertCommand = (DbSourceCommand)this.insertCommand.Clone();
				dbSource.insertCommand.SetParent(dbSource);
			}
			if (this.updateCommand != null)
			{
				dbSource.updateCommand = (DbSourceCommand)this.updateCommand.Clone();
				dbSource.updateCommand.SetParent(dbSource);
			}
			if (this.deleteCommand != null)
			{
				dbSource.deleteCommand = (DbSourceCommand)this.deleteCommand.Clone();
				dbSource.deleteCommand.SetParent(dbSource);
			}
			dbSource.Name = this.Name;
			dbSource.Modifier = base.Modifier;
			dbSource.scalarCallRetval = this.scalarCallRetval;
			dbSource.generateMethods = this.generateMethods;
			dbSource.queryType = this.queryType;
			dbSource.getMethodModifier = this.getMethodModifier;
			dbSource.getMethodName = this.getMethodName;
			dbSource.generatePagingMethods = this.generatePagingMethods;
			return dbSource;
		}

		[DataSourceXmlAttribute]
		public bool GenerateShortCommands
		{
			get
			{
				return this.generateShortCommands;
			}
			set
			{
				this.generateShortCommands = value;
			}
		}

		internal DbSourceCommand GetActiveCommand()
		{
			switch (this.CommandOperation)
			{
			case CommandOperation.Select:
				return this.SelectCommand;
			case CommandOperation.Insert:
				return this.InsertCommand;
			case CommandOperation.Update:
				return this.UpdateCommand;
			case CommandOperation.Delete:
				return this.DeleteCommand;
			default:
				return null;
			}
		}

		void IDataSourceXmlSpecialOwner.ReadSpecialItem(string propertyName, XmlNode xmlNode, DataSourceXmlSerializer serializer)
		{
			if (propertyName.Equals("ScalarCallRetval"))
			{
				this.scalarCallRetval = typeof(object);
				if (StringUtil.NotEmptyAfterTrim(xmlNode.InnerText))
				{
					this.scalarCallRetval = Type.GetType(xmlNode.InnerText, false);
				}
			}
		}

		void IDataSourceXmlSpecialOwner.WriteSpecialItem(string propertyName, XmlWriter writer, DataSourceXmlSerializer serializer)
		{
			if (propertyName.Equals("ScalarCallRetval"))
			{
				writer.WriteString(this.scalarCallRetval.AssemblyQualifiedName);
			}
		}

		internal const string TYPE_NAME_FOR_QUERY = "Query";

		internal const string TYPE_NAME_FOR_FUNCTION = "Query";

		private const string PROPERTY_COMMANDTEXT = "CommandText";

		internal const string INSTANCE_NAME_FOR_FILLMETHOD_MAIN = "Fill";

		internal const string INSTANCE_NAME_FOR_GETMETHOD_MAIN = "GetData";

		internal const string INSTANCE_NAME_FOR_FILLMETHOD = "FillBy";

		internal const string INSTANCE_NAME_FOR_GETMETHOD = "GetDataBy";

		internal const string INSTANCE_NAME_FOR_FUNCTION = "Query";

		private IDesignConnection connection;

		private DbSourceCommand selectCommand;

		private DbSourceCommand insertCommand;

		private DbSourceCommand updateCommand;

		private DbSourceCommand deleteCommand;

		private DbObjectType dbObjectType;

		private string connectionRef;

		private Type scalarCallRetval = typeof(object);

		private string userGetMethodName;

		private string getMethodName;

		private MemberAttributes getMethodModifier = MemberAttributes.Public;

		private QueryType queryType;

		private GenerateMethodTypes generateMethods = GenerateMethodTypes.Both;

		private bool generatePagingMethods;

		private bool generateShortCommands = true;

		private bool useOptimisticConcurrency = true;

		private TypeEnum parameterType;
	}
}

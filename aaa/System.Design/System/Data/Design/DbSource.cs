using System;
using System.CodeDom;
using System.ComponentModel;
using System.Xml;

namespace System.Data.Design
{
	// Token: 0x0200008B RID: 139
	[DataSourceXmlClass("DbSource")]
	internal class DbSource : Source, IDataSourceXmlSpecialOwner
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x0000AB2C File Offset: 0x00009B2C
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

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0000AB82 File Offset: 0x00009B82
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x0000AB9E File Offset: 0x00009B9E
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

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0000ABA7 File Offset: 0x00009BA7
		[Browsable(false)]
		[DataSourceXmlAttribute(SpecialWay = true)]
		public Type ScalarCallRetval
		{
			get
			{
				return this.scalarCallRetval;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0000ABAF File Offset: 0x00009BAF
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x0000ABB7 File Offset: 0x00009BB7
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0000ABC0 File Offset: 0x00009BC0
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x0000ABC8 File Offset: 0x00009BC8
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0000ABD1 File Offset: 0x00009BD1
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x0000ABFC File Offset: 0x00009BFC
		// (set) Token: 0x0600058E RID: 1422 RVA: 0x0000AC04 File Offset: 0x00009C04
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x0000AC0D File Offset: 0x00009C0D
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x0000AC15 File Offset: 0x00009C15
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0000AC1E File Offset: 0x00009C1E
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x0000AC44 File Offset: 0x00009C44
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

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x0000AC7E File Offset: 0x00009C7E
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x0000AC86 File Offset: 0x00009C86
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x0000AC8F File Offset: 0x00009C8F
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x0000ACCC File Offset: 0x00009CCC
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x0000ACD5 File Offset: 0x00009CD5
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x0000ACDD File Offset: 0x00009CDD
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x0000ACE6 File Offset: 0x00009CE6
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x0000ACEE File Offset: 0x00009CEE
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

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x0000ACF7 File Offset: 0x00009CF7
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x0000ACFF File Offset: 0x00009CFF
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

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x0000AD08 File Offset: 0x00009D08
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

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x0000AD1F File Offset: 0x00009D1F
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

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0000AD39 File Offset: 0x00009D39
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x0000AD41 File Offset: 0x00009D41
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

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0000AD59 File Offset: 0x00009D59
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x0000AD61 File Offset: 0x00009D61
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

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x0000AD9E File Offset: 0x00009D9E
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x0000ADA6 File Offset: 0x00009DA6
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

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0000ADE3 File Offset: 0x00009DE3
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x0000ADEB File Offset: 0x00009DEB
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

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0000AE28 File Offset: 0x00009E28
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x0000AE30 File Offset: 0x00009E30
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

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x0000AE6D File Offset: 0x00009E6D
		// (set) Token: 0x060005AA RID: 1450 RVA: 0x0000AE75 File Offset: 0x00009E75
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

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x0000AE7E File Offset: 0x00009E7E
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x0000AE86 File Offset: 0x00009E86
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

		// Token: 0x060005AD RID: 1453 RVA: 0x0000AE8F File Offset: 0x00009E8F
		internal override bool NameExist(string nameToCheck)
		{
			return StringUtil.EqualValue(this.FillMethodName, nameToCheck, true) || StringUtil.EqualValue(this.GetMethodName, nameToCheck, true);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0000AEB0 File Offset: 0x00009EB0
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x0000AFEA File Offset: 0x00009FEA
		// (set) Token: 0x060005B0 RID: 1456 RVA: 0x0000AFF2 File Offset: 0x00009FF2
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

		// Token: 0x060005B1 RID: 1457 RVA: 0x0000AFFC File Offset: 0x00009FFC
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

		// Token: 0x060005B2 RID: 1458 RVA: 0x0000B049 File Offset: 0x0000A049
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

		// Token: 0x060005B3 RID: 1459 RVA: 0x0000B087 File Offset: 0x0000A087
		void IDataSourceXmlSpecialOwner.WriteSpecialItem(string propertyName, XmlWriter writer, DataSourceXmlSerializer serializer)
		{
			if (propertyName.Equals("ScalarCallRetval"))
			{
				writer.WriteString(this.scalarCallRetval.AssemblyQualifiedName);
			}
		}

		// Token: 0x04000AEF RID: 2799
		internal const string TYPE_NAME_FOR_QUERY = "Query";

		// Token: 0x04000AF0 RID: 2800
		internal const string TYPE_NAME_FOR_FUNCTION = "Query";

		// Token: 0x04000AF1 RID: 2801
		private const string PROPERTY_COMMANDTEXT = "CommandText";

		// Token: 0x04000AF2 RID: 2802
		internal const string INSTANCE_NAME_FOR_FILLMETHOD_MAIN = "Fill";

		// Token: 0x04000AF3 RID: 2803
		internal const string INSTANCE_NAME_FOR_GETMETHOD_MAIN = "GetData";

		// Token: 0x04000AF4 RID: 2804
		internal const string INSTANCE_NAME_FOR_FILLMETHOD = "FillBy";

		// Token: 0x04000AF5 RID: 2805
		internal const string INSTANCE_NAME_FOR_GETMETHOD = "GetDataBy";

		// Token: 0x04000AF6 RID: 2806
		internal const string INSTANCE_NAME_FOR_FUNCTION = "Query";

		// Token: 0x04000AF7 RID: 2807
		private IDesignConnection connection;

		// Token: 0x04000AF8 RID: 2808
		private DbSourceCommand selectCommand;

		// Token: 0x04000AF9 RID: 2809
		private DbSourceCommand insertCommand;

		// Token: 0x04000AFA RID: 2810
		private DbSourceCommand updateCommand;

		// Token: 0x04000AFB RID: 2811
		private DbSourceCommand deleteCommand;

		// Token: 0x04000AFC RID: 2812
		private DbObjectType dbObjectType;

		// Token: 0x04000AFD RID: 2813
		private string connectionRef;

		// Token: 0x04000AFE RID: 2814
		private Type scalarCallRetval = typeof(object);

		// Token: 0x04000AFF RID: 2815
		private string userGetMethodName;

		// Token: 0x04000B00 RID: 2816
		private string getMethodName;

		// Token: 0x04000B01 RID: 2817
		private MemberAttributes getMethodModifier = MemberAttributes.Public;

		// Token: 0x04000B02 RID: 2818
		private QueryType queryType;

		// Token: 0x04000B03 RID: 2819
		private GenerateMethodTypes generateMethods = GenerateMethodTypes.Both;

		// Token: 0x04000B04 RID: 2820
		private bool generatePagingMethods;

		// Token: 0x04000B05 RID: 2821
		private bool generateShortCommands = true;

		// Token: 0x04000B06 RID: 2822
		private bool useOptimisticConcurrency = true;

		// Token: 0x04000B07 RID: 2823
		private TypeEnum parameterType;
	}
}

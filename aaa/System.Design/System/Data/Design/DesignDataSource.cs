using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

namespace System.Data.Design
{
	// Token: 0x02000096 RID: 150
	[DataSourceXmlClass("DataSource")]
	internal class DesignDataSource : DataSourceComponent, IDataSourceNamedObject, INamedObject, IDataSourceCommandTarget
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0000C084 File Offset: 0x0000B084
		internal DataSet DataSet
		{
			get
			{
				if (this.dataSet == null)
				{
					this.dataSet = new DataSet();
					this.dataSet.Locale = CultureInfo.InvariantCulture;
					this.dataSet.EnforceConstraints = false;
				}
				return this.dataSet;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x0000C0BC File Offset: 0x0000B0BC
		[DisplayName("DefaultConnection")]
		public DesignConnection DefaultConnection
		{
			get
			{
				if (this.DesignConnections.Count > 0 && this.defaultConnectionIndex >= 0 && this.defaultConnectionIndex < this.DesignConnections.Count)
				{
					return ((IList)this.DesignConnections)[this.defaultConnectionIndex] as DesignConnection;
				}
				return null;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x0000C10B File Offset: 0x0000B10B
		[Browsable(false)]
		[DisplayName("Connections")]
		[DataSourceXmlSubItem(Name = "Connections", ItemType = typeof(DesignConnection))]
		public DesignConnectionCollection DesignConnections
		{
			get
			{
				if (this.designConnections == null)
				{
					this.designConnections = new DesignConnectionCollection(this);
				}
				return this.designConnections;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x0000C127 File Offset: 0x0000B127
		[Browsable(false)]
		public DesignRelationCollection DesignRelations
		{
			get
			{
				if (this.designRelations == null)
				{
					this.designRelations = new DesignRelationCollection(this);
				}
				return this.designRelations;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x0000C143 File Offset: 0x0000B143
		[DataSourceXmlSubItem(Name = "Tables", ItemType = typeof(DesignConnection))]
		[Browsable(false)]
		public DesignTableCollection DesignTables
		{
			get
			{
				if (this.designTables == null)
				{
					this.designTables = new DesignTableCollection(this);
				}
				return this.designTables;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x0000C160 File Offset: 0x0000B160
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x0000C192 File Offset: 0x0000B192
		[DefaultValue(true)]
		public bool EnableTableAdapterManager
		{
			get
			{
				bool flag = false;
				bool.TryParse(this.DataSet.ExtendedProperties["EnableTableAdapterManager"] as string, out flag);
				return flag;
			}
			set
			{
				this.DataSet.ExtendedProperties["EnableTableAdapterManager"] = value.ToString();
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x0000C1B0 File Offset: 0x0000B1B0
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x0000C1B8 File Offset: 0x0000B1B8
		[DataSourceXmlAttribute]
		[DefaultValue(TypeAttributes.Public)]
		public TypeAttributes Modifier
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

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x0000C1C1 File Offset: 0x0000B1C1
		// (set) Token: 0x0600064E RID: 1614 RVA: 0x0000C1CE File Offset: 0x0000B1CE
		[MergableProperty(false)]
		[DefaultValue("")]
		public string Name
		{
			get
			{
				return this.DataSet.DataSetName;
			}
			set
			{
				this.DataSet.DataSetName = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x0000C1DC File Offset: 0x0000B1DC
		[Browsable(false)]
		public string PublicTypeName
		{
			get
			{
				return "DataSet";
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x0000C1E3 File Offset: 0x0000B1E3
		[DataSourceXmlSubItem(typeof(Source))]
		[Browsable(false)]
		public SourceCollection Sources
		{
			get
			{
				if (this.sources == null)
				{
					this.sources = new SourceCollection(this);
				}
				return this.sources;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x0000C1FF File Offset: 0x0000B1FF
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x0000C207 File Offset: 0x0000B207
		[DataSourceXmlAttribute]
		public SchemaSerializationMode SchemaSerializationMode
		{
			get
			{
				return this.schemaSerializationMode;
			}
			set
			{
				this.schemaSerializationMode = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x0000C210 File Offset: 0x0000B210
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x0000C22C File Offset: 0x0000B22C
		internal string UserDataSetName
		{
			get
			{
				return this.DataSet.ExtendedProperties[DesignDataSource.EXTPROPNAME_USER_DATASETNAME] as string;
			}
			set
			{
				this.DataSet.ExtendedProperties[DesignDataSource.EXTPROPNAME_USER_DATASETNAME] = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x0000C244 File Offset: 0x0000B244
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x0000C260 File Offset: 0x0000B260
		internal string GeneratorDataSetName
		{
			get
			{
				return this.DataSet.ExtendedProperties[DesignDataSource.EXTPROPNAME_GENERATOR_DATASETNAME] as string;
			}
			set
			{
				this.DataSet.ExtendedProperties[DesignDataSource.EXTPROPNAME_GENERATOR_DATASETNAME] = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x0000C278 File Offset: 0x0000B278
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x0000C280 File Offset: 0x0000B280
		[DataSourceXmlAttribute]
		[Browsable(false)]
		[DefaultValue(null)]
		public string FunctionsComponentName
		{
			get
			{
				return this.functionsComponentName;
			}
			set
			{
				this.functionsComponentName = value;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x0000C289 File Offset: 0x0000B289
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x0000C291 File Offset: 0x0000B291
		[DataSourceXmlAttribute]
		[DefaultValue(null)]
		[Browsable(false)]
		public string UserFunctionsComponentName
		{
			get
			{
				return this.userFunctionsComponentName;
			}
			set
			{
				this.userFunctionsComponentName = value;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x0000C29A File Offset: 0x0000B29A
		// (set) Token: 0x0600065C RID: 1628 RVA: 0x0000C2A2 File Offset: 0x0000B2A2
		[DataSourceXmlAttribute]
		[Browsable(false)]
		[DefaultValue(null)]
		public string GeneratorFunctionsComponentClassName
		{
			get
			{
				return this.generatorFunctionsComponentClassName;
			}
			set
			{
				this.generatorFunctionsComponentClassName = value;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0000C2AB File Offset: 0x0000B2AB
		internal override StringCollection NamingPropertyNames
		{
			get
			{
				return this.namingPropNames;
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0000C2B4 File Offset: 0x0000B2B4
		void IDataSourceCommandTarget.AddChild(object child, bool fixName)
		{
			Type type = child.GetType();
			if (typeof(DesignTable).IsAssignableFrom(type))
			{
				this.DesignTables.Add((DesignTable)child);
				return;
			}
			if (typeof(DesignRelation).IsAssignableFrom(type))
			{
				this.DesignRelations.Add((DesignRelation)child);
				return;
			}
			if (typeof(IDesignConnection).IsAssignableFrom(type))
			{
				this.DesignConnections.Add((IDesignConnection)child);
				return;
			}
			if (typeof(Source).IsAssignableFrom(type))
			{
				this.Sources.Add((Source)child);
			}
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0000C35C File Offset: 0x0000B35C
		bool IDataSourceCommandTarget.CanAddChildOfType(Type childType)
		{
			return typeof(DesignTable).IsAssignableFrom(childType) || typeof(IDesignConnection).IsAssignableFrom(childType) || typeof(Source).IsAssignableFrom(childType) || (typeof(DesignRelation).IsAssignableFrom(childType) && ((ICollection)this.DesignTables).Count > 0);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0000C3C4 File Offset: 0x0000B3C4
		bool IDataSourceCommandTarget.CanInsertChildOfType(Type childType, object refChild)
		{
			if (typeof(Source).IsAssignableFrom(childType))
			{
				return refChild is Source;
			}
			if (typeof(IDesignConnection).IsAssignableFrom(childType))
			{
				return refChild is IDesignConnection;
			}
			return typeof(DesignTable).IsAssignableFrom(childType);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0000C420 File Offset: 0x0000B420
		bool IDataSourceCommandTarget.CanRemoveChildren(ICollection children)
		{
			foreach (object obj in children)
			{
				if (!this.CanRemoveChild(obj))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0000C478 File Offset: 0x0000B478
		private bool CanRemoveChild(object child)
		{
			bool flag = false;
			Type type = child.GetType();
			if (typeof(DesignTable).IsAssignableFrom(type))
			{
				flag = this.DesignTables.Contains((DesignTable)child);
			}
			else if (typeof(DesignRelation).IsAssignableFrom(type))
			{
				flag = this.DesignRelations.Contains((DesignRelation)child);
			}
			else if (typeof(IDesignConnection).IsAssignableFrom(type))
			{
				flag = this.DesignConnections.Contains((IDesignConnection)child);
			}
			else if (typeof(Source).IsAssignableFrom(type))
			{
				flag = this.Sources.Contains((Source)child);
			}
			return flag;
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0000C528 File Offset: 0x0000B528
		internal ArrayList GetRelatedRelations(ICollection tableList)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.DesignRelations)
			{
				DesignRelation designRelation = (DesignRelation)obj;
				DesignTable parentDesignTable = designRelation.ParentDesignTable;
				DesignTable childDesignTable = designRelation.ChildDesignTable;
				foreach (object obj2 in tableList)
				{
					if (parentDesignTable == obj2 || childDesignTable == obj2)
					{
						arrayList.Add(designRelation);
						break;
					}
				}
			}
			return arrayList;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0000C5EC File Offset: 0x0000B5EC
		void IDataSourceCommandTarget.InsertChild(object child, object refChild)
		{
			if (child is DesignTable)
			{
				this.DesignTables.InsertBefore(child, refChild);
				return;
			}
			if (child is DesignRelation)
			{
				this.DesignRelations.InsertBefore(child, refChild);
				return;
			}
			if (child is Source)
			{
				this.Sources.InsertBefore(child, refChild);
				return;
			}
			if (child is IDesignConnection)
			{
				this.DesignConnections.InsertBefore(child, refChild);
			}
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0000C650 File Offset: 0x0000B650
		object IDataSourceCommandTarget.GetObject(int index, bool getSiblingIfOutOfRange)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0000C657 File Offset: 0x0000B657
		int IDataSourceCommandTarget.IndexOf(object child)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0000C660 File Offset: 0x0000B660
		public void ReadXmlSchema(Stream stream)
		{
			DataSourceXmlTextReader dataSourceXmlTextReader = new DataSourceXmlTextReader(this, stream);
			this.ReadXmlSchema(dataSourceXmlTextReader);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0000C67C File Offset: 0x0000B67C
		public void ReadXmlSchema(TextReader textReader)
		{
			DataSourceXmlTextReader dataSourceXmlTextReader = new DataSourceXmlTextReader(this, textReader);
			this.ReadXmlSchema(dataSourceXmlTextReader);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0000C698 File Offset: 0x0000B698
		private void ReadXmlSchema(DataSourceXmlTextReader xmlReader)
		{
			this.designConnections = new DesignConnectionCollection(this);
			this.designTables = new DesignTableCollection(this);
			this.designRelations = new DesignRelationCollection(this);
			this.sources = new SourceCollection(this);
			this.serializer = new DataSourceXmlSerializer();
			this.dataSet = new DataSet();
			this.dataSet.Locale = CultureInfo.InvariantCulture;
			DataSet dataSet = new DataSet();
			dataSet.Locale = CultureInfo.InvariantCulture;
			dataSet.ReadXmlSchema(xmlReader);
			this.dataSet = dataSet;
			foreach (object obj in this.dataSet.Tables)
			{
				DataTable dataTable = (DataTable)obj;
				DesignTable designTable = this.designTables[dataTable.TableName];
				if (designTable == null)
				{
					this.designTables.Add(new DesignTable(dataTable, TableType.DataTable));
				}
				else
				{
					designTable.DataTable = dataTable;
				}
				foreach (object obj2 in dataTable.Constraints)
				{
					Constraint constraint = (Constraint)obj2;
					ForeignKeyConstraint foreignKeyConstraint = constraint as ForeignKeyConstraint;
					if (foreignKeyConstraint != null)
					{
						this.designRelations.Add(new DesignRelation(foreignKeyConstraint));
					}
				}
			}
			foreach (object obj3 in this.dataSet.Relations)
			{
				DataRelation dataRelation = (DataRelation)obj3;
				DesignRelation designRelation = this.designRelations[dataRelation.ChildKeyConstraint];
				if (designRelation != null)
				{
					designRelation.DataRelation = dataRelation;
				}
				else
				{
					this.designRelations.Add(new DesignRelation(dataRelation));
				}
			}
			foreach (object obj4 in this.Sources)
			{
				Source source = (Source)obj4;
				this.SetConnectionProperty(source);
			}
			foreach (object obj5 in this.DesignTables)
			{
				DesignTable designTable2 = (DesignTable)obj5;
				this.SetConnectionProperty(designTable2.MainSource);
				foreach (object obj6 in designTable2.Sources)
				{
					Source source2 = (Source)obj6;
					this.SetConnectionProperty(source2);
				}
			}
			this.serializer.InitializeObjects();
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0000C990 File Offset: 0x0000B990
		private void SetConnectionProperty(Source source)
		{
			DbSource dbSource = source as DbSource;
			if (dbSource == null)
			{
				return;
			}
			string connectionRef = dbSource.ConnectionRef;
			if (connectionRef != null)
			{
				if (connectionRef.Length == 0)
				{
					return;
				}
				IDesignConnection designConnection = this.DesignConnections.Get(connectionRef);
				if (designConnection == null)
				{
					return;
				}
				dbSource.Connection = designConnection;
			}
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0000C9D4 File Offset: 0x0000B9D4
		internal void ReadDataSourceExtraInformation(XmlTextReader xmlTextReader)
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlNode xmlNode = xmlDocument.ReadNode(xmlTextReader);
			xmlDocument.AppendChild(xmlNode);
			if (this.serializer != null)
			{
				this.serializer.DeserializeBody((XmlElement)xmlNode, this);
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0000CA14 File Offset: 0x0000BA14
		void IDataSourceCommandTarget.RemoveChildren(ICollection children)
		{
			SortedList sortedList = new SortedList();
			foreach (object obj in children)
			{
				if (obj is DesignTable)
				{
					sortedList.Add(-this.DesignTables.IndexOf((DesignTable)obj), obj);
				}
				else
				{
					this.RemoveChild(obj);
				}
			}
			ArrayList relatedRelations = this.GetRelatedRelations(children);
			foreach (object obj2 in relatedRelations)
			{
				DesignRelation designRelation = (DesignRelation)obj2;
				this.RemoveChild(designRelation);
			}
			foreach (object obj3 in sortedList.Values)
			{
				if (obj3 is DesignTable)
				{
					this.RemoveChild(obj3);
				}
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0000CB40 File Offset: 0x0000BB40
		private void RemoveChild(object child)
		{
			Type type = child.GetType();
			if (typeof(DesignTable).IsAssignableFrom(type))
			{
				this.DesignTables.Remove((DesignTable)child);
				return;
			}
			if (typeof(DesignRelation).IsAssignableFrom(type))
			{
				this.DesignRelations.Remove((DesignRelation)child);
				return;
			}
			if (typeof(IDesignConnection).IsAssignableFrom(type))
			{
				this.DesignConnections.Remove((IDesignConnection)child);
				return;
			}
			if (typeof(Source).IsAssignableFrom(type))
			{
				this.Sources.Remove((Source)child);
			}
		}

		// Token: 0x04000B2A RID: 2858
		private const string EXTPROPNAME_ENABLE_TABLEADAPTERMANAGER = "EnableTableAdapterManager";

		// Token: 0x04000B2B RID: 2859
		private DataSet dataSet;

		// Token: 0x04000B2C RID: 2860
		private DesignTableCollection designTables;

		// Token: 0x04000B2D RID: 2861
		private DesignRelationCollection designRelations;

		// Token: 0x04000B2E RID: 2862
		private DesignConnectionCollection designConnections;

		// Token: 0x04000B2F RID: 2863
		private int defaultConnectionIndex;

		// Token: 0x04000B30 RID: 2864
		private SourceCollection sources;

		// Token: 0x04000B31 RID: 2865
		private TypeAttributes modifier = TypeAttributes.Public;

		// Token: 0x04000B32 RID: 2866
		private SchemaSerializationMode schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

		// Token: 0x04000B33 RID: 2867
		private DataSourceXmlSerializer serializer;

		// Token: 0x04000B34 RID: 2868
		private StringCollection namingPropNames = new StringCollection();

		// Token: 0x04000B35 RID: 2869
		internal static string EXTPROPNAME_USER_DATASETNAME = "Generator_UserDSName";

		// Token: 0x04000B36 RID: 2870
		internal static string EXTPROPNAME_GENERATOR_DATASETNAME = "Generator_DataSetName";

		// Token: 0x04000B37 RID: 2871
		private string functionsComponentName;

		// Token: 0x04000B38 RID: 2872
		private string userFunctionsComponentName;

		// Token: 0x04000B39 RID: 2873
		private string generatorFunctionsComponentClassName;
	}
}

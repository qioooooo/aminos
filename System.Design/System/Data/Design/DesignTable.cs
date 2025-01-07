using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Design;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace System.Data.Design
{
	internal class DesignTable : DataSourceComponent, IDataSourceNamedObject, INamedObject, IDataSourceXmlSerializable, IDataSourceXmlSpecialOwner, IDataSourceInitAfterLoading, IDataSourceCommandTarget
	{
		private event EventHandler tableTypeChanged;

		private event EventHandler constraintsChanged;

		private event EventHandler dataAccessorChanged;

		private event EventHandler dataAccessorChanging;

		public DesignTable()
			: this(null, TableType.DataTable)
		{
		}

		public DesignTable(DataTable dataTable)
			: this(dataTable, TableType.DataTable)
		{
		}

		public DesignTable(DataTable dataTable, TableType tableType)
		{
			if (dataTable == null)
			{
				this.dataTable = new DataTable();
				this.dataTable.Locale = CultureInfo.InvariantCulture;
			}
			else
			{
				this.dataTable = dataTable;
			}
			this.TableType = tableType;
			this.AddRemoveConstraintMonitor(true);
			this.namingPropNames.AddRange(new string[] { "typedPlural", "typedName" });
		}

		public DesignTable(DataTable dataTable, TableType tableType, DataColumnMappingCollection mappings)
			: this(dataTable, tableType)
		{
			this.mappings = mappings;
		}

		[Browsable(false)]
		[DataSourceXmlAttribute]
		public string BaseClass
		{
			get
			{
				if (StringUtil.NotEmptyAfterTrim(this.baseClass))
				{
					return this.baseClass;
				}
				return "System.ComponentModel.Component";
			}
			set
			{
				this.baseClass = value;
			}
		}

		public IDesignConnection Connection
		{
			get
			{
				if (this.TableType == TableType.RadTable)
				{
					DbSource dbSource = this.EnsureDbSource();
					return dbSource.Connection;
				}
				return null;
			}
			set
			{
				if (this.TableType == TableType.RadTable)
				{
					DbSource dbSource = this.EnsureDbSource();
					dbSource.Connection = value;
				}
			}
		}

		internal event EventHandler ConstraintChanged
		{
			add
			{
				this.constraintsChanged = (EventHandler)Delegate.Combine(this.constraintsChanged, value);
			}
			remove
			{
				this.constraintsChanged = (EventHandler)Delegate.Remove(this.constraintsChanged, value);
			}
		}

		internal DataAccessor DataAccessor
		{
			get
			{
				return this.dataAccessor;
			}
			set
			{
				if (this.dataAccessorChanging != null)
				{
					this.dataAccessorChanging(this, new EventArgs());
				}
				this.dataAccessor = value;
				if (this.dataAccessorChanged != null)
				{
					this.dataAccessorChanged(this, new EventArgs());
				}
			}
		}

		internal event EventHandler DataAccessorChanged
		{
			add
			{
				this.dataAccessorChanged = (EventHandler)Delegate.Combine(this.dataAccessorChanged, value);
			}
			remove
			{
				this.dataAccessorChanged = (EventHandler)Delegate.Remove(this.dataAccessorChanged, value);
			}
		}

		internal event EventHandler DataAccessorChanging
		{
			add
			{
				this.dataAccessorChanging = (EventHandler)Delegate.Combine(this.dataAccessorChanging, value);
			}
			remove
			{
				this.dataAccessorChanging = (EventHandler)Delegate.Remove(this.dataAccessorChanging, value);
			}
		}

		[DataSourceXmlAttribute]
		[Browsable(false)]
		public string DataAccessorName
		{
			get
			{
				if (StringUtil.NotEmptyAfterTrim(this.dataAccessorName))
				{
					return this.dataAccessorName;
				}
				return this.Name + "TableAdapter";
			}
			set
			{
				this.dataAccessorName = value;
			}
		}

		[Browsable(false)]
		public DataTable DataTable
		{
			get
			{
				return this.dataTable;
			}
			set
			{
				if (this.dataTable != value)
				{
					if (this.dataTable != null)
					{
						this.AddRemoveConstraintMonitor(false);
					}
					this.dataTable = value;
					if (this.dataTable != null)
					{
						this.AddRemoveConstraintMonitor(true);
					}
				}
			}
		}

		[DefaultValue(null)]
		public DbSourceCommand DeleteCommand
		{
			get
			{
				DbSource dbSource = this.EnsureDbSource();
				return dbSource.DeleteCommand;
			}
			set
			{
				DbSource dbSource = this.EnsureDbSource();
				dbSource.DeleteCommand = value;
			}
		}

		[Browsable(false)]
		public DesignColumnCollection DesignColumns
		{
			get
			{
				if (this.designColumns == null)
				{
					this.designColumns = new DesignColumnCollection(this);
				}
				return this.designColumns;
			}
		}

		protected override object ExternalPropertyHost
		{
			get
			{
				return this.dataTable;
			}
		}

		internal bool HasAnyUpdateCommand
		{
			get
			{
				return this.TableType == TableType.RadTable && this.MainSource != null && this.MainSource is DbSource && ((DbSource)this.MainSource).CommandOperation == CommandOperation.Select && (this.DeleteCommand != null || this.InsertCommand != null || this.UpdateCommand != null);
			}
		}

		internal bool HasAnyExpressionColumn
		{
			get
			{
				DataTable dataTable = this.DataTable;
				foreach (object obj in dataTable.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj;
					if (dataColumn.Expression != null && dataColumn.Expression.Length > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		[DefaultValue(null)]
		public DbSourceCommand InsertCommand
		{
			get
			{
				DbSource dbSource = this.EnsureDbSource();
				return dbSource.InsertCommand;
			}
			set
			{
				DbSource dbSource = this.EnsureDbSource();
				dbSource.InsertCommand = value;
			}
		}

		[Browsable(false)]
		[DataSourceXmlSubItem(Name = "MainSource", ItemType = typeof(Source))]
		public Source MainSource
		{
			get
			{
				if (this.mainSource == null)
				{
					DbSource dbSource = new DbSource();
					if (this.Owner != null)
					{
						dbSource.Connection = this.Owner.DefaultConnection;
					}
					this.MainSource = dbSource;
				}
				return this.mainSource;
			}
			set
			{
				if (this.mainSource != null)
				{
					this.mainSource.Owner = null;
				}
				this.mainSource = value;
				if (value != null)
				{
					this.mainSource.Owner = this;
					if (StringUtil.EmptyOrSpace(this.mainSource.Name))
					{
						this.mainSource.Name = "Fill";
					}
				}
			}
		}

		[DataSourceXmlElement(Name = "Mappings", SpecialWay = true)]
		[Browsable(false)]
		public DataColumnMappingCollection Mappings
		{
			get
			{
				if (this.mappings == null)
				{
					this.mappings = new DataColumnMappingCollection();
				}
				return this.mappings;
			}
			set
			{
				this.mappings = value;
			}
		}

		private bool ShouldSerializeMappings()
		{
			return this.mappings != null && this.mappings.Count > 0;
		}

		[DataSourceXmlAttribute]
		[DefaultValue(TypeAttributes.Public)]
		public TypeAttributes DataAccessorModifier
		{
			get
			{
				return this.dataAccessorModifier;
			}
			set
			{
				this.dataAccessorModifier = value;
			}
		}

		[DataSourceXmlAttribute]
		[DefaultValue("")]
		[MergableProperty(false)]
		public string Name
		{
			get
			{
				return this.dataTable.TableName;
			}
			set
			{
				if (this.dataTable.TableName != value)
				{
					if (this.CollectionParent != null)
					{
						this.CollectionParent.ValidateUniqueName(this, value);
					}
					this.dataTable.TableName = value;
				}
			}
		}

		internal DesignDataSource Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				if (this.owner != value)
				{
					if (this.owner != null)
					{
						string @namespace = this.owner.DataSet.Namespace;
					}
					this.owner = value;
				}
			}
		}

		public DbSourceParameterCollection Parameters
		{
			get
			{
				DbSource dbSource = this.MainSource as DbSource;
				if (dbSource != null && dbSource.SelectCommand != null)
				{
					return dbSource.SelectCommand.Parameters;
				}
				return null;
			}
		}

		private bool ShouldSerializeParameters()
		{
			if (this.TableType != TableType.RadTable)
			{
				return false;
			}
			DbSourceParameterCollection parameters = this.Parameters;
			return parameters != null && 0 < parameters.Count;
		}

		[Browsable(false)]
		public DataColumn[] PrimaryKeyColumns
		{
			get
			{
				return this.DataTable.PrimaryKey;
			}
			set
			{
				this.AddRemoveConstraintMonitor(false);
				try
				{
					base.SetPropertyValue("PrimaryKey", value);
					this.OnConstraintChanged();
				}
				finally
				{
					this.AddRemoveConstraintMonitor(true);
				}
			}
		}

		[DefaultValue(null)]
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
		public string PublicTypeName
		{
			get
			{
				string text;
				switch (this.tableType)
				{
				case TableType.DataTable:
					text = "DataTable";
					break;
				case TableType.RadTable:
					text = "DataTable";
					break;
				default:
					return null;
				}
				return text;
			}
		}

		[Browsable(false)]
		public DbSourceCommand SelectCommand
		{
			get
			{
				DbSource dbSource = this.EnsureDbSource();
				return dbSource.SelectCommand;
			}
			set
			{
				DbSource dbSource = this.EnsureDbSource();
				dbSource.SelectCommand = value;
			}
		}

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

		[Browsable(false)]
		public TableType TableType
		{
			get
			{
				return this.tableType;
			}
			set
			{
				this.tableType = value;
				if (this.tableType == TableType.RadTable)
				{
					this.DataAccessor = new DataAccessor(this);
					return;
				}
				this.DataAccessor = null;
			}
		}

		internal event EventHandler TableTypeChanged
		{
			add
			{
				this.tableTypeChanged = (EventHandler)Delegate.Combine(this.tableTypeChanged, value);
			}
			remove
			{
				this.tableTypeChanged = (EventHandler)Delegate.Remove(this.tableTypeChanged, value);
			}
		}

		[DefaultValue(null)]
		public DbSourceCommand UpdateCommand
		{
			get
			{
				DbSource dbSource = this.EnsureDbSource();
				return dbSource.UpdateCommand;
			}
			set
			{
				DbSource dbSource = this.EnsureDbSource();
				dbSource.UpdateCommand = value;
			}
		}

		[DefaultValue(false)]
		[Browsable(false)]
		[DataSourceXmlAttribute(ItemType = typeof(bool))]
		public bool WebServiceAttribute
		{
			get
			{
				return this.webServiceAttribute;
			}
			set
			{
				this.webServiceAttribute = value;
			}
		}

		[DataSourceXmlAttribute]
		[Browsable(false)]
		public string WebServiceDescription
		{
			get
			{
				return this.webServiceDescription;
			}
			set
			{
				this.webServiceDescription = value;
			}
		}

		[Browsable(false)]
		[DataSourceXmlAttribute]
		public string WebServiceNamespace
		{
			get
			{
				return this.webServiceNamespace;
			}
			set
			{
				this.webServiceNamespace = value;
			}
		}

		void IDataSourceCommandTarget.AddChild(object child, bool fixName)
		{
			if (child is DesignColumn)
			{
				this.DesignColumns.Add((DesignColumn)child);
				return;
			}
			if (child is Source)
			{
				if (child is DbSource)
				{
					((DbSource)child).Connection = this.Connection;
					if (this.Connection != null)
					{
						((DbSource)child).ConnectionRef = this.Connection.Name;
					}
				}
				this.Sources.Add((Source)child);
			}
		}

		private void AddRemoveConstraintMonitor(bool addEventHandler)
		{
			if (addEventHandler)
			{
				if (this.DataTable != null)
				{
					this.DataTable.Constraints.CollectionChanged += this.OnConstraintCollectionChanged;
					return;
				}
			}
			else if (this.DataTable != null)
			{
				this.DataTable.Constraints.CollectionChanged -= this.OnConstraintCollectionChanged;
			}
		}

		bool IDataSourceCommandTarget.CanAddChildOfType(Type childType)
		{
			return typeof(DesignColumn).IsAssignableFrom(childType) || (this.TableType != TableType.DataTable && typeof(Source).IsAssignableFrom(childType)) || (typeof(DesignRelation).IsAssignableFrom(childType) && this.DesignColumns.Count > 0);
		}

		bool IDataSourceCommandTarget.CanInsertChildOfType(Type childType, object refChild)
		{
			if (typeof(DesignColumn).IsAssignableFrom(childType))
			{
				return refChild is DesignColumn;
			}
			return typeof(Source).IsAssignableFrom(childType) && this.TableType != TableType.DataTable && refChild is Source;
		}

		bool IDataSourceCommandTarget.CanRemoveChildren(ICollection children)
		{
			bool flag = true;
			foreach (object obj in children)
			{
				if (obj is DesignColumn)
				{
					if (((DesignColumn)obj).DesignTable != this)
					{
						flag = false;
						break;
					}
				}
				else if (obj is Source)
				{
					if (!this.Sources.Contains((Source)obj))
					{
						flag = false;
						break;
					}
				}
				else
				{
					if (!(obj is DataAccessor))
					{
						flag = false;
						break;
					}
					if (((DataAccessor)obj).DesignTable != this)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		internal void ConvertTableTypeTo(TableType newTableType)
		{
			if (newTableType != this.tableType)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.OnComponentChanging(this, null);
				}
				try
				{
					this.TableType = newTableType;
					this.mainSource = null;
					this.sources = null;
					this.mappings = null;
					this.provider = string.Empty;
					this.OnTableTypeChanged();
				}
				finally
				{
					if (componentChangeService != null)
					{
						componentChangeService.OnComponentChanged(this, null, null, null);
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.AddRemoveConstraintMonitor(false);
			}
			base.Dispose(disposing);
		}

		private DbSource EnsureDbSource()
		{
			if (this.tableType != TableType.RadTable)
			{
				throw new InternalException(null, "Operation invalid. Table gets data from something else than a database.", 20007, false, false);
			}
			if (this.MainSource == null)
			{
				this.MainSource = new DbSource();
			}
			DbSource dbSource = this.mainSource as DbSource;
			if (dbSource == null)
			{
				throw new InternalException(null, "Operation invalid. Table gets data from something else than a database.", 20007, false, false);
			}
			if (dbSource.DeleteCommand != null && StringUtil.EmptyOrSpace(dbSource.DeleteCommand.Name))
			{
				dbSource.DeleteCommand.Name = "(DeleteCommand)";
			}
			if (dbSource.UpdateCommand != null && StringUtil.EmptyOrSpace(dbSource.UpdateCommand.Name))
			{
				dbSource.UpdateCommand.Name = "(UpdateCommand)";
			}
			if (dbSource.SelectCommand != null && StringUtil.EmptyOrSpace(dbSource.SelectCommand.Name))
			{
				dbSource.SelectCommand.Name = "(SelectCommand)";
			}
			if (dbSource.InsertCommand != null && StringUtil.EmptyOrSpace(dbSource.InsertCommand.Name))
			{
				dbSource.InsertCommand.Name = "(InsertCommand)";
			}
			return dbSource;
		}

		object IDataSourceCommandTarget.GetObject(int index, bool getSiblingIfOutOfRange)
		{
			int count = this.DesignColumns.Count;
			int num = ((this.TableType == TableType.DataTable) ? 0 : this.Sources.Count);
			int num2 = ((this.TableType == TableType.DataTable) ? count : (count + num + 1));
			if (num2 <= 0)
			{
				return null;
			}
			if (!getSiblingIfOutOfRange && (index < 0 || index >= num2))
			{
				return null;
			}
			if (index >= num2)
			{
				index = num2 - 1;
			}
			IList list = this.Sources;
			if (index < 0)
			{
				if (count > 0)
				{
					return this.DesignColumns[0];
				}
				if (this.mainSource != null)
				{
					return this.mainSource;
				}
				if (num > 0)
				{
					return list[0];
				}
				return null;
			}
			else
			{
				if (index < count)
				{
					return this.DesignColumns[index];
				}
				if (this.TableType != TableType.DataTable)
				{
					index -= count;
					if (index == 0)
					{
						return this.MainSource;
					}
					index--;
					if (index < num)
					{
						return list[index];
					}
				}
				return null;
			}
		}

		internal ArrayList GetRelatedDataConstraints(ICollection columns, bool uniqueOnly)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.dataTable.Constraints)
			{
				Constraint constraint = (Constraint)obj;
				DataColumn[] array = null;
				if (constraint is UniqueConstraint)
				{
					array = ((UniqueConstraint)constraint).Columns;
				}
				else if (!uniqueOnly && constraint is ForeignKeyConstraint)
				{
					array = ((ForeignKeyConstraint)constraint).Columns;
				}
				if (array != null)
				{
					foreach (object obj2 in columns)
					{
						if (obj2 is DesignColumn)
						{
							DesignColumn designColumn = obj2 as DesignColumn;
							if (((IList)array).Contains(designColumn.DataColumn))
							{
								arrayList.Add(constraint);
								break;
							}
						}
					}
				}
			}
			return arrayList;
		}

		internal bool IsForeignKeyConstraint(DataColumn column)
		{
			foreach (object obj in this.dataTable.Constraints)
			{
				Constraint constraint = (Constraint)obj;
				DataColumn[] array = null;
				if (constraint is ForeignKeyConstraint)
				{
					array = ((ForeignKeyConstraint)constraint).Columns;
				}
				if (array != null && ((IList)array).Contains(column))
				{
					return true;
				}
			}
			return false;
		}

		internal string GetUniqueRelationName(string proposedName)
		{
			return this.GetUniqueRelationName(proposedName, true, 1);
		}

		internal string GetUniqueRelationName(string proposedName, int startSuffix)
		{
			return this.GetUniqueRelationName(proposedName, false, startSuffix);
		}

		internal string GetUniqueRelationName(string proposedName, bool firstTryProposedName, int startSuffix)
		{
			if (this.Owner == null)
			{
				throw new InternalException("Need have DataSource");
			}
			SimpleNamedObjectCollection simpleNamedObjectCollection = new SimpleNamedObjectCollection();
			foreach (object obj in this.Owner.DesignRelations)
			{
				DesignRelation designRelation = (DesignRelation)obj;
				simpleNamedObjectCollection.Add(new SimpleNamedObject(designRelation.Name));
			}
			foreach (object obj2 in this.DataTable.Constraints)
			{
				Constraint constraint = (Constraint)obj2;
				simpleNamedObjectCollection.Add(new SimpleNamedObject(constraint.ConstraintName));
			}
			INameService nameService = simpleNamedObjectCollection.GetNameService();
			if (firstTryProposedName)
			{
				return nameService.CreateUniqueName(simpleNamedObjectCollection, proposedName);
			}
			return nameService.CreateUniqueName(simpleNamedObjectCollection, proposedName, startSuffix);
		}

		int IDataSourceCommandTarget.IndexOf(object child)
		{
			if (child is DesignColumn)
			{
				return this.DesignColumns.IndexOf((DesignColumn)child);
			}
			if (child is Source && this.TableType != TableType.DataTable)
			{
				if (child == this.mainSource)
				{
					return this.DesignColumns.Count;
				}
				int num = this.Sources.IndexOf((Source)child);
				if (num >= 0)
				{
					return this.DesignColumns.Count + num + 1;
				}
			}
			return -1;
		}

		void IDataSourceInitAfterLoading.InitializeAfterLoading()
		{
			if (this.Name == null || this.Name.Length == 0)
			{
				throw new DataSourceSerializationException(SR.GetString("DTDS_NameIsRequired", new object[] { "RadTable" }));
			}
			if (this.dataTable.DataSet != this.Owner.DataSet)
			{
				throw new DataSourceSerializationException(SR.GetString("DTDS_TableNotMatch", new object[] { this.Name }));
			}
		}

		void IDataSourceCommandTarget.InsertChild(object child, object refChild)
		{
			if (refChild == null)
			{
				((IDataSourceCommandTarget)this).AddChild(child, true);
				return;
			}
			if (child is DesignColumn)
			{
				this.DesignColumns.InsertBefore(child, refChild);
				return;
			}
			if (this.TableType != TableType.DataTable && child is Source)
			{
				this.Sources.InsertBefore(child, refChild);
			}
		}

		private bool IsInConstraintCollection(Constraint constraint)
		{
			return this.DataTable != null && this.DataTable.Constraints[constraint.ConstraintName] == constraint;
		}

		private void OnConstraintCollectionChanged(object sender, CollectionChangeEventArgs ccevent)
		{
			if (!this.inAccessConstraints)
			{
				this.OnConstraintChanged();
			}
		}

		private void OnConstraintChanged()
		{
			if (this.constraintsChanged != null)
			{
				this.constraintsChanged(this, new EventArgs());
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.OnComponentChanged(this, null, null, null);
				}
			}
		}

		internal void OnTableTypeChanged()
		{
			if (this.tableTypeChanged != null)
			{
				this.tableTypeChanged(this, EventArgs.Empty);
			}
		}

		private bool AddPrimaryKeyFromSchemaTable(DataTable schemaTable)
		{
			if (schemaTable.PrimaryKey.Length > 0 && this.DataTable.PrimaryKey.Length == 0)
			{
				DataColumn[] array = new DataColumn[schemaTable.PrimaryKey.Length];
				for (int i = 0; i < schemaTable.PrimaryKey.Length; i++)
				{
					DataColumn dataColumn = schemaTable.PrimaryKey[i];
					if (!this.Mappings.Contains(dataColumn.ColumnName))
					{
						return false;
					}
					string dataSetColumn = this.Mappings[dataColumn.ColumnName].DataSetColumn;
					if (!this.DataTable.Columns.Contains(dataSetColumn))
					{
						return false;
					}
					DataColumn dataColumn2 = this.DataTable.Columns[dataSetColumn];
					array[i] = dataColumn2;
				}
				this.PrimaryKeyColumns = array;
				return true;
			}
			return false;
		}

		void IDataSourceXmlSpecialOwner.ReadSpecialItem(string propertyName, XmlNode xmlNode, DataSourceXmlSerializer serializer)
		{
			if (propertyName == "Mappings")
			{
				string text = string.Empty;
				string text2 = string.Empty;
				XmlElement xmlElement = xmlNode as XmlElement;
				if (xmlElement != null)
				{
					foreach (object obj in xmlElement.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj;
						XmlElement xmlElement2 = xmlNode2 as XmlElement;
						if (xmlElement2 != null && xmlElement2.LocalName == "Mapping")
						{
							XmlAttribute xmlAttribute = xmlElement2.Attributes["SourceColumn"];
							if (xmlAttribute != null)
							{
								text = xmlAttribute.InnerText;
							}
							xmlAttribute = xmlElement2.Attributes["DataSetColumn"];
							if (xmlAttribute != null)
							{
								text2 = xmlAttribute.InnerText;
							}
							DataColumnMapping dataColumnMapping = new DataColumnMapping(text, text2);
							this.Mappings.Add(dataColumnMapping);
						}
					}
				}
			}
		}

		void IDataSourceXmlSerializable.ReadXml(XmlElement xmlElement, DataSourceXmlSerializer serializer)
		{
			if (xmlElement.LocalName == "TableAdapter" || xmlElement.LocalName == "DbTable")
			{
				this.TableType = TableType.RadTable;
				serializer.DeserializeBody(xmlElement, this);
			}
		}

		private DataColumn FindSharedColumn(ICollection dataColumns, ICollection designColumns)
		{
			foreach (object obj in dataColumns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				foreach (object obj2 in designColumns)
				{
					DesignColumn designColumn = obj2 as DesignColumn;
					if (designColumn != null && designColumn.DataColumn == dataColumn)
					{
						return dataColumn;
					}
				}
			}
			return null;
		}

		private void RemoveColumnsFromSource(Source source, string[] colsToRemove)
		{
		}

		void IDataSourceCommandTarget.RemoveChildren(ICollection children)
		{
			if (this.owner != null)
			{
				ArrayList relatedRelations = this.owner.GetRelatedRelations(new DesignTable[] { this });
				if (relatedRelations.Count > 0)
				{
					int num = 0;
					ArrayList arrayList = new ArrayList();
					foreach (object obj in relatedRelations)
					{
						DesignRelation designRelation = (DesignRelation)obj;
						if (designRelation.ParentDesignTable == this)
						{
							DataColumn dataColumn = this.FindSharedColumn(designRelation.ParentDataColumns, children);
							if (dataColumn != null)
							{
								num++;
								arrayList.Add(designRelation);
								continue;
							}
						}
						if (designRelation.ChildDesignTable == this)
						{
							DataColumn dataColumn2 = this.FindSharedColumn(designRelation.ChildDataColumns, children);
							if (dataColumn2 != null)
							{
								num++;
								arrayList.Add(designRelation);
							}
						}
					}
					if (num > 0)
					{
						foreach (object obj2 in arrayList)
						{
							DesignRelation designRelation2 = (DesignRelation)obj2;
							if (designRelation2.Owner != null)
							{
								designRelation2.Owner.DesignRelations.Remove(designRelation2);
							}
						}
					}
				}
			}
			ArrayList arrayList2 = this.GetRelatedDataConstraints(children, true);
			foreach (object obj3 in arrayList2)
			{
				UniqueConstraint uniqueConstraint = (UniqueConstraint)obj3;
				if (uniqueConstraint.IsPrimaryKey)
				{
					this.PrimaryKeyColumns = null;
				}
				else
				{
					this.RemoveConstraint(uniqueConstraint);
				}
			}
			arrayList2 = this.GetRelatedDataConstraints(children, false);
			foreach (object obj4 in arrayList2)
			{
				Constraint constraint = (Constraint)obj4;
				this.RemoveConstraint(constraint);
			}
			ArrayList arrayList3 = new ArrayList();
			foreach (object obj5 in children)
			{
				if (obj5 is DesignColumn)
				{
					DesignColumn designColumn = (DesignColumn)obj5;
					string[] array = DataDesignUtil.MapColumnNames(this.Mappings, new string[] { designColumn.Name }, DataDesignUtil.MappingDirection.DataSetToSource);
					arrayList3.Add(array[0]);
					this.DesignColumns.Remove((DesignColumn)obj5);
					this.RemoveColumnMapping(designColumn.Name);
				}
				else if (obj5 is Source)
				{
					this.Sources.Remove((Source)obj5);
				}
				else if (obj5 is DataAccessor)
				{
					this.ConvertTableTypeTo(TableType.DataTable);
				}
			}
			if (arrayList3.Count > 0)
			{
				string[] array2 = (string[])arrayList3.ToArray(typeof(string));
				this.RemoveColumnsFromSource(this.MainSource, array2);
				foreach (object obj6 in this.Sources)
				{
					Source source = (Source)obj6;
					this.RemoveColumnsFromSource(source, array2);
				}
			}
		}

		internal void RemoveConstraint(Constraint constraint)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.OnComponentChanging(this, null);
			}
			try
			{
				this.inAccessConstraints = true;
				if (this.dataTable.Constraints.CanRemove(constraint))
				{
					this.dataTable.Constraints.Remove(constraint);
				}
				else if (this.dataTable.Constraints.Count == 1)
				{
					if (this.dataTable.Constraints[0] == constraint)
					{
						this.dataTable.Constraints.Clear();
					}
				}
				else
				{
					Constraint[] array = new Constraint[this.dataTable.Constraints.Count - 1];
					ArrayList arrayList = new ArrayList();
					int num = 0;
					foreach (object obj in this.dataTable.Constraints)
					{
						Constraint constraint2 = (Constraint)obj;
						if (constraint2 != constraint)
						{
							array[num++] = constraint2;
						}
					}
					if (this.Owner != null)
					{
						foreach (object obj2 in this.Owner.DataSet.Relations)
						{
							DataRelation dataRelation = (DataRelation)obj2;
							if (dataRelation.ChildTable == this.dataTable)
							{
								arrayList.Add(dataRelation);
							}
						}
						foreach (object obj3 in arrayList)
						{
							DataRelation dataRelation2 = (DataRelation)obj3;
							this.Owner.DataSet.Relations.Remove(dataRelation2);
						}
					}
					this.dataTable.Constraints.Clear();
					this.dataTable.Constraints.AddRange(array);
					if (this.Owner != null)
					{
						foreach (object obj4 in arrayList)
						{
							DataRelation dataRelation3 = (DataRelation)obj4;
							this.Owner.DataSet.Relations.Add(dataRelation3);
						}
					}
				}
			}
			finally
			{
				this.inAccessConstraints = false;
				this.OnConstraintChanged();
			}
		}

		internal void RemoveColumnMapping(string columnName)
		{
		}

		internal void RemoveKey(UniqueConstraint constraint)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.owner.DesignRelations)
			{
				DesignRelation designRelation = (DesignRelation)obj;
				DataRelation dataRelation = designRelation.DataRelation;
				if (dataRelation != null && dataRelation.ParentKeyConstraint == constraint)
				{
					arrayList.Add(designRelation);
				}
			}
			foreach (object obj2 in arrayList)
			{
				DesignRelation designRelation2 = (DesignRelation)obj2;
				this.owner.DesignRelations.Remove(designRelation2);
			}
			this.RemoveConstraint(constraint);
		}

		internal void SetTypeForUndo(TableType newType)
		{
			this.tableType = newType;
		}

		void IDataSourceXmlSpecialOwner.WriteSpecialItem(string propertyName, XmlWriter writer, DataSourceXmlSerializer serializer)
		{
			if (propertyName == "Mappings")
			{
				foreach (object obj in this.Mappings)
				{
					DataColumnMapping dataColumnMapping = (DataColumnMapping)obj;
					writer.WriteStartElement(string.Empty, "Mapping", "urn:schemas-microsoft-com:xml-msdatasource");
					writer.WriteAttributeString("SourceColumn", dataColumnMapping.SourceColumn);
					writer.WriteAttributeString("DataSetColumn", dataColumnMapping.DataSetColumn);
					writer.WriteEndElement();
				}
			}
		}

		void IDataSourceXmlSerializable.WriteXml(XmlWriter xmlWriter, DataSourceXmlSerializer serializer)
		{
			switch (this.TableType)
			{
			case TableType.DataTable:
				break;
			case TableType.RadTable:
				xmlWriter.WriteStartElement(string.Empty, "TableAdapter", "urn:schemas-microsoft-com:xml-msdatasource");
				serializer.SerializeBody(xmlWriter, this);
				xmlWriter.WriteFullEndElement();
				break;
			default:
				return;
			}
		}

		internal void UpdateColumnMappingDataSetColumnName(string oldName, string newName)
		{
		}

		internal void UpdateColumnMappingSourceColumnName(string dataSetColumn, string newSourceColumn)
		{
		}

		internal string UserTableName
		{
			get
			{
				return this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_USER_TABLENAME] as string;
			}
			set
			{
				this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_USER_TABLENAME] = value;
			}
		}

		internal string GeneratorRunFillName
		{
			get
			{
				return this.generatorRunFillName;
			}
			set
			{
				this.generatorRunFillName = value;
			}
		}

		internal string GeneratorTablePropName
		{
			get
			{
				return this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_TABLEPROPNAME] as string;
			}
			set
			{
				this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_TABLEPROPNAME] = value;
			}
		}

		internal string GeneratorTableVarName
		{
			get
			{
				return this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_TABLEVARNAME] as string;
			}
			set
			{
				this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_TABLEVARNAME] = value;
			}
		}

		internal string GeneratorTableClassName
		{
			get
			{
				return this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_TABLECLASSNAME] as string;
			}
			set
			{
				this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_TABLECLASSNAME] = value;
			}
		}

		internal string GeneratorRowClassName
		{
			get
			{
				return this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWCLASSNAME] as string;
			}
			set
			{
				this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWCLASSNAME] = value;
			}
		}

		internal string GeneratorRowEvHandlerName
		{
			get
			{
				return this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWEVHANDLERNAME] as string;
			}
			set
			{
				this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWEVHANDLERNAME] = value;
			}
		}

		internal string GeneratorRowEvArgName
		{
			get
			{
				return this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWEVARGNAME] as string;
			}
			set
			{
				this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWEVARGNAME] = value;
			}
		}

		internal string GeneratorRowChangingName
		{
			get
			{
				return this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWCHANGINGNAME] as string;
			}
			set
			{
				this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWCHANGINGNAME] = value;
			}
		}

		internal string GeneratorRowChangedName
		{
			get
			{
				return this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWCHANGEDNAME] as string;
			}
			set
			{
				this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWCHANGEDNAME] = value;
			}
		}

		internal string GeneratorRowDeletingName
		{
			get
			{
				return this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWDELETINGNAME] as string;
			}
			set
			{
				this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWDELETINGNAME] = value;
			}
		}

		internal string GeneratorRowDeletedName
		{
			get
			{
				return this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWDELETEDNAME] as string;
			}
			set
			{
				this.dataTable.ExtendedProperties[DesignTable.EXTPROPNAME_GENERATOR_ROWDELETEDNAME] = value;
			}
		}

		internal override StringCollection NamingPropertyNames
		{
			get
			{
				return this.namingPropNames;
			}
		}

		[DefaultValue(null)]
		[Browsable(false)]
		[DataSourceXmlAttribute]
		public string GeneratorDataComponentClassName
		{
			get
			{
				return this.generatorDataComponentClassName;
			}
			set
			{
				this.generatorDataComponentClassName = value;
			}
		}

		[Browsable(false)]
		[DataSourceXmlAttribute]
		[DefaultValue(null)]
		public string UserDataComponentName
		{
			get
			{
				return this.userDataComponentName;
			}
			set
			{
				this.userDataComponentName = value;
			}
		}

		[Browsable(false)]
		public override string GeneratorName
		{
			get
			{
				return this.GeneratorTablePropName;
			}
		}

		internal DesignTable.CodeGenPropertyCache PropertyCache
		{
			get
			{
				return this.codeGenPropertyCache;
			}
			set
			{
				this.codeGenPropertyCache = value;
			}
		}

		private const string DATATABLE_NAMEROOT = "DataTable";

		private const string RADTABLE_NAMEROOT = "DataTable";

		private const string KEY_NAMEROOT = "Key";

		private const string PRIMARYKEY_PROPERTY = "PrimaryKey";

		internal const string MAINSOURCE_PROPERTY = "MainSource";

		private const string MAINSOURCE_NAME = "Fill";

		internal const string NAME_PROPERTY = "Name";

		private TableType tableType;

		private DataTable dataTable;

		private DataAccessor dataAccessor;

		private DesignColumnCollection designColumns;

		private DesignDataSource owner;

		private TypeAttributes dataAccessorModifier = TypeAttributes.Public;

		private Source mainSource;

		private SourceCollection sources;

		private DataColumnMappingCollection mappings;

		private bool webServiceAttribute;

		private string webServiceNamespace;

		private string webServiceDescription;

		private string provider;

		private string generatorRunFillName;

		private string baseClass;

		private string dataAccessorName;

		private bool inAccessConstraints;

		private string generatorDataComponentClassName;

		private string userDataComponentName;

		private DesignTable.CodeGenPropertyCache codeGenPropertyCache;

		private StringCollection namingPropNames = new StringCollection();

		internal static string EXTPROPNAME_USER_TABLENAME = "Generator_UserTableName";

		internal static string EXTPROPNAME_GENERATOR_TABLEPROPNAME = "Generator_TablePropName";

		internal static string EXTPROPNAME_GENERATOR_TABLEVARNAME = "Generator_TableVarName";

		internal static string EXTPROPNAME_GENERATOR_TABLECLASSNAME = "Generator_TableClassName";

		internal static string EXTPROPNAME_GENERATOR_ROWCLASSNAME = "Generator_RowClassName";

		internal static string EXTPROPNAME_GENERATOR_ROWEVHANDLERNAME = "Generator_RowEvHandlerName";

		internal static string EXTPROPNAME_GENERATOR_ROWEVARGNAME = "Generator_RowEvArgName";

		internal static string EXTPROPNAME_GENERATOR_ROWCHANGINGNAME = "Generator_RowChangingName";

		internal static string EXTPROPNAME_GENERATOR_ROWCHANGEDNAME = "Generator_RowChangedName";

		internal static string EXTPROPNAME_GENERATOR_ROWDELETINGNAME = "Generator_RowDeletingName";

		internal static string EXTPROPNAME_GENERATOR_ROWDELETEDNAME = "Generator_RowDeletedName";

		internal class CodeGenPropertyCache
		{
			internal Type AdapterType
			{
				get
				{
					if (this.adapterType == null)
					{
						if (this.designTable == null || this.designTable.Connection == null || this.designTable.Connection.Provider == null)
						{
							return null;
						}
						DbProviderFactory factory = ProviderManager.GetFactory(this.designTable.Connection.Provider);
						if (factory != null)
						{
							DataAdapter dataAdapter = factory.CreateDataAdapter();
							if (dataAdapter != null)
							{
								this.adapterType = dataAdapter.GetType();
							}
						}
					}
					return this.adapterType;
				}
			}

			internal Type ConnectionType
			{
				get
				{
					if (this.connectionType == null && this.designTable != null && this.designTable.Connection != null)
					{
						IDbConnection dbConnection = this.designTable.Connection.CreateEmptyDbConnection();
						if (dbConnection != null)
						{
							this.connectionType = dbConnection.GetType();
						}
					}
					return this.connectionType;
				}
			}

			internal Type TransactionType
			{
				get
				{
					if (this.transactionType == null)
					{
						if (this.designTable == null || this.designTable.Connection == null || this.designTable.Connection.Provider == null)
						{
							return null;
						}
						DbProviderFactory factory = ProviderManager.GetFactory(this.designTable.Connection.Provider);
						if (factory != null)
						{
							Type type = factory.CreateCommand().GetType();
							foreach (object obj in TypeDescriptor.GetProperties(type))
							{
								PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
								if (StringUtil.EqualValue(propertyDescriptor.Name, "Transaction"))
								{
									this.transactionType = propertyDescriptor.PropertyType;
									break;
								}
							}
						}
						if (this.transactionType == null)
						{
							this.transactionType = typeof(IDbTransaction);
						}
					}
					return this.transactionType;
				}
			}

			internal string TAMAdapterPropName
			{
				get
				{
					return this.tamAdapterPropName;
				}
				set
				{
					this.tamAdapterPropName = value;
				}
			}

			internal string TAMAdapterVarName
			{
				get
				{
					return this.tamAdapterVarName;
				}
				set
				{
					this.tamAdapterVarName = value;
				}
			}

			internal CodeGenPropertyCache(DesignTable designTable)
			{
				this.designTable = designTable;
			}

			private DesignTable designTable;

			private Type connectionType;

			private Type transactionType;

			private Type adapterType;

			private string tamAdapterPropName;

			private string tamAdapterVarName;
		}
	}
}

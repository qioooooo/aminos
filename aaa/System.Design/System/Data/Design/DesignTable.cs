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
	// Token: 0x0200009C RID: 156
	internal class DesignTable : DataSourceComponent, IDataSourceNamedObject, INamedObject, IDataSourceXmlSerializable, IDataSourceXmlSpecialOwner, IDataSourceInitAfterLoading, IDataSourceCommandTarget
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060006C5 RID: 1733 RVA: 0x0000D5A0 File Offset: 0x0000C5A0
		// (remove) Token: 0x060006C6 RID: 1734 RVA: 0x0000D5B9 File Offset: 0x0000C5B9
		private event EventHandler tableTypeChanged;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060006C7 RID: 1735 RVA: 0x0000D5D2 File Offset: 0x0000C5D2
		// (remove) Token: 0x060006C8 RID: 1736 RVA: 0x0000D5EB File Offset: 0x0000C5EB
		private event EventHandler constraintsChanged;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060006C9 RID: 1737 RVA: 0x0000D604 File Offset: 0x0000C604
		// (remove) Token: 0x060006CA RID: 1738 RVA: 0x0000D61D File Offset: 0x0000C61D
		private event EventHandler dataAccessorChanged;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060006CB RID: 1739 RVA: 0x0000D636 File Offset: 0x0000C636
		// (remove) Token: 0x060006CC RID: 1740 RVA: 0x0000D64F File Offset: 0x0000C64F
		private event EventHandler dataAccessorChanging;

		// Token: 0x060006CD RID: 1741 RVA: 0x0000D668 File Offset: 0x0000C668
		public DesignTable()
			: this(null, TableType.DataTable)
		{
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0000D672 File Offset: 0x0000C672
		public DesignTable(DataTable dataTable)
			: this(dataTable, TableType.DataTable)
		{
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0000D67C File Offset: 0x0000C67C
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

		// Token: 0x060006D0 RID: 1744 RVA: 0x0000D6F9 File Offset: 0x0000C6F9
		public DesignTable(DataTable dataTable, TableType tableType, DataColumnMappingCollection mappings)
			: this(dataTable, tableType)
		{
			this.mappings = mappings;
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0000D70A File Offset: 0x0000C70A
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x0000D725 File Offset: 0x0000C725
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

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0000D730 File Offset: 0x0000C730
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x0000D758 File Offset: 0x0000C758
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

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060006D5 RID: 1749 RVA: 0x0000D77C File Offset: 0x0000C77C
		// (remove) Token: 0x060006D6 RID: 1750 RVA: 0x0000D795 File Offset: 0x0000C795
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

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0000D7AE File Offset: 0x0000C7AE
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x0000D7B6 File Offset: 0x0000C7B6
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

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060006D9 RID: 1753 RVA: 0x0000D7F1 File Offset: 0x0000C7F1
		// (remove) Token: 0x060006DA RID: 1754 RVA: 0x0000D80A File Offset: 0x0000C80A
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

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060006DB RID: 1755 RVA: 0x0000D823 File Offset: 0x0000C823
		// (remove) Token: 0x060006DC RID: 1756 RVA: 0x0000D83C File Offset: 0x0000C83C
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

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0000D855 File Offset: 0x0000C855
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x0000D87B File Offset: 0x0000C87B
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

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x0000D884 File Offset: 0x0000C884
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x0000D88C File Offset: 0x0000C88C
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

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0000D8BC File Offset: 0x0000C8BC
		// (set) Token: 0x060006E2 RID: 1762 RVA: 0x0000D8D8 File Offset: 0x0000C8D8
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

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x0000D8F3 File Offset: 0x0000C8F3
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

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0000D90F File Offset: 0x0000C90F
		protected override object ExternalPropertyHost
		{
			get
			{
				return this.dataTable;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0000D918 File Offset: 0x0000C918
		internal bool HasAnyUpdateCommand
		{
			get
			{
				return this.TableType == TableType.RadTable && this.MainSource != null && this.MainSource is DbSource && ((DbSource)this.MainSource).CommandOperation == CommandOperation.Select && (this.DeleteCommand != null || this.InsertCommand != null || this.UpdateCommand != null);
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0000D974 File Offset: 0x0000C974
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

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x0000D9F0 File Offset: 0x0000C9F0
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x0000DA0C File Offset: 0x0000CA0C
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0000DA28 File Offset: 0x0000CA28
		// (set) Token: 0x060006EA RID: 1770 RVA: 0x0000DA6C File Offset: 0x0000CA6C
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

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x0000DAC5 File Offset: 0x0000CAC5
		// (set) Token: 0x060006EC RID: 1772 RVA: 0x0000DAE0 File Offset: 0x0000CAE0
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

		// Token: 0x060006ED RID: 1773 RVA: 0x0000DAE9 File Offset: 0x0000CAE9
		private bool ShouldSerializeMappings()
		{
			return this.mappings != null && this.mappings.Count > 0;
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0000DB03 File Offset: 0x0000CB03
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x0000DB0B File Offset: 0x0000CB0B
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

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0000DB14 File Offset: 0x0000CB14
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x0000DB21 File Offset: 0x0000CB21
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

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x0000DB57 File Offset: 0x0000CB57
		// (set) Token: 0x060006F3 RID: 1779 RVA: 0x0000DB5F File Offset: 0x0000CB5F
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

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0000DB8C File Offset: 0x0000CB8C
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

		// Token: 0x060006F5 RID: 1781 RVA: 0x0000DBC0 File Offset: 0x0000CBC0
		private bool ShouldSerializeParameters()
		{
			if (this.TableType != TableType.RadTable)
			{
				return false;
			}
			DbSourceParameterCollection parameters = this.Parameters;
			return parameters != null && 0 < parameters.Count;
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0000DBED File Offset: 0x0000CBED
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x0000DBFC File Offset: 0x0000CBFC
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

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0000DC3C File Offset: 0x0000CC3C
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x0000DC44 File Offset: 0x0000CC44
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

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0000DC50 File Offset: 0x0000CC50
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

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x0000DC88 File Offset: 0x0000CC88
		// (set) Token: 0x060006FC RID: 1788 RVA: 0x0000DCA4 File Offset: 0x0000CCA4
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

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x0000DCBF File Offset: 0x0000CCBF
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

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0000DCDB File Offset: 0x0000CCDB
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x0000DCE3 File Offset: 0x0000CCE3
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

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000700 RID: 1792 RVA: 0x0000DD09 File Offset: 0x0000CD09
		// (remove) Token: 0x06000701 RID: 1793 RVA: 0x0000DD22 File Offset: 0x0000CD22
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

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0000DD3C File Offset: 0x0000CD3C
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x0000DD58 File Offset: 0x0000CD58
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

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x0000DD73 File Offset: 0x0000CD73
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x0000DD7B File Offset: 0x0000CD7B
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

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x0000DD84 File Offset: 0x0000CD84
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x0000DD8C File Offset: 0x0000CD8C
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

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0000DD95 File Offset: 0x0000CD95
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x0000DD9D File Offset: 0x0000CD9D
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

		// Token: 0x0600070A RID: 1802 RVA: 0x0000DDA8 File Offset: 0x0000CDA8
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

		// Token: 0x0600070B RID: 1803 RVA: 0x0000DE20 File Offset: 0x0000CE20
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

		// Token: 0x0600070C RID: 1804 RVA: 0x0000DE7C File Offset: 0x0000CE7C
		bool IDataSourceCommandTarget.CanAddChildOfType(Type childType)
		{
			return typeof(DesignColumn).IsAssignableFrom(childType) || (this.TableType != TableType.DataTable && typeof(Source).IsAssignableFrom(childType)) || (typeof(DesignRelation).IsAssignableFrom(childType) && this.DesignColumns.Count > 0);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0000DEDC File Offset: 0x0000CEDC
		bool IDataSourceCommandTarget.CanInsertChildOfType(Type childType, object refChild)
		{
			if (typeof(DesignColumn).IsAssignableFrom(childType))
			{
				return refChild is DesignColumn;
			}
			return typeof(Source).IsAssignableFrom(childType) && this.TableType != TableType.DataTable && refChild is Source;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0000DF2C File Offset: 0x0000CF2C
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

		// Token: 0x0600070F RID: 1807 RVA: 0x0000DFD0 File Offset: 0x0000CFD0
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

		// Token: 0x06000710 RID: 1808 RVA: 0x0000E054 File Offset: 0x0000D054
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.AddRemoveConstraintMonitor(false);
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0000E068 File Offset: 0x0000D068
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

		// Token: 0x06000712 RID: 1810 RVA: 0x0000E170 File Offset: 0x0000D170
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

		// Token: 0x06000713 RID: 1811 RVA: 0x0000E240 File Offset: 0x0000D240
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

		// Token: 0x06000714 RID: 1812 RVA: 0x0000E344 File Offset: 0x0000D344
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

		// Token: 0x06000715 RID: 1813 RVA: 0x0000E3C8 File Offset: 0x0000D3C8
		internal string GetUniqueRelationName(string proposedName)
		{
			return this.GetUniqueRelationName(proposedName, true, 1);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0000E3D3 File Offset: 0x0000D3D3
		internal string GetUniqueRelationName(string proposedName, int startSuffix)
		{
			return this.GetUniqueRelationName(proposedName, false, startSuffix);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0000E3E0 File Offset: 0x0000D3E0
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

		// Token: 0x06000718 RID: 1816 RVA: 0x0000E4E4 File Offset: 0x0000D4E4
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

		// Token: 0x06000719 RID: 1817 RVA: 0x0000E558 File Offset: 0x0000D558
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

		// Token: 0x0600071A RID: 1818 RVA: 0x0000E5D4 File Offset: 0x0000D5D4
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

		// Token: 0x0600071B RID: 1819 RVA: 0x0000E620 File Offset: 0x0000D620
		private bool IsInConstraintCollection(Constraint constraint)
		{
			return this.DataTable != null && this.DataTable.Constraints[constraint.ConstraintName] == constraint;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0000E645 File Offset: 0x0000D645
		private void OnConstraintCollectionChanged(object sender, CollectionChangeEventArgs ccevent)
		{
			if (!this.inAccessConstraints)
			{
				this.OnConstraintChanged();
			}
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0000E658 File Offset: 0x0000D658
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

		// Token: 0x0600071E RID: 1822 RVA: 0x0000E6A1 File Offset: 0x0000D6A1
		internal void OnTableTypeChanged()
		{
			if (this.tableTypeChanged != null)
			{
				this.tableTypeChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0000E6BC File Offset: 0x0000D6BC
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

		// Token: 0x06000720 RID: 1824 RVA: 0x0000E778 File Offset: 0x0000D778
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

		// Token: 0x06000721 RID: 1825 RVA: 0x0000E874 File Offset: 0x0000D874
		void IDataSourceXmlSerializable.ReadXml(XmlElement xmlElement, DataSourceXmlSerializer serializer)
		{
			if (xmlElement.LocalName == "TableAdapter" || xmlElement.LocalName == "DbTable")
			{
				this.TableType = TableType.RadTable;
				serializer.DeserializeBody(xmlElement, this);
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0000E8AC File Offset: 0x0000D8AC
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

		// Token: 0x06000723 RID: 1827 RVA: 0x0000E958 File Offset: 0x0000D958
		private void RemoveColumnsFromSource(Source source, string[] colsToRemove)
		{
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0000E95C File Offset: 0x0000D95C
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

		// Token: 0x06000725 RID: 1829 RVA: 0x0000ECC0 File Offset: 0x0000DCC0
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

		// Token: 0x06000726 RID: 1830 RVA: 0x0000EF8C File Offset: 0x0000DF8C
		internal void RemoveColumnMapping(string columnName)
		{
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0000EF90 File Offset: 0x0000DF90
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

		// Token: 0x06000728 RID: 1832 RVA: 0x0000F06C File Offset: 0x0000E06C
		internal void SetTypeForUndo(TableType newType)
		{
			this.tableType = newType;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0000F078 File Offset: 0x0000E078
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

		// Token: 0x0600072A RID: 1834 RVA: 0x0000F114 File Offset: 0x0000E114
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

		// Token: 0x0600072B RID: 1835 RVA: 0x0000F15A File Offset: 0x0000E15A
		internal void UpdateColumnMappingDataSetColumnName(string oldName, string newName)
		{
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0000F15C File Offset: 0x0000E15C
		internal void UpdateColumnMappingSourceColumnName(string dataSetColumn, string newSourceColumn)
		{
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0000F15E File Offset: 0x0000E15E
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x0000F17A File Offset: 0x0000E17A
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

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0000F192 File Offset: 0x0000E192
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x0000F19A File Offset: 0x0000E19A
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

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0000F1A3 File Offset: 0x0000E1A3
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x0000F1BF File Offset: 0x0000E1BF
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

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x0000F1D7 File Offset: 0x0000E1D7
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x0000F1F3 File Offset: 0x0000E1F3
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

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x0000F20B File Offset: 0x0000E20B
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x0000F227 File Offset: 0x0000E227
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

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0000F23F File Offset: 0x0000E23F
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x0000F25B File Offset: 0x0000E25B
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

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x0000F273 File Offset: 0x0000E273
		// (set) Token: 0x0600073A RID: 1850 RVA: 0x0000F28F File Offset: 0x0000E28F
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

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x0000F2A7 File Offset: 0x0000E2A7
		// (set) Token: 0x0600073C RID: 1852 RVA: 0x0000F2C3 File Offset: 0x0000E2C3
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

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x0000F2DB File Offset: 0x0000E2DB
		// (set) Token: 0x0600073E RID: 1854 RVA: 0x0000F2F7 File Offset: 0x0000E2F7
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

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x0000F30F File Offset: 0x0000E30F
		// (set) Token: 0x06000740 RID: 1856 RVA: 0x0000F32B File Offset: 0x0000E32B
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

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0000F343 File Offset: 0x0000E343
		// (set) Token: 0x06000742 RID: 1858 RVA: 0x0000F35F File Offset: 0x0000E35F
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

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x0000F377 File Offset: 0x0000E377
		// (set) Token: 0x06000744 RID: 1860 RVA: 0x0000F393 File Offset: 0x0000E393
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

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x0000F3AB File Offset: 0x0000E3AB
		internal override StringCollection NamingPropertyNames
		{
			get
			{
				return this.namingPropNames;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x0000F3B3 File Offset: 0x0000E3B3
		// (set) Token: 0x06000747 RID: 1863 RVA: 0x0000F3BB File Offset: 0x0000E3BB
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

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x0000F3C4 File Offset: 0x0000E3C4
		// (set) Token: 0x06000749 RID: 1865 RVA: 0x0000F3CC File Offset: 0x0000E3CC
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

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x0000F3D5 File Offset: 0x0000E3D5
		[Browsable(false)]
		public override string GeneratorName
		{
			get
			{
				return this.GeneratorTablePropName;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0000F3DD File Offset: 0x0000E3DD
		// (set) Token: 0x0600074C RID: 1868 RVA: 0x0000F3E5 File Offset: 0x0000E3E5
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

		// Token: 0x04000B5A RID: 2906
		private const string DATATABLE_NAMEROOT = "DataTable";

		// Token: 0x04000B5B RID: 2907
		private const string RADTABLE_NAMEROOT = "DataTable";

		// Token: 0x04000B5C RID: 2908
		private const string KEY_NAMEROOT = "Key";

		// Token: 0x04000B5D RID: 2909
		private const string PRIMARYKEY_PROPERTY = "PrimaryKey";

		// Token: 0x04000B5E RID: 2910
		internal const string MAINSOURCE_PROPERTY = "MainSource";

		// Token: 0x04000B5F RID: 2911
		private const string MAINSOURCE_NAME = "Fill";

		// Token: 0x04000B60 RID: 2912
		internal const string NAME_PROPERTY = "Name";

		// Token: 0x04000B61 RID: 2913
		private TableType tableType;

		// Token: 0x04000B62 RID: 2914
		private DataTable dataTable;

		// Token: 0x04000B63 RID: 2915
		private DataAccessor dataAccessor;

		// Token: 0x04000B64 RID: 2916
		private DesignColumnCollection designColumns;

		// Token: 0x04000B65 RID: 2917
		private DesignDataSource owner;

		// Token: 0x04000B66 RID: 2918
		private TypeAttributes dataAccessorModifier = TypeAttributes.Public;

		// Token: 0x04000B67 RID: 2919
		private Source mainSource;

		// Token: 0x04000B68 RID: 2920
		private SourceCollection sources;

		// Token: 0x04000B69 RID: 2921
		private DataColumnMappingCollection mappings;

		// Token: 0x04000B6A RID: 2922
		private bool webServiceAttribute;

		// Token: 0x04000B6B RID: 2923
		private string webServiceNamespace;

		// Token: 0x04000B6C RID: 2924
		private string webServiceDescription;

		// Token: 0x04000B6D RID: 2925
		private string provider;

		// Token: 0x04000B6E RID: 2926
		private string generatorRunFillName;

		// Token: 0x04000B6F RID: 2927
		private string baseClass;

		// Token: 0x04000B70 RID: 2928
		private string dataAccessorName;

		// Token: 0x04000B73 RID: 2931
		private bool inAccessConstraints;

		// Token: 0x04000B76 RID: 2934
		private string generatorDataComponentClassName;

		// Token: 0x04000B77 RID: 2935
		private string userDataComponentName;

		// Token: 0x04000B78 RID: 2936
		private DesignTable.CodeGenPropertyCache codeGenPropertyCache;

		// Token: 0x04000B79 RID: 2937
		private StringCollection namingPropNames = new StringCollection();

		// Token: 0x04000B7A RID: 2938
		internal static string EXTPROPNAME_USER_TABLENAME = "Generator_UserTableName";

		// Token: 0x04000B7B RID: 2939
		internal static string EXTPROPNAME_GENERATOR_TABLEPROPNAME = "Generator_TablePropName";

		// Token: 0x04000B7C RID: 2940
		internal static string EXTPROPNAME_GENERATOR_TABLEVARNAME = "Generator_TableVarName";

		// Token: 0x04000B7D RID: 2941
		internal static string EXTPROPNAME_GENERATOR_TABLECLASSNAME = "Generator_TableClassName";

		// Token: 0x04000B7E RID: 2942
		internal static string EXTPROPNAME_GENERATOR_ROWCLASSNAME = "Generator_RowClassName";

		// Token: 0x04000B7F RID: 2943
		internal static string EXTPROPNAME_GENERATOR_ROWEVHANDLERNAME = "Generator_RowEvHandlerName";

		// Token: 0x04000B80 RID: 2944
		internal static string EXTPROPNAME_GENERATOR_ROWEVARGNAME = "Generator_RowEvArgName";

		// Token: 0x04000B81 RID: 2945
		internal static string EXTPROPNAME_GENERATOR_ROWCHANGINGNAME = "Generator_RowChangingName";

		// Token: 0x04000B82 RID: 2946
		internal static string EXTPROPNAME_GENERATOR_ROWCHANGEDNAME = "Generator_RowChangedName";

		// Token: 0x04000B83 RID: 2947
		internal static string EXTPROPNAME_GENERATOR_ROWDELETINGNAME = "Generator_RowDeletingName";

		// Token: 0x04000B84 RID: 2948
		internal static string EXTPROPNAME_GENERATOR_ROWDELETEDNAME = "Generator_RowDeletedName";

		// Token: 0x0200009D RID: 157
		internal class CodeGenPropertyCache
		{
			// Token: 0x170000EF RID: 239
			// (get) Token: 0x0600074E RID: 1870 RVA: 0x0000F46C File Offset: 0x0000E46C
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

			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x0600074F RID: 1871 RVA: 0x0000F4E0 File Offset: 0x0000E4E0
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

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x06000750 RID: 1872 RVA: 0x0000F530 File Offset: 0x0000E530
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

			// Token: 0x170000F2 RID: 242
			// (get) Token: 0x06000751 RID: 1873 RVA: 0x0000F61C File Offset: 0x0000E61C
			// (set) Token: 0x06000752 RID: 1874 RVA: 0x0000F624 File Offset: 0x0000E624
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

			// Token: 0x170000F3 RID: 243
			// (get) Token: 0x06000753 RID: 1875 RVA: 0x0000F62D File Offset: 0x0000E62D
			// (set) Token: 0x06000754 RID: 1876 RVA: 0x0000F635 File Offset: 0x0000E635
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

			// Token: 0x06000755 RID: 1877 RVA: 0x0000F63E File Offset: 0x0000E63E
			internal CodeGenPropertyCache(DesignTable designTable)
			{
				this.designTable = designTable;
			}

			// Token: 0x04000B85 RID: 2949
			private DesignTable designTable;

			// Token: 0x04000B86 RID: 2950
			private Type connectionType;

			// Token: 0x04000B87 RID: 2951
			private Type transactionType;

			// Token: 0x04000B88 RID: 2952
			private Type adapterType;

			// Token: 0x04000B89 RID: 2953
			private string tamAdapterPropName;

			// Token: 0x04000B8A RID: 2954
			private string tamAdapterVarName;
		}
	}
}

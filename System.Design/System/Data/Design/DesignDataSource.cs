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
	[DataSourceXmlClass("DataSource")]
	internal class DesignDataSource : DataSourceComponent, IDataSourceNamedObject, INamedObject, IDataSourceCommandTarget
	{
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

		[Browsable(false)]
		public string PublicTypeName
		{
			get
			{
				return "DataSet";
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

		internal override StringCollection NamingPropertyNames
		{
			get
			{
				return this.namingPropNames;
			}
		}

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

		bool IDataSourceCommandTarget.CanAddChildOfType(Type childType)
		{
			return typeof(DesignTable).IsAssignableFrom(childType) || typeof(IDesignConnection).IsAssignableFrom(childType) || typeof(Source).IsAssignableFrom(childType) || (typeof(DesignRelation).IsAssignableFrom(childType) && ((ICollection)this.DesignTables).Count > 0);
		}

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

		object IDataSourceCommandTarget.GetObject(int index, bool getSiblingIfOutOfRange)
		{
			throw new NotImplementedException();
		}

		int IDataSourceCommandTarget.IndexOf(object child)
		{
			throw new NotImplementedException();
		}

		public void ReadXmlSchema(Stream stream)
		{
			DataSourceXmlTextReader dataSourceXmlTextReader = new DataSourceXmlTextReader(this, stream);
			this.ReadXmlSchema(dataSourceXmlTextReader);
		}

		public void ReadXmlSchema(TextReader textReader)
		{
			DataSourceXmlTextReader dataSourceXmlTextReader = new DataSourceXmlTextReader(this, textReader);
			this.ReadXmlSchema(dataSourceXmlTextReader);
		}

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

		private const string EXTPROPNAME_ENABLE_TABLEADAPTERMANAGER = "EnableTableAdapterManager";

		private DataSet dataSet;

		private DesignTableCollection designTables;

		private DesignRelationCollection designRelations;

		private DesignConnectionCollection designConnections;

		private int defaultConnectionIndex;

		private SourceCollection sources;

		private TypeAttributes modifier = TypeAttributes.Public;

		private SchemaSerializationMode schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

		private DataSourceXmlSerializer serializer;

		private StringCollection namingPropNames = new StringCollection();

		internal static string EXTPROPNAME_USER_DATASETNAME = "Generator_UserDSName";

		internal static string EXTPROPNAME_GENERATOR_DATASETNAME = "Generator_DataSetName";

		private string functionsComponentName;

		private string userFunctionsComponentName;

		private string generatorFunctionsComponentClassName;
	}
}

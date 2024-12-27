using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data
{
	// Token: 0x02000094 RID: 148
	[XmlSchemaProvider("GetDataSetSchema")]
	[Designer("Microsoft.VSDesigner.Data.VS.DataSetDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("DataSetName")]
	[ResDescription("DataSetDescr")]
	[XmlRoot("DataSet")]
	[ToolboxItem("Microsoft.VSDesigner.Data.VS.DataSetToolboxItem, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Serializable]
	public class DataSet : MarshalByValueComponent, IListSource, IXmlSerializable, ISupportInitializeNotification, ISupportInitialize, ISerializable
	{
		// Token: 0x06000876 RID: 2166 RVA: 0x001E40D0 File Offset: 0x001E34D0
		public DataSet()
		{
			GC.SuppressFinalize(this);
			Bid.Trace("<ds.DataSet.DataSet|API> %d#\n", this.ObjectID);
			this.tableCollection = new DataTableCollection(this);
			this.relationCollection = new DataRelationCollection.DataSetRelationCollection(this);
			this._culture = CultureInfo.CurrentCulture;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x001E4174 File Offset: 0x001E3574
		public DataSet(string dataSetName)
			: this()
		{
			this.DataSetName = dataSetName;
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x001E4190 File Offset: 0x001E3590
		// (set) Token: 0x06000879 RID: 2169 RVA: 0x001E41A4 File Offset: 0x001E35A4
		[DefaultValue(SerializationFormat.Xml)]
		public SerializationFormat RemotingFormat
		{
			get
			{
				return this._remotingFormat;
			}
			set
			{
				if (value != SerializationFormat.Binary && value != SerializationFormat.Xml)
				{
					throw ExceptionBuilder.InvalidRemotingFormat(value);
				}
				this._remotingFormat = value;
				for (int i = 0; i < this.Tables.Count; i++)
				{
					this.Tables[i].RemotingFormat = value;
				}
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x001E41F0 File Offset: 0x001E35F0
		// (set) Token: 0x0600087B RID: 2171 RVA: 0x001E4200 File Offset: 0x001E3600
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual SchemaSerializationMode SchemaSerializationMode
		{
			get
			{
				return SchemaSerializationMode.IncludeSchema;
			}
			set
			{
				if (value != SchemaSerializationMode.IncludeSchema)
				{
					throw ExceptionBuilder.CannotChangeSchemaSerializationMode();
				}
			}
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x001E4218 File Offset: 0x001E3618
		protected bool IsBinarySerialized(SerializationInfo info, StreamingContext context)
		{
			SerializationFormat serializationFormat = SerializationFormat.Xml;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Name == "DataSet.RemotingFormat")
				{
					serializationFormat = (SerializationFormat)enumerator.Value;
					break;
				}
			}
			return serializationFormat == SerializationFormat.Binary;
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x001E425C File Offset: 0x001E365C
		protected SchemaSerializationMode DetermineSchemaSerializationMode(SerializationInfo info, StreamingContext context)
		{
			SchemaSerializationMode schemaSerializationMode = SchemaSerializationMode.IncludeSchema;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Name == "SchemaSerializationMode.DataSet")
				{
					schemaSerializationMode = (SchemaSerializationMode)enumerator.Value;
					break;
				}
			}
			return schemaSerializationMode;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x001E42A0 File Offset: 0x001E36A0
		protected SchemaSerializationMode DetermineSchemaSerializationMode(XmlReader reader)
		{
			SchemaSerializationMode schemaSerializationMode = SchemaSerializationMode.IncludeSchema;
			reader.MoveToContent();
			if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
			{
				string attribute = reader.GetAttribute("SchemaSerializationMode", "urn:schemas-microsoft-com:xml-msdata");
				if (string.Compare(attribute, "ExcludeSchema", StringComparison.OrdinalIgnoreCase) == 0)
				{
					schemaSerializationMode = SchemaSerializationMode.ExcludeSchema;
				}
				else if (string.Compare(attribute, "IncludeSchema", StringComparison.OrdinalIgnoreCase) == 0)
				{
					schemaSerializationMode = SchemaSerializationMode.IncludeSchema;
				}
				else if (attribute != null)
				{
					throw ExceptionBuilder.InvalidSchemaSerializationMode(typeof(SchemaSerializationMode), attribute);
				}
			}
			return schemaSerializationMode;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x001E4314 File Offset: 0x001E3714
		protected void GetSerializationData(SerializationInfo info, StreamingContext context)
		{
			SerializationFormat serializationFormat = SerializationFormat.Xml;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Name == "DataSet.RemotingFormat")
				{
					serializationFormat = (SerializationFormat)enumerator.Value;
					break;
				}
			}
			this.DeserializeDataSetData(info, context, serializationFormat);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x001E4360 File Offset: 0x001E3760
		protected DataSet(SerializationInfo info, StreamingContext context)
			: this(info, context, true)
		{
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x001E4378 File Offset: 0x001E3778
		protected DataSet(SerializationInfo info, StreamingContext context, bool ConstructSchema)
			: this()
		{
			SerializationFormat serializationFormat = SerializationFormat.Xml;
			SchemaSerializationMode schemaSerializationMode = SchemaSerializationMode.IncludeSchema;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name;
				if ((name = enumerator.Name) != null)
				{
					if (!(name == "DataSet.RemotingFormat"))
					{
						if (name == "SchemaSerializationMode.DataSet")
						{
							schemaSerializationMode = (SchemaSerializationMode)enumerator.Value;
						}
					}
					else
					{
						serializationFormat = (SerializationFormat)enumerator.Value;
					}
				}
			}
			if (schemaSerializationMode == SchemaSerializationMode.ExcludeSchema)
			{
				this.InitializeDerivedDataSet();
			}
			if (serializationFormat == SerializationFormat.Xml && !ConstructSchema)
			{
				return;
			}
			this.DeserializeDataSet(info, context, serializationFormat, schemaSerializationMode);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x001E43FC File Offset: 0x001E37FC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			SerializationFormat remotingFormat = this.RemotingFormat;
			this.SerializeDataSet(info, context, remotingFormat);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x001E441C File Offset: 0x001E381C
		protected virtual void InitializeDerivedDataSet()
		{
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x001E442C File Offset: 0x001E382C
		private void SerializeDataSet(SerializationInfo info, StreamingContext context, SerializationFormat remotingFormat)
		{
			info.AddValue("DataSet.RemotingVersion", new Version(2, 0));
			if (remotingFormat != SerializationFormat.Xml)
			{
				info.AddValue("DataSet.RemotingFormat", remotingFormat);
			}
			if (SchemaSerializationMode.IncludeSchema != this.SchemaSerializationMode)
			{
				info.AddValue("SchemaSerializationMode.DataSet", this.SchemaSerializationMode);
			}
			if (remotingFormat != SerializationFormat.Xml)
			{
				if (this.SchemaSerializationMode == SchemaSerializationMode.IncludeSchema)
				{
					this.SerializeDataSetProperties(info, context);
					info.AddValue("DataSet.Tables.Count", this.Tables.Count);
					for (int i = 0; i < this.Tables.Count; i++)
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter(null, new StreamingContext(context.State, false));
						MemoryStream memoryStream = new MemoryStream();
						binaryFormatter.Serialize(memoryStream, this.Tables[i]);
						memoryStream.Position = 0L;
						info.AddValue(string.Format(CultureInfo.InvariantCulture, "DataSet.Tables_{0}", new object[] { i }), memoryStream.GetBuffer());
					}
					for (int j = 0; j < this.Tables.Count; j++)
					{
						this.Tables[j].SerializeConstraints(info, context, j, true);
					}
					this.SerializeRelations(info, context);
					for (int k = 0; k < this.Tables.Count; k++)
					{
						this.Tables[k].SerializeExpressionColumns(info, context, k);
					}
				}
				else
				{
					this.SerializeDataSetProperties(info, context);
				}
				for (int l = 0; l < this.Tables.Count; l++)
				{
					this.Tables[l].SerializeTableData(info, context, l);
				}
				return;
			}
			string xmlSchemaForRemoting = this.GetXmlSchemaForRemoting(null);
			info.AddValue("XmlSchema", xmlSchemaForRemoting);
			StringBuilder stringBuilder = new StringBuilder(this.EstimatedXmlStringSize() * 2);
			StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			this.WriteXml(xmlTextWriter, XmlWriteMode.DiffGram);
			string text = stringWriter.ToString();
			info.AddValue("XmlDiffGram", text);
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x001E4628 File Offset: 0x001E3A28
		internal void DeserializeDataSet(SerializationInfo info, StreamingContext context, SerializationFormat remotingFormat, SchemaSerializationMode schemaSerializationMode)
		{
			this.DeserializeDataSetSchema(info, context, remotingFormat, schemaSerializationMode);
			this.DeserializeDataSetData(info, context, remotingFormat);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x001E464C File Offset: 0x001E3A4C
		private void DeserializeDataSetSchema(SerializationInfo info, StreamingContext context, SerializationFormat remotingFormat, SchemaSerializationMode schemaSerializationMode)
		{
			if (remotingFormat == SerializationFormat.Xml)
			{
				string text = (string)info.GetValue("XmlSchema", typeof(string));
				if (text != null)
				{
					this.ReadXmlSchema(new XmlTextReader(new StringReader(text)), true);
				}
				return;
			}
			if (schemaSerializationMode == SchemaSerializationMode.IncludeSchema)
			{
				this.DeserializeDataSetProperties(info, context);
				int @int = info.GetInt32("DataSet.Tables.Count");
				for (int i = 0; i < @int; i++)
				{
					byte[] array = (byte[])info.GetValue(string.Format(CultureInfo.InvariantCulture, "DataSet.Tables_{0}", new object[] { i }), typeof(byte[]));
					MemoryStream memoryStream = new MemoryStream(array);
					memoryStream.Position = 0L;
					BinaryFormatter binaryFormatter = new BinaryFormatter(null, new StreamingContext(context.State, false));
					DataTable dataTable = (DataTable)binaryFormatter.Deserialize(memoryStream);
					this.Tables.Add(dataTable);
				}
				for (int j = 0; j < @int; j++)
				{
					this.Tables[j].DeserializeConstraints(info, context, j, true);
				}
				this.DeserializeRelations(info, context);
				for (int k = 0; k < @int; k++)
				{
					this.Tables[k].DeserializeExpressionColumns(info, context, k);
				}
				return;
			}
			this.DeserializeDataSetProperties(info, context);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x001E4798 File Offset: 0x001E3B98
		private void DeserializeDataSetData(SerializationInfo info, StreamingContext context, SerializationFormat remotingFormat)
		{
			if (remotingFormat != SerializationFormat.Xml)
			{
				for (int i = 0; i < this.Tables.Count; i++)
				{
					this.Tables[i].DeserializeTableData(info, context, i);
				}
				return;
			}
			string text = (string)info.GetValue("XmlDiffGram", typeof(string));
			if (text != null)
			{
				this.ReadXml(new XmlTextReader(new StringReader(text)), XmlReadMode.DiffGram);
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x001E4804 File Offset: 0x001E3C04
		private void SerializeDataSetProperties(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("DataSet.DataSetName", this.DataSetName);
			info.AddValue("DataSet.Namespace", this.Namespace);
			info.AddValue("DataSet.Prefix", this.Prefix);
			info.AddValue("DataSet.CaseSensitive", this.CaseSensitive);
			info.AddValue("DataSet.LocaleLCID", this.Locale.LCID);
			info.AddValue("DataSet.EnforceConstraints", this.EnforceConstraints);
			info.AddValue("DataSet.ExtendedProperties", this.ExtendedProperties);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x001E4890 File Offset: 0x001E3C90
		private void DeserializeDataSetProperties(SerializationInfo info, StreamingContext context)
		{
			this.dataSetName = info.GetString("DataSet.DataSetName");
			this.namespaceURI = info.GetString("DataSet.Namespace");
			this._datasetPrefix = info.GetString("DataSet.Prefix");
			this._caseSensitive = info.GetBoolean("DataSet.CaseSensitive");
			int num = (int)info.GetValue("DataSet.LocaleLCID", typeof(int));
			this._culture = new CultureInfo(num);
			this._cultureUserSet = true;
			this.enforceConstraints = info.GetBoolean("DataSet.EnforceConstraints");
			this.extendedProperties = (PropertyCollection)info.GetValue("DataSet.ExtendedProperties", typeof(PropertyCollection));
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x001E4940 File Offset: 0x001E3D40
		private void SerializeRelations(SerializationInfo info, StreamingContext context)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.Relations)
			{
				DataRelation dataRelation = (DataRelation)obj;
				int[] array = new int[dataRelation.ParentColumns.Length + 1];
				array[0] = this.Tables.IndexOf(dataRelation.ParentTable);
				for (int i = 1; i < array.Length; i++)
				{
					array[i] = dataRelation.ParentColumns[i - 1].Ordinal;
				}
				int[] array2 = new int[dataRelation.ChildColumns.Length + 1];
				array2[0] = this.Tables.IndexOf(dataRelation.ChildTable);
				for (int j = 1; j < array2.Length; j++)
				{
					array2[j] = dataRelation.ChildColumns[j - 1].Ordinal;
				}
				arrayList.Add(new ArrayList { dataRelation.RelationName, array, array2, dataRelation.Nested, dataRelation.extendedProperties });
			}
			info.AddValue("DataSet.Relations", arrayList);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x001E4A9C File Offset: 0x001E3E9C
		private void DeserializeRelations(SerializationInfo info, StreamingContext context)
		{
			ArrayList arrayList = (ArrayList)info.GetValue("DataSet.Relations", typeof(ArrayList));
			foreach (object obj in arrayList)
			{
				ArrayList arrayList2 = (ArrayList)obj;
				string text = (string)arrayList2[0];
				int[] array = (int[])arrayList2[1];
				int[] array2 = (int[])arrayList2[2];
				bool flag = (bool)arrayList2[3];
				PropertyCollection propertyCollection = (PropertyCollection)arrayList2[4];
				DataColumn[] array3 = new DataColumn[array.Length - 1];
				for (int i = 0; i < array3.Length; i++)
				{
					array3[i] = this.Tables[array[0]].Columns[array[i + 1]];
				}
				DataColumn[] array4 = new DataColumn[array2.Length - 1];
				for (int j = 0; j < array4.Length; j++)
				{
					array4[j] = this.Tables[array2[0]].Columns[array2[j + 1]];
				}
				DataRelation dataRelation = new DataRelation(text, array3, array4, false);
				dataRelation.CheckMultipleNested = false;
				dataRelation.Nested = flag;
				dataRelation.extendedProperties = propertyCollection;
				this.Relations.Add(dataRelation);
				dataRelation.CheckMultipleNested = true;
			}
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x001E4C1C File Offset: 0x001E401C
		internal void FailedEnableConstraints()
		{
			this.EnforceConstraints = false;
			throw ExceptionBuilder.EnforceConstraint();
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x001E4C38 File Offset: 0x001E4038
		// (set) Token: 0x0600088E RID: 2190 RVA: 0x001E4C4C File Offset: 0x001E404C
		[ResCategory("DataCategory_Data")]
		[DefaultValue(false)]
		[ResDescription("DataSetCaseSensitiveDescr")]
		public bool CaseSensitive
		{
			get
			{
				return this._caseSensitive;
			}
			set
			{
				if (this._caseSensitive != value)
				{
					bool caseSensitive = this._caseSensitive;
					this._caseSensitive = value;
					if (!this.ValidateCaseConstraint())
					{
						this._caseSensitive = caseSensitive;
						throw ExceptionBuilder.CannotChangeCaseLocale();
					}
					foreach (object obj in this.Tables)
					{
						DataTable dataTable = (DataTable)obj;
						dataTable.SetCaseSensitiveValue(value, false, true);
					}
				}
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x001E4CE0 File Offset: 0x001E40E0
		bool IListSource.ContainsListCollection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x001E4CF0 File Offset: 0x001E40F0
		[ResDescription("DataSetDefaultViewDescr")]
		[Browsable(false)]
		public DataViewManager DefaultViewManager
		{
			get
			{
				if (this.defaultViewManager == null)
				{
					lock (this._defaultViewManagerLock)
					{
						if (this.defaultViewManager == null)
						{
							this.defaultViewManager = new DataViewManager(this, true);
						}
					}
				}
				return this.defaultViewManager;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x001E4D54 File Offset: 0x001E4154
		// (set) Token: 0x06000892 RID: 2194 RVA: 0x001E4D68 File Offset: 0x001E4168
		[DefaultValue(true)]
		[ResDescription("DataSetEnforceConstraintsDescr")]
		public bool EnforceConstraints
		{
			get
			{
				return this.enforceConstraints;
			}
			set
			{
				IntPtr intPtr;
				Bid.ScopeEnter(out intPtr, "<ds.DataSet.set_EnforceConstraints|API> %d#, %d{bool}\n", this.ObjectID, value);
				try
				{
					if (this.enforceConstraints != value)
					{
						if (value)
						{
							this.EnableConstraints();
						}
						this.enforceConstraints = value;
					}
				}
				finally
				{
					Bid.ScopeLeave(ref intPtr);
				}
			}
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x001E4DC8 File Offset: 0x001E41C8
		internal void RestoreEnforceConstraints(bool value)
		{
			this.enforceConstraints = value;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x001E4DDC File Offset: 0x001E41DC
		internal void EnableConstraints()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.EnableConstraints|INFO> %d#\n", this.ObjectID);
			try
			{
				bool flag = false;
				ConstraintEnumerator constraintEnumerator = new ConstraintEnumerator(this);
				while (constraintEnumerator.GetNext())
				{
					Constraint constraint = constraintEnumerator.GetConstraint();
					flag |= constraint.IsConstraintViolated();
				}
				foreach (object obj in this.Tables)
				{
					DataTable dataTable = (DataTable)obj;
					foreach (object obj2 in dataTable.Columns)
					{
						DataColumn dataColumn = (DataColumn)obj2;
						if (!dataColumn.AllowDBNull)
						{
							flag |= dataColumn.IsNotAllowDBNullViolated();
						}
						if (dataColumn.MaxLength >= 0)
						{
							flag |= dataColumn.IsMaxLengthViolated();
						}
					}
				}
				if (flag)
				{
					this.FailedEnableConstraints();
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x001E4F18 File Offset: 0x001E4318
		// (set) Token: 0x06000896 RID: 2198 RVA: 0x001E4F2C File Offset: 0x001E432C
		[ResDescription("DataSetDataSetNameDescr")]
		[ResCategory("DataCategory_Data")]
		[DefaultValue("")]
		public string DataSetName
		{
			get
			{
				return this.dataSetName;
			}
			set
			{
				Bid.Trace("<ds.DataSet.set_DataSetName|API> %d#, '%ls'\n", this.ObjectID, value);
				if (value != this.dataSetName)
				{
					if (value == null || value.Length == 0)
					{
						throw ExceptionBuilder.SetDataSetNameToEmpty();
					}
					DataTable dataTable = this.Tables[value, this.Namespace];
					if (dataTable != null && !dataTable.fNestedInDataset)
					{
						throw ExceptionBuilder.SetDataSetNameConflicting(value);
					}
					this.RaisePropertyChanging("DataSetName");
					this.dataSetName = value;
				}
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x001E4FA0 File Offset: 0x001E43A0
		// (set) Token: 0x06000898 RID: 2200 RVA: 0x001E4FB4 File Offset: 0x001E43B4
		[ResDescription("DataSetNamespaceDescr")]
		[ResCategory("DataCategory_Data")]
		[DefaultValue("")]
		public string Namespace
		{
			get
			{
				return this.namespaceURI;
			}
			set
			{
				Bid.Trace("<ds.DataSet.set_Namespace|API> %d#, '%ls'\n", this.ObjectID, value);
				if (value == null)
				{
					value = string.Empty;
				}
				if (value != this.namespaceURI)
				{
					this.RaisePropertyChanging("Namespace");
					foreach (object obj in this.Tables)
					{
						DataTable dataTable = (DataTable)obj;
						if (dataTable.tableNamespace == null && (dataTable.NestedParentRelations.Length == 0 || (dataTable.NestedParentRelations.Length == 1 && dataTable.NestedParentRelations[0].ChildTable == dataTable)))
						{
							if (this.Tables.Contains(dataTable.TableName, value, false, true))
							{
								throw ExceptionBuilder.DuplicateTableName2(dataTable.TableName, value);
							}
							dataTable.CheckCascadingNamespaceConflict(value);
							dataTable.DoRaiseNamespaceChange();
						}
					}
					this.namespaceURI = value;
					if (ADP.IsEmpty(value))
					{
						this._datasetPrefix = string.Empty;
					}
				}
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x001E50C0 File Offset: 0x001E44C0
		// (set) Token: 0x0600089A RID: 2202 RVA: 0x001E50D4 File Offset: 0x001E44D4
		[DefaultValue("")]
		[ResCategory("DataCategory_Data")]
		[ResDescription("DataSetPrefixDescr")]
		public string Prefix
		{
			get
			{
				return this._datasetPrefix;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (XmlConvert.DecodeName(value) == value && XmlConvert.EncodeName(value) != value)
				{
					throw ExceptionBuilder.InvalidPrefix(value);
				}
				if (value != this._datasetPrefix)
				{
					this.RaisePropertyChanging("Prefix");
					this._datasetPrefix = value;
				}
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x001E5130 File Offset: 0x001E4530
		[ResDescription("ExtendedPropertiesDescr")]
		[ResCategory("DataCategory_Data")]
		[Browsable(false)]
		public PropertyCollection ExtendedProperties
		{
			get
			{
				if (this.extendedProperties == null)
				{
					this.extendedProperties = new PropertyCollection();
				}
				return this.extendedProperties;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x001E5158 File Offset: 0x001E4558
		[ResDescription("DataSetHasErrorsDescr")]
		[Browsable(false)]
		public bool HasErrors
		{
			get
			{
				for (int i = 0; i < this.Tables.Count; i++)
				{
					if (this.Tables[i].HasErrors)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x001E5194 File Offset: 0x001E4594
		[Browsable(false)]
		public bool IsInitialized
		{
			get
			{
				return !this.fInitInProgress;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x001E51AC File Offset: 0x001E45AC
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x001E51C0 File Offset: 0x001E45C0
		[ResCategory("DataCategory_Data")]
		[ResDescription("DataSetLocaleDescr")]
		public CultureInfo Locale
		{
			get
			{
				return this._culture;
			}
			set
			{
				IntPtr intPtr;
				Bid.ScopeEnter(out intPtr, "<ds.DataSet.set_Locale|API> %d#\n", this.ObjectID);
				try
				{
					if (value != null)
					{
						if (!this._culture.Equals(value))
						{
							this.SetLocaleValue(value, true);
						}
						this._cultureUserSet = true;
					}
				}
				finally
				{
					Bid.ScopeLeave(ref intPtr);
				}
			}
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x001E5228 File Offset: 0x001E4628
		internal void SetLocaleValue(CultureInfo value, bool userSet)
		{
			bool flag = false;
			bool flag2 = false;
			int num = 0;
			CultureInfo culture = this._culture;
			bool cultureUserSet = this._cultureUserSet;
			try
			{
				this._culture = value;
				this._cultureUserSet = userSet;
				foreach (object obj in this.Tables)
				{
					DataTable dataTable = (DataTable)obj;
					if (!dataTable.ShouldSerializeLocale())
					{
						dataTable.SetLocaleValue(value, false, false);
					}
				}
				flag = this.ValidateLocaleConstraint();
				if (flag)
				{
					flag = false;
					foreach (object obj2 in this.Tables)
					{
						DataTable dataTable2 = (DataTable)obj2;
						num++;
						if (!dataTable2.ShouldSerializeLocale())
						{
							dataTable2.SetLocaleValue(value, false, true);
						}
					}
					flag = true;
				}
			}
			catch
			{
				flag2 = true;
				throw;
			}
			finally
			{
				if (!flag)
				{
					this._culture = culture;
					this._cultureUserSet = cultureUserSet;
					foreach (object obj3 in this.Tables)
					{
						DataTable dataTable3 = (DataTable)obj3;
						if (!dataTable3.ShouldSerializeLocale())
						{
							dataTable3.SetLocaleValue(culture, false, false);
						}
					}
					try
					{
						for (int i = 0; i < num; i++)
						{
							if (!this.Tables[i].ShouldSerializeLocale())
							{
								this.Tables[i].SetLocaleValue(culture, false, true);
							}
						}
					}
					catch (Exception ex)
					{
						if (!ADP.IsCatchableExceptionType(ex))
						{
							throw;
						}
						ADP.TraceExceptionWithoutRethrow(ex);
					}
					if (!flag2)
					{
						throw ExceptionBuilder.CannotChangeCaseLocale(null);
					}
				}
			}
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x001E5470 File Offset: 0x001E4870
		internal bool ShouldSerializeLocale()
		{
			return this._cultureUserSet;
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x001E5484 File Offset: 0x001E4884
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x001E5498 File Offset: 0x001E4898
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				ISite site = this.Site;
				if (value == null && site != null)
				{
					IContainer container = site.Container;
					if (container != null)
					{
						for (int i = 0; i < this.Tables.Count; i++)
						{
							if (this.Tables[i].Site != null)
							{
								container.Remove(this.Tables[i]);
							}
						}
					}
				}
				base.Site = value;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x001E5500 File Offset: 0x001E4900
		[ResDescription("DataSetRelationsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[ResCategory("DataCategory_Data")]
		public DataRelationCollection Relations
		{
			get
			{
				return this.relationCollection;
			}
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x001E5514 File Offset: 0x001E4914
		protected virtual bool ShouldSerializeRelations()
		{
			return true;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x001E5524 File Offset: 0x001E4924
		private void ResetRelations()
		{
			this.Relations.Clear();
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x001E553C File Offset: 0x001E493C
		[ResCategory("DataCategory_Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[ResDescription("DataSetTablesDescr")]
		public DataTableCollection Tables
		{
			get
			{
				return this.tableCollection;
			}
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x001E5550 File Offset: 0x001E4950
		protected virtual bool ShouldSerializeTables()
		{
			return true;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x001E5560 File Offset: 0x001E4960
		private void ResetTables()
		{
			this.Tables.Clear();
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x001E5578 File Offset: 0x001E4978
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x001E558C File Offset: 0x001E498C
		internal bool FBoundToDocument
		{
			get
			{
				return this.fBoundToDocument;
			}
			set
			{
				this.fBoundToDocument = value;
			}
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x001E55A0 File Offset: 0x001E49A0
		public void AcceptChanges()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.AcceptChanges|API> %d#\n", this.ObjectID);
			try
			{
				for (int i = 0; i < this.Tables.Count; i++)
				{
					this.Tables[i].AcceptChanges();
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060008AD RID: 2221 RVA: 0x001E560C File Offset: 0x001E4A0C
		// (remove) Token: 0x060008AE RID: 2222 RVA: 0x001E5630 File Offset: 0x001E4A30
		internal event PropertyChangedEventHandler PropertyChanging
		{
			add
			{
				this.onPropertyChangingDelegate = (PropertyChangedEventHandler)Delegate.Combine(this.onPropertyChangingDelegate, value);
			}
			remove
			{
				this.onPropertyChangingDelegate = (PropertyChangedEventHandler)Delegate.Remove(this.onPropertyChangingDelegate, value);
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060008AF RID: 2223 RVA: 0x001E5654 File Offset: 0x001E4A54
		// (remove) Token: 0x060008B0 RID: 2224 RVA: 0x001E5678 File Offset: 0x001E4A78
		[ResDescription("DataSetMergeFailedDescr")]
		[ResCategory("DataCategory_Action")]
		public event MergeFailedEventHandler MergeFailed
		{
			add
			{
				this.onMergeFailed = (MergeFailedEventHandler)Delegate.Combine(this.onMergeFailed, value);
			}
			remove
			{
				this.onMergeFailed = (MergeFailedEventHandler)Delegate.Remove(this.onMergeFailed, value);
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060008B1 RID: 2225 RVA: 0x001E569C File Offset: 0x001E4A9C
		// (remove) Token: 0x060008B2 RID: 2226 RVA: 0x001E56C0 File Offset: 0x001E4AC0
		internal event DataRowCreatedEventHandler DataRowCreated
		{
			add
			{
				this.onDataRowCreated = (DataRowCreatedEventHandler)Delegate.Combine(this.onDataRowCreated, value);
			}
			remove
			{
				this.onDataRowCreated = (DataRowCreatedEventHandler)Delegate.Remove(this.onDataRowCreated, value);
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060008B3 RID: 2227 RVA: 0x001E56E4 File Offset: 0x001E4AE4
		// (remove) Token: 0x060008B4 RID: 2228 RVA: 0x001E5708 File Offset: 0x001E4B08
		internal event DataSetClearEventhandler ClearFunctionCalled
		{
			add
			{
				this.onClearFunctionCalled = (DataSetClearEventhandler)Delegate.Combine(this.onClearFunctionCalled, value);
			}
			remove
			{
				this.onClearFunctionCalled = (DataSetClearEventhandler)Delegate.Remove(this.onClearFunctionCalled, value);
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060008B5 RID: 2229 RVA: 0x001E572C File Offset: 0x001E4B2C
		// (remove) Token: 0x060008B6 RID: 2230 RVA: 0x001E5750 File Offset: 0x001E4B50
		[ResDescription("DataSetInitializedDescr")]
		[ResCategory("DataCategory_Action")]
		public event EventHandler Initialized
		{
			add
			{
				this.onInitialized = (EventHandler)Delegate.Combine(this.onInitialized, value);
			}
			remove
			{
				this.onInitialized = (EventHandler)Delegate.Remove(this.onInitialized, value);
			}
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x001E5774 File Offset: 0x001E4B74
		public void BeginInit()
		{
			this.fInitInProgress = true;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x001E5788 File Offset: 0x001E4B88
		public void EndInit()
		{
			this.Tables.FinishInitCollection();
			for (int i = 0; i < this.Tables.Count; i++)
			{
				this.Tables[i].Columns.FinishInitCollection();
			}
			for (int j = 0; j < this.Tables.Count; j++)
			{
				this.Tables[j].Constraints.FinishInitConstraints();
			}
			((DataRelationCollection.DataSetRelationCollection)this.Relations).FinishInitRelations();
			this.fInitInProgress = false;
			this.OnInitialized();
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x001E5818 File Offset: 0x001E4C18
		public void Clear()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.Clear|API> %d#\n", this.ObjectID);
			try
			{
				this.OnClearFunctionCalled(null);
				bool flag = this.EnforceConstraints;
				this.EnforceConstraints = false;
				for (int i = 0; i < this.Tables.Count; i++)
				{
					this.Tables[i].Clear();
				}
				this.EnforceConstraints = flag;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x001E58A0 File Offset: 0x001E4CA0
		public virtual DataSet Clone()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.Clone|API> %d#\n", this.ObjectID);
			DataSet dataSet2;
			try
			{
				DataSet dataSet = (DataSet)Activator.CreateInstance(base.GetType(), true);
				if (dataSet.Tables.Count > 0)
				{
					dataSet.Reset();
				}
				dataSet.DataSetName = this.DataSetName;
				dataSet.CaseSensitive = this.CaseSensitive;
				dataSet._culture = this._culture;
				dataSet._cultureUserSet = this._cultureUserSet;
				dataSet.EnforceConstraints = this.EnforceConstraints;
				dataSet.Namespace = this.Namespace;
				dataSet.Prefix = this.Prefix;
				dataSet.RemotingFormat = this.RemotingFormat;
				dataSet.fIsSchemaLoading = true;
				DataTableCollection tables = this.Tables;
				for (int i = 0; i < tables.Count; i++)
				{
					DataTable dataTable = tables[i].Clone(dataSet);
					dataTable.tableNamespace = tables[i].Namespace;
					dataSet.Tables.Add(dataTable);
				}
				for (int j = 0; j < tables.Count; j++)
				{
					ConstraintCollection constraints = tables[j].Constraints;
					for (int k = 0; k < constraints.Count; k++)
					{
						if (!(constraints[k] is UniqueConstraint))
						{
							ForeignKeyConstraint foreignKeyConstraint = constraints[k] as ForeignKeyConstraint;
							if (foreignKeyConstraint.Table != foreignKeyConstraint.RelatedTable)
							{
								dataSet.Tables[j].Constraints.Add(constraints[k].Clone(dataSet));
							}
						}
					}
				}
				DataRelationCollection relations = this.Relations;
				for (int l = 0; l < relations.Count; l++)
				{
					DataRelation dataRelation = relations[l].Clone(dataSet);
					dataRelation.CheckMultipleNested = false;
					dataSet.Relations.Add(dataRelation);
					dataRelation.CheckMultipleNested = true;
				}
				if (this.extendedProperties != null)
				{
					foreach (object obj in this.extendedProperties.Keys)
					{
						dataSet.ExtendedProperties[obj] = this.extendedProperties[obj];
					}
				}
				foreach (object obj2 in this.Tables)
				{
					DataTable dataTable2 = (DataTable)obj2;
					foreach (object obj3 in dataTable2.Columns)
					{
						DataColumn dataColumn = (DataColumn)obj3;
						if (dataColumn.Expression.Length != 0)
						{
							dataSet.Tables[dataTable2.TableName, dataTable2.Namespace].Columns[dataColumn.ColumnName].Expression = dataColumn.Expression;
						}
					}
				}
				for (int m = 0; m < tables.Count; m++)
				{
					dataSet.Tables[m].tableNamespace = tables[m].tableNamespace;
				}
				dataSet.fIsSchemaLoading = false;
				dataSet2 = dataSet;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataSet2;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x001E5C40 File Offset: 0x001E5040
		public DataSet Copy()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.Copy|API> %d#\n", this.ObjectID);
			DataSet dataSet2;
			try
			{
				DataSet dataSet = this.Clone();
				bool flag = dataSet.EnforceConstraints;
				dataSet.EnforceConstraints = false;
				foreach (object obj in this.Tables)
				{
					DataTable dataTable = (DataTable)obj;
					DataTable dataTable2 = dataSet.Tables[dataTable.TableName, dataTable.Namespace];
					foreach (object obj2 in dataTable.Rows)
					{
						DataRow dataRow = (DataRow)obj2;
						dataTable.CopyRow(dataTable2, dataRow);
					}
				}
				dataSet.EnforceConstraints = flag;
				dataSet2 = dataSet;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataSet2;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x001E5D6C File Offset: 0x001E516C
		internal int EstimatedXmlStringSize()
		{
			int num = 100;
			for (int i = 0; i < this.Tables.Count; i++)
			{
				int num2 = this.Tables[i].TableName.Length + 4 << 2;
				DataTable dataTable = this.Tables[i];
				for (int j = 0; j < dataTable.Columns.Count; j++)
				{
					num2 += dataTable.Columns[j].ColumnName.Length + 4 << 2;
					num2 += 20;
				}
				num += dataTable.Rows.Count * num2;
			}
			return num;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x001E5E10 File Offset: 0x001E5210
		public DataSet GetChanges()
		{
			return this.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified);
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x001E5E28 File Offset: 0x001E5228
		public DataSet GetChanges(DataRowState rowStates)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.GetChanges|API> %d#, rowStates=%d{ds.DataRowState}\n", this.ObjectID, (int)rowStates);
			DataSet dataSet2;
			try
			{
				DataSet dataSet = null;
				bool flag = false;
				if ((rowStates & ~(DataRowState.Unchanged | DataRowState.Added | DataRowState.Deleted | DataRowState.Modified)) != (DataRowState)0)
				{
					throw ExceptionBuilder.InvalidRowState(rowStates);
				}
				DataSet.TableChanges[] array = new DataSet.TableChanges[this.Tables.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new DataSet.TableChanges(this.Tables[i].Rows.Count);
				}
				this.MarkModifiedRows(array, rowStates);
				for (int j = 0; j < array.Length; j++)
				{
					if (0 < array[j].HasChanges)
					{
						if (dataSet == null)
						{
							dataSet = this.Clone();
							flag = dataSet.EnforceConstraints;
							dataSet.EnforceConstraints = false;
						}
						DataTable dataTable = this.Tables[j];
						DataTable dataTable2 = dataSet.Tables[dataTable.TableName, dataTable.Namespace];
						int num = 0;
						while (0 < array[j].HasChanges)
						{
							if (array[j][num])
							{
								dataTable.CopyRow(dataTable2, dataTable.Rows[num]);
								DataSet.TableChanges[] array2 = array;
								int num2 = j;
								array2[num2].HasChanges = array2[num2].HasChanges - 1;
							}
							num++;
						}
					}
				}
				if (dataSet != null)
				{
					dataSet.EnforceConstraints = flag;
				}
				dataSet2 = dataSet;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataSet2;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x001E5F9C File Offset: 0x001E539C
		private void MarkModifiedRows(DataSet.TableChanges[] bitMatrix, DataRowState rowStates)
		{
			for (int i = 0; i < bitMatrix.Length; i++)
			{
				DataRowCollection rows = this.Tables[i].Rows;
				int count = rows.Count;
				for (int j = 0; j < count; j++)
				{
					DataRow dataRow = rows[j];
					DataRowState rowState = dataRow.RowState;
					if ((rowStates & rowState) != (DataRowState)0 && !bitMatrix[i][j])
					{
						bitMatrix[i][j] = true;
						if (DataRowState.Deleted != rowState)
						{
							this.MarkRelatedRowsAsModified(bitMatrix, dataRow);
						}
					}
				}
			}
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x001E6020 File Offset: 0x001E5420
		private void MarkRelatedRowsAsModified(DataSet.TableChanges[] bitMatrix, DataRow row)
		{
			DataRelationCollection parentRelations = row.Table.ParentRelations;
			int count = parentRelations.Count;
			for (int i = 0; i < count; i++)
			{
				DataRow[] parentRows = row.GetParentRows(parentRelations[i], DataRowVersion.Current);
				foreach (DataRow dataRow in parentRows)
				{
					int num = this.Tables.IndexOf(dataRow.Table);
					int num2 = dataRow.Table.Rows.IndexOf(dataRow);
					if (!bitMatrix[num][num2])
					{
						bitMatrix[num][num2] = true;
						if (DataRowState.Deleted != dataRow.RowState)
						{
							this.MarkRelatedRowsAsModified(bitMatrix, dataRow);
						}
					}
				}
			}
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x001E60D8 File Offset: 0x001E54D8
		IList IListSource.GetList()
		{
			return this.DefaultViewManager;
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x001E60EC File Offset: 0x001E54EC
		internal string GetRemotingDiffGram(DataTable table)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			if (stringWriter != null)
			{
				new NewDiffgramGen(table, false).Save(xmlTextWriter, table);
			}
			return stringWriter.ToString();
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x001E612C File Offset: 0x001E552C
		public string GetXml()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.GetXml|API> %d#\n", this.ObjectID);
			string text;
			try
			{
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				if (stringWriter != null)
				{
					XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
					xmlTextWriter.Formatting = Formatting.Indented;
					new XmlDataTreeWriter(this).Save(xmlTextWriter, false);
				}
				text = stringWriter.ToString();
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return text;
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x001E61A4 File Offset: 0x001E55A4
		public string GetXmlSchema()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.GetXmlSchema|API> %d#\n", this.ObjectID);
			string text;
			try
			{
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
				xmlTextWriter.Formatting = Formatting.Indented;
				if (stringWriter != null)
				{
					new XmlTreeGen(SchemaFormat.Public).Save(this, xmlTextWriter);
				}
				text = stringWriter.ToString();
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return text;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x001E621C File Offset: 0x001E561C
		internal string GetXmlSchemaForRemoting(DataTable table)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			if (stringWriter != null)
			{
				if (table == null)
				{
					if (this.SchemaSerializationMode == SchemaSerializationMode.ExcludeSchema)
					{
						new XmlTreeGen(SchemaFormat.RemotingSkipSchema).Save(this, xmlTextWriter);
					}
					else
					{
						new XmlTreeGen(SchemaFormat.Remoting).Save(this, xmlTextWriter);
					}
				}
				else
				{
					new XmlTreeGen(SchemaFormat.Remoting).Save(table, xmlTextWriter);
				}
			}
			return stringWriter.ToString();
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x001E6284 File Offset: 0x001E5684
		public bool HasChanges()
		{
			return this.HasChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x001E629C File Offset: 0x001E569C
		public bool HasChanges(DataRowState rowStates)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.HasChanges|API> %d#, rowStates=%d{ds.DataRowState}\n", this.ObjectID, (int)rowStates);
			bool flag;
			try
			{
				if ((rowStates & ~(DataRowState.Detached | DataRowState.Unchanged | DataRowState.Added | DataRowState.Deleted | DataRowState.Modified)) != (DataRowState)0)
				{
					throw ExceptionBuilder.ArgumentOutOfRange("rowState");
				}
				for (int i = 0; i < this.Tables.Count; i++)
				{
					DataTable dataTable = this.Tables[i];
					for (int j = 0; j < dataTable.Rows.Count; j++)
					{
						DataRow dataRow = dataTable.Rows[j];
						if ((dataRow.RowState & rowStates) != (DataRowState)0)
						{
							return true;
						}
					}
				}
				flag = false;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return flag;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x001E634C File Offset: 0x001E574C
		public void InferXmlSchema(XmlReader reader, string[] nsArray)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.InferXmlSchema|API> %d#\n", this.ObjectID);
			try
			{
				if (reader != null)
				{
					XmlDocument xmlDocument = new XmlDocument();
					if (reader.NodeType == XmlNodeType.Element)
					{
						XmlNode xmlNode = xmlDocument.ReadNode(reader);
						xmlDocument.AppendChild(xmlNode);
					}
					else
					{
						xmlDocument.Load(reader);
					}
					if (xmlDocument.DocumentElement != null)
					{
						this.InferSchema(xmlDocument, nsArray, XmlReadMode.InferSchema);
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x001E63D4 File Offset: 0x001E57D4
		public void InferXmlSchema(Stream stream, string[] nsArray)
		{
			if (stream == null)
			{
				return;
			}
			this.InferXmlSchema(new XmlTextReader(stream), nsArray);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x001E63F4 File Offset: 0x001E57F4
		public void InferXmlSchema(TextReader reader, string[] nsArray)
		{
			if (reader == null)
			{
				return;
			}
			this.InferXmlSchema(new XmlTextReader(reader), nsArray);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x001E6414 File Offset: 0x001E5814
		public void InferXmlSchema(string fileName, string[] nsArray)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(fileName);
			try
			{
				this.InferXmlSchema(xmlTextReader, nsArray);
			}
			finally
			{
				xmlTextReader.Close();
			}
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x001E6458 File Offset: 0x001E5858
		public void ReadXmlSchema(XmlReader reader)
		{
			this.ReadXmlSchema(reader, false);
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x001E6470 File Offset: 0x001E5870
		internal void ReadXmlSchema(XmlReader reader, bool denyResolving)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.ReadXmlSchema|INFO> %d#, reader, denyResolving=%d{bool}\n", this.ObjectID, denyResolving);
			try
			{
				int num = -1;
				if (reader != null)
				{
					if (reader is XmlTextReader)
					{
						((XmlTextReader)reader).WhitespaceHandling = WhitespaceHandling.None;
					}
					XmlDocument xmlDocument = new XmlDocument();
					if (reader.NodeType == XmlNodeType.Element)
					{
						num = reader.Depth;
					}
					reader.MoveToContent();
					if (reader.NodeType == XmlNodeType.Element)
					{
						if (reader.LocalName == "Schema" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-data")
						{
							this.ReadXDRSchema(reader);
						}
						else if (reader.LocalName == "schema" && reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
						{
							this.ReadXSDSchema(reader, denyResolving);
						}
						else
						{
							if (reader.LocalName == "schema" && reader.NamespaceURI.StartsWith("http://www.w3.org/", StringComparison.Ordinal))
							{
								throw ExceptionBuilder.DataSetUnsupportedSchema("http://www.w3.org/2001/XMLSchema");
							}
							XmlElement xmlElement = xmlDocument.CreateElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
							if (reader.HasAttributes)
							{
								int attributeCount = reader.AttributeCount;
								for (int i = 0; i < attributeCount; i++)
								{
									reader.MoveToAttribute(i);
									if (reader.NamespaceURI.Equals("http://www.w3.org/2000/xmlns/"))
									{
										xmlElement.SetAttribute(reader.Name, reader.GetAttribute(i));
									}
									else
									{
										XmlAttribute xmlAttribute = xmlElement.SetAttributeNode(reader.LocalName, reader.NamespaceURI);
										xmlAttribute.Prefix = reader.Prefix;
										xmlAttribute.Value = reader.GetAttribute(i);
									}
								}
							}
							reader.Read();
							while (this.MoveToElement(reader, num))
							{
								if (reader.LocalName == "Schema" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-data")
								{
									this.ReadXDRSchema(reader);
									return;
								}
								if (reader.LocalName == "schema" && reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
								{
									this.ReadXSDSchema(reader, denyResolving);
									return;
								}
								if (reader.LocalName == "schema" && reader.NamespaceURI.StartsWith("http://www.w3.org/", StringComparison.Ordinal))
								{
									throw ExceptionBuilder.DataSetUnsupportedSchema("http://www.w3.org/2001/XMLSchema");
								}
								XmlNode xmlNode = xmlDocument.ReadNode(reader);
								xmlElement.AppendChild(xmlNode);
							}
							this.ReadEndElement(reader);
							xmlDocument.AppendChild(xmlElement);
							this.InferSchema(xmlDocument, null, XmlReadMode.Auto);
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x001E66F4 File Offset: 0x001E5AF4
		internal bool MoveToElement(XmlReader reader, int depth)
		{
			while (!reader.EOF && reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.Element && reader.Depth > depth)
			{
				reader.Read();
			}
			return reader.NodeType == XmlNodeType.Element;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x001E6738 File Offset: 0x001E5B38
		private static void MoveToElement(XmlReader reader)
		{
			while (!reader.EOF && reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.Element)
			{
				reader.Read();
			}
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x001E676C File Offset: 0x001E5B6C
		internal void ReadEndElement(XmlReader reader)
		{
			while (reader.NodeType == XmlNodeType.Whitespace)
			{
				reader.Skip();
			}
			if (reader.NodeType == XmlNodeType.None)
			{
				reader.Skip();
				return;
			}
			if (reader.NodeType == XmlNodeType.EndElement)
			{
				reader.ReadEndElement();
			}
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x001E67AC File Offset: 0x001E5BAC
		internal void ReadXSDSchema(XmlReader reader, bool denyResolving)
		{
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
			int num = 1;
			if (reader.LocalName == "schema" && reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema" && reader.HasAttributes)
			{
				string attribute = reader.GetAttribute("schemafragmentcount", "urn:schemas-microsoft-com:xml-msdata");
				if (!ADP.IsEmpty(attribute))
				{
					num = int.Parse(attribute, null);
				}
			}
			while (reader.LocalName == "schema" && reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
			{
				XmlSchema xmlSchema = XmlSchema.Read(reader, null);
				xmlSchemaSet.Add(xmlSchema);
				this.ReadEndElement(reader);
				if (--num > 0)
				{
					DataSet.MoveToElement(reader);
				}
				while (reader.NodeType == XmlNodeType.Whitespace)
				{
					reader.Skip();
				}
			}
			xmlSchemaSet.Compile();
			XSDSchema xsdschema = new XSDSchema();
			xsdschema.LoadSchema(xmlSchemaSet, this);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x001E6884 File Offset: 0x001E5C84
		internal void ReadXDRSchema(XmlReader reader)
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlNode xmlNode = xmlDocument.ReadNode(reader);
			xmlDocument.AppendChild(xmlNode);
			XDRSchema xdrschema = new XDRSchema(this, false);
			this.DataSetName = xmlDocument.DocumentElement.LocalName;
			xdrschema.LoadSchema((XmlElement)xmlNode, this);
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x001E68D0 File Offset: 0x001E5CD0
		public void ReadXmlSchema(Stream stream)
		{
			if (stream == null)
			{
				return;
			}
			this.ReadXmlSchema(new XmlTextReader(stream), false);
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x001E68F0 File Offset: 0x001E5CF0
		public void ReadXmlSchema(TextReader reader)
		{
			if (reader == null)
			{
				return;
			}
			this.ReadXmlSchema(new XmlTextReader(reader), false);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x001E6910 File Offset: 0x001E5D10
		public void ReadXmlSchema(string fileName)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(fileName);
			try
			{
				this.ReadXmlSchema(xmlTextReader, false);
			}
			finally
			{
				xmlTextReader.Close();
			}
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x001E6954 File Offset: 0x001E5D54
		public void WriteXmlSchema(Stream stream)
		{
			if (stream == null)
			{
				return;
			}
			this.WriteXmlSchema(new XmlTextWriter(stream, null)
			{
				Formatting = Formatting.Indented
			});
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x001E697C File Offset: 0x001E5D7C
		public void WriteXmlSchema(TextWriter writer)
		{
			if (writer == null)
			{
				return;
			}
			this.WriteXmlSchema(new XmlTextWriter(writer)
			{
				Formatting = Formatting.Indented
			});
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x001E69A4 File Offset: 0x001E5DA4
		public void WriteXmlSchema(XmlWriter writer)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.WriteXmlSchema|API> %d#\n", this.ObjectID);
			try
			{
				this.WriteXmlSchema(writer, SchemaFormat.Public);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x001E69F4 File Offset: 0x001E5DF4
		private void WriteXmlSchema(XmlWriter writer, SchemaFormat schemaFormat)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.WriteXmlSchema|INFO> %d#, schemaFormat=%d{ds.SchemaFormat}\n", this.ObjectID, (int)schemaFormat);
			try
			{
				if (writer != null)
				{
					XmlTreeGen xmlTreeGen;
					if (schemaFormat == SchemaFormat.WebService && this.SchemaSerializationMode == SchemaSerializationMode.ExcludeSchema && writer.WriteState == WriteState.Element)
					{
						xmlTreeGen = new XmlTreeGen(SchemaFormat.WebServiceSkipSchema);
					}
					else
					{
						xmlTreeGen = new XmlTreeGen(schemaFormat);
					}
					xmlTreeGen.Save(this, writer);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x001E6A70 File Offset: 0x001E5E70
		public void WriteXmlSchema(string fileName)
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(fileName, null);
			try
			{
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.WriteStartDocument(true);
				this.WriteXmlSchema(xmlTextWriter);
				xmlTextWriter.WriteEndDocument();
			}
			finally
			{
				xmlTextWriter.Close();
			}
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x001E6AC8 File Offset: 0x001E5EC8
		public XmlReadMode ReadXml(XmlReader reader)
		{
			return this.ReadXml(reader, false);
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x001E6AE0 File Offset: 0x001E5EE0
		internal XmlReadMode ReadXml(XmlReader reader, bool denyResolving)
		{
			IDisposable disposable = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.ReadXml|INFO> %d#, denyResolving=%d{bool}\n", this.ObjectID, denyResolving);
			XmlReadMode xmlReadMode2;
			try
			{
				disposable = TypeLimiter.EnterRestrictedScope(this);
				try
				{
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					int num = -1;
					XmlReadMode xmlReadMode = XmlReadMode.Auto;
					bool flag5 = false;
					bool flag6 = false;
					for (int i = 0; i < this.Tables.Count; i++)
					{
						this.Tables[i].rowDiffId = null;
					}
					if (reader == null)
					{
						xmlReadMode2 = xmlReadMode;
					}
					else
					{
						if (this.Tables.Count == 0)
						{
							flag5 = true;
						}
						if (reader is XmlTextReader)
						{
							((XmlTextReader)reader).WhitespaceHandling = WhitespaceHandling.Significant;
						}
						XmlDocument xmlDocument = new XmlDocument();
						XmlDataLoader xmlDataLoader = null;
						reader.MoveToContent();
						if (reader.NodeType == XmlNodeType.Element)
						{
							num = reader.Depth;
						}
						if (reader.NodeType == XmlNodeType.Element)
						{
							if (reader.LocalName == "diffgram" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1")
							{
								this.ReadXmlDiffgram(reader);
								this.ReadEndElement(reader);
								return XmlReadMode.DiffGram;
							}
							if (reader.LocalName == "Schema" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-data")
							{
								this.ReadXDRSchema(reader);
								return XmlReadMode.ReadSchema;
							}
							if (reader.LocalName == "schema" && reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
							{
								this.ReadXSDSchema(reader, denyResolving);
								return XmlReadMode.ReadSchema;
							}
							if (reader.LocalName == "schema" && reader.NamespaceURI.StartsWith("http://www.w3.org/", StringComparison.Ordinal))
							{
								throw ExceptionBuilder.DataSetUnsupportedSchema("http://www.w3.org/2001/XMLSchema");
							}
							XmlElement xmlElement = xmlDocument.CreateElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
							if (reader.HasAttributes)
							{
								int attributeCount = reader.AttributeCount;
								for (int j = 0; j < attributeCount; j++)
								{
									reader.MoveToAttribute(j);
									if (reader.NamespaceURI.Equals("http://www.w3.org/2000/xmlns/"))
									{
										xmlElement.SetAttribute(reader.Name, reader.GetAttribute(j));
									}
									else
									{
										XmlAttribute xmlAttribute = xmlElement.SetAttributeNode(reader.LocalName, reader.NamespaceURI);
										xmlAttribute.Prefix = reader.Prefix;
										xmlAttribute.Value = reader.GetAttribute(j);
									}
								}
							}
							reader.Read();
							string value = reader.Value;
							while (this.MoveToElement(reader, num))
							{
								if (reader.LocalName == "diffgram" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1")
								{
									this.ReadXmlDiffgram(reader);
									xmlReadMode = XmlReadMode.DiffGram;
								}
								if (!flag2 && !flag && reader.LocalName == "Schema" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-data")
								{
									this.ReadXDRSchema(reader);
									flag2 = true;
									flag4 = true;
								}
								else if (reader.LocalName == "schema" && reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
								{
									this.ReadXSDSchema(reader, denyResolving);
									flag2 = true;
								}
								else
								{
									if (reader.LocalName == "schema" && reader.NamespaceURI.StartsWith("http://www.w3.org/", StringComparison.Ordinal))
									{
										throw ExceptionBuilder.DataSetUnsupportedSchema("http://www.w3.org/2001/XMLSchema");
									}
									if (reader.LocalName == "diffgram" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1")
									{
										this.ReadXmlDiffgram(reader);
										flag3 = true;
										xmlReadMode = XmlReadMode.DiffGram;
									}
									else
									{
										while (!reader.EOF && reader.NodeType == XmlNodeType.Whitespace)
										{
											reader.Read();
										}
										if (reader.NodeType == XmlNodeType.Element)
										{
											flag = true;
											if (!flag2 && this.Tables.Count == 0)
											{
												XmlNode xmlNode = xmlDocument.ReadNode(reader);
												xmlElement.AppendChild(xmlNode);
											}
											else
											{
												if (xmlDataLoader == null)
												{
													xmlDataLoader = new XmlDataLoader(this, flag4, xmlElement, false);
												}
												xmlDataLoader.LoadData(reader);
												flag6 = true;
												if (flag2)
												{
													xmlReadMode = XmlReadMode.ReadSchema;
												}
												else
												{
													xmlReadMode = XmlReadMode.IgnoreSchema;
												}
											}
										}
									}
								}
							}
							this.ReadEndElement(reader);
							bool flag7 = false;
							bool flag8 = this.fTopLevelTable;
							if (!flag2 && this.Tables.Count == 0 && !xmlElement.HasChildNodes)
							{
								this.fTopLevelTable = true;
								flag7 = true;
								if (value != null && value.Length > 0)
								{
									xmlElement.InnerText = value;
								}
							}
							if (!flag5 && value != null && value.Length > 0)
							{
								xmlElement.InnerText = value;
							}
							xmlDocument.AppendChild(xmlElement);
							if (xmlDataLoader == null)
							{
								xmlDataLoader = new XmlDataLoader(this, flag4, xmlElement, false);
							}
							if (!flag5 && !flag6)
							{
								XmlElement documentElement = xmlDocument.DocumentElement;
								if (documentElement.ChildNodes.Count == 0 || (documentElement.ChildNodes.Count == 1 && documentElement.FirstChild.GetType() == typeof(XmlText)))
								{
									bool flag9 = this.fTopLevelTable;
									if (this.DataSetName != documentElement.Name && this.namespaceURI != documentElement.NamespaceURI && this.Tables.Contains(documentElement.Name, (documentElement.NamespaceURI.Length == 0) ? null : documentElement.NamespaceURI, false, true))
									{
										this.fTopLevelTable = true;
									}
									try
									{
										xmlDataLoader.LoadData(xmlDocument);
									}
									finally
									{
										this.fTopLevelTable = flag9;
									}
								}
							}
							if (!flag3)
							{
								if (!flag2 && this.Tables.Count == 0)
								{
									this.InferSchema(xmlDocument, null, XmlReadMode.Auto);
									xmlReadMode = XmlReadMode.InferSchema;
									xmlDataLoader.FromInference = true;
									try
									{
										xmlDataLoader.LoadData(xmlDocument);
									}
									finally
									{
										xmlDataLoader.FromInference = false;
									}
								}
								if (flag7)
								{
									this.fTopLevelTable = flag8;
								}
							}
						}
						xmlReadMode2 = xmlReadMode;
					}
				}
				finally
				{
					for (int k = 0; k < this.Tables.Count; k++)
					{
						this.Tables[k].rowDiffId = null;
					}
				}
			}
			finally
			{
				if (disposable != null)
				{
					disposable.Dispose();
				}
				Bid.ScopeLeave(ref intPtr);
			}
			return xmlReadMode2;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x001E70DC File Offset: 0x001E64DC
		public XmlReadMode ReadXml(Stream stream)
		{
			if (stream == null)
			{
				return XmlReadMode.Auto;
			}
			return this.ReadXml(new XmlTextReader(stream)
			{
				XmlResolver = null
			}, false);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x001E7104 File Offset: 0x001E6504
		public XmlReadMode ReadXml(TextReader reader)
		{
			if (reader == null)
			{
				return XmlReadMode.Auto;
			}
			return this.ReadXml(new XmlTextReader(reader)
			{
				XmlResolver = null
			}, false);
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x001E712C File Offset: 0x001E652C
		public XmlReadMode ReadXml(string fileName)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(fileName);
			xmlTextReader.XmlResolver = null;
			XmlReadMode xmlReadMode;
			try
			{
				xmlReadMode = this.ReadXml(xmlTextReader, false);
			}
			finally
			{
				xmlTextReader.Close();
			}
			return xmlReadMode;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x001E7178 File Offset: 0x001E6578
		internal void InferSchema(XmlDocument xdoc, string[] excludedNamespaces, XmlReadMode mode)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.InferSchema|INFO> %d#, mode=%d{ds.XmlReadMode}\n", this.ObjectID, (int)mode);
			try
			{
				string text = xdoc.DocumentElement.NamespaceURI;
				if (excludedNamespaces == null)
				{
					excludedNamespaces = new string[0];
				}
				XmlNodeReader xmlNodeReader = new XmlIgnoreNamespaceReader(xdoc, excludedNamespaces);
				XmlSchemaInference xmlSchemaInference = new XmlSchemaInference();
				xmlSchemaInference.Occurrence = XmlSchemaInference.InferenceOption.Relaxed;
				if (mode == XmlReadMode.InferTypedSchema)
				{
					xmlSchemaInference.TypeInference = XmlSchemaInference.InferenceOption.Restricted;
				}
				else
				{
					xmlSchemaInference.TypeInference = XmlSchemaInference.InferenceOption.Relaxed;
				}
				XmlSchemaSet xmlSchemaSet = xmlSchemaInference.InferSchema(xmlNodeReader);
				xmlSchemaSet.Compile();
				XSDSchema xsdschema = new XSDSchema();
				xsdschema.FromInference = true;
				try
				{
					xsdschema.LoadSchema(xmlSchemaSet, this);
				}
				finally
				{
					xsdschema.FromInference = false;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x001E7244 File Offset: 0x001E6644
		private bool IsEmpty()
		{
			foreach (object obj in this.Tables)
			{
				DataTable dataTable = (DataTable)obj;
				if (dataTable.Rows.Count > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x001E72B8 File Offset: 0x001E66B8
		private void ReadXmlDiffgram(XmlReader reader)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.ReadXmlDiffgram|INFO> %d#\n", this.ObjectID);
			try
			{
				int depth = reader.Depth;
				bool flag = this.EnforceConstraints;
				this.EnforceConstraints = false;
				bool flag2 = this.IsEmpty();
				DataSet dataSet;
				if (flag2)
				{
					dataSet = this;
				}
				else
				{
					dataSet = this.Clone();
					dataSet.EnforceConstraints = false;
				}
				foreach (object obj in dataSet.Tables)
				{
					DataTable dataTable = (DataTable)obj;
					dataTable.Rows.nullInList = 0;
				}
				reader.MoveToContent();
				if (!(reader.LocalName != "diffgram") || !(reader.NamespaceURI != "urn:schemas-microsoft-com:xml-diffgram-v1"))
				{
					reader.Read();
					if (reader.NodeType == XmlNodeType.Whitespace)
					{
						this.MoveToElement(reader, reader.Depth - 1);
					}
					dataSet.fInLoadDiffgram = true;
					if (reader.Depth > depth)
					{
						if (reader.NamespaceURI != "urn:schemas-microsoft-com:xml-diffgram-v1" && reader.NamespaceURI != "urn:schemas-microsoft-com:xml-msdata")
						{
							XmlDocument xmlDocument = new XmlDocument();
							XmlElement xmlElement = xmlDocument.CreateElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
							reader.Read();
							if (reader.NodeType == XmlNodeType.Whitespace)
							{
								this.MoveToElement(reader, reader.Depth - 1);
							}
							if (reader.Depth - 1 > depth)
							{
								new XmlDataLoader(dataSet, false, xmlElement, false)
								{
									isDiffgram = true
								}.LoadData(reader);
							}
							this.ReadEndElement(reader);
							if (reader.NodeType == XmlNodeType.Whitespace)
							{
								this.MoveToElement(reader, reader.Depth - 1);
							}
						}
						if ((reader.LocalName == "before" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1") || (reader.LocalName == "errors" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1"))
						{
							XMLDiffLoader xmldiffLoader = new XMLDiffLoader();
							xmldiffLoader.LoadDiffGram(dataSet, reader);
						}
						while (reader.Depth > depth)
						{
							reader.Read();
						}
						this.ReadEndElement(reader);
					}
					foreach (object obj2 in dataSet.Tables)
					{
						DataTable dataTable2 = (DataTable)obj2;
						if (dataTable2.Rows.nullInList > 0)
						{
							throw ExceptionBuilder.RowInsertMissing(dataTable2.TableName);
						}
					}
					dataSet.fInLoadDiffgram = false;
					foreach (object obj3 in dataSet.Tables)
					{
						DataTable dataTable3 = (DataTable)obj3;
						DataRelation[] nestedParentRelations = dataTable3.NestedParentRelations;
						foreach (DataRelation dataRelation in nestedParentRelations)
						{
							if (dataRelation.ParentTable == dataTable3)
							{
								foreach (object obj4 in dataTable3.Rows)
								{
									DataRow dataRow = (DataRow)obj4;
									foreach (DataRelation dataRelation2 in nestedParentRelations)
									{
										dataRow.CheckForLoops(dataRelation2);
									}
								}
							}
						}
					}
					if (!flag2)
					{
						this.Merge(dataSet);
						if (this.dataSetName == "NewDataSet")
						{
							this.dataSetName = dataSet.dataSetName;
						}
						dataSet.EnforceConstraints = flag;
					}
					this.EnforceConstraints = flag;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x001E76D0 File Offset: 0x001E6AD0
		public XmlReadMode ReadXml(XmlReader reader, XmlReadMode mode)
		{
			return this.ReadXml(reader, mode, false);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x001E76E8 File Offset: 0x001E6AE8
		internal XmlReadMode ReadXml(XmlReader reader, XmlReadMode mode, bool denyResolving)
		{
			IDisposable disposable = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.ReadXml|INFO> %d#, mode=%d{ds.XmlReadMode}, denyResolving=%d{bool}\n", this.ObjectID, (int)mode, denyResolving);
			XmlReadMode xmlReadMode2;
			try
			{
				disposable = TypeLimiter.EnterRestrictedScope(this);
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				int num = -1;
				XmlReadMode xmlReadMode = mode;
				if (reader == null)
				{
					xmlReadMode2 = xmlReadMode;
				}
				else if (mode == XmlReadMode.Auto)
				{
					xmlReadMode2 = this.ReadXml(reader);
				}
				else
				{
					if (reader is XmlTextReader)
					{
						((XmlTextReader)reader).WhitespaceHandling = WhitespaceHandling.Significant;
					}
					XmlDocument xmlDocument = new XmlDocument();
					if (mode != XmlReadMode.Fragment && reader.NodeType == XmlNodeType.Element)
					{
						num = reader.Depth;
					}
					reader.MoveToContent();
					XmlDataLoader xmlDataLoader = null;
					if (reader.NodeType == XmlNodeType.Element)
					{
						XmlElement xmlElement;
						if (mode == XmlReadMode.Fragment)
						{
							xmlDocument.AppendChild(xmlDocument.CreateElement("ds_sqlXmlWraPPeR"));
							xmlElement = xmlDocument.DocumentElement;
						}
						else
						{
							if (reader.LocalName == "diffgram" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1")
							{
								if (mode == XmlReadMode.DiffGram || mode == XmlReadMode.IgnoreSchema)
								{
									this.ReadXmlDiffgram(reader);
									this.ReadEndElement(reader);
								}
								else
								{
									reader.Skip();
								}
								return xmlReadMode;
							}
							if (reader.LocalName == "Schema" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-data")
							{
								if (mode != XmlReadMode.IgnoreSchema && mode != XmlReadMode.InferSchema && mode != XmlReadMode.InferTypedSchema)
								{
									this.ReadXDRSchema(reader);
								}
								else
								{
									reader.Skip();
								}
								return xmlReadMode;
							}
							if (reader.LocalName == "schema" && reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
							{
								if (mode != XmlReadMode.IgnoreSchema && mode != XmlReadMode.InferSchema && mode != XmlReadMode.InferTypedSchema)
								{
									this.ReadXSDSchema(reader, denyResolving);
								}
								else
								{
									reader.Skip();
								}
								return xmlReadMode;
							}
							if (reader.LocalName == "schema" && reader.NamespaceURI.StartsWith("http://www.w3.org/", StringComparison.Ordinal))
							{
								throw ExceptionBuilder.DataSetUnsupportedSchema("http://www.w3.org/2001/XMLSchema");
							}
							xmlElement = xmlDocument.CreateElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
							if (reader.HasAttributes)
							{
								int attributeCount = reader.AttributeCount;
								for (int i = 0; i < attributeCount; i++)
								{
									reader.MoveToAttribute(i);
									if (reader.NamespaceURI.Equals("http://www.w3.org/2000/xmlns/"))
									{
										xmlElement.SetAttribute(reader.Name, reader.GetAttribute(i));
									}
									else
									{
										XmlAttribute xmlAttribute = xmlElement.SetAttributeNode(reader.LocalName, reader.NamespaceURI);
										xmlAttribute.Prefix = reader.Prefix;
										xmlAttribute.Value = reader.GetAttribute(i);
									}
								}
							}
							reader.Read();
						}
						while (this.MoveToElement(reader, num))
						{
							if (reader.LocalName == "Schema" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-data")
							{
								if (!flag && !flag2 && mode != XmlReadMode.IgnoreSchema && mode != XmlReadMode.InferSchema && mode != XmlReadMode.InferTypedSchema)
								{
									this.ReadXDRSchema(reader);
									flag = true;
									flag3 = true;
								}
								else
								{
									reader.Skip();
								}
							}
							else if (reader.LocalName == "schema" && reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
							{
								if (mode != XmlReadMode.IgnoreSchema && mode != XmlReadMode.InferSchema && mode != XmlReadMode.InferTypedSchema)
								{
									this.ReadXSDSchema(reader, denyResolving);
									flag = true;
								}
								else
								{
									reader.Skip();
								}
							}
							else if (reader.LocalName == "diffgram" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1")
							{
								if (mode == XmlReadMode.DiffGram || mode == XmlReadMode.IgnoreSchema)
								{
									this.ReadXmlDiffgram(reader);
									xmlReadMode = XmlReadMode.DiffGram;
								}
								else
								{
									reader.Skip();
								}
							}
							else
							{
								if (reader.LocalName == "schema" && reader.NamespaceURI.StartsWith("http://www.w3.org/", StringComparison.Ordinal))
								{
									throw ExceptionBuilder.DataSetUnsupportedSchema("http://www.w3.org/2001/XMLSchema");
								}
								if (mode == XmlReadMode.DiffGram)
								{
									reader.Skip();
								}
								else
								{
									flag2 = true;
									if (mode == XmlReadMode.InferSchema || mode == XmlReadMode.InferTypedSchema)
									{
										XmlNode xmlNode = xmlDocument.ReadNode(reader);
										xmlElement.AppendChild(xmlNode);
									}
									else
									{
										if (xmlDataLoader == null)
										{
											xmlDataLoader = new XmlDataLoader(this, flag3, xmlElement, mode == XmlReadMode.IgnoreSchema);
										}
										xmlDataLoader.LoadData(reader);
									}
								}
							}
						}
						this.ReadEndElement(reader);
						xmlDocument.AppendChild(xmlElement);
						if (xmlDataLoader == null)
						{
							xmlDataLoader = new XmlDataLoader(this, flag3, mode == XmlReadMode.IgnoreSchema);
						}
						if (mode == XmlReadMode.DiffGram)
						{
							return xmlReadMode;
						}
						if (mode == XmlReadMode.InferSchema || mode == XmlReadMode.InferTypedSchema)
						{
							this.InferSchema(xmlDocument, null, mode);
							xmlReadMode = XmlReadMode.InferSchema;
							xmlDataLoader.FromInference = true;
							try
							{
								xmlDataLoader.LoadData(xmlDocument);
							}
							finally
							{
								xmlDataLoader.FromInference = false;
							}
						}
					}
					xmlReadMode2 = xmlReadMode;
				}
			}
			finally
			{
				if (disposable != null)
				{
					disposable.Dispose();
				}
				Bid.ScopeLeave(ref intPtr);
			}
			return xmlReadMode2;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x001E7B5C File Offset: 0x001E6F5C
		public XmlReadMode ReadXml(Stream stream, XmlReadMode mode)
		{
			if (stream == null)
			{
				return XmlReadMode.Auto;
			}
			XmlTextReader xmlTextReader = ((mode == XmlReadMode.Fragment) ? new XmlTextReader(stream, XmlNodeType.Element, null) : new XmlTextReader(stream));
			xmlTextReader.XmlResolver = null;
			return this.ReadXml(xmlTextReader, mode, false);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x001E7B94 File Offset: 0x001E6F94
		public XmlReadMode ReadXml(TextReader reader, XmlReadMode mode)
		{
			if (reader == null)
			{
				return XmlReadMode.Auto;
			}
			XmlTextReader xmlTextReader = ((mode == XmlReadMode.Fragment) ? new XmlTextReader(reader.ReadToEnd(), XmlNodeType.Element, null) : new XmlTextReader(reader));
			xmlTextReader.XmlResolver = null;
			return this.ReadXml(xmlTextReader, mode, false);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x001E7BD0 File Offset: 0x001E6FD0
		public XmlReadMode ReadXml(string fileName, XmlReadMode mode)
		{
			XmlTextReader xmlTextReader = null;
			if (mode == XmlReadMode.Fragment)
			{
				FileStream fileStream = new FileStream(fileName, FileMode.Open);
				xmlTextReader = new XmlTextReader(fileStream, XmlNodeType.Element, null);
			}
			else
			{
				xmlTextReader = new XmlTextReader(fileName);
			}
			xmlTextReader.XmlResolver = null;
			XmlReadMode xmlReadMode;
			try
			{
				xmlReadMode = this.ReadXml(xmlTextReader, mode, false);
			}
			finally
			{
				xmlTextReader.Close();
			}
			return xmlReadMode;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x001E7C34 File Offset: 0x001E7034
		public void WriteXml(Stream stream)
		{
			this.WriteXml(stream, XmlWriteMode.IgnoreSchema);
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x001E7C4C File Offset: 0x001E704C
		public void WriteXml(TextWriter writer)
		{
			this.WriteXml(writer, XmlWriteMode.IgnoreSchema);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x001E7C64 File Offset: 0x001E7064
		public void WriteXml(XmlWriter writer)
		{
			this.WriteXml(writer, XmlWriteMode.IgnoreSchema);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x001E7C7C File Offset: 0x001E707C
		public void WriteXml(string fileName)
		{
			this.WriteXml(fileName, XmlWriteMode.IgnoreSchema);
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x001E7C94 File Offset: 0x001E7094
		public void WriteXml(Stream stream, XmlWriteMode mode)
		{
			if (stream != null)
			{
				this.WriteXml(new XmlTextWriter(stream, null)
				{
					Formatting = Formatting.Indented
				}, mode);
			}
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x001E7CBC File Offset: 0x001E70BC
		public void WriteXml(TextWriter writer, XmlWriteMode mode)
		{
			if (writer != null)
			{
				this.WriteXml(new XmlTextWriter(writer)
				{
					Formatting = Formatting.Indented
				}, mode);
			}
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x001E7CE4 File Offset: 0x001E70E4
		public void WriteXml(XmlWriter writer, XmlWriteMode mode)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.WriteXml|API> %d#, mode=%d{ds.XmlWriteMode}\n", this.ObjectID, (int)mode);
			try
			{
				if (writer != null)
				{
					if (mode == XmlWriteMode.DiffGram)
					{
						new NewDiffgramGen(this).Save(writer);
					}
					else
					{
						new XmlDataTreeWriter(this).Save(writer, mode == XmlWriteMode.WriteSchema);
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x001E7D50 File Offset: 0x001E7150
		public void WriteXml(string fileName, XmlWriteMode mode)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.WriteXml|API> %d#, fileName='%ls', mode=%d{ds.XmlWriteMode}\n", this.ObjectID, fileName, (int)mode);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(fileName, null);
			try
			{
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.WriteStartDocument(true);
				if (xmlTextWriter != null)
				{
					if (mode == XmlWriteMode.DiffGram)
					{
						new NewDiffgramGen(this).Save(xmlTextWriter);
					}
					else
					{
						new XmlDataTreeWriter(this).Save(xmlTextWriter, mode == XmlWriteMode.WriteSchema);
					}
				}
				xmlTextWriter.WriteEndDocument();
			}
			finally
			{
				xmlTextWriter.Close();
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x001E7DE0 File Offset: 0x001E71E0
		internal DataRelationCollection GetParentRelations(DataTable table)
		{
			return table.ParentRelations;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x001E7DF4 File Offset: 0x001E71F4
		public void Merge(DataSet dataSet)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.Merge|API> %d#, dataSet=%d\n", this.ObjectID, (dataSet != null) ? dataSet.ObjectID : 0);
			try
			{
				this.Merge(dataSet, false, MissingSchemaAction.Add);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x001E7E50 File Offset: 0x001E7250
		public void Merge(DataSet dataSet, bool preserveChanges)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.Merge|API> %d#, dataSet=%d, preserveChanges=%d{bool}\n", this.ObjectID, (dataSet != null) ? dataSet.ObjectID : 0, preserveChanges);
			try
			{
				this.Merge(dataSet, preserveChanges, MissingSchemaAction.Add);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x001E7EAC File Offset: 0x001E72AC
		public void Merge(DataSet dataSet, bool preserveChanges, MissingSchemaAction missingSchemaAction)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.Merge|API> %d#, dataSet=%d, preserveChanges=%d{bool}, missingSchemaAction=%d{ds.MissingSchemaAction}\n", this.ObjectID, (dataSet != null) ? dataSet.ObjectID : 0, preserveChanges, (int)missingSchemaAction);
			try
			{
				if (dataSet == null)
				{
					throw ExceptionBuilder.ArgumentNull("dataSet");
				}
				switch (missingSchemaAction)
				{
				case MissingSchemaAction.Add:
				case MissingSchemaAction.Ignore:
				case MissingSchemaAction.Error:
				case MissingSchemaAction.AddWithKey:
				{
					Merger merger = new Merger(this, preserveChanges, missingSchemaAction);
					merger.MergeDataSet(dataSet);
					break;
				}
				default:
					throw ADP.InvalidMissingSchemaAction(missingSchemaAction);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x001E7F44 File Offset: 0x001E7344
		public void Merge(DataTable table)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.Merge|API> %d#, table=%d\n", this.ObjectID, (table != null) ? table.ObjectID : 0);
			try
			{
				this.Merge(table, false, MissingSchemaAction.Add);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x001E7FA0 File Offset: 0x001E73A0
		public void Merge(DataTable table, bool preserveChanges, MissingSchemaAction missingSchemaAction)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.Merge|API> %d#, table=%d, preserveChanges=%d{bool}, missingSchemaAction=%d{ds.MissingSchemaAction}\n", this.ObjectID, (table != null) ? table.ObjectID : 0, preserveChanges, (int)missingSchemaAction);
			try
			{
				if (table == null)
				{
					throw ExceptionBuilder.ArgumentNull("table");
				}
				switch (missingSchemaAction)
				{
				case MissingSchemaAction.Add:
				case MissingSchemaAction.Ignore:
				case MissingSchemaAction.Error:
				case MissingSchemaAction.AddWithKey:
				{
					Merger merger = new Merger(this, preserveChanges, missingSchemaAction);
					merger.MergeTable(table);
					break;
				}
				default:
					throw ADP.InvalidMissingSchemaAction(missingSchemaAction);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x001E8038 File Offset: 0x001E7438
		public void Merge(DataRow[] rows)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.Merge|API> %d#, rows\n", this.ObjectID);
			try
			{
				this.Merge(rows, false, MissingSchemaAction.Add);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x001E8088 File Offset: 0x001E7488
		public void Merge(DataRow[] rows, bool preserveChanges, MissingSchemaAction missingSchemaAction)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.Merge|API> %d#, preserveChanges=%d{bool}, missingSchemaAction=%d{ds.MissingSchemaAction}\n", this.ObjectID, preserveChanges, (int)missingSchemaAction);
			try
			{
				if (rows == null)
				{
					throw ExceptionBuilder.ArgumentNull("rows");
				}
				switch (missingSchemaAction)
				{
				case MissingSchemaAction.Add:
				case MissingSchemaAction.Ignore:
				case MissingSchemaAction.Error:
				case MissingSchemaAction.AddWithKey:
				{
					Merger merger = new Merger(this, preserveChanges, missingSchemaAction);
					merger.MergeRows(rows);
					break;
				}
				default:
					throw ADP.InvalidMissingSchemaAction(missingSchemaAction);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x001E8114 File Offset: 0x001E7514
		protected virtual void OnPropertyChanging(PropertyChangedEventArgs pcevent)
		{
			if (this.onPropertyChangingDelegate != null)
			{
				this.onPropertyChangingDelegate(this, pcevent);
			}
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x001E8138 File Offset: 0x001E7538
		internal void OnMergeFailed(MergeFailedEventArgs mfevent)
		{
			if (this.onMergeFailed != null)
			{
				this.onMergeFailed(this, mfevent);
				return;
			}
			throw ExceptionBuilder.MergeFailed(mfevent.Conflict);
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x001E8168 File Offset: 0x001E7568
		internal void RaiseMergeFailed(DataTable table, string conflict, MissingSchemaAction missingSchemaAction)
		{
			if (MissingSchemaAction.Error == missingSchemaAction)
			{
				throw ExceptionBuilder.MergeFailed(conflict);
			}
			MergeFailedEventArgs mergeFailedEventArgs = new MergeFailedEventArgs(table, conflict);
			this.OnMergeFailed(mergeFailedEventArgs);
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x001E8190 File Offset: 0x001E7590
		internal void OnDataRowCreated(DataRow row)
		{
			if (this.onDataRowCreated != null)
			{
				this.onDataRowCreated(this, row);
			}
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x001E81B4 File Offset: 0x001E75B4
		internal void OnClearFunctionCalled(DataTable table)
		{
			if (this.onClearFunctionCalled != null)
			{
				this.onClearFunctionCalled(this, table);
			}
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x001E81D8 File Offset: 0x001E75D8
		private void OnInitialized()
		{
			if (this.onInitialized != null)
			{
				this.onInitialized(this, EventArgs.Empty);
			}
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x001E8200 File Offset: 0x001E7600
		protected internal virtual void OnRemoveTable(DataTable table)
		{
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x001E8210 File Offset: 0x001E7610
		internal void OnRemovedTable(DataTable table)
		{
			DataViewManager dataViewManager = this.defaultViewManager;
			if (dataViewManager != null)
			{
				dataViewManager.DataViewSettings.Remove(table);
			}
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x001E8234 File Offset: 0x001E7634
		protected virtual void OnRemoveRelation(DataRelation relation)
		{
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x001E8244 File Offset: 0x001E7644
		internal void OnRemoveRelationHack(DataRelation relation)
		{
			this.OnRemoveRelation(relation);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x001E8258 File Offset: 0x001E7658
		protected internal void RaisePropertyChanging(string name)
		{
			this.OnPropertyChanging(new PropertyChangedEventArgs(name));
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x001E8274 File Offset: 0x001E7674
		internal DataTable[] TopLevelTables()
		{
			return this.TopLevelTables(false);
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x001E8288 File Offset: 0x001E7688
		internal DataTable[] TopLevelTables(bool forSchema)
		{
			List<DataTable> list = new List<DataTable>();
			if (forSchema)
			{
				for (int i = 0; i < this.Tables.Count; i++)
				{
					DataTable dataTable = this.Tables[i];
					if (dataTable.NestedParentsCount > 1 || dataTable.SelfNested)
					{
						list.Add(dataTable);
					}
				}
			}
			for (int j = 0; j < this.Tables.Count; j++)
			{
				DataTable dataTable2 = this.Tables[j];
				if (dataTable2.NestedParentsCount == 0 && !list.Contains(dataTable2))
				{
					list.Add(dataTable2);
				}
			}
			if (list.Count == 0)
			{
				return DataSet.zeroTables;
			}
			return list.ToArray();
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x001E832C File Offset: 0x001E772C
		public virtual void RejectChanges()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.RejectChanges|API> %d#\n", this.ObjectID);
			try
			{
				bool flag = this.EnforceConstraints;
				this.EnforceConstraints = false;
				for (int i = 0; i < this.Tables.Count; i++)
				{
					this.Tables[i].RejectChanges();
				}
				this.EnforceConstraints = flag;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x001E83B0 File Offset: 0x001E77B0
		public virtual void Reset()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.Reset|API> %d#\n", this.ObjectID);
			try
			{
				for (int i = 0; i < this.Tables.Count; i++)
				{
					ConstraintCollection constraints = this.Tables[i].Constraints;
					int j = 0;
					while (j < constraints.Count)
					{
						if (constraints[j] is ForeignKeyConstraint)
						{
							constraints.Remove(constraints[j]);
						}
						else
						{
							j++;
						}
					}
				}
				this.Clear();
				this.Relations.Clear();
				this.Tables.Clear();
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x001E8468 File Offset: 0x001E7868
		internal bool ValidateCaseConstraint()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.ValidateCaseConstraint|INFO> %d#\n", this.ObjectID);
			bool flag;
			try
			{
				for (int i = 0; i < this.Relations.Count; i++)
				{
					DataRelation dataRelation = this.Relations[i];
					if (dataRelation.ChildTable.CaseSensitive != dataRelation.ParentTable.CaseSensitive)
					{
						return false;
					}
				}
				for (int j = 0; j < this.Tables.Count; j++)
				{
					ConstraintCollection constraints = this.Tables[j].Constraints;
					for (int k = 0; k < constraints.Count; k++)
					{
						if (constraints[k] is ForeignKeyConstraint)
						{
							ForeignKeyConstraint foreignKeyConstraint = (ForeignKeyConstraint)constraints[k];
							if (foreignKeyConstraint.Table.CaseSensitive != foreignKeyConstraint.RelatedTable.CaseSensitive)
							{
								return false;
							}
						}
					}
				}
				flag = true;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return flag;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x001E8574 File Offset: 0x001E7974
		internal bool ValidateLocaleConstraint()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.ValidateLocaleConstraint|INFO> %d#\n", this.ObjectID);
			bool flag;
			try
			{
				for (int i = 0; i < this.Relations.Count; i++)
				{
					DataRelation dataRelation = this.Relations[i];
					if (dataRelation.ChildTable.Locale.LCID != dataRelation.ParentTable.Locale.LCID)
					{
						return false;
					}
				}
				for (int j = 0; j < this.Tables.Count; j++)
				{
					ConstraintCollection constraints = this.Tables[j].Constraints;
					for (int k = 0; k < constraints.Count; k++)
					{
						if (constraints[k] is ForeignKeyConstraint)
						{
							ForeignKeyConstraint foreignKeyConstraint = (ForeignKeyConstraint)constraints[k];
							if (foreignKeyConstraint.Table.Locale.LCID != foreignKeyConstraint.RelatedTable.Locale.LCID)
							{
								return false;
							}
						}
					}
				}
				flag = true;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return flag;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x001E8694 File Offset: 0x001E7A94
		internal DataTable FindTable(DataTable baseTable, PropertyDescriptor[] props, int propStart)
		{
			if (props.Length < propStart + 1)
			{
				return baseTable;
			}
			PropertyDescriptor propertyDescriptor = props[propStart];
			if (baseTable == null)
			{
				if (propertyDescriptor is DataTablePropertyDescriptor)
				{
					return this.FindTable(((DataTablePropertyDescriptor)propertyDescriptor).Table, props, propStart + 1);
				}
				return null;
			}
			else
			{
				if (propertyDescriptor is DataRelationPropertyDescriptor)
				{
					return this.FindTable(((DataRelationPropertyDescriptor)propertyDescriptor).Relation.ChildTable, props, propStart + 1);
				}
				return null;
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x001E86F8 File Offset: 0x001E7AF8
		protected virtual void ReadXmlSerializable(XmlReader reader)
		{
			this.UseDataSetSchemaOnly = false;
			this.UdtIsWrapped = false;
			if (reader.HasAttributes)
			{
				if (reader.MoveToAttribute("xsi:nil"))
				{
					string attribute = reader.GetAttribute("xsi:nil");
					if (string.Compare(attribute, "true", StringComparison.Ordinal) == 0)
					{
						this.MoveToElement(reader, 1);
						return;
					}
				}
				if (reader.MoveToAttribute("msdata:UseDataSetSchemaOnly"))
				{
					string attribute2 = reader.GetAttribute("msdata:UseDataSetSchemaOnly");
					if (string.Equals(attribute2, "true", StringComparison.Ordinal) || string.Equals(attribute2, "1", StringComparison.Ordinal))
					{
						this.UseDataSetSchemaOnly = true;
					}
					else if (!string.Equals(attribute2, "false", StringComparison.Ordinal) && !string.Equals(attribute2, "0", StringComparison.Ordinal))
					{
						throw ExceptionBuilder.InvalidAttributeValue("UseDataSetSchemaOnly", attribute2);
					}
				}
				if (reader.MoveToAttribute("msdata:UDTColumnValueWrapped"))
				{
					string attribute3 = reader.GetAttribute("msdata:UDTColumnValueWrapped");
					if (string.Equals(attribute3, "true", StringComparison.Ordinal) || string.Equals(attribute3, "1", StringComparison.Ordinal))
					{
						this.UdtIsWrapped = true;
					}
					else if (!string.Equals(attribute3, "false", StringComparison.Ordinal) && !string.Equals(attribute3, "0", StringComparison.Ordinal))
					{
						throw ExceptionBuilder.InvalidAttributeValue("UDTColumnValueWrapped", attribute3);
					}
				}
			}
			this.ReadXml(reader, XmlReadMode.DiffGram, true);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x001E8828 File Offset: 0x001E7C28
		protected virtual XmlSchema GetSchemaSerializable()
		{
			return null;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x001E8838 File Offset: 0x001E7C38
		public static XmlSchemaComplexType GetDataSetSchema(XmlSchemaSet schemaSet)
		{
			if (DataSet.schemaTypeForWSDL == null)
			{
				XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
				XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
				if (DataSet.PublishLegacyWSDL())
				{
					XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
					xmlSchemaElement.RefName = new XmlQualifiedName("schema", "http://www.w3.org/2001/XMLSchema");
					xmlSchemaSequence.Items.Add(xmlSchemaElement);
					XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
					xmlSchemaSequence.Items.Add(xmlSchemaAny);
				}
				else
				{
					XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
					xmlSchemaAny2.Namespace = "http://www.w3.org/2001/XMLSchema";
					xmlSchemaAny2.MinOccurs = 0m;
					xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
					xmlSchemaSequence.Items.Add(xmlSchemaAny2);
					xmlSchemaAny2 = new XmlSchemaAny();
					xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
					xmlSchemaAny2.MinOccurs = 0m;
					xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
					xmlSchemaSequence.Items.Add(xmlSchemaAny2);
					xmlSchemaSequence.MaxOccurs = decimal.MaxValue;
				}
				xmlSchemaComplexType.Particle = xmlSchemaSequence;
				DataSet.schemaTypeForWSDL = xmlSchemaComplexType;
			}
			return DataSet.schemaTypeForWSDL;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x001E8928 File Offset: 0x001E7D28
		private static bool PublishLegacyWSDL()
		{
			float num = 1f;
			NameValueCollection nameValueCollection = (NameValueCollection)PrivilegedConfigurationManager.GetSection("system.data.dataset");
			if (nameValueCollection != null)
			{
				string[] values = nameValueCollection.GetValues("WSDL_VERSION");
				if (values != null && 0 < values.Length && values[0] != null)
				{
					num = float.Parse(values[0], CultureInfo.InvariantCulture);
				}
			}
			return num < 2f;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x001E8980 File Offset: 0x001E7D80
		XmlSchema IXmlSerializable.GetSchema()
		{
			if (base.GetType() == typeof(DataSet))
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			XmlWriter xmlWriter = new XmlTextWriter(memoryStream, null);
			if (xmlWriter != null)
			{
				new XmlTreeGen(SchemaFormat.WebService).Save(this, xmlWriter);
			}
			memoryStream.Position = 0L;
			return XmlSchema.Read(new XmlTextReader(memoryStream), null);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x001E89D4 File Offset: 0x001E7DD4
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			bool flag = true;
			XmlTextReader xmlTextReader = null;
			IXmlTextParser xmlTextParser = reader as IXmlTextParser;
			if (xmlTextParser != null)
			{
				flag = xmlTextParser.Normalized;
				xmlTextParser.Normalized = false;
			}
			else
			{
				xmlTextReader = reader as XmlTextReader;
				if (xmlTextReader != null)
				{
					flag = xmlTextReader.Normalization;
					xmlTextReader.Normalization = false;
				}
			}
			this.ReadXmlSerializable(reader);
			if (xmlTextParser != null)
			{
				xmlTextParser.Normalized = flag;
				return;
			}
			if (xmlTextReader != null)
			{
				xmlTextReader.Normalization = flag;
			}
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x001E8A34 File Offset: 0x001E7E34
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			this.WriteXmlSchema(writer, SchemaFormat.WebService);
			this.WriteXml(writer, XmlWriteMode.DiffGram);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x001E8A54 File Offset: 0x001E7E54
		public virtual void Load(IDataReader reader, LoadOption loadOption, FillErrorEventHandler errorHandler, params DataTable[] tables)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.Load|API> reader, loadOption=%d{ds.LoadOption}", (int)loadOption);
			try
			{
				foreach (DataTable dataTable in tables)
				{
					ADP.CheckArgumentNull(dataTable, "tables");
					if (dataTable.DataSet != this)
					{
						throw ExceptionBuilder.TableNotInTheDataSet(dataTable.TableName);
					}
				}
				LoadAdapter loadAdapter = new LoadAdapter();
				loadAdapter.FillLoadOption = loadOption;
				loadAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
				if (errorHandler != null)
				{
					loadAdapter.FillError += errorHandler;
				}
				loadAdapter.FillFromReader(tables, reader, 0, 0);
				if (!reader.IsClosed && !reader.NextResult())
				{
					reader.Close();
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x001E8B0C File Offset: 0x001E7F0C
		public void Load(IDataReader reader, LoadOption loadOption, params DataTable[] tables)
		{
			this.Load(reader, loadOption, null, tables);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x001E8B24 File Offset: 0x001E7F24
		public void Load(IDataReader reader, LoadOption loadOption, params string[] tables)
		{
			ADP.CheckArgumentNull(tables, "tables");
			DataTable[] array = new DataTable[tables.Length];
			for (int i = 0; i < tables.Length; i++)
			{
				DataTable dataTable = this.Tables[tables[i]];
				if (dataTable == null)
				{
					dataTable = new DataTable(tables[i]);
					this.Tables.Add(dataTable);
				}
				array[i] = dataTable;
			}
			this.Load(reader, loadOption, null, array);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x001E8B88 File Offset: 0x001E7F88
		public DataTableReader CreateDataReader()
		{
			if (this.Tables.Count == 0)
			{
				throw ExceptionBuilder.CannotCreateDataReaderOnEmptyDataSet();
			}
			DataTable[] array = new DataTable[this.Tables.Count];
			for (int i = 0; i < this.Tables.Count; i++)
			{
				array[i] = this.Tables[i];
			}
			return this.CreateDataReader(array);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x001E8BE8 File Offset: 0x001E7FE8
		public DataTableReader CreateDataReader(params DataTable[] dataTables)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataSet.GetDataReader|API> %d#\n", this.ObjectID);
			DataTableReader dataTableReader;
			try
			{
				if (dataTables.Length == 0)
				{
					throw ExceptionBuilder.DataTableReaderArgumentIsEmpty();
				}
				for (int i = 0; i < dataTables.Length; i++)
				{
					if (dataTables[i] == null)
					{
						throw ExceptionBuilder.ArgumentContainsNullValue();
					}
				}
				dataTableReader = new DataTableReader(dataTables);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataTableReader;
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x001E8C5C File Offset: 0x001E805C
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x001E8C70 File Offset: 0x001E8070
		internal string MainTableName
		{
			get
			{
				return this.mainTableName;
			}
			set
			{
				this.mainTableName = value;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x001E8C84 File Offset: 0x001E8084
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x0400078B RID: 1931
		private const string KEY_XMLSCHEMA = "XmlSchema";

		// Token: 0x0400078C RID: 1932
		private const string KEY_XMLDIFFGRAM = "XmlDiffGram";

		// Token: 0x0400078D RID: 1933
		private DataViewManager defaultViewManager;

		// Token: 0x0400078E RID: 1934
		private readonly DataTableCollection tableCollection;

		// Token: 0x0400078F RID: 1935
		private readonly DataRelationCollection relationCollection;

		// Token: 0x04000790 RID: 1936
		internal PropertyCollection extendedProperties;

		// Token: 0x04000791 RID: 1937
		private string dataSetName = "NewDataSet";

		// Token: 0x04000792 RID: 1938
		private string _datasetPrefix = string.Empty;

		// Token: 0x04000793 RID: 1939
		internal string namespaceURI = string.Empty;

		// Token: 0x04000794 RID: 1940
		private bool enforceConstraints = true;

		// Token: 0x04000795 RID: 1941
		private bool _caseSensitive;

		// Token: 0x04000796 RID: 1942
		private CultureInfo _culture;

		// Token: 0x04000797 RID: 1943
		private bool _cultureUserSet;

		// Token: 0x04000798 RID: 1944
		internal bool fInReadXml;

		// Token: 0x04000799 RID: 1945
		internal bool fInLoadDiffgram;

		// Token: 0x0400079A RID: 1946
		internal bool fTopLevelTable;

		// Token: 0x0400079B RID: 1947
		internal bool fInitInProgress;

		// Token: 0x0400079C RID: 1948
		internal bool fEnableCascading = true;

		// Token: 0x0400079D RID: 1949
		internal bool fIsSchemaLoading;

		// Token: 0x0400079E RID: 1950
		private bool fBoundToDocument;

		// Token: 0x0400079F RID: 1951
		private PropertyChangedEventHandler onPropertyChangingDelegate;

		// Token: 0x040007A0 RID: 1952
		private MergeFailedEventHandler onMergeFailed;

		// Token: 0x040007A1 RID: 1953
		private DataRowCreatedEventHandler onDataRowCreated;

		// Token: 0x040007A2 RID: 1954
		private DataSetClearEventhandler onClearFunctionCalled;

		// Token: 0x040007A3 RID: 1955
		private EventHandler onInitialized;

		// Token: 0x040007A4 RID: 1956
		internal static readonly DataTable[] zeroTables = new DataTable[0];

		// Token: 0x040007A5 RID: 1957
		internal string mainTableName = "";

		// Token: 0x040007A6 RID: 1958
		private SerializationFormat _remotingFormat;

		// Token: 0x040007A7 RID: 1959
		private object _defaultViewManagerLock = new object();

		// Token: 0x040007A8 RID: 1960
		private static int _objectTypeCount;

		// Token: 0x040007A9 RID: 1961
		private readonly int _objectID = Interlocked.Increment(ref DataSet._objectTypeCount);

		// Token: 0x040007AA RID: 1962
		private static XmlSchemaComplexType schemaTypeForWSDL = null;

		// Token: 0x040007AB RID: 1963
		internal bool UseDataSetSchemaOnly;

		// Token: 0x040007AC RID: 1964
		internal bool UdtIsWrapped;

		// Token: 0x02000095 RID: 149
		private struct TableChanges
		{
			// Token: 0x0600091A RID: 2330 RVA: 0x001E8CB8 File Offset: 0x001E80B8
			internal TableChanges(int rowCount)
			{
				this._rowChanges = new BitArray(rowCount);
				this._hasChanges = 0;
			}

			// Token: 0x1700011D RID: 285
			// (get) Token: 0x0600091B RID: 2331 RVA: 0x001E8CD8 File Offset: 0x001E80D8
			// (set) Token: 0x0600091C RID: 2332 RVA: 0x001E8CEC File Offset: 0x001E80EC
			internal int HasChanges
			{
				get
				{
					return this._hasChanges;
				}
				set
				{
					this._hasChanges = value;
				}
			}

			// Token: 0x1700011E RID: 286
			internal bool this[int index]
			{
				get
				{
					return this._rowChanges[index];
				}
				set
				{
					this._rowChanges[index] = value;
					this._hasChanges++;
				}
			}

			// Token: 0x040007AD RID: 1965
			private BitArray _rowChanges;

			// Token: 0x040007AE RID: 1966
			private int _hasChanges;
		}
	}
}

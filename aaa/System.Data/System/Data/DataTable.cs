using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data
{
	// Token: 0x0200009A RID: 154
	[Editor("Microsoft.VSDesigner.Data.Design.DataTableEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("TableName")]
	[ToolboxItem(false)]
	[DefaultEvent("RowChanging")]
	[XmlSchemaProvider("GetDataTableSchema")]
	[DesignTimeVisible(false)]
	[Serializable]
	public class DataTable : MarshalByValueComponent, IListSource, ISupportInitializeNotification, ISupportInitialize, ISerializable, IXmlSerializable
	{
		// Token: 0x0600092A RID: 2346 RVA: 0x001E9550 File Offset: 0x001E8950
		public DataTable()
		{
			GC.SuppressFinalize(this);
			Bid.Trace("<ds.DataTable.DataTable|API> %d#\n", this.ObjectID);
			this.nextRowID = 1L;
			this.recordManager = new RecordManager(this);
			this._culture = CultureInfo.CurrentCulture;
			this.columnCollection = new DataColumnCollection(this);
			this.constraintCollection = new ConstraintCollection(this);
			this.rowCollection = new DataRowCollection(this);
			this.indexes = new List<Index>();
			this.rowBuilder = new DataRowBuilder(this, -1);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x001E9668 File Offset: 0x001E8A68
		public DataTable(string tableName)
			: this()
		{
			this.tableName = ((tableName == null) ? "" : tableName);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x001E968C File Offset: 0x001E8A8C
		public DataTable(string tableName, string tableNamespace)
			: this(tableName)
		{
			this.Namespace = tableNamespace;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x001E96A8 File Offset: 0x001E8AA8
		protected DataTable(SerializationInfo info, StreamingContext context)
			: this()
		{
			bool flag = context.Context == null || Convert.ToBoolean(context.Context, CultureInfo.InvariantCulture);
			SerializationFormat serializationFormat = SerializationFormat.Xml;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name;
				if ((name = enumerator.Name) != null && name == "DataTable.RemotingFormat")
				{
					serializationFormat = (SerializationFormat)enumerator.Value;
				}
			}
			this.DeserializeDataTable(info, context, flag, serializationFormat);
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x001E971C File Offset: 0x001E8B1C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			SerializationFormat remotingFormat = this.RemotingFormat;
			bool flag = context.Context == null || Convert.ToBoolean(context.Context, CultureInfo.InvariantCulture);
			this.SerializeDataTable(info, context, flag, remotingFormat);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x001E9758 File Offset: 0x001E8B58
		private void SerializeDataTable(SerializationInfo info, StreamingContext context, bool isSingleTable, SerializationFormat remotingFormat)
		{
			info.AddValue("DataTable.RemotingVersion", new Version(2, 0));
			if (remotingFormat != SerializationFormat.Xml)
			{
				info.AddValue("DataTable.RemotingFormat", remotingFormat);
			}
			if (remotingFormat != SerializationFormat.Xml)
			{
				this.SerializeTableSchema(info, context, isSingleTable);
				if (isSingleTable)
				{
					this.SerializeTableData(info, context, 0);
					return;
				}
			}
			else
			{
				string text = "";
				bool flag = false;
				if (this.dataSet == null)
				{
					DataSet dataSet = new DataSet("tmpDataSet");
					dataSet.SetLocaleValue(this._culture, this._cultureUserSet);
					dataSet.CaseSensitive = this.CaseSensitive;
					dataSet.namespaceURI = this.Namespace;
					dataSet.Tables.Add(this);
					flag = true;
				}
				else
				{
					text = this.DataSet.Namespace;
					this.DataSet.namespaceURI = this.Namespace;
				}
				info.AddValue("XmlSchema", this.dataSet.GetXmlSchemaForRemoting(this));
				info.AddValue("XmlDiffGram", this.dataSet.GetRemotingDiffGram(this));
				if (flag)
				{
					this.dataSet.Tables.Remove(this);
					return;
				}
				this.dataSet.namespaceURI = text;
			}
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x001E986C File Offset: 0x001E8C6C
		internal void DeserializeDataTable(SerializationInfo info, StreamingContext context, bool isSingleTable, SerializationFormat remotingFormat)
		{
			if (remotingFormat != SerializationFormat.Xml)
			{
				this.DeserializeTableSchema(info, context, isSingleTable);
				if (isSingleTable)
				{
					this.DeserializeTableData(info, context, 0);
					this.ResetIndexes();
					return;
				}
			}
			else
			{
				string text = (string)info.GetValue("XmlSchema", typeof(string));
				string text2 = (string)info.GetValue("XmlDiffGram", typeof(string));
				if (text != null)
				{
					DataSet dataSet = new DataSet();
					dataSet.ReadXmlSchema(new XmlTextReader(new StringReader(text)));
					DataTable dataTable = dataSet.Tables[0];
					dataTable.CloneTo(this, null, false);
					this.Namespace = dataTable.Namespace;
					if (text2 != null)
					{
						dataSet.Tables.Remove(dataSet.Tables[0]);
						dataSet.Tables.Add(this);
						dataSet.ReadXml(new XmlTextReader(new StringReader(text2)), XmlReadMode.DiffGram);
						dataSet.Tables.Remove(this);
					}
				}
			}
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x001E9958 File Offset: 0x001E8D58
		internal void SerializeTableSchema(SerializationInfo info, StreamingContext context, bool isSingleTable)
		{
			info.AddValue("DataTable.TableName", this.TableName);
			info.AddValue("DataTable.Namespace", this.Namespace);
			info.AddValue("DataTable.Prefix", this.Prefix);
			info.AddValue("DataTable.CaseSensitive", this._caseSensitive);
			info.AddValue("DataTable.caseSensitiveAmbient", !this._caseSensitiveUserSet);
			info.AddValue("DataTable.LocaleLCID", this.Locale.LCID);
			info.AddValue("DataTable.MinimumCapacity", this.recordManager.MinimumCapacity);
			info.AddValue("DataTable.NestedInDataSet", this.fNestedInDataset);
			info.AddValue("DataTable.TypeName", this.TypeName.ToString());
			info.AddValue("DataTable.RepeatableElement", this.repeatableElement);
			info.AddValue("DataTable.ExtendedProperties", this.ExtendedProperties);
			info.AddValue("DataTable.Columns.Count", this.Columns.Count);
			if (isSingleTable && !this.CheckForClosureOnExpressionTables(new List<DataTable> { this }))
			{
				throw ExceptionBuilder.CanNotRemoteDataTable();
			}
			IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
			for (int i = 0; i < this.Columns.Count; i++)
			{
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.ColumnName", new object[] { i }), this.Columns[i].ColumnName);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.Namespace", new object[] { i }), this.Columns[i]._columnUri);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.Prefix", new object[] { i }), this.Columns[i].Prefix);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.ColumnMapping", new object[] { i }), this.Columns[i].ColumnMapping);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.AllowDBNull", new object[] { i }), this.Columns[i].AllowDBNull);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.AutoIncrement", new object[] { i }), this.Columns[i].AutoIncrement);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.AutoIncrementStep", new object[] { i }), this.Columns[i].AutoIncrementStep);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.AutoIncrementSeed", new object[] { i }), this.Columns[i].AutoIncrementSeed);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.Caption", new object[] { i }), this.Columns[i].Caption);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.DefaultValue", new object[] { i }), this.Columns[i].DefaultValue);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.ReadOnly", new object[] { i }), this.Columns[i].ReadOnly);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.MaxLength", new object[] { i }), this.Columns[i].MaxLength);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.DataType", new object[] { i }), this.Columns[i].DataType);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.XmlDataType", new object[] { i }), this.Columns[i].XmlDataType);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.SimpleType", new object[] { i }), this.Columns[i].SimpleType);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.DateTimeMode", new object[] { i }), this.Columns[i].DateTimeMode);
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.AutoIncrementCurrent", new object[] { i }), this.Columns[i].autoIncrementCurrent);
				if (isSingleTable)
				{
					info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.Expression", new object[] { i }), this.Columns[i].Expression);
				}
				info.AddValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.ExtendedProperties", new object[] { i }), this.Columns[i].extendedProperties);
			}
			if (isSingleTable)
			{
				this.SerializeConstraints(info, context, 0, false);
			}
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x001E9EA4 File Offset: 0x001E92A4
		internal void DeserializeTableSchema(SerializationInfo info, StreamingContext context, bool isSingleTable)
		{
			this.tableName = info.GetString("DataTable.TableName");
			this.tableNamespace = info.GetString("DataTable.Namespace");
			this.tablePrefix = info.GetString("DataTable.Prefix");
			bool boolean = info.GetBoolean("DataTable.CaseSensitive");
			this.SetCaseSensitiveValue(boolean, true, false);
			this._caseSensitiveUserSet = !info.GetBoolean("DataTable.caseSensitiveAmbient");
			int num = (int)info.GetValue("DataTable.LocaleLCID", typeof(int));
			CultureInfo cultureInfo = new CultureInfo(num);
			this.SetLocaleValue(cultureInfo, true, false);
			this._cultureUserSet = true;
			this.MinimumCapacity = info.GetInt32("DataTable.MinimumCapacity");
			this.fNestedInDataset = info.GetBoolean("DataTable.NestedInDataSet");
			string @string = info.GetString("DataTable.TypeName");
			this.typeName = new XmlQualifiedName(@string);
			this.repeatableElement = info.GetBoolean("DataTable.RepeatableElement");
			this.extendedProperties = (PropertyCollection)info.GetValue("DataTable.ExtendedProperties", typeof(PropertyCollection));
			int @int = info.GetInt32("DataTable.Columns.Count");
			string[] array = new string[@int];
			IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
			for (int i = 0; i < @int; i++)
			{
				DataColumn dataColumn = new DataColumn();
				dataColumn.ColumnName = info.GetString(string.Format(invariantCulture, "DataTable.DataColumn_{0}.ColumnName", new object[] { i }));
				dataColumn._columnUri = info.GetString(string.Format(invariantCulture, "DataTable.DataColumn_{0}.Namespace", new object[] { i }));
				dataColumn.Prefix = info.GetString(string.Format(invariantCulture, "DataTable.DataColumn_{0}.Prefix", new object[] { i }));
				dataColumn.DataType = (Type)info.GetValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.DataType", new object[] { i }), typeof(Type));
				dataColumn.XmlDataType = (string)info.GetValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.XmlDataType", new object[] { i }), typeof(string));
				dataColumn.SimpleType = (SimpleType)info.GetValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.SimpleType", new object[] { i }), typeof(SimpleType));
				dataColumn.ColumnMapping = (MappingType)info.GetValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.ColumnMapping", new object[] { i }), typeof(MappingType));
				dataColumn.DateTimeMode = (DataSetDateTime)info.GetValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.DateTimeMode", new object[] { i }), typeof(DataSetDateTime));
				dataColumn.AllowDBNull = info.GetBoolean(string.Format(invariantCulture, "DataTable.DataColumn_{0}.AllowDBNull", new object[] { i }));
				dataColumn.AutoIncrement = info.GetBoolean(string.Format(invariantCulture, "DataTable.DataColumn_{0}.AutoIncrement", new object[] { i }));
				dataColumn.AutoIncrementStep = info.GetInt64(string.Format(invariantCulture, "DataTable.DataColumn_{0}.AutoIncrementStep", new object[] { i }));
				dataColumn.AutoIncrementSeed = info.GetInt64(string.Format(invariantCulture, "DataTable.DataColumn_{0}.AutoIncrementSeed", new object[] { i }));
				dataColumn.Caption = info.GetString(string.Format(invariantCulture, "DataTable.DataColumn_{0}.Caption", new object[] { i }));
				dataColumn.DefaultValue = info.GetValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.DefaultValue", new object[] { i }), typeof(object));
				dataColumn.ReadOnly = info.GetBoolean(string.Format(invariantCulture, "DataTable.DataColumn_{0}.ReadOnly", new object[] { i }));
				dataColumn.MaxLength = info.GetInt32(string.Format(invariantCulture, "DataTable.DataColumn_{0}.MaxLength", new object[] { i }));
				dataColumn.autoIncrementCurrent = info.GetInt64(string.Format(invariantCulture, "DataTable.DataColumn_{0}.AutoIncrementCurrent", new object[] { i }));
				if (isSingleTable)
				{
					array[i] = info.GetString(string.Format(invariantCulture, "DataTable.DataColumn_{0}.Expression", new object[] { i }));
				}
				dataColumn.extendedProperties = (PropertyCollection)info.GetValue(string.Format(invariantCulture, "DataTable.DataColumn_{0}.ExtendedProperties", new object[] { i }), typeof(PropertyCollection));
				this.Columns.Add(dataColumn);
			}
			if (isSingleTable)
			{
				for (int j = 0; j < @int; j++)
				{
					if (array[j] != null)
					{
						this.Columns[j].Expression = array[j];
					}
				}
			}
			if (isSingleTable)
			{
				this.DeserializeConstraints(info, context, 0, false);
			}
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x001EA3C4 File Offset: 0x001E97C4
		internal void SerializeConstraints(SerializationInfo info, StreamingContext context, int serIndex, bool allConstraints)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < this.Constraints.Count; i++)
			{
				Constraint constraint = this.Constraints[i];
				UniqueConstraint uniqueConstraint = constraint as UniqueConstraint;
				if (uniqueConstraint != null)
				{
					int[] array = new int[uniqueConstraint.Columns.Length];
					for (int j = 0; j < array.Length; j++)
					{
						array[j] = uniqueConstraint.Columns[j].Ordinal;
					}
					arrayList.Add(new ArrayList { "U", uniqueConstraint.ConstraintName, array, uniqueConstraint.IsPrimaryKey, uniqueConstraint.ExtendedProperties });
				}
				else
				{
					ForeignKeyConstraint foreignKeyConstraint = constraint as ForeignKeyConstraint;
					bool flag = allConstraints || (foreignKeyConstraint.Table == this && foreignKeyConstraint.RelatedTable == this);
					if (flag)
					{
						int[] array2 = new int[foreignKeyConstraint.RelatedColumns.Length + 1];
						array2[0] = (allConstraints ? this.DataSet.Tables.IndexOf(foreignKeyConstraint.RelatedTable) : 0);
						for (int k = 1; k < array2.Length; k++)
						{
							array2[k] = foreignKeyConstraint.RelatedColumns[k - 1].Ordinal;
						}
						int[] array3 = new int[foreignKeyConstraint.Columns.Length + 1];
						array3[0] = (allConstraints ? this.DataSet.Tables.IndexOf(foreignKeyConstraint.Table) : 0);
						for (int l = 1; l < array3.Length; l++)
						{
							array3[l] = foreignKeyConstraint.Columns[l - 1].Ordinal;
						}
						arrayList.Add(new ArrayList
						{
							"F",
							foreignKeyConstraint.ConstraintName,
							array2,
							array3,
							new int[]
							{
								(int)foreignKeyConstraint.AcceptRejectRule,
								(int)foreignKeyConstraint.UpdateRule,
								(int)foreignKeyConstraint.DeleteRule
							},
							foreignKeyConstraint.ExtendedProperties
						});
					}
				}
			}
			info.AddValue(string.Format(CultureInfo.InvariantCulture, "DataTable_{0}.Constraints", new object[] { serIndex }), arrayList);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x001EA624 File Offset: 0x001E9A24
		internal void DeserializeConstraints(SerializationInfo info, StreamingContext context, int serIndex, bool allConstraints)
		{
			ArrayList arrayList = (ArrayList)info.GetValue(string.Format(CultureInfo.InvariantCulture, "DataTable_{0}.Constraints", new object[] { serIndex }), typeof(ArrayList));
			foreach (object obj in arrayList)
			{
				ArrayList arrayList2 = (ArrayList)obj;
				string text = (string)arrayList2[0];
				if (text.Equals("U"))
				{
					string text2 = (string)arrayList2[1];
					int[] array = (int[])arrayList2[2];
					bool flag = (bool)arrayList2[3];
					PropertyCollection propertyCollection = (PropertyCollection)arrayList2[4];
					DataColumn[] array2 = new DataColumn[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array2[i] = this.Columns[array[i]];
					}
					UniqueConstraint uniqueConstraint = new UniqueConstraint(text2, array2, flag);
					uniqueConstraint.extendedProperties = propertyCollection;
					this.Constraints.Add(uniqueConstraint);
				}
				else
				{
					string text3 = (string)arrayList2[1];
					int[] array3 = (int[])arrayList2[2];
					int[] array4 = (int[])arrayList2[3];
					int[] array5 = (int[])arrayList2[4];
					PropertyCollection propertyCollection2 = (PropertyCollection)arrayList2[5];
					DataTable dataTable = ((!allConstraints) ? this : this.DataSet.Tables[array3[0]]);
					DataColumn[] array6 = new DataColumn[array3.Length - 1];
					for (int j = 0; j < array6.Length; j++)
					{
						array6[j] = dataTable.Columns[array3[j + 1]];
					}
					DataTable dataTable2 = ((!allConstraints) ? this : this.DataSet.Tables[array4[0]]);
					DataColumn[] array7 = new DataColumn[array4.Length - 1];
					for (int k = 0; k < array7.Length; k++)
					{
						array7[k] = dataTable2.Columns[array4[k + 1]];
					}
					ForeignKeyConstraint foreignKeyConstraint = new ForeignKeyConstraint(text3, array6, array7);
					foreignKeyConstraint.AcceptRejectRule = (AcceptRejectRule)array5[0];
					foreignKeyConstraint.UpdateRule = (Rule)array5[1];
					foreignKeyConstraint.DeleteRule = (Rule)array5[2];
					foreignKeyConstraint.extendedProperties = propertyCollection2;
					this.Constraints.Add(foreignKeyConstraint, false);
				}
			}
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x001EA89C File Offset: 0x001E9C9C
		internal void SerializeExpressionColumns(SerializationInfo info, StreamingContext context, int serIndex)
		{
			int count = this.Columns.Count;
			for (int i = 0; i < count; i++)
			{
				info.AddValue(string.Format(CultureInfo.InvariantCulture, "DataTable_{0}.DataColumn_{1}.Expression", new object[] { serIndex, i }), this.Columns[i].Expression);
			}
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x001EA904 File Offset: 0x001E9D04
		internal void DeserializeExpressionColumns(SerializationInfo info, StreamingContext context, int serIndex)
		{
			int count = this.Columns.Count;
			for (int i = 0; i < count; i++)
			{
				string @string = info.GetString(string.Format(CultureInfo.InvariantCulture, "DataTable_{0}.DataColumn_{1}.Expression", new object[] { serIndex, i }));
				if (@string.Length != 0)
				{
					this.Columns[i].Expression = @string;
				}
			}
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x001EA974 File Offset: 0x001E9D74
		internal void SerializeTableData(SerializationInfo info, StreamingContext context, int serIndex)
		{
			int count = this.Columns.Count;
			int count2 = this.Rows.Count;
			int num = 0;
			int num2 = 0;
			BitArray bitArray = new BitArray(count2 * 3, false);
			int i = 0;
			while (i < count2)
			{
				int num3 = i * 3;
				DataRow dataRow = this.Rows[i];
				DataRowState rowState = dataRow.RowState;
				DataRowState dataRowState = rowState;
				switch (dataRowState)
				{
				case DataRowState.Unchanged:
					break;
				case DataRowState.Detached | DataRowState.Unchanged:
					goto IL_00A6;
				case DataRowState.Added:
					bitArray[num3 + 1] = true;
					break;
				default:
					if (dataRowState != DataRowState.Deleted)
					{
						if (dataRowState != DataRowState.Modified)
						{
							goto IL_00A6;
						}
						bitArray[num3] = true;
						num++;
					}
					else
					{
						bitArray[num3] = true;
						bitArray[num3 + 1] = true;
					}
					break;
				}
				if (-1 != dataRow.tempRecord)
				{
					bitArray[num3 + 2] = true;
					num2++;
				}
				i++;
				continue;
				IL_00A6:
				throw ExceptionBuilder.InvalidRowState(rowState);
			}
			int num4 = count2 + num + num2;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			if (num4 > 0)
			{
				for (int j = 0; j < count; j++)
				{
					object emptyColumnStore = this.Columns[j].GetEmptyColumnStore(num4);
					arrayList.Add(emptyColumnStore);
					BitArray bitArray2 = new BitArray(num4);
					arrayList2.Add(bitArray2);
				}
			}
			int num5 = 0;
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			for (int k = 0; k < count2; k++)
			{
				int num6 = this.Rows[k].CopyValuesIntoStore(arrayList, arrayList2, num5);
				this.GetRowAndColumnErrors(k, hashtable, hashtable2);
				num5 += num6;
			}
			IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
			info.AddValue(string.Format(invariantCulture, "DataTable_{0}.Rows.Count", new object[] { serIndex }), count2);
			info.AddValue(string.Format(invariantCulture, "DataTable_{0}.Records.Count", new object[] { serIndex }), num4);
			info.AddValue(string.Format(invariantCulture, "DataTable_{0}.RowStates", new object[] { serIndex }), bitArray);
			info.AddValue(string.Format(invariantCulture, "DataTable_{0}.Records", new object[] { serIndex }), arrayList);
			info.AddValue(string.Format(invariantCulture, "DataTable_{0}.NullBits", new object[] { serIndex }), arrayList2);
			info.AddValue(string.Format(invariantCulture, "DataTable_{0}.RowErrors", new object[] { serIndex }), hashtable);
			info.AddValue(string.Format(invariantCulture, "DataTable_{0}.ColumnErrors", new object[] { serIndex }), hashtable2);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x001EAC18 File Offset: 0x001EA018
		internal void DeserializeTableData(SerializationInfo info, StreamingContext context, int serIndex)
		{
			bool flag = this.enforceConstraints;
			bool flag2 = this.inDataLoad;
			try
			{
				this.enforceConstraints = false;
				this.inDataLoad = true;
				IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
				int @int = info.GetInt32(string.Format(invariantCulture, "DataTable_{0}.Rows.Count", new object[] { serIndex }));
				int int2 = info.GetInt32(string.Format(invariantCulture, "DataTable_{0}.Records.Count", new object[] { serIndex }));
				BitArray bitArray = (BitArray)info.GetValue(string.Format(invariantCulture, "DataTable_{0}.RowStates", new object[] { serIndex }), typeof(BitArray));
				ArrayList arrayList = (ArrayList)info.GetValue(string.Format(invariantCulture, "DataTable_{0}.Records", new object[] { serIndex }), typeof(ArrayList));
				ArrayList arrayList2 = (ArrayList)info.GetValue(string.Format(invariantCulture, "DataTable_{0}.NullBits", new object[] { serIndex }), typeof(ArrayList));
				Hashtable hashtable = (Hashtable)info.GetValue(string.Format(invariantCulture, "DataTable_{0}.RowErrors", new object[] { serIndex }), typeof(Hashtable));
				hashtable.OnDeserialization(this);
				Hashtable hashtable2 = (Hashtable)info.GetValue(string.Format(invariantCulture, "DataTable_{0}.ColumnErrors", new object[] { serIndex }), typeof(Hashtable));
				hashtable2.OnDeserialization(this);
				if (int2 > 0)
				{
					for (int i = 0; i < this.Columns.Count; i++)
					{
						this.Columns[i].SetStorage(arrayList[i], (BitArray)arrayList2[i]);
					}
					int num = 0;
					DataRow[] array = new DataRow[int2];
					for (int j = 0; j < @int; j++)
					{
						DataRow dataRow = this.NewEmptyRow();
						array[num] = dataRow;
						int num2 = j * 3;
						DataRowState dataRowState = this.ConvertToRowState(bitArray, num2);
						switch (dataRowState)
						{
						case DataRowState.Unchanged:
							dataRow.oldRecord = num;
							dataRow.newRecord = num;
							num++;
							break;
						case DataRowState.Detached | DataRowState.Unchanged:
							break;
						case DataRowState.Added:
							dataRow.oldRecord = -1;
							dataRow.newRecord = num;
							num++;
							break;
						default:
							if (dataRowState != DataRowState.Deleted)
							{
								if (dataRowState == DataRowState.Modified)
								{
									dataRow.oldRecord = num;
									dataRow.newRecord = num + 1;
									array[num + 1] = dataRow;
									num += 2;
								}
							}
							else
							{
								dataRow.oldRecord = num;
								dataRow.newRecord = -1;
								num++;
							}
							break;
						}
						if (bitArray[num2 + 2])
						{
							dataRow.tempRecord = num;
							array[num] = dataRow;
							num++;
						}
						else
						{
							dataRow.tempRecord = -1;
						}
						this.Rows.ArrayAdd(dataRow);
						dataRow.rowID = this.nextRowID;
						this.nextRowID += 1L;
						this.ConvertToRowError(j, hashtable, hashtable2);
					}
					this.recordManager.SetRowCache(array);
					this.ResetIndexes();
				}
			}
			finally
			{
				this.enforceConstraints = flag;
				this.inDataLoad = flag2;
			}
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x001EAF50 File Offset: 0x001EA350
		private DataRowState ConvertToRowState(BitArray bitStates, int bitIndex)
		{
			bool flag = bitStates[bitIndex];
			bool flag2 = bitStates[bitIndex + 1];
			if (!flag && !flag2)
			{
				return DataRowState.Unchanged;
			}
			if (!flag && flag2)
			{
				return DataRowState.Added;
			}
			if (flag && !flag2)
			{
				return DataRowState.Modified;
			}
			if (flag && flag2)
			{
				return DataRowState.Deleted;
			}
			throw ExceptionBuilder.InvalidRowBitPattern();
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x001EAF98 File Offset: 0x001EA398
		internal void GetRowAndColumnErrors(int rowIndex, Hashtable rowErrors, Hashtable colErrors)
		{
			DataRow dataRow = this.Rows[rowIndex];
			if (dataRow.HasErrors)
			{
				rowErrors.Add(rowIndex, dataRow.RowError);
				DataColumn[] columnsInError = dataRow.GetColumnsInError();
				if (columnsInError.Length > 0)
				{
					int[] array = new int[columnsInError.Length];
					string[] array2 = new string[columnsInError.Length];
					for (int i = 0; i < columnsInError.Length; i++)
					{
						array[i] = columnsInError[i].Ordinal;
						array2[i] = dataRow.GetColumnError(columnsInError[i]);
					}
					ArrayList arrayList = new ArrayList();
					arrayList.Add(array);
					arrayList.Add(array2);
					colErrors.Add(rowIndex, arrayList);
				}
			}
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x001EB03C File Offset: 0x001EA43C
		private void ConvertToRowError(int rowIndex, Hashtable rowErrors, Hashtable colErrors)
		{
			DataRow dataRow = this.Rows[rowIndex];
			if (rowErrors.ContainsKey(rowIndex))
			{
				dataRow.RowError = (string)rowErrors[rowIndex];
			}
			if (colErrors.ContainsKey(rowIndex))
			{
				ArrayList arrayList = (ArrayList)colErrors[rowIndex];
				int[] array = (int[])arrayList[0];
				string[] array2 = (string[])arrayList[1];
				for (int i = 0; i < array.Length; i++)
				{
					dataRow.SetColumnError(array[i], array2[i]);
				}
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x001EB0D4 File Offset: 0x001EA4D4
		// (set) Token: 0x0600093D RID: 2365 RVA: 0x001EB0E8 File Offset: 0x001EA4E8
		[ResDescription("DataTableCaseSensitiveDescr")]
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
					bool caseSensitiveUserSet = this._caseSensitiveUserSet;
					this._caseSensitive = value;
					this._caseSensitiveUserSet = true;
					if (this.DataSet != null && !this.DataSet.ValidateCaseConstraint())
					{
						this._caseSensitive = caseSensitive;
						this._caseSensitiveUserSet = caseSensitiveUserSet;
						throw ExceptionBuilder.CannotChangeCaseLocale();
					}
					this.SetCaseSensitiveValue(value, true, true);
				}
				this._caseSensitiveUserSet = true;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x001EB154 File Offset: 0x001EA554
		internal bool AreIndexEventsSuspended
		{
			get
			{
				return 0 < this._suspendIndexEvents;
			}
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x001EB16C File Offset: 0x001EA56C
		internal void RestoreIndexEvents(bool forceReset)
		{
			Bid.Trace("<ds.DataTable.RestoreIndexEvents|Info> %d#, %d\n", this.ObjectID, this._suspendIndexEvents);
			if (0 < this._suspendIndexEvents)
			{
				this._suspendIndexEvents--;
				if (this._suspendIndexEvents == 0)
				{
					Exception ex = null;
					this.SetShadowIndexes();
					try
					{
						int count = this.shadowIndexes.Count;
						for (int i = 0; i < count; i++)
						{
							Index index = this.shadowIndexes[i];
							try
							{
								if (forceReset || index.HasRemoteAggregate)
								{
									index.Reset();
								}
								else
								{
									index.FireResetEvent();
								}
							}
							catch (Exception ex2)
							{
								if (!ADP.IsCatchableExceptionType(ex2))
								{
									throw;
								}
								ExceptionBuilder.TraceExceptionWithoutRethrow(ex2);
								if (ex == null)
								{
									ex = ex2;
								}
							}
						}
						if (ex != null)
						{
							throw ex;
						}
					}
					finally
					{
						this.RestoreShadowIndexes();
					}
				}
			}
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x001EB258 File Offset: 0x001EA658
		internal void SuspendIndexEvents()
		{
			Bid.Trace("<ds.DataTable.SuspendIndexEvents|Info> %d#, %d\n", this.ObjectID, this._suspendIndexEvents);
			this._suspendIndexEvents++;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x001EB28C File Offset: 0x001EA68C
		[Browsable(false)]
		public bool IsInitialized
		{
			get
			{
				return !this.fInitInProgress;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x001EB2A4 File Offset: 0x001EA6A4
		private bool IsTypedDataTable
		{
			get
			{
				switch (this._isTypedDataTable)
				{
				case 0:
					this._isTypedDataTable = ((base.GetType() != typeof(DataTable)) ? 1 : 2);
					return 1 == this._isTypedDataTable;
				case 1:
					return true;
				default:
					return false;
				}
			}
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x001EB2F4 File Offset: 0x001EA6F4
		internal bool SetCaseSensitiveValue(bool isCaseSensitive, bool userSet, bool resetIndexes)
		{
			if (userSet || (!this._caseSensitiveUserSet && this._caseSensitive != isCaseSensitive))
			{
				this._caseSensitive = isCaseSensitive;
				if (isCaseSensitive)
				{
					this._compareFlags = CompareOptions.None;
				}
				else
				{
					this._compareFlags = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
				}
				if (resetIndexes)
				{
					this.ResetIndexes();
					foreach (object obj in this.Constraints)
					{
						Constraint constraint = (Constraint)obj;
						constraint.CheckConstraint();
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x001EB394 File Offset: 0x001EA794
		private void ResetCaseSensitive()
		{
			this.SetCaseSensitiveValue(this.dataSet != null && this.dataSet.CaseSensitive, true, true);
			this._caseSensitiveUserSet = false;
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x001EB3C8 File Offset: 0x001EA7C8
		internal bool ShouldSerializeCaseSensitive()
		{
			return this._caseSensitiveUserSet;
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x001EB3DC File Offset: 0x001EA7DC
		internal bool SelfNested
		{
			get
			{
				foreach (object obj in this.ParentRelations)
				{
					DataRelation dataRelation = (DataRelation)obj;
					if (dataRelation.Nested && dataRelation.ParentTable == this)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x001EB454 File Offset: 0x001EA854
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal List<Index> LiveIndexes
		{
			get
			{
				if (!this.AreIndexEventsSuspended)
				{
					int num = this.indexes.Count - 1;
					while (0 <= num)
					{
						Index index = this.indexes[num];
						if (index.RefCount <= 1)
						{
							index.RemoveRef();
						}
						num--;
					}
				}
				return this.indexes;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000948 RID: 2376 RVA: 0x001EB4A4 File Offset: 0x001EA8A4
		// (set) Token: 0x06000949 RID: 2377 RVA: 0x001EB4B8 File Offset: 0x001EA8B8
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
				if (this.DataSet != null && value != this.DataSet.RemotingFormat)
				{
					throw ExceptionBuilder.CanNotSetRemotingFormat();
				}
				this._remotingFormat = value;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x001EB4F8 File Offset: 0x001EA8F8
		// (set) Token: 0x0600094B RID: 2379 RVA: 0x001EB50C File Offset: 0x001EA90C
		internal int UKColumnPositionForInference
		{
			get
			{
				return this.ukColumnPositionForInference;
			}
			set
			{
				this.ukColumnPositionForInference = value;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x001EB520 File Offset: 0x001EA920
		[ResDescription("DataTableChildRelationsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public DataRelationCollection ChildRelations
		{
			get
			{
				if (this.childRelationsCollection == null)
				{
					this.childRelationsCollection = new DataRelationCollection.DataTableRelationCollection(this, false);
				}
				return this.childRelationsCollection;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x001EB548 File Offset: 0x001EA948
		[ResCategory("DataCategory_Data")]
		[ResDescription("DataTableColumnsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public DataColumnCollection Columns
		{
			get
			{
				return this.columnCollection;
			}
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x001EB55C File Offset: 0x001EA95C
		private void ResetColumns()
		{
			this.Columns.Clear();
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x001EB574 File Offset: 0x001EA974
		private CompareInfo CompareInfo
		{
			get
			{
				if (this._compareInfo == null)
				{
					this._compareInfo = this.Locale.CompareInfo;
				}
				return this._compareInfo;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x001EB5A0 File Offset: 0x001EA9A0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[ResCategory("DataCategory_Data")]
		[ResDescription("DataTableConstraintsDescr")]
		public ConstraintCollection Constraints
		{
			get
			{
				return this.constraintCollection;
			}
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x001EB5B4 File Offset: 0x001EA9B4
		private void ResetConstraints()
		{
			this.Constraints.Clear();
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x001EB5CC File Offset: 0x001EA9CC
		[ResDescription("DataTableDataSetDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public DataSet DataSet
		{
			get
			{
				return this.dataSet;
			}
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x001EB5E0 File Offset: 0x001EA9E0
		internal void SetDataSet(DataSet dataSet)
		{
			if (this.dataSet != dataSet)
			{
				this.dataSet = dataSet;
				DataColumnCollection columns = this.Columns;
				for (int i = 0; i < columns.Count; i++)
				{
					columns[i].OnSetDataSet();
				}
				if (this.DataSet != null)
				{
					this.defaultView = null;
				}
				if (dataSet != null)
				{
					this._remotingFormat = dataSet.RemotingFormat;
				}
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x001EB640 File Offset: 0x001EAA40
		[Browsable(false)]
		[ResDescription("DataTableDefaultViewDescr")]
		public DataView DefaultView
		{
			get
			{
				DataView dataView = this.defaultView;
				if (dataView == null)
				{
					if (this.dataSet != null)
					{
						dataView = this.dataSet.DefaultViewManager.CreateDataView(this);
					}
					else
					{
						dataView = new DataView(this, true);
						dataView.SetIndex2("", DataViewRowState.CurrentRows, null, true);
					}
					dataView = Interlocked.CompareExchange<DataView>(ref this.defaultView, dataView, null);
					if (dataView == null)
					{
						dataView = this.defaultView;
					}
				}
				return dataView;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x001EB6A4 File Offset: 0x001EAAA4
		// (set) Token: 0x06000956 RID: 2390 RVA: 0x001EB6B8 File Offset: 0x001EAAB8
		[ResDescription("DataTableDisplayExpressionDescr")]
		[ResCategory("DataCategory_Data")]
		[DefaultValue("")]
		public string DisplayExpression
		{
			get
			{
				return this.DisplayExpressionInternal;
			}
			set
			{
				if (value != null && value.Length > 0)
				{
					this.displayExpression = new DataExpression(this, value);
					return;
				}
				this.displayExpression = null;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x001EB6E8 File Offset: 0x001EAAE8
		internal string DisplayExpressionInternal
		{
			get
			{
				if (this.displayExpression == null)
				{
					return "";
				}
				return this.displayExpression.Expression;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x001EB710 File Offset: 0x001EAB10
		// (set) Token: 0x06000959 RID: 2393 RVA: 0x001EB744 File Offset: 0x001EAB44
		internal bool EnforceConstraints
		{
			get
			{
				if (this.SuspendEnforceConstraints)
				{
					return false;
				}
				if (this.dataSet != null)
				{
					return this.dataSet.EnforceConstraints;
				}
				return this.enforceConstraints;
			}
			set
			{
				if (this.dataSet == null && this.enforceConstraints != value)
				{
					if (value)
					{
						this.EnableConstraints();
					}
					this.enforceConstraints = value;
				}
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x001EB774 File Offset: 0x001EAB74
		// (set) Token: 0x0600095B RID: 2395 RVA: 0x001EB788 File Offset: 0x001EAB88
		internal bool SuspendEnforceConstraints
		{
			get
			{
				return this._suspendEnforceConstraints;
			}
			set
			{
				this._suspendEnforceConstraints = value;
			}
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x001EB79C File Offset: 0x001EAB9C
		internal void EnableConstraints()
		{
			bool flag = false;
			foreach (object obj in this.Constraints)
			{
				Constraint constraint = (Constraint)obj;
				if (constraint is UniqueConstraint)
				{
					flag |= constraint.IsConstraintViolated();
				}
			}
			foreach (object obj2 in this.Columns)
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
			if (flag)
			{
				this.EnforceConstraints = false;
				throw ExceptionBuilder.EnforceConstraint();
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x001EB898 File Offset: 0x001EAC98
		[Browsable(false)]
		[ResCategory("DataCategory_Data")]
		[ResDescription("ExtendedPropertiesDescr")]
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

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x001EB8C0 File Offset: 0x001EACC0
		internal IFormatProvider FormatProvider
		{
			get
			{
				if (this._formatProvider == null)
				{
					CultureInfo cultureInfo = this.Locale;
					if (cultureInfo.IsNeutralCulture)
					{
						cultureInfo = CultureInfo.InvariantCulture;
					}
					this._formatProvider = cultureInfo;
				}
				return this._formatProvider;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x001EB8F8 File Offset: 0x001EACF8
		[ResDescription("DataTableHasErrorsDescr")]
		[Browsable(false)]
		public bool HasErrors
		{
			get
			{
				for (int i = 0; i < this.Rows.Count; i++)
				{
					if (this.Rows[i].HasErrors)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x001EB934 File Offset: 0x001EAD34
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x001EB948 File Offset: 0x001EAD48
		[ResDescription("DataTableLocaleDescr")]
		public CultureInfo Locale
		{
			get
			{
				return this._culture;
			}
			set
			{
				IntPtr intPtr;
				Bid.ScopeEnter(out intPtr, "<ds.DataTable.set_Locale|API> %d#\n", this.ObjectID);
				try
				{
					bool flag = true;
					if (value == null)
					{
						flag = false;
						value = ((this.dataSet != null) ? this.dataSet.Locale : this._culture);
					}
					if (this._culture != value && !this._culture.Equals(value))
					{
						bool flag2 = false;
						bool flag3 = false;
						CultureInfo culture = this._culture;
						bool cultureUserSet = this._cultureUserSet;
						try
						{
							this._cultureUserSet = true;
							this.SetLocaleValue(value, true, false);
							if (this.DataSet == null || this.DataSet.ValidateLocaleConstraint())
							{
								flag2 = false;
								this.SetLocaleValue(value, true, true);
								flag2 = true;
							}
						}
						catch
						{
							flag3 = true;
							throw;
						}
						finally
						{
							if (!flag2)
							{
								try
								{
									this.SetLocaleValue(culture, true, true);
								}
								catch (Exception ex)
								{
									if (!ADP.IsCatchableExceptionType(ex))
									{
										throw;
									}
									ADP.TraceExceptionWithoutRethrow(ex);
								}
								this._cultureUserSet = cultureUserSet;
								if (!flag3)
								{
									throw ExceptionBuilder.CannotChangeCaseLocale(null);
								}
							}
						}
						this.SetLocaleValue(value, true, true);
					}
					this._cultureUserSet = flag;
				}
				finally
				{
					Bid.ScopeLeave(ref intPtr);
				}
			}
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x001EBAB0 File Offset: 0x001EAEB0
		internal bool SetLocaleValue(CultureInfo culture, bool userSet, bool resetIndexes)
		{
			if (userSet || resetIndexes || (!this._cultureUserSet && !this._culture.Equals(culture)))
			{
				this._culture = culture;
				this._compareInfo = null;
				this._formatProvider = null;
				this._hashCodeProvider = null;
				foreach (object obj in this.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj;
					dataColumn._hashCode = this.GetSpecialHashCode(dataColumn.ColumnName);
				}
				if (resetIndexes)
				{
					this.ResetIndexes();
					foreach (object obj2 in this.Constraints)
					{
						Constraint constraint = (Constraint)obj2;
						constraint.CheckConstraint();
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x001EBBC4 File Offset: 0x001EAFC4
		internal bool ShouldSerializeLocale()
		{
			return this._cultureUserSet;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x001EBBD8 File Offset: 0x001EAFD8
		// (set) Token: 0x06000965 RID: 2405 RVA: 0x001EBBF0 File Offset: 0x001EAFF0
		[ResDescription("DataTableMinimumCapacityDescr")]
		[ResCategory("DataCategory_Data")]
		[DefaultValue(50)]
		public int MinimumCapacity
		{
			get
			{
				return this.recordManager.MinimumCapacity;
			}
			set
			{
				if (value != this.recordManager.MinimumCapacity)
				{
					this.recordManager.MinimumCapacity = value;
				}
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x001EBC18 File Offset: 0x001EB018
		internal int RecordCapacity
		{
			get
			{
				return this.recordManager.RecordCapacity;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x001EBC30 File Offset: 0x001EB030
		// (set) Token: 0x06000968 RID: 2408 RVA: 0x001EBC44 File Offset: 0x001EB044
		internal int ElementColumnCount
		{
			get
			{
				return this.elementColumnCount;
			}
			set
			{
				if (value > 0 && this.xmlText != null)
				{
					throw ExceptionBuilder.TableCannotAddToSimpleContent();
				}
				this.elementColumnCount = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x001EBC6C File Offset: 0x001EB06C
		[Browsable(false)]
		[ResDescription("DataTableParentRelationsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataRelationCollection ParentRelations
		{
			get
			{
				if (this.parentRelationsCollection == null)
				{
					this.parentRelationsCollection = new DataRelationCollection.DataTableRelationCollection(this, true);
				}
				return this.parentRelationsCollection;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x001EBC94 File Offset: 0x001EB094
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x001EBCA8 File Offset: 0x001EB0A8
		internal bool MergingData
		{
			get
			{
				return this.mergingData;
			}
			set
			{
				this.mergingData = value;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x001EBCBC File Offset: 0x001EB0BC
		internal DataRelation[] NestedParentRelations
		{
			get
			{
				return this._nestedParentRelations;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x001EBCD0 File Offset: 0x001EB0D0
		internal bool SchemaLoading
		{
			get
			{
				return this.schemaLoading;
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x001EBCE4 File Offset: 0x001EB0E4
		internal void CacheNestedParent()
		{
			this._nestedParentRelations = this.FindNestedParentRelations();
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x001EBD00 File Offset: 0x001EB100
		private DataRelation[] FindNestedParentRelations()
		{
			List<DataRelation> list = null;
			foreach (object obj in this.ParentRelations)
			{
				DataRelation dataRelation = (DataRelation)obj;
				if (dataRelation.Nested)
				{
					if (list == null)
					{
						list = new List<DataRelation>();
					}
					list.Add(dataRelation);
				}
			}
			if (list == null || list.Count == 0)
			{
				return DataTable.EmptyArrayDataRelation;
			}
			return list.ToArray();
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x001EBD90 File Offset: 0x001EB190
		internal int NestedParentsCount
		{
			get
			{
				int num = 0;
				foreach (object obj in this.ParentRelations)
				{
					DataRelation dataRelation = (DataRelation)obj;
					if (dataRelation.Nested)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x001EBE00 File Offset: 0x001EB200
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x001EBE2C File Offset: 0x001EB22C
		[Editor("Microsoft.VSDesigner.Data.Design.PrimaryKeyEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResCategory("DataCategory_Data")]
		[TypeConverter(typeof(PrimaryKeyTypeConverter))]
		[ResDescription("DataTablePrimaryKeyDescr")]
		public DataColumn[] PrimaryKey
		{
			get
			{
				UniqueConstraint uniqueConstraint = this.primaryKey;
				if (uniqueConstraint != null)
				{
					return uniqueConstraint.Key.ToArray();
				}
				return DataTable.zeroColumns;
			}
			set
			{
				UniqueConstraint uniqueConstraint = null;
				if (this.fInitInProgress && value != null)
				{
					this.delayedSetPrimaryKey = value;
					return;
				}
				if (value != null && value.Length != 0)
				{
					int num = 0;
					int num2 = 0;
					while (num2 < value.Length && value[num2] != null)
					{
						num++;
						num2++;
					}
					if (num != 0)
					{
						DataColumn[] array = value;
						if (num != value.Length)
						{
							array = new DataColumn[num];
							for (int i = 0; i < num; i++)
							{
								array[i] = value[i];
							}
						}
						uniqueConstraint = new UniqueConstraint(array);
						if (uniqueConstraint.Table != this)
						{
							throw ExceptionBuilder.TableForeignPrimaryKey();
						}
					}
				}
				if (uniqueConstraint == this.primaryKey || (uniqueConstraint != null && uniqueConstraint.Equals(this.primaryKey)))
				{
					return;
				}
				UniqueConstraint uniqueConstraint2;
				if ((uniqueConstraint2 = (UniqueConstraint)this.Constraints.FindConstraint(uniqueConstraint)) != null)
				{
					uniqueConstraint.ColumnsReference.CopyTo(uniqueConstraint2.Key.ColumnsReference, 0);
					uniqueConstraint = uniqueConstraint2;
				}
				UniqueConstraint uniqueConstraint3 = this.primaryKey;
				this.primaryKey = null;
				if (uniqueConstraint3 != null)
				{
					uniqueConstraint3.ConstraintIndex.RemoveRef();
					if (this.loadIndex != null)
					{
						this.loadIndex.RemoveRef();
						this.loadIndex = null;
					}
					if (this.loadIndexwithOriginalAdded != null)
					{
						this.loadIndexwithOriginalAdded.RemoveRef();
						this.loadIndexwithOriginalAdded = null;
					}
					if (this.loadIndexwithCurrentDeleted != null)
					{
						this.loadIndexwithCurrentDeleted.RemoveRef();
						this.loadIndexwithCurrentDeleted = null;
					}
					this.Constraints.Remove(uniqueConstraint3);
				}
				if (uniqueConstraint != null && uniqueConstraint2 == null)
				{
					this.Constraints.Add(uniqueConstraint);
				}
				this.primaryKey = uniqueConstraint;
				this._primaryIndex = ((uniqueConstraint != null) ? uniqueConstraint.Key.GetIndexDesc() : DataTable.zeroIndexField);
				if (this.primaryKey != null)
				{
					uniqueConstraint.ConstraintIndex.AddRef();
					for (int j = 0; j < uniqueConstraint.ColumnsReference.Length; j++)
					{
						uniqueConstraint.ColumnsReference[j].AllowDBNull = false;
					}
				}
			}
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x001EBFF0 File Offset: 0x001EB3F0
		private bool ShouldSerializePrimaryKey()
		{
			return this.primaryKey != null;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x001EC00C File Offset: 0x001EB40C
		private void ResetPrimaryKey()
		{
			this.PrimaryKey = null;
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x001EC020 File Offset: 0x001EB420
		[ResDescription("DataTableRowsDescr")]
		[Browsable(false)]
		public DataRowCollection Rows
		{
			get
			{
				return this.rowCollection;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x001EC034 File Offset: 0x001EB434
		// (set) Token: 0x06000977 RID: 2423 RVA: 0x001EC048 File Offset: 0x001EB448
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Data")]
		[ResDescription("DataTableTableNameDescr")]
		[DefaultValue("")]
		public string TableName
		{
			get
			{
				return this.tableName;
			}
			set
			{
				IntPtr intPtr;
				Bid.ScopeEnter(out intPtr, "<ds.DataTable.set_TableName|API> %d#, value='%ls'\n", this.ObjectID, value);
				try
				{
					if (value == null)
					{
						value = "";
					}
					CultureInfo locale = this.Locale;
					if (string.Compare(this.tableName, value, true, locale) != 0)
					{
						if (this.dataSet != null)
						{
							if (value.Length == 0)
							{
								throw ExceptionBuilder.NoTableName();
							}
							if (string.Compare(value, this.dataSet.DataSetName, true, this.dataSet.Locale) == 0 && !this.fNestedInDataset)
							{
								throw ExceptionBuilder.DatasetConflictingName(this.dataSet.DataSetName);
							}
							DataRelation[] nestedParentRelations = this.NestedParentRelations;
							if (nestedParentRelations.Length == 0)
							{
								this.dataSet.Tables.RegisterName(value, this.Namespace);
							}
							else
							{
								foreach (DataRelation dataRelation in nestedParentRelations)
								{
									if (!dataRelation.ParentTable.Columns.CanRegisterName(value))
									{
										throw ExceptionBuilder.CannotAddDuplicate2(value);
									}
								}
								this.dataSet.Tables.RegisterName(value, this.Namespace);
								foreach (DataRelation dataRelation2 in nestedParentRelations)
								{
									dataRelation2.ParentTable.Columns.RegisterColumnName(value, null, this);
									dataRelation2.ParentTable.Columns.UnregisterName(this.TableName);
								}
							}
							if (this.tableName.Length != 0)
							{
								this.dataSet.Tables.UnregisterName(this.tableName);
							}
						}
						this.RaisePropertyChanging("TableName");
						this.tableName = value;
						this.encodedTableName = null;
					}
					else if (string.Compare(this.tableName, value, false, locale) != 0)
					{
						this.RaisePropertyChanging("TableName");
						this.tableName = value;
						this.encodedTableName = null;
					}
				}
				finally
				{
					Bid.ScopeLeave(ref intPtr);
				}
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x001EC220 File Offset: 0x001EB620
		internal string EncodedTableName
		{
			get
			{
				string text = this.encodedTableName;
				if (text == null)
				{
					text = XmlConvert.EncodeLocalName(this.TableName);
					this.encodedTableName = text;
				}
				return text;
			}
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x001EC24C File Offset: 0x001EB64C
		private string GetInheritedNamespace(List<DataTable> visitedTables)
		{
			DataRelation[] nestedParentRelations = this.NestedParentRelations;
			if (nestedParentRelations.Length > 0)
			{
				foreach (DataRelation dataRelation in nestedParentRelations)
				{
					if (dataRelation.ParentTable.tableNamespace != null)
					{
						return dataRelation.ParentTable.tableNamespace;
					}
				}
				int num = 0;
				while (num < nestedParentRelations.Length && (nestedParentRelations[num].ParentTable == this || visitedTables.Contains(nestedParentRelations[num].ParentTable)))
				{
					num++;
				}
				if (num < nestedParentRelations.Length)
				{
					DataTable parentTable = nestedParentRelations[num].ParentTable;
					if (!visitedTables.Contains(parentTable))
					{
						visitedTables.Add(parentTable);
					}
					return parentTable.GetInheritedNamespace(visitedTables);
				}
			}
			if (this.DataSet != null)
			{
				return this.DataSet.Namespace;
			}
			return string.Empty;
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x001EC300 File Offset: 0x001EB700
		// (set) Token: 0x0600097B RID: 2427 RVA: 0x001EC328 File Offset: 0x001EB728
		[ResDescription("DataTableNamespaceDescr")]
		[ResCategory("DataCategory_Data")]
		public string Namespace
		{
			get
			{
				if (this.tableNamespace == null)
				{
					return this.GetInheritedNamespace(new List<DataTable>());
				}
				return this.tableNamespace;
			}
			set
			{
				IntPtr intPtr;
				Bid.ScopeEnter(out intPtr, "<ds.DataTable.set_Namespace|API> %d#, value='%ls'\n", this.ObjectID, value);
				try
				{
					if (value != this.tableNamespace)
					{
						if (this.dataSet != null)
						{
							string text = ((value == null) ? this.GetInheritedNamespace(new List<DataTable>()) : value);
							if (text != this.Namespace)
							{
								if (this.dataSet.Tables.Contains(this.TableName, text, true, true))
								{
									throw ExceptionBuilder.DuplicateTableName2(this.TableName, text);
								}
								this.CheckCascadingNamespaceConflict(text);
							}
						}
						this.CheckNamespaceValidityForNestedRelations(value);
						this.DoRaiseNamespaceChange();
					}
					this.tableNamespace = value;
				}
				finally
				{
					Bid.ScopeLeave(ref intPtr);
				}
			}
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x001EC3E8 File Offset: 0x001EB7E8
		internal bool IsNamespaceInherited()
		{
			return null == this.tableNamespace;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x001EC400 File Offset: 0x001EB800
		internal void CheckCascadingNamespaceConflict(string realNamespace)
		{
			foreach (object obj in this.ChildRelations)
			{
				DataRelation dataRelation = (DataRelation)obj;
				if (dataRelation.Nested && dataRelation.ChildTable != this && dataRelation.ChildTable.tableNamespace == null)
				{
					DataTable childTable = dataRelation.ChildTable;
					if (this.dataSet.Tables.Contains(childTable.TableName, realNamespace, false, true))
					{
						throw ExceptionBuilder.DuplicateTableName2(this.TableName, realNamespace);
					}
					childTable.CheckCascadingNamespaceConflict(realNamespace);
				}
			}
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x001EC4B4 File Offset: 0x001EB8B4
		internal void CheckNamespaceValidityForNestedRelations(string realNamespace)
		{
			foreach (object obj in this.ChildRelations)
			{
				DataRelation dataRelation = (DataRelation)obj;
				if (dataRelation.Nested)
				{
					if (realNamespace != null)
					{
						dataRelation.ChildTable.CheckNamespaceValidityForNestedParentRelations(realNamespace, this);
					}
					else
					{
						dataRelation.ChildTable.CheckNamespaceValidityForNestedParentRelations(this.GetInheritedNamespace(new List<DataTable>()), this);
					}
				}
			}
			if (realNamespace == null)
			{
				this.CheckNamespaceValidityForNestedParentRelations(this.GetInheritedNamespace(new List<DataTable>()), this);
			}
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x001EC558 File Offset: 0x001EB958
		internal void CheckNamespaceValidityForNestedParentRelations(string ns, DataTable parentTable)
		{
			foreach (object obj in this.ParentRelations)
			{
				DataRelation dataRelation = (DataRelation)obj;
				if (dataRelation.Nested && dataRelation.ParentTable != parentTable && dataRelation.ParentTable.Namespace != ns)
				{
					throw ExceptionBuilder.InValidNestedRelation(this.TableName);
				}
			}
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x001EC5E8 File Offset: 0x001EB9E8
		internal void DoRaiseNamespaceChange()
		{
			this.RaisePropertyChanging("Namespace");
			foreach (object obj in this.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				if (dataColumn._columnUri == null)
				{
					dataColumn.RaisePropertyChanging("Namespace");
				}
			}
			foreach (object obj2 in this.ChildRelations)
			{
				DataRelation dataRelation = (DataRelation)obj2;
				if (dataRelation.Nested && dataRelation.ChildTable != this)
				{
					DataTable childTable = dataRelation.ChildTable;
					dataRelation.ChildTable.DoRaiseNamespaceChange();
				}
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x001EC6DC File Offset: 0x001EBADC
		private bool ShouldSerializeNamespace()
		{
			return this.tableNamespace != null;
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x001EC6F8 File Offset: 0x001EBAF8
		private void ResetNamespace()
		{
			this.Namespace = null;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x001EC70C File Offset: 0x001EBB0C
		public virtual void BeginInit()
		{
			this.fInitInProgress = true;
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x001EC720 File Offset: 0x001EBB20
		public virtual void EndInit()
		{
			if (this.dataSet == null || !this.dataSet.fInitInProgress)
			{
				this.Columns.FinishInitCollection();
				this.Constraints.FinishInitConstraints();
				foreach (object obj in this.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj;
					if (dataColumn.Computed)
					{
						dataColumn.Expression = dataColumn.Expression;
					}
				}
			}
			this.fInitInProgress = false;
			if (this.delayedSetPrimaryKey != null)
			{
				this.PrimaryKey = this.delayedSetPrimaryKey;
				this.delayedSetPrimaryKey = null;
			}
			if (this.delayedViews.Count > 0)
			{
				foreach (DataView dataView in this.delayedViews)
				{
					dataView.EndInit();
				}
				this.delayedViews.Clear();
			}
			this.OnInitialized();
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x001EC84C File Offset: 0x001EBC4C
		// (set) Token: 0x06000986 RID: 2438 RVA: 0x001EC860 File Offset: 0x001EBC60
		[ResCategory("DataCategory_Data")]
		[DefaultValue("")]
		[ResDescription("DataTablePrefixDescr")]
		public string Prefix
		{
			get
			{
				return this.tablePrefix;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				Bid.Trace("<ds.DataTable.set_Prefix|API> %d#, value='%ls'\n", this.ObjectID, value);
				if (XmlConvert.DecodeName(value) == value && XmlConvert.EncodeName(value) != value)
				{
					throw ExceptionBuilder.InvalidPrefix(value);
				}
				this.tablePrefix = value;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x001EC8B4 File Offset: 0x001EBCB4
		// (set) Token: 0x06000988 RID: 2440 RVA: 0x001EC8C8 File Offset: 0x001EBCC8
		internal DataColumn XmlText
		{
			get
			{
				return this.xmlText;
			}
			set
			{
				if (this.xmlText != value)
				{
					if (this.xmlText != null)
					{
						if (value != null)
						{
							throw ExceptionBuilder.MultipleTextOnlyColumns();
						}
						this.Columns.Remove(this.xmlText);
					}
					else if (value != this.Columns[value.ColumnName])
					{
						this.Columns.Add(value);
					}
					this.xmlText = value;
				}
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x001EC92C File Offset: 0x001EBD2C
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x001EC940 File Offset: 0x001EBD40
		internal decimal MaxOccurs
		{
			get
			{
				return this.maxOccurs;
			}
			set
			{
				this.maxOccurs = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x001EC954 File Offset: 0x001EBD54
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x001EC968 File Offset: 0x001EBD68
		internal decimal MinOccurs
		{
			get
			{
				return this.minOccurs;
			}
			set
			{
				this.minOccurs = value;
			}
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x001EC97C File Offset: 0x001EBD7C
		internal void SetKeyValues(DataKey key, object[] keyValues, int record)
		{
			for (int i = 0; i < keyValues.Length; i++)
			{
				key.ColumnsReference[i][record] = keyValues[i];
			}
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x001EC9AC File Offset: 0x001EBDAC
		internal DataRow FindByIndex(Index ndx, object[] key)
		{
			Range range = ndx.FindRecords(key);
			if (range.IsNull)
			{
				return null;
			}
			return this.recordManager[ndx.GetRecord(range.Min)];
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x001EC9E4 File Offset: 0x001EBDE4
		internal DataRow FindMergeTarget(DataRow row, DataKey key, Index ndx)
		{
			DataRow dataRow = null;
			if (key.HasValue)
			{
				int num = ((row.oldRecord == -1) ? row.newRecord : row.oldRecord);
				object[] keyValues = key.GetKeyValues(num);
				dataRow = this.FindByIndex(ndx, keyValues);
			}
			return dataRow;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x001ECA28 File Offset: 0x001EBE28
		private void SetMergeRecords(DataRow row, int newRecord, int oldRecord, DataRowAction action)
		{
			if (newRecord != -1)
			{
				this.SetNewRecord(row, newRecord, action, true, true);
				this.SetOldRecord(row, oldRecord);
				return;
			}
			this.SetOldRecord(row, oldRecord);
			if (row.newRecord != -1)
			{
				this.SetNewRecord(row, newRecord, action, true, true);
			}
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x001ECA6C File Offset: 0x001EBE6C
		internal DataRow MergeRow(DataRow row, DataRow targetRow, bool preserveChanges, Index idxSearch)
		{
			if (targetRow == null)
			{
				targetRow = this.NewEmptyRow();
				targetRow.oldRecord = this.recordManager.ImportRecord(row.Table, row.oldRecord);
				targetRow.newRecord = targetRow.oldRecord;
				if (row.oldRecord != row.newRecord)
				{
					targetRow.newRecord = this.recordManager.ImportRecord(row.Table, row.newRecord);
				}
				this.InsertRow(targetRow, -1L);
			}
			else
			{
				int tempRecord = targetRow.tempRecord;
				targetRow.tempRecord = -1;
				try
				{
					DataRowState rowState = targetRow.RowState;
					int num = ((rowState == DataRowState.Added) ? targetRow.newRecord : targetRow.oldRecord);
					if (targetRow.RowState == DataRowState.Unchanged && row.RowState == DataRowState.Unchanged)
					{
						int num2 = targetRow.oldRecord;
						int num3 = (preserveChanges ? this.recordManager.CopyRecord(this, num2, -1) : targetRow.newRecord);
						num2 = this.recordManager.CopyRecord(row.Table, row.oldRecord, targetRow.oldRecord);
						this.SetMergeRecords(targetRow, num3, num2, DataRowAction.Change);
					}
					else if (row.newRecord == -1)
					{
						int num2 = targetRow.oldRecord;
						int num3;
						if (preserveChanges)
						{
							num3 = ((targetRow.RowState == DataRowState.Unchanged) ? this.recordManager.CopyRecord(this, num2, -1) : targetRow.newRecord);
						}
						else
						{
							num3 = -1;
						}
						num2 = this.recordManager.CopyRecord(row.Table, row.oldRecord, num2);
						if (num != ((rowState == DataRowState.Added) ? num3 : num2))
						{
							this.SetMergeRecords(targetRow, num3, num2, (num3 == -1) ? DataRowAction.Delete : DataRowAction.Change);
							idxSearch.Reset();
							int num4 = ((rowState == DataRowState.Added) ? num3 : num2);
						}
						else
						{
							this.SetMergeRecords(targetRow, num3, num2, (num3 == -1) ? DataRowAction.Delete : DataRowAction.Change);
						}
					}
					else
					{
						int num2 = targetRow.oldRecord;
						int num3 = targetRow.newRecord;
						if (targetRow.RowState == DataRowState.Unchanged)
						{
							num3 = this.recordManager.CopyRecord(this, num2, -1);
						}
						num2 = this.recordManager.CopyRecord(row.Table, row.oldRecord, num2);
						if (!preserveChanges)
						{
							num3 = this.recordManager.CopyRecord(row.Table, row.newRecord, num3);
						}
						this.SetMergeRecords(targetRow, num3, num2, DataRowAction.Change);
					}
					if (rowState == DataRowState.Added && targetRow.oldRecord != -1)
					{
						idxSearch.Reset();
					}
				}
				finally
				{
					targetRow.tempRecord = tempRecord;
				}
			}
			if (row.HasErrors)
			{
				if (targetRow.RowError.Length == 0)
				{
					targetRow.RowError = row.RowError;
				}
				else
				{
					DataRow dataRow = targetRow;
					dataRow.RowError = dataRow.RowError + " ]:[ " + row.RowError;
				}
				DataColumn[] columnsInError = row.GetColumnsInError();
				for (int i = 0; i < columnsInError.Length; i++)
				{
					DataColumn dataColumn = targetRow.Table.Columns[columnsInError[i].ColumnName];
					targetRow.SetColumnError(dataColumn, row.GetColumnError(columnsInError[i]));
				}
			}
			else if (!preserveChanges)
			{
				targetRow.ClearErrors();
			}
			return targetRow;
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x001ECD40 File Offset: 0x001EC140
		public void AcceptChanges()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.AcceptChanges|API> %d#\n", this.ObjectID);
			try
			{
				DataRow[] array = new DataRow[this.Rows.Count];
				this.Rows.CopyTo(array, 0);
				this.SuspendIndexEvents();
				try
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].rowID != -1L)
						{
							array[i].AcceptChanges();
						}
					}
				}
				finally
				{
					this.RestoreIndexEvents(false);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x001ECDF0 File Offset: 0x001EC1F0
		protected virtual DataTable CreateInstance()
		{
			return (DataTable)Activator.CreateInstance(base.GetType(), true);
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x001ECE10 File Offset: 0x001EC210
		public virtual DataTable Clone()
		{
			return this.Clone(null);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x001ECE24 File Offset: 0x001EC224
		internal DataTable Clone(DataSet cloneDS)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.Clone|INFO> %d#, cloneDS=%d\n", this.ObjectID, (cloneDS != null) ? cloneDS.ObjectID : 0);
			DataTable dataTable2;
			try
			{
				DataTable dataTable = this.CreateInstance();
				if (dataTable.Columns.Count > 0)
				{
					dataTable.Reset();
				}
				dataTable2 = this.CloneTo(dataTable, cloneDS, false);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataTable2;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x001ECE9C File Offset: 0x001EC29C
		private DataTable IncrementalCloneTo(DataTable sourceTable, DataTable targetTable)
		{
			foreach (object obj in sourceTable.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				if (targetTable.Columns[dataColumn.ColumnName] == null)
				{
					targetTable.Columns.Add(dataColumn.Clone());
				}
			}
			return targetTable;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x001ECF20 File Offset: 0x001EC320
		private DataTable CloneHierarchy(DataTable sourceTable, DataSet ds, Hashtable visitedMap)
		{
			if (visitedMap == null)
			{
				visitedMap = new Hashtable();
			}
			if (visitedMap.Contains(sourceTable))
			{
				return (DataTable)visitedMap[sourceTable];
			}
			DataTable dataTable = ds.Tables[sourceTable.TableName, sourceTable.Namespace];
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				dataTable = this.IncrementalCloneTo(sourceTable, dataTable);
			}
			else
			{
				if (dataTable == null)
				{
					dataTable = new DataTable();
					ds.Tables.Add(dataTable);
				}
				dataTable = sourceTable.CloneTo(dataTable, ds, true);
			}
			visitedMap[sourceTable] = dataTable;
			foreach (object obj in sourceTable.ChildRelations)
			{
				DataRelation dataRelation = (DataRelation)obj;
				this.CloneHierarchy(dataRelation.ChildTable, ds, visitedMap);
			}
			return dataTable;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x001ED00C File Offset: 0x001EC40C
		private DataTable CloneTo(DataTable clone, DataSet cloneDS, bool skipExpressionColumns)
		{
			clone.tableName = this.tableName;
			clone.tableNamespace = this.tableNamespace;
			clone.tablePrefix = this.tablePrefix;
			clone.fNestedInDataset = this.fNestedInDataset;
			clone._culture = this._culture;
			clone._cultureUserSet = this._cultureUserSet;
			clone._compareInfo = this._compareInfo;
			clone._compareFlags = this._compareFlags;
			clone._formatProvider = this._formatProvider;
			clone._hashCodeProvider = this._hashCodeProvider;
			clone._caseSensitive = this._caseSensitive;
			clone._caseSensitiveUserSet = this._caseSensitiveUserSet;
			clone.displayExpression = this.displayExpression;
			clone.typeName = this.typeName;
			clone.repeatableElement = this.repeatableElement;
			clone.MinimumCapacity = this.MinimumCapacity;
			clone.RemotingFormat = this.RemotingFormat;
			DataColumnCollection columns = this.Columns;
			for (int i = 0; i < columns.Count; i++)
			{
				clone.Columns.Add(columns[i].Clone());
			}
			if (!skipExpressionColumns && cloneDS == null)
			{
				for (int j = 0; j < columns.Count; j++)
				{
					clone.Columns[columns[j].ColumnName].Expression = columns[j].Expression;
				}
			}
			DataColumn[] array = this.PrimaryKey;
			if (array.Length > 0)
			{
				DataColumn[] array2 = new DataColumn[array.Length];
				for (int k = 0; k < array.Length; k++)
				{
					array2[k] = clone.Columns[array[k].Ordinal];
				}
				clone.PrimaryKey = array2;
			}
			for (int l = 0; l < this.Constraints.Count; l++)
			{
				ForeignKeyConstraint foreignKeyConstraint = this.Constraints[l] as ForeignKeyConstraint;
				UniqueConstraint uniqueConstraint = this.Constraints[l] as UniqueConstraint;
				if (foreignKeyConstraint != null)
				{
					if (foreignKeyConstraint.Table == foreignKeyConstraint.RelatedTable)
					{
						ForeignKeyConstraint foreignKeyConstraint2 = foreignKeyConstraint.Clone(clone);
						Constraint constraint = clone.Constraints.FindConstraint(foreignKeyConstraint2);
						if (constraint != null)
						{
							constraint.ConstraintName = this.Constraints[l].ConstraintName;
						}
					}
				}
				else if (uniqueConstraint != null)
				{
					UniqueConstraint uniqueConstraint2 = uniqueConstraint.Clone(clone);
					Constraint constraint2 = clone.Constraints.FindConstraint(uniqueConstraint2);
					if (constraint2 != null)
					{
						constraint2.ConstraintName = this.Constraints[l].ConstraintName;
						foreach (object obj in uniqueConstraint2.ExtendedProperties.Keys)
						{
							constraint2.ExtendedProperties[obj] = uniqueConstraint2.ExtendedProperties[obj];
						}
					}
				}
			}
			for (int m = 0; m < this.Constraints.Count; m++)
			{
				if (!clone.Constraints.Contains(this.Constraints[m].ConstraintName, true))
				{
					ForeignKeyConstraint foreignKeyConstraint3 = this.Constraints[m] as ForeignKeyConstraint;
					UniqueConstraint uniqueConstraint3 = this.Constraints[m] as UniqueConstraint;
					if (foreignKeyConstraint3 != null)
					{
						if (foreignKeyConstraint3.Table == foreignKeyConstraint3.RelatedTable)
						{
							ForeignKeyConstraint foreignKeyConstraint4 = foreignKeyConstraint3.Clone(clone);
							if (foreignKeyConstraint4 != null)
							{
								clone.Constraints.Add(foreignKeyConstraint4);
							}
						}
					}
					else if (uniqueConstraint3 != null)
					{
						clone.Constraints.Add(uniqueConstraint3.Clone(clone));
					}
				}
			}
			if (this.extendedProperties != null)
			{
				foreach (object obj2 in this.extendedProperties.Keys)
				{
					clone.ExtendedProperties[obj2] = this.extendedProperties[obj2];
				}
			}
			return clone;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x001ED410 File Offset: 0x001EC810
		public DataTable Copy()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.Copy|API> %d#\n", this.ObjectID);
			DataTable dataTable2;
			try
			{
				DataTable dataTable = this.Clone();
				foreach (object obj in this.Rows)
				{
					DataRow dataRow = (DataRow)obj;
					this.CopyRow(dataTable, dataRow);
				}
				dataTable2 = dataTable;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataTable2;
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600099A RID: 2458 RVA: 0x001ED4B8 File Offset: 0x001EC8B8
		// (remove) Token: 0x0600099B RID: 2459 RVA: 0x001ED4EC File Offset: 0x001EC8EC
		[ResDescription("DataTableColumnChangingDescr")]
		[ResCategory("DataCategory_Data")]
		public event DataColumnChangeEventHandler ColumnChanging
		{
			add
			{
				Bid.Trace("<ds.DataTable.add_ColumnChanging|API> %d#\n", this.ObjectID);
				this.onColumnChangingDelegate = (DataColumnChangeEventHandler)Delegate.Combine(this.onColumnChangingDelegate, value);
			}
			remove
			{
				Bid.Trace("<ds.DataTable.remove_ColumnChanging|API> %d#\n", this.ObjectID);
				this.onColumnChangingDelegate = (DataColumnChangeEventHandler)Delegate.Remove(this.onColumnChangingDelegate, value);
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600099C RID: 2460 RVA: 0x001ED520 File Offset: 0x001EC920
		// (remove) Token: 0x0600099D RID: 2461 RVA: 0x001ED554 File Offset: 0x001EC954
		[ResCategory("DataCategory_Data")]
		[ResDescription("DataTableColumnChangedDescr")]
		public event DataColumnChangeEventHandler ColumnChanged
		{
			add
			{
				Bid.Trace("<ds.DataTable.add_ColumnChanged|API> %d#\n", this.ObjectID);
				this.onColumnChangedDelegate = (DataColumnChangeEventHandler)Delegate.Combine(this.onColumnChangedDelegate, value);
			}
			remove
			{
				Bid.Trace("<ds.DataTable.remove_ColumnChanged|API> %d#\n", this.ObjectID);
				this.onColumnChangedDelegate = (DataColumnChangeEventHandler)Delegate.Remove(this.onColumnChangedDelegate, value);
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600099E RID: 2462 RVA: 0x001ED588 File Offset: 0x001EC988
		// (remove) Token: 0x0600099F RID: 2463 RVA: 0x001ED5AC File Offset: 0x001EC9AC
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

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060009A0 RID: 2464 RVA: 0x001ED5D0 File Offset: 0x001EC9D0
		// (remove) Token: 0x060009A1 RID: 2465 RVA: 0x001ED604 File Offset: 0x001ECA04
		internal event PropertyChangedEventHandler PropertyChanging
		{
			add
			{
				Bid.Trace("<ds.DataTable.add_PropertyChanging|INFO> %d#\n", this.ObjectID);
				this.onPropertyChangingDelegate = (PropertyChangedEventHandler)Delegate.Combine(this.onPropertyChangingDelegate, value);
			}
			remove
			{
				Bid.Trace("<ds.DataTable.remove_PropertyChanging|INFO> %d#\n", this.ObjectID);
				this.onPropertyChangingDelegate = (PropertyChangedEventHandler)Delegate.Remove(this.onPropertyChangingDelegate, value);
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060009A2 RID: 2466 RVA: 0x001ED638 File Offset: 0x001ECA38
		// (remove) Token: 0x060009A3 RID: 2467 RVA: 0x001ED66C File Offset: 0x001ECA6C
		[ResDescription("DataTableRowChangedDescr")]
		[ResCategory("DataCategory_Data")]
		public event DataRowChangeEventHandler RowChanged
		{
			add
			{
				Bid.Trace("<ds.DataTable.add_RowChanged|API> %d#\n", this.ObjectID);
				this.onRowChangedDelegate = (DataRowChangeEventHandler)Delegate.Combine(this.onRowChangedDelegate, value);
			}
			remove
			{
				Bid.Trace("<ds.DataTable.remove_RowChanged|API> %d#\n", this.ObjectID);
				this.onRowChangedDelegate = (DataRowChangeEventHandler)Delegate.Remove(this.onRowChangedDelegate, value);
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060009A4 RID: 2468 RVA: 0x001ED6A0 File Offset: 0x001ECAA0
		// (remove) Token: 0x060009A5 RID: 2469 RVA: 0x001ED6D4 File Offset: 0x001ECAD4
		[ResCategory("DataCategory_Data")]
		[ResDescription("DataTableRowChangingDescr")]
		public event DataRowChangeEventHandler RowChanging
		{
			add
			{
				Bid.Trace("<ds.DataTable.add_RowChanging|API> %d#\n", this.ObjectID);
				this.onRowChangingDelegate = (DataRowChangeEventHandler)Delegate.Combine(this.onRowChangingDelegate, value);
			}
			remove
			{
				Bid.Trace("<ds.DataTable.remove_RowChanging|API> %d#\n", this.ObjectID);
				this.onRowChangingDelegate = (DataRowChangeEventHandler)Delegate.Remove(this.onRowChangingDelegate, value);
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060009A6 RID: 2470 RVA: 0x001ED708 File Offset: 0x001ECB08
		// (remove) Token: 0x060009A7 RID: 2471 RVA: 0x001ED73C File Offset: 0x001ECB3C
		[ResCategory("DataCategory_Data")]
		[ResDescription("DataTableRowDeletingDescr")]
		public event DataRowChangeEventHandler RowDeleting
		{
			add
			{
				Bid.Trace("<ds.DataTable.add_RowDeleting|API> %d#\n", this.ObjectID);
				this.onRowDeletingDelegate = (DataRowChangeEventHandler)Delegate.Combine(this.onRowDeletingDelegate, value);
			}
			remove
			{
				Bid.Trace("<ds.DataTable.remove_RowDeleting|API> %d#\n", this.ObjectID);
				this.onRowDeletingDelegate = (DataRowChangeEventHandler)Delegate.Remove(this.onRowDeletingDelegate, value);
			}
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060009A8 RID: 2472 RVA: 0x001ED770 File Offset: 0x001ECB70
		// (remove) Token: 0x060009A9 RID: 2473 RVA: 0x001ED7A4 File Offset: 0x001ECBA4
		[ResDescription("DataTableRowDeletedDescr")]
		[ResCategory("DataCategory_Data")]
		public event DataRowChangeEventHandler RowDeleted
		{
			add
			{
				Bid.Trace("<ds.DataTable.add_RowDeleted|API> %d#\n", this.ObjectID);
				this.onRowDeletedDelegate = (DataRowChangeEventHandler)Delegate.Combine(this.onRowDeletedDelegate, value);
			}
			remove
			{
				Bid.Trace("<ds.DataTable.remove_RowDeleted|API> %d#\n", this.ObjectID);
				this.onRowDeletedDelegate = (DataRowChangeEventHandler)Delegate.Remove(this.onRowDeletedDelegate, value);
			}
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060009AA RID: 2474 RVA: 0x001ED7D8 File Offset: 0x001ECBD8
		// (remove) Token: 0x060009AB RID: 2475 RVA: 0x001ED80C File Offset: 0x001ECC0C
		[ResCategory("DataCategory_Data")]
		[ResDescription("DataTableRowsClearingDescr")]
		public event DataTableClearEventHandler TableClearing
		{
			add
			{
				Bid.Trace("<ds.DataTable.add_TableClearing|API> %d#\n", this.ObjectID);
				this.onTableClearingDelegate = (DataTableClearEventHandler)Delegate.Combine(this.onTableClearingDelegate, value);
			}
			remove
			{
				Bid.Trace("<ds.DataTable.remove_TableClearing|API> %d#\n", this.ObjectID);
				this.onTableClearingDelegate = (DataTableClearEventHandler)Delegate.Remove(this.onTableClearingDelegate, value);
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060009AC RID: 2476 RVA: 0x001ED840 File Offset: 0x001ECC40
		// (remove) Token: 0x060009AD RID: 2477 RVA: 0x001ED874 File Offset: 0x001ECC74
		[ResDescription("DataTableRowsClearedDescr")]
		[ResCategory("DataCategory_Data")]
		public event DataTableClearEventHandler TableCleared
		{
			add
			{
				Bid.Trace("<ds.DataTable.add_TableCleared|API> %d#\n", this.ObjectID);
				this.onTableClearedDelegate = (DataTableClearEventHandler)Delegate.Combine(this.onTableClearedDelegate, value);
			}
			remove
			{
				Bid.Trace("<ds.DataTable.remove_TableCleared|API> %d#\n", this.ObjectID);
				this.onTableClearedDelegate = (DataTableClearEventHandler)Delegate.Remove(this.onTableClearedDelegate, value);
			}
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060009AE RID: 2478 RVA: 0x001ED8A8 File Offset: 0x001ECCA8
		// (remove) Token: 0x060009AF RID: 2479 RVA: 0x001ED8CC File Offset: 0x001ECCCC
		[ResDescription("DataTableRowsNewRowDescr")]
		[ResCategory("DataCategory_Data")]
		public event DataTableNewRowEventHandler TableNewRow
		{
			add
			{
				this.onTableNewRowDelegate = (DataTableNewRowEventHandler)Delegate.Combine(this.onTableNewRowDelegate, value);
			}
			remove
			{
				this.onTableNewRowDelegate = (DataTableNewRowEventHandler)Delegate.Remove(this.onTableNewRowDelegate, value);
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x001ED8F0 File Offset: 0x001ECCF0
		// (set) Token: 0x060009B1 RID: 2481 RVA: 0x001ED904 File Offset: 0x001ECD04
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
						for (int i = 0; i < this.Columns.Count; i++)
						{
							if (this.Columns[i].Site != null)
							{
								container.Remove(this.Columns[i]);
							}
						}
					}
				}
				base.Site = value;
			}
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x001ED96C File Offset: 0x001ECD6C
		internal DataRow AddRecords(int oldRecord, int newRecord)
		{
			DataRow dataRow;
			if (oldRecord == -1 && newRecord == -1)
			{
				dataRow = this.NewRow(-1);
				this.AddRow(dataRow);
			}
			else
			{
				dataRow = this.NewEmptyRow();
				dataRow.oldRecord = oldRecord;
				dataRow.newRecord = newRecord;
				this.InsertRow(dataRow, -1L);
			}
			return dataRow;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x001ED9B4 File Offset: 0x001ECDB4
		internal void AddRow(DataRow row)
		{
			this.AddRow(row, -1);
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x001ED9CC File Offset: 0x001ECDCC
		internal void AddRow(DataRow row, int proposedID)
		{
			this.InsertRow(row, proposedID, -1);
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x001ED9E4 File Offset: 0x001ECDE4
		internal void InsertRow(DataRow row, int proposedID, int pos)
		{
			this.InsertRow(row, (long)proposedID, pos, true);
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x001ED9FC File Offset: 0x001ECDFC
		internal void InsertRow(DataRow row, long proposedID, int pos, bool fireEvent)
		{
			Exception ex = null;
			if (row == null)
			{
				throw ExceptionBuilder.ArgumentNull("row");
			}
			if (row.Table != this)
			{
				throw ExceptionBuilder.RowAlreadyInOtherCollection();
			}
			if (row.rowID != -1L)
			{
				throw ExceptionBuilder.RowAlreadyInTheCollection();
			}
			row.BeginEdit();
			int tempRecord = row.tempRecord;
			row.tempRecord = -1;
			if (proposedID == -1L)
			{
				proposedID = this.nextRowID;
			}
			bool flag;
			if (flag = this.nextRowID <= proposedID)
			{
				this.nextRowID = checked(proposedID + 1L);
			}
			try
			{
				try
				{
					row.rowID = proposedID;
					this.SetNewRecordWorker(row, tempRecord, DataRowAction.Add, false, pos, fireEvent, out ex);
				}
				catch
				{
					if (flag && this.nextRowID == proposedID + 1L)
					{
						this.nextRowID = proposedID;
					}
					row.rowID = -1L;
					row.tempRecord = tempRecord;
					throw;
				}
				if (ex != null)
				{
					throw ex;
				}
				if (this.EnforceConstraints && !this.inLoad)
				{
					int count = this.columnCollection.Count;
					for (int i = 0; i < count; i++)
					{
						DataColumn dataColumn = this.columnCollection[i];
						if (dataColumn.Computed)
						{
							dataColumn.CheckColumnConstraint(row, DataRowAction.Add);
						}
					}
				}
			}
			finally
			{
				row.ResetLastChangedColumn();
			}
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x001EDB44 File Offset: 0x001ECF44
		internal void CheckNotModifying(DataRow row)
		{
			if (row.tempRecord != -1)
			{
				row.EndEdit();
			}
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x001EDB60 File Offset: 0x001ECF60
		public void Clear()
		{
			this.Clear(true);
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x001EDB74 File Offset: 0x001ECF74
		internal void Clear(bool clearAll)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.Clear|INFO> %d#, clearAll=%d{bool}\n", this.ObjectID, clearAll);
			try
			{
				this.rowDiffId = null;
				if (this.dataSet != null)
				{
					this.dataSet.OnClearFunctionCalled(this);
				}
				bool flag = this.Rows.Count != 0;
				DataTableClearEventArgs dataTableClearEventArgs = null;
				if (flag)
				{
					dataTableClearEventArgs = new DataTableClearEventArgs(this);
					this.OnTableClearing(dataTableClearEventArgs);
				}
				if (this.dataSet != null && this.dataSet.EnforceConstraints)
				{
					ParentForeignKeyConstraintEnumerator parentForeignKeyConstraintEnumerator = new ParentForeignKeyConstraintEnumerator(this.dataSet, this);
					while (parentForeignKeyConstraintEnumerator.GetNext())
					{
						ForeignKeyConstraint foreignKeyConstraint = parentForeignKeyConstraintEnumerator.GetForeignKeyConstraint();
						foreignKeyConstraint.CheckCanClearParentTable(this);
					}
				}
				this.recordManager.Clear(clearAll);
				foreach (object obj in this.Rows)
				{
					DataRow dataRow = (DataRow)obj;
					dataRow.oldRecord = -1;
					dataRow.newRecord = -1;
					dataRow.tempRecord = -1;
					dataRow.rowID = -1L;
					dataRow.RBTreeNodeId = 0;
				}
				this.Rows.ArrayClear();
				this.ResetIndexes();
				if (flag)
				{
					this.OnTableCleared(dataTableClearEventArgs);
				}
				foreach (object obj2 in this.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj2;
					this.EvaluateDependentExpressions(dataColumn);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x001EDD30 File Offset: 0x001ED130
		internal void CascadeAll(DataRow row, DataRowAction action)
		{
			if (this.DataSet != null && this.DataSet.fEnableCascading)
			{
				ParentForeignKeyConstraintEnumerator parentForeignKeyConstraintEnumerator = new ParentForeignKeyConstraintEnumerator(this.dataSet, this);
				while (parentForeignKeyConstraintEnumerator.GetNext())
				{
					parentForeignKeyConstraintEnumerator.GetForeignKeyConstraint().CheckCascade(row, action);
				}
			}
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x001EDD78 File Offset: 0x001ED178
		internal void CommitRow(DataRow row)
		{
			DataRowChangeEventArgs dataRowChangeEventArgs = this.OnRowChanging(null, row, DataRowAction.Commit);
			if (!this.inDataLoad)
			{
				this.CascadeAll(row, DataRowAction.Commit);
			}
			this.SetOldRecord(row, row.newRecord);
			this.OnRowChanged(dataRowChangeEventArgs, row, DataRowAction.Commit);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x001EDDB8 File Offset: 0x001ED1B8
		internal int Compare(string s1, string s2)
		{
			return this.Compare(s1, s2, null);
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x001EDDD0 File Offset: 0x001ED1D0
		internal int Compare(string s1, string s2, CompareInfo comparer)
		{
			if (s1 == s2)
			{
				return 0;
			}
			if (s1 == null)
			{
				return -1;
			}
			if (s2 == null)
			{
				return 1;
			}
			int i = s1.Length;
			int num = s2.Length;
			while (i > 0)
			{
				if (s1[i - 1] != ' ' && s1[i - 1] != '\u3000')
				{
					IL_006C:
					while (num > 0 && (s2[num - 1] == ' ' || s2[num - 1] == '\u3000'))
					{
						num--;
					}
					return (comparer ?? this.CompareInfo).Compare(s1, 0, i, s2, 0, num, this._compareFlags);
				}
				i--;
			}
			goto IL_006C;
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x001EDE6C File Offset: 0x001ED26C
		internal int IndexOf(string s1, string s2)
		{
			return this.CompareInfo.IndexOf(s1, s2, this._compareFlags);
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x001EDE8C File Offset: 0x001ED28C
		internal bool IsSuffix(string s1, string s2)
		{
			return this.CompareInfo.IsSuffix(s1, s2, this._compareFlags);
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x001EDEAC File Offset: 0x001ED2AC
		public object Compute(string expression, string filter)
		{
			DataRow[] array = this.Select(filter, "", DataViewRowState.CurrentRows);
			DataExpression dataExpression = new DataExpression(this, expression);
			return dataExpression.Evaluate(array);
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x001EDED8 File Offset: 0x001ED2D8
		bool IListSource.ContainsListCollection
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x001EDEE8 File Offset: 0x001ED2E8
		internal void CopyRow(DataTable table, DataRow row)
		{
			int num = -1;
			int num2 = -1;
			if (row == null)
			{
				return;
			}
			if (row.oldRecord != -1)
			{
				num = table.recordManager.ImportRecord(row.Table, row.oldRecord);
			}
			if (row.newRecord != -1)
			{
				if (row.newRecord != row.oldRecord)
				{
					num2 = table.recordManager.ImportRecord(row.Table, row.newRecord);
				}
				else
				{
					num2 = num;
				}
			}
			DataRow dataRow = table.AddRecords(num, num2);
			if (row.HasErrors)
			{
				dataRow.RowError = row.RowError;
				DataColumn[] columnsInError = row.GetColumnsInError();
				for (int i = 0; i < columnsInError.Length; i++)
				{
					DataColumn dataColumn = dataRow.Table.Columns[columnsInError[i].ColumnName];
					dataRow.SetColumnError(dataColumn, row.GetColumnError(columnsInError[i]));
				}
			}
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x001EDFB4 File Offset: 0x001ED3B4
		internal void DeleteRow(DataRow row)
		{
			if (row.newRecord == -1)
			{
				throw ExceptionBuilder.RowAlreadyDeleted();
			}
			this.SetNewRecord(row, -1, DataRowAction.Delete, false, true);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x001EDFDC File Offset: 0x001ED3DC
		private void CheckPrimaryKey()
		{
			if (this.primaryKey == null)
			{
				throw ExceptionBuilder.TableMissingPrimaryKey();
			}
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x001EDFF8 File Offset: 0x001ED3F8
		internal DataRow FindByPrimaryKey(object[] values)
		{
			this.CheckPrimaryKey();
			return this.FindRow(this.primaryKey.Key, values);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x001EE020 File Offset: 0x001ED420
		internal DataRow FindByPrimaryKey(object value)
		{
			this.CheckPrimaryKey();
			return this.FindRow(this.primaryKey.Key, value);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x001EE048 File Offset: 0x001ED448
		private DataRow FindRow(DataKey key, object[] values)
		{
			Index index = this.GetIndex(this.NewIndexDesc(key));
			Range range = index.FindRecords(values);
			if (range.IsNull)
			{
				return null;
			}
			return this.recordManager[index.GetRecord(range.Min)];
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x001EE090 File Offset: 0x001ED490
		private DataRow FindRow(DataKey key, object value)
		{
			Index index = this.GetIndex(this.NewIndexDesc(key));
			Range range = index.FindRecords(value);
			if (range.IsNull)
			{
				return null;
			}
			return this.recordManager[index.GetRecord(range.Min)];
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x001EE0D8 File Offset: 0x001ED4D8
		internal string FormatSortString(IndexField[] indexDesc)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IndexField indexField in indexDesc)
			{
				if (0 < stringBuilder.Length)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(indexField.Column.ColumnName);
				if (indexField.IsDescending)
				{
					stringBuilder.Append(" DESC");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x001EE14C File Offset: 0x001ED54C
		internal void FreeRecord(ref int record)
		{
			this.recordManager.FreeRecord(ref record);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x001EE168 File Offset: 0x001ED568
		public DataTable GetChanges()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.GetChanges|API> %d#\n", this.ObjectID);
			DataTable dataTable2;
			try
			{
				DataTable dataTable = this.Clone();
				for (int i = 0; i < this.Rows.Count; i++)
				{
					DataRow dataRow = this.Rows[i];
					if (dataRow.oldRecord != dataRow.newRecord)
					{
						dataTable.ImportRow(dataRow);
					}
				}
				if (dataTable.Rows.Count == 0)
				{
					dataTable2 = null;
				}
				else
				{
					dataTable2 = dataTable;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataTable2;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x001EE204 File Offset: 0x001ED604
		public DataTable GetChanges(DataRowState rowStates)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.GetChanges|API> %d#, rowStates=%d{ds.DataRowState}\n", this.ObjectID, (int)rowStates);
			DataTable dataTable2;
			try
			{
				DataTable dataTable = this.Clone();
				for (int i = 0; i < this.Rows.Count; i++)
				{
					DataRow dataRow = this.Rows[i];
					if ((dataRow.RowState & rowStates) != (DataRowState)0)
					{
						dataTable.ImportRow(dataRow);
					}
				}
				if (dataTable.Rows.Count == 0)
				{
					dataTable2 = null;
				}
				else
				{
					dataTable2 = dataTable;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataTable2;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x001EE29C File Offset: 0x001ED69C
		public DataRow[] GetErrors()
		{
			List<DataRow> list = new List<DataRow>();
			for (int i = 0; i < this.Rows.Count; i++)
			{
				DataRow dataRow = this.Rows[i];
				if (dataRow.HasErrors)
				{
					list.Add(dataRow);
				}
			}
			DataRow[] array = this.NewRowArray(list.Count);
			list.CopyTo(array);
			return array;
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x001EE2F8 File Offset: 0x001ED6F8
		internal Index GetIndex(IndexField[] indexDesc)
		{
			return this.GetIndex(indexDesc, DataViewRowState.CurrentRows, null);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x001EE310 File Offset: 0x001ED710
		internal Index GetIndex(string sort, DataViewRowState recordStates, IFilter rowFilter)
		{
			return this.GetIndex(this.ParseSortString(sort), recordStates, rowFilter);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x001EE32C File Offset: 0x001ED72C
		internal Index GetIndex(IndexField[] indexDesc, DataViewRowState recordStates, IFilter rowFilter)
		{
			this.indexesLock.AcquireReaderLock(-1);
			try
			{
				for (int i = 0; i < this.indexes.Count; i++)
				{
					Index index = this.indexes[i];
					if (index != null && index.Equal(indexDesc, recordStates, rowFilter))
					{
						return index;
					}
				}
			}
			finally
			{
				this.indexesLock.ReleaseReaderLock();
			}
			Index index2 = new Index(this, indexDesc, recordStates, rowFilter);
			index2.AddRef();
			return index2;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x001EE3B8 File Offset: 0x001ED7B8
		IList IListSource.GetList()
		{
			return this.DefaultView;
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x001EE3CC File Offset: 0x001ED7CC
		internal List<DataViewListener> GetListeners()
		{
			return this._dataViewListeners;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x001EE3E0 File Offset: 0x001ED7E0
		internal int GetSpecialHashCode(string name)
		{
			int num = 0;
			while (num < name.Length && '\u3000' > name[num])
			{
				num++;
			}
			if (name.Length == num)
			{
				if (this._hashCodeProvider == null)
				{
					this._hashCodeProvider = StringComparer.Create(this.Locale, true);
				}
				return this._hashCodeProvider.GetHashCode(name);
			}
			return 0;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x001EE440 File Offset: 0x001ED840
		public void ImportRow(DataRow row)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.ImportRow|API> %d#\n", this.ObjectID);
			try
			{
				int num = -1;
				int num2 = -1;
				if (row != null)
				{
					if (row.oldRecord != -1)
					{
						num = this.recordManager.ImportRecord(row.Table, row.oldRecord);
					}
					if (row.newRecord != -1)
					{
						if (row.RowState != DataRowState.Unchanged)
						{
							num2 = this.recordManager.ImportRecord(row.Table, row.newRecord);
						}
						else
						{
							num2 = num;
						}
					}
					if (num != -1 || num2 != -1)
					{
						DataRow dataRow = this.AddRecords(num, num2);
						if (row.HasErrors)
						{
							dataRow.RowError = row.RowError;
							DataColumn[] columnsInError = row.GetColumnsInError();
							for (int i = 0; i < columnsInError.Length; i++)
							{
								DataColumn dataColumn = dataRow.Table.Columns[columnsInError[i].ColumnName];
								dataRow.SetColumnError(dataColumn, row.GetColumnError(columnsInError[i]));
							}
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x001EE54C File Offset: 0x001ED94C
		internal void InsertRow(DataRow row, long proposedID)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.InsertRow|INFO> %d#, row=%d\n", this.ObjectID, row.ObjectID);
			try
			{
				if (row.Table != this)
				{
					throw ExceptionBuilder.RowAlreadyInOtherCollection();
				}
				if (row.rowID != -1L)
				{
					throw ExceptionBuilder.RowAlreadyInTheCollection();
				}
				if (row.oldRecord == -1 && row.newRecord == -1)
				{
					throw ExceptionBuilder.RowEmpty();
				}
				if (proposedID == -1L)
				{
					proposedID = this.nextRowID;
				}
				row.rowID = proposedID;
				if (this.nextRowID <= proposedID)
				{
					this.nextRowID = checked(proposedID + 1L);
				}
				DataRowChangeEventArgs dataRowChangeEventArgs = null;
				if (row.newRecord != -1)
				{
					row.tempRecord = row.newRecord;
					row.newRecord = -1;
					try
					{
						dataRowChangeEventArgs = this.RaiseRowChanging(null, row, DataRowAction.Add, true);
					}
					catch
					{
						row.tempRecord = -1;
						throw;
					}
					row.newRecord = row.tempRecord;
					row.tempRecord = -1;
				}
				if (row.oldRecord != -1)
				{
					this.recordManager[row.oldRecord] = row;
				}
				if (row.newRecord != -1)
				{
					this.recordManager[row.newRecord] = row;
				}
				this.Rows.ArrayAdd(row);
				if (row.RowState == DataRowState.Unchanged)
				{
					this.RecordStateChanged(row.oldRecord, DataViewRowState.None, DataViewRowState.Unchanged);
				}
				else
				{
					this.RecordStateChanged(row.oldRecord, DataViewRowState.None, row.GetRecordState(row.oldRecord), row.newRecord, DataViewRowState.None, row.GetRecordState(row.newRecord));
				}
				if (this.dependentColumns != null && this.dependentColumns.Count > 0)
				{
					this.EvaluateExpressions(row, DataRowAction.Add, null);
				}
				this.RaiseRowChanged(dataRowChangeEventArgs, row, DataRowAction.Add);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x001EE70C File Offset: 0x001EDB0C
		private IndexField[] NewIndexDesc(DataKey key)
		{
			IndexField[] indexDesc = key.GetIndexDesc();
			IndexField[] array = new IndexField[indexDesc.Length];
			Array.Copy(indexDesc, 0, array, 0, indexDesc.Length);
			return array;
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x001EE738 File Offset: 0x001EDB38
		internal int NewRecord()
		{
			return this.NewRecord(-1);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x001EE74C File Offset: 0x001EDB4C
		internal int NewUninitializedRecord()
		{
			return this.recordManager.NewRecordBase();
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x001EE764 File Offset: 0x001EDB64
		internal int NewRecordFromArray(object[] value)
		{
			int count = this.columnCollection.Count;
			if (count < value.Length)
			{
				throw ExceptionBuilder.ValueArrayLength();
			}
			int num = this.recordManager.NewRecordBase();
			int num2;
			try
			{
				for (int i = 0; i < value.Length; i++)
				{
					if (value[i] != null)
					{
						this.columnCollection[i][num] = value[i];
					}
					else
					{
						this.columnCollection[i].Init(num);
					}
				}
				for (int j = value.Length; j < count; j++)
				{
					this.columnCollection[j].Init(num);
				}
				num2 = num;
			}
			catch (Exception ex)
			{
				if (ADP.IsCatchableOrSecurityExceptionType(ex))
				{
					this.FreeRecord(ref num);
				}
				throw;
			}
			return num2;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x001EE82C File Offset: 0x001EDC2C
		internal int NewRecord(int sourceRecord)
		{
			int num = this.recordManager.NewRecordBase();
			int count = this.columnCollection.Count;
			if (-1 == sourceRecord)
			{
				for (int i = 0; i < count; i++)
				{
					this.columnCollection[i].Init(num);
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					this.columnCollection[j].Copy(sourceRecord, num);
				}
			}
			return num;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x001EE898 File Offset: 0x001EDC98
		internal DataRow NewEmptyRow()
		{
			this.rowBuilder._record = -1;
			DataRow dataRow = this.NewRowFromBuilder(this.rowBuilder);
			if (this.dataSet != null)
			{
				this.DataSet.OnDataRowCreated(dataRow);
			}
			return dataRow;
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x001EE8D4 File Offset: 0x001EDCD4
		private DataRow NewUninitializedRow()
		{
			return this.NewRow(this.NewUninitializedRecord());
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x001EE8F0 File Offset: 0x001EDCF0
		public DataRow NewRow()
		{
			DataRow dataRow = this.NewRow(-1);
			this.NewRowCreated(dataRow);
			return dataRow;
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x001EE910 File Offset: 0x001EDD10
		internal DataRow CreateEmptyRow()
		{
			DataRow dataRow = this.NewUninitializedRow();
			foreach (object obj in this.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				if (!XmlToDatasetMap.IsMappedColumn(dataColumn))
				{
					if (!dataColumn.AutoIncrement)
					{
						if (dataColumn.AllowDBNull)
						{
							dataRow[dataColumn] = DBNull.Value;
						}
						else if (dataColumn.DefaultValue != null)
						{
							dataRow[dataColumn] = dataColumn.DefaultValue;
						}
					}
					else
					{
						dataColumn.Init(dataRow.tempRecord);
					}
				}
			}
			return dataRow;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x001EE9C0 File Offset: 0x001EDDC0
		private void NewRowCreated(DataRow row)
		{
			if (this.onTableNewRowDelegate != null)
			{
				DataTableNewRowEventArgs dataTableNewRowEventArgs = new DataTableNewRowEventArgs(row);
				this.OnTableNewRow(dataTableNewRowEventArgs);
			}
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x001EE9E4 File Offset: 0x001EDDE4
		internal DataRow NewRow(int record)
		{
			if (-1 == record)
			{
				record = this.NewRecord(-1);
			}
			this.rowBuilder._record = record;
			DataRow dataRow = this.NewRowFromBuilder(this.rowBuilder);
			this.recordManager[record] = dataRow;
			if (this.dataSet != null)
			{
				this.DataSet.OnDataRowCreated(dataRow);
			}
			return dataRow;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x001EEA3C File Offset: 0x001EDE3C
		protected virtual DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new DataRow(builder);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x001EEA50 File Offset: 0x001EDE50
		protected virtual Type GetRowType()
		{
			return typeof(DataRow);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x001EEA68 File Offset: 0x001EDE68
		protected internal DataRow[] NewRowArray(int size)
		{
			if (this.IsTypedDataTable)
			{
				if (size == 0)
				{
					if (this.EmptyDataRowArray == null)
					{
						this.EmptyDataRowArray = (DataRow[])Array.CreateInstance(this.GetRowType(), 0);
					}
					return this.EmptyDataRowArray;
				}
				return (DataRow[])Array.CreateInstance(this.GetRowType(), size);
			}
			else
			{
				if (size != 0)
				{
					return new DataRow[size];
				}
				return DataTable.zeroRows;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x001EEAC8 File Offset: 0x001EDEC8
		internal bool NeedColumnChangeEvents
		{
			get
			{
				return this.IsTypedDataTable || this.onColumnChangingDelegate != null || null != this.onColumnChangedDelegate;
			}
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x001EEAF4 File Offset: 0x001EDEF4
		protected internal virtual void OnColumnChanging(DataColumnChangeEventArgs e)
		{
			if (this.onColumnChangingDelegate != null)
			{
				Bid.Trace("<ds.DataTable.OnColumnChanging|INFO> %d#\n", this.ObjectID);
				this.onColumnChangingDelegate(this, e);
			}
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x001EEB28 File Offset: 0x001EDF28
		protected internal virtual void OnColumnChanged(DataColumnChangeEventArgs e)
		{
			if (this.onColumnChangedDelegate != null)
			{
				Bid.Trace("<ds.DataTable.OnColumnChanged|INFO> %d#\n", this.ObjectID);
				this.onColumnChangedDelegate(this, e);
			}
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x001EEB5C File Offset: 0x001EDF5C
		protected virtual void OnPropertyChanging(PropertyChangedEventArgs pcevent)
		{
			if (this.onPropertyChangingDelegate != null)
			{
				Bid.Trace("<ds.DataTable.OnPropertyChanging|INFO> %d#\n", this.ObjectID);
				this.onPropertyChangingDelegate(this, pcevent);
			}
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x001EEB90 File Offset: 0x001EDF90
		internal void OnRemoveColumnInternal(DataColumn column)
		{
			this.OnRemoveColumn(column);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x001EEBA4 File Offset: 0x001EDFA4
		protected virtual void OnRemoveColumn(DataColumn column)
		{
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x001EEBB4 File Offset: 0x001EDFB4
		private DataRowChangeEventArgs OnRowChanged(DataRowChangeEventArgs args, DataRow eRow, DataRowAction eAction)
		{
			if (this.onRowChangedDelegate != null || this.IsTypedDataTable)
			{
				if (args == null)
				{
					args = new DataRowChangeEventArgs(eRow, eAction);
				}
				this.OnRowChanged(args);
			}
			return args;
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x001EEBE8 File Offset: 0x001EDFE8
		private DataRowChangeEventArgs OnRowChanging(DataRowChangeEventArgs args, DataRow eRow, DataRowAction eAction)
		{
			if (this.onRowChangingDelegate != null || this.IsTypedDataTable)
			{
				if (args == null)
				{
					args = new DataRowChangeEventArgs(eRow, eAction);
				}
				this.OnRowChanging(args);
			}
			return args;
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x001EEC1C File Offset: 0x001EE01C
		protected virtual void OnRowChanged(DataRowChangeEventArgs e)
		{
			if (this.onRowChangedDelegate != null)
			{
				Bid.Trace("<ds.DataTable.OnRowChanged|INFO> %d#\n", this.ObjectID);
				this.onRowChangedDelegate(this, e);
			}
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x001EEC50 File Offset: 0x001EE050
		protected virtual void OnRowChanging(DataRowChangeEventArgs e)
		{
			if (this.onRowChangingDelegate != null)
			{
				Bid.Trace("<ds.DataTable.OnRowChanging|INFO> %d#\n", this.ObjectID);
				this.onRowChangingDelegate(this, e);
			}
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x001EEC84 File Offset: 0x001EE084
		protected virtual void OnRowDeleting(DataRowChangeEventArgs e)
		{
			if (this.onRowDeletingDelegate != null)
			{
				Bid.Trace("<ds.DataTable.OnRowDeleting|INFO> %d#\n", this.ObjectID);
				this.onRowDeletingDelegate(this, e);
			}
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x001EECB8 File Offset: 0x001EE0B8
		protected virtual void OnRowDeleted(DataRowChangeEventArgs e)
		{
			if (this.onRowDeletedDelegate != null)
			{
				Bid.Trace("<ds.DataTable.OnRowDeleted|INFO> %d#\n", this.ObjectID);
				this.onRowDeletedDelegate(this, e);
			}
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x001EECEC File Offset: 0x001EE0EC
		protected virtual void OnTableCleared(DataTableClearEventArgs e)
		{
			if (this.onTableClearedDelegate != null)
			{
				Bid.Trace("<ds.DataTable.OnTableCleared|INFO> %d#\n", this.ObjectID);
				this.onTableClearedDelegate(this, e);
			}
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x001EED20 File Offset: 0x001EE120
		protected virtual void OnTableClearing(DataTableClearEventArgs e)
		{
			if (this.onTableClearingDelegate != null)
			{
				Bid.Trace("<ds.DataTable.OnTableClearing|INFO> %d#\n", this.ObjectID);
				this.onTableClearingDelegate(this, e);
			}
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x001EED54 File Offset: 0x001EE154
		protected virtual void OnTableNewRow(DataTableNewRowEventArgs e)
		{
			if (this.onTableNewRowDelegate != null)
			{
				Bid.Trace("<ds.DataTable.OnTableNewRow|INFO> %d#\n", this.ObjectID);
				this.onTableNewRowDelegate(this, e);
			}
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x001EED88 File Offset: 0x001EE188
		private void OnInitialized()
		{
			if (this.onInitialized != null)
			{
				Bid.Trace("<ds.DataTable.OnInitialized|INFO> %d#\n", this.ObjectID);
				this.onInitialized(this, EventArgs.Empty);
			}
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x001EEDC0 File Offset: 0x001EE1C0
		internal IndexField[] ParseSortString(string sortString)
		{
			IndexField[] array = DataTable.zeroIndexField;
			if (sortString != null && 0 < sortString.Length)
			{
				string[] array2 = sortString.Split(new char[] { ',' });
				array = new IndexField[array2.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i].Trim();
					int length = text.Length;
					bool flag = false;
					if (length >= 5 && string.Compare(text, length - 4, " ASC", 0, 4, StringComparison.OrdinalIgnoreCase) == 0)
					{
						text = text.Substring(0, length - 4).Trim();
					}
					else if (length >= 6 && string.Compare(text, length - 5, " DESC", 0, 5, StringComparison.OrdinalIgnoreCase) == 0)
					{
						flag = true;
						text = text.Substring(0, length - 5).Trim();
					}
					if (text.StartsWith("[", StringComparison.Ordinal))
					{
						if (!text.EndsWith("]", StringComparison.Ordinal))
						{
							throw ExceptionBuilder.InvalidSortString(array2[i]);
						}
						text = text.Substring(1, text.Length - 2);
					}
					DataColumn dataColumn = this.Columns[text];
					if (dataColumn == null)
					{
						throw ExceptionBuilder.ColumnOutOfRange(text);
					}
					array[i] = new IndexField(dataColumn, flag);
				}
			}
			return array;
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x001EEEEC File Offset: 0x001EE2EC
		internal void RaisePropertyChanging(string name)
		{
			this.OnPropertyChanging(new PropertyChangedEventArgs(name));
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x001EEF08 File Offset: 0x001EE308
		internal void RecordChanged(int record)
		{
			this.SetShadowIndexes();
			try
			{
				int count = this.shadowIndexes.Count;
				for (int i = 0; i < count; i++)
				{
					Index index = this.shadowIndexes[i];
					if (0 < index.RefCount)
					{
						index.RecordChanged(record);
					}
				}
			}
			finally
			{
				this.RestoreShadowIndexes();
			}
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x001EEF78 File Offset: 0x001EE378
		internal void RecordChanged(int[] oldIndex, int[] newIndex)
		{
			this.SetShadowIndexes();
			try
			{
				int count = this.shadowIndexes.Count;
				for (int i = 0; i < count; i++)
				{
					Index index = this.shadowIndexes[i];
					if (0 < index.RefCount)
					{
						index.RecordChanged(oldIndex[i], newIndex[i]);
					}
				}
			}
			finally
			{
				this.RestoreShadowIndexes();
			}
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x001EEFEC File Offset: 0x001EE3EC
		internal void RecordStateChanged(int record, DataViewRowState oldState, DataViewRowState newState)
		{
			this.SetShadowIndexes();
			try
			{
				int count = this.shadowIndexes.Count;
				for (int i = 0; i < count; i++)
				{
					Index index = this.shadowIndexes[i];
					if (0 < index.RefCount)
					{
						index.RecordStateChanged(record, oldState, newState);
					}
				}
			}
			finally
			{
				this.RestoreShadowIndexes();
			}
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x001EF05C File Offset: 0x001EE45C
		internal void RecordStateChanged(int record1, DataViewRowState oldState1, DataViewRowState newState1, int record2, DataViewRowState oldState2, DataViewRowState newState2)
		{
			this.SetShadowIndexes();
			try
			{
				int count = this.shadowIndexes.Count;
				for (int i = 0; i < count; i++)
				{
					Index index = this.shadowIndexes[i];
					if (0 < index.RefCount)
					{
						if (record1 != -1 && record2 != -1)
						{
							index.RecordStateChanged(record1, oldState1, newState1, record2, oldState2, newState2);
						}
						else if (record1 != -1)
						{
							index.RecordStateChanged(record1, oldState1, newState1);
						}
						else if (record2 != -1)
						{
							index.RecordStateChanged(record2, oldState2, newState2);
						}
					}
				}
			}
			finally
			{
				this.RestoreShadowIndexes();
			}
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x001EF0FC File Offset: 0x001EE4FC
		internal int[] RemoveRecordFromIndexes(DataRow row, DataRowVersion version)
		{
			int num = this.LiveIndexes.Count;
			int[] array = new int[num];
			int recordFromVersion = row.GetRecordFromVersion(version);
			DataViewRowState recordState = row.GetRecordState(recordFromVersion);
			while (--num >= 0)
			{
				if (row.HasVersion(version) && (recordState & this.indexes[num].RecordStates) != DataViewRowState.None)
				{
					int index = this.indexes[num].GetIndex(recordFromVersion);
					if (index > -1)
					{
						array[num] = index;
						this.indexes[num].DeleteRecordFromIndex(index);
					}
					else
					{
						array[num] = -1;
					}
				}
				else
				{
					array[num] = -1;
				}
			}
			return array;
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x001EF190 File Offset: 0x001EE590
		internal int[] InsertRecordToIndexes(DataRow row, DataRowVersion version)
		{
			int num = this.LiveIndexes.Count;
			int[] array = new int[num];
			int recordFromVersion = row.GetRecordFromVersion(version);
			DataViewRowState recordState = row.GetRecordState(recordFromVersion);
			while (--num >= 0)
			{
				if (row.HasVersion(version))
				{
					if ((recordState & this.indexes[num].RecordStates) != DataViewRowState.None)
					{
						array[num] = this.indexes[num].InsertRecordToIndex(recordFromVersion);
					}
					else
					{
						array[num] = -1;
					}
				}
			}
			return array;
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x001EF204 File Offset: 0x001EE604
		internal void SilentlySetValue(DataRow dr, DataColumn dc, DataRowVersion version, object newValue)
		{
			int recordFromVersion = dr.GetRecordFromVersion(version);
			if ((DataStorage.IsTypeCustomType(dc.DataType) && newValue != dc[recordFromVersion]) || !dc.CompareValueTo(recordFromVersion, newValue, true))
			{
				int[] array = dr.Table.RemoveRecordFromIndexes(dr, version);
				dc.SetValue(recordFromVersion, newValue);
				int[] array2 = dr.Table.InsertRecordToIndexes(dr, version);
				if (dr.HasVersion(version))
				{
					if (version != DataRowVersion.Original)
					{
						dr.Table.RecordChanged(array, array2);
					}
					if (dc.dependentColumns != null)
					{
						dc.Table.EvaluateDependentExpressions(dc.dependentColumns, dr, version, null);
					}
				}
			}
			dr.ResetLastChangedColumn();
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x001EF2AC File Offset: 0x001EE6AC
		public void RejectChanges()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.RejectChanges|API> %d#\n", this.ObjectID);
			try
			{
				DataRow[] array = new DataRow[this.Rows.Count];
				this.Rows.CopyTo(array, 0);
				for (int i = 0; i < array.Length; i++)
				{
					this.RollbackRow(array[i]);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x001EF328 File Offset: 0x001EE728
		internal void RemoveRow(DataRow row, bool check)
		{
			if (row.rowID == -1L)
			{
				throw ExceptionBuilder.RowAlreadyRemoved();
			}
			if (check && this.dataSet != null)
			{
				ParentForeignKeyConstraintEnumerator parentForeignKeyConstraintEnumerator = new ParentForeignKeyConstraintEnumerator(this.dataSet, this);
				while (parentForeignKeyConstraintEnumerator.GetNext())
				{
					parentForeignKeyConstraintEnumerator.GetForeignKeyConstraint().CheckCanRemoveParentRow(row);
				}
			}
			int num = row.oldRecord;
			int newRecord = row.newRecord;
			DataViewRowState recordState = row.GetRecordState(num);
			DataViewRowState recordState2 = row.GetRecordState(newRecord);
			row.oldRecord = -1;
			row.newRecord = -1;
			if (num == newRecord)
			{
				num = -1;
			}
			this.RecordStateChanged(num, recordState, DataViewRowState.None, newRecord, recordState2, DataViewRowState.None);
			this.FreeRecord(ref num);
			this.FreeRecord(ref newRecord);
			row.rowID = -1L;
			this.Rows.ArrayRemove(row);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x001EF3D8 File Offset: 0x001EE7D8
		public virtual void Reset()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.Reset|API> %d#\n", this.ObjectID);
			try
			{
				this.Clear();
				this.ResetConstraints();
				DataRelationCollection dataRelationCollection = this.ParentRelations;
				int i = dataRelationCollection.Count;
				while (i > 0)
				{
					i--;
					dataRelationCollection.RemoveAt(i);
				}
				dataRelationCollection = this.ChildRelations;
				i = dataRelationCollection.Count;
				while (i > 0)
				{
					i--;
					dataRelationCollection.RemoveAt(i);
				}
				this.Columns.Clear();
				this.indexes.Clear();
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x001EF480 File Offset: 0x001EE880
		internal void ResetIndexes()
		{
			this.ResetInternalIndexes(null);
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x001EF494 File Offset: 0x001EE894
		internal void ResetInternalIndexes(DataColumn column)
		{
			this.SetShadowIndexes();
			try
			{
				int count = this.shadowIndexes.Count;
				for (int i = 0; i < count; i++)
				{
					Index index = this.shadowIndexes[i];
					if (0 < index.RefCount)
					{
						if (column == null)
						{
							index.Reset();
						}
						else
						{
							bool flag = false;
							foreach (IndexField indexField in index.IndexFields)
							{
								if (object.ReferenceEquals(column, indexField.Column))
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								index.Reset();
							}
						}
					}
				}
			}
			finally
			{
				this.RestoreShadowIndexes();
			}
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x001EF54C File Offset: 0x001EE94C
		internal void RollbackRow(DataRow row)
		{
			row.CancelEdit();
			this.SetNewRecord(row, row.oldRecord, DataRowAction.Rollback, false, true);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x001EF570 File Offset: 0x001EE970
		private DataRowChangeEventArgs RaiseRowChanged(DataRowChangeEventArgs args, DataRow eRow, DataRowAction eAction)
		{
			try
			{
				if (this.UpdatingCurrent(eRow, eAction) && (this.IsTypedDataTable || this.onRowChangedDelegate != null))
				{
					args = this.OnRowChanged(args, eRow, eAction);
				}
				else if (DataRowAction.Delete == eAction && eRow.newRecord == -1 && (this.IsTypedDataTable || this.onRowDeletedDelegate != null))
				{
					if (args == null)
					{
						args = new DataRowChangeEventArgs(eRow, eAction);
					}
					this.OnRowDeleted(args);
				}
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
			}
			return args;
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x001EF608 File Offset: 0x001EEA08
		private DataRowChangeEventArgs RaiseRowChanging(DataRowChangeEventArgs args, DataRow eRow, DataRowAction eAction)
		{
			if (this.UpdatingCurrent(eRow, eAction) && (this.IsTypedDataTable || this.onRowChangingDelegate != null))
			{
				eRow.inChangingEvent = true;
				try
				{
					return this.OnRowChanging(args, eRow, eAction);
				}
				finally
				{
					eRow.inChangingEvent = false;
				}
			}
			if (DataRowAction.Delete == eAction && eRow.newRecord != -1 && (this.IsTypedDataTable || this.onRowDeletingDelegate != null))
			{
				eRow.inDeletingEvent = true;
				try
				{
					if (args == null)
					{
						args = new DataRowChangeEventArgs(eRow, eAction);
					}
					this.OnRowDeleting(args);
				}
				finally
				{
					eRow.inDeletingEvent = false;
				}
			}
			return args;
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x001EF6C4 File Offset: 0x001EEAC4
		private DataRowChangeEventArgs RaiseRowChanging(DataRowChangeEventArgs args, DataRow eRow, DataRowAction eAction, bool fireEvent)
		{
			if (this.EnforceConstraints && !this.inLoad)
			{
				int count = this.columnCollection.Count;
				for (int i = 0; i < count; i++)
				{
					DataColumn dataColumn = this.columnCollection[i];
					if (!dataColumn.Computed || eAction != DataRowAction.Add)
					{
						dataColumn.CheckColumnConstraint(eRow, eAction);
					}
				}
				int count2 = this.constraintCollection.Count;
				for (int j = 0; j < count2; j++)
				{
					this.constraintCollection[j].CheckConstraint(eRow, eAction);
				}
			}
			if (fireEvent)
			{
				args = this.RaiseRowChanging(args, eRow, eAction);
			}
			if (!this.inDataLoad && !this.MergingData && eAction != DataRowAction.Nothing && eAction != DataRowAction.ChangeOriginal)
			{
				this.CascadeAll(eRow, eAction);
			}
			return args;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x001EF778 File Offset: 0x001EEB78
		public DataRow[] Select()
		{
			Bid.Trace("<ds.DataTable.Select|API> %d#\n", this.ObjectID);
			return new Select(this, "", "", DataViewRowState.CurrentRows).SelectRows();
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x001EF7AC File Offset: 0x001EEBAC
		public DataRow[] Select(string filterExpression)
		{
			Bid.Trace("<ds.DataTable.Select|API> %d#, filterExpression='%ls'\n", this.ObjectID, filterExpression);
			return new Select(this, filterExpression, "", DataViewRowState.CurrentRows).SelectRows();
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x001EF7E0 File Offset: 0x001EEBE0
		public DataRow[] Select(string filterExpression, string sort)
		{
			Bid.Trace("<ds.DataTable.Select|API> %d#, filterExpression='%ls', sort='%ls'\n", this.ObjectID, filterExpression, sort);
			return new Select(this, filterExpression, sort, DataViewRowState.CurrentRows).SelectRows();
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x001EF810 File Offset: 0x001EEC10
		public DataRow[] Select(string filterExpression, string sort, DataViewRowState recordStates)
		{
			Bid.Trace("<ds.DataTable.Select|API> %d#, filterExpression='%ls', sort='%ls', recordStates=%d{ds.DataViewRowState}\n", this.ObjectID, filterExpression, sort, (int)recordStates);
			return new Select(this, filterExpression, sort, recordStates).SelectRows();
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x001EF840 File Offset: 0x001EEC40
		internal void SetNewRecord(DataRow row, int proposedRecord, DataRowAction action, bool isInMerge, bool fireEvent)
		{
			Exception ex = null;
			this.SetNewRecordWorker(row, proposedRecord, action, isInMerge, -1, fireEvent, out ex);
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x001EF864 File Offset: 0x001EEC64
		private void SetNewRecordWorker(DataRow row, int proposedRecord, DataRowAction action, bool isInMerge, int position, bool fireEvent, out Exception deferredException)
		{
			deferredException = null;
			if (row.tempRecord != proposedRecord)
			{
				if (!this.inDataLoad)
				{
					row.CheckInTable();
					this.CheckNotModifying(row);
				}
				if (proposedRecord == row.newRecord)
				{
					if (isInMerge)
					{
						this.RaiseRowChanged(null, row, action);
					}
					return;
				}
				row.tempRecord = proposedRecord;
			}
			DataRowChangeEventArgs dataRowChangeEventArgs = null;
			try
			{
				row._action = action;
				dataRowChangeEventArgs = this.RaiseRowChanging(null, row, action, fireEvent);
			}
			catch
			{
				row.tempRecord = -1;
				throw;
			}
			finally
			{
				row._action = DataRowAction.Nothing;
			}
			row.tempRecord = -1;
			int newRecord = row.newRecord;
			int num = ((proposedRecord != -1) ? proposedRecord : ((row.RowState != DataRowState.Unchanged) ? row.oldRecord : (-1)));
			if (action == DataRowAction.Add)
			{
				if (position == -1)
				{
					this.Rows.ArrayAdd(row);
				}
				else
				{
					this.Rows.ArrayInsert(row, position);
				}
			}
			List<DataRow> list = null;
			if ((action == DataRowAction.Delete || action == DataRowAction.Change) && this.dependentColumns != null && this.dependentColumns.Count > 0)
			{
				list = new List<DataRow>();
				for (int i = 0; i < this.ParentRelations.Count; i++)
				{
					DataRelation dataRelation = this.ParentRelations[i];
					if (dataRelation.ChildTable == row.Table)
					{
						list.InsertRange(list.Count, row.GetParentRows(dataRelation));
					}
				}
				for (int j = 0; j < this.ChildRelations.Count; j++)
				{
					DataRelation dataRelation2 = this.ChildRelations[j];
					if (dataRelation2.ParentTable == row.Table)
					{
						list.InsertRange(list.Count, row.GetChildRows(dataRelation2));
					}
				}
			}
			if (this.LiveIndexes.Count != 0)
			{
				DataViewRowState recordState = row.GetRecordState(newRecord);
				DataViewRowState recordState2 = row.GetRecordState(num);
				row.newRecord = proposedRecord;
				if (proposedRecord != -1)
				{
					this.recordManager[proposedRecord] = row;
				}
				DataViewRowState recordState3 = row.GetRecordState(newRecord);
				DataViewRowState recordState4 = row.GetRecordState(num);
				this.RecordStateChanged(newRecord, recordState, recordState3, num, recordState2, recordState4);
			}
			else
			{
				row.newRecord = proposedRecord;
				if (proposedRecord != -1)
				{
					this.recordManager[proposedRecord] = row;
				}
			}
			if (-1 != newRecord && newRecord != row.oldRecord && newRecord != row.tempRecord && newRecord != row.newRecord && row == this.recordManager[newRecord])
			{
				this.FreeRecord(ref newRecord);
			}
			if (row.RowState == DataRowState.Detached && row.rowID != -1L)
			{
				this.RemoveRow(row, false);
			}
			if (this.dependentColumns != null && this.dependentColumns.Count > 0)
			{
				try
				{
					this.EvaluateExpressions(row, action, list);
				}
				catch (Exception ex)
				{
					if (action != DataRowAction.Add)
					{
						throw ex;
					}
					deferredException = ex;
				}
			}
			try
			{
				if (fireEvent)
				{
					this.RaiseRowChanged(dataRowChangeEventArgs, row, action);
				}
			}
			catch (Exception ex2)
			{
				if (!ADP.IsCatchableExceptionType(ex2))
				{
					throw;
				}
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex2);
			}
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x001EFB74 File Offset: 0x001EEF74
		internal void SetOldRecord(DataRow row, int proposedRecord)
		{
			if (!this.inDataLoad)
			{
				row.CheckInTable();
				this.CheckNotModifying(row);
			}
			if (proposedRecord == row.oldRecord)
			{
				return;
			}
			int oldRecord = row.oldRecord;
			try
			{
				if (this.LiveIndexes.Count != 0)
				{
					DataViewRowState recordState = row.GetRecordState(oldRecord);
					DataViewRowState recordState2 = row.GetRecordState(proposedRecord);
					row.oldRecord = proposedRecord;
					if (proposedRecord != -1)
					{
						this.recordManager[proposedRecord] = row;
					}
					DataViewRowState recordState3 = row.GetRecordState(oldRecord);
					DataViewRowState recordState4 = row.GetRecordState(proposedRecord);
					this.RecordStateChanged(oldRecord, recordState, recordState3, proposedRecord, recordState2, recordState4);
				}
				else
				{
					row.oldRecord = proposedRecord;
					if (proposedRecord != -1)
					{
						this.recordManager[proposedRecord] = row;
					}
				}
			}
			finally
			{
				if (oldRecord != -1 && oldRecord != row.tempRecord && oldRecord != row.oldRecord && oldRecord != row.newRecord)
				{
					this.FreeRecord(ref oldRecord);
				}
				if (row.RowState == DataRowState.Detached && row.rowID != -1L)
				{
					this.RemoveRow(row, false);
				}
			}
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x001EFC78 File Offset: 0x001EF078
		private void RestoreShadowIndexes()
		{
			this.shadowCount--;
			if (this.shadowCount == 0)
			{
				this.shadowIndexes = null;
			}
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x001EFCA4 File Offset: 0x001EF0A4
		private void SetShadowIndexes()
		{
			if (this.shadowIndexes == null)
			{
				this.shadowIndexes = this.LiveIndexes;
				this.shadowCount = 1;
				return;
			}
			this.shadowCount++;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x001EFCDC File Offset: 0x001EF0DC
		internal void ShadowIndexCopy()
		{
			if (this.shadowIndexes == this.indexes)
			{
				this.shadowIndexes = new List<Index>(this.indexes);
			}
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x001EFD08 File Offset: 0x001EF108
		public override string ToString()
		{
			if (this.displayExpression == null)
			{
				return this.TableName;
			}
			return this.TableName + " + " + this.DisplayExpressionInternal;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x001EFD3C File Offset: 0x001EF13C
		public void BeginLoadData()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.BeginLoadData|API> %d#\n", this.ObjectID);
			try
			{
				if (!this.inDataLoad)
				{
					this.inDataLoad = true;
					this.loadIndex = null;
					this.initialLoad = this.Rows.Count == 0;
					if (this.initialLoad)
					{
						this.SuspendIndexEvents();
					}
					else
					{
						if (this.primaryKey != null)
						{
							this.loadIndex = this.primaryKey.Key.GetSortIndex(DataViewRowState.OriginalRows);
						}
						if (this.loadIndex != null)
						{
							this.loadIndex.AddRef();
						}
					}
					if (this.DataSet != null)
					{
						this.savedEnforceConstraints = this.DataSet.EnforceConstraints;
						this.DataSet.EnforceConstraints = false;
					}
					else
					{
						this.EnforceConstraints = false;
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x001EFE24 File Offset: 0x001EF224
		public void EndLoadData()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.EndLoadData|API> %d#\n", this.ObjectID);
			try
			{
				if (this.inDataLoad)
				{
					if (this.loadIndex != null)
					{
						this.loadIndex.RemoveRef();
					}
					if (this.loadIndexwithOriginalAdded != null)
					{
						this.loadIndexwithOriginalAdded.RemoveRef();
					}
					if (this.loadIndexwithCurrentDeleted != null)
					{
						this.loadIndexwithCurrentDeleted.RemoveRef();
					}
					this.loadIndex = null;
					this.loadIndexwithOriginalAdded = null;
					this.loadIndexwithCurrentDeleted = null;
					this.inDataLoad = false;
					this.RestoreIndexEvents(false);
					if (this.DataSet != null)
					{
						this.DataSet.EnforceConstraints = this.savedEnforceConstraints;
					}
					else
					{
						this.EnforceConstraints = true;
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x001EFEF8 File Offset: 0x001EF2F8
		public DataRow LoadDataRow(object[] values, bool fAcceptChanges)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.LoadDataRow|API> %d#, fAcceptChanges=%d{bool}\n", this.ObjectID, fAcceptChanges);
			DataRow dataRow2;
			try
			{
				if (this.inDataLoad)
				{
					int num = this.NewRecordFromArray(values);
					DataRow dataRow;
					if (this.loadIndex != null)
					{
						int num2 = this.loadIndex.FindRecord(num);
						if (num2 != -1)
						{
							int record = this.loadIndex.GetRecord(num2);
							dataRow = this.recordManager[record];
							dataRow.CancelEdit();
							if (dataRow.RowState == DataRowState.Deleted)
							{
								this.SetNewRecord(dataRow, dataRow.oldRecord, DataRowAction.Rollback, false, true);
							}
							this.SetNewRecord(dataRow, num, DataRowAction.Change, false, true);
							if (fAcceptChanges)
							{
								dataRow.AcceptChanges();
							}
							return dataRow;
						}
					}
					dataRow = this.NewRow(num);
					this.AddRow(dataRow);
					if (fAcceptChanges)
					{
						dataRow.AcceptChanges();
					}
					dataRow2 = dataRow;
				}
				else
				{
					DataRow dataRow = this.UpdatingAdd(values);
					if (fAcceptChanges)
					{
						dataRow.AcceptChanges();
					}
					dataRow2 = dataRow;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataRow2;
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x001EFFF0 File Offset: 0x001EF3F0
		public DataRow LoadDataRow(object[] values, LoadOption loadOption)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.LoadDataRow|API> %d#, loadOption=%d{ds.LoadOption}\n", this.ObjectID, (int)loadOption);
			DataRow dataRow2;
			try
			{
				Index index = null;
				if (this.primaryKey != null)
				{
					if (loadOption == LoadOption.Upsert)
					{
						if (this.loadIndexwithCurrentDeleted == null)
						{
							this.loadIndexwithCurrentDeleted = this.primaryKey.Key.GetSortIndex(DataViewRowState.Unchanged | DataViewRowState.Added | DataViewRowState.Deleted | DataViewRowState.ModifiedCurrent);
							if (this.loadIndexwithCurrentDeleted != null)
							{
								this.loadIndexwithCurrentDeleted.AddRef();
							}
						}
						index = this.loadIndexwithCurrentDeleted;
					}
					else
					{
						if (this.loadIndexwithOriginalAdded == null)
						{
							this.loadIndexwithOriginalAdded = this.primaryKey.Key.GetSortIndex(DataViewRowState.Unchanged | DataViewRowState.Added | DataViewRowState.Deleted | DataViewRowState.ModifiedOriginal);
							if (this.loadIndexwithOriginalAdded != null)
							{
								this.loadIndexwithOriginalAdded.AddRef();
							}
						}
						index = this.loadIndexwithOriginalAdded;
					}
				}
				if (this.inDataLoad && !this.AreIndexEventsSuspended)
				{
					this.SuspendIndexEvents();
				}
				DataRow dataRow = this.LoadRow(values, loadOption, index);
				dataRow2 = dataRow;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataRow2;
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x001F00E8 File Offset: 0x001EF4E8
		internal DataRow UpdatingAdd(object[] values)
		{
			Index index = null;
			if (this.primaryKey != null)
			{
				index = this.primaryKey.Key.GetSortIndex(DataViewRowState.OriginalRows);
			}
			if (index == null)
			{
				return this.Rows.Add(values);
			}
			int num = this.NewRecordFromArray(values);
			int num2 = index.FindRecord(num);
			if (num2 != -1)
			{
				int record = index.GetRecord(num2);
				DataRow dataRow = this.recordManager[record];
				dataRow.RejectChanges();
				dataRow.SetNewRecord(num);
				return dataRow;
			}
			DataRow dataRow2 = this.NewRow(num);
			this.Rows.Add(dataRow2);
			return dataRow2;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x001F0178 File Offset: 0x001EF578
		internal bool UpdatingCurrent(DataRow row, DataRowAction action)
		{
			return action == DataRowAction.Add || action == DataRowAction.Change || action == DataRowAction.Rollback || action == DataRowAction.ChangeOriginal || action == DataRowAction.ChangeCurrentAndOriginal;
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x001F01A0 File Offset: 0x001EF5A0
		internal DataColumn AddUniqueKey(int position)
		{
			if (this._colUnique != null)
			{
				return this._colUnique;
			}
			DataColumn[] array = this.PrimaryKey;
			if (array.Length == 1)
			{
				return array[0];
			}
			string text = XMLSchema.GenUniqueColumnName(this.TableName + "_Id", this);
			DataColumn dataColumn = new DataColumn(text, typeof(int), null, MappingType.Hidden);
			dataColumn.Prefix = this.tablePrefix;
			dataColumn.AutoIncrement = true;
			dataColumn.AllowDBNull = false;
			dataColumn.Unique = true;
			if (position == -1)
			{
				this.Columns.Add(dataColumn);
			}
			else
			{
				for (int i = this.Columns.Count - 1; i >= position; i--)
				{
					this.Columns[i].SetOrdinalInternal(i + 1);
				}
				this.Columns.AddAt(position, dataColumn);
				dataColumn.SetOrdinalInternal(position);
			}
			if (array.Length == 0)
			{
				this.PrimaryKey = new DataColumn[] { dataColumn };
			}
			this._colUnique = dataColumn;
			return this._colUnique;
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x001F0290 File Offset: 0x001EF690
		internal DataColumn AddUniqueKey()
		{
			return this.AddUniqueKey(-1);
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x001F02A4 File Offset: 0x001EF6A4
		internal DataColumn AddForeignKey(DataColumn parentKey)
		{
			string text = XMLSchema.GenUniqueColumnName(parentKey.ColumnName, this);
			DataColumn dataColumn = new DataColumn(text, parentKey.DataType, null, MappingType.Hidden);
			this.Columns.Add(dataColumn);
			return dataColumn;
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x001F02DC File Offset: 0x001EF6DC
		internal void UpdatePropertyDescriptorCollectionCache()
		{
			this.propertyDescriptorCollectionCache = null;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x001F02F0 File Offset: 0x001EF6F0
		internal PropertyDescriptorCollection GetPropertyDescriptorCollection(Attribute[] attributes)
		{
			if (this.propertyDescriptorCollectionCache == null)
			{
				int count = this.Columns.Count;
				int count2 = this.ChildRelations.Count;
				PropertyDescriptor[] array = new PropertyDescriptor[count + count2];
				for (int i = 0; i < count; i++)
				{
					array[i] = new DataColumnPropertyDescriptor(this.Columns[i]);
				}
				for (int j = 0; j < count2; j++)
				{
					array[count + j] = new DataRelationPropertyDescriptor(this.ChildRelations[j]);
				}
				this.propertyDescriptorCollectionCache = new PropertyDescriptorCollection(array);
			}
			return this.propertyDescriptorCollectionCache;
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x001F0380 File Offset: 0x001EF780
		// (set) Token: 0x06000A1D RID: 2589 RVA: 0x001F03A8 File Offset: 0x001EF7A8
		internal XmlQualifiedName TypeName
		{
			get
			{
				if (this.typeName != null)
				{
					return (XmlQualifiedName)this.typeName;
				}
				return XmlQualifiedName.Empty;
			}
			set
			{
				this.typeName = value;
			}
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x001F03BC File Offset: 0x001EF7BC
		public void Merge(DataTable table)
		{
			this.Merge(table, false, MissingSchemaAction.Add);
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x001F03D4 File Offset: 0x001EF7D4
		public void Merge(DataTable table, bool preserveChanges)
		{
			this.Merge(table, preserveChanges, MissingSchemaAction.Add);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x001F03EC File Offset: 0x001EF7EC
		public void Merge(DataTable table, bool preserveChanges, MissingSchemaAction missingSchemaAction)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.Merge|API> %d#, table=%d, preserveChanges=%d{bool}, missingSchemaAction=%d{ds.MissingSchemaAction}\n", this.ObjectID, (table != null) ? table.ObjectID : 0, preserveChanges, (int)missingSchemaAction);
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

		// Token: 0x06000A21 RID: 2593 RVA: 0x001F0484 File Offset: 0x001EF884
		public void Load(IDataReader reader)
		{
			this.Load(reader, LoadOption.PreserveChanges, null);
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x001F049C File Offset: 0x001EF89C
		public void Load(IDataReader reader, LoadOption loadOption)
		{
			this.Load(reader, loadOption, null);
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x001F04B4 File Offset: 0x001EF8B4
		public virtual void Load(IDataReader reader, LoadOption loadOption, FillErrorEventHandler errorHandler)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.Load|API> %d#, loadOption=%d{ds.LoadOption}\n", this.ObjectID, (int)loadOption);
			try
			{
				if (this.PrimaryKey.Length == 0)
				{
					DataTableReader dataTableReader = reader as DataTableReader;
					if (dataTableReader != null && dataTableReader.CurrentDataTable == this)
					{
						return;
					}
				}
				LoadAdapter loadAdapter = new LoadAdapter();
				loadAdapter.FillLoadOption = loadOption;
				loadAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
				if (errorHandler != null)
				{
					loadAdapter.FillError += errorHandler;
				}
				loadAdapter.FillFromReader(new DataTable[] { this }, reader, 0, 0);
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

		// Token: 0x06000A24 RID: 2596 RVA: 0x001F0564 File Offset: 0x001EF964
		private DataRow LoadRow(object[] values, LoadOption loadOption, Index searchIndex)
		{
			DataRow dataRow = null;
			int num2;
			if (searchIndex != null)
			{
				int[] array = new int[0];
				if (this.primaryKey != null)
				{
					array = new int[this.primaryKey.ColumnsReference.Length];
					for (int i = 0; i < this.primaryKey.ColumnsReference.Length; i++)
					{
						array[i] = this.primaryKey.ColumnsReference[i].Ordinal;
					}
				}
				object[] array2 = new object[array.Length];
				for (int j = 0; j < array.Length; j++)
				{
					array2[j] = values[array[j]];
				}
				Range range = searchIndex.FindRecords(array2);
				if (!range.IsNull)
				{
					int num = 0;
					for (int k = range.Min; k <= range.Max; k++)
					{
						int record = searchIndex.GetRecord(k);
						dataRow = this.recordManager[record];
						num2 = this.NewRecordFromArray(values);
						for (int l = 0; l < values.Length; l++)
						{
							if (values[l] == null)
							{
								this.columnCollection[l].Copy(record, num2);
							}
						}
						for (int m = values.Length; m < this.columnCollection.Count; m++)
						{
							this.columnCollection[m].Copy(record, num2);
						}
						if (loadOption != LoadOption.Upsert || dataRow.RowState != DataRowState.Deleted)
						{
							this.SetDataRowWithLoadOption(dataRow, num2, loadOption, true);
						}
						else
						{
							num++;
						}
					}
					if (num == 0)
					{
						return dataRow;
					}
				}
			}
			num2 = this.NewRecordFromArray(values);
			dataRow = this.NewRow(num2);
			DataRowAction dataRowAction;
			switch (loadOption)
			{
			case LoadOption.OverwriteChanges:
			case LoadOption.PreserveChanges:
				dataRowAction = DataRowAction.ChangeCurrentAndOriginal;
				break;
			case LoadOption.Upsert:
				dataRowAction = DataRowAction.Add;
				break;
			default:
				throw ExceptionBuilder.ArgumentOutOfRange("LoadOption");
			}
			DataRowChangeEventArgs dataRowChangeEventArgs = this.RaiseRowChanging(null, dataRow, dataRowAction);
			this.InsertRow(dataRow, -1L, -1, false);
			switch (loadOption)
			{
			case LoadOption.OverwriteChanges:
			case LoadOption.PreserveChanges:
				this.SetOldRecord(dataRow, num2);
				break;
			case LoadOption.Upsert:
				break;
			default:
				throw ExceptionBuilder.ArgumentOutOfRange("LoadOption");
			}
			this.RaiseRowChanged(dataRowChangeEventArgs, dataRow, dataRowAction);
			return dataRow;
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x001F0764 File Offset: 0x001EFB64
		private void SetDataRowWithLoadOption(DataRow dataRow, int recordNo, LoadOption loadOption, bool checkReadOnly)
		{
			bool flag = false;
			if (checkReadOnly)
			{
				foreach (object obj in this.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj;
					if (dataColumn.ReadOnly && !dataColumn.Computed)
					{
						switch (loadOption)
						{
						case LoadOption.OverwriteChanges:
							if (dataRow[dataColumn, DataRowVersion.Current] != dataColumn[recordNo] || dataRow[dataColumn, DataRowVersion.Original] != dataColumn[recordNo])
							{
								flag = true;
							}
							break;
						case LoadOption.PreserveChanges:
							if (dataRow[dataColumn, DataRowVersion.Original] != dataColumn[recordNo])
							{
								flag = true;
							}
							break;
						case LoadOption.Upsert:
							if (dataRow[dataColumn, DataRowVersion.Current] != dataColumn[recordNo])
							{
								flag = true;
							}
							break;
						}
					}
				}
			}
			DataRowChangeEventArgs dataRowChangeEventArgs = null;
			DataRowAction dataRowAction = DataRowAction.Nothing;
			int tempRecord = dataRow.tempRecord;
			dataRow.tempRecord = recordNo;
			switch (loadOption)
			{
			case LoadOption.OverwriteChanges:
				dataRowAction = DataRowAction.ChangeCurrentAndOriginal;
				break;
			case LoadOption.PreserveChanges:
			{
				DataRowState rowState = dataRow.RowState;
				if (rowState == DataRowState.Unchanged)
				{
					dataRowAction = DataRowAction.ChangeCurrentAndOriginal;
				}
				else
				{
					dataRowAction = DataRowAction.ChangeOriginal;
				}
				break;
			}
			case LoadOption.Upsert:
			{
				DataRowState rowState2 = dataRow.RowState;
				if (rowState2 != DataRowState.Unchanged)
				{
					if (rowState2 == DataRowState.Deleted)
					{
						break;
					}
				}
				else
				{
					using (IEnumerator enumerator2 = dataRow.Table.Columns.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							DataColumn dataColumn2 = (DataColumn)obj2;
							if (dataColumn2.Compare(dataRow.newRecord, recordNo) != 0)
							{
								dataRowAction = DataRowAction.Change;
								break;
							}
						}
						break;
					}
				}
				dataRowAction = DataRowAction.Change;
				break;
			}
			default:
				throw ExceptionBuilder.ArgumentOutOfRange("LoadOption");
			}
			try
			{
				dataRowChangeEventArgs = this.RaiseRowChanging(null, dataRow, dataRowAction);
				if (dataRowAction == DataRowAction.Nothing)
				{
					dataRow.inChangingEvent = true;
					try
					{
						dataRowChangeEventArgs = this.OnRowChanging(dataRowChangeEventArgs, dataRow, dataRowAction);
					}
					finally
					{
						dataRow.inChangingEvent = false;
					}
				}
			}
			finally
			{
				if (DataRowState.Detached == dataRow.RowState)
				{
					if (-1 != tempRecord)
					{
						this.FreeRecord(ref tempRecord);
					}
				}
				else if (dataRow.tempRecord != recordNo)
				{
					if (-1 != tempRecord)
					{
						this.FreeRecord(ref tempRecord);
					}
					if (-1 != recordNo)
					{
						this.FreeRecord(ref recordNo);
					}
					recordNo = dataRow.tempRecord;
				}
				else
				{
					dataRow.tempRecord = tempRecord;
				}
			}
			if (dataRow.tempRecord != -1)
			{
				dataRow.CancelEdit();
			}
			switch (loadOption)
			{
			case LoadOption.OverwriteChanges:
				this.SetNewRecord(dataRow, recordNo, DataRowAction.Change, false, false);
				this.SetOldRecord(dataRow, recordNo);
				break;
			case LoadOption.PreserveChanges:
				if (dataRow.RowState == DataRowState.Unchanged)
				{
					this.SetOldRecord(dataRow, recordNo);
					this.SetNewRecord(dataRow, recordNo, DataRowAction.Change, false, false);
				}
				else
				{
					this.SetOldRecord(dataRow, recordNo);
				}
				break;
			case LoadOption.Upsert:
				if (dataRow.RowState == DataRowState.Unchanged)
				{
					this.SetNewRecord(dataRow, recordNo, DataRowAction.Change, false, false);
					if (!dataRow.HasChanges())
					{
						this.SetOldRecord(dataRow, recordNo);
					}
				}
				else
				{
					if (dataRow.RowState == DataRowState.Deleted)
					{
						dataRow.RejectChanges();
					}
					this.SetNewRecord(dataRow, recordNo, DataRowAction.Change, false, false);
				}
				break;
			default:
				throw ExceptionBuilder.ArgumentOutOfRange("LoadOption");
			}
			if (flag)
			{
				string @string = Res.GetString("Load_ReadOnlyDataModified");
				if (dataRow.RowError.Length == 0)
				{
					dataRow.RowError = @string;
				}
				else
				{
					dataRow.RowError = dataRow.RowError + " ]:[ " + @string;
				}
				foreach (object obj3 in this.Columns)
				{
					DataColumn dataColumn3 = (DataColumn)obj3;
					if (dataColumn3.ReadOnly && !dataColumn3.Computed)
					{
						dataRow.SetColumnError(dataColumn3, @string);
					}
				}
			}
			dataRowChangeEventArgs = this.RaiseRowChanged(dataRowChangeEventArgs, dataRow, dataRowAction);
			if (dataRowAction == DataRowAction.Nothing)
			{
				dataRow.inChangingEvent = true;
				try
				{
					this.OnRowChanged(dataRowChangeEventArgs, dataRow, dataRowAction);
				}
				finally
				{
					dataRow.inChangingEvent = false;
				}
			}
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x001F0B9C File Offset: 0x001EFF9C
		public DataTableReader CreateDataReader()
		{
			return new DataTableReader(this);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x001F0BB0 File Offset: 0x001EFFB0
		public void WriteXml(Stream stream)
		{
			this.WriteXml(stream, XmlWriteMode.IgnoreSchema, false);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x001F0BC8 File Offset: 0x001EFFC8
		public void WriteXml(Stream stream, bool writeHierarchy)
		{
			this.WriteXml(stream, XmlWriteMode.IgnoreSchema, writeHierarchy);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x001F0BE0 File Offset: 0x001EFFE0
		public void WriteXml(TextWriter writer)
		{
			this.WriteXml(writer, XmlWriteMode.IgnoreSchema, false);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x001F0BF8 File Offset: 0x001EFFF8
		public void WriteXml(TextWriter writer, bool writeHierarchy)
		{
			this.WriteXml(writer, XmlWriteMode.IgnoreSchema, writeHierarchy);
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x001F0C10 File Offset: 0x001F0010
		public void WriteXml(XmlWriter writer)
		{
			this.WriteXml(writer, XmlWriteMode.IgnoreSchema, false);
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x001F0C28 File Offset: 0x001F0028
		public void WriteXml(XmlWriter writer, bool writeHierarchy)
		{
			this.WriteXml(writer, XmlWriteMode.IgnoreSchema, writeHierarchy);
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x001F0C40 File Offset: 0x001F0040
		public void WriteXml(string fileName)
		{
			this.WriteXml(fileName, XmlWriteMode.IgnoreSchema, false);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x001F0C58 File Offset: 0x001F0058
		public void WriteXml(string fileName, bool writeHierarchy)
		{
			this.WriteXml(fileName, XmlWriteMode.IgnoreSchema, writeHierarchy);
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x001F0C70 File Offset: 0x001F0070
		public void WriteXml(Stream stream, XmlWriteMode mode)
		{
			this.WriteXml(stream, mode, false);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x001F0C88 File Offset: 0x001F0088
		public void WriteXml(Stream stream, XmlWriteMode mode, bool writeHierarchy)
		{
			if (stream != null)
			{
				this.WriteXml(new XmlTextWriter(stream, null)
				{
					Formatting = Formatting.Indented
				}, mode, writeHierarchy);
			}
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x001F0CB0 File Offset: 0x001F00B0
		public void WriteXml(TextWriter writer, XmlWriteMode mode)
		{
			this.WriteXml(writer, mode, false);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x001F0CC8 File Offset: 0x001F00C8
		public void WriteXml(TextWriter writer, XmlWriteMode mode, bool writeHierarchy)
		{
			if (writer != null)
			{
				this.WriteXml(new XmlTextWriter(writer)
				{
					Formatting = Formatting.Indented
				}, mode, writeHierarchy);
			}
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x001F0CF0 File Offset: 0x001F00F0
		public void WriteXml(XmlWriter writer, XmlWriteMode mode)
		{
			this.WriteXml(writer, mode, false);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x001F0D08 File Offset: 0x001F0108
		public void WriteXml(XmlWriter writer, XmlWriteMode mode, bool writeHierarchy)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.WriteXml|API> %d#, mode=%d{ds.XmlWriteMode}\n", this.ObjectID, (int)mode);
			try
			{
				if (this.tableName.Length == 0)
				{
					throw ExceptionBuilder.CanNotSerializeDataTableWithEmptyName();
				}
				if (writer != null)
				{
					if (mode == XmlWriteMode.DiffGram)
					{
						new NewDiffgramGen(this, writeHierarchy).Save(writer, this);
					}
					else if (mode == XmlWriteMode.WriteSchema)
					{
						DataSet dataSet = null;
						string text = this.tableNamespace;
						if (this.DataSet == null)
						{
							dataSet = new DataSet();
							dataSet.SetLocaleValue(this._culture, this._cultureUserSet);
							dataSet.CaseSensitive = this.CaseSensitive;
							dataSet.Namespace = this.Namespace;
							dataSet.RemotingFormat = this.RemotingFormat;
							dataSet.Tables.Add(this);
						}
						if (writer != null)
						{
							XmlDataTreeWriter xmlDataTreeWriter = new XmlDataTreeWriter(this, writeHierarchy);
							xmlDataTreeWriter.Save(writer, true);
						}
						if (dataSet != null)
						{
							dataSet.Tables.Remove(this);
							this.tableNamespace = text;
						}
					}
					else
					{
						XmlDataTreeWriter xmlDataTreeWriter2 = new XmlDataTreeWriter(this, writeHierarchy);
						xmlDataTreeWriter2.Save(writer, false);
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x001F0E18 File Offset: 0x001F0218
		public void WriteXml(string fileName, XmlWriteMode mode)
		{
			this.WriteXml(fileName, mode, false);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x001F0E30 File Offset: 0x001F0230
		public void WriteXml(string fileName, XmlWriteMode mode, bool writeHierarchy)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.WriteXml|API> %d#, fileName='%ls', mode=%d{ds.XmlWriteMode}\n", this.ObjectID, fileName, (int)mode);
			try
			{
				using (XmlTextWriter xmlTextWriter = new XmlTextWriter(fileName, null))
				{
					xmlTextWriter.Formatting = Formatting.Indented;
					xmlTextWriter.WriteStartDocument(true);
					this.WriteXml(xmlTextWriter, mode, writeHierarchy);
					xmlTextWriter.WriteEndDocument();
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x001F0EC0 File Offset: 0x001F02C0
		public void WriteXmlSchema(Stream stream)
		{
			this.WriteXmlSchema(stream, false);
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x001F0ED8 File Offset: 0x001F02D8
		public void WriteXmlSchema(Stream stream, bool writeHierarchy)
		{
			if (stream == null)
			{
				return;
			}
			this.WriteXmlSchema(new XmlTextWriter(stream, null)
			{
				Formatting = Formatting.Indented
			}, writeHierarchy);
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x001F0F00 File Offset: 0x001F0300
		public void WriteXmlSchema(TextWriter writer)
		{
			this.WriteXmlSchema(writer, false);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x001F0F18 File Offset: 0x001F0318
		public void WriteXmlSchema(TextWriter writer, bool writeHierarchy)
		{
			if (writer == null)
			{
				return;
			}
			this.WriteXmlSchema(new XmlTextWriter(writer)
			{
				Formatting = Formatting.Indented
			}, writeHierarchy);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x001F0F40 File Offset: 0x001F0340
		private bool CheckForClosureOnExpressions(DataTable dt, bool writeHierarchy)
		{
			List<DataTable> list = new List<DataTable>();
			list.Add(dt);
			if (writeHierarchy)
			{
				this.CreateTableList(dt, list);
			}
			return this.CheckForClosureOnExpressionTables(list);
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x001F0F6C File Offset: 0x001F036C
		private bool CheckForClosureOnExpressionTables(List<DataTable> tableList)
		{
			foreach (DataTable dataTable in tableList)
			{
				foreach (object obj in dataTable.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj;
					if (dataColumn.Expression.Length != 0)
					{
						DataColumn[] dependency = dataColumn.DataExpression.GetDependency();
						for (int i = 0; i < dependency.Length; i++)
						{
							if (!tableList.Contains(dependency[i].Table))
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x001F1058 File Offset: 0x001F0458
		public void WriteXmlSchema(XmlWriter writer)
		{
			this.WriteXmlSchema(writer, false);
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x001F1070 File Offset: 0x001F0470
		public void WriteXmlSchema(XmlWriter writer, bool writeHierarchy)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.WriteXmlSchema|API> %d#\n", this.ObjectID);
			try
			{
				if (this.tableName.Length == 0)
				{
					throw ExceptionBuilder.CanNotSerializeDataTableWithEmptyName();
				}
				if (!this.CheckForClosureOnExpressions(this, writeHierarchy))
				{
					throw ExceptionBuilder.CanNotSerializeDataTableHierarchy();
				}
				DataSet dataSet = null;
				string text = this.tableNamespace;
				if (this.DataSet == null)
				{
					dataSet = new DataSet();
					dataSet.SetLocaleValue(this._culture, this._cultureUserSet);
					dataSet.CaseSensitive = this.CaseSensitive;
					dataSet.Namespace = this.Namespace;
					dataSet.RemotingFormat = this.RemotingFormat;
					dataSet.Tables.Add(this);
				}
				if (writer != null)
				{
					XmlTreeGen xmlTreeGen = new XmlTreeGen(SchemaFormat.Public);
					xmlTreeGen.Save(null, this, writer, writeHierarchy);
				}
				if (dataSet != null)
				{
					dataSet.Tables.Remove(this);
					this.tableNamespace = text;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x001F115C File Offset: 0x001F055C
		public void WriteXmlSchema(string fileName)
		{
			this.WriteXmlSchema(fileName, false);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x001F1174 File Offset: 0x001F0574
		public void WriteXmlSchema(string fileName, bool writeHierarchy)
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(fileName, null);
			try
			{
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.WriteStartDocument(true);
				this.WriteXmlSchema(xmlTextWriter, writeHierarchy);
				xmlTextWriter.WriteEndDocument();
			}
			finally
			{
				xmlTextWriter.Close();
			}
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x001F11CC File Offset: 0x001F05CC
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

		// Token: 0x06000A42 RID: 2626 RVA: 0x001F11F4 File Offset: 0x001F05F4
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

		// Token: 0x06000A43 RID: 2627 RVA: 0x001F121C File Offset: 0x001F061C
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

		// Token: 0x06000A44 RID: 2628 RVA: 0x001F1268 File Offset: 0x001F0668
		public XmlReadMode ReadXml(XmlReader reader)
		{
			return this.ReadXml(reader, false);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x001F1280 File Offset: 0x001F0680
		private void RestoreConstraint(bool originalEnforceConstraint)
		{
			if (this.DataSet != null)
			{
				this.DataSet.EnforceConstraints = originalEnforceConstraint;
				return;
			}
			this.EnforceConstraints = originalEnforceConstraint;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x001F12AC File Offset: 0x001F06AC
		private bool IsEmptyXml(XmlReader reader)
		{
			if (reader.IsEmptyElement)
			{
				if (reader.AttributeCount == 0 || (reader.LocalName == "diffgram" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1"))
				{
					return true;
				}
				if (reader.AttributeCount == 1)
				{
					reader.MoveToAttribute(0);
					if (this.Namespace == reader.Value && this.Prefix == reader.LocalName && reader.Prefix == "xmlns" && reader.NamespaceURI == "http://www.w3.org/2000/xmlns/")
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x001F1350 File Offset: 0x001F0750
		internal XmlReadMode ReadXml(XmlReader reader, bool denyResolving)
		{
			IDisposable disposable = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.ReadXml|INFO> %d#, denyResolving=%d{bool}\n", this.ObjectID, denyResolving);
			XmlReadMode xmlReadMode2;
			try
			{
				disposable = TypeLimiter.EnterRestrictedScope(this);
				try
				{
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					XmlReadMode xmlReadMode = XmlReadMode.Auto;
					this.rowDiffId = null;
					if (reader == null)
					{
						xmlReadMode2 = xmlReadMode;
					}
					else
					{
						bool flag4;
						if (this.DataSet != null)
						{
							flag4 = this.DataSet.EnforceConstraints;
							this.DataSet.EnforceConstraints = false;
						}
						else
						{
							flag4 = this.EnforceConstraints;
							this.EnforceConstraints = false;
						}
						if (reader is XmlTextReader)
						{
							((XmlTextReader)reader).WhitespaceHandling = WhitespaceHandling.Significant;
						}
						XmlDocument xmlDocument = new XmlDocument();
						XmlDataLoader xmlDataLoader = null;
						reader.MoveToContent();
						if (this.Columns.Count == 0 && this.IsEmptyXml(reader))
						{
							reader.Read();
							xmlReadMode2 = xmlReadMode;
						}
						else
						{
							if (reader.NodeType == XmlNodeType.Element)
							{
								int depth = reader.Depth;
								if (reader.LocalName == "diffgram" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1")
								{
									if (this.Columns.Count != 0)
									{
										this.ReadXmlDiffgram(reader);
										this.ReadEndElement(reader);
										this.RestoreConstraint(flag4);
										return XmlReadMode.DiffGram;
									}
									if (reader.IsEmptyElement)
									{
										reader.Read();
										return XmlReadMode.DiffGram;
									}
									throw ExceptionBuilder.DataTableInferenceNotSupported();
								}
								else
								{
									if (reader.LocalName == "Schema" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-data")
									{
										this.ReadXDRSchema(reader);
										this.RestoreConstraint(flag4);
										return XmlReadMode.ReadSchema;
									}
									if (reader.LocalName == "schema" && reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
									{
										this.ReadXmlSchema(reader, denyResolving);
										this.RestoreConstraint(flag4);
										return XmlReadMode.ReadSchema;
									}
									if (reader.LocalName == "schema" && reader.NamespaceURI.StartsWith("http://www.w3.org/", StringComparison.Ordinal))
									{
										if (this.DataSet != null)
										{
											this.DataSet.RestoreEnforceConstraints(flag4);
										}
										else
										{
											this.enforceConstraints = flag4;
										}
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
									while (this.MoveToElement(reader, depth))
									{
										if (reader.LocalName == "diffgram" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1")
										{
											this.ReadXmlDiffgram(reader);
											this.ReadEndElement(reader);
											this.RestoreConstraint(flag4);
											return XmlReadMode.DiffGram;
										}
										if (!flag2 && !flag && reader.LocalName == "Schema" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-data")
										{
											this.ReadXDRSchema(reader);
											flag2 = true;
											flag3 = true;
										}
										else if (reader.LocalName == "schema" && reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
										{
											this.ReadXmlSchema(reader, denyResolving);
											flag2 = true;
										}
										else
										{
											if (reader.LocalName == "schema" && reader.NamespaceURI.StartsWith("http://www.w3.org/", StringComparison.Ordinal))
											{
												if (this.DataSet != null)
												{
													this.DataSet.RestoreEnforceConstraints(flag4);
												}
												else
												{
													this.enforceConstraints = flag4;
												}
												throw ExceptionBuilder.DataSetUnsupportedSchema("http://www.w3.org/2001/XMLSchema");
											}
											if (reader.LocalName == "diffgram" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1")
											{
												this.ReadXmlDiffgram(reader);
												xmlReadMode = XmlReadMode.DiffGram;
											}
											else
											{
												flag = true;
												if (!flag2 && this.Columns.Count == 0)
												{
													XmlNode xmlNode = xmlDocument.ReadNode(reader);
													xmlElement.AppendChild(xmlNode);
												}
												else
												{
													if (xmlDataLoader == null)
													{
														xmlDataLoader = new XmlDataLoader(this, flag3, xmlElement, false);
													}
													xmlDataLoader.LoadData(reader);
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
									this.ReadEndElement(reader);
									xmlDocument.AppendChild(xmlElement);
									if (!flag2 && this.Columns.Count == 0)
									{
										if (this.IsEmptyXml(reader))
										{
											reader.Read();
											return xmlReadMode;
										}
										throw ExceptionBuilder.DataTableInferenceNotSupported();
									}
									else if (xmlDataLoader == null)
									{
										xmlDataLoader = new XmlDataLoader(this, flag3, false);
									}
								}
							}
							this.RestoreConstraint(flag4);
							xmlReadMode2 = xmlReadMode;
						}
					}
				}
				finally
				{
					this.rowDiffId = null;
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

		// Token: 0x06000A48 RID: 2632 RVA: 0x001F1834 File Offset: 0x001F0C34
		internal XmlReadMode ReadXml(XmlReader reader, XmlReadMode mode, bool denyResolving)
		{
			IDisposable disposable = null;
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
				else
				{
					bool flag4;
					if (this.DataSet != null)
					{
						flag4 = this.DataSet.EnforceConstraints;
						this.DataSet.EnforceConstraints = false;
					}
					else
					{
						flag4 = this.EnforceConstraints;
						this.EnforceConstraints = false;
					}
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
					if (this.Columns.Count == 0 && this.IsEmptyXml(reader))
					{
						reader.Read();
						xmlReadMode2 = xmlReadMode;
					}
					else
					{
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
										if (this.Columns.Count == 0)
										{
											if (reader.IsEmptyElement)
											{
												reader.Read();
												return XmlReadMode.DiffGram;
											}
											throw ExceptionBuilder.DataTableInferenceNotSupported();
										}
										else
										{
											this.ReadXmlDiffgram(reader);
											this.ReadEndElement(reader);
										}
									}
									else
									{
										reader.Skip();
									}
									this.RestoreConstraint(flag4);
									return xmlReadMode;
								}
								if (reader.LocalName == "Schema" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-data")
								{
									if (mode != XmlReadMode.IgnoreSchema && mode != XmlReadMode.InferSchema)
									{
										this.ReadXDRSchema(reader);
									}
									else
									{
										reader.Skip();
									}
									this.RestoreConstraint(flag4);
									return xmlReadMode;
								}
								if (reader.LocalName == "schema" && reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
								{
									if (mode != XmlReadMode.IgnoreSchema && mode != XmlReadMode.InferSchema)
									{
										this.ReadXmlSchema(reader, denyResolving);
									}
									else
									{
										reader.Skip();
									}
									this.RestoreConstraint(flag4);
									return xmlReadMode;
								}
								if (reader.LocalName == "schema" && reader.NamespaceURI.StartsWith("http://www.w3.org/", StringComparison.Ordinal))
								{
									if (this.DataSet != null)
									{
										this.DataSet.RestoreEnforceConstraints(flag4);
									}
									else
									{
										this.enforceConstraints = flag4;
									}
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
									if (!flag && !flag2 && mode != XmlReadMode.IgnoreSchema && mode != XmlReadMode.InferSchema)
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
									if (mode != XmlReadMode.IgnoreSchema && mode != XmlReadMode.InferSchema)
									{
										this.ReadXmlSchema(reader, denyResolving);
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
										if (this.Columns.Count == 0)
										{
											if (reader.IsEmptyElement)
											{
												reader.Read();
												return XmlReadMode.DiffGram;
											}
											throw ExceptionBuilder.DataTableInferenceNotSupported();
										}
										else
										{
											this.ReadXmlDiffgram(reader);
											xmlReadMode = XmlReadMode.DiffGram;
										}
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
										if (this.DataSet != null)
										{
											this.DataSet.RestoreEnforceConstraints(flag4);
										}
										else
										{
											this.enforceConstraints = flag4;
										}
										throw ExceptionBuilder.DataSetUnsupportedSchema("http://www.w3.org/2001/XMLSchema");
									}
									if (mode == XmlReadMode.DiffGram)
									{
										reader.Skip();
									}
									else
									{
										flag2 = true;
										if (mode == XmlReadMode.InferSchema)
										{
											XmlNode xmlNode = xmlDocument.ReadNode(reader);
											xmlElement.AppendChild(xmlNode);
										}
										else
										{
											if (this.Columns.Count == 0)
											{
												throw ExceptionBuilder.DataTableInferenceNotSupported();
											}
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
								this.RestoreConstraint(flag4);
								return xmlReadMode;
							}
							if (mode == XmlReadMode.InferSchema && this.Columns.Count == 0)
							{
								throw ExceptionBuilder.DataTableInferenceNotSupported();
							}
						}
						this.RestoreConstraint(flag4);
						xmlReadMode2 = xmlReadMode;
					}
				}
			}
			finally
			{
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return xmlReadMode2;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x001F1D60 File Offset: 0x001F1160
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

		// Token: 0x06000A4A RID: 2634 RVA: 0x001F1DA0 File Offset: 0x001F11A0
		internal void ReadXDRSchema(XmlReader reader)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.ReadNode(reader);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x001F1DBC File Offset: 0x001F11BC
		internal bool MoveToElement(XmlReader reader, int depth)
		{
			while (!reader.EOF && reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.Element && reader.Depth > depth)
			{
				reader.Read();
			}
			return reader.NodeType == XmlNodeType.Element;
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x001F1E00 File Offset: 0x001F1200
		private void ReadXmlDiffgram(XmlReader reader)
		{
			int depth = reader.Depth;
			bool flag = this.EnforceConstraints;
			this.EnforceConstraints = false;
			bool flag2;
			DataTable dataTable;
			if (this.Rows.Count == 0)
			{
				flag2 = true;
				dataTable = this;
			}
			else
			{
				flag2 = false;
				dataTable = this.Clone();
				dataTable.EnforceConstraints = false;
			}
			dataTable.Rows.nullInList = 0;
			reader.MoveToContent();
			if (reader.LocalName != "diffgram" && reader.NamespaceURI != "urn:schemas-microsoft-com:xml-diffgram-v1")
			{
				return;
			}
			reader.Read();
			if (reader.NodeType == XmlNodeType.Whitespace)
			{
				this.MoveToElement(reader, reader.Depth - 1);
			}
			dataTable.fInLoadDiffgram = true;
			if (reader.Depth > depth)
			{
				if (reader.NamespaceURI != "urn:schemas-microsoft-com:xml-diffgram-v1" && reader.NamespaceURI != "urn:schemas-microsoft-com:xml-msdata")
				{
					XmlDocument xmlDocument = new XmlDocument();
					XmlElement xmlElement = xmlDocument.CreateElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
					reader.Read();
					if (reader.Depth - 1 > depth)
					{
						new XmlDataLoader(dataTable, false, xmlElement, false)
						{
							isDiffgram = true
						}.LoadData(reader);
					}
					this.ReadEndElement(reader);
				}
				if ((reader.LocalName == "before" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1") || (reader.LocalName == "errors" && reader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1"))
				{
					XMLDiffLoader xmldiffLoader = new XMLDiffLoader();
					xmldiffLoader.LoadDiffGram(dataTable, reader);
				}
				while (reader.Depth > depth)
				{
					reader.Read();
				}
				this.ReadEndElement(reader);
			}
			if (dataTable.Rows.nullInList > 0)
			{
				throw ExceptionBuilder.RowInsertMissing(dataTable.TableName);
			}
			dataTable.fInLoadDiffgram = false;
			List<DataTable> list = new List<DataTable>();
			list.Add(this);
			this.CreateTableList(this, list);
			for (int i = 0; i < list.Count; i++)
			{
				DataRelation[] nestedParentRelations = list[i].NestedParentRelations;
				foreach (DataRelation dataRelation in nestedParentRelations)
				{
					if (dataRelation != null && dataRelation.ParentTable == list[i])
					{
						foreach (object obj in list[i].Rows)
						{
							DataRow dataRow = (DataRow)obj;
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
				this.Merge(dataTable);
			}
			this.EnforceConstraints = flag;
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x001F20D0 File Offset: 0x001F14D0
		internal void ReadXSDSchema(XmlReader reader, bool denyResolving)
		{
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
			while (reader.LocalName == "schema" && reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
			{
				XmlSchema xmlSchema = XmlSchema.Read(reader, null);
				xmlSchemaSet.Add(xmlSchema);
				this.ReadEndElement(reader);
			}
			xmlSchemaSet.Compile();
			XSDSchema xsdschema = new XSDSchema();
			xsdschema.LoadSchema(xmlSchemaSet, this);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x001F2134 File Offset: 0x001F1534
		public void ReadXmlSchema(Stream stream)
		{
			if (stream == null)
			{
				return;
			}
			this.ReadXmlSchema(new XmlTextReader(stream), false);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x001F2154 File Offset: 0x001F1554
		public void ReadXmlSchema(TextReader reader)
		{
			if (reader == null)
			{
				return;
			}
			this.ReadXmlSchema(new XmlTextReader(reader), false);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x001F2174 File Offset: 0x001F1574
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

		// Token: 0x06000A51 RID: 2641 RVA: 0x001F21B8 File Offset: 0x001F15B8
		public void ReadXmlSchema(XmlReader reader)
		{
			this.ReadXmlSchema(reader, false);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x001F21D0 File Offset: 0x001F15D0
		internal void ReadXmlSchema(XmlReader reader, bool denyResolving)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataTable.ReadXmlSchema|INFO> %d#, denyResolving=%d{bool}\n", this.ObjectID, denyResolving);
			try
			{
				DataSet dataSet = new DataSet();
				SerializationFormat remotingFormat = this.RemotingFormat;
				dataSet.ReadXmlSchema(reader, denyResolving);
				string mainTableName = dataSet.MainTableName;
				if (!ADP.IsEmpty(this.tableName) || !ADP.IsEmpty(mainTableName))
				{
					DataTable dataTable = null;
					if (!ADP.IsEmpty(this.tableName))
					{
						if (!ADP.IsEmpty(this.Namespace))
						{
							dataTable = dataSet.Tables[this.tableName, this.Namespace];
						}
						else
						{
							int num = dataSet.Tables.InternalIndexOf(this.tableName);
							if (num > -1)
							{
								dataTable = dataSet.Tables[num];
							}
						}
					}
					else
					{
						string text = "";
						int num2 = mainTableName.IndexOf(':');
						if (num2 > -1)
						{
							text = mainTableName.Substring(0, num2);
						}
						string text2 = mainTableName.Substring(num2 + 1, mainTableName.Length - num2 - 1);
						dataTable = dataSet.Tables[text2, text];
					}
					if (dataTable == null)
					{
						string text3 = string.Empty;
						if (!ADP.IsEmpty(this.tableName))
						{
							text3 = ((this.Namespace.Length > 0) ? (this.Namespace + ":" + this.tableName) : this.tableName);
						}
						else
						{
							text3 = mainTableName;
						}
						throw ExceptionBuilder.TableNotFound(text3);
					}
					dataTable._remotingFormat = remotingFormat;
					List<DataTable> list = new List<DataTable>();
					list.Add(dataTable);
					this.CreateTableList(dataTable, list);
					List<DataRelation> list2 = new List<DataRelation>();
					this.CreateRelationList(list, list2);
					if (list2.Count == 0)
					{
						if (this.Columns.Count == 0)
						{
							DataTable dataTable2 = dataTable;
							if (dataTable2 != null)
							{
								dataTable2.CloneTo(this, null, false);
							}
							if (this.DataSet == null && this.tableNamespace == null)
							{
								this.tableNamespace = dataTable2.Namespace;
							}
						}
					}
					else
					{
						if (ADP.IsEmpty(this.TableName))
						{
							this.TableName = dataTable.TableName;
							if (!ADP.IsEmpty(dataTable.Namespace))
							{
								this.Namespace = dataTable.Namespace;
							}
						}
						if (this.DataSet == null)
						{
							DataSet dataSet2 = new DataSet(dataSet.DataSetName);
							dataSet2.SetLocaleValue(dataSet.Locale, dataSet.ShouldSerializeLocale());
							dataSet2.CaseSensitive = dataSet.CaseSensitive;
							dataSet2.Namespace = dataSet.Namespace;
							dataSet2.mainTableName = dataSet.mainTableName;
							dataSet2.RemotingFormat = dataSet.RemotingFormat;
							dataSet2.Tables.Add(this);
						}
						this.CloneHierarchy(dataTable, this.DataSet, null);
						foreach (DataTable dataTable3 in list)
						{
							DataTable dataTable4 = this.DataSet.Tables[dataTable3.tableName, dataTable3.Namespace];
							DataTable dataTable5 = dataSet.Tables[dataTable3.tableName, dataTable3.Namespace];
							foreach (object obj in dataTable5.Constraints)
							{
								Constraint constraint = (Constraint)obj;
								ForeignKeyConstraint foreignKeyConstraint = constraint as ForeignKeyConstraint;
								if (foreignKeyConstraint != null && foreignKeyConstraint.Table != foreignKeyConstraint.RelatedTable && list.Contains(foreignKeyConstraint.Table) && list.Contains(foreignKeyConstraint.RelatedTable))
								{
									ForeignKeyConstraint foreignKeyConstraint2 = (ForeignKeyConstraint)foreignKeyConstraint.Clone(dataTable4.DataSet);
									if (!dataTable4.Constraints.Contains(foreignKeyConstraint2.ConstraintName))
									{
										dataTable4.Constraints.Add(foreignKeyConstraint2);
									}
								}
							}
						}
						foreach (DataRelation dataRelation in list2)
						{
							if (!this.DataSet.Relations.Contains(dataRelation.RelationName))
							{
								this.DataSet.Relations.Add(dataRelation.Clone(this.DataSet));
							}
						}
						foreach (DataTable dataTable6 in list)
						{
							foreach (object obj2 in dataTable6.Columns)
							{
								DataColumn dataColumn = (DataColumn)obj2;
								bool flag = false;
								if (dataColumn.Expression.Length != 0)
								{
									DataColumn[] dependency = dataColumn.DataExpression.GetDependency();
									for (int i = 0; i < dependency.Length; i++)
									{
										if (!list.Contains(dependency[i].Table))
										{
											flag = true;
											break;
										}
									}
								}
								if (!flag)
								{
									this.DataSet.Tables[dataTable6.TableName, dataTable6.Namespace].Columns[dataColumn.ColumnName].Expression = dataColumn.Expression;
								}
							}
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x001F276C File Offset: 0x001F1B6C
		private void CreateTableList(DataTable currentTable, List<DataTable> tableList)
		{
			foreach (object obj in currentTable.ChildRelations)
			{
				DataRelation dataRelation = (DataRelation)obj;
				if (!tableList.Contains(dataRelation.ChildTable))
				{
					tableList.Add(dataRelation.ChildTable);
					this.CreateTableList(dataRelation.ChildTable, tableList);
				}
			}
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x001F27F4 File Offset: 0x001F1BF4
		private void CreateRelationList(List<DataTable> tableList, List<DataRelation> relationList)
		{
			foreach (DataTable dataTable in tableList)
			{
				foreach (object obj in dataTable.ChildRelations)
				{
					DataRelation dataRelation = (DataRelation)obj;
					if (tableList.Contains(dataRelation.ChildTable) && tableList.Contains(dataRelation.ParentTable))
					{
						relationList.Add(dataRelation);
					}
				}
			}
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x001F28B8 File Offset: 0x001F1CB8
		public static XmlSchemaComplexType GetDataTableSchema(XmlSchemaSet schemaSet)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
			xmlSchemaAny.Namespace = "http://www.w3.org/2001/XMLSchema";
			xmlSchemaAny.MinOccurs = 0m;
			xmlSchemaAny.MaxOccurs = decimal.MaxValue;
			xmlSchemaAny.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny);
			xmlSchemaAny = new XmlSchemaAny();
			xmlSchemaAny.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
			xmlSchemaAny.MinOccurs = 1m;
			xmlSchemaAny.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			return xmlSchemaComplexType;
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x001F294C File Offset: 0x001F1D4C
		XmlSchema IXmlSerializable.GetSchema()
		{
			return this.GetSchema();
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x001F2960 File Offset: 0x001F1D60
		protected virtual XmlSchema GetSchema()
		{
			if (base.GetType() == typeof(DataTable))
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

		// Token: 0x06000A58 RID: 2648 RVA: 0x001F29B4 File Offset: 0x001F1DB4
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			IXmlTextParser xmlTextParser = reader as IXmlTextParser;
			bool flag = true;
			if (xmlTextParser != null)
			{
				flag = xmlTextParser.Normalized;
				xmlTextParser.Normalized = false;
			}
			this.ReadXmlSerializable(reader);
			if (xmlTextParser != null)
			{
				xmlTextParser.Normalized = flag;
			}
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x001F29EC File Offset: 0x001F1DEC
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			this.WriteXmlSchema(writer, false);
			this.WriteXml(writer, XmlWriteMode.DiffGram, false);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x001F2A0C File Offset: 0x001F1E0C
		protected virtual void ReadXmlSerializable(XmlReader reader)
		{
			this.ReadXml(reader, XmlReadMode.DiffGram, true);
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x001F2A24 File Offset: 0x001F1E24
		internal Hashtable RowDiffId
		{
			get
			{
				if (this.rowDiffId == null)
				{
					this.rowDiffId = new Hashtable();
				}
				return this.rowDiffId;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x001F2A4C File Offset: 0x001F1E4C
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x001F2A60 File Offset: 0x001F1E60
		internal void AddDependentColumn(DataColumn expressionColumn)
		{
			if (this.dependentColumns == null)
			{
				this.dependentColumns = new List<DataColumn>();
			}
			if (!this.dependentColumns.Contains(expressionColumn))
			{
				this.dependentColumns.Add(expressionColumn);
			}
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x001F2A9C File Offset: 0x001F1E9C
		internal void RemoveDependentColumn(DataColumn expressionColumn)
		{
			if (this.dependentColumns != null && this.dependentColumns.Contains(expressionColumn))
			{
				this.dependentColumns.Remove(expressionColumn);
			}
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x001F2ACC File Offset: 0x001F1ECC
		internal void EvaluateExpressions()
		{
			if (this.dependentColumns != null && 0 < this.dependentColumns.Count)
			{
				foreach (object obj in this.Rows)
				{
					DataRow dataRow = (DataRow)obj;
					if (dataRow.oldRecord != -1 && dataRow.oldRecord != dataRow.newRecord)
					{
						this.EvaluateDependentExpressions(this.dependentColumns, dataRow, DataRowVersion.Original, null);
					}
					if (dataRow.newRecord != -1)
					{
						this.EvaluateDependentExpressions(this.dependentColumns, dataRow, DataRowVersion.Current, null);
					}
					if (dataRow.tempRecord != -1)
					{
						this.EvaluateDependentExpressions(this.dependentColumns, dataRow, DataRowVersion.Proposed, null);
					}
				}
			}
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x001F2BA8 File Offset: 0x001F1FA8
		internal void EvaluateExpressions(DataRow row, DataRowAction action, List<DataRow> cachedRows)
		{
			if (action == DataRowAction.Add || action == DataRowAction.Change || action == DataRowAction.Rollback)
			{
				if (row.oldRecord != -1 && row.oldRecord != row.newRecord)
				{
					this.EvaluateDependentExpressions(this.dependentColumns, row, DataRowVersion.Original, cachedRows);
				}
				if (row.newRecord != -1)
				{
					this.EvaluateDependentExpressions(this.dependentColumns, row, DataRowVersion.Current, cachedRows);
				}
				if (row.tempRecord != -1)
				{
					this.EvaluateDependentExpressions(this.dependentColumns, row, DataRowVersion.Proposed, cachedRows);
					return;
				}
			}
			else if (action == DataRowAction.Delete && this.dependentColumns != null)
			{
				foreach (DataColumn dataColumn in this.dependentColumns)
				{
					if (dataColumn.DataExpression != null && dataColumn.DataExpression.HasLocalAggregate() && dataColumn.Table == this)
					{
						for (int i = 0; i < this.Rows.Count; i++)
						{
							DataRow dataRow = this.Rows[i];
							if (dataRow.oldRecord != -1 && dataRow.oldRecord != dataRow.newRecord)
							{
								this.EvaluateDependentExpressions(this.dependentColumns, dataRow, DataRowVersion.Original, null);
							}
							if (dataRow.newRecord != -1)
							{
								this.EvaluateDependentExpressions(this.dependentColumns, dataRow, DataRowVersion.Current, null);
							}
							if (dataRow.tempRecord != -1)
							{
								this.EvaluateDependentExpressions(this.dependentColumns, dataRow, DataRowVersion.Proposed, null);
							}
						}
						break;
					}
				}
				foreach (DataRow dataRow2 in cachedRows)
				{
					if (dataRow2.oldRecord != -1 && dataRow2.oldRecord != dataRow2.newRecord)
					{
						dataRow2.Table.EvaluateDependentExpressions(dataRow2.Table.dependentColumns, dataRow2, DataRowVersion.Original, null);
					}
					if (dataRow2.newRecord != -1)
					{
						dataRow2.Table.EvaluateDependentExpressions(dataRow2.Table.dependentColumns, dataRow2, DataRowVersion.Current, null);
					}
					if (dataRow2.tempRecord != -1)
					{
						dataRow2.Table.EvaluateDependentExpressions(dataRow2.Table.dependentColumns, dataRow2, DataRowVersion.Proposed, null);
					}
				}
			}
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x001F2E08 File Offset: 0x001F2208
		internal void EvaluateExpressions(DataColumn column)
		{
			int count = column.table.Rows.Count;
			if (column.DataExpression.IsTableAggregate() && count > 0)
			{
				object obj = column.DataExpression.Evaluate();
				for (int i = 0; i < count; i++)
				{
					DataRow dataRow = column.table.Rows[i];
					if (dataRow.oldRecord != -1 && dataRow.oldRecord != dataRow.newRecord)
					{
						column[dataRow.oldRecord] = obj;
					}
					if (dataRow.newRecord != -1)
					{
						column[dataRow.newRecord] = obj;
					}
					if (dataRow.tempRecord != -1)
					{
						column[dataRow.tempRecord] = obj;
					}
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					DataRow dataRow2 = column.table.Rows[j];
					if (dataRow2.oldRecord != -1 && dataRow2.oldRecord != dataRow2.newRecord)
					{
						column[dataRow2.oldRecord] = column.DataExpression.Evaluate(dataRow2, DataRowVersion.Original);
					}
					if (dataRow2.newRecord != -1)
					{
						column[dataRow2.newRecord] = column.DataExpression.Evaluate(dataRow2, DataRowVersion.Current);
					}
					if (dataRow2.tempRecord != -1)
					{
						column[dataRow2.tempRecord] = column.DataExpression.Evaluate(dataRow2, DataRowVersion.Proposed);
					}
				}
			}
			column.Table.ResetInternalIndexes(column);
			this.EvaluateDependentExpressions(column);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x001F2F7C File Offset: 0x001F237C
		internal void EvaluateDependentExpressions(DataColumn column)
		{
			if (column.dependentColumns != null)
			{
				foreach (DataColumn dataColumn in column.dependentColumns)
				{
					if (dataColumn.table != null && !object.ReferenceEquals(column, dataColumn))
					{
						this.EvaluateExpressions(dataColumn);
					}
				}
			}
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x001F2FF4 File Offset: 0x001F23F4
		internal void EvaluateDependentExpressions(List<DataColumn> columns, DataRow row, DataRowVersion version, List<DataRow> cachedRows)
		{
			if (columns == null)
			{
				return;
			}
			int num = columns.Count;
			for (int i = 0; i < num; i++)
			{
				if (columns[i].Table == this)
				{
					DataColumn dataColumn = columns[i];
					if (dataColumn.DataExpression != null && dataColumn.DataExpression.HasLocalAggregate())
					{
						DataRowVersion dataRowVersion = ((version == DataRowVersion.Proposed) ? DataRowVersion.Default : version);
						bool flag = dataColumn.DataExpression.IsTableAggregate();
						object obj = null;
						if (flag)
						{
							obj = dataColumn.DataExpression.Evaluate(row, dataRowVersion);
						}
						for (int j = 0; j < this.Rows.Count; j++)
						{
							DataRow dataRow = this.Rows[j];
							if (dataRow.RowState != DataRowState.Deleted && (dataRowVersion != DataRowVersion.Original || (dataRow.oldRecord != -1 && dataRow.oldRecord != dataRow.newRecord)))
							{
								if (!flag)
								{
									obj = dataColumn.DataExpression.Evaluate(dataRow, dataRowVersion);
								}
								this.SilentlySetValue(dataRow, dataColumn, dataRowVersion, obj);
							}
						}
					}
					else if (row.RowState != DataRowState.Deleted && (version != DataRowVersion.Original || (row.oldRecord != -1 && row.oldRecord != row.newRecord)))
					{
						this.SilentlySetValue(row, dataColumn, version, (dataColumn.DataExpression == null) ? dataColumn.DefaultValue : dataColumn.DataExpression.Evaluate(row, version));
					}
				}
			}
			num = columns.Count;
			for (int k = 0; k < num; k++)
			{
				DataColumn dataColumn2 = columns[k];
				if (dataColumn2.Table != this || (dataColumn2.DataExpression != null && !dataColumn2.DataExpression.HasLocalAggregate()))
				{
					DataRowVersion dataRowVersion2 = ((version == DataRowVersion.Proposed) ? DataRowVersion.Default : version);
					if (cachedRows != null)
					{
						foreach (DataRow dataRow2 in cachedRows)
						{
							if (dataRow2.Table == dataColumn2.Table && (dataRowVersion2 != DataRowVersion.Original || dataRow2.newRecord != dataRow2.oldRecord) && dataRow2 != null && dataRow2.RowState != DataRowState.Deleted && (version != DataRowVersion.Original || dataRow2.oldRecord != -1))
							{
								object obj2 = dataColumn2.DataExpression.Evaluate(dataRow2, dataRowVersion2);
								this.SilentlySetValue(dataRow2, dataColumn2, dataRowVersion2, obj2);
							}
						}
					}
					for (int l = 0; l < this.ParentRelations.Count; l++)
					{
						DataRelation dataRelation = this.ParentRelations[l];
						if (dataRelation.ParentTable == dataColumn2.Table)
						{
							foreach (DataRow dataRow3 in row.GetParentRows(dataRelation, version))
							{
								if ((cachedRows == null || !cachedRows.Contains(dataRow3)) && (dataRowVersion2 != DataRowVersion.Original || dataRow3.newRecord != dataRow3.oldRecord) && dataRow3 != null && dataRow3.RowState != DataRowState.Deleted && (version != DataRowVersion.Original || dataRow3.oldRecord != -1))
								{
									object obj3 = dataColumn2.DataExpression.Evaluate(dataRow3, dataRowVersion2);
									this.SilentlySetValue(dataRow3, dataColumn2, dataRowVersion2, obj3);
								}
							}
						}
					}
					for (int n = 0; n < this.ChildRelations.Count; n++)
					{
						DataRelation dataRelation2 = this.ChildRelations[n];
						if (dataRelation2.ChildTable == dataColumn2.Table)
						{
							foreach (DataRow dataRow4 in row.GetChildRows(dataRelation2, version))
							{
								if ((cachedRows == null || !cachedRows.Contains(dataRow4)) && (dataRowVersion2 != DataRowVersion.Original || dataRow4.newRecord != dataRow4.oldRecord) && dataRow4 != null && dataRow4.RowState != DataRowState.Deleted && (version != DataRowVersion.Original || dataRow4.oldRecord != -1))
								{
									object obj4 = dataColumn2.DataExpression.Evaluate(dataRow4, dataRowVersion2);
									this.SilentlySetValue(dataRow4, dataColumn2, dataRowVersion2, obj4);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x040007B8 RID: 1976
		private const string KEY_XMLSCHEMA = "XmlSchema";

		// Token: 0x040007B9 RID: 1977
		private const string KEY_XMLDIFFGRAM = "XmlDiffGram";

		// Token: 0x040007BA RID: 1978
		private const string KEY_NAME = "TableName";

		// Token: 0x040007BB RID: 1979
		private DataSet dataSet;

		// Token: 0x040007BC RID: 1980
		private DataView defaultView;

		// Token: 0x040007BD RID: 1981
		internal long nextRowID;

		// Token: 0x040007BE RID: 1982
		internal readonly DataRowCollection rowCollection;

		// Token: 0x040007BF RID: 1983
		internal readonly DataColumnCollection columnCollection;

		// Token: 0x040007C0 RID: 1984
		private readonly ConstraintCollection constraintCollection;

		// Token: 0x040007C1 RID: 1985
		private int elementColumnCount;

		// Token: 0x040007C2 RID: 1986
		internal DataRelationCollection parentRelationsCollection;

		// Token: 0x040007C3 RID: 1987
		internal DataRelationCollection childRelationsCollection;

		// Token: 0x040007C4 RID: 1988
		internal readonly RecordManager recordManager;

		// Token: 0x040007C5 RID: 1989
		internal readonly List<Index> indexes;

		// Token: 0x040007C6 RID: 1990
		private List<Index> shadowIndexes;

		// Token: 0x040007C7 RID: 1991
		private int shadowCount;

		// Token: 0x040007C8 RID: 1992
		internal PropertyCollection extendedProperties;

		// Token: 0x040007C9 RID: 1993
		private string tableName = "";

		// Token: 0x040007CA RID: 1994
		internal string tableNamespace;

		// Token: 0x040007CB RID: 1995
		private string tablePrefix = "";

		// Token: 0x040007CC RID: 1996
		internal DataExpression displayExpression;

		// Token: 0x040007CD RID: 1997
		internal bool fNestedInDataset = true;

		// Token: 0x040007CE RID: 1998
		private CultureInfo _culture;

		// Token: 0x040007CF RID: 1999
		private bool _cultureUserSet;

		// Token: 0x040007D0 RID: 2000
		private CompareInfo _compareInfo;

		// Token: 0x040007D1 RID: 2001
		private CompareOptions _compareFlags = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;

		// Token: 0x040007D2 RID: 2002
		private IFormatProvider _formatProvider;

		// Token: 0x040007D3 RID: 2003
		private StringComparer _hashCodeProvider;

		// Token: 0x040007D4 RID: 2004
		private bool _caseSensitive;

		// Token: 0x040007D5 RID: 2005
		private bool _caseSensitiveUserSet;

		// Token: 0x040007D6 RID: 2006
		internal string encodedTableName;

		// Token: 0x040007D7 RID: 2007
		internal DataColumn xmlText;

		// Token: 0x040007D8 RID: 2008
		internal DataColumn _colUnique;

		// Token: 0x040007D9 RID: 2009
		internal bool textOnly;

		// Token: 0x040007DA RID: 2010
		internal decimal minOccurs = 1m;

		// Token: 0x040007DB RID: 2011
		internal decimal maxOccurs = 1m;

		// Token: 0x040007DC RID: 2012
		internal bool repeatableElement;

		// Token: 0x040007DD RID: 2013
		private object typeName;

		// Token: 0x040007DE RID: 2014
		private static readonly int[] zeroIntegers = new int[0];

		// Token: 0x040007DF RID: 2015
		internal static readonly DataColumn[] zeroColumns = new DataColumn[0];

		// Token: 0x040007E0 RID: 2016
		internal static readonly DataRow[] zeroRows = new DataRow[0];

		// Token: 0x040007E1 RID: 2017
		internal UniqueConstraint primaryKey;

		// Token: 0x040007E2 RID: 2018
		internal static readonly IndexField[] zeroIndexField = new IndexField[0];

		// Token: 0x040007E3 RID: 2019
		internal IndexField[] _primaryIndex = DataTable.zeroIndexField;

		// Token: 0x040007E4 RID: 2020
		private DataColumn[] delayedSetPrimaryKey;

		// Token: 0x040007E5 RID: 2021
		private Index loadIndex;

		// Token: 0x040007E6 RID: 2022
		private Index loadIndexwithOriginalAdded;

		// Token: 0x040007E7 RID: 2023
		private Index loadIndexwithCurrentDeleted;

		// Token: 0x040007E8 RID: 2024
		private int _suspendIndexEvents;

		// Token: 0x040007E9 RID: 2025
		private bool savedEnforceConstraints;

		// Token: 0x040007EA RID: 2026
		private bool inDataLoad;

		// Token: 0x040007EB RID: 2027
		private bool initialLoad;

		// Token: 0x040007EC RID: 2028
		private bool schemaLoading;

		// Token: 0x040007ED RID: 2029
		private bool enforceConstraints = true;

		// Token: 0x040007EE RID: 2030
		internal bool _suspendEnforceConstraints;

		// Token: 0x040007EF RID: 2031
		protected internal bool fInitInProgress;

		// Token: 0x040007F0 RID: 2032
		private bool inLoad;

		// Token: 0x040007F1 RID: 2033
		internal bool fInLoadDiffgram;

		// Token: 0x040007F2 RID: 2034
		private byte _isTypedDataTable;

		// Token: 0x040007F3 RID: 2035
		private DataRow[] EmptyDataRowArray;

		// Token: 0x040007F4 RID: 2036
		private PropertyDescriptorCollection propertyDescriptorCollectionCache;

		// Token: 0x040007F5 RID: 2037
		private static readonly DataRelation[] EmptyArrayDataRelation = new DataRelation[0];

		// Token: 0x040007F6 RID: 2038
		private DataRelation[] _nestedParentRelations = DataTable.EmptyArrayDataRelation;

		// Token: 0x040007F7 RID: 2039
		internal List<DataColumn> dependentColumns;

		// Token: 0x040007F8 RID: 2040
		private bool mergingData;

		// Token: 0x040007F9 RID: 2041
		private DataRowChangeEventHandler onRowChangedDelegate;

		// Token: 0x040007FA RID: 2042
		private DataRowChangeEventHandler onRowChangingDelegate;

		// Token: 0x040007FB RID: 2043
		private DataRowChangeEventHandler onRowDeletingDelegate;

		// Token: 0x040007FC RID: 2044
		private DataRowChangeEventHandler onRowDeletedDelegate;

		// Token: 0x040007FD RID: 2045
		private DataColumnChangeEventHandler onColumnChangedDelegate;

		// Token: 0x040007FE RID: 2046
		private DataColumnChangeEventHandler onColumnChangingDelegate;

		// Token: 0x040007FF RID: 2047
		private DataTableClearEventHandler onTableClearingDelegate;

		// Token: 0x04000800 RID: 2048
		private DataTableClearEventHandler onTableClearedDelegate;

		// Token: 0x04000801 RID: 2049
		private DataTableNewRowEventHandler onTableNewRowDelegate;

		// Token: 0x04000802 RID: 2050
		private PropertyChangedEventHandler onPropertyChangingDelegate;

		// Token: 0x04000803 RID: 2051
		private EventHandler onInitialized;

		// Token: 0x04000804 RID: 2052
		private readonly DataRowBuilder rowBuilder;

		// Token: 0x04000805 RID: 2053
		internal readonly List<DataView> delayedViews = new List<DataView>();

		// Token: 0x04000806 RID: 2054
		private readonly List<DataViewListener> _dataViewListeners = new List<DataViewListener>();

		// Token: 0x04000807 RID: 2055
		internal Hashtable rowDiffId;

		// Token: 0x04000808 RID: 2056
		internal readonly ReaderWriterLock indexesLock = new ReaderWriterLock();

		// Token: 0x04000809 RID: 2057
		internal int ukColumnPositionForInference = -1;

		// Token: 0x0400080A RID: 2058
		private SerializationFormat _remotingFormat;

		// Token: 0x0400080B RID: 2059
		private static int _objectTypeCount;

		// Token: 0x0400080C RID: 2060
		private readonly int _objectID = Interlocked.Increment(ref DataTable._objectTypeCount);
	}
}

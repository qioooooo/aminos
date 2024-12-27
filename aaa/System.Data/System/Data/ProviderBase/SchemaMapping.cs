using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Xml;

namespace System.Data.ProviderBase
{
	// Token: 0x0200027D RID: 637
	internal sealed class SchemaMapping
	{
		// Token: 0x06002182 RID: 8578 RVA: 0x0026802C File Offset: 0x0026742C
		internal SchemaMapping(DataAdapter adapter, DataSet dataset, DataTable datatable, DataReaderContainer dataReader, bool keyInfo, SchemaType schemaType, string sourceTableName, bool gettingData, DataColumn parentChapterColumn, object parentChapterValue)
		{
			this._dataSet = dataset;
			this._dataTable = datatable;
			this._adapter = adapter;
			this._dataReader = dataReader;
			if (keyInfo)
			{
				this._schemaTable = dataReader.GetSchemaTable();
			}
			if (adapter.ShouldSerializeFillLoadOption())
			{
				this._loadOption = adapter.FillLoadOption;
			}
			else if (adapter.AcceptChangesDuringFill)
			{
				this._loadOption = (LoadOption)4;
			}
			else
			{
				this._loadOption = (LoadOption)5;
			}
			MissingMappingAction missingMappingAction;
			MissingSchemaAction missingSchemaAction;
			if (SchemaType.Mapped == schemaType)
			{
				missingMappingAction = this._adapter.MissingMappingAction;
				missingSchemaAction = this._adapter.MissingSchemaAction;
				if (!ADP.IsEmpty(sourceTableName))
				{
					this._tableMapping = this._adapter.GetTableMappingBySchemaAction(sourceTableName, sourceTableName, missingMappingAction);
				}
				else if (this._dataTable != null)
				{
					int num = this._adapter.IndexOfDataSetTable(this._dataTable.TableName);
					if (-1 != num)
					{
						this._tableMapping = this._adapter.TableMappings[num];
					}
					else
					{
						switch (missingMappingAction)
						{
						case MissingMappingAction.Passthrough:
							this._tableMapping = new DataTableMapping(this._dataTable.TableName, this._dataTable.TableName);
							break;
						case MissingMappingAction.Ignore:
							this._tableMapping = null;
							break;
						case MissingMappingAction.Error:
							throw ADP.MissingTableMappingDestination(this._dataTable.TableName);
						default:
							throw ADP.InvalidMissingMappingAction(missingMappingAction);
						}
					}
				}
			}
			else
			{
				if (SchemaType.Source != schemaType)
				{
					throw ADP.InvalidSchemaType(schemaType);
				}
				missingMappingAction = MissingMappingAction.Passthrough;
				missingSchemaAction = MissingSchemaAction.Add;
				if (!ADP.IsEmpty(sourceTableName))
				{
					this._tableMapping = DataTableMappingCollection.GetTableMappingBySchemaAction(null, sourceTableName, sourceTableName, missingMappingAction);
				}
				else if (this._dataTable != null)
				{
					int num2 = this._adapter.IndexOfDataSetTable(this._dataTable.TableName);
					if (-1 != num2)
					{
						this._tableMapping = this._adapter.TableMappings[num2];
					}
					else
					{
						this._tableMapping = new DataTableMapping(this._dataTable.TableName, this._dataTable.TableName);
					}
				}
			}
			if (this._tableMapping != null)
			{
				if (this._dataTable == null)
				{
					this._dataTable = this._tableMapping.GetDataTableBySchemaAction(this._dataSet, missingSchemaAction);
				}
				if (this._dataTable != null)
				{
					this._fieldNames = SchemaMapping.GenerateFieldNames(dataReader);
					if (this._schemaTable == null)
					{
						this._readerDataValues = this.SetupSchemaWithoutKeyInfo(missingMappingAction, missingSchemaAction, gettingData, parentChapterColumn, parentChapterValue);
						return;
					}
					this._readerDataValues = this.SetupSchemaWithKeyInfo(missingMappingAction, missingSchemaAction, gettingData, parentChapterColumn, parentChapterValue);
				}
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06002183 RID: 8579 RVA: 0x00268284 File Offset: 0x00267684
		internal DataReaderContainer DataReader
		{
			get
			{
				return this._dataReader;
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x00268298 File Offset: 0x00267698
		internal DataTable DataTable
		{
			get
			{
				return this._dataTable;
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06002185 RID: 8581 RVA: 0x002682AC File Offset: 0x002676AC
		internal object[] DataValues
		{
			get
			{
				return this._readerDataValues;
			}
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x002682C0 File Offset: 0x002676C0
		internal void ApplyToDataRow(DataRow dataRow)
		{
			DataColumnCollection columns = dataRow.Table.Columns;
			this._dataReader.GetValues(this._readerDataValues);
			object[] mappedValues = this.GetMappedValues();
			bool[] array = new bool[mappedValues.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = columns[i].ReadOnly;
			}
			try
			{
				try
				{
					for (int j = 0; j < array.Length; j++)
					{
						if (columns[j].Expression.Length == 0)
						{
							columns[j].ReadOnly = false;
						}
					}
					for (int k = 0; k < mappedValues.Length; k++)
					{
						if (mappedValues[k] != null)
						{
							dataRow[k] = mappedValues[k];
						}
					}
				}
				finally
				{
					for (int l = 0; l < array.Length; l++)
					{
						if (columns[l].Expression.Length == 0)
						{
							columns[l].ReadOnly = array[l];
						}
					}
				}
			}
			finally
			{
				if (this._chapterMap != null)
				{
					this.FreeDataRowChapters();
				}
			}
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x002683EC File Offset: 0x002677EC
		private void MappedChapterIndex()
		{
			int mappedLength = this._mappedLength;
			for (int i = 0; i < mappedLength; i++)
			{
				int num = this._indexMap[i];
				if (0 <= num)
				{
					this._mappedDataValues[num] = this._readerDataValues[i];
					if (this._chapterMap[i])
					{
						this._mappedDataValues[num] = null;
					}
				}
			}
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x0026843C File Offset: 0x0026783C
		private void MappedChapter()
		{
			int mappedLength = this._mappedLength;
			for (int i = 0; i < mappedLength; i++)
			{
				this._mappedDataValues[i] = this._readerDataValues[i];
				if (this._chapterMap[i])
				{
					this._mappedDataValues[i] = null;
				}
			}
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x00268480 File Offset: 0x00267880
		private void MappedIndex()
		{
			int mappedLength = this._mappedLength;
			for (int i = 0; i < mappedLength; i++)
			{
				int num = this._indexMap[i];
				if (0 <= num)
				{
					this._mappedDataValues[num] = this._readerDataValues[i];
				}
			}
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x002684C0 File Offset: 0x002678C0
		private void MappedValues()
		{
			int mappedLength = this._mappedLength;
			for (int i = 0; i < mappedLength; i++)
			{
				this._mappedDataValues[i] = this._readerDataValues[i];
			}
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x002684F0 File Offset: 0x002678F0
		private object[] GetMappedValues()
		{
			if (this._xmlMap != null)
			{
				for (int i = 0; i < this._xmlMap.Length; i++)
				{
					if (this._xmlMap[i] != 0)
					{
						string text = this._readerDataValues[i] as string;
						if (text == null && this._readerDataValues[i] is SqlString)
						{
							SqlString sqlString = (SqlString)this._readerDataValues[i];
							if (!sqlString.IsNull)
							{
								text = sqlString.Value;
							}
							else
							{
								int num = this._xmlMap[i];
								if (num == 1)
								{
									this._readerDataValues[i] = global::System.Data.SqlTypes.SqlXml.Null;
								}
								else
								{
									this._readerDataValues[i] = DBNull.Value;
								}
							}
						}
						if (text != null)
						{
							switch (this._xmlMap[i])
							{
							case 1:
							{
								XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
								xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
								XmlReader xmlReader = XmlReader.Create(new StringReader(text), xmlReaderSettings, null);
								this._readerDataValues[i] = new SqlXml(xmlReader);
								break;
							}
							case 2:
							{
								XmlDocument xmlDocument = new XmlDocument();
								xmlDocument.LoadXml(text);
								this._readerDataValues[i] = xmlDocument;
								break;
							}
							}
						}
					}
				}
			}
			switch (this._mappedMode)
			{
			default:
				return this._readerDataValues;
			case 1:
				this.MappedValues();
				break;
			case 2:
				this.MappedIndex();
				break;
			case 3:
				this.MappedChapter();
				break;
			case 4:
				this.MappedChapterIndex();
				break;
			}
			return this._mappedDataValues;
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x0026864C File Offset: 0x00267A4C
		internal void LoadDataRowWithClear()
		{
			for (int i = 0; i < this._readerDataValues.Length; i++)
			{
				this._readerDataValues[i] = null;
			}
			this.LoadDataRow();
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x0026867C File Offset: 0x00267A7C
		internal void LoadDataRow()
		{
			try
			{
				this._dataReader.GetValues(this._readerDataValues);
				object[] mappedValues = this.GetMappedValues();
				DataRow dataRow;
				switch (this._loadOption)
				{
				case LoadOption.OverwriteChanges:
				case LoadOption.PreserveChanges:
				case LoadOption.Upsert:
					dataRow = this._dataTable.LoadDataRow(mappedValues, this._loadOption);
					break;
				case (LoadOption)4:
					dataRow = this._dataTable.LoadDataRow(mappedValues, true);
					break;
				case (LoadOption)5:
					dataRow = this._dataTable.LoadDataRow(mappedValues, false);
					break;
				default:
					throw ADP.InvalidLoadOption(this._loadOption);
				}
				if (this._chapterMap != null && this._dataSet != null)
				{
					this.LoadDataRowChapters(dataRow);
				}
			}
			finally
			{
				if (this._chapterMap != null)
				{
					this.FreeDataRowChapters();
				}
			}
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x00268750 File Offset: 0x00267B50
		private void FreeDataRowChapters()
		{
			for (int i = 0; i < this._chapterMap.Length; i++)
			{
				if (this._chapterMap[i])
				{
					IDisposable disposable = this._readerDataValues[i] as IDisposable;
					if (disposable != null)
					{
						this._readerDataValues[i] = null;
						disposable.Dispose();
					}
				}
			}
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x0026879C File Offset: 0x00267B9C
		internal int LoadDataRowChapters(DataRow dataRow)
		{
			int num = 0;
			int num2 = this._chapterMap.Length;
			for (int i = 0; i < num2; i++)
			{
				if (this._chapterMap[i])
				{
					object obj = this._readerDataValues[i];
					if (obj != null && !Convert.IsDBNull(obj))
					{
						this._readerDataValues[i] = null;
						using (IDataReader dataReader = (IDataReader)obj)
						{
							if (!dataReader.IsClosed)
							{
								DataColumn dataColumn;
								object obj2;
								if (this._indexMap == null)
								{
									dataColumn = this._dataTable.Columns[i];
									obj2 = dataRow[dataColumn];
								}
								else
								{
									dataColumn = this._dataTable.Columns[this._indexMap[i]];
									obj2 = dataRow[dataColumn];
								}
								string text = this._tableMapping.SourceTable + this._fieldNames[i];
								DataReaderContainer dataReaderContainer = DataReaderContainer.Create(dataReader, this._dataReader.ReturnProviderSpecificTypes);
								num += this._adapter.FillFromReader(this._dataSet, null, text, dataReaderContainer, 0, 0, dataColumn, obj2);
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x002688CC File Offset: 0x00267CCC
		private int[] CreateIndexMap(int count, int index)
		{
			int[] array = new int[count];
			for (int i = 0; i < index; i++)
			{
				array[i] = i;
			}
			return array;
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x002688F4 File Offset: 0x00267CF4
		private static string[] GenerateFieldNames(DataReaderContainer dataReader)
		{
			string[] array = new string[dataReader.FieldCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = dataReader.GetName(i);
			}
			ADP.BuildSchemaTableInfoTableNames(array);
			return array;
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x0026892C File Offset: 0x00267D2C
		private DataColumn[] ResizeColumnArray(DataColumn[] rgcol, int len)
		{
			DataColumn[] array = new DataColumn[len];
			Array.Copy(rgcol, array, len);
			return array;
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x0026894C File Offset: 0x00267D4C
		private void AddItemToAllowRollback(ref List<object> items, object value)
		{
			if (items == null)
			{
				items = new List<object>();
			}
			items.Add(value);
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x0026896C File Offset: 0x00267D6C
		private void RollbackAddedItems(List<object> items)
		{
			if (items != null)
			{
				int num = items.Count - 1;
				while (0 <= num)
				{
					if (items[num] != null)
					{
						DataColumn dataColumn = items[num] as DataColumn;
						if (dataColumn != null)
						{
							if (dataColumn.Table != null)
							{
								dataColumn.Table.Columns.Remove(dataColumn);
							}
						}
						else
						{
							DataTable dataTable = items[num] as DataTable;
							if (dataTable != null && dataTable.DataSet != null)
							{
								dataTable.DataSet.Tables.Remove(dataTable);
							}
						}
					}
					num--;
				}
			}
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x002689EC File Offset: 0x00267DEC
		private object[] SetupSchemaWithoutKeyInfo(MissingMappingAction mappingAction, MissingSchemaAction schemaAction, bool gettingData, DataColumn parentChapterColumn, object chapterValue)
		{
			int[] array = null;
			bool[] array2 = null;
			int num = 0;
			int fieldCount = this._dataReader.FieldCount;
			object[] array3 = null;
			List<object> list = null;
			try
			{
				DataColumnCollection dataColumnCollection = this._dataTable.Columns;
				for (int i = 0; i < fieldCount; i++)
				{
					bool flag = false;
					Type type = this._dataReader.GetFieldType(i);
					if (type == null)
					{
						throw ADP.MissingDataReaderFieldType(i);
					}
					if (typeof(IDataReader).IsAssignableFrom(type))
					{
						if (array2 == null)
						{
							array2 = new bool[fieldCount];
						}
						flag = (array2[i] = true);
						type = typeof(int);
					}
					else if (typeof(SqlXml).IsAssignableFrom(type))
					{
						if (this._xmlMap == null)
						{
							this._xmlMap = new int[fieldCount];
						}
						this._xmlMap[i] = 1;
					}
					else if (typeof(XmlReader).IsAssignableFrom(type))
					{
						type = typeof(string);
						if (this._xmlMap == null)
						{
							this._xmlMap = new int[fieldCount];
						}
						this._xmlMap[i] = 2;
					}
					DataColumn dataColumn = this._tableMapping.GetDataColumn(this._fieldNames[i], type, this._dataTable, mappingAction, schemaAction);
					if (dataColumn == null)
					{
						if (array == null)
						{
							array = this.CreateIndexMap(fieldCount, i);
						}
						array[i] = -1;
					}
					else
					{
						if (this._xmlMap != null && this._xmlMap[i] != 0)
						{
							if (typeof(SqlXml) == dataColumn.DataType)
							{
								this._xmlMap[i] = 1;
							}
							else if (typeof(XmlDocument) == dataColumn.DataType)
							{
								this._xmlMap[i] = 2;
							}
							else
							{
								this._xmlMap[i] = 0;
								int num2 = 0;
								for (int j = 0; j < this._xmlMap.Length; j++)
								{
									num2 += this._xmlMap[j];
								}
								if (num2 == 0)
								{
									this._xmlMap = null;
								}
							}
						}
						if (dataColumn.Table == null)
						{
							if (flag)
							{
								dataColumn.AllowDBNull = false;
								dataColumn.AutoIncrement = true;
								dataColumn.ReadOnly = true;
							}
							this.AddItemToAllowRollback(ref list, dataColumn);
							dataColumnCollection.Add(dataColumn);
						}
						else if (flag && !dataColumn.AutoIncrement)
						{
							throw ADP.FillChapterAutoIncrement();
						}
						if (array != null)
						{
							array[i] = dataColumn.Ordinal;
						}
						else if (i != dataColumn.Ordinal)
						{
							array = this.CreateIndexMap(fieldCount, i);
							array[i] = dataColumn.Ordinal;
						}
						num++;
					}
				}
				bool flag2 = false;
				DataColumn dataColumn2 = null;
				if (chapterValue != null)
				{
					Type type2 = chapterValue.GetType();
					dataColumn2 = this._tableMapping.GetDataColumn(this._tableMapping.SourceTable, type2, this._dataTable, mappingAction, schemaAction);
					if (dataColumn2 != null)
					{
						if (dataColumn2.Table == null)
						{
							this.AddItemToAllowRollback(ref list, dataColumn2);
							dataColumnCollection.Add(dataColumn2);
							flag2 = null != parentChapterColumn;
						}
						num++;
					}
				}
				if (0 < num)
				{
					if (this._dataSet != null && this._dataTable.DataSet == null)
					{
						this.AddItemToAllowRollback(ref list, this._dataTable);
						this._dataSet.Tables.Add(this._dataTable);
					}
					if (gettingData)
					{
						if (dataColumnCollection == null)
						{
							dataColumnCollection = this._dataTable.Columns;
						}
						this._indexMap = array;
						this._chapterMap = array2;
						array3 = this.SetupMapping(fieldCount, dataColumnCollection, dataColumn2, chapterValue);
					}
					else
					{
						this._mappedMode = -1;
					}
				}
				else
				{
					this._dataTable = null;
				}
				if (flag2)
				{
					this.AddRelation(parentChapterColumn, dataColumn2);
				}
			}
			catch (Exception ex)
			{
				if (ADP.IsCatchableOrSecurityExceptionType(ex))
				{
					this.RollbackAddedItems(list);
				}
				throw;
			}
			return array3;
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x00268D5C File Offset: 0x0026815C
		private object[] SetupSchemaWithKeyInfo(MissingMappingAction mappingAction, MissingSchemaAction schemaAction, bool gettingData, DataColumn parentChapterColumn, object chapterValue)
		{
			DbSchemaRow[] sortedSchemaRows = DbSchemaRow.GetSortedSchemaRows(this._schemaTable, this._dataReader.ReturnProviderSpecificTypes);
			if (sortedSchemaRows.Length == 0)
			{
				this._dataTable = null;
				return null;
			}
			bool flag = (this._dataTable.PrimaryKey.Length == 0 && ((LoadOption)4 <= this._loadOption || this._dataTable.Rows.Count == 0)) || 0 == this._dataTable.Columns.Count;
			DataColumn[] array = null;
			int num = 0;
			bool flag2 = true;
			string text = null;
			string text2 = null;
			bool flag3 = false;
			bool flag4 = false;
			int[] array2 = null;
			bool[] array3 = null;
			int num2 = 0;
			object[] array4 = null;
			List<object> list = null;
			DataColumnCollection columns = this._dataTable.Columns;
			try
			{
				for (int i = 0; i < sortedSchemaRows.Length; i++)
				{
					DbSchemaRow dbSchemaRow = sortedSchemaRows[i];
					int unsortedIndex = dbSchemaRow.UnsortedIndex;
					bool flag5 = false;
					Type type = dbSchemaRow.DataType;
					if (type == null)
					{
						type = this._dataReader.GetFieldType(i);
					}
					if (type == null)
					{
						throw ADP.MissingDataReaderFieldType(i);
					}
					if (typeof(IDataReader).IsAssignableFrom(type))
					{
						if (array3 == null)
						{
							array3 = new bool[sortedSchemaRows.Length];
						}
						flag5 = (array3[unsortedIndex] = true);
						type = typeof(int);
					}
					else if (typeof(SqlXml).IsAssignableFrom(type))
					{
						if (this._xmlMap == null)
						{
							this._xmlMap = new int[sortedSchemaRows.Length];
						}
						this._xmlMap[i] = 1;
					}
					else if (typeof(XmlReader).IsAssignableFrom(type))
					{
						type = typeof(string);
						if (this._xmlMap == null)
						{
							this._xmlMap = new int[sortedSchemaRows.Length];
						}
						this._xmlMap[i] = 2;
					}
					DataColumn dataColumn = null;
					if (!dbSchemaRow.IsHidden)
					{
						dataColumn = this._tableMapping.GetDataColumn(this._fieldNames[i], type, this._dataTable, mappingAction, schemaAction);
					}
					string baseTableName = dbSchemaRow.BaseTableName;
					if (dataColumn == null)
					{
						if (array2 == null)
						{
							array2 = this.CreateIndexMap(sortedSchemaRows.Length, unsortedIndex);
						}
						array2[unsortedIndex] = -1;
						if (dbSchemaRow.IsKey && (flag3 || dbSchemaRow.BaseTableName == text))
						{
							flag = false;
							array = null;
						}
					}
					else
					{
						if (this._xmlMap != null && this._xmlMap[i] != 0)
						{
							if (typeof(SqlXml) == dataColumn.DataType)
							{
								this._xmlMap[i] = 1;
							}
							else if (typeof(XmlDocument) == dataColumn.DataType)
							{
								this._xmlMap[i] = 2;
							}
							else
							{
								this._xmlMap[i] = 0;
								int num3 = 0;
								for (int j = 0; j < this._xmlMap.Length; j++)
								{
									num3 += this._xmlMap[j];
								}
								if (num3 == 0)
								{
									this._xmlMap = null;
								}
							}
						}
						if (dbSchemaRow.IsKey && baseTableName != text)
						{
							if (text == null)
							{
								text = baseTableName;
							}
							else
							{
								flag3 = true;
							}
						}
						if (flag5)
						{
							if (dataColumn.Table == null)
							{
								dataColumn.AllowDBNull = false;
								dataColumn.AutoIncrement = true;
								dataColumn.ReadOnly = true;
							}
							else if (!dataColumn.AutoIncrement)
							{
								throw ADP.FillChapterAutoIncrement();
							}
						}
						else
						{
							if (!flag4 && baseTableName != text2 && !ADP.IsEmpty(baseTableName))
							{
								if (text2 == null)
								{
									text2 = baseTableName;
								}
								else
								{
									flag4 = true;
								}
							}
							if ((LoadOption)4 <= this._loadOption)
							{
								if (dbSchemaRow.IsAutoIncrement && DataColumn.IsAutoIncrementType(type))
								{
									dataColumn.AutoIncrement = true;
									if (!dbSchemaRow.AllowDBNull)
									{
										dataColumn.AllowDBNull = false;
									}
								}
								if (type == typeof(string))
								{
									dataColumn.MaxLength = ((dbSchemaRow.Size > 0) ? dbSchemaRow.Size : (-1));
								}
								if (dbSchemaRow.IsReadOnly)
								{
									dataColumn.ReadOnly = true;
								}
								if (!dbSchemaRow.AllowDBNull && (!dbSchemaRow.IsReadOnly || dbSchemaRow.IsKey))
								{
									dataColumn.AllowDBNull = false;
								}
								if (dbSchemaRow.IsUnique && !dbSchemaRow.IsKey && !type.IsArray)
								{
									dataColumn.Unique = true;
									if (!dbSchemaRow.AllowDBNull)
									{
										dataColumn.AllowDBNull = false;
									}
								}
							}
							else if (dataColumn.Table == null)
							{
								dataColumn.AutoIncrement = dbSchemaRow.IsAutoIncrement;
								dataColumn.AllowDBNull = dbSchemaRow.AllowDBNull;
								dataColumn.ReadOnly = dbSchemaRow.IsReadOnly;
								dataColumn.Unique = dbSchemaRow.IsUnique;
								if (type == typeof(string) || type == typeof(SqlString))
								{
									dataColumn.MaxLength = dbSchemaRow.Size;
								}
							}
						}
						if (dataColumn.Table == null)
						{
							if ((LoadOption)4 > this._loadOption)
							{
								this.AddAdditionalProperties(dataColumn, dbSchemaRow.DataRow);
							}
							this.AddItemToAllowRollback(ref list, dataColumn);
							columns.Add(dataColumn);
						}
						if (flag && dbSchemaRow.IsKey)
						{
							if (array == null)
							{
								array = new DataColumn[sortedSchemaRows.Length];
							}
							array[num++] = dataColumn;
							if (flag2 && dataColumn.AllowDBNull)
							{
								flag2 = false;
							}
						}
						if (array2 != null)
						{
							array2[unsortedIndex] = dataColumn.Ordinal;
						}
						else if (unsortedIndex != dataColumn.Ordinal)
						{
							array2 = this.CreateIndexMap(sortedSchemaRows.Length, unsortedIndex);
							array2[unsortedIndex] = dataColumn.Ordinal;
						}
						num2++;
					}
				}
				bool flag6 = false;
				DataColumn dataColumn2 = null;
				if (chapterValue != null)
				{
					Type type2 = chapterValue.GetType();
					dataColumn2 = this._tableMapping.GetDataColumn(this._tableMapping.SourceTable, type2, this._dataTable, mappingAction, schemaAction);
					if (dataColumn2 != null)
					{
						if (dataColumn2.Table == null)
						{
							dataColumn2.ReadOnly = true;
							dataColumn2.AllowDBNull = false;
							this.AddItemToAllowRollback(ref list, dataColumn2);
							columns.Add(dataColumn2);
							flag6 = null != parentChapterColumn;
						}
						num2++;
					}
				}
				if (0 < num2)
				{
					if (this._dataSet != null && this._dataTable.DataSet == null)
					{
						this.AddItemToAllowRollback(ref list, this._dataTable);
						this._dataSet.Tables.Add(this._dataTable);
					}
					if (flag && array != null)
					{
						if (num < array.Length)
						{
							array = this.ResizeColumnArray(array, num);
						}
						if (flag2)
						{
							this._dataTable.PrimaryKey = array;
						}
						else
						{
							UniqueConstraint uniqueConstraint = new UniqueConstraint("", array);
							ConstraintCollection constraints = this._dataTable.Constraints;
							int count = constraints.Count;
							for (int k = 0; k < count; k++)
							{
								if (uniqueConstraint.Equals(constraints[k]))
								{
									uniqueConstraint = null;
									break;
								}
							}
							if (uniqueConstraint != null)
							{
								constraints.Add(uniqueConstraint);
							}
						}
					}
					if (!flag4 && !ADP.IsEmpty(text2) && ADP.IsEmpty(this._dataTable.TableName))
					{
						this._dataTable.TableName = text2;
					}
					if (gettingData)
					{
						this._indexMap = array2;
						this._chapterMap = array3;
						array4 = this.SetupMapping(sortedSchemaRows.Length, columns, dataColumn2, chapterValue);
					}
					else
					{
						this._mappedMode = -1;
					}
				}
				else
				{
					this._dataTable = null;
				}
				if (flag6)
				{
					this.AddRelation(parentChapterColumn, dataColumn2);
				}
			}
			catch (Exception ex)
			{
				if (ADP.IsCatchableOrSecurityExceptionType(ex))
				{
					this.RollbackAddedItems(list);
				}
				throw;
			}
			return array4;
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x00269418 File Offset: 0x00268818
		private void AddAdditionalProperties(DataColumn targetColumn, DataRow schemaRow)
		{
			DataColumnCollection columns = schemaRow.Table.Columns;
			DataColumn dataColumn = columns[SchemaTableOptionalColumn.DefaultValue];
			if (dataColumn != null)
			{
				targetColumn.DefaultValue = schemaRow[dataColumn];
			}
			dataColumn = columns[SchemaTableOptionalColumn.AutoIncrementSeed];
			if (dataColumn != null)
			{
				object obj = schemaRow[dataColumn];
				if (DBNull.Value != obj)
				{
					targetColumn.AutoIncrementSeed = ((IConvertible)obj).ToInt64(CultureInfo.InvariantCulture);
				}
			}
			dataColumn = columns[SchemaTableOptionalColumn.AutoIncrementStep];
			if (dataColumn != null)
			{
				object obj2 = schemaRow[dataColumn];
				if (DBNull.Value != obj2)
				{
					targetColumn.AutoIncrementStep = ((IConvertible)obj2).ToInt64(CultureInfo.InvariantCulture);
				}
			}
			dataColumn = columns[SchemaTableOptionalColumn.ColumnMapping];
			if (dataColumn != null)
			{
				object obj3 = schemaRow[dataColumn];
				if (DBNull.Value != obj3)
				{
					targetColumn.ColumnMapping = (MappingType)((IConvertible)obj3).ToInt32(CultureInfo.InvariantCulture);
				}
			}
			dataColumn = columns[SchemaTableOptionalColumn.BaseColumnNamespace];
			if (dataColumn != null)
			{
				object obj4 = schemaRow[dataColumn];
				if (DBNull.Value != obj4)
				{
					targetColumn.Namespace = ((IConvertible)obj4).ToString(CultureInfo.InvariantCulture);
				}
			}
			dataColumn = columns[SchemaTableOptionalColumn.Expression];
			if (dataColumn != null)
			{
				object obj5 = schemaRow[dataColumn];
				if (DBNull.Value != obj5)
				{
					targetColumn.Expression = ((IConvertible)obj5).ToString(CultureInfo.InvariantCulture);
				}
			}
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x00269560 File Offset: 0x00268960
		private void AddRelation(DataColumn parentChapterColumn, DataColumn chapterColumn)
		{
			if (this._dataSet != null)
			{
				string columnName = chapterColumn.ColumnName;
				DataRelation dataRelation = new DataRelation(columnName, new DataColumn[] { parentChapterColumn }, new DataColumn[] { chapterColumn }, false);
				int num = 1;
				string text = columnName;
				DataRelationCollection relations = this._dataSet.Relations;
				while (-1 != relations.IndexOf(text))
				{
					text = columnName + num;
					num++;
				}
				dataRelation.RelationName = text;
				relations.Add(dataRelation);
			}
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x002695E0 File Offset: 0x002689E0
		private object[] SetupMapping(int count, DataColumnCollection columnCollection, DataColumn chapterColumn, object chapterValue)
		{
			object[] array = new object[count];
			if (this._indexMap == null)
			{
				int count2 = columnCollection.Count;
				bool flag = null != this._chapterMap;
				if (count != count2 || flag)
				{
					this._mappedDataValues = new object[count2];
					if (flag)
					{
						this._mappedMode = 3;
						this._mappedLength = count;
					}
					else
					{
						this._mappedMode = 1;
						this._mappedLength = Math.Min(count, count2);
					}
				}
				else
				{
					this._mappedMode = 0;
				}
			}
			else
			{
				this._mappedDataValues = new object[columnCollection.Count];
				this._mappedMode = ((this._chapterMap == null) ? 2 : 4);
				this._mappedLength = count;
			}
			if (chapterColumn != null)
			{
				this._mappedDataValues[chapterColumn.Ordinal] = chapterValue;
			}
			return array;
		}

		// Token: 0x040015EB RID: 5611
		private const int MapExactMatch = 0;

		// Token: 0x040015EC RID: 5612
		private const int MapDifferentSize = 1;

		// Token: 0x040015ED RID: 5613
		private const int MapReorderedValues = 2;

		// Token: 0x040015EE RID: 5614
		private const int MapChapters = 3;

		// Token: 0x040015EF RID: 5615
		private const int MapChaptersReordered = 4;

		// Token: 0x040015F0 RID: 5616
		private const int SqlXml = 1;

		// Token: 0x040015F1 RID: 5617
		private const int XmlDocument = 2;

		// Token: 0x040015F2 RID: 5618
		private readonly DataSet _dataSet;

		// Token: 0x040015F3 RID: 5619
		private DataTable _dataTable;

		// Token: 0x040015F4 RID: 5620
		private readonly DataAdapter _adapter;

		// Token: 0x040015F5 RID: 5621
		private readonly DataReaderContainer _dataReader;

		// Token: 0x040015F6 RID: 5622
		private readonly DataTable _schemaTable;

		// Token: 0x040015F7 RID: 5623
		private readonly DataTableMapping _tableMapping;

		// Token: 0x040015F8 RID: 5624
		private readonly string[] _fieldNames;

		// Token: 0x040015F9 RID: 5625
		private readonly object[] _readerDataValues;

		// Token: 0x040015FA RID: 5626
		private object[] _mappedDataValues;

		// Token: 0x040015FB RID: 5627
		private int[] _indexMap;

		// Token: 0x040015FC RID: 5628
		private bool[] _chapterMap;

		// Token: 0x040015FD RID: 5629
		private int[] _xmlMap;

		// Token: 0x040015FE RID: 5630
		private int _mappedMode;

		// Token: 0x040015FF RID: 5631
		private int _mappedLength;

		// Token: 0x04001600 RID: 5632
		private readonly LoadOption _loadOption;
	}
}

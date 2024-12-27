using System;
using System.Collections;
using System.Xml;

namespace System.Data
{
	// Token: 0x020000FE RID: 254
	internal sealed class XmlToDatasetMap
	{
		// Token: 0x06000EE3 RID: 3811 RVA: 0x0021024C File Offset: 0x0020F64C
		public XmlToDatasetMap(DataSet dataSet, XmlNameTable nameTable)
		{
			this.BuildIdentityMap(dataSet, nameTable);
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00210268 File Offset: 0x0020F668
		public XmlToDatasetMap(XmlNameTable nameTable, DataSet dataSet)
		{
			this.BuildIdentityMap(nameTable, dataSet);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00210284 File Offset: 0x0020F684
		public XmlToDatasetMap(DataTable dataTable, XmlNameTable nameTable)
		{
			this.BuildIdentityMap(dataTable, nameTable);
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x002102A0 File Offset: 0x0020F6A0
		public XmlToDatasetMap(XmlNameTable nameTable, DataTable dataTable)
		{
			this.BuildIdentityMap(nameTable, dataTable);
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x002102BC File Offset: 0x0020F6BC
		internal static bool IsMappedColumn(DataColumn c)
		{
			return c.ColumnMapping != MappingType.Hidden;
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x002102D8 File Offset: 0x0020F6D8
		private XmlToDatasetMap.TableSchemaInfo AddTableSchema(DataTable table, XmlNameTable nameTable)
		{
			string text = nameTable.Get(table.EncodedTableName);
			string text2 = nameTable.Get(table.Namespace);
			if (text == null)
			{
				return null;
			}
			XmlToDatasetMap.TableSchemaInfo tableSchemaInfo = new XmlToDatasetMap.TableSchemaInfo(table);
			this.tableSchemaMap[new XmlToDatasetMap.XmlNodeIdentety(text, text2)] = tableSchemaInfo;
			return tableSchemaInfo;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00210320 File Offset: 0x0020F720
		private XmlToDatasetMap.TableSchemaInfo AddTableSchema(XmlNameTable nameTable, DataTable table)
		{
			string encodedTableName = table.EncodedTableName;
			string text = nameTable.Get(encodedTableName);
			if (text == null)
			{
				text = nameTable.Add(encodedTableName);
			}
			table.encodedTableName = text;
			string text2 = nameTable.Get(table.Namespace);
			if (text2 == null)
			{
				text2 = nameTable.Add(table.Namespace);
			}
			else if (table.tableNamespace != null)
			{
				table.tableNamespace = text2;
			}
			XmlToDatasetMap.TableSchemaInfo tableSchemaInfo = new XmlToDatasetMap.TableSchemaInfo(table);
			this.tableSchemaMap[new XmlToDatasetMap.XmlNodeIdentety(text, text2)] = tableSchemaInfo;
			return tableSchemaInfo;
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00210398 File Offset: 0x0020F798
		private bool AddColumnSchema(DataColumn col, XmlNameTable nameTable, XmlToDatasetMap.XmlNodeIdHashtable columns)
		{
			string text = nameTable.Get(col.EncodedColumnName);
			string text2 = nameTable.Get(col.Namespace);
			if (text == null)
			{
				return false;
			}
			XmlToDatasetMap.XmlNodeIdentety xmlNodeIdentety = new XmlToDatasetMap.XmlNodeIdentety(text, text2);
			columns[xmlNodeIdentety] = col;
			if (col.ColumnName.StartsWith("xml", StringComparison.OrdinalIgnoreCase))
			{
				this.HandleSpecialColumn(col, nameTable, columns);
			}
			return true;
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x002103F4 File Offset: 0x0020F7F4
		private bool AddColumnSchema(XmlNameTable nameTable, DataColumn col, XmlToDatasetMap.XmlNodeIdHashtable columns)
		{
			string text = XmlConvert.EncodeLocalName(col.ColumnName);
			string text2 = nameTable.Get(text);
			if (text2 == null)
			{
				text2 = nameTable.Add(text);
			}
			col.encodedColumnName = text2;
			string text3 = nameTable.Get(col.Namespace);
			if (text3 == null)
			{
				text3 = nameTable.Add(col.Namespace);
			}
			else if (col._columnUri != null)
			{
				col._columnUri = text3;
			}
			XmlToDatasetMap.XmlNodeIdentety xmlNodeIdentety = new XmlToDatasetMap.XmlNodeIdentety(text2, text3);
			columns[xmlNodeIdentety] = col;
			if (col.ColumnName.StartsWith("xml", StringComparison.OrdinalIgnoreCase))
			{
				this.HandleSpecialColumn(col, nameTable, columns);
			}
			return true;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00210484 File Offset: 0x0020F884
		private void BuildIdentityMap(DataSet dataSet, XmlNameTable nameTable)
		{
			this.tableSchemaMap = new XmlToDatasetMap.XmlNodeIdHashtable(dataSet.Tables.Count);
			foreach (object obj in dataSet.Tables)
			{
				DataTable dataTable = (DataTable)obj;
				XmlToDatasetMap.TableSchemaInfo tableSchemaInfo = this.AddTableSchema(dataTable, nameTable);
				if (tableSchemaInfo != null)
				{
					foreach (object obj2 in dataTable.Columns)
					{
						DataColumn dataColumn = (DataColumn)obj2;
						if (XmlToDatasetMap.IsMappedColumn(dataColumn))
						{
							this.AddColumnSchema(dataColumn, nameTable, tableSchemaInfo.ColumnsSchemaMap);
						}
					}
				}
			}
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00210574 File Offset: 0x0020F974
		private void BuildIdentityMap(XmlNameTable nameTable, DataSet dataSet)
		{
			this.tableSchemaMap = new XmlToDatasetMap.XmlNodeIdHashtable(dataSet.Tables.Count);
			string text = nameTable.Get(dataSet.Namespace);
			if (text == null)
			{
				text = nameTable.Add(dataSet.Namespace);
			}
			dataSet.namespaceURI = text;
			foreach (object obj in dataSet.Tables)
			{
				DataTable dataTable = (DataTable)obj;
				XmlToDatasetMap.TableSchemaInfo tableSchemaInfo = this.AddTableSchema(nameTable, dataTable);
				if (tableSchemaInfo != null)
				{
					foreach (object obj2 in dataTable.Columns)
					{
						DataColumn dataColumn = (DataColumn)obj2;
						if (XmlToDatasetMap.IsMappedColumn(dataColumn))
						{
							this.AddColumnSchema(nameTable, dataColumn, tableSchemaInfo.ColumnsSchemaMap);
						}
					}
					foreach (object obj3 in dataTable.ChildRelations)
					{
						DataRelation dataRelation = (DataRelation)obj3;
						if (dataRelation.Nested)
						{
							string text2 = XmlConvert.EncodeLocalName(dataRelation.ChildTable.TableName);
							string text3 = nameTable.Get(text2);
							if (text3 == null)
							{
								text3 = nameTable.Add(text2);
							}
							string text4 = nameTable.Get(dataRelation.ChildTable.Namespace);
							if (text4 == null)
							{
								text4 = nameTable.Add(dataRelation.ChildTable.Namespace);
							}
							XmlToDatasetMap.XmlNodeIdentety xmlNodeIdentety = new XmlToDatasetMap.XmlNodeIdentety(text3, text4);
							tableSchemaInfo.ColumnsSchemaMap[xmlNodeIdentety] = dataRelation.ChildTable;
						}
					}
				}
			}
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0021076C File Offset: 0x0020FB6C
		private void BuildIdentityMap(DataTable dataTable, XmlNameTable nameTable)
		{
			this.tableSchemaMap = new XmlToDatasetMap.XmlNodeIdHashtable(1);
			XmlToDatasetMap.TableSchemaInfo tableSchemaInfo = this.AddTableSchema(dataTable, nameTable);
			if (tableSchemaInfo != null)
			{
				foreach (object obj in dataTable.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj;
					if (XmlToDatasetMap.IsMappedColumn(dataColumn))
					{
						this.AddColumnSchema(dataColumn, nameTable, tableSchemaInfo.ColumnsSchemaMap);
					}
				}
			}
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x002107FC File Offset: 0x0020FBFC
		private void BuildIdentityMap(XmlNameTable nameTable, DataTable dataTable)
		{
			ArrayList selfAndDescendants = this.GetSelfAndDescendants(dataTable);
			this.tableSchemaMap = new XmlToDatasetMap.XmlNodeIdHashtable(selfAndDescendants.Count);
			foreach (object obj in selfAndDescendants)
			{
				DataTable dataTable2 = (DataTable)obj;
				XmlToDatasetMap.TableSchemaInfo tableSchemaInfo = this.AddTableSchema(nameTable, dataTable2);
				if (tableSchemaInfo != null)
				{
					foreach (object obj2 in dataTable2.Columns)
					{
						DataColumn dataColumn = (DataColumn)obj2;
						if (XmlToDatasetMap.IsMappedColumn(dataColumn))
						{
							this.AddColumnSchema(nameTable, dataColumn, tableSchemaInfo.ColumnsSchemaMap);
						}
					}
					foreach (object obj3 in dataTable2.ChildRelations)
					{
						DataRelation dataRelation = (DataRelation)obj3;
						if (dataRelation.Nested)
						{
							string text = XmlConvert.EncodeLocalName(dataRelation.ChildTable.TableName);
							string text2 = nameTable.Get(text);
							if (text2 == null)
							{
								text2 = nameTable.Add(text);
							}
							string text3 = nameTable.Get(dataRelation.ChildTable.Namespace);
							if (text3 == null)
							{
								text3 = nameTable.Add(dataRelation.ChildTable.Namespace);
							}
							XmlToDatasetMap.XmlNodeIdentety xmlNodeIdentety = new XmlToDatasetMap.XmlNodeIdentety(text2, text3);
							tableSchemaInfo.ColumnsSchemaMap[xmlNodeIdentety] = dataRelation.ChildTable;
						}
					}
				}
			}
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x002109CC File Offset: 0x0020FDCC
		private ArrayList GetSelfAndDescendants(DataTable dt)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.Add(dt);
			for (int i = 0; i < arrayList.Count; i++)
			{
				foreach (object obj in ((DataTable)arrayList[i]).ChildRelations)
				{
					DataRelation dataRelation = (DataRelation)obj;
					if (!arrayList.Contains(dataRelation.ChildTable))
					{
						arrayList.Add(dataRelation.ChildTable);
					}
				}
			}
			return arrayList;
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00210A74 File Offset: 0x0020FE74
		public object GetColumnSchema(XmlNode node, bool fIgnoreNamespace)
		{
			XmlNode xmlNode = ((node.NodeType == XmlNodeType.Attribute) ? ((XmlAttribute)node).OwnerElement : node.ParentNode);
			while (xmlNode != null && xmlNode.NodeType == XmlNodeType.Element)
			{
				XmlToDatasetMap.TableSchemaInfo tableSchemaInfo = (XmlToDatasetMap.TableSchemaInfo)(fIgnoreNamespace ? this.tableSchemaMap[xmlNode.LocalName] : this.tableSchemaMap[xmlNode]);
				xmlNode = xmlNode.ParentNode;
				if (tableSchemaInfo != null)
				{
					if (fIgnoreNamespace)
					{
						return tableSchemaInfo.ColumnsSchemaMap[node.LocalName];
					}
					return tableSchemaInfo.ColumnsSchemaMap[node];
				}
			}
			return null;
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00210B04 File Offset: 0x0020FF04
		public object GetColumnSchema(DataTable table, XmlReader dataReader, bool fIgnoreNamespace)
		{
			if (this.lastTableSchemaInfo == null || this.lastTableSchemaInfo.TableSchema != table)
			{
				this.lastTableSchemaInfo = (XmlToDatasetMap.TableSchemaInfo)(fIgnoreNamespace ? this.tableSchemaMap[table.EncodedTableName] : this.tableSchemaMap[table]);
			}
			if (fIgnoreNamespace)
			{
				return this.lastTableSchemaInfo.ColumnsSchemaMap[dataReader.LocalName];
			}
			return this.lastTableSchemaInfo.ColumnsSchemaMap[dataReader];
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00210B80 File Offset: 0x0020FF80
		public object GetSchemaForNode(XmlNode node, bool fIgnoreNamespace)
		{
			XmlToDatasetMap.TableSchemaInfo tableSchemaInfo = null;
			if (node.NodeType == XmlNodeType.Element)
			{
				tableSchemaInfo = (XmlToDatasetMap.TableSchemaInfo)(fIgnoreNamespace ? this.tableSchemaMap[node.LocalName] : this.tableSchemaMap[node]);
			}
			if (tableSchemaInfo != null)
			{
				return tableSchemaInfo.TableSchema;
			}
			return this.GetColumnSchema(node, fIgnoreNamespace);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00210BD4 File Offset: 0x0020FFD4
		public DataTable GetTableForNode(XmlReader node, bool fIgnoreNamespace)
		{
			XmlToDatasetMap.TableSchemaInfo tableSchemaInfo = (XmlToDatasetMap.TableSchemaInfo)(fIgnoreNamespace ? this.tableSchemaMap[node.LocalName] : this.tableSchemaMap[node]);
			if (tableSchemaInfo != null)
			{
				this.lastTableSchemaInfo = tableSchemaInfo;
				return this.lastTableSchemaInfo.TableSchema;
			}
			return null;
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00210C20 File Offset: 0x00210020
		private void HandleSpecialColumn(DataColumn col, XmlNameTable nameTable, XmlToDatasetMap.XmlNodeIdHashtable columns)
		{
			string text;
			if ('x' == col.ColumnName[0])
			{
				text = "_x0078_";
			}
			else
			{
				text = "_x0058_";
			}
			text += col.ColumnName.Substring(1);
			if (nameTable.Get(text) == null)
			{
				nameTable.Add(text);
			}
			string text2 = nameTable.Get(col.Namespace);
			XmlToDatasetMap.XmlNodeIdentety xmlNodeIdentety = new XmlToDatasetMap.XmlNodeIdentety(text, text2);
			columns[xmlNodeIdentety] = col;
		}

		// Token: 0x04000A9A RID: 2714
		private XmlToDatasetMap.XmlNodeIdHashtable tableSchemaMap;

		// Token: 0x04000A9B RID: 2715
		private XmlToDatasetMap.TableSchemaInfo lastTableSchemaInfo;

		// Token: 0x020000FF RID: 255
		private sealed class XmlNodeIdentety
		{
			// Token: 0x06000EF6 RID: 3830 RVA: 0x00210C8C File Offset: 0x0021008C
			public XmlNodeIdentety(string localName, string namespaceURI)
			{
				this.LocalName = localName;
				this.NamespaceURI = namespaceURI;
			}

			// Token: 0x06000EF7 RID: 3831 RVA: 0x00210CB0 File Offset: 0x002100B0
			public override int GetHashCode()
			{
				return this.LocalName.GetHashCode();
			}

			// Token: 0x06000EF8 RID: 3832 RVA: 0x00210CC8 File Offset: 0x002100C8
			public override bool Equals(object obj)
			{
				XmlToDatasetMap.XmlNodeIdentety xmlNodeIdentety = (XmlToDatasetMap.XmlNodeIdentety)obj;
				return string.Compare(this.LocalName, xmlNodeIdentety.LocalName, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(this.NamespaceURI, xmlNodeIdentety.NamespaceURI, StringComparison.OrdinalIgnoreCase) == 0;
			}

			// Token: 0x04000A9C RID: 2716
			public string LocalName;

			// Token: 0x04000A9D RID: 2717
			public string NamespaceURI;
		}

		// Token: 0x02000100 RID: 256
		internal sealed class XmlNodeIdHashtable : Hashtable
		{
			// Token: 0x06000EF9 RID: 3833 RVA: 0x00210D08 File Offset: 0x00210108
			public XmlNodeIdHashtable(int capacity)
				: base(capacity)
			{
			}

			// Token: 0x1700022F RID: 559
			public object this[XmlNode node]
			{
				get
				{
					this.id.LocalName = node.LocalName;
					this.id.NamespaceURI = node.NamespaceURI;
					return this[this.id];
				}
			}

			// Token: 0x17000230 RID: 560
			public object this[XmlReader dataReader]
			{
				get
				{
					this.id.LocalName = dataReader.LocalName;
					this.id.NamespaceURI = dataReader.NamespaceURI;
					return this[this.id];
				}
			}

			// Token: 0x17000231 RID: 561
			public object this[DataTable table]
			{
				get
				{
					this.id.LocalName = table.EncodedTableName;
					this.id.NamespaceURI = table.Namespace;
					return this[this.id];
				}
			}

			// Token: 0x17000232 RID: 562
			public object this[string name]
			{
				get
				{
					this.id.LocalName = name;
					this.id.NamespaceURI = string.Empty;
					return this[this.id];
				}
			}

			// Token: 0x04000A9E RID: 2718
			private XmlToDatasetMap.XmlNodeIdentety id = new XmlToDatasetMap.XmlNodeIdentety(string.Empty, string.Empty);
		}

		// Token: 0x02000101 RID: 257
		private sealed class TableSchemaInfo
		{
			// Token: 0x06000EFE RID: 3838 RVA: 0x00210E20 File Offset: 0x00210220
			public TableSchemaInfo(DataTable tableSchema)
			{
				this.TableSchema = tableSchema;
				this.ColumnsSchemaMap = new XmlToDatasetMap.XmlNodeIdHashtable(tableSchema.Columns.Count);
			}

			// Token: 0x04000A9F RID: 2719
			public DataTable TableSchema;

			// Token: 0x04000AA0 RID: 2720
			public XmlToDatasetMap.XmlNodeIdHashtable ColumnsSchemaMap;
		}
	}
}

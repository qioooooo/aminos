using System;
using System.Collections;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data
{
	// Token: 0x020000F0 RID: 240
	internal sealed class XmlDataLoader
	{
		// Token: 0x06000DE8 RID: 3560 RVA: 0x00202AEC File Offset: 0x00201EEC
		internal XmlDataLoader(DataSet dataset, bool IsXdr, bool ignoreSchema)
		{
			this.dataSet = dataset;
			this.nodeToRowMap = new Hashtable();
			this.fIsXdr = IsXdr;
			this.ignoreSchema = ignoreSchema;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00202B20 File Offset: 0x00201F20
		internal XmlDataLoader(DataSet dataset, bool IsXdr, XmlElement topNode, bool ignoreSchema)
		{
			this.dataSet = dataset;
			this.nodeToRowMap = new Hashtable();
			this.fIsXdr = IsXdr;
			this.childRowsStack = new Stack(50);
			this.topMostNode = topNode;
			this.ignoreSchema = ignoreSchema;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00202B68 File Offset: 0x00201F68
		internal XmlDataLoader(DataTable datatable, bool IsXdr, bool ignoreSchema)
		{
			this.dataSet = null;
			this.dataTable = datatable;
			this.isTableLevel = true;
			this.nodeToRowMap = new Hashtable();
			this.fIsXdr = IsXdr;
			this.ignoreSchema = ignoreSchema;
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00202BAC File Offset: 0x00201FAC
		internal XmlDataLoader(DataTable datatable, bool IsXdr, XmlElement topNode, bool ignoreSchema)
		{
			this.dataSet = null;
			this.dataTable = datatable;
			this.isTableLevel = true;
			this.nodeToRowMap = new Hashtable();
			this.fIsXdr = IsXdr;
			this.childRowsStack = new Stack(50);
			this.topMostNode = topNode;
			this.ignoreSchema = ignoreSchema;
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x00202C04 File Offset: 0x00202004
		// (set) Token: 0x06000DED RID: 3565 RVA: 0x00202C18 File Offset: 0x00202018
		internal bool FromInference
		{
			get
			{
				return this.fromInference;
			}
			set
			{
				this.fromInference = value;
			}
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00202C2C File Offset: 0x0020202C
		private void AttachRows(DataRow parentRow, XmlNode parentElement)
		{
			if (parentElement == null)
			{
				return;
			}
			for (XmlNode xmlNode = parentElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					XmlElement xmlElement = (XmlElement)xmlNode;
					DataRow rowFromElement = this.GetRowFromElement(xmlElement);
					if (rowFromElement != null && rowFromElement.RowState == DataRowState.Detached)
					{
						if (parentRow != null)
						{
							rowFromElement.SetNestedParentRow(parentRow, false);
						}
						rowFromElement.Table.Rows.Add(rowFromElement);
					}
					else if (rowFromElement == null)
					{
						this.AttachRows(parentRow, xmlNode);
					}
					this.AttachRows(rowFromElement, xmlNode);
				}
			}
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00202CA8 File Offset: 0x002020A8
		private int CountNonNSAttributes(XmlNode node)
		{
			int num = 0;
			for (int i = 0; i < node.Attributes.Count; i++)
			{
				XmlAttribute xmlAttribute = node.Attributes[i];
				if (!this.FExcludedNamespace(node.Attributes[i].NamespaceURI))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x00202CF8 File Offset: 0x002020F8
		private string GetValueForTextOnlyColums(XmlNode n)
		{
			string text = null;
			while (n != null && (n.NodeType == XmlNodeType.Whitespace || !this.IsTextLikeNode(n.NodeType)))
			{
				n = n.NextSibling;
			}
			if (n != null)
			{
				if (this.IsTextLikeNode(n.NodeType) && (n.NextSibling == null || !this.IsTextLikeNode(n.NodeType)))
				{
					text = n.Value;
					n = n.NextSibling;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (n != null && this.IsTextLikeNode(n.NodeType))
					{
						stringBuilder.Append(n.Value);
						n = n.NextSibling;
					}
					text = stringBuilder.ToString();
				}
			}
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00202DA4 File Offset: 0x002021A4
		private string GetInitialTextFromNodes(ref XmlNode n)
		{
			string text = null;
			if (n != null)
			{
				while (n.NodeType == XmlNodeType.Whitespace)
				{
					n = n.NextSibling;
				}
				if (this.IsTextLikeNode(n.NodeType) && (n.NextSibling == null || !this.IsTextLikeNode(n.NodeType)))
				{
					text = n.Value;
					n = n.NextSibling;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (n != null && this.IsTextLikeNode(n.NodeType))
					{
						stringBuilder.Append(n.Value);
						n = n.NextSibling;
					}
					text = stringBuilder.ToString();
				}
			}
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00202E50 File Offset: 0x00202250
		private DataColumn GetTextOnlyColumn(DataRow row)
		{
			DataColumnCollection columns = row.Table.Columns;
			int count = columns.Count;
			for (int i = 0; i < count; i++)
			{
				DataColumn dataColumn = columns[i];
				if (this.IsTextOnly(dataColumn))
				{
					return dataColumn;
				}
			}
			return null;
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00202E90 File Offset: 0x00202290
		internal DataRow GetRowFromElement(XmlElement e)
		{
			return (DataRow)this.nodeToRowMap[e];
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00202EB0 File Offset: 0x002022B0
		internal bool FColumnElement(XmlElement e)
		{
			if (this.nodeToSchemaMap.GetColumnSchema(e, this.FIgnoreNamespace(e)) == null)
			{
				return false;
			}
			if (this.CountNonNSAttributes(e) > 0)
			{
				return false;
			}
			for (XmlNode xmlNode = e.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode is XmlElement)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00202F00 File Offset: 0x00202300
		private bool FExcludedNamespace(string ns)
		{
			return ns.Equals("http://www.w3.org/2000/xmlns/") || (this.htableExcludedNS != null && this.htableExcludedNS.Contains(ns));
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00202F34 File Offset: 0x00202334
		private bool FIgnoreNamespace(XmlNode node)
		{
			if (!this.fIsXdr)
			{
				return false;
			}
			XmlNode xmlNode;
			if (node is XmlAttribute)
			{
				xmlNode = ((XmlAttribute)node).OwnerElement;
			}
			else
			{
				xmlNode = node;
			}
			return xmlNode.NamespaceURI.StartsWith("x-schema:#", StringComparison.Ordinal);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00202F7C File Offset: 0x0020237C
		private bool FIgnoreNamespace(XmlReader node)
		{
			return this.fIsXdr && node.NamespaceURI.StartsWith("x-schema:#", StringComparison.Ordinal);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00202FA8 File Offset: 0x002023A8
		internal bool IsTextLikeNode(XmlNodeType n)
		{
			switch (n)
			{
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				break;
			case XmlNodeType.EntityReference:
				throw ExceptionBuilder.FoundEntity();
			default:
				switch (n)
				{
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					break;
				default:
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00202FE8 File Offset: 0x002023E8
		internal bool IsTextOnly(DataColumn c)
		{
			return c.ColumnMapping == MappingType.SimpleContent;
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00203004 File Offset: 0x00202404
		internal void LoadData(XmlDocument xdoc)
		{
			if (xdoc.DocumentElement == null)
			{
				return;
			}
			bool flag;
			if (this.isTableLevel)
			{
				flag = this.dataTable.EnforceConstraints;
				this.dataTable.EnforceConstraints = false;
			}
			else
			{
				flag = this.dataSet.EnforceConstraints;
				this.dataSet.EnforceConstraints = false;
				this.dataSet.fInReadXml = true;
			}
			if (this.isTableLevel)
			{
				this.nodeToSchemaMap = new XmlToDatasetMap(this.dataTable, xdoc.NameTable);
			}
			else
			{
				this.nodeToSchemaMap = new XmlToDatasetMap(this.dataSet, xdoc.NameTable);
			}
			DataRow dataRow = null;
			if (this.isTableLevel || (this.dataSet != null && this.dataSet.fTopLevelTable))
			{
				XmlElement documentElement = xdoc.DocumentElement;
				DataTable dataTable = (DataTable)this.nodeToSchemaMap.GetSchemaForNode(documentElement, this.FIgnoreNamespace(documentElement));
				if (dataTable != null)
				{
					dataRow = dataTable.CreateEmptyRow();
					this.nodeToRowMap[documentElement] = dataRow;
					this.LoadRowData(dataRow, documentElement);
					dataTable.Rows.Add(dataRow);
				}
			}
			this.LoadRows(dataRow, xdoc.DocumentElement);
			this.AttachRows(dataRow, xdoc.DocumentElement);
			if (this.isTableLevel)
			{
				this.dataTable.EnforceConstraints = flag;
				return;
			}
			this.dataSet.fInReadXml = false;
			this.dataSet.EnforceConstraints = flag;
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0020314C File Offset: 0x0020254C
		private void LoadRowData(DataRow row, XmlElement rowElement)
		{
			DataTable table = row.Table;
			if (this.FromInference)
			{
				table.Prefix = rowElement.Prefix;
			}
			Hashtable hashtable = new Hashtable();
			row.BeginEdit();
			XmlNode xmlNode = rowElement.FirstChild;
			DataColumn textOnlyColumn = this.GetTextOnlyColumn(row);
			if (textOnlyColumn != null)
			{
				hashtable[textOnlyColumn] = textOnlyColumn;
				string valueForTextOnlyColums = this.GetValueForTextOnlyColums(xmlNode);
				if (XMLSchema.GetBooleanAttribute(rowElement, "nil", "http://www.w3.org/2001/XMLSchema-instance", false) && ADP.IsEmpty(valueForTextOnlyColums))
				{
					row[textOnlyColumn] = DBNull.Value;
				}
				else
				{
					this.SetRowValueFromXmlText(row, textOnlyColumn, valueForTextOnlyColums);
				}
			}
			while (xmlNode != null && xmlNode != rowElement)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					XmlElement xmlElement = (XmlElement)xmlNode;
					object obj = this.nodeToSchemaMap.GetSchemaForNode(xmlElement, this.FIgnoreNamespace(xmlElement));
					if (obj is DataTable && this.FColumnElement(xmlElement))
					{
						obj = this.nodeToSchemaMap.GetColumnSchema(xmlElement, this.FIgnoreNamespace(xmlElement));
					}
					if (obj == null || obj is DataColumn)
					{
						xmlNode = xmlElement.FirstChild;
						if (obj != null && obj is DataColumn)
						{
							DataColumn dataColumn = (DataColumn)obj;
							if (dataColumn.Table == row.Table && dataColumn.ColumnMapping != MappingType.Attribute && hashtable[dataColumn] == null)
							{
								hashtable[dataColumn] = dataColumn;
								string valueForTextOnlyColums2 = this.GetValueForTextOnlyColums(xmlNode);
								if (XMLSchema.GetBooleanAttribute(xmlElement, "nil", "http://www.w3.org/2001/XMLSchema-instance", false) && ADP.IsEmpty(valueForTextOnlyColums2))
								{
									row[dataColumn] = DBNull.Value;
								}
								else
								{
									this.SetRowValueFromXmlText(row, dataColumn, valueForTextOnlyColums2);
								}
							}
						}
						else if (obj == null && xmlNode != null)
						{
							continue;
						}
						if (xmlNode == null)
						{
							xmlNode = xmlElement;
						}
					}
				}
				while (xmlNode != rowElement && xmlNode.NextSibling == null)
				{
					xmlNode = xmlNode.ParentNode;
				}
				if (xmlNode != rowElement)
				{
					xmlNode = xmlNode.NextSibling;
				}
			}
			foreach (object obj2 in rowElement.Attributes)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)obj2;
				object columnSchema = this.nodeToSchemaMap.GetColumnSchema(xmlAttribute, this.FIgnoreNamespace(xmlAttribute));
				if (columnSchema != null && columnSchema is DataColumn)
				{
					DataColumn dataColumn2 = (DataColumn)columnSchema;
					if (dataColumn2.ColumnMapping == MappingType.Attribute && hashtable[dataColumn2] == null)
					{
						hashtable[dataColumn2] = dataColumn2;
						xmlNode = xmlAttribute.FirstChild;
						this.SetRowValueFromXmlText(row, dataColumn2, this.GetInitialTextFromNodes(ref xmlNode));
					}
				}
			}
			foreach (object obj3 in row.Table.Columns)
			{
				DataColumn dataColumn3 = (DataColumn)obj3;
				if (hashtable[dataColumn3] == null && XmlToDatasetMap.IsMappedColumn(dataColumn3))
				{
					if (!dataColumn3.AutoIncrement)
					{
						if (dataColumn3.AllowDBNull)
						{
							row[dataColumn3] = DBNull.Value;
						}
						else
						{
							row[dataColumn3] = dataColumn3.DefaultValue;
						}
					}
					else
					{
						dataColumn3.Init(row.tempRecord);
					}
				}
			}
			row.EndEdit();
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00203478 File Offset: 0x00202878
		private void LoadRows(DataRow parentRow, XmlNode parentElement)
		{
			if (parentElement == null)
			{
				return;
			}
			if ((parentElement.LocalName == "schema" && parentElement.NamespaceURI == "http://www.w3.org/2001/XMLSchema") || (parentElement.LocalName == "sync" && parentElement.NamespaceURI == "urn:schemas-microsoft-com:xml-updategram") || (parentElement.LocalName == "Schema" && parentElement.NamespaceURI == "urn:schemas-microsoft-com:xml-data"))
			{
				return;
			}
			for (XmlNode xmlNode = parentElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode is XmlElement)
				{
					XmlElement xmlElement = (XmlElement)xmlNode;
					object schemaForNode = this.nodeToSchemaMap.GetSchemaForNode(xmlElement, this.FIgnoreNamespace(xmlElement));
					if (schemaForNode != null && schemaForNode is DataTable)
					{
						DataRow dataRow = this.GetRowFromElement(xmlElement);
						if (dataRow == null)
						{
							if (parentRow != null && this.FColumnElement(xmlElement))
							{
								goto IL_00F5;
							}
							dataRow = ((DataTable)schemaForNode).CreateEmptyRow();
							this.nodeToRowMap[xmlElement] = dataRow;
							this.LoadRowData(dataRow, xmlElement);
						}
						this.LoadRows(dataRow, xmlNode);
					}
					else
					{
						this.LoadRows(null, xmlNode);
					}
				}
				IL_00F5:;
			}
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x00203588 File Offset: 0x00202988
		private void SetRowValueFromXmlText(DataRow row, DataColumn col, string xmlText)
		{
			row[col] = col.ConvertXmlToObject(xmlText);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x002035A4 File Offset: 0x002029A4
		internal void LoadTopMostRow(ref bool[] foundColumns)
		{
			object schemaForNode = this.nodeToSchemaMap.GetSchemaForNode(this.topMostNode, this.FIgnoreNamespace(this.topMostNode));
			if (schemaForNode is DataTable)
			{
				DataTable dataTable = (DataTable)schemaForNode;
				this.topMostRow = dataTable.CreateEmptyRow();
				foundColumns = new bool[this.topMostRow.Table.Columns.Count];
				foreach (object obj in this.topMostNode.Attributes)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)obj;
					object columnSchema = this.nodeToSchemaMap.GetColumnSchema(xmlAttribute, this.FIgnoreNamespace(xmlAttribute));
					if (columnSchema != null && columnSchema is DataColumn)
					{
						DataColumn dataColumn = (DataColumn)columnSchema;
						if (dataColumn.ColumnMapping == MappingType.Attribute)
						{
							XmlNode firstChild = xmlAttribute.FirstChild;
							this.SetRowValueFromXmlText(this.topMostRow, dataColumn, this.GetInitialTextFromNodes(ref firstChild));
							foundColumns[dataColumn.Ordinal] = true;
						}
					}
				}
			}
			this.topMostNode = null;
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x002036C4 File Offset: 0x00202AC4
		private void InitNameTable()
		{
			XmlNameTable nameTable = this.dataReader.NameTable;
			this.XSD_XMLNS_NS = nameTable.Add("http://www.w3.org/2000/xmlns/");
			this.XDR_SCHEMA = nameTable.Add("Schema");
			this.XDRNS = nameTable.Add("urn:schemas-microsoft-com:xml-data");
			this.SQL_SYNC = nameTable.Add("sync");
			this.UPDGNS = nameTable.Add("urn:schemas-microsoft-com:xml-updategram");
			this.XSD_SCHEMA = nameTable.Add("schema");
			this.XSDNS = nameTable.Add("http://www.w3.org/2001/XMLSchema");
			this.DFFNS = nameTable.Add("urn:schemas-microsoft-com:xml-diffgram-v1");
			this.MSDNS = nameTable.Add("urn:schemas-microsoft-com:xml-msdata");
			this.DIFFID = nameTable.Add("id");
			this.HASCHANGES = nameTable.Add("hasChanges");
			this.ROWORDER = nameTable.Add("rowOrder");
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x002037AC File Offset: 0x00202BAC
		internal void LoadData(XmlReader reader)
		{
			this.dataReader = DataTextReader.CreateReader(reader);
			int depth = this.dataReader.Depth;
			bool flag = (this.isTableLevel ? this.dataTable.EnforceConstraints : this.dataSet.EnforceConstraints);
			this.InitNameTable();
			if (this.nodeToSchemaMap == null)
			{
				this.nodeToSchemaMap = (this.isTableLevel ? new XmlToDatasetMap(this.dataReader.NameTable, this.dataTable) : new XmlToDatasetMap(this.dataReader.NameTable, this.dataSet));
			}
			if (this.isTableLevel)
			{
				this.dataTable.EnforceConstraints = false;
			}
			else
			{
				this.dataSet.EnforceConstraints = false;
				this.dataSet.fInReadXml = true;
			}
			if (this.topMostNode != null)
			{
				if (!this.isDiffgram && !this.isTableLevel)
				{
					DataTable dataTable = this.nodeToSchemaMap.GetSchemaForNode(this.topMostNode, this.FIgnoreNamespace(this.topMostNode)) as DataTable;
					if (dataTable != null)
					{
						this.LoadTopMostTable(dataTable);
					}
				}
				this.topMostNode = null;
			}
			while (!this.dataReader.EOF && this.dataReader.Depth >= depth)
			{
				if (reader.NodeType != XmlNodeType.Element)
				{
					this.dataReader.Read();
				}
				else
				{
					DataTable tableForNode = this.nodeToSchemaMap.GetTableForNode(this.dataReader, this.FIgnoreNamespace(this.dataReader));
					if (tableForNode == null)
					{
						if (!this.ProcessXsdSchema())
						{
							this.dataReader.Read();
						}
					}
					else
					{
						this.LoadTable(tableForNode, false);
					}
				}
			}
			if (this.isTableLevel)
			{
				this.dataTable.EnforceConstraints = flag;
				return;
			}
			this.dataSet.fInReadXml = false;
			this.dataSet.EnforceConstraints = flag;
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0020395C File Offset: 0x00202D5C
		private void LoadTopMostTable(DataTable table)
		{
			bool flag = this.isTableLevel || this.dataSet.DataSetName != table.TableName;
			bool flag2 = false;
			int num = this.dataReader.Depth - 1;
			int i = this.childRowsStack.Count;
			DataColumnCollection columns = table.Columns;
			object[] array = new object[columns.Count];
			DataColumn dataColumn;
			using (IEnumerator enumerator = this.topMostNode.Attributes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					XmlAttribute xmlAttribute = (XmlAttribute)obj;
					dataColumn = this.nodeToSchemaMap.GetColumnSchema(xmlAttribute, this.FIgnoreNamespace(xmlAttribute)) as DataColumn;
					if (dataColumn != null && dataColumn.ColumnMapping == MappingType.Attribute)
					{
						XmlNode firstChild = xmlAttribute.FirstChild;
						array[dataColumn.Ordinal] = dataColumn.ConvertXmlToObject(this.GetInitialTextFromNodes(ref firstChild));
						flag2 = true;
					}
				}
				goto IL_01FE;
			}
			IL_00E8:
			XmlNodeType nodeType = this.dataReader.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Element:
			{
				object columnSchema = this.nodeToSchemaMap.GetColumnSchema(table, this.dataReader, this.FIgnoreNamespace(this.dataReader));
				dataColumn = columnSchema as DataColumn;
				if (dataColumn != null)
				{
					if (array[dataColumn.Ordinal] == null)
					{
						this.LoadColumn(dataColumn, array);
						flag2 = true;
						goto IL_01FE;
					}
					this.dataReader.Read();
					goto IL_01FE;
				}
				else
				{
					DataTable dataTable = columnSchema as DataTable;
					if (dataTable != null)
					{
						this.LoadTable(dataTable, true);
						flag2 = true;
						goto IL_01FE;
					}
					if (this.ProcessXsdSchema())
					{
						goto IL_01FE;
					}
					if (!flag2 && !flag)
					{
						return;
					}
					this.dataReader.Read();
					goto IL_01FE;
				}
				break;
			}
			case XmlNodeType.Attribute:
				goto IL_01F2;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				break;
			case XmlNodeType.EntityReference:
				throw ExceptionBuilder.FoundEntity();
			default:
				switch (nodeType)
				{
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					break;
				default:
					goto IL_01F2;
				}
				break;
			}
			string text = this.dataReader.ReadString();
			dataColumn = table.xmlText;
			if (dataColumn != null && array[dataColumn.Ordinal] == null)
			{
				array[dataColumn.Ordinal] = dataColumn.ConvertXmlToObject(text);
				goto IL_01FE;
			}
			goto IL_01FE;
			IL_01F2:
			this.dataReader.Read();
			IL_01FE:
			if (num >= this.dataReader.Depth)
			{
				this.dataReader.Read();
				for (int j = array.Length - 1; j >= 0; j--)
				{
					if (array[j] == null)
					{
						dataColumn = columns[j];
						if (dataColumn.AllowDBNull && dataColumn.ColumnMapping != MappingType.Hidden && !dataColumn.AutoIncrement)
						{
							array[j] = DBNull.Value;
						}
					}
				}
				DataRow dataRow = table.Rows.AddWithColumnEvents(array);
				while (i < this.childRowsStack.Count)
				{
					DataRow dataRow2 = (DataRow)this.childRowsStack.Pop();
					bool flag3 = dataRow2.RowState == DataRowState.Unchanged;
					dataRow2.SetNestedParentRow(dataRow, false);
					if (flag3)
					{
						dataRow2.oldRecord = dataRow2.newRecord;
					}
				}
				return;
			}
			goto IL_00E8;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00203C34 File Offset: 0x00203034
		private void LoadTable(DataTable table, bool isNested)
		{
			int i = this.dataReader.Depth;
			int j = this.childRowsStack.Count;
			DataColumnCollection columns = table.Columns;
			object[] array = new object[columns.Count];
			int num = -1;
			string text = string.Empty;
			string text2 = null;
			bool flag = false;
			for (int k = this.dataReader.AttributeCount - 1; k >= 0; k--)
			{
				this.dataReader.MoveToAttribute(k);
				DataColumn dataColumn = this.nodeToSchemaMap.GetColumnSchema(table, this.dataReader, this.FIgnoreNamespace(this.dataReader)) as DataColumn;
				if (dataColumn != null && dataColumn.ColumnMapping == MappingType.Attribute)
				{
					array[dataColumn.Ordinal] = dataColumn.ConvertXmlToObject(this.dataReader.Value);
				}
				if (this.isDiffgram)
				{
					if (this.dataReader.NamespaceURI == "urn:schemas-microsoft-com:xml-diffgram-v1")
					{
						string localName;
						if ((localName = this.dataReader.LocalName) != null)
						{
							if (!(localName == "id"))
							{
								if (!(localName == "hasChanges"))
								{
									if (localName == "hasErrors")
									{
										flag = (bool)Convert.ChangeType(this.dataReader.Value, typeof(bool), CultureInfo.InvariantCulture);
									}
								}
								else
								{
									text2 = this.dataReader.Value;
								}
							}
							else
							{
								text = this.dataReader.Value;
							}
						}
					}
					else if (this.dataReader.NamespaceURI == "urn:schemas-microsoft-com:xml-msdata")
					{
						if (this.dataReader.LocalName == "rowOrder")
						{
							num = (int)Convert.ChangeType(this.dataReader.Value, typeof(int), CultureInfo.InvariantCulture);
						}
						else if (this.dataReader.LocalName.StartsWith("hidden", StringComparison.Ordinal))
						{
							dataColumn = columns[XmlConvert.DecodeName(this.dataReader.LocalName.Substring(6))];
							if (dataColumn != null && dataColumn.ColumnMapping == MappingType.Hidden)
							{
								array[dataColumn.Ordinal] = dataColumn.ConvertXmlToObject(this.dataReader.Value);
							}
						}
					}
				}
			}
			if (this.dataReader.Read() && i < this.dataReader.Depth)
			{
				while (i < this.dataReader.Depth)
				{
					XmlNodeType nodeType = this.dataReader.NodeType;
					DataColumn dataColumn;
					switch (nodeType)
					{
					case XmlNodeType.Element:
					{
						object columnSchema = this.nodeToSchemaMap.GetColumnSchema(table, this.dataReader, this.FIgnoreNamespace(this.dataReader));
						dataColumn = columnSchema as DataColumn;
						if (dataColumn != null)
						{
							if (array[dataColumn.Ordinal] == null)
							{
								this.LoadColumn(dataColumn, array);
								continue;
							}
							this.dataReader.Read();
							continue;
						}
						else
						{
							DataTable dataTable = columnSchema as DataTable;
							if (dataTable != null)
							{
								this.LoadTable(dataTable, true);
								continue;
							}
							if (this.ProcessXsdSchema())
							{
								continue;
							}
							DataTable tableForNode = this.nodeToSchemaMap.GetTableForNode(this.dataReader, this.FIgnoreNamespace(this.dataReader));
							if (tableForNode != null)
							{
								this.LoadTable(tableForNode, false);
								continue;
							}
							this.dataReader.Read();
							continue;
						}
						break;
					}
					case XmlNodeType.Attribute:
						goto IL_0379;
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
						break;
					case XmlNodeType.EntityReference:
						throw ExceptionBuilder.FoundEntity();
					default:
						switch (nodeType)
						{
						case XmlNodeType.Whitespace:
						case XmlNodeType.SignificantWhitespace:
							break;
						default:
							goto IL_0379;
						}
						break;
					}
					string text3 = this.dataReader.ReadString();
					dataColumn = table.xmlText;
					if (dataColumn != null && array[dataColumn.Ordinal] == null)
					{
						array[dataColumn.Ordinal] = dataColumn.ConvertXmlToObject(text3);
						continue;
					}
					continue;
					IL_0379:
					this.dataReader.Read();
				}
				this.dataReader.Read();
			}
			DataRow dataRow;
			if (this.isDiffgram)
			{
				dataRow = table.NewRow(table.NewUninitializedRecord());
				dataRow.BeginEdit();
				for (int l = array.Length - 1; l >= 0; l--)
				{
					DataColumn dataColumn = columns[l];
					dataColumn[dataRow.tempRecord] = ((array[l] != null) ? array[l] : DBNull.Value);
				}
				dataRow.EndEdit();
				table.Rows.DiffInsertAt(dataRow, num);
				if (text2 == null)
				{
					dataRow.oldRecord = dataRow.newRecord;
				}
				if (text2 == "modified" || flag)
				{
					table.RowDiffId[text] = dataRow;
				}
			}
			else
			{
				for (int m = array.Length - 1; m >= 0; m--)
				{
					if (array[m] == null)
					{
						DataColumn dataColumn = columns[m];
						if (dataColumn.AllowDBNull && dataColumn.ColumnMapping != MappingType.Hidden && !dataColumn.AutoIncrement)
						{
							array[m] = DBNull.Value;
						}
					}
				}
				dataRow = table.Rows.AddWithColumnEvents(array);
			}
			while (j < this.childRowsStack.Count)
			{
				DataRow dataRow2 = (DataRow)this.childRowsStack.Pop();
				bool flag2 = dataRow2.RowState == DataRowState.Unchanged;
				dataRow2.SetNestedParentRow(dataRow, false);
				if (flag2)
				{
					dataRow2.oldRecord = dataRow2.newRecord;
				}
			}
			if (isNested)
			{
				this.childRowsStack.Push(dataRow);
			}
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00204130 File Offset: 0x00203530
		private void LoadColumn(DataColumn column, object[] foundColumns)
		{
			string text = string.Empty;
			string text2 = null;
			int i = this.dataReader.Depth;
			if (this.dataReader.AttributeCount > 0)
			{
				text2 = this.dataReader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			}
			if (column.IsCustomType)
			{
				object obj = null;
				string text3 = null;
				string text4 = null;
				XmlRootAttribute xmlRootAttribute = null;
				if (this.dataReader.AttributeCount > 0)
				{
					text3 = this.dataReader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
					text4 = this.dataReader.GetAttribute("InstanceType", "urn:schemas-microsoft-com:xml-msdata");
				}
				bool flag = !column.ImplementsIXMLSerializable && (column.DataType != typeof(object) && text4 == null) && text3 == null;
				if (text2 != null && XmlConvert.ToBoolean(text2))
				{
					if (!flag && text4 != null && text4.Length > 0)
					{
						obj = SqlUdtStorage.GetStaticNullForUdtType(Type.GetType(text4));
					}
					if (obj == null)
					{
						obj = DBNull.Value;
					}
					if (!this.dataReader.IsEmptyElement)
					{
						while (this.dataReader.Read() && i < this.dataReader.Depth)
						{
						}
					}
					this.dataReader.Read();
				}
				else
				{
					bool flag2 = false;
					if (column.Table.DataSet != null && column.Table.DataSet.UdtIsWrapped)
					{
						this.dataReader.Read();
						flag2 = true;
					}
					if (flag)
					{
						if (flag2)
						{
							xmlRootAttribute = new XmlRootAttribute(this.dataReader.LocalName);
							xmlRootAttribute.Namespace = this.dataReader.NamespaceURI;
						}
						else
						{
							xmlRootAttribute = new XmlRootAttribute(column.EncodedColumnName);
							xmlRootAttribute.Namespace = column.Namespace;
						}
					}
					obj = column.ConvertXmlToObject(this.dataReader, xmlRootAttribute);
					if (flag2)
					{
						this.dataReader.Read();
					}
				}
				foundColumns[column.Ordinal] = obj;
				return;
			}
			if (this.dataReader.Read() && i < this.dataReader.Depth)
			{
				while (i < this.dataReader.Depth)
				{
					XmlNodeType nodeType = this.dataReader.NodeType;
					switch (nodeType)
					{
					case XmlNodeType.Element:
					{
						if (this.ProcessXsdSchema())
						{
							continue;
						}
						object columnSchema = this.nodeToSchemaMap.GetColumnSchema(column.Table, this.dataReader, this.FIgnoreNamespace(this.dataReader));
						DataColumn dataColumn = columnSchema as DataColumn;
						if (dataColumn != null)
						{
							if (foundColumns[dataColumn.Ordinal] == null)
							{
								this.LoadColumn(dataColumn, foundColumns);
								continue;
							}
							this.dataReader.Read();
							continue;
						}
						else
						{
							DataTable dataTable = columnSchema as DataTable;
							if (dataTable != null)
							{
								this.LoadTable(dataTable, true);
								continue;
							}
							DataTable tableForNode = this.nodeToSchemaMap.GetTableForNode(this.dataReader, this.FIgnoreNamespace(this.dataReader));
							if (tableForNode != null)
							{
								this.LoadTable(tableForNode, false);
								continue;
							}
							this.dataReader.Read();
							continue;
						}
						break;
					}
					case XmlNodeType.Attribute:
						goto IL_0356;
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
						break;
					case XmlNodeType.EntityReference:
						throw ExceptionBuilder.FoundEntity();
					default:
						switch (nodeType)
						{
						case XmlNodeType.Whitespace:
						case XmlNodeType.SignificantWhitespace:
							break;
						default:
							goto IL_0356;
						}
						break;
					}
					if (text.Length == 0)
					{
						text = this.dataReader.Value;
						while (this.dataReader.Read() && i < this.dataReader.Depth)
						{
							if (!this.IsTextLikeNode(this.dataReader.NodeType))
							{
								break;
							}
							text += this.dataReader.Value;
						}
						continue;
					}
					this.dataReader.ReadString();
					continue;
					IL_0356:
					this.dataReader.Read();
				}
				this.dataReader.Read();
			}
			if (text.Length == 0 && text2 != null && XmlConvert.ToBoolean(text2))
			{
				foundColumns[column.Ordinal] = DBNull.Value;
				return;
			}
			foundColumns[column.Ordinal] = column.ConvertXmlToObject(text);
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x002044F0 File Offset: 0x002038F0
		private bool ProcessXsdSchema()
		{
			if (this.dataReader.LocalName == this.XSD_SCHEMA && this.dataReader.NamespaceURI == this.XSDNS)
			{
				if (this.ignoreSchema)
				{
					this.dataReader.Skip();
				}
				else if (this.isTableLevel)
				{
					this.dataTable.ReadXSDSchema(this.dataReader, false);
					this.nodeToSchemaMap = new XmlToDatasetMap(this.dataReader.NameTable, this.dataTable);
				}
				else
				{
					this.dataSet.ReadXSDSchema(this.dataReader, false);
					this.nodeToSchemaMap = new XmlToDatasetMap(this.dataReader.NameTable, this.dataSet);
				}
			}
			else
			{
				if ((this.dataReader.LocalName != this.XDR_SCHEMA || this.dataReader.NamespaceURI != this.XDRNS) && (this.dataReader.LocalName != this.SQL_SYNC || this.dataReader.NamespaceURI != this.UPDGNS))
				{
					return false;
				}
				this.dataReader.Skip();
			}
			return true;
		}

		// Token: 0x04000975 RID: 2421
		private DataSet dataSet;

		// Token: 0x04000976 RID: 2422
		private XmlToDatasetMap nodeToSchemaMap;

		// Token: 0x04000977 RID: 2423
		private Hashtable nodeToRowMap;

		// Token: 0x04000978 RID: 2424
		private Stack childRowsStack;

		// Token: 0x04000979 RID: 2425
		private Hashtable htableExcludedNS;

		// Token: 0x0400097A RID: 2426
		private bool fIsXdr;

		// Token: 0x0400097B RID: 2427
		internal bool isDiffgram;

		// Token: 0x0400097C RID: 2428
		private DataRow topMostRow;

		// Token: 0x0400097D RID: 2429
		private XmlElement topMostNode;

		// Token: 0x0400097E RID: 2430
		private bool ignoreSchema;

		// Token: 0x0400097F RID: 2431
		private DataTable dataTable;

		// Token: 0x04000980 RID: 2432
		private bool isTableLevel;

		// Token: 0x04000981 RID: 2433
		private bool fromInference;

		// Token: 0x04000982 RID: 2434
		private XmlReader dataReader;

		// Token: 0x04000983 RID: 2435
		private object XSD_XMLNS_NS;

		// Token: 0x04000984 RID: 2436
		private object XDR_SCHEMA;

		// Token: 0x04000985 RID: 2437
		private object XDRNS;

		// Token: 0x04000986 RID: 2438
		private object SQL_SYNC;

		// Token: 0x04000987 RID: 2439
		private object UPDGNS;

		// Token: 0x04000988 RID: 2440
		private object XSD_SCHEMA;

		// Token: 0x04000989 RID: 2441
		private object XSDNS;

		// Token: 0x0400098A RID: 2442
		private object DFFNS;

		// Token: 0x0400098B RID: 2443
		private object MSDNS;

		// Token: 0x0400098C RID: 2444
		private object DIFFID;

		// Token: 0x0400098D RID: 2445
		private object HASCHANGES;

		// Token: 0x0400098E RID: 2446
		private object ROWORDER;
	}
}

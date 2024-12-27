using System;
using System.Collections;
using System.Data;

namespace System.Xml
{
	// Token: 0x02000386 RID: 902
	internal sealed class DataSetMapper
	{
		// Token: 0x06002FD8 RID: 12248 RVA: 0x002B1E90 File Offset: 0x002B1290
		internal DataSetMapper()
		{
			this.tableSchemaMap = new Hashtable();
			this.columnSchemaMap = new Hashtable();
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x002B1EBC File Offset: 0x002B12BC
		internal void SetupMapping(XmlDataDocument xd, DataSet ds)
		{
			if (this.IsMapped())
			{
				this.tableSchemaMap = new Hashtable();
				this.columnSchemaMap = new Hashtable();
			}
			this.doc = xd;
			this.dataSet = ds;
			foreach (object obj in this.dataSet.Tables)
			{
				DataTable dataTable = (DataTable)obj;
				this.AddTableSchema(dataTable);
				foreach (object obj2 in dataTable.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj2;
					if (!DataSetMapper.IsNotMapped(dataColumn))
					{
						this.AddColumnSchema(dataColumn);
					}
				}
			}
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x002B1FB4 File Offset: 0x002B13B4
		internal bool IsMapped()
		{
			return this.dataSet != null;
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x002B1FD0 File Offset: 0x002B13D0
		internal DataTable SearchMatchingTableSchema(string localName, string namespaceURI)
		{
			object identity = DataSetMapper.GetIdentity(localName, namespaceURI);
			return (DataTable)this.tableSchemaMap[identity];
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x002B1FF8 File Offset: 0x002B13F8
		internal DataTable SearchMatchingTableSchema(XmlBoundElement rowElem, XmlBoundElement elem)
		{
			DataTable dataTable = this.SearchMatchingTableSchema(elem.LocalName, elem.NamespaceURI);
			if (dataTable == null)
			{
				return null;
			}
			if (rowElem == null)
			{
				return dataTable;
			}
			if (this.GetColumnSchemaForNode(rowElem, elem) == null)
			{
				return dataTable;
			}
			foreach (object obj in elem.Attributes)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)obj;
				if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					return dataTable;
				}
			}
			for (XmlNode xmlNode = elem.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					return dataTable;
				}
			}
			return null;
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x002B20BC File Offset: 0x002B14BC
		internal DataColumn GetColumnSchemaForNode(XmlBoundElement rowElem, XmlNode node)
		{
			object identity = DataSetMapper.GetIdentity(rowElem.LocalName, rowElem.NamespaceURI);
			object identity2 = DataSetMapper.GetIdentity(node.LocalName, node.NamespaceURI);
			Hashtable hashtable = (Hashtable)this.columnSchemaMap[identity];
			if (hashtable == null)
			{
				return null;
			}
			DataColumn dataColumn = (DataColumn)hashtable[identity2];
			if (dataColumn == null)
			{
				return null;
			}
			MappingType columnMapping = dataColumn.ColumnMapping;
			if (node.NodeType == XmlNodeType.Attribute && columnMapping == MappingType.Attribute)
			{
				return dataColumn;
			}
			if (node.NodeType == XmlNodeType.Element && columnMapping == MappingType.Element)
			{
				return dataColumn;
			}
			return null;
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x002B2140 File Offset: 0x002B1540
		internal DataTable GetTableSchemaForElement(XmlElement elem)
		{
			XmlBoundElement xmlBoundElement = elem as XmlBoundElement;
			if (xmlBoundElement == null)
			{
				return null;
			}
			return this.GetTableSchemaForElement(xmlBoundElement);
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x002B2160 File Offset: 0x002B1560
		internal DataTable GetTableSchemaForElement(XmlBoundElement be)
		{
			DataRow row = be.Row;
			if (row != null)
			{
				return row.Table;
			}
			return null;
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x002B2180 File Offset: 0x002B1580
		internal static bool IsNotMapped(DataColumn c)
		{
			return c.ColumnMapping == MappingType.Hidden;
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x002B2198 File Offset: 0x002B1598
		internal DataRow GetRowFromElement(XmlElement e)
		{
			XmlBoundElement xmlBoundElement = e as XmlBoundElement;
			if (xmlBoundElement != null)
			{
				return xmlBoundElement.Row;
			}
			return null;
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x002B21B8 File Offset: 0x002B15B8
		internal DataRow GetRowFromElement(XmlBoundElement be)
		{
			return be.Row;
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x002B21CC File Offset: 0x002B15CC
		internal bool GetRegion(XmlNode node, out XmlBoundElement rowElem)
		{
			while (node != null)
			{
				XmlBoundElement xmlBoundElement = node as XmlBoundElement;
				if (xmlBoundElement != null && this.GetRowFromElement(xmlBoundElement) != null)
				{
					rowElem = xmlBoundElement;
					return true;
				}
				if (node.NodeType == XmlNodeType.Attribute)
				{
					node = ((XmlAttribute)node).OwnerElement;
				}
				else
				{
					node = node.ParentNode;
				}
			}
			rowElem = null;
			return false;
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x002B221C File Offset: 0x002B161C
		internal bool IsRegionRadical(XmlBoundElement rowElem)
		{
			if (rowElem.ElementState == ElementState.Defoliated)
			{
				return true;
			}
			DataTable tableSchemaForElement = this.GetTableSchemaForElement(rowElem);
			DataColumnCollection columns = tableSchemaForElement.Columns;
			int num = 0;
			int count = rowElem.Attributes.Count;
			for (int i = 0; i < count; i++)
			{
				XmlAttribute xmlAttribute = rowElem.Attributes[i];
				if (!xmlAttribute.Specified)
				{
					return false;
				}
				DataColumn columnSchemaForNode = this.GetColumnSchemaForNode(rowElem, xmlAttribute);
				if (columnSchemaForNode == null)
				{
					return false;
				}
				if (!this.IsNextColumn(columns, ref num, columnSchemaForNode))
				{
					return false;
				}
				XmlNode firstChild = xmlAttribute.FirstChild;
				if (firstChild == null || firstChild.NodeType != XmlNodeType.Text || firstChild.NextSibling != null)
				{
					return false;
				}
			}
			num = 0;
			for (XmlNode xmlNode = rowElem.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode.NodeType != XmlNodeType.Element)
				{
					return false;
				}
				XmlElement xmlElement = xmlNode as XmlElement;
				if (this.GetRowFromElement(xmlElement) != null)
				{
					IL_0139:
					while (xmlNode != null)
					{
						if (xmlNode.NodeType != XmlNodeType.Element)
						{
							return false;
						}
						if (this.GetRowFromElement((XmlElement)xmlNode) == null)
						{
							return false;
						}
						xmlNode = xmlNode.NextSibling;
					}
					return true;
				}
				DataColumn columnSchemaForNode2 = this.GetColumnSchemaForNode(rowElem, xmlElement);
				if (columnSchemaForNode2 == null)
				{
					return false;
				}
				if (!this.IsNextColumn(columns, ref num, columnSchemaForNode2))
				{
					return false;
				}
				if (xmlElement.HasAttributes)
				{
					return false;
				}
				XmlNode firstChild2 = xmlElement.FirstChild;
				if (firstChild2 == null || firstChild2.NodeType != XmlNodeType.Text || firstChild2.NextSibling != null)
				{
					return false;
				}
			}
			goto IL_0139;
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x002B2368 File Offset: 0x002B1768
		private void AddTableSchema(DataTable table)
		{
			object identity = DataSetMapper.GetIdentity(table.EncodedTableName, table.Namespace);
			this.tableSchemaMap[identity] = table;
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x002B2394 File Offset: 0x002B1794
		private void AddColumnSchema(DataColumn col)
		{
			DataTable table = col.Table;
			object identity = DataSetMapper.GetIdentity(table.EncodedTableName, table.Namespace);
			object identity2 = DataSetMapper.GetIdentity(col.EncodedColumnName, col.Namespace);
			Hashtable hashtable = (Hashtable)this.columnSchemaMap[identity];
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				this.columnSchemaMap[identity] = hashtable;
			}
			hashtable[identity2] = col;
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x002B23FC File Offset: 0x002B17FC
		private static object GetIdentity(string localName, string namespaceURI)
		{
			return localName + ":" + namespaceURI;
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x002B2418 File Offset: 0x002B1818
		private bool IsNextColumn(DataColumnCollection columns, ref int iColumn, DataColumn col)
		{
			while (iColumn < columns.Count)
			{
				if (columns[iColumn] == col)
				{
					iColumn++;
					return true;
				}
				iColumn++;
			}
			return false;
		}

		// Token: 0x04001D92 RID: 7570
		internal const string strReservedXmlns = "http://www.w3.org/2000/xmlns/";

		// Token: 0x04001D93 RID: 7571
		private Hashtable tableSchemaMap;

		// Token: 0x04001D94 RID: 7572
		private Hashtable columnSchemaMap;

		// Token: 0x04001D95 RID: 7573
		private XmlDataDocument doc;

		// Token: 0x04001D96 RID: 7574
		private DataSet dataSet;
	}
}

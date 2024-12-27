using System;
using System.Data;
using System.Diagnostics;

namespace System.Xml
{
	// Token: 0x02000385 RID: 901
	internal sealed class DataPointer : IXmlDataVirtualNode
	{
		// Token: 0x06002FAE RID: 12206 RVA: 0x002B1134 File Offset: 0x002B0534
		internal DataPointer(XmlDataDocument doc, XmlNode node)
		{
			this.doc = doc;
			this.node = node;
			this.column = null;
			this.fOnValue = false;
			this.bNeedFoliate = false;
			this._isInUse = true;
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x002B1174 File Offset: 0x002B0574
		internal DataPointer(DataPointer pointer)
		{
			this.doc = pointer.doc;
			this.node = pointer.node;
			this.column = pointer.column;
			this.fOnValue = pointer.fOnValue;
			this.bNeedFoliate = false;
			this._isInUse = true;
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x002B11C8 File Offset: 0x002B05C8
		internal void AddPointer()
		{
			this.doc.AddPointer(this);
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x002B11E4 File Offset: 0x002B05E4
		private XmlBoundElement GetRowElement()
		{
			XmlBoundElement xmlBoundElement;
			if (this.column != null)
			{
				xmlBoundElement = this.node as XmlBoundElement;
				return xmlBoundElement;
			}
			this.doc.Mapper.GetRegion(this.node, out xmlBoundElement);
			return xmlBoundElement;
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002FB2 RID: 12210 RVA: 0x002B1224 File Offset: 0x002B0624
		private DataRow Row
		{
			get
			{
				XmlBoundElement rowElement = this.GetRowElement();
				if (rowElement == null)
				{
					return null;
				}
				return rowElement.Row;
			}
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x002B1244 File Offset: 0x002B0644
		private static bool IsFoliated(XmlNode node)
		{
			return node == null || !(node is XmlBoundElement) || ((XmlBoundElement)node).IsFoliated;
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x002B126C File Offset: 0x002B066C
		internal void MoveTo(DataPointer pointer)
		{
			this.doc = pointer.doc;
			this.node = pointer.node;
			this.column = pointer.column;
			this.fOnValue = pointer.fOnValue;
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x002B12AC File Offset: 0x002B06AC
		private void MoveTo(XmlNode node)
		{
			this.node = node;
			this.column = null;
			this.fOnValue = false;
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x002B12D0 File Offset: 0x002B06D0
		private void MoveTo(XmlNode node, DataColumn column, bool fOnValue)
		{
			this.node = node;
			this.column = column;
			this.fOnValue = fOnValue;
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x002B12F4 File Offset: 0x002B06F4
		private DataColumn NextColumn(DataRow row, DataColumn col, bool fAttribute, bool fNulls)
		{
			if (row.RowState == DataRowState.Deleted)
			{
				return null;
			}
			DataTable table = row.Table;
			DataColumnCollection columns = table.Columns;
			int i = ((col != null) ? (col.Ordinal + 1) : 0);
			int count = columns.Count;
			DataRowVersion dataRowVersion = ((row.RowState == DataRowState.Detached) ? DataRowVersion.Proposed : DataRowVersion.Current);
			while (i < count)
			{
				DataColumn dataColumn = columns[i];
				if (!this.doc.IsNotMapped(dataColumn) && dataColumn.ColumnMapping == MappingType.Attribute == fAttribute && (fNulls || !Convert.IsDBNull(row[dataColumn, dataRowVersion])))
				{
					return dataColumn;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x002B138C File Offset: 0x002B078C
		private DataColumn NthColumn(DataRow row, bool fAttribute, int iColumn, bool fNulls)
		{
			DataColumn dataColumn = null;
			checked
			{
				while ((dataColumn = this.NextColumn(row, dataColumn, fAttribute, fNulls)) != null)
				{
					if (iColumn == 0)
					{
						return dataColumn;
					}
					iColumn--;
				}
				return null;
			}
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x002B13B8 File Offset: 0x002B07B8
		private int ColumnCount(DataRow row, bool fAttribute, bool fNulls)
		{
			DataColumn dataColumn = null;
			int num = 0;
			while ((dataColumn = this.NextColumn(row, dataColumn, fAttribute, fNulls)) != null)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x002B13E0 File Offset: 0x002B07E0
		internal bool MoveToFirstChild()
		{
			this.RealFoliate();
			if (this.node == null)
			{
				return false;
			}
			if (this.column != null)
			{
				if (this.fOnValue)
				{
					return false;
				}
				this.fOnValue = true;
				return true;
			}
			else
			{
				if (!DataPointer.IsFoliated(this.node))
				{
					DataColumn dataColumn = this.NextColumn(this.Row, null, false, false);
					if (dataColumn != null)
					{
						this.MoveTo(this.node, dataColumn, this.doc.IsTextOnly(dataColumn));
						return true;
					}
				}
				XmlNode xmlNode = this.doc.SafeFirstChild(this.node);
				if (xmlNode != null)
				{
					this.MoveTo(xmlNode);
					return true;
				}
				return false;
			}
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x002B1474 File Offset: 0x002B0874
		internal bool MoveToNextSibling()
		{
			this.RealFoliate();
			if (this.node != null)
			{
				if (this.column != null)
				{
					if (this.fOnValue && !this.doc.IsTextOnly(this.column))
					{
						return false;
					}
					DataColumn dataColumn = this.NextColumn(this.Row, this.column, false, false);
					if (dataColumn != null)
					{
						this.MoveTo(this.node, dataColumn, false);
						return true;
					}
					XmlNode xmlNode = this.doc.SafeFirstChild(this.node);
					if (xmlNode != null)
					{
						this.MoveTo(xmlNode);
						return true;
					}
				}
				else
				{
					XmlNode xmlNode2 = this.doc.SafeNextSibling(this.node);
					if (xmlNode2 != null)
					{
						this.MoveTo(xmlNode2);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x002B151C File Offset: 0x002B091C
		internal bool MoveToParent()
		{
			this.RealFoliate();
			if (this.node != null)
			{
				if (this.column != null)
				{
					if (this.fOnValue && !this.doc.IsTextOnly(this.column))
					{
						this.MoveTo(this.node, this.column, false);
						return true;
					}
					if (this.column.ColumnMapping != MappingType.Attribute)
					{
						this.MoveTo(this.node, null, false);
						return true;
					}
				}
				else
				{
					XmlNode parentNode = this.node.ParentNode;
					if (parentNode != null)
					{
						this.MoveTo(parentNode);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x002B15A8 File Offset: 0x002B09A8
		internal bool MoveToOwnerElement()
		{
			this.RealFoliate();
			if (this.node != null)
			{
				if (this.column != null)
				{
					if (this.fOnValue || this.doc.IsTextOnly(this.column) || this.column.ColumnMapping != MappingType.Attribute)
					{
						return false;
					}
					this.MoveTo(this.node, null, false);
					return true;
				}
				else if (this.node.NodeType == XmlNodeType.Attribute)
				{
					XmlNode ownerElement = ((XmlAttribute)this.node).OwnerElement;
					if (ownerElement != null)
					{
						this.MoveTo(ownerElement, null, false);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002FBE RID: 12222 RVA: 0x002B1634 File Offset: 0x002B0A34
		internal int AttributeCount
		{
			get
			{
				this.RealFoliate();
				if (this.node == null || this.column != null || this.node.NodeType != XmlNodeType.Element)
				{
					return 0;
				}
				if (!DataPointer.IsFoliated(this.node))
				{
					return this.ColumnCount(this.Row, true, false);
				}
				return this.node.Attributes.Count;
			}
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x002B1694 File Offset: 0x002B0A94
		internal bool MoveToAttribute(int i)
		{
			this.RealFoliate();
			if (i < 0)
			{
				return false;
			}
			if (this.node != null && (this.column == null || this.column.ColumnMapping == MappingType.Attribute) && this.node.NodeType == XmlNodeType.Element)
			{
				if (!DataPointer.IsFoliated(this.node))
				{
					DataColumn dataColumn = this.NthColumn(this.Row, true, i, false);
					if (dataColumn != null)
					{
						this.MoveTo(this.node, dataColumn, false);
						return true;
					}
				}
				else
				{
					XmlNode xmlNode = this.node.Attributes.Item(i);
					if (xmlNode != null)
					{
						this.MoveTo(xmlNode, null, false);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002FC0 RID: 12224 RVA: 0x002B1730 File Offset: 0x002B0B30
		internal XmlNodeType NodeType
		{
			get
			{
				this.RealFoliate();
				if (this.node == null)
				{
					return XmlNodeType.None;
				}
				if (this.column == null)
				{
					return this.node.NodeType;
				}
				if (this.fOnValue)
				{
					return XmlNodeType.Text;
				}
				if (this.column.ColumnMapping == MappingType.Attribute)
				{
					return XmlNodeType.Attribute;
				}
				return XmlNodeType.Element;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002FC1 RID: 12225 RVA: 0x002B177C File Offset: 0x002B0B7C
		internal string LocalName
		{
			get
			{
				this.RealFoliate();
				if (this.node == null)
				{
					return string.Empty;
				}
				if (this.column == null)
				{
					string localName = this.node.LocalName;
					if (this.IsLocalNameEmpty(this.node.NodeType))
					{
						return string.Empty;
					}
					return localName;
				}
				else
				{
					if (this.fOnValue)
					{
						return string.Empty;
					}
					return this.doc.NameTable.Add(this.column.EncodedColumnName);
				}
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002FC2 RID: 12226 RVA: 0x002B17F8 File Offset: 0x002B0BF8
		internal string NamespaceURI
		{
			get
			{
				this.RealFoliate();
				if (this.node == null)
				{
					return string.Empty;
				}
				if (this.column == null)
				{
					return this.node.NamespaceURI;
				}
				if (this.fOnValue)
				{
					return string.Empty;
				}
				return this.doc.NameTable.Add(this.column.Namespace);
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06002FC3 RID: 12227 RVA: 0x002B1858 File Offset: 0x002B0C58
		internal string Name
		{
			get
			{
				this.RealFoliate();
				if (this.node == null)
				{
					return string.Empty;
				}
				if (this.column == null)
				{
					string name = this.node.Name;
					if (this.IsLocalNameEmpty(this.node.NodeType))
					{
						return string.Empty;
					}
					return name;
				}
				else
				{
					string prefix = this.Prefix;
					string localName = this.LocalName;
					if (prefix == null || prefix.Length <= 0)
					{
						return localName;
					}
					if (localName != null && localName.Length > 0)
					{
						return this.doc.NameTable.Add(prefix + ":" + localName);
					}
					return prefix;
				}
			}
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x002B18F0 File Offset: 0x002B0CF0
		private bool IsLocalNameEmpty(XmlNodeType nt)
		{
			switch (nt)
			{
			case XmlNodeType.None:
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.Comment:
			case XmlNodeType.Document:
			case XmlNodeType.DocumentFragment:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
			case XmlNodeType.EndElement:
			case XmlNodeType.EndEntity:
				return true;
			case XmlNodeType.Element:
			case XmlNodeType.Attribute:
			case XmlNodeType.EntityReference:
			case XmlNodeType.Entity:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.DocumentType:
			case XmlNodeType.Notation:
			case XmlNodeType.XmlDeclaration:
				return false;
			default:
				return true;
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002FC5 RID: 12229 RVA: 0x002B1954 File Offset: 0x002B0D54
		internal string Prefix
		{
			get
			{
				this.RealFoliate();
				if (this.node == null)
				{
					return string.Empty;
				}
				if (this.column == null)
				{
					return this.node.Prefix;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002FC6 RID: 12230 RVA: 0x002B1990 File Offset: 0x002B0D90
		internal string Value
		{
			get
			{
				this.RealFoliate();
				if (this.node == null)
				{
					return null;
				}
				if (this.column == null)
				{
					return this.node.Value;
				}
				if (this.column.ColumnMapping != MappingType.Attribute && !this.fOnValue)
				{
					return null;
				}
				DataRow row = this.Row;
				DataRowVersion dataRowVersion = ((row.RowState == DataRowState.Detached) ? DataRowVersion.Proposed : DataRowVersion.Current);
				object obj = row[this.column, dataRowVersion];
				if (!Convert.IsDBNull(obj))
				{
					return this.column.ConvertObjectToXml(obj);
				}
				return null;
			}
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x002B1A1C File Offset: 0x002B0E1C
		bool IXmlDataVirtualNode.IsOnNode(XmlNode nodeToCheck)
		{
			this.RealFoliate();
			return nodeToCheck == this.node;
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x002B1A38 File Offset: 0x002B0E38
		bool IXmlDataVirtualNode.IsOnColumn(DataColumn col)
		{
			this.RealFoliate();
			return col == this.column;
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x002B1A54 File Offset: 0x002B0E54
		internal XmlNode GetNode()
		{
			return this.node;
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002FCA RID: 12234 RVA: 0x002B1A68 File Offset: 0x002B0E68
		internal bool IsEmptyElement
		{
			get
			{
				this.RealFoliate();
				return this.node != null && this.column == null && this.node.NodeType == XmlNodeType.Element && ((XmlElement)this.node).IsEmpty;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002FCB RID: 12235 RVA: 0x002B1AAC File Offset: 0x002B0EAC
		internal bool IsDefault
		{
			get
			{
				this.RealFoliate();
				return this.node != null && this.column == null && this.node.NodeType == XmlNodeType.Attribute && !((XmlAttribute)this.node).Specified;
			}
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x002B1AF4 File Offset: 0x002B0EF4
		void IXmlDataVirtualNode.OnFoliated(XmlNode foliatedNode)
		{
			if (this.node == foliatedNode)
			{
				if (this.column == null)
				{
					return;
				}
				this.bNeedFoliate = true;
			}
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x002B1B1C File Offset: 0x002B0F1C
		internal void RealFoliate()
		{
			if (!this.bNeedFoliate)
			{
				return;
			}
			XmlNode xmlNode;
			if (this.doc.IsTextOnly(this.column))
			{
				xmlNode = this.node.FirstChild;
			}
			else
			{
				if (this.column.ColumnMapping == MappingType.Attribute)
				{
					xmlNode = this.node.Attributes.GetNamedItem(this.column.EncodedColumnName, this.column.Namespace);
				}
				else
				{
					xmlNode = this.node.FirstChild;
					while (xmlNode != null && (!(xmlNode.LocalName == this.column.EncodedColumnName) || !(xmlNode.NamespaceURI == this.column.Namespace)))
					{
						xmlNode = xmlNode.NextSibling;
					}
				}
				if (xmlNode != null && this.fOnValue)
				{
					xmlNode = xmlNode.FirstChild;
				}
			}
			if (xmlNode == null)
			{
				throw new InvalidOperationException(Res.GetString("DataDom_Foliation"));
			}
			this.node = xmlNode;
			this.column = null;
			this.fOnValue = false;
			this.bNeedFoliate = false;
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06002FCE RID: 12238 RVA: 0x002B1C18 File Offset: 0x002B1018
		internal string PublicId
		{
			get
			{
				XmlNodeType nodeType = this.NodeType;
				XmlNodeType xmlNodeType = nodeType;
				if (xmlNodeType != XmlNodeType.Entity)
				{
					switch (xmlNodeType)
					{
					case XmlNodeType.DocumentType:
						return ((XmlDocumentType)this.node).PublicId;
					case XmlNodeType.Notation:
						return ((XmlNotation)this.node).PublicId;
					}
					return null;
				}
				return ((XmlEntity)this.node).PublicId;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06002FCF RID: 12239 RVA: 0x002B1C80 File Offset: 0x002B1080
		internal string SystemId
		{
			get
			{
				XmlNodeType nodeType = this.NodeType;
				XmlNodeType xmlNodeType = nodeType;
				if (xmlNodeType != XmlNodeType.Entity)
				{
					switch (xmlNodeType)
					{
					case XmlNodeType.DocumentType:
						return ((XmlDocumentType)this.node).SystemId;
					case XmlNodeType.Notation:
						return ((XmlNotation)this.node).SystemId;
					}
					return null;
				}
				return ((XmlEntity)this.node).SystemId;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06002FD0 RID: 12240 RVA: 0x002B1CE8 File Offset: 0x002B10E8
		internal string InternalSubset
		{
			get
			{
				if (this.NodeType == XmlNodeType.DocumentType)
				{
					return ((XmlDocumentType)this.node).InternalSubset;
				}
				return null;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06002FD1 RID: 12241 RVA: 0x002B1D14 File Offset: 0x002B1114
		internal XmlDeclaration Declaration
		{
			get
			{
				XmlNode xmlNode = this.doc.SafeFirstChild(this.doc);
				if (xmlNode != null && xmlNode.NodeType == XmlNodeType.XmlDeclaration)
				{
					return (XmlDeclaration)xmlNode;
				}
				return null;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06002FD2 RID: 12242 RVA: 0x002B1D48 File Offset: 0x002B1148
		internal string Encoding
		{
			get
			{
				if (this.NodeType == XmlNodeType.XmlDeclaration)
				{
					return ((XmlDeclaration)this.node).Encoding;
				}
				if (this.NodeType == XmlNodeType.Document)
				{
					XmlDeclaration declaration = this.Declaration;
					if (declaration != null)
					{
						return declaration.Encoding;
					}
				}
				return null;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06002FD3 RID: 12243 RVA: 0x002B1D8C File Offset: 0x002B118C
		internal string Standalone
		{
			get
			{
				if (this.NodeType == XmlNodeType.XmlDeclaration)
				{
					return ((XmlDeclaration)this.node).Standalone;
				}
				if (this.NodeType == XmlNodeType.Document)
				{
					XmlDeclaration declaration = this.Declaration;
					if (declaration != null)
					{
						return declaration.Standalone;
					}
				}
				return null;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06002FD4 RID: 12244 RVA: 0x002B1DD0 File Offset: 0x002B11D0
		internal string Version
		{
			get
			{
				if (this.NodeType == XmlNodeType.XmlDeclaration)
				{
					return ((XmlDeclaration)this.node).Version;
				}
				if (this.NodeType == XmlNodeType.Document)
				{
					XmlDeclaration declaration = this.Declaration;
					if (declaration != null)
					{
						return declaration.Version;
					}
				}
				return null;
			}
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x002B1E14 File Offset: 0x002B1214
		[Conditional("DEBUG")]
		private void AssertValid()
		{
			if (this.column != null)
			{
				XmlBoundElement xmlBoundElement = this.node as XmlBoundElement;
				DataRow row = xmlBoundElement.Row;
				ElementState elementState = xmlBoundElement.ElementState;
				DataRowState rowState = row.RowState;
			}
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x002B1E4C File Offset: 0x002B124C
		bool IXmlDataVirtualNode.IsInUse()
		{
			return this._isInUse;
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x002B1E60 File Offset: 0x002B1260
		internal void SetNoLongerUse()
		{
			this.node = null;
			this.column = null;
			this.fOnValue = false;
			this.bNeedFoliate = false;
			this._isInUse = false;
		}

		// Token: 0x04001D8C RID: 7564
		private XmlDataDocument doc;

		// Token: 0x04001D8D RID: 7565
		private XmlNode node;

		// Token: 0x04001D8E RID: 7566
		private DataColumn column;

		// Token: 0x04001D8F RID: 7567
		private bool fOnValue;

		// Token: 0x04001D90 RID: 7568
		private bool bNeedFoliate;

		// Token: 0x04001D91 RID: 7569
		private bool _isInUse;
	}
}

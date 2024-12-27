using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Xml
{
	// Token: 0x020000CB RID: 203
	public sealed class XmlAttributeCollection : XmlNamedNodeMap, ICollection, IEnumerable
	{
		// Token: 0x06000C1A RID: 3098 RVA: 0x000371A9 File Offset: 0x000361A9
		internal XmlAttributeCollection(XmlNode parent)
			: base(parent)
		{
		}

		// Token: 0x170002B7 RID: 695
		[IndexerName("ItemOf")]
		public XmlAttribute this[int i]
		{
			get
			{
				XmlAttribute xmlAttribute;
				try
				{
					xmlAttribute = (XmlAttribute)base.Nodes[i];
				}
				catch (ArgumentOutOfRangeException)
				{
					throw new IndexOutOfRangeException(Res.GetString("Xdom_IndexOutOfRange"));
				}
				return xmlAttribute;
			}
		}

		// Token: 0x170002B8 RID: 696
		[IndexerName("ItemOf")]
		public XmlAttribute this[string name]
		{
			get
			{
				ArrayList nodes = base.Nodes;
				int hashCode = XmlName.GetHashCode(name);
				for (int i = 0; i < nodes.Count; i++)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)nodes[i];
					if (hashCode == xmlAttribute.LocalNameHash && name == xmlAttribute.Name)
					{
						return xmlAttribute;
					}
				}
				return null;
			}
		}

		// Token: 0x170002B9 RID: 697
		[IndexerName("ItemOf")]
		public XmlAttribute this[string localName, string namespaceURI]
		{
			get
			{
				ArrayList nodes = base.Nodes;
				int hashCode = XmlName.GetHashCode(localName);
				for (int i = 0; i < nodes.Count; i++)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)nodes[i];
					if (hashCode == xmlAttribute.LocalNameHash && localName == xmlAttribute.LocalName && namespaceURI == xmlAttribute.NamespaceURI)
					{
						return xmlAttribute;
					}
				}
				return null;
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x000372B0 File Offset: 0x000362B0
		internal int FindNodeOffset(XmlAttribute node)
		{
			ArrayList nodes = base.Nodes;
			for (int i = 0; i < nodes.Count; i++)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)nodes[i];
				if (xmlAttribute.LocalNameHash == node.LocalNameHash && xmlAttribute.Name == node.Name && xmlAttribute.NamespaceURI == node.NamespaceURI)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0003731C File Offset: 0x0003631C
		internal int FindNodeOffsetNS(XmlAttribute node)
		{
			ArrayList nodes = base.Nodes;
			for (int i = 0; i < nodes.Count; i++)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)nodes[i];
				if (xmlAttribute.LocalNameHash == node.LocalNameHash && xmlAttribute.LocalName == node.LocalName && xmlAttribute.NamespaceURI == node.NamespaceURI)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00037388 File Offset: 0x00036388
		public override XmlNode SetNamedItem(XmlNode node)
		{
			if (node != null && !(node is XmlAttribute))
			{
				throw new ArgumentException(Res.GetString("Xdom_AttrCol_Object"));
			}
			int num = base.FindNodeOffset(node.LocalName, node.NamespaceURI);
			if (num == -1)
			{
				return this.InternalAppendAttribute((XmlAttribute)node);
			}
			XmlNode xmlNode = base.RemoveNodeAt(num);
			this.InsertNodeAt(num, node);
			return xmlNode;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x000373E8 File Offset: 0x000363E8
		public XmlAttribute Prepend(XmlAttribute node)
		{
			if (node.OwnerDocument != null && node.OwnerDocument != this.parent.OwnerDocument)
			{
				throw new ArgumentException(Res.GetString("Xdom_NamedNode_Context"));
			}
			if (node.OwnerElement != null)
			{
				this.Detach(node);
			}
			this.RemoveDuplicateAttribute(node);
			this.InsertNodeAt(0, node);
			return node;
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00037444 File Offset: 0x00036444
		public XmlAttribute Append(XmlAttribute node)
		{
			XmlDocument ownerDocument = node.OwnerDocument;
			if (ownerDocument == null || !ownerDocument.IsLoading)
			{
				if (ownerDocument != null && ownerDocument != this.parent.OwnerDocument)
				{
					throw new ArgumentException(Res.GetString("Xdom_NamedNode_Context"));
				}
				if (node.OwnerElement != null)
				{
					this.Detach(node);
				}
				this.AddNode(node);
			}
			else
			{
				base.AddNodeForLoad(node, ownerDocument);
				this.InsertParentIntoElementIdAttrMap(node);
			}
			return node;
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x000374B0 File Offset: 0x000364B0
		public XmlAttribute InsertBefore(XmlAttribute newNode, XmlAttribute refNode)
		{
			if (newNode == refNode)
			{
				return newNode;
			}
			if (refNode == null)
			{
				return this.Append(newNode);
			}
			if (refNode.OwnerElement != this.parent)
			{
				throw new ArgumentException(Res.GetString("Xdom_AttrCol_Insert"));
			}
			if (newNode.OwnerDocument != null && newNode.OwnerDocument != this.parent.OwnerDocument)
			{
				throw new ArgumentException(Res.GetString("Xdom_NamedNode_Context"));
			}
			if (newNode.OwnerElement != null)
			{
				this.Detach(newNode);
			}
			int num = base.FindNodeOffset(refNode.LocalName, refNode.NamespaceURI);
			int num2 = this.RemoveDuplicateAttribute(newNode);
			if (num2 >= 0 && num2 < num)
			{
				num--;
			}
			this.InsertNodeAt(num, newNode);
			return newNode;
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00037558 File Offset: 0x00036558
		public XmlAttribute InsertAfter(XmlAttribute newNode, XmlAttribute refNode)
		{
			if (newNode == refNode)
			{
				return newNode;
			}
			if (refNode == null)
			{
				return this.Prepend(newNode);
			}
			if (refNode.OwnerElement != this.parent)
			{
				throw new ArgumentException(Res.GetString("Xdom_AttrCol_Insert"));
			}
			if (newNode.OwnerDocument != null && newNode.OwnerDocument != this.parent.OwnerDocument)
			{
				throw new ArgumentException(Res.GetString("Xdom_NamedNode_Context"));
			}
			if (newNode.OwnerElement != null)
			{
				this.Detach(newNode);
			}
			int num = base.FindNodeOffset(refNode.LocalName, refNode.NamespaceURI);
			int num2 = this.RemoveDuplicateAttribute(newNode);
			if (num2 >= 0 && num2 < num)
			{
				num--;
			}
			this.InsertNodeAt(num + 1, newNode);
			return newNode;
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00037604 File Offset: 0x00036604
		public XmlAttribute Remove(XmlAttribute node)
		{
			if (this.nodes != null)
			{
				int count = this.nodes.Count;
				for (int i = 0; i < count; i++)
				{
					if (this.nodes[i] == node)
					{
						this.RemoveNodeAt(i);
						return node;
					}
				}
			}
			return null;
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0003764B File Offset: 0x0003664B
		public XmlAttribute RemoveAt(int i)
		{
			if (i < 0 || i >= this.Count || this.nodes == null)
			{
				return null;
			}
			return (XmlAttribute)this.RemoveNodeAt(i);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00037670 File Offset: 0x00036670
		public void RemoveAll()
		{
			int i = this.Count;
			while (i > 0)
			{
				i--;
				this.RemoveAt(i);
			}
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00037698 File Offset: 0x00036698
		void ICollection.CopyTo(Array array, int index)
		{
			int i = 0;
			int count = base.Nodes.Count;
			while (i < count)
			{
				array.SetValue(this.nodes[i], index);
				i++;
				index++;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x000376D5 File Offset: 0x000366D5
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x000376D8 File Offset: 0x000366D8
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x000376DB File Offset: 0x000366DB
		int ICollection.Count
		{
			get
			{
				return base.Count;
			}
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x000376E4 File Offset: 0x000366E4
		public void CopyTo(XmlAttribute[] array, int index)
		{
			int i = 0;
			int count = this.Count;
			while (i < count)
			{
				array[index] = (XmlAttribute)((XmlNode)this.nodes[i]).CloneNode(true);
				i++;
				index++;
			}
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00037728 File Offset: 0x00036728
		internal override XmlNode AddNode(XmlNode node)
		{
			this.RemoveDuplicateAttribute((XmlAttribute)node);
			XmlNode xmlNode = base.AddNode(node);
			this.InsertParentIntoElementIdAttrMap((XmlAttribute)node);
			return xmlNode;
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00037758 File Offset: 0x00036758
		internal override XmlNode InsertNodeAt(int i, XmlNode node)
		{
			XmlNode xmlNode = base.InsertNodeAt(i, node);
			this.InsertParentIntoElementIdAttrMap((XmlAttribute)node);
			return xmlNode;
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x0003777C File Offset: 0x0003677C
		internal override XmlNode RemoveNodeAt(int i)
		{
			XmlNode xmlNode = base.RemoveNodeAt(i);
			this.RemoveParentFromElementIdAttrMap((XmlAttribute)xmlNode);
			XmlAttribute defaultAttribute = this.parent.OwnerDocument.GetDefaultAttribute((XmlElement)this.parent, xmlNode.Prefix, xmlNode.LocalName, xmlNode.NamespaceURI);
			if (defaultAttribute != null)
			{
				this.InsertNodeAt(i, defaultAttribute);
			}
			return xmlNode;
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x000377D8 File Offset: 0x000367D8
		internal void Detach(XmlAttribute attr)
		{
			attr.OwnerElement.Attributes.Remove(attr);
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x000377EC File Offset: 0x000367EC
		internal void InsertParentIntoElementIdAttrMap(XmlAttribute attr)
		{
			XmlElement xmlElement = this.parent as XmlElement;
			if (xmlElement != null)
			{
				if (this.parent.OwnerDocument == null)
				{
					return;
				}
				XmlName idinfoByElement = this.parent.OwnerDocument.GetIDInfoByElement(xmlElement.XmlName);
				if (idinfoByElement != null && idinfoByElement.Prefix == attr.XmlName.Prefix && idinfoByElement.LocalName == attr.XmlName.LocalName)
				{
					this.parent.OwnerDocument.AddElementWithId(attr.Value, xmlElement);
				}
			}
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00037878 File Offset: 0x00036878
		internal void RemoveParentFromElementIdAttrMap(XmlAttribute attr)
		{
			XmlElement xmlElement = this.parent as XmlElement;
			if (xmlElement != null)
			{
				if (this.parent.OwnerDocument == null)
				{
					return;
				}
				XmlName idinfoByElement = this.parent.OwnerDocument.GetIDInfoByElement(xmlElement.XmlName);
				if (idinfoByElement != null && idinfoByElement.Prefix == attr.XmlName.Prefix && idinfoByElement.LocalName == attr.XmlName.LocalName)
				{
					this.parent.OwnerDocument.RemoveElementWithId(attr.Value, xmlElement);
				}
			}
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00037904 File Offset: 0x00036904
		internal int RemoveDuplicateAttribute(XmlAttribute attr)
		{
			int num = base.FindNodeOffset(attr.LocalName, attr.NamespaceURI);
			if (num != -1)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Nodes[num];
				base.RemoveNodeAt(num);
				this.RemoveParentFromElementIdAttrMap(xmlAttribute);
			}
			return num;
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x0003794C File Offset: 0x0003694C
		internal bool PrepareParentInElementIdAttrMap(string attrPrefix, string attrLocalName)
		{
			XmlElement xmlElement = this.parent as XmlElement;
			XmlDocument ownerDocument = this.parent.OwnerDocument;
			XmlName idinfoByElement = ownerDocument.GetIDInfoByElement(xmlElement.XmlName);
			return idinfoByElement != null && idinfoByElement.Prefix == attrPrefix && idinfoByElement.LocalName == attrLocalName;
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x000379A0 File Offset: 0x000369A0
		internal void ResetParentInElementIdAttrMap(string oldVal, string newVal)
		{
			XmlElement xmlElement = this.parent as XmlElement;
			XmlDocument ownerDocument = this.parent.OwnerDocument;
			ownerDocument.RemoveElementWithId(oldVal, xmlElement);
			ownerDocument.AddElementWithId(newVal, xmlElement);
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x000379D8 File Offset: 0x000369D8
		internal XmlAttribute InternalAppendAttribute(XmlAttribute node)
		{
			XmlNode xmlNode = base.AddNode(node);
			this.InsertParentIntoElementIdAttrMap(node);
			return (XmlAttribute)xmlNode;
		}
	}
}

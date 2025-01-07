using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Xml
{
	public sealed class XmlAttributeCollection : XmlNamedNodeMap, ICollection, IEnumerable
	{
		internal XmlAttributeCollection(XmlNode parent)
			: base(parent)
		{
		}

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

		public XmlAttribute RemoveAt(int i)
		{
			if (i < 0 || i >= this.Count || this.nodes == null)
			{
				return null;
			}
			return (XmlAttribute)this.RemoveNodeAt(i);
		}

		public void RemoveAll()
		{
			int i = this.Count;
			while (i > 0)
			{
				i--;
				this.RemoveAt(i);
			}
		}

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

		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		int ICollection.Count
		{
			get
			{
				return base.Count;
			}
		}

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

		internal override XmlNode AddNode(XmlNode node)
		{
			this.RemoveDuplicateAttribute((XmlAttribute)node);
			XmlNode xmlNode = base.AddNode(node);
			this.InsertParentIntoElementIdAttrMap((XmlAttribute)node);
			return xmlNode;
		}

		internal override XmlNode InsertNodeAt(int i, XmlNode node)
		{
			XmlNode xmlNode = base.InsertNodeAt(i, node);
			this.InsertParentIntoElementIdAttrMap((XmlAttribute)node);
			return xmlNode;
		}

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

		internal void Detach(XmlAttribute attr)
		{
			attr.OwnerElement.Attributes.Remove(attr);
		}

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

		internal bool PrepareParentInElementIdAttrMap(string attrPrefix, string attrLocalName)
		{
			XmlElement xmlElement = this.parent as XmlElement;
			XmlDocument ownerDocument = this.parent.OwnerDocument;
			XmlName idinfoByElement = ownerDocument.GetIDInfoByElement(xmlElement.XmlName);
			return idinfoByElement != null && idinfoByElement.Prefix == attrPrefix && idinfoByElement.LocalName == attrLocalName;
		}

		internal void ResetParentInElementIdAttrMap(string oldVal, string newVal)
		{
			XmlElement xmlElement = this.parent as XmlElement;
			XmlDocument ownerDocument = this.parent.OwnerDocument;
			ownerDocument.RemoveElementWithId(oldVal, xmlElement);
			ownerDocument.AddElementWithId(newVal, xmlElement);
		}

		internal XmlAttribute InternalAppendAttribute(XmlAttribute node)
		{
			XmlNode xmlNode = base.AddNode(node);
			this.InsertParentIntoElementIdAttrMap(node);
			return (XmlAttribute)xmlNode;
		}
	}
}

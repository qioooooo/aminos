using System;
using System.Collections;

namespace System.Xml
{
	// Token: 0x020000CA RID: 202
	public class XmlNamedNodeMap : IEnumerable
	{
		// Token: 0x06000C09 RID: 3081 RVA: 0x00036D91 File Offset: 0x00035D91
		internal XmlNamedNodeMap(XmlNode parent)
		{
			this.parent = parent;
			this.nodes = null;
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00036DA8 File Offset: 0x00035DA8
		public virtual XmlNode GetNamedItem(string name)
		{
			int num = this.FindNodeOffset(name);
			if (num >= 0)
			{
				return (XmlNode)this.Nodes[num];
			}
			return null;
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00036DD4 File Offset: 0x00035DD4
		public virtual XmlNode SetNamedItem(XmlNode node)
		{
			if (node == null)
			{
				return null;
			}
			int num = this.FindNodeOffset(node.LocalName, node.NamespaceURI);
			if (num == -1)
			{
				this.AddNode(node);
				return null;
			}
			return this.ReplaceNodeAt(num, node);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00036E10 File Offset: 0x00035E10
		public virtual XmlNode RemoveNamedItem(string name)
		{
			int num = this.FindNodeOffset(name);
			if (num >= 0)
			{
				return this.RemoveNodeAt(num);
			}
			return null;
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000C0D RID: 3085 RVA: 0x00036E32 File Offset: 0x00035E32
		public virtual int Count
		{
			get
			{
				if (this.nodes != null)
				{
					return this.nodes.Count;
				}
				return 0;
			}
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x00036E4C File Offset: 0x00035E4C
		public virtual XmlNode Item(int index)
		{
			if (index < 0 || index >= this.Nodes.Count)
			{
				return null;
			}
			XmlNode xmlNode;
			try
			{
				xmlNode = (XmlNode)this.Nodes[index];
			}
			catch (ArgumentOutOfRangeException)
			{
				throw new IndexOutOfRangeException(Res.GetString("Xdom_IndexOutOfRange"));
			}
			return xmlNode;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00036EA4 File Offset: 0x00035EA4
		public virtual XmlNode GetNamedItem(string localName, string namespaceURI)
		{
			int num = this.FindNodeOffset(localName, namespaceURI);
			if (num >= 0)
			{
				return (XmlNode)this.Nodes[num];
			}
			return null;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00036ED4 File Offset: 0x00035ED4
		public virtual XmlNode RemoveNamedItem(string localName, string namespaceURI)
		{
			int num = this.FindNodeOffset(localName, namespaceURI);
			if (num >= 0)
			{
				return this.RemoveNodeAt(num);
			}
			return null;
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x00036EF7 File Offset: 0x00035EF7
		internal ArrayList Nodes
		{
			get
			{
				if (this.nodes == null)
				{
					this.nodes = new ArrayList();
				}
				return this.nodes;
			}
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00036F12 File Offset: 0x00035F12
		public virtual IEnumerator GetEnumerator()
		{
			if (this.nodes == null)
			{
				return XmlDocument.EmptyEnumerator;
			}
			return this.Nodes.GetEnumerator();
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00036F30 File Offset: 0x00035F30
		internal int FindNodeOffset(string name)
		{
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				XmlNode xmlNode = (XmlNode)this.Nodes[i];
				if (name == xmlNode.Name)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00036F74 File Offset: 0x00035F74
		internal int FindNodeOffset(string localName, string namespaceURI)
		{
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				XmlNode xmlNode = (XmlNode)this.Nodes[i];
				if (xmlNode.LocalName == localName && xmlNode.NamespaceURI == namespaceURI)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00036FC8 File Offset: 0x00035FC8
		internal virtual XmlNode AddNode(XmlNode node)
		{
			XmlNode xmlNode;
			if (node.NodeType == XmlNodeType.Attribute)
			{
				xmlNode = ((XmlAttribute)node).OwnerElement;
			}
			else
			{
				xmlNode = node.ParentNode;
			}
			string value = node.Value;
			XmlNodeChangedEventArgs eventArgs = this.parent.GetEventArgs(node, xmlNode, this.parent, value, value, XmlNodeChangedAction.Insert);
			if (eventArgs != null)
			{
				this.parent.BeforeEvent(eventArgs);
			}
			this.Nodes.Add(node);
			node.SetParent(this.parent);
			if (eventArgs != null)
			{
				this.parent.AfterEvent(eventArgs);
			}
			return node;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0003704C File Offset: 0x0003604C
		internal virtual XmlNode AddNodeForLoad(XmlNode node, XmlDocument doc)
		{
			XmlNodeChangedEventArgs insertEventArgsForLoad = doc.GetInsertEventArgsForLoad(node, this.parent);
			if (insertEventArgsForLoad != null)
			{
				doc.BeforeEvent(insertEventArgsForLoad);
			}
			this.Nodes.Add(node);
			node.SetParent(this.parent);
			if (insertEventArgsForLoad != null)
			{
				doc.AfterEvent(insertEventArgsForLoad);
			}
			return node;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00037098 File Offset: 0x00036098
		internal virtual XmlNode RemoveNodeAt(int i)
		{
			XmlNode xmlNode = (XmlNode)this.Nodes[i];
			string value = xmlNode.Value;
			XmlNodeChangedEventArgs eventArgs = this.parent.GetEventArgs(xmlNode, this.parent, null, value, value, XmlNodeChangedAction.Remove);
			if (eventArgs != null)
			{
				this.parent.BeforeEvent(eventArgs);
			}
			this.Nodes.RemoveAt(i);
			xmlNode.SetParent(null);
			if (eventArgs != null)
			{
				this.parent.AfterEvent(eventArgs);
			}
			return xmlNode;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00037108 File Offset: 0x00036108
		internal XmlNode ReplaceNodeAt(int i, XmlNode node)
		{
			XmlNode xmlNode = this.RemoveNodeAt(i);
			this.InsertNodeAt(i, node);
			return xmlNode;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00037128 File Offset: 0x00036128
		internal virtual XmlNode InsertNodeAt(int i, XmlNode node)
		{
			XmlNode xmlNode;
			if (node.NodeType == XmlNodeType.Attribute)
			{
				xmlNode = ((XmlAttribute)node).OwnerElement;
			}
			else
			{
				xmlNode = node.ParentNode;
			}
			string value = node.Value;
			XmlNodeChangedEventArgs eventArgs = this.parent.GetEventArgs(node, xmlNode, this.parent, value, value, XmlNodeChangedAction.Insert);
			if (eventArgs != null)
			{
				this.parent.BeforeEvent(eventArgs);
			}
			this.Nodes.Insert(i, node);
			node.SetParent(this.parent);
			if (eventArgs != null)
			{
				this.parent.AfterEvent(eventArgs);
			}
			return node;
		}

		// Token: 0x040008EE RID: 2286
		internal XmlNode parent;

		// Token: 0x040008EF RID: 2287
		internal ArrayList nodes;
	}
}

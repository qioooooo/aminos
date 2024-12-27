using System;
using System.Text;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000CD RID: 205
	public abstract class XmlCharacterData : XmlLinkedNode
	{
		// Token: 0x06000C3B RID: 3131 RVA: 0x00037A7D File Offset: 0x00036A7D
		protected internal XmlCharacterData(string data, XmlDocument doc)
			: base(doc)
		{
			this.data = data;
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x00037A8D File Offset: 0x00036A8D
		// (set) Token: 0x06000C3D RID: 3133 RVA: 0x00037A95 File Offset: 0x00036A95
		public override string Value
		{
			get
			{
				return this.Data;
			}
			set
			{
				this.Data = value;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x00037A9E File Offset: 0x00036A9E
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x00037AA6 File Offset: 0x00036AA6
		public override string InnerText
		{
			get
			{
				return this.Value;
			}
			set
			{
				this.Value = value;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x00037AAF File Offset: 0x00036AAF
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x00037AC8 File Offset: 0x00036AC8
		public virtual string Data
		{
			get
			{
				if (this.data != null)
				{
					return this.data;
				}
				return string.Empty;
			}
			set
			{
				XmlNode parentNode = this.ParentNode;
				XmlNodeChangedEventArgs eventArgs = this.GetEventArgs(this, parentNode, parentNode, this.data, value, XmlNodeChangedAction.Change);
				if (eventArgs != null)
				{
					this.BeforeEvent(eventArgs);
				}
				this.data = value;
				if (eventArgs != null)
				{
					this.AfterEvent(eventArgs);
				}
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x00037B09 File Offset: 0x00036B09
		public virtual int Length
		{
			get
			{
				if (this.data != null)
				{
					return this.data.Length;
				}
				return 0;
			}
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00037B20 File Offset: 0x00036B20
		public virtual string Substring(int offset, int count)
		{
			int num = ((this.data != null) ? this.data.Length : 0);
			if (num > 0)
			{
				if (num < offset + count)
				{
					count = num - offset;
				}
				return this.data.Substring(offset, count);
			}
			return string.Empty;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00037B68 File Offset: 0x00036B68
		public virtual void AppendData(string strData)
		{
			XmlNode parentNode = this.ParentNode;
			int num = ((this.data != null) ? this.data.Length : 0);
			if (strData != null)
			{
				num += strData.Length;
			}
			string text = new StringBuilder(num).Append(this.data).Append(strData).ToString();
			XmlNodeChangedEventArgs eventArgs = this.GetEventArgs(this, parentNode, parentNode, this.data, text, XmlNodeChangedAction.Change);
			if (eventArgs != null)
			{
				this.BeforeEvent(eventArgs);
			}
			this.data = text;
			if (eventArgs != null)
			{
				this.AfterEvent(eventArgs);
			}
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00037BEC File Offset: 0x00036BEC
		public virtual void InsertData(int offset, string strData)
		{
			XmlNode parentNode = this.ParentNode;
			int num = ((this.data != null) ? this.data.Length : 0);
			if (strData != null)
			{
				num += strData.Length;
			}
			string text = new StringBuilder(num).Append(this.data).Insert(offset, strData).ToString();
			XmlNodeChangedEventArgs eventArgs = this.GetEventArgs(this, parentNode, parentNode, this.data, text, XmlNodeChangedAction.Change);
			if (eventArgs != null)
			{
				this.BeforeEvent(eventArgs);
			}
			this.data = text;
			if (eventArgs != null)
			{
				this.AfterEvent(eventArgs);
			}
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00037C70 File Offset: 0x00036C70
		public virtual void DeleteData(int offset, int count)
		{
			int num = ((this.data != null) ? this.data.Length : 0);
			if (num > 0 && num < offset + count)
			{
				count = Math.Max(num - offset, 0);
			}
			string text = new StringBuilder(this.data).Remove(offset, count).ToString();
			XmlNode parentNode = this.ParentNode;
			XmlNodeChangedEventArgs eventArgs = this.GetEventArgs(this, parentNode, parentNode, this.data, text, XmlNodeChangedAction.Change);
			if (eventArgs != null)
			{
				this.BeforeEvent(eventArgs);
			}
			this.data = text;
			if (eventArgs != null)
			{
				this.AfterEvent(eventArgs);
			}
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00037CF8 File Offset: 0x00036CF8
		public virtual void ReplaceData(int offset, int count, string strData)
		{
			int num = ((this.data != null) ? this.data.Length : 0);
			if (num > 0 && num < offset + count)
			{
				count = Math.Max(num - offset, 0);
			}
			StringBuilder stringBuilder = new StringBuilder(this.data).Remove(offset, count);
			string text = stringBuilder.Insert(offset, strData).ToString();
			XmlNode parentNode = this.ParentNode;
			XmlNodeChangedEventArgs eventArgs = this.GetEventArgs(this, parentNode, parentNode, this.data, text, XmlNodeChangedAction.Change);
			if (eventArgs != null)
			{
				this.BeforeEvent(eventArgs);
			}
			this.data = text;
			if (eventArgs != null)
			{
				this.AfterEvent(eventArgs);
			}
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00037D8C File Offset: 0x00036D8C
		internal bool CheckOnData(string data)
		{
			return XmlCharType.Instance.IsOnlyWhitespace(data);
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00037DA8 File Offset: 0x00036DA8
		internal bool DecideXPNodeTypeForTextNodes(XmlNode node, ref XPathNodeType xnt)
		{
			while (node != null)
			{
				XmlNodeType nodeType = node.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					xnt = XPathNodeType.Text;
					return false;
				case XmlNodeType.EntityReference:
					if (!this.DecideXPNodeTypeForTextNodes(node.FirstChild, ref xnt))
					{
						return false;
					}
					break;
				default:
					switch (nodeType)
					{
					case XmlNodeType.Whitespace:
						break;
					case XmlNodeType.SignificantWhitespace:
						xnt = XPathNodeType.SignificantWhitespace;
						break;
					default:
						return false;
					}
					break;
				}
				node = node.NextSibling;
			}
			return true;
		}

		// Token: 0x040008F1 RID: 2289
		private string data;
	}
}

using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000D2 RID: 210
	public class XmlComment : XmlCharacterData
	{
		// Token: 0x06000C64 RID: 3172 RVA: 0x00038050 File Offset: 0x00037050
		protected internal XmlComment(string comment, XmlDocument doc)
			: base(comment, doc)
		{
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x0003805A File Offset: 0x0003705A
		public override string Name
		{
			get
			{
				return this.OwnerDocument.strCommentName;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x00038067 File Offset: 0x00037067
		public override string LocalName
		{
			get
			{
				return this.OwnerDocument.strCommentName;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x00038074 File Offset: 0x00037074
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Comment;
			}
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00038077 File Offset: 0x00037077
		public override XmlNode CloneNode(bool deep)
		{
			return this.OwnerDocument.CreateComment(this.Data);
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0003808A File Offset: 0x0003708A
		public override void WriteTo(XmlWriter w)
		{
			w.WriteComment(this.Data);
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00038098 File Offset: 0x00037098
		public override void WriteContentTo(XmlWriter w)
		{
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x0003809A File Offset: 0x0003709A
		internal override XPathNodeType XPNodeType
		{
			get
			{
				return XPathNodeType.Comment;
			}
		}
	}
}

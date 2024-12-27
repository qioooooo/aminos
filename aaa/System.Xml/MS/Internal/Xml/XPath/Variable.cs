using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000160 RID: 352
	internal class Variable : AstNode
	{
		// Token: 0x0600131A RID: 4890 RVA: 0x00052FDD File Offset: 0x00051FDD
		public Variable(string name, string prefix)
		{
			this.localname = name;
			this.prefix = prefix;
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x0600131B RID: 4891 RVA: 0x00052FF3 File Offset: 0x00051FF3
		public override AstNode.AstType Type
		{
			get
			{
				return AstNode.AstType.Variable;
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x0600131C RID: 4892 RVA: 0x00052FF6 File Offset: 0x00051FF6
		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Any;
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x0600131D RID: 4893 RVA: 0x00052FF9 File Offset: 0x00051FF9
		public string Localname
		{
			get
			{
				return this.localname;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x0600131E RID: 4894 RVA: 0x00053001 File Offset: 0x00052001
		public string Prefix
		{
			get
			{
				return this.prefix;
			}
		}

		// Token: 0x04000BE1 RID: 3041
		private string localname;

		// Token: 0x04000BE2 RID: 3042
		private string prefix;
	}
}

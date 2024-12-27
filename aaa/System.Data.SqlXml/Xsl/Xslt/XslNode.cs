using System;
using System.Collections.Generic;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x0200010C RID: 268
	internal class XslNode
	{
		// Token: 0x06000BD6 RID: 3030 RVA: 0x0003D25A File Offset: 0x0003C25A
		public XslNode(XslNodeType nodeType, QilName name, object arg, XslVersion xslVer)
		{
			this.NodeType = nodeType;
			this.Name = name;
			this.Arg = arg;
			this.XslVersion = xslVer;
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x0003D27F File Offset: 0x0003C27F
		public XslNode(XslNodeType nodeType)
		{
			this.NodeType = nodeType;
			this.XslVersion = XslVersion.Version10;
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x0003D295 File Offset: 0x0003C295
		public string Select
		{
			get
			{
				return (string)this.Arg;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x0003D2A2 File Offset: 0x0003C2A2
		public bool ForwardsCompatible
		{
			get
			{
				return this.XslVersion == XslVersion.ForwardsCompatible;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0003D2AD File Offset: 0x0003C2AD
		public IList<XslNode> Content
		{
			get
			{
				return this.content ?? XslNode.EmptyList;
			}
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x0003D2BE File Offset: 0x0003C2BE
		public void SetContent(List<XslNode> content)
		{
			this.content = content;
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x0003D2C7 File Offset: 0x0003C2C7
		public void AddContent(XslNode node)
		{
			if (this.content == null)
			{
				this.content = new List<XslNode>();
			}
			this.content.Add(node);
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x0003D2E8 File Offset: 0x0003C2E8
		public void InsertContent(IEnumerable<XslNode> collection)
		{
			if (this.content == null)
			{
				this.content = new List<XslNode>(collection);
				return;
			}
			this.content.InsertRange(0, collection);
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x0003D30C File Offset: 0x0003C30C
		internal string TraceName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0400084B RID: 2123
		public readonly XslNodeType NodeType;

		// Token: 0x0400084C RID: 2124
		public ISourceLineInfo SourceLine;

		// Token: 0x0400084D RID: 2125
		public NsDecl Namespaces;

		// Token: 0x0400084E RID: 2126
		public readonly QilName Name;

		// Token: 0x0400084F RID: 2127
		public readonly object Arg;

		// Token: 0x04000850 RID: 2128
		public readonly XslVersion XslVersion;

		// Token: 0x04000851 RID: 2129
		public XslFlags Flags;

		// Token: 0x04000852 RID: 2130
		private List<XslNode> content;

		// Token: 0x04000853 RID: 2131
		private static readonly IList<XslNode> EmptyList = new List<XslNode>().AsReadOnly();
	}
}

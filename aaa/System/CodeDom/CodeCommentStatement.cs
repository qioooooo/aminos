using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200004A RID: 74
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeCommentStatement : CodeStatement
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x00012DE7 File Offset: 0x00011DE7
		public CodeCommentStatement()
		{
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00012DEF File Offset: 0x00011DEF
		public CodeCommentStatement(CodeComment comment)
		{
			this.comment = comment;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00012DFE File Offset: 0x00011DFE
		public CodeCommentStatement(string text)
		{
			this.comment = new CodeComment(text);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00012E12 File Offset: 0x00011E12
		public CodeCommentStatement(string text, bool docComment)
		{
			this.comment = new CodeComment(text, docComment);
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x00012E27 File Offset: 0x00011E27
		// (set) Token: 0x060002EA RID: 746 RVA: 0x00012E2F File Offset: 0x00011E2F
		public CodeComment Comment
		{
			get
			{
				return this.comment;
			}
			set
			{
				this.comment = value;
			}
		}

		// Token: 0x04000807 RID: 2055
		private CodeComment comment;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000049 RID: 73
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeComment : CodeObject
	{
		// Token: 0x060002DE RID: 734 RVA: 0x00012D8A File Offset: 0x00011D8A
		public CodeComment()
		{
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00012D92 File Offset: 0x00011D92
		public CodeComment(string text)
		{
			this.Text = text;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00012DA1 File Offset: 0x00011DA1
		public CodeComment(string text, bool docComment)
		{
			this.Text = text;
			this.docComment = docComment;
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00012DB7 File Offset: 0x00011DB7
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x00012DBF File Offset: 0x00011DBF
		public bool DocComment
		{
			get
			{
				return this.docComment;
			}
			set
			{
				this.docComment = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00012DC8 File Offset: 0x00011DC8
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x00012DDE File Offset: 0x00011DDE
		public string Text
		{
			get
			{
				if (this.text != null)
				{
					return this.text;
				}
				return string.Empty;
			}
			set
			{
				this.text = value;
			}
		}

		// Token: 0x04000805 RID: 2053
		private string text;

		// Token: 0x04000806 RID: 2054
		private bool docComment;
	}
}

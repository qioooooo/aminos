using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000076 RID: 118
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeSnippetTypeMember : CodeTypeMember
	{
		// Token: 0x06000433 RID: 1075 RVA: 0x000146DC File Offset: 0x000136DC
		public CodeSnippetTypeMember()
		{
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x000146E4 File Offset: 0x000136E4
		public CodeSnippetTypeMember(string text)
		{
			this.Text = text;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x000146F3 File Offset: 0x000136F3
		// (set) Token: 0x06000436 RID: 1078 RVA: 0x00014709 File Offset: 0x00013709
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

		// Token: 0x04000878 RID: 2168
		private string text;
	}
}

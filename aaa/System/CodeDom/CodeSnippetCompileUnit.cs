using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000073 RID: 115
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeSnippetCompileUnit : CodeCompileUnit
	{
		// Token: 0x06000425 RID: 1061 RVA: 0x00014629 File Offset: 0x00013629
		public CodeSnippetCompileUnit()
		{
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00014631 File Offset: 0x00013631
		public CodeSnippetCompileUnit(string value)
		{
			this.Value = value;
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00014640 File Offset: 0x00013640
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x00014656 File Offset: 0x00013656
		public string Value
		{
			get
			{
				if (this.value != null)
				{
					return this.value;
				}
				return string.Empty;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x0001465F File Offset: 0x0001365F
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x00014667 File Offset: 0x00013667
		public CodeLinePragma LinePragma
		{
			get
			{
				return this.linePragma;
			}
			set
			{
				this.linePragma = value;
			}
		}

		// Token: 0x04000874 RID: 2164
		private string value;

		// Token: 0x04000875 RID: 2165
		private CodeLinePragma linePragma;
	}
}

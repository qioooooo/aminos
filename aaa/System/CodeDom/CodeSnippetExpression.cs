using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000074 RID: 116
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeSnippetExpression : CodeExpression
	{
		// Token: 0x0600042B RID: 1067 RVA: 0x00014670 File Offset: 0x00013670
		public CodeSnippetExpression()
		{
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00014678 File Offset: 0x00013678
		public CodeSnippetExpression(string value)
		{
			this.Value = value;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00014687 File Offset: 0x00013687
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x0001469D File Offset: 0x0001369D
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

		// Token: 0x04000876 RID: 2166
		private string value;
	}
}

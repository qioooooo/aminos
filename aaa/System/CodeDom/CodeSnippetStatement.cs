using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000075 RID: 117
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeSnippetStatement : CodeStatement
	{
		// Token: 0x0600042F RID: 1071 RVA: 0x000146A6 File Offset: 0x000136A6
		public CodeSnippetStatement()
		{
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000146AE File Offset: 0x000136AE
		public CodeSnippetStatement(string value)
		{
			this.Value = value;
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x000146BD File Offset: 0x000136BD
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x000146D3 File Offset: 0x000136D3
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

		// Token: 0x04000877 RID: 2167
		private string value;
	}
}

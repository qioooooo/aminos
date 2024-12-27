using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000068 RID: 104
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeNamespaceImport : CodeObject
	{
		// Token: 0x060003CE RID: 974 RVA: 0x00013FC8 File Offset: 0x00012FC8
		public CodeNamespaceImport()
		{
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00013FD0 File Offset: 0x00012FD0
		public CodeNamespaceImport(string nameSpace)
		{
			this.Namespace = nameSpace;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x00013FDF File Offset: 0x00012FDF
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x00013FE7 File Offset: 0x00012FE7
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

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00013FF0 File Offset: 0x00012FF0
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x00014006 File Offset: 0x00013006
		public string Namespace
		{
			get
			{
				if (this.nameSpace != null)
				{
					return this.nameSpace;
				}
				return string.Empty;
			}
			set
			{
				this.nameSpace = value;
			}
		}

		// Token: 0x0400085E RID: 2142
		private string nameSpace;

		// Token: 0x0400085F RID: 2143
		private CodeLinePragma linePragma;
	}
}

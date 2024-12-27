using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000079 RID: 121
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeThrowExceptionStatement : CodeStatement
	{
		// Token: 0x06000446 RID: 1094 RVA: 0x00014838 File Offset: 0x00013838
		public CodeThrowExceptionStatement()
		{
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00014840 File Offset: 0x00013840
		public CodeThrowExceptionStatement(CodeExpression toThrow)
		{
			this.ToThrow = toThrow;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0001484F File Offset: 0x0001384F
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x00014857 File Offset: 0x00013857
		public CodeExpression ToThrow
		{
			get
			{
				return this.toThrow;
			}
			set
			{
				this.toThrow = value;
			}
		}

		// Token: 0x04000879 RID: 2169
		private CodeExpression toThrow;
	}
}

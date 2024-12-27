using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000050 RID: 80
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeConstructor : CodeMemberMethod
	{
		// Token: 0x06000321 RID: 801 RVA: 0x00013337 File Offset: 0x00012337
		public CodeConstructor()
		{
			base.Name = ".ctor";
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000322 RID: 802 RVA: 0x00013360 File Offset: 0x00012360
		public CodeExpressionCollection BaseConstructorArgs
		{
			get
			{
				return this.baseConstructorArgs;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000323 RID: 803 RVA: 0x00013368 File Offset: 0x00012368
		public CodeExpressionCollection ChainedConstructorArgs
		{
			get
			{
				return this.chainedConstructorArgs;
			}
		}

		// Token: 0x04000825 RID: 2085
		private CodeExpressionCollection baseConstructorArgs = new CodeExpressionCollection();

		// Token: 0x04000826 RID: 2086
		private CodeExpressionCollection chainedConstructorArgs = new CodeExpressionCollection();
	}
}

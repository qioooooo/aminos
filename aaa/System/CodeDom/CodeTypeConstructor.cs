using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200007B RID: 123
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeConstructor : CodeMemberMethod
	{
		// Token: 0x06000450 RID: 1104 RVA: 0x00014948 File Offset: 0x00013948
		public CodeTypeConstructor()
		{
			base.Name = ".cctor";
		}
	}
}

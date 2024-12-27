using System;
using System.CodeDom;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000162 RID: 354
	public interface ICodeDomDesignerReload
	{
		// Token: 0x06000D2A RID: 3370
		bool ShouldReloadDesigner(CodeCompileUnit newTree);
	}
}

using System;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000021 RID: 33
	[Guid("E93D012C-56BB-4f32-864F-7C75EDA17B14")]
	[ComVisible(true)]
	public interface IErrorHandler
	{
		// Token: 0x06000132 RID: 306
		bool OnCompilerError(IVsaFullErrorInfo error);
	}
}

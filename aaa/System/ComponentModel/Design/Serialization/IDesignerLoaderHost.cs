using System;
using System.Collections;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001A7 RID: 423
	public interface IDesignerLoaderHost : IDesignerHost, IServiceContainer, IServiceProvider
	{
		// Token: 0x06000D11 RID: 3345
		void EndLoad(string baseClassName, bool successful, ICollection errorCollection);

		// Token: 0x06000D12 RID: 3346
		void Reload();
	}
}

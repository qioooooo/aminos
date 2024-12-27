using System;
using System.Collections;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001A9 RID: 425
	public interface IDesignerLoaderService
	{
		// Token: 0x06000D17 RID: 3351
		void AddLoadDependency();

		// Token: 0x06000D18 RID: 3352
		void DependentLoadComplete(bool successful, ICollection errorCollection);

		// Token: 0x06000D19 RID: 3353
		bool Reload();
	}
}

using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x0200018F RID: 399
	public interface ITreeDesigner : IDesigner, IDisposable
	{
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000CA1 RID: 3233
		ICollection Children { get; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000CA2 RID: 3234
		IDesigner Parent { get; }
	}
}

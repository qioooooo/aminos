using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x0200017C RID: 380
	[ComVisible(true)]
	public interface IDesigner : IDisposable
	{
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000C30 RID: 3120
		IComponent Component { get; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000C31 RID: 3121
		DesignerVerbCollection Verbs { get; }

		// Token: 0x06000C32 RID: 3122
		void DoDefaultAction();

		// Token: 0x06000C33 RID: 3123
		void Initialize(IComponent component);
	}
}

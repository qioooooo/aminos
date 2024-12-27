using System;
using System.Diagnostics;

namespace System.ComponentModel.Design
{
	// Token: 0x0200012D RID: 301
	public interface IComponentDesignerDebugService
	{
		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000BC7 RID: 3015
		// (set) Token: 0x06000BC8 RID: 3016
		int IndentLevel { get; set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000BC9 RID: 3017
		TraceListenerCollection Listeners { get; }

		// Token: 0x06000BCA RID: 3018
		void Assert(bool condition, string message);

		// Token: 0x06000BCB RID: 3019
		void Fail(string message);

		// Token: 0x06000BCC RID: 3020
		void Trace(string message, string category);
	}
}

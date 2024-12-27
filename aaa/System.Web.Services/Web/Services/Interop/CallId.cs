using System;
using System.Runtime.InteropServices;

namespace System.Web.Services.Interop
{
	// Token: 0x0200001B RID: 27
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct CallId
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00002DFA File Offset: 0x00001DFA
		public CallId(string machine, int pid, IntPtr userThread, long stackPtr, string entryPoint, string destMachine)
		{
			this.szMachine = machine;
			this.dwPid = pid;
			this.userThread = userThread;
			this.addStackPointer = stackPtr;
			this.szEntryPoint = entryPoint;
			this.szDestinationMachine = destMachine;
		}

		// Token: 0x04000226 RID: 550
		public string szMachine;

		// Token: 0x04000227 RID: 551
		public int dwPid;

		// Token: 0x04000228 RID: 552
		public IntPtr userThread;

		// Token: 0x04000229 RID: 553
		public long addStackPointer;

		// Token: 0x0400022A RID: 554
		public string szEntryPoint;

		// Token: 0x0400022B RID: 555
		public string szDestinationMachine;
	}
}

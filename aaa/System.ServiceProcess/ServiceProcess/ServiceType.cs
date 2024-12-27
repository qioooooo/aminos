using System;

namespace System.ServiceProcess
{
	// Token: 0x02000034 RID: 52
	[Flags]
	public enum ServiceType
	{
		// Token: 0x0400022D RID: 557
		Adapter = 4,
		// Token: 0x0400022E RID: 558
		FileSystemDriver = 2,
		// Token: 0x0400022F RID: 559
		InteractiveProcess = 256,
		// Token: 0x04000230 RID: 560
		KernelDriver = 1,
		// Token: 0x04000231 RID: 561
		RecognizerDriver = 8,
		// Token: 0x04000232 RID: 562
		Win32OwnProcess = 16,
		// Token: 0x04000233 RID: 563
		Win32ShareProcess = 32
	}
}

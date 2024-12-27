using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000519 RID: 1305
	[Flags]
	public enum RegistrationClassContext
	{
		// Token: 0x040019CC RID: 6604
		InProcessServer = 1,
		// Token: 0x040019CD RID: 6605
		InProcessHandler = 2,
		// Token: 0x040019CE RID: 6606
		LocalServer = 4,
		// Token: 0x040019CF RID: 6607
		InProcessServer16 = 8,
		// Token: 0x040019D0 RID: 6608
		RemoteServer = 16,
		// Token: 0x040019D1 RID: 6609
		InProcessHandler16 = 32,
		// Token: 0x040019D2 RID: 6610
		Reserved1 = 64,
		// Token: 0x040019D3 RID: 6611
		Reserved2 = 128,
		// Token: 0x040019D4 RID: 6612
		Reserved3 = 256,
		// Token: 0x040019D5 RID: 6613
		Reserved4 = 512,
		// Token: 0x040019D6 RID: 6614
		NoCodeDownload = 1024,
		// Token: 0x040019D7 RID: 6615
		Reserved5 = 2048,
		// Token: 0x040019D8 RID: 6616
		NoCustomMarshal = 4096,
		// Token: 0x040019D9 RID: 6617
		EnableCodeDownload = 8192,
		// Token: 0x040019DA RID: 6618
		NoFailureLog = 16384,
		// Token: 0x040019DB RID: 6619
		DisableActivateAsActivator = 32768,
		// Token: 0x040019DC RID: 6620
		EnableActivateAsActivator = 65536,
		// Token: 0x040019DD RID: 6621
		FromDefaultContext = 131072
	}
}

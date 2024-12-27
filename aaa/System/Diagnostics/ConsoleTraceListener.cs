using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020001BC RID: 444
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class ConsoleTraceListener : TextWriterTraceListener
	{
		// Token: 0x06000DBA RID: 3514 RVA: 0x0002BDB4 File Offset: 0x0002ADB4
		public ConsoleTraceListener()
			: base(Console.Out)
		{
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0002BDC1 File Offset: 0x0002ADC1
		public ConsoleTraceListener(bool useErrorStream)
			: base(useErrorStream ? Console.Error : Console.Out)
		{
		}
	}
}

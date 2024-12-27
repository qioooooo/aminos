using System;
using System.Diagnostics;

namespace System.IO.Compression
{
	// Token: 0x020001FF RID: 511
	internal class CompressionTracingSwitch : Switch
	{
		// Token: 0x0600115F RID: 4447 RVA: 0x00038D04 File Offset: 0x00037D04
		internal CompressionTracingSwitch(string displayName, string description)
			: base(displayName, description)
		{
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x00038D0E File Offset: 0x00037D0E
		public static bool Verbose
		{
			get
			{
				return CompressionTracingSwitch.tracingSwitch.SwitchSetting >= 2;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001161 RID: 4449 RVA: 0x00038D20 File Offset: 0x00037D20
		public static bool Informational
		{
			get
			{
				return CompressionTracingSwitch.tracingSwitch.SwitchSetting >= 1;
			}
		}

		// Token: 0x04000FBE RID: 4030
		internal static CompressionTracingSwitch tracingSwitch = new CompressionTracingSwitch("CompressionSwitch", "Compression Library Tracing Switch");
	}
}

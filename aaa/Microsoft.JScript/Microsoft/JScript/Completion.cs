using System;

namespace Microsoft.JScript
{
	// Token: 0x02000049 RID: 73
	internal sealed class Completion
	{
		// Token: 0x06000389 RID: 905 RVA: 0x00016650 File Offset: 0x00015650
		internal Completion()
		{
			this.Continue = 0;
			this.Exit = 0;
			this.Return = false;
			this.value = null;
		}

		// Token: 0x040001C9 RID: 457
		internal int Continue;

		// Token: 0x040001CA RID: 458
		internal int Exit;

		// Token: 0x040001CB RID: 459
		internal bool Return;

		// Token: 0x040001CC RID: 460
		public object value;
	}
}

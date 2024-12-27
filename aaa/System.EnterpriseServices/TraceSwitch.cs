using System;

namespace System.EnterpriseServices
{
	// Token: 0x0200009C RID: 156
	internal class TraceSwitch : BaseSwitch
	{
		// Token: 0x060003B2 RID: 946 RVA: 0x0000C097 File Offset: 0x0000B097
		internal TraceSwitch(string name)
			: base(name)
		{
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0000C0A0 File Offset: 0x0000B0A0
		internal int Level
		{
			get
			{
				return base.Value;
			}
		}
	}
}

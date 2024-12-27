using System;

namespace System.EnterpriseServices
{
	// Token: 0x0200009B RID: 155
	internal class BooleanSwitch : BaseSwitch
	{
		// Token: 0x060003B0 RID: 944 RVA: 0x0000C080 File Offset: 0x0000B080
		internal BooleanSwitch(string name)
			: base(name)
		{
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0000C089 File Offset: 0x0000B089
		internal bool Enabled
		{
			get
			{
				return base.Value != 0;
			}
		}
	}
}

using System;

namespace System.Web.Configuration
{
	// Token: 0x020001B2 RID: 434
	internal abstract class CapabilitiesRule
	{
		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001921 RID: 6433 RVA: 0x0007823C File Offset: 0x0007723C
		internal virtual int Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x06001922 RID: 6434
		internal abstract void Evaluate(CapabilitiesState state);

		// Token: 0x04001708 RID: 5896
		internal const int Use = 0;

		// Token: 0x04001709 RID: 5897
		internal const int Assign = 1;

		// Token: 0x0400170A RID: 5898
		internal const int Filter = 2;

		// Token: 0x0400170B RID: 5899
		internal const int Case = 3;

		// Token: 0x0400170C RID: 5900
		internal int _type;
	}
}

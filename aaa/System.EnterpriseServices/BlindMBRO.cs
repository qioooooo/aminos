using System;

namespace System.EnterpriseServices
{
	// Token: 0x0200002E RID: 46
	internal class BlindMBRO : MarshalByRefObject
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00004569 File Offset: 0x00003569
		public BlindMBRO(MarshalByRefObject holder)
		{
			this._holder = holder;
		}

		// Token: 0x04000063 RID: 99
		private MarshalByRefObject _holder;
	}
}

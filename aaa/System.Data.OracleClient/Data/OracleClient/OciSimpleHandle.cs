using System;

namespace System.Data.OracleClient
{
	// Token: 0x02000045 RID: 69
	internal abstract class OciSimpleHandle : OciHandle
	{
		// Token: 0x0600020F RID: 527 RVA: 0x0005B540 File Offset: 0x0005A940
		internal OciSimpleHandle(OciHandle parent, OCI.HTYPE handleType, IntPtr value)
			: base(handleType)
		{
			this.handle = value;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0005B55C File Offset: 0x0005A95C
		public override bool IsInvalid
		{
			get
			{
				return true;
			}
		}
	}
}

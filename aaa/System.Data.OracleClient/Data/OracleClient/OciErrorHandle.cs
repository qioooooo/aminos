using System;

namespace System.Data.OracleClient
{
	// Token: 0x0200003A RID: 58
	internal sealed class OciErrorHandle : OciHandle
	{
		// Token: 0x060001FD RID: 509 RVA: 0x0005B2F0 File Offset: 0x0005A6F0
		internal OciErrorHandle(OciHandle parent)
			: base(parent, OCI.HTYPE.OCI_HTYPE_ERROR)
		{
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0005B308 File Offset: 0x0005A708
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0005B31C File Offset: 0x0005A71C
		internal bool ConnectionIsBroken
		{
			get
			{
				return this._connectionIsBroken;
			}
			set
			{
				this._connectionIsBroken = value;
			}
		}

		// Token: 0x04000324 RID: 804
		private bool _connectionIsBroken;
	}
}

using System;
using System.Security.Principal;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200002D RID: 45
	public class QuotaControl : DirectoryControl
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x000050A8 File Offset: 0x000040A8
		public QuotaControl()
			: base("1.2.840.113556.1.4.1852", null, true, true)
		{
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000050B8 File Offset: 0x000040B8
		public QuotaControl(SecurityIdentifier querySid)
			: this()
		{
			this.QuerySid = querySid;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x000050C7 File Offset: 0x000040C7
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x000050DF File Offset: 0x000040DF
		public SecurityIdentifier QuerySid
		{
			get
			{
				if (this.sid == null)
				{
					return null;
				}
				return new SecurityIdentifier(this.sid, 0);
			}
			set
			{
				if (value == null)
				{
					this.sid = null;
					return;
				}
				this.sid = new byte[value.BinaryLength];
				value.GetBinaryForm(this.sid, 0);
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005110 File Offset: 0x00004110
		public override byte[] GetValue()
		{
			this.directoryControlValue = BerConverter.Encode("{o}", new object[] { this.sid });
			return base.GetValue();
		}

		// Token: 0x04000100 RID: 256
		private byte[] sid;
	}
}

using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000019 RID: 25
	public class AsqResponseControl : DirectoryControl
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00004230 File Offset: 0x00003230
		internal AsqResponseControl(int result, bool criticality, byte[] controlValue)
			: base("1.2.840.113556.1.4.1504", controlValue, criticality, true)
		{
			this.result = (ResultCode)result;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00004247 File Offset: 0x00003247
		public ResultCode Result
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x040000E2 RID: 226
		private ResultCode result;
	}
}

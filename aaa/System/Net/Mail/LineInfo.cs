using System;

namespace System.Net.Mail
{
	// Token: 0x020006C7 RID: 1735
	internal struct LineInfo
	{
		// Token: 0x06003585 RID: 13701 RVA: 0x000E3D55 File Offset: 0x000E2D55
		internal LineInfo(SmtpStatusCode statusCode, string line)
		{
			this.statusCode = statusCode;
			this.line = line;
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06003586 RID: 13702 RVA: 0x000E3D65 File Offset: 0x000E2D65
		internal string Line
		{
			get
			{
				return this.line;
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06003587 RID: 13703 RVA: 0x000E3D6D File Offset: 0x000E2D6D
		internal SmtpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
		}

		// Token: 0x040030DE RID: 12510
		private string line;

		// Token: 0x040030DF RID: 12511
		private SmtpStatusCode statusCode;
	}
}

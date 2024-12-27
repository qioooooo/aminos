using System;
using System.Text;

namespace System.Net
{
	// Token: 0x020004BE RID: 1214
	internal class ResponseDescription
	{
		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060025AE RID: 9646 RVA: 0x000961C5 File Offset: 0x000951C5
		internal bool PositiveIntermediate
		{
			get
			{
				return this.Status >= 100 && this.Status <= 199;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x000961E3 File Offset: 0x000951E3
		internal bool PositiveCompletion
		{
			get
			{
				return this.Status >= 200 && this.Status <= 299;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x060025B0 RID: 9648 RVA: 0x00096204 File Offset: 0x00095204
		internal bool TransientFailure
		{
			get
			{
				return this.Status >= 400 && this.Status <= 499;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x060025B1 RID: 9649 RVA: 0x00096225 File Offset: 0x00095225
		internal bool PermanentFailure
		{
			get
			{
				return this.Status >= 500 && this.Status <= 599;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x060025B2 RID: 9650 RVA: 0x00096246 File Offset: 0x00095246
		internal bool InvalidStatusCode
		{
			get
			{
				return this.Status < 100 || this.Status > 599;
			}
		}

		// Token: 0x04002547 RID: 9543
		internal const int NoStatus = -1;

		// Token: 0x04002548 RID: 9544
		internal bool Multiline;

		// Token: 0x04002549 RID: 9545
		internal int Status = -1;

		// Token: 0x0400254A RID: 9546
		internal string StatusDescription;

		// Token: 0x0400254B RID: 9547
		internal StringBuilder StatusBuffer = new StringBuilder();

		// Token: 0x0400254C RID: 9548
		internal string StatusCodeString;
	}
}

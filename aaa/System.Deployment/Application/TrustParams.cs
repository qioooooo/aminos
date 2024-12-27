using System;

namespace System.Deployment.Application
{
	// Token: 0x0200003C RID: 60
	internal class TrustParams
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000DC11 File Offset: 0x0000CC11
		// (set) Token: 0x06000208 RID: 520 RVA: 0x0000DC19 File Offset: 0x0000CC19
		public bool NoPrompt
		{
			get
			{
				return this.noPrompt;
			}
			set
			{
				this.noPrompt = value;
			}
		}

		// Token: 0x040001BF RID: 447
		private bool noPrompt;
	}
}

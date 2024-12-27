using System;

namespace System.Web
{
	// Token: 0x02000023 RID: 35
	internal class UseLastUnhandledErrorFormatter : UnhandledErrorFormatter
	{
		// Token: 0x060000DC RID: 220 RVA: 0x000055C3 File Offset: 0x000045C3
		internal UseLastUnhandledErrorFormatter(Exception e)
			: base(e)
		{
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000055CC File Offset: 0x000045CC
		internal override void PrepareFormatter()
		{
			base.PrepareFormatter();
			this._initialException = this.Exception;
		}
	}
}

using System;
using System.Runtime.ConstrainedExecution;

namespace System.Drawing
{
	// Token: 0x02000038 RID: 56
	public sealed class BufferedGraphicsManager
	{
		// Token: 0x06000254 RID: 596 RVA: 0x0000A113 File Offset: 0x00009113
		private BufferedGraphicsManager()
		{
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000A11B File Offset: 0x0000911B
		static BufferedGraphicsManager()
		{
			AppDomain.CurrentDomain.ProcessExit += BufferedGraphicsManager.OnShutdown;
			AppDomain.CurrentDomain.DomainUnload += BufferedGraphicsManager.OnShutdown;
			BufferedGraphicsManager.bufferedGraphicsContext = new BufferedGraphicsContext();
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000A153 File Offset: 0x00009153
		public static BufferedGraphicsContext Current
		{
			get
			{
				return BufferedGraphicsManager.bufferedGraphicsContext;
			}
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000A15A File Offset: 0x0000915A
		[PrePrepareMethod]
		private static void OnShutdown(object sender, EventArgs e)
		{
			BufferedGraphicsManager.Current.Invalidate();
		}

		// Token: 0x0400026A RID: 618
		private static BufferedGraphicsContext bufferedGraphicsContext;
	}
}

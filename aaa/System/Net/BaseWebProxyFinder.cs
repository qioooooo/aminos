using System;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x02000380 RID: 896
	internal abstract class BaseWebProxyFinder : IWebProxyFinder, IDisposable
	{
		// Token: 0x06001BF8 RID: 7160 RVA: 0x00069636 File Offset: 0x00068636
		public BaseWebProxyFinder(AutoWebProxyScriptEngine engine)
		{
			this.engine = engine;
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x00069645 File Offset: 0x00068645
		public bool IsValid
		{
			get
			{
				return this.state == BaseWebProxyFinder.AutoWebProxyState.Completed || this.state == BaseWebProxyFinder.AutoWebProxyState.Uninitialized;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001BFA RID: 7162 RVA: 0x0006965B File Offset: 0x0006865B
		public bool IsUnrecognizedScheme
		{
			get
			{
				return this.state == BaseWebProxyFinder.AutoWebProxyState.UnrecognizedScheme;
			}
		}

		// Token: 0x06001BFB RID: 7163
		public abstract bool GetProxies(Uri destination, out IList<string> proxyList);

		// Token: 0x06001BFC RID: 7164
		public abstract void Abort();

		// Token: 0x06001BFD RID: 7165 RVA: 0x00069666 File Offset: 0x00068666
		public virtual void Reset()
		{
			this.State = BaseWebProxyFinder.AutoWebProxyState.Uninitialized;
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x0006966F File Offset: 0x0006866F
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001BFF RID: 7167 RVA: 0x00069678 File Offset: 0x00068678
		// (set) Token: 0x06001C00 RID: 7168 RVA: 0x00069680 File Offset: 0x00068680
		protected BaseWebProxyFinder.AutoWebProxyState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001C01 RID: 7169 RVA: 0x00069689 File Offset: 0x00068689
		protected AutoWebProxyScriptEngine Engine
		{
			get
			{
				return this.engine;
			}
		}

		// Token: 0x06001C02 RID: 7170
		protected abstract void Dispose(bool disposing);

		// Token: 0x04001C92 RID: 7314
		private BaseWebProxyFinder.AutoWebProxyState state;

		// Token: 0x04001C93 RID: 7315
		private AutoWebProxyScriptEngine engine;

		// Token: 0x02000381 RID: 897
		protected enum AutoWebProxyState
		{
			// Token: 0x04001C95 RID: 7317
			Uninitialized,
			// Token: 0x04001C96 RID: 7318
			DiscoveryFailure,
			// Token: 0x04001C97 RID: 7319
			DownloadFailure,
			// Token: 0x04001C98 RID: 7320
			CompilationFailure,
			// Token: 0x04001C99 RID: 7321
			UnrecognizedScheme,
			// Token: 0x04001C9A RID: 7322
			Completed
		}
	}
}

using System;
using System.Web.Hosting;

namespace System.Web
{
	// Token: 0x02000065 RID: 101
	internal class HttpContextWrapper : IDisposable
	{
		// Token: 0x0600045B RID: 1115 RVA: 0x000131EC File Offset: 0x000121EC
		internal static HttpContext SwitchContext(HttpContext context)
		{
			return ContextBase.SwitchContext(context) as HttpContext;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000131F9 File Offset: 0x000121F9
		internal HttpContextWrapper(HttpContext context)
		{
			if (context != null)
			{
				this._savedContext = HttpContextWrapper.SwitchContext(context);
				this._needToUndo = this._savedContext != context;
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00013222 File Offset: 0x00012222
		void IDisposable.Dispose()
		{
			if (this._needToUndo)
			{
				HttpContextWrapper.SwitchContext(this._savedContext);
				this._savedContext = null;
				this._needToUndo = false;
			}
		}

		// Token: 0x0400101E RID: 4126
		private bool _needToUndo;

		// Token: 0x0400101F RID: 4127
		private HttpContext _savedContext;
	}
}

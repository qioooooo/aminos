using System;

namespace System.Web.Util
{
	// Token: 0x02000754 RID: 1876
	internal class CalliEventHandlerDelegateProxy
	{
		// Token: 0x06005AFB RID: 23291 RVA: 0x0016F2F2 File Offset: 0x0016E2F2
		internal CalliEventHandlerDelegateProxy(object target, IntPtr functionPointer, bool argless)
		{
			this._argless = argless;
			this._target = target;
			this._functionPointer = functionPointer;
		}

		// Token: 0x06005AFC RID: 23292 RVA: 0x0016F30F File Offset: 0x0016E30F
		internal void Callback(object sender, EventArgs e)
		{
			if (this._argless)
			{
				CalliHelper.ArglessFunctionCaller(this._functionPointer, this._target);
				return;
			}
			CalliHelper.EventArgFunctionCaller(this._functionPointer, this._target, sender, e);
		}

		// Token: 0x170017A2 RID: 6050
		// (get) Token: 0x06005AFD RID: 23293 RVA: 0x0016F33E File Offset: 0x0016E33E
		internal EventHandler Handler
		{
			get
			{
				return new EventHandler(this.Callback);
			}
		}

		// Token: 0x040030FF RID: 12543
		private IntPtr _functionPointer;

		// Token: 0x04003100 RID: 12544
		private object _target;

		// Token: 0x04003101 RID: 12545
		private bool _argless;
	}
}

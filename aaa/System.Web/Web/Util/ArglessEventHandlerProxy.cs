using System;
using System.Reflection;

namespace System.Web.Util
{
	// Token: 0x02000752 RID: 1874
	internal class ArglessEventHandlerProxy
	{
		// Token: 0x06005AF4 RID: 23284 RVA: 0x0016F2B4 File Offset: 0x0016E2B4
		internal ArglessEventHandlerProxy(object target, MethodInfo arglessMethod)
		{
			this._target = target;
			this._arglessMethod = arglessMethod;
		}

		// Token: 0x06005AF5 RID: 23285 RVA: 0x0016F2CA File Offset: 0x0016E2CA
		internal void Callback(object sender, EventArgs e)
		{
			this._arglessMethod.Invoke(this._target, new object[0]);
		}

		// Token: 0x170017A1 RID: 6049
		// (get) Token: 0x06005AF6 RID: 23286 RVA: 0x0016F2E4 File Offset: 0x0016E2E4
		internal EventHandler Handler
		{
			get
			{
				return new EventHandler(this.Callback);
			}
		}

		// Token: 0x040030FD RID: 12541
		private object _target;

		// Token: 0x040030FE RID: 12542
		private MethodInfo _arglessMethod;
	}
}

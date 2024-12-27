using System;
using System.ComponentModel;

namespace System.Deployment.Application
{
	// Token: 0x0200003E RID: 62
	internal class BindCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x0600020D RID: 525 RVA: 0x0000DC22 File Offset: 0x0000CC22
		internal BindCompletedEventArgs(Exception error, bool cancelled, object userState, ActivationContext actCtx, string name, bool cached)
			: base(error, cancelled, userState)
		{
			this._actCtx = actCtx;
			this._name = name;
			this._cached = cached;
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000DC45 File Offset: 0x0000CC45
		public ActivationContext ActivationContext
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._actCtx;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000DC53 File Offset: 0x0000CC53
		public string FriendlyName
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._name;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000DC61 File Offset: 0x0000CC61
		public bool IsCached
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._cached;
			}
		}

		// Token: 0x040001C0 RID: 448
		private readonly ActivationContext _actCtx;

		// Token: 0x040001C1 RID: 449
		private readonly string _name;

		// Token: 0x040001C2 RID: 450
		private readonly bool _cached;
	}
}

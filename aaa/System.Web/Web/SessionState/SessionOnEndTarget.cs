using System;
using System.Web.Util;

namespace System.Web.SessionState
{
	// Token: 0x02000374 RID: 884
	internal class SessionOnEndTarget
	{
		// Token: 0x06002AD8 RID: 10968 RVA: 0x000BD5D3 File Offset: 0x000BC5D3
		internal SessionOnEndTarget()
		{
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06002AD9 RID: 10969 RVA: 0x000BD5DB File Offset: 0x000BC5DB
		// (set) Token: 0x06002ADA RID: 10970 RVA: 0x000BD5E3 File Offset: 0x000BC5E3
		internal int SessionEndEventHandlerCount
		{
			get
			{
				return this._sessionEndEventHandlerCount;
			}
			set
			{
				this._sessionEndEventHandlerCount = value;
			}
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x000BD5EC File Offset: 0x000BC5EC
		internal void RaiseOnEnd(HttpSessionState sessionState)
		{
			if (this._sessionEndEventHandlerCount > 0)
			{
				HttpApplicationFactory.EndSession(sessionState, this, EventArgs.Empty);
			}
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x000BD604 File Offset: 0x000BC604
		internal void RaiseSessionOnEnd(string id, SessionStateStoreData item)
		{
			HttpSessionStateContainer httpSessionStateContainer = new HttpSessionStateContainer(id, item.Items, item.StaticObjects, item.Timeout, false, SessionStateModule.s_configCookieless, SessionStateModule.s_configMode, true);
			HttpSessionState httpSessionState = new HttpSessionState(httpSessionStateContainer);
			if (HttpRuntime.ShutdownInProgress)
			{
				this.RaiseOnEnd(httpSessionState);
				return;
			}
			SessionOnEndTargetWorkItem sessionOnEndTargetWorkItem = new SessionOnEndTargetWorkItem(this, httpSessionState);
			WorkItem.PostInternal(new WorkItemCallback(sessionOnEndTargetWorkItem.RaiseOnEndCallback));
		}

		// Token: 0x04001F80 RID: 8064
		internal int _sessionEndEventHandlerCount;
	}
}

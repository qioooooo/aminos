using System;

namespace System.Web.SessionState
{
	// Token: 0x02000373 RID: 883
	internal class SessionOnEndTargetWorkItem
	{
		// Token: 0x06002AD6 RID: 10966 RVA: 0x000BD5AA File Offset: 0x000BC5AA
		internal SessionOnEndTargetWorkItem(SessionOnEndTarget target, HttpSessionState sessionState)
		{
			this._target = target;
			this._sessionState = sessionState;
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x000BD5C0 File Offset: 0x000BC5C0
		internal void RaiseOnEndCallback()
		{
			this._target.RaiseOnEnd(this._sessionState);
		}

		// Token: 0x04001F7E RID: 8062
		private SessionOnEndTarget _target;

		// Token: 0x04001F7F RID: 8063
		private HttpSessionState _sessionState;
	}
}

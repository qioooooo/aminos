﻿using System;

namespace System.Web.Management
{
	// Token: 0x020002DE RID: 734
	internal enum WebEventType
	{
		// Token: 0x04001D29 RID: 7465
		WEBEVENT_BASE_EVENT,
		// Token: 0x04001D2A RID: 7466
		WEBEVENT_MANAGEMENT_EVENT,
		// Token: 0x04001D2B RID: 7467
		WEBEVENT_APP_LIFETIME_EVENT,
		// Token: 0x04001D2C RID: 7468
		WEBEVENT_REQUEST_EVENT,
		// Token: 0x04001D2D RID: 7469
		WEBEVENT_HEARTBEAT_EVENT,
		// Token: 0x04001D2E RID: 7470
		WEBEVENT_BASE_ERROR_EVENT,
		// Token: 0x04001D2F RID: 7471
		WEBEVENT_REQUEST_ERROR_EVENT,
		// Token: 0x04001D30 RID: 7472
		WEBEVENT_ERROR_EVENT,
		// Token: 0x04001D31 RID: 7473
		WEBEVENT_AUDIT_EVENT,
		// Token: 0x04001D32 RID: 7474
		WEBEVENT_SUCCESS_AUDIT_EVENT,
		// Token: 0x04001D33 RID: 7475
		WEBEVENT_AUTHENTICATION_SUCCESS_AUDIT_EVENT,
		// Token: 0x04001D34 RID: 7476
		WEBEVENT_FAILURE_AUDIT_EVENT,
		// Token: 0x04001D35 RID: 7477
		WEBEVENT_AUTHENTICATION_FAILURE_AUDIT_EVENT,
		// Token: 0x04001D36 RID: 7478
		WEBEVENT_VIEWSTATE_FAILURE_AUDIT_EVENT
	}
}

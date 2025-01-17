﻿using System;

namespace System.Web
{
	// Token: 0x02000026 RID: 38
	internal enum EtwTraceType
	{
		// Token: 0x04000D48 RID: 3400
		ETW_TYPE_START = 1,
		// Token: 0x04000D49 RID: 3401
		ETW_TYPE_END,
		// Token: 0x04000D4A RID: 3402
		ETW_TYPE_REQ_QUEUED,
		// Token: 0x04000D4B RID: 3403
		ETW_TYPE_REQ_DEQUEUED,
		// Token: 0x04000D4C RID: 3404
		ETW_TYPE_GETAPPDOMAIN_ENTER,
		// Token: 0x04000D4D RID: 3405
		ETW_TYPE_GETAPPDOMAIN_LEAVE,
		// Token: 0x04000D4E RID: 3406
		ETW_TYPE_APPDOMAIN_ENTER,
		// Token: 0x04000D4F RID: 3407
		ETW_TYPE_START_HANDLER,
		// Token: 0x04000D50 RID: 3408
		ETW_TYPE_END_HANDLER,
		// Token: 0x04000D51 RID: 3409
		ETW_TYPE_PIPELINE_ENTER,
		// Token: 0x04000D52 RID: 3410
		ETW_TYPE_PIPELINE_LEAVE,
		// Token: 0x04000D53 RID: 3411
		ETW_TYPE_MAPHANDLER_ENTER,
		// Token: 0x04000D54 RID: 3412
		ETW_TYPE_MAPHANDLER_LEAVE,
		// Token: 0x04000D55 RID: 3413
		ETW_TYPE_PARSE_ENTER,
		// Token: 0x04000D56 RID: 3414
		ETW_TYPE_PARSE_LEAVE,
		// Token: 0x04000D57 RID: 3415
		ETW_TYPE_COMPILE_ENTER,
		// Token: 0x04000D58 RID: 3416
		ETW_TYPE_COMPILE_LEAVE,
		// Token: 0x04000D59 RID: 3417
		ETW_TYPE_HTTPHANDLER_ENTER,
		// Token: 0x04000D5A RID: 3418
		ETW_TYPE_HTTPHANDLER_LEAVE,
		// Token: 0x04000D5B RID: 3419
		ETW_TYPE_SESSIONSTATE_PARTITION_START,
		// Token: 0x04000D5C RID: 3420
		ETW_TYPE_SESSIONSTATE_PARTITION_END,
		// Token: 0x04000D5D RID: 3421
		ETW_TYPE_PAGE_PRE_INIT_ENTER,
		// Token: 0x04000D5E RID: 3422
		ETW_TYPE_PAGE_PRE_INIT_LEAVE,
		// Token: 0x04000D5F RID: 3423
		ETW_TYPE_PAGE_INIT_ENTER,
		// Token: 0x04000D60 RID: 3424
		ETW_TYPE_PAGE_INIT_LEAVE,
		// Token: 0x04000D61 RID: 3425
		ETW_TYPE_PAGE_LOAD_VIEWSTATE_ENTER,
		// Token: 0x04000D62 RID: 3426
		ETW_TYPE_PAGE_LOAD_VIEWSTATE_LEAVE,
		// Token: 0x04000D63 RID: 3427
		ETW_TYPE_PAGE_LOAD_POSTDATA_ENTER,
		// Token: 0x04000D64 RID: 3428
		ETW_TYPE_PAGE_LOAD_POSTDATA_LEAVE,
		// Token: 0x04000D65 RID: 3429
		ETW_TYPE_PAGE_LOAD_ENTER,
		// Token: 0x04000D66 RID: 3430
		ETW_TYPE_PAGE_LOAD_LEAVE,
		// Token: 0x04000D67 RID: 3431
		ETW_TYPE_PAGE_POST_DATA_CHANGED_ENTER,
		// Token: 0x04000D68 RID: 3432
		ETW_TYPE_PAGE_POST_DATA_CHANGED_LEAVE,
		// Token: 0x04000D69 RID: 3433
		ETW_TYPE_PAGE_RAISE_POSTBACK_ENTER,
		// Token: 0x04000D6A RID: 3434
		ETW_TYPE_PAGE_RAISE_POSTBACK_LEAVE,
		// Token: 0x04000D6B RID: 3435
		ETW_TYPE_PAGE_PRE_RENDER_ENTER,
		// Token: 0x04000D6C RID: 3436
		ETW_TYPE_PAGE_PRE_RENDER_LEAVE,
		// Token: 0x04000D6D RID: 3437
		ETW_TYPE_PAGE_SAVE_VIEWSTATE_ENTER,
		// Token: 0x04000D6E RID: 3438
		ETW_TYPE_PAGE_SAVE_VIEWSTATE_LEAVE,
		// Token: 0x04000D6F RID: 3439
		ETW_TYPE_PAGE_RENDER_ENTER,
		// Token: 0x04000D70 RID: 3440
		ETW_TYPE_PAGE_RENDER_LEAVE,
		// Token: 0x04000D71 RID: 3441
		ETW_TYPE_SESSION_DATA_BEGIN,
		// Token: 0x04000D72 RID: 3442
		ETW_TYPE_SESSION_DATA_END,
		// Token: 0x04000D73 RID: 3443
		ETW_TYPE_PROFILE_BEGIN,
		// Token: 0x04000D74 RID: 3444
		ETW_TYPE_PROFILE_END,
		// Token: 0x04000D75 RID: 3445
		ETW_TYPE_ROLE_IS_USER_IN_ROLE,
		// Token: 0x04000D76 RID: 3446
		ETW_TYPE_ROLE_GET_USER_ROLES,
		// Token: 0x04000D77 RID: 3447
		ETW_TYPE_ROLE_BEGIN,
		// Token: 0x04000D78 RID: 3448
		ETW_TYPE_ROLE_END,
		// Token: 0x04000D79 RID: 3449
		ETW_TYPE_WEB_EVENT_RAISE_START,
		// Token: 0x04000D7A RID: 3450
		ETW_TYPE_WEB_EVENT_RAISE_END,
		// Token: 0x04000D7B RID: 3451
		ETW_TYPE_WEB_EVENT_DELIVER_START,
		// Token: 0x04000D7C RID: 3452
		ETW_TYPE_WEB_EVENT_DELIVER_END
	}
}

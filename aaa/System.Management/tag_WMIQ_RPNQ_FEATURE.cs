﻿using System;

namespace System.Management
{
	// Token: 0x020000F8 RID: 248
	internal enum tag_WMIQ_RPNQ_FEATURE
	{
		// Token: 0x0400050B RID: 1291
		WMIQ_RPNF_WHERE_CLAUSE_PRESENT = 1,
		// Token: 0x0400050C RID: 1292
		WMIQ_RPNF_QUERY_IS_CONJUNCTIVE,
		// Token: 0x0400050D RID: 1293
		WMIQ_RPNF_QUERY_IS_DISJUNCTIVE = 4,
		// Token: 0x0400050E RID: 1294
		WMIQ_RPNF_PROJECTION = 8,
		// Token: 0x0400050F RID: 1295
		WMIQ_RPNF_FEATURE_SELECT_STAR = 16,
		// Token: 0x04000510 RID: 1296
		WMIQ_RPNF_EQUALITY_TESTS_ONLY = 32,
		// Token: 0x04000511 RID: 1297
		WMIQ_RPNF_COUNT_STAR = 64,
		// Token: 0x04000512 RID: 1298
		WMIQ_RPNF_QUALIFIED_NAMES_IN_SELECT = 128,
		// Token: 0x04000513 RID: 1299
		WMIQ_RPNF_QUALIFIED_NAMES_IN_WHERE = 256,
		// Token: 0x04000514 RID: 1300
		WMIQ_RPNF_PROP_TO_PROP_TESTS = 512,
		// Token: 0x04000515 RID: 1301
		WMIQ_RPNF_ORDER_BY = 1024,
		// Token: 0x04000516 RID: 1302
		WMIQ_RPNF_ISA_USED = 2048,
		// Token: 0x04000517 RID: 1303
		WMIQ_RPNF_ISNOTA_USED = 4096,
		// Token: 0x04000518 RID: 1304
		WMIQ_RPNF_GROUP_BY_HAVING = 8192,
		// Token: 0x04000519 RID: 1305
		WMIQ_RPNF_WITHIN_INTERVAL = 16384,
		// Token: 0x0400051A RID: 1306
		WMIQ_RPNF_WITHIN_AGGREGATE = 32768,
		// Token: 0x0400051B RID: 1307
		WMIQ_RPNF_SYSPROP_CLASS = 65536,
		// Token: 0x0400051C RID: 1308
		WMIQ_RPNF_REFERENCE_TESTS = 131072,
		// Token: 0x0400051D RID: 1309
		WMIQ_RPNF_DATETIME_TESTS = 262144,
		// Token: 0x0400051E RID: 1310
		WMIQ_RPNF_ARRAY_ACCESS = 524288,
		// Token: 0x0400051F RID: 1311
		WMIQ_RPNF_QUALIFIER_FILTER = 1048576,
		// Token: 0x04000520 RID: 1312
		WMIQ_RPNF_SELECTED_FROM_PATH = 2097152
	}
}

﻿using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000033 RID: 51
	internal enum SAPIErrorCodes
	{
		// Token: 0x04000118 RID: 280
		S_OK,
		// Token: 0x04000119 RID: 281
		S_FALSE,
		// Token: 0x0400011A RID: 282
		SP_NO_RULE_ACTIVE = 282709,
		// Token: 0x0400011B RID: 283
		SP_NO_RULES_TO_ACTIVATE = 282747,
		// Token: 0x0400011C RID: 284
		S_LIMIT_REACHED = 282751,
		// Token: 0x0400011D RID: 285
		E_FAIL = -2147467259,
		// Token: 0x0400011E RID: 286
		SP_NO_PARSE_FOUND = 282668,
		// Token: 0x0400011F RID: 287
		SP_WORD_EXISTS_WITHOUT_PRONUNCIATION = 282679,
		// Token: 0x04000120 RID: 288
		SPERR_FIRST = -2147201023,
		// Token: 0x04000121 RID: 289
		SPERR_LAST = -2147200890,
		// Token: 0x04000122 RID: 290
		STG_E_FILENOTFOUND = -2147287038,
		// Token: 0x04000123 RID: 291
		CLASS_E_CLASSNOTAVAILABLE = -2147221231,
		// Token: 0x04000124 RID: 292
		REGDB_E_CLASSNOTREG = -2147221164,
		// Token: 0x04000125 RID: 293
		SPERR_UNSUPPORTED_FORMAT = -2147201021,
		// Token: 0x04000126 RID: 294
		SPERR_UNSUPPORTED_PHONEME = -2147200902,
		// Token: 0x04000127 RID: 295
		SPERR_VOICE_NOT_FOUND = -2147200877,
		// Token: 0x04000128 RID: 296
		SPERR_NOT_IN_LEX = -2147200999,
		// Token: 0x04000129 RID: 297
		SPERR_TOO_MANY_GRAMMARS = -2147200990,
		// Token: 0x0400012A RID: 298
		SPERR_INVALID_IMPORT = -2147200988,
		// Token: 0x0400012B RID: 299
		SPERR_STREAM_CLOSED = -2147200968,
		// Token: 0x0400012C RID: 300
		SPERR_NO_MORE_ITEMS,
		// Token: 0x0400012D RID: 301
		SPERR_NOT_FOUND,
		// Token: 0x0400012E RID: 302
		SPERR_NOT_TOPLEVEL_RULE = -2147200940,
		// Token: 0x0400012F RID: 303
		SPERR_SHARED_ENGINE_DISABLED = -2147200906,
		// Token: 0x04000130 RID: 304
		SPERR_RECOGNIZER_NOT_FOUND,
		// Token: 0x04000131 RID: 305
		SPERR_AUDIO_NOT_FOUND,
		// Token: 0x04000132 RID: 306
		SPERR_NOT_SUPPORTED_FOR_INPROC_RECOGNIZER = -2147200893,
		// Token: 0x04000133 RID: 307
		SPERR_LEX_INVALID_DATA = -2147200891,
		// Token: 0x04000134 RID: 308
		SPERR_CFG_INVALID_DATA
	}
}

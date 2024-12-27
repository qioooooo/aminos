﻿using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200002E RID: 46
	internal enum SPEVENTENUM : ushort
	{
		// Token: 0x040000EC RID: 236
		SPEI_UNDEFINED,
		// Token: 0x040000ED RID: 237
		SPEI_START_INPUT_STREAM,
		// Token: 0x040000EE RID: 238
		SPEI_END_INPUT_STREAM,
		// Token: 0x040000EF RID: 239
		SPEI_VOICE_CHANGE,
		// Token: 0x040000F0 RID: 240
		SPEI_TTS_BOOKMARK,
		// Token: 0x040000F1 RID: 241
		SPEI_WORD_BOUNDARY,
		// Token: 0x040000F2 RID: 242
		SPEI_PHONEME,
		// Token: 0x040000F3 RID: 243
		SPEI_SENTENCE_BOUNDARY,
		// Token: 0x040000F4 RID: 244
		SPEI_VISEME,
		// Token: 0x040000F5 RID: 245
		SPEI_TTS_AUDIO_LEVEL,
		// Token: 0x040000F6 RID: 246
		SPEI_TTS_PRIVATE = 15,
		// Token: 0x040000F7 RID: 247
		SPEI_MIN_TTS = 1,
		// Token: 0x040000F8 RID: 248
		SPEI_MAX_TTS = 15,
		// Token: 0x040000F9 RID: 249
		SPEI_END_SR_STREAM = 34,
		// Token: 0x040000FA RID: 250
		SPEI_SOUND_START,
		// Token: 0x040000FB RID: 251
		SPEI_SOUND_END,
		// Token: 0x040000FC RID: 252
		SPEI_PHRASE_START,
		// Token: 0x040000FD RID: 253
		SPEI_RECOGNITION,
		// Token: 0x040000FE RID: 254
		SPEI_HYPOTHESIS,
		// Token: 0x040000FF RID: 255
		SPEI_SR_BOOKMARK,
		// Token: 0x04000100 RID: 256
		SPEI_PROPERTY_NUM_CHANGE,
		// Token: 0x04000101 RID: 257
		SPEI_PROPERTY_STRING_CHANGE,
		// Token: 0x04000102 RID: 258
		SPEI_FALSE_RECOGNITION,
		// Token: 0x04000103 RID: 259
		SPEI_INTERFERENCE,
		// Token: 0x04000104 RID: 260
		SPEI_REQUEST_UI,
		// Token: 0x04000105 RID: 261
		SPEI_RECO_STATE_CHANGE,
		// Token: 0x04000106 RID: 262
		SPEI_ADAPTATION,
		// Token: 0x04000107 RID: 263
		SPEI_START_SR_STREAM,
		// Token: 0x04000108 RID: 264
		SPEI_RECO_OTHER_CONTEXT,
		// Token: 0x04000109 RID: 265
		SPEI_SR_AUDIO_LEVEL,
		// Token: 0x0400010A RID: 266
		SPEI_SR_RETAINEDAUDIO,
		// Token: 0x0400010B RID: 267
		SPEI_SR_PRIVATE,
		// Token: 0x0400010C RID: 268
		SPEI_ACTIVE_CATEGORY_CHANGED,
		// Token: 0x0400010D RID: 269
		SPEI_TEXTFEEDBACK,
		// Token: 0x0400010E RID: 270
		SPEI_RECOGNITION_ALL,
		// Token: 0x0400010F RID: 271
		SPEI_BARGE_IN,
		// Token: 0x04000110 RID: 272
		SPEI_RESERVED1 = 30,
		// Token: 0x04000111 RID: 273
		SPEI_RESERVED2 = 33,
		// Token: 0x04000112 RID: 274
		SPEI_RESERVED3 = 63
	}
}

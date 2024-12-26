using System;

namespace Microsoft.JScript
{
	// Token: 0x02000033 RID: 51
	public enum JSBuiltin
	{
		// Token: 0x040000A0 RID: 160
		None,
		// Token: 0x040000A1 RID: 161
		Array_concat,
		// Token: 0x040000A2 RID: 162
		Array_join,
		// Token: 0x040000A3 RID: 163
		Array_pop,
		// Token: 0x040000A4 RID: 164
		Array_push,
		// Token: 0x040000A5 RID: 165
		Array_reverse,
		// Token: 0x040000A6 RID: 166
		Array_shift,
		// Token: 0x040000A7 RID: 167
		Array_slice,
		// Token: 0x040000A8 RID: 168
		Array_sort,
		// Token: 0x040000A9 RID: 169
		Array_splice,
		// Token: 0x040000AA RID: 170
		Array_toLocaleString,
		// Token: 0x040000AB RID: 171
		Array_toString,
		// Token: 0x040000AC RID: 172
		Array_unshift,
		// Token: 0x040000AD RID: 173
		Boolean_toString,
		// Token: 0x040000AE RID: 174
		Boolean_valueOf,
		// Token: 0x040000AF RID: 175
		Date_getDate,
		// Token: 0x040000B0 RID: 176
		Date_getDay,
		// Token: 0x040000B1 RID: 177
		Date_getFullYear,
		// Token: 0x040000B2 RID: 178
		Date_getHours,
		// Token: 0x040000B3 RID: 179
		Date_getMilliseconds,
		// Token: 0x040000B4 RID: 180
		Date_getMinutes,
		// Token: 0x040000B5 RID: 181
		Date_getMonth,
		// Token: 0x040000B6 RID: 182
		Date_getSeconds,
		// Token: 0x040000B7 RID: 183
		Date_getTime,
		// Token: 0x040000B8 RID: 184
		Date_getTimezoneOffset,
		// Token: 0x040000B9 RID: 185
		Date_getUTCDate,
		// Token: 0x040000BA RID: 186
		Date_getUTCDay,
		// Token: 0x040000BB RID: 187
		Date_getUTCFullYear,
		// Token: 0x040000BC RID: 188
		Date_getUTCHours,
		// Token: 0x040000BD RID: 189
		Date_getUTCMilliseconds,
		// Token: 0x040000BE RID: 190
		Date_getUTCMinutes,
		// Token: 0x040000BF RID: 191
		Date_getUTCMonth,
		// Token: 0x040000C0 RID: 192
		Date_getUTCSeconds,
		// Token: 0x040000C1 RID: 193
		Date_getVarDate,
		// Token: 0x040000C2 RID: 194
		Date_getYear,
		// Token: 0x040000C3 RID: 195
		Date_parse,
		// Token: 0x040000C4 RID: 196
		Date_setDate,
		// Token: 0x040000C5 RID: 197
		Date_setFullYear,
		// Token: 0x040000C6 RID: 198
		Date_setHours,
		// Token: 0x040000C7 RID: 199
		Date_setMinutes,
		// Token: 0x040000C8 RID: 200
		Date_setMilliseconds,
		// Token: 0x040000C9 RID: 201
		Date_setMonth,
		// Token: 0x040000CA RID: 202
		Date_setSeconds,
		// Token: 0x040000CB RID: 203
		Date_setTime,
		// Token: 0x040000CC RID: 204
		Date_setUTCDate,
		// Token: 0x040000CD RID: 205
		Date_setUTCFullYear,
		// Token: 0x040000CE RID: 206
		Date_setUTCHours,
		// Token: 0x040000CF RID: 207
		Date_setUTCMinutes,
		// Token: 0x040000D0 RID: 208
		Date_setUTCMilliseconds,
		// Token: 0x040000D1 RID: 209
		Date_setUTCMonth,
		// Token: 0x040000D2 RID: 210
		Date_setUTCSeconds,
		// Token: 0x040000D3 RID: 211
		Date_setYear,
		// Token: 0x040000D4 RID: 212
		Date_toDateString,
		// Token: 0x040000D5 RID: 213
		Date_toGMTString,
		// Token: 0x040000D6 RID: 214
		Date_toLocaleDateString,
		// Token: 0x040000D7 RID: 215
		Date_toLocaleString,
		// Token: 0x040000D8 RID: 216
		Date_toLocaleTimeString,
		// Token: 0x040000D9 RID: 217
		Date_toString,
		// Token: 0x040000DA RID: 218
		Date_toTimeString,
		// Token: 0x040000DB RID: 219
		Date_toUTCString,
		// Token: 0x040000DC RID: 220
		Date_UTC,
		// Token: 0x040000DD RID: 221
		Date_valueOf,
		// Token: 0x040000DE RID: 222
		Enumerator_atEnd,
		// Token: 0x040000DF RID: 223
		Enumerator_item,
		// Token: 0x040000E0 RID: 224
		Enumerator_moveFirst,
		// Token: 0x040000E1 RID: 225
		Enumerator_moveNext,
		// Token: 0x040000E2 RID: 226
		Error_toString,
		// Token: 0x040000E3 RID: 227
		Function_apply,
		// Token: 0x040000E4 RID: 228
		Function_call,
		// Token: 0x040000E5 RID: 229
		Function_toString,
		// Token: 0x040000E6 RID: 230
		Global_CollectGarbage,
		// Token: 0x040000E7 RID: 231
		Global_decodeURI,
		// Token: 0x040000E8 RID: 232
		Global_decodeURIComponent,
		// Token: 0x040000E9 RID: 233
		Global_encodeURI,
		// Token: 0x040000EA RID: 234
		Global_encodeURIComponent,
		// Token: 0x040000EB RID: 235
		Global_escape,
		// Token: 0x040000EC RID: 236
		Global_eval,
		// Token: 0x040000ED RID: 237
		Global_GetObject,
		// Token: 0x040000EE RID: 238
		Global_isNaN,
		// Token: 0x040000EF RID: 239
		Global_isFinite,
		// Token: 0x040000F0 RID: 240
		Global_parseFloat,
		// Token: 0x040000F1 RID: 241
		Global_parseInt,
		// Token: 0x040000F2 RID: 242
		Global_ScriptEngine,
		// Token: 0x040000F3 RID: 243
		Global_ScriptEngineBuildVersion,
		// Token: 0x040000F4 RID: 244
		Global_ScriptEngineMajorVersion,
		// Token: 0x040000F5 RID: 245
		Global_ScriptEngineMinorVersion,
		// Token: 0x040000F6 RID: 246
		Global_unescape,
		// Token: 0x040000F7 RID: 247
		Math_abs,
		// Token: 0x040000F8 RID: 248
		Math_acos,
		// Token: 0x040000F9 RID: 249
		Math_asin,
		// Token: 0x040000FA RID: 250
		Math_atan,
		// Token: 0x040000FB RID: 251
		Math_atan2,
		// Token: 0x040000FC RID: 252
		Math_ceil,
		// Token: 0x040000FD RID: 253
		Math_cos,
		// Token: 0x040000FE RID: 254
		Math_exp,
		// Token: 0x040000FF RID: 255
		Math_floor,
		// Token: 0x04000100 RID: 256
		Math_log,
		// Token: 0x04000101 RID: 257
		Math_max,
		// Token: 0x04000102 RID: 258
		Math_min,
		// Token: 0x04000103 RID: 259
		Math_pow,
		// Token: 0x04000104 RID: 260
		Math_random,
		// Token: 0x04000105 RID: 261
		Math_round,
		// Token: 0x04000106 RID: 262
		Math_sin,
		// Token: 0x04000107 RID: 263
		Math_sqrt,
		// Token: 0x04000108 RID: 264
		Math_tan,
		// Token: 0x04000109 RID: 265
		Number_toExponential,
		// Token: 0x0400010A RID: 266
		Number_toFixed,
		// Token: 0x0400010B RID: 267
		Number_toLocaleString,
		// Token: 0x0400010C RID: 268
		Number_toPrecision,
		// Token: 0x0400010D RID: 269
		Number_toString,
		// Token: 0x0400010E RID: 270
		Number_valueOf,
		// Token: 0x0400010F RID: 271
		Object_hasOwnProperty,
		// Token: 0x04000110 RID: 272
		Object_isPrototypeOf,
		// Token: 0x04000111 RID: 273
		Object_propertyIsEnumerable,
		// Token: 0x04000112 RID: 274
		Object_toLocaleString,
		// Token: 0x04000113 RID: 275
		Object_toString,
		// Token: 0x04000114 RID: 276
		Object_valueOf,
		// Token: 0x04000115 RID: 277
		RegExp_compile,
		// Token: 0x04000116 RID: 278
		RegExp_exec,
		// Token: 0x04000117 RID: 279
		RegExp_test,
		// Token: 0x04000118 RID: 280
		RegExp_toString,
		// Token: 0x04000119 RID: 281
		String_anchor,
		// Token: 0x0400011A RID: 282
		String_big,
		// Token: 0x0400011B RID: 283
		String_blink,
		// Token: 0x0400011C RID: 284
		String_bold,
		// Token: 0x0400011D RID: 285
		String_charAt,
		// Token: 0x0400011E RID: 286
		String_charCodeAt,
		// Token: 0x0400011F RID: 287
		String_concat,
		// Token: 0x04000120 RID: 288
		String_fixed,
		// Token: 0x04000121 RID: 289
		String_fontcolor,
		// Token: 0x04000122 RID: 290
		String_fontsize,
		// Token: 0x04000123 RID: 291
		String_fromCharCode,
		// Token: 0x04000124 RID: 292
		String_indexOf,
		// Token: 0x04000125 RID: 293
		String_italics,
		// Token: 0x04000126 RID: 294
		String_lastIndexOf,
		// Token: 0x04000127 RID: 295
		String_link,
		// Token: 0x04000128 RID: 296
		String_localeCompare,
		// Token: 0x04000129 RID: 297
		String_match,
		// Token: 0x0400012A RID: 298
		String_replace,
		// Token: 0x0400012B RID: 299
		String_search,
		// Token: 0x0400012C RID: 300
		String_slice,
		// Token: 0x0400012D RID: 301
		String_small,
		// Token: 0x0400012E RID: 302
		String_split,
		// Token: 0x0400012F RID: 303
		String_strike,
		// Token: 0x04000130 RID: 304
		String_sub,
		// Token: 0x04000131 RID: 305
		String_substr,
		// Token: 0x04000132 RID: 306
		String_substring,
		// Token: 0x04000133 RID: 307
		String_sup,
		// Token: 0x04000134 RID: 308
		String_toLocaleLowerCase,
		// Token: 0x04000135 RID: 309
		String_toLocaleUpperCase,
		// Token: 0x04000136 RID: 310
		String_toLowerCase,
		// Token: 0x04000137 RID: 311
		String_toString,
		// Token: 0x04000138 RID: 312
		String_toUpperCase,
		// Token: 0x04000139 RID: 313
		String_valueOf,
		// Token: 0x0400013A RID: 314
		VBArray_dimensions,
		// Token: 0x0400013B RID: 315
		VBArray_getItem,
		// Token: 0x0400013C RID: 316
		VBArray_lbound,
		// Token: 0x0400013D RID: 317
		VBArray_toArray,
		// Token: 0x0400013E RID: 318
		VBArray_ubound
	}
}

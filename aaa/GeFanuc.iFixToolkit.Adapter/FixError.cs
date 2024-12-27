using System;

namespace GeFanuc.iFixToolkit.Adapter
{
	// Token: 0x0200000C RID: 12
	public sealed class FixError
	{
		// Token: 0x06000041 RID: 65 RVA: 0x000025AF File Offset: 0x000015AF
		private FixError()
		{
		}

		// Token: 0x0400003A RID: 58
		public const short FE_OK = 0;

		// Token: 0x0400003B RID: 59
		public const short FE_DOS_FCN = 1;

		// Token: 0x0400003C RID: 60
		public const short FE_DOS_FNF = 2;

		// Token: 0x0400003D RID: 61
		public const short FE_DOS_PNF = 3;

		// Token: 0x0400003E RID: 62
		public const short FE_DOS_TMF = 4;

		// Token: 0x0400003F RID: 63
		public const short FE_DOS_ACC = 5;

		// Token: 0x04000040 RID: 64
		public const short FE_DOS_HND = 6;

		// Token: 0x04000041 RID: 65
		public const short FE_DOS_MCB = 7;

		// Token: 0x04000042 RID: 66
		public const short FE_DOS_MEM = 8;

		// Token: 0x04000043 RID: 67
		public const short FE_DOS_SPAWN = 9;

		// Token: 0x04000044 RID: 68
		public const short FE_DOS_DNF = 10;

		// Token: 0x04000045 RID: 69
		public const short FE_FIX_PROCESS_STILL_RUNNING = 89;

		// Token: 0x04000046 RID: 70
		public const short FE_VIRTUAL_MEMORY_LOW = 90;

		// Token: 0x04000047 RID: 71
		public const short FE_FIX_NOT_LOADED = 91;

		// Token: 0x04000048 RID: 72
		public const short FE_NT_WIN32_ERR = 92;

		// Token: 0x04000049 RID: 73
		public const short FE_DUP_PROC = 93;

		// Token: 0x0400004A RID: 74
		public const short FE_FIX_NOTSTARTED = 94;

		// Token: 0x0400004B RID: 75
		public const short FE_SYS_INTEG = 95;

		// Token: 0x0400004C RID: 76
		public const short FE_ERR_INTERN = 96;

		// Token: 0x0400004D RID: 77
		public const short FE_NO_EMS_MAPS = 97;

		// Token: 0x0400004E RID: 78
		public const short FE_NO_EMS_MEMORY = 98;

		// Token: 0x0400004F RID: 79
		public const short FE_MEMORY = 99;

		// Token: 0x04000050 RID: 80
		public const short FE_ERR = 100;

		// Token: 0x04000051 RID: 81
		public const short FE_OPEN = 101;

		// Token: 0x04000052 RID: 82
		public const short FE_CREATE = 102;

		// Token: 0x04000053 RID: 83
		public const short FE_UPDATE = 103;

		// Token: 0x04000054 RID: 84
		public const short FE_CLOSE = 104;

		// Token: 0x04000055 RID: 85
		public const short FE_READ = 105;

		// Token: 0x04000056 RID: 86
		public const short FE_WRITE = 106;

		// Token: 0x04000057 RID: 87
		public const short FE_DIR = 107;

		// Token: 0x04000058 RID: 88
		public const short FE_DEF = 108;

		// Token: 0x04000059 RID: 89
		public const short FE_VERERR = 109;

		// Token: 0x0400005A RID: 90
		public const short FE_FILEPATH = 110;

		// Token: 0x0400005B RID: 91
		public const short FE_DELETE = 111;

		// Token: 0x0400005C RID: 92
		public const short FE_FILE_NOTFOUND = 112;

		// Token: 0x0400005D RID: 93
		public const short FM_FILE_DELETED = 113;

		// Token: 0x0400005E RID: 94
		public const short FE_FILE_EXIST = 114;

		// Token: 0x0400005F RID: 95
		public const short FE_SEEK = 115;

		// Token: 0x04000060 RID: 96
		public const short FE_FKEY = 116;

		// Token: 0x04000061 RID: 97
		public const short FE_KEY_OPTION = 117;

		// Token: 0x04000062 RID: 98
		public const short FE_PRINTER_OFFLINE = 118;

		// Token: 0x04000063 RID: 99
		public const short FE_PRN_NOT_OPENED = 119;

		// Token: 0x04000064 RID: 100
		public const short FE_BACKUP_FAILED = 120;

		// Token: 0x04000065 RID: 101
		public const short FE_NO_DRIVE = 121;

		// Token: 0x04000066 RID: 102
		public const short FE_NO_DIR = 122;

		// Token: 0x04000067 RID: 103
		public const short FE_BAD_FILENAME = 123;

		// Token: 0x04000068 RID: 104
		public const short FE_FILE_RO = 124;

		// Token: 0x04000069 RID: 105
		public const short FE_FILE_NOT_EXIST = 125;

		// Token: 0x0400006A RID: 106
		public const short FE_SPECIFIX_OPTION = 126;

		// Token: 0x0400006B RID: 107
		public const short FE_DMACS_OPTION = 127;

		// Token: 0x0400006C RID: 108
		public const short FE_KEY_DEMO = 128;

		// Token: 0x0400006D RID: 109
		public const short FE_MISSING_KEY = 129;

		// Token: 0x0400006E RID: 110
		public const short FE_MISSING_SEC_FILE = 130;

		// Token: 0x0400006F RID: 111
		public const short FE_KEY_SEC_MISMATCH = 131;

		// Token: 0x04000070 RID: 112
		public const short FE_FATAL = 200;

		// Token: 0x04000071 RID: 113
		public const short FE_INTERNAL = 201;

		// Token: 0x04000072 RID: 114
		public const short FE_WARN = 202;

		// Token: 0x04000073 RID: 115
		public const short FE_ERROR = 203;

		// Token: 0x04000074 RID: 116
		public const short FE_MALLOC = 250;

		// Token: 0x04000075 RID: 117
		public const short FM_ASK_CLEAR_DEF = 251;

		// Token: 0x04000076 RID: 118
		public const short FM_RETURN = 300;

		// Token: 0x04000077 RID: 119
		public const short FM_FILNAM = 301;

		// Token: 0x04000078 RID: 120
		public const short FM_COMMAND = 302;

		// Token: 0x04000079 RID: 121
		public const short FM_INV_SEL = 304;

		// Token: 0x0400007A RID: 122
		public const short FM_HELP = 305;

		// Token: 0x0400007B RID: 123
		public const short FM_NO_DEF = 306;

		// Token: 0x0400007C RID: 124
		public const short FM_DIR_FILS = 307;

		// Token: 0x0400007D RID: 125
		public const short FM_PAGE = 308;

		// Token: 0x0400007E RID: 126
		public const short FM_PATH = 309;

		// Token: 0x0400007F RID: 127
		public const short FM_FILES = 310;

		// Token: 0x04000080 RID: 128
		public const short FM_ASK_SAVE = 311;

		// Token: 0x04000081 RID: 129
		public const short FM_VERSION = 312;

		// Token: 0x04000082 RID: 130
		public const short FM_FMMS = 313;

		// Token: 0x04000083 RID: 131
		public const short FM_LDBA = 314;

		// Token: 0x04000084 RID: 132
		public const short FM_GET_TAG = 315;

		// Token: 0x04000085 RID: 133
		public const short FM_BGN_EXIT = 316;

		// Token: 0x04000086 RID: 134
		public const short FM_MENU_INSERT = 317;

		// Token: 0x04000087 RID: 135
		public const short FM_WORKING = 318;

		// Token: 0x04000088 RID: 136
		public const short FM_FILESAVE = 319;

		// Token: 0x04000089 RID: 137
		public const short FM_IGNORE = 320;

		// Token: 0x0400008A RID: 138
		public const short FM_NEW_TAG = 321;

		// Token: 0x0400008B RID: 139
		public const short FM_EXIT = 322;

		// Token: 0x0400008C RID: 140
		public const short FM_DATA_EXIT = 323;

		// Token: 0x0400008D RID: 141
		public const short FM_MENU_OSTRIKE = 324;

		// Token: 0x0400008E RID: 142
		public const short FM_EXITING = 325;

		// Token: 0x0400008F RID: 143
		public const short FM_PASSWORD = 326;

		// Token: 0x04000090 RID: 144
		public const short FM_NO_FILES = 327;

		// Token: 0x04000091 RID: 145
		public const short FM_TOO_MANY_FILES = 328;

		// Token: 0x04000092 RID: 146
		public const short FM_NO_DIR_FOR_EXTEN = 329;

		// Token: 0x04000093 RID: 147
		public const short FM_FOR_NODE = 330;

		// Token: 0x04000094 RID: 148
		public const short FM_TRY_AGAIN = 331;

		// Token: 0x04000095 RID: 149
		public const short FM_USER_CANCEL = 332;

		// Token: 0x04000096 RID: 150
		public const short FM_USER_ANYWAY = 333;

		// Token: 0x04000097 RID: 151
		public const short FM_BY = 334;

		// Token: 0x04000098 RID: 152
		public const short FE_SAC_STOP = 402;

		// Token: 0x04000099 RID: 153
		public const short FE_DISABLE_SCAN_ON = 403;

		// Token: 0x0400009A RID: 154
		public const short FE_DISABLE_SCAN_OFF = 404;

		// Token: 0x0400009B RID: 155
		public const short FM_SAC_VERSION = 410;

		// Token: 0x0400009C RID: 156
		public const short FM_SAC_INIT = 411;

		// Token: 0x0400009D RID: 157
		public const short FM_SAC_WARM = 412;

		// Token: 0x0400009E RID: 158
		public const short FM_SAC_COLD = 413;

		// Token: 0x0400009F RID: 159
		public const short FM_SAC_OVER = 414;

		// Token: 0x040000A0 RID: 160
		public const short FM_SAC_FAIL = 415;

		// Token: 0x040000A1 RID: 161
		public const short FE_SAC_RELOAD = 416;

		// Token: 0x040000A2 RID: 162
		public const short FE_SAC_NEXT = 417;

		// Token: 0x040000A3 RID: 163
		public const short FE_SAC_OFF = 418;

		// Token: 0x040000A4 RID: 164
		public const short FE_SAC_DBB = 419;

		// Token: 0x040000A5 RID: 165
		public const short FE_SAC_LOOP = 420;

		// Token: 0x040000A6 RID: 166
		public const short FE_SAC_BROK = 421;

		// Token: 0x040000A7 RID: 167
		public const short FE_SAC_DATAFMT = 422;

		// Token: 0x040000A8 RID: 168
		public const short FE_SAC_BADSEC = 423;

		// Token: 0x040000A9 RID: 169
		public const short FE_PG_ERROR = 424;

		// Token: 0x040000AA RID: 170
		public const short FE_PG_RUN = 425;

		// Token: 0x040000AB RID: 171
		public const short FE_PG_STOP = 426;

		// Token: 0x040000AC RID: 172
		public const short FE_PG_ERRSTEP = 427;

		// Token: 0x040000AD RID: 173
		public const short FE_PG_MSGSTEP = 428;

		// Token: 0x040000AE RID: 174
		public const short FE_PG_CALL = 429;

		// Token: 0x040000AF RID: 175
		public const short FE_PG_STEP = 430;

		// Token: 0x040000B0 RID: 176
		public const short FE_PG_DEBUG = 431;

		// Token: 0x040000B1 RID: 177
		public const short FE_PG_OPTYP = 432;

		// Token: 0x040000B2 RID: 178
		public const short FE_EV_ERROR = 433;

		// Token: 0x040000B3 RID: 179
		public const short FE_EV_RUN = 434;

		// Token: 0x040000B4 RID: 180
		public const short FE_EV_STOP = 435;

		// Token: 0x040000B5 RID: 181
		public const short FE_EV_OPEN = 436;

		// Token: 0x040000B6 RID: 182
		public const short FE_EV_CLOSE = 437;

		// Token: 0x040000B7 RID: 183
		public const short FE_EV_OPTYP = 438;

		// Token: 0x040000B8 RID: 184
		public const short FE_EV_ALARM = 439;

		// Token: 0x040000B9 RID: 185
		public const short FE_SP_CLAMP = 440;

		// Token: 0x040000BA RID: 186
		public const short FE_TV_CLAMP = 441;

		// Token: 0x040000BB RID: 187
		public const short FE_SAC_SCANR = 442;

		// Token: 0x040000BC RID: 188
		public const short FE_SAC_ONSCAN = 443;

		// Token: 0x040000BD RID: 189
		public const short FE_SAC_NOEXCEPT = 444;

		// Token: 0x040000BE RID: 190
		public const short FE_SAC_BADCHAIN = 445;

		// Token: 0x040000BF RID: 191
		public const short FE_SAC_STOPPED = 446;

		// Token: 0x040000C0 RID: 192
		public const short FE_SAC_RESTART = 447;

		// Token: 0x040000C1 RID: 193
		public const short FE_SAC_PANIC = 448;

		// Token: 0x040000C2 RID: 194
		public const short FE_TM_LOGIC = 449;

		// Token: 0x040000C3 RID: 195
		public const short FE_PG_BADSTEP = 450;

		// Token: 0x040000C4 RID: 196
		public const short FE_SD_WAIT = 451;

		// Token: 0x040000C5 RID: 197
		public const short FM_SAC_ALM_RES = 452;

		// Token: 0x040000C6 RID: 198
		public const short FM_SAC_ALM_SUS = 453;

		// Token: 0x040000C7 RID: 199
		public const short FE_SAC_ERR_STP = 454;

		// Token: 0x040000C8 RID: 200
		public const short FE_SAC_INP_PAT = 455;

		// Token: 0x040000C9 RID: 201
		public const short FE_SAC_STP = 456;

		// Token: 0x040000CA RID: 202
		public const short FE_SAC_CUR_PAT = 457;

		// Token: 0x040000CB RID: 203
		public const short FE_SAC_NXT_STP = 458;

		// Token: 0x040000CC RID: 204
		public const short FE_SAC_IOERR_STP = 459;

		// Token: 0x040000CD RID: 205
		public const short FE_SAC_FOR_BIT = 460;

		// Token: 0x040000CE RID: 206
		public const short FE_SAC_RTN = 461;

		// Token: 0x040000CF RID: 207
		public const short FE_SAC_ADJUST = 462;

		// Token: 0x040000D0 RID: 208
		public const short FE_SAC_NXT_BLK = 463;

		// Token: 0x040000D1 RID: 209
		public const short FE_SAC_RESET = 464;

		// Token: 0x040000D2 RID: 210
		public const short FE_SAC_IGNORE = 465;

		// Token: 0x040000D3 RID: 211
		public const short FE_SAC_PER_DEF = 466;

		// Token: 0x040000D4 RID: 212
		public const short FE_SAC_TIM_DEF = 467;

		// Token: 0x040000D5 RID: 213
		public const short FE_SAC_SET_CON = 468;

		// Token: 0x040000D6 RID: 214
		public const short FM_SAC_ILL_TAR = 469;

		// Token: 0x040000D7 RID: 215
		public const short FM_SAC_BLK_MAX = 470;

		// Token: 0x040000D8 RID: 216
		public const short FM_SAC_BLK_SEC = 471;

		// Token: 0x040000D9 RID: 217
		public const short FM_SAC_BLK_RES = 472;

		// Token: 0x040000DA RID: 218
		public const short FM_SAC_TVC_RES = 473;

		// Token: 0x040000DB RID: 219
		public const short FE_ERR_FROM_DRV = 474;

		// Token: 0x040000DC RID: 220
		public const short FE_SAC_UNSUP_BLK = 475;

		// Token: 0x040000DD RID: 221
		public const short FE_EXCEPT_TIME = 476;

		// Token: 0x040000DE RID: 222
		public const short FM_SAC_REMOTE_ACK = 477;

		// Token: 0x040000DF RID: 223
		public const short FE_IO_ABORT = 500;

		// Token: 0x040000E0 RID: 224
		public const short FE_IO_FULL = 501;

		// Token: 0x040000E1 RID: 225
		public const short FE_IO_DUP = 502;

		// Token: 0x040000E2 RID: 226
		public const short FE_IO_ERR = 510;

		// Token: 0x040000E3 RID: 227
		public const short FE_IO_BUSY = 511;

		// Token: 0x040000E4 RID: 228
		public const short FE_IO_WAIT = 512;

		// Token: 0x040000E5 RID: 229
		public const short FE_IO_DCB = 513;

		// Token: 0x040000E6 RID: 230
		public const short FE_IO_ADDR = 514;

		// Token: 0x040000E7 RID: 231
		public const short FE_IO_OPT = 515;

		// Token: 0x040000E8 RID: 232
		public const short FE_IO_SIG = 516;

		// Token: 0x040000E9 RID: 233
		public const short FE_IO_EGU = 517;

		// Token: 0x040000EA RID: 234
		public const short FE_IO_COMM = 518;

		// Token: 0x040000EB RID: 235
		public const short FE_IO_DATAFMT = 519;

		// Token: 0x040000EC RID: 236
		public const short FE_IO_ACCEPTED = 520;

		// Token: 0x040000ED RID: 237
		public const short FE_IO_NOEXCEPT = 521;

		// Token: 0x040000EE RID: 238
		public const short FE_IO_DUP_EXCEPT = 522;

		// Token: 0x040000EF RID: 239
		public const short FE_IO_NODIT = 523;

		// Token: 0x040000F0 RID: 240
		public const short FE_IO_DEV = 530;

		// Token: 0x040000F1 RID: 241
		public const short FE_IO_SEP = 531;

		// Token: 0x040000F2 RID: 242
		public const short FE_IO_1_ADDR = 532;

		// Token: 0x040000F3 RID: 243
		public const short FE_IO_2_ADDR = 533;

		// Token: 0x040000F4 RID: 244
		public const short FE_IO_3_ADDR = 534;

		// Token: 0x040000F5 RID: 245
		public const short FE_IO_4_ADDR = 535;

		// Token: 0x040000F6 RID: 246
		public const short FE_IO_5_ADDR = 536;

		// Token: 0x040000F7 RID: 247
		public const short FE_IO_6_ADDR = 537;

		// Token: 0x040000F8 RID: 248
		public const short FE_IO_7_ADDR = 538;

		// Token: 0x040000F9 RID: 249
		public const short FE_IO_8_ADDR = 539;

		// Token: 0x040000FA RID: 250
		public const short FE_IO_9_ADDR = 540;

		// Token: 0x040000FB RID: 251
		public const short FE_IO_10_ADDR = 541;

		// Token: 0x040000FC RID: 252
		public const short FE_IOC_CREATE_DRIVER = 600;

		// Token: 0x040000FD RID: 253
		public const short FE_IOC_START_DRIVER = 601;

		// Token: 0x040000FE RID: 254
		public const short FE_IOC_STOP_DRIVER = 602;

		// Token: 0x040000FF RID: 255
		public const short FE_IOC_TERMINATE_DRIVER = 603;

		// Token: 0x04000100 RID: 256
		public const short FE_IOC_MSLOT_CREATE = 604;

		// Token: 0x04000101 RID: 257
		public const short FE_IOC_MSLOT_WRITE = 605;

		// Token: 0x04000102 RID: 258
		public const short FE_RTL_LOST = 1001;

		// Token: 0x04000103 RID: 259
		public const short FE_RTL_SIZE = 1002;

		// Token: 0x04000104 RID: 260
		public const short FE_RTL_LIMIT = 1003;

		// Token: 0x04000105 RID: 261
		public const short FE_RTL_VERSION = 1004;

		// Token: 0x04000106 RID: 262
		public const short FE_RTL_INTERNAL = 1005;

		// Token: 0x04000107 RID: 263
		public const short FM_RTL_WARM = 1006;

		// Token: 0x04000108 RID: 264
		public const short FM_RTL_LOAD = 1007;

		// Token: 0x04000109 RID: 265
		public const short FM_RTL_RELOAD = 1008;

		// Token: 0x0400010A RID: 266
		public const short FE_RTL_EXIST = 1009;

		// Token: 0x0400010B RID: 267
		public const short FE_NO_TDBSLOTS = 1010;

		// Token: 0x0400010C RID: 268
		public const short FE_UPL_MISMATCH = 1011;

		// Token: 0x0400010D RID: 269
		public const short FE_RTL_FILENAME = 1012;

		// Token: 0x0400010E RID: 270
		public const short FE_RTL_NOPATH = 1013;

		// Token: 0x0400010F RID: 271
		public const short FE_NO87 = 1014;

		// Token: 0x04000110 RID: 272
		public const short FE_TAG_GROUP_NOT_RESOLVED = 1072;

		// Token: 0x04000111 RID: 273
		public const short FE_I_WAS_A_VARIABLE = 1073;

		// Token: 0x04000112 RID: 274
		public const short FE_OLD_NO_SVC = 1101;

		// Token: 0x04000113 RID: 275
		public const short FE_NO_FN = 1102;

		// Token: 0x04000114 RID: 276
		public const short FE_NO_LIB = 1103;

		// Token: 0x04000115 RID: 277
		public const short FE_LIB_RES = 1104;

		// Token: 0x04000116 RID: 278
		public const short FE_NO_FCA = 1105;

		// Token: 0x04000117 RID: 279
		public const short FE_TSR_CONTINUE = 1106;

		// Token: 0x04000118 RID: 280
		public const short FE_NO_SVC = 1150;

		// Token: 0x04000119 RID: 281
		public const short FE_NO_SVC_TST = 1151;

		// Token: 0x0400011A RID: 282
		public const short FE_NO_SVC_FMMS = 1152;

		// Token: 0x0400011B RID: 283
		public const short FE_NO_SVC_LDBA = 1153;

		// Token: 0x0400011C RID: 284
		public const short FE_NO_SVC_DRV0A = 1154;

		// Token: 0x0400011D RID: 285
		public const short FE_NO_SVC_DRV0B = 1155;

		// Token: 0x0400011E RID: 286
		public const short FE_NO_SVC_DRV1A = 1156;

		// Token: 0x0400011F RID: 287
		public const short FE_NO_SVC_DRV1B = 1157;

		// Token: 0x04000120 RID: 288
		public const short FE_NO_SVC_DRV2A = 1158;

		// Token: 0x04000121 RID: 289
		public const short FE_NO_SVC_DRV2B = 1159;

		// Token: 0x04000122 RID: 290
		public const short FE_NO_SVC_DRV3A = 1160;

		// Token: 0x04000123 RID: 291
		public const short FE_NO_SVC_DRV3B = 1161;

		// Token: 0x04000124 RID: 292
		public const short FE_NO_SVC_DRV4A = 1162;

		// Token: 0x04000125 RID: 293
		public const short FE_NO_SVC_DRV4B = 1163;

		// Token: 0x04000126 RID: 294
		public const short FE_NO_SVC_DRV5A = 1164;

		// Token: 0x04000127 RID: 295
		public const short FE_NO_SVC_DRV5B = 1165;

		// Token: 0x04000128 RID: 296
		public const short FE_NO_SVC_DRV6A = 1166;

		// Token: 0x04000129 RID: 297
		public const short FE_NO_SVC_DRV6B = 1167;

		// Token: 0x0400012A RID: 298
		public const short FE_NO_SVC_DRV7A = 1168;

		// Token: 0x0400012B RID: 299
		public const short FE_NO_SVC_DRV7B = 1169;

		// Token: 0x0400012C RID: 300
		public const short FE_NO_SVC_NET = 1170;

		// Token: 0x0400012D RID: 301
		public const short FE_NO_SVC_RDBA = 1171;

		// Token: 0x0400012E RID: 302
		public const short FE_NO_SVC_SACAPI = 1172;

		// Token: 0x0400012F RID: 303
		public const short FE_NO_SVC_ALM = 1173;

		// Token: 0x04000130 RID: 304
		public const short FE_NO_SVC_NNT = 1174;

		// Token: 0x04000131 RID: 305
		public const short FE_NO_SVC_ART = 1175;

		// Token: 0x04000132 RID: 306
		public const short FE_NO_SVC_FIO = 1176;

		// Token: 0x04000133 RID: 307
		public const short FE_NO_SVC_SAM = 1177;

		// Token: 0x04000134 RID: 308
		public const short FE_NO_SVC_TIM = 1178;

		// Token: 0x04000135 RID: 309
		public const short FE_NO_SVC_KEY = 1179;

		// Token: 0x04000136 RID: 310
		public const short FE_NO_SVC_LOK = 1180;

		// Token: 0x04000137 RID: 311
		public const short FE_FMMS_LIB_RES = 1181;

		// Token: 0x04000138 RID: 312
		public const short FE_NO_SVC_DT = 1183;

		// Token: 0x04000139 RID: 313
		public const short FE_NO_SVC_MDBA = 1184;

		// Token: 0x0400013A RID: 314
		public const short FE_MDBA_LIB_RES = 1185;

		// Token: 0x0400013B RID: 315
		public const short FE_ILL_FDT = 1201;

		// Token: 0x0400013C RID: 316
		public const short FE_BLK_ID = 1202;

		// Token: 0x0400013D RID: 317
		public const short FE_FDT_IP = 1203;

		// Token: 0x0400013E RID: 318
		public const short FE_FDT_TSIZ = 1204;

		// Token: 0x0400013F RID: 319
		public const short FE_FDT_TBAD = 1205;

		// Token: 0x04000140 RID: 320
		public const short FE_FDT_BSIZ = 1206;

		// Token: 0x04000141 RID: 321
		public const short FE_FDT_NONE = 1207;

		// Token: 0x04000142 RID: 322
		public const short FE_FDT_TYPE = 1208;

		// Token: 0x04000143 RID: 323
		public const short FE_FDT_FLD = 1209;

		// Token: 0x04000144 RID: 324
		public const short FE_FDT_IPN = 1210;

		// Token: 0x04000145 RID: 325
		public const short FE_NO_INIT = 1211;

		// Token: 0x04000146 RID: 326
		public const short FE_FDT_DATA = 1212;

		// Token: 0x04000147 RID: 327
		public const short FE_FDT_DP = 1213;

		// Token: 0x04000148 RID: 328
		public const short FE_NO_CHECK = 1214;

		// Token: 0x04000149 RID: 329
		public const short FE_EXP_SYN = 1220;

		// Token: 0x0400014A RID: 330
		public const short FE_EXP_LPAR = 1221;

		// Token: 0x0400014B RID: 331
		public const short FE_EXP_RPAR = 1222;

		// Token: 0x0400014C RID: 332
		public const short FE_EXP_TERM = 1223;

		// Token: 0x0400014D RID: 333
		public const short FE_EXP_CMPLX = 1224;

		// Token: 0x0400014E RID: 334
		public const short FE_EXP_OPER = 1225;

		// Token: 0x0400014F RID: 335
		public const short FE_FLT_FLD = 1226;

		// Token: 0x04000150 RID: 336
		public const short FE_FDT_TAGL = 1227;

		// Token: 0x04000151 RID: 337
		public const short FE_FDT_BLOCK = 1228;

		// Token: 0x04000152 RID: 338
		public const short FE_SYN_SYM = 1229;

		// Token: 0x04000153 RID: 339
		public const short FE_PPARM_NUM = 1230;

		// Token: 0x04000154 RID: 340
		public const short FE_PPARM_DEF = 1231;

		// Token: 0x04000155 RID: 341
		public const short FE_LUVAR_NUM = 1232;

		// Token: 0x04000156 RID: 342
		public const short FE_LUVAR_TAG = 1233;

		// Token: 0x04000157 RID: 343
		public const short FE_TIME_NEG = 1234;

		// Token: 0x04000158 RID: 344
		public const short FE_RES_CSTEP = 1235;

		// Token: 0x04000159 RID: 345
		public const short FE_ALM_NOTFOUND = 1236;

		// Token: 0x0400015A RID: 346
		public const short FE_OPC_UNC_Q = 1250;

		// Token: 0x0400015B RID: 347
		public const short FE_STK1 = 1251;

		// Token: 0x0400015C RID: 348
		public const short FE_STK2 = 1252;

		// Token: 0x0400015D RID: 349
		public const short FE_STK3 = 1253;

		// Token: 0x0400015E RID: 350
		public const short FE_STK4 = 1254;

		// Token: 0x0400015F RID: 351
		public const short FE_STK5 = 1255;

		// Token: 0x04000160 RID: 352
		public const short FE_STK6 = 1256;

		// Token: 0x04000161 RID: 353
		public const short FE_STK7 = 1257;

		// Token: 0x04000162 RID: 354
		public const short FE_STK8 = 1258;

		// Token: 0x04000163 RID: 355
		public const short FE_STK9 = 1259;

		// Token: 0x04000164 RID: 356
		public const short FE_STK10 = 1260;

		// Token: 0x04000165 RID: 357
		public const short FE_STRL = 1301;

		// Token: 0x04000166 RID: 358
		public const short FE_NODE_INS = 1302;

		// Token: 0x04000167 RID: 359
		public const short FE_TAG_INS = 1303;

		// Token: 0x04000168 RID: 360
		public const short FE_FLD_INS = 1304;

		// Token: 0x04000169 RID: 361
		public const short FM_NODE_TITLE = 1305;

		// Token: 0x0400016A RID: 362
		public const short FM_TAG_TITLE = 1306;

		// Token: 0x0400016B RID: 363
		public const short FM_FIELD_TITLE = 1307;

		// Token: 0x0400016C RID: 364
		public const short FE_EMPTY_DATABASE = 1308;

		// Token: 0x0400016D RID: 365
		public const short FE_NO_FIELDS = 1309;

		// Token: 0x0400016E RID: 366
		public const short FE_NO_NODES = 1310;

		// Token: 0x0400016F RID: 367
		public const short FE_NODE_ERR = 1311;

		// Token: 0x04000170 RID: 368
		public const short FE_TAG_ERR = 1312;

		// Token: 0x04000171 RID: 369
		public const short FE_FIELD_ERR = 1313;

		// Token: 0x04000172 RID: 370
		public const short FE_FLDTYP_ERR = 1314;

		// Token: 0x04000173 RID: 371
		public const short FM_SYSTEM_TITLE = 1315;

		// Token: 0x04000174 RID: 372
		public const short FE_NO_DRIVERS = 1316;

		// Token: 0x04000175 RID: 373
		public const short FM_DRIVER_TITLE = 1317;

		// Token: 0x04000176 RID: 374
		public const short FE_ACE_NO_TIMERS = 1318;

		// Token: 0x04000177 RID: 375
		public const short FE_ACE_PRIORITY = 1319;

		// Token: 0x04000178 RID: 376
		public const short FE_BUF_TOO_SMALL = 1320;

		// Token: 0x04000179 RID: 377
		public const short FE_FLOATING_FORMAT = 1321;

		// Token: 0x0400017A RID: 378
		public const short FE_DBCHAR_INVALID = 1322;

		// Token: 0x0400017B RID: 379
		public const short FE_NODE_CMD_ARG = 1323;

		// Token: 0x0400017C RID: 380
		public const short FE_NODE_NOT_IN_REG = 1324;

		// Token: 0x0400017D RID: 381
		public const short FE_SCU_CMD_ARG = 1325;

		// Token: 0x0400017E RID: 382
		public const short FE_SCU_NOT_IN_REG = 1326;

		// Token: 0x0400017F RID: 383
		public const short FE_REG_ERR = 1327;

		// Token: 0x04000180 RID: 384
		public const short FE_PSP_ERR = 1328;

		// Token: 0x04000181 RID: 385
		public const short FE_QUE2SMALL = 1351;

		// Token: 0x04000182 RID: 386
		public const short FE_QUE2BIG = 1352;

		// Token: 0x04000183 RID: 387
		public const short FE_QUENULL = 1353;

		// Token: 0x04000184 RID: 388
		public const short FE_QUE_FULL = 1354;

		// Token: 0x04000185 RID: 389
		public const short FE_QUE_INUSE = 1355;

		// Token: 0x04000186 RID: 390
		public const short FE_QUE_HDRSIZE = 1356;

		// Token: 0x04000187 RID: 391
		public const short FE_QUE_ELEMSIZE = 1357;

		// Token: 0x04000188 RID: 392
		public const short FE_QUE_NOTINIT = 1358;

		// Token: 0x04000189 RID: 393
		public const short FE_NO_MENU = 1401;

		// Token: 0x0400018A RID: 394
		public const short FE_REFRESH = 1402;

		// Token: 0x0400018B RID: 395
		public const short FE_MENU_LIN = 1403;

		// Token: 0x0400018C RID: 396
		public const short FE_MENU_COL = 1404;

		// Token: 0x0400018D RID: 397
		public const short FE_MENU_NOTLOAD = 1405;

		// Token: 0x0400018E RID: 398
		public const short FW_MEN_SYSDOWN = 1450;

		// Token: 0x0400018F RID: 399
		public const short FW_BGD_STOP = 1451;

		// Token: 0x04000190 RID: 400
		public const short FE_HLPNONE = 1501;

		// Token: 0x04000191 RID: 401
		public const short FE_HLPNOENT = 1502;

		// Token: 0x04000192 RID: 402
		public const short FE_HLPNOMEM = 1503;

		// Token: 0x04000193 RID: 403
		public const short FE_HLPSTRL = 1504;

		// Token: 0x04000194 RID: 404
		public const short FE_CANT_SAVE_HELP = 1505;

		// Token: 0x04000195 RID: 405
		public const short FE_HLPNOENTB = 1506;

		// Token: 0x04000196 RID: 406
		public const short FM_HLPLOOK = 1550;

		// Token: 0x04000197 RID: 407
		public const short FM_ESC_UP_DN = 1551;

		// Token: 0x04000198 RID: 408
		public const short FM_HELP_TITLE = 1552;

		// Token: 0x04000199 RID: 409
		public const short FE_NET_LOWERR = 1600;

		// Token: 0x0400019A RID: 410
		public const short FE_NET_BAD_BUF_LEN = 1601;

		// Token: 0x0400019B RID: 411
		public const short FE_NET_BAD_CMD = 1603;

		// Token: 0x0400019C RID: 412
		public const short FE_NET_TMO = 1605;

		// Token: 0x0400019D RID: 413
		public const short FE_NET_MSG_INCOMPLETE = 1606;

		// Token: 0x0400019E RID: 414
		public const short FE_NET_ILLEGAL_BUFADDR = 1607;

		// Token: 0x0400019F RID: 415
		public const short FE_NET_BAD_LSN = 1608;

		// Token: 0x040001A0 RID: 416
		public const short FE_NET_NO_RES = 1609;

		// Token: 0x040001A1 RID: 417
		public const short FE_NET_TERM = 1610;

		// Token: 0x040001A2 RID: 418
		public const short FE_NET_CANCELED = 1611;

		// Token: 0x040001A3 RID: 419
		public const short FE_NET_DUP_NAME = 1613;

		// Token: 0x040001A4 RID: 420
		public const short FE_NET_NAME_TABLE_FULL = 1614;

		// Token: 0x040001A5 RID: 421
		public const short FE_NET_NAME_DEREG = 1615;

		// Token: 0x040001A6 RID: 422
		public const short FE_NET_LSN_TABLE_FULL = 1617;

		// Token: 0x040001A7 RID: 423
		public const short FE_NET_SESS_OPEN_RJCTD = 1618;

		// Token: 0x040001A8 RID: 424
		public const short FE_NET_BAD_NUM = 1619;

		// Token: 0x040001A9 RID: 425
		public const short FE_NET_NO_ANSWER = 1620;

		// Token: 0x040001AA RID: 426
		public const short FE_NET_BAD_NAME = 1621;

		// Token: 0x040001AB RID: 427
		public const short FE_NET_NAME_IN_USE = 1622;

		// Token: 0x040001AC RID: 428
		public const short FE_NET_NAME_DELETED = 1623;

		// Token: 0x040001AD RID: 429
		public const short FE_NET_ABNORMAL_END = 1624;

		// Token: 0x040001AE RID: 430
		public const short FE_NET_NAME_CONFLICT = 1625;

		// Token: 0x040001AF RID: 431
		public const short FE_NET_BAD_REM_DEV = 1626;

		// Token: 0x040001B0 RID: 432
		public const short FE_NET_INTERFACE_BUSY = 1633;

		// Token: 0x040001B1 RID: 433
		public const short FE_NET_2_MANY_OUT = 1634;

		// Token: 0x040001B2 RID: 434
		public const short FE_NET_BAD_LANA_NUM = 1635;

		// Token: 0x040001B3 RID: 435
		public const short FE_NET_CPLT_WHILE_CAN = 1636;

		// Token: 0x040001B4 RID: 436
		public const short FE_NET_BAD_CANCEL = 1638;

		// Token: 0x040001B5 RID: 437
		public const short FE_NET_LCL_NAME_IN_USE = 1648;

		// Token: 0x040001B6 RID: 438
		public const short FE_NET_UNKNOWN = 1650;

		// Token: 0x040001B7 RID: 439
		public const short FE_NET_DLL = 1651;

		// Token: 0x040001B8 RID: 440
		public const short FE_NET_NO_RESET = 1652;

		// Token: 0x040001B9 RID: 441
		public const short FE_NET_OS_RESOURCE_EX = 1653;

		// Token: 0x040001BA RID: 442
		public const short FE_NET_MAX_APPS = 1654;

		// Token: 0x040001BB RID: 443
		public const short FE_NET_NO_SAPS = 1655;

		// Token: 0x040001BC RID: 444
		public const short FE_NET_NO_OS_RESOURCES = 1656;

		// Token: 0x040001BD RID: 445
		public const short FE_NET_INVALID_ADDR = 1657;

		// Token: 0x040001BE RID: 446
		public const short FE_NET_INVDDID = 1659;

		// Token: 0x040001BF RID: 447
		public const short FE_NET_LOCKFAIL = 1660;

		// Token: 0x040001C0 RID: 448
		public const short FE_NETBIOS_PROTOCOL_NAME = 1662;

		// Token: 0x040001C1 RID: 449
		public const short FE_NET_NOT_LOADED = 1663;

		// Token: 0x040001C2 RID: 450
		public const short FE_NET_SYS_ERR = 1664;

		// Token: 0x040001C3 RID: 451
		public const short FE_NET_HOT_CARRIER_DETECT = 1665;

		// Token: 0x040001C4 RID: 452
		public const short FE_NET_HOT_CARRIER_SENT = 1666;

		// Token: 0x040001C5 RID: 453
		public const short FE_NET_NO_CARRIER = 1667;

		// Token: 0x040001C6 RID: 454
		public const short FE_NET_BAD_AD = 1670;

		// Token: 0x040001C7 RID: 455
		public const short FE_NET_NO_ADAPTER = 1671;

		// Token: 0x040001C8 RID: 456
		public const short FE_NET_ALREADY_LOADED = 1678;

		// Token: 0x040001C9 RID: 457
		public const short FE_NET_CABLE_FAULT = 1679;

		// Token: 0x040001CA RID: 458
		public const short FE_NET_CFG_FORM = 1683;

		// Token: 0x040001CB RID: 459
		public const short FE_NET_RES = 1685;

		// Token: 0x040001CC RID: 460
		public const short FE_NET_HANDLE = 1686;

		// Token: 0x040001CD RID: 461
		public const short FE_NET_NO_FN = 1687;

		// Token: 0x040001CE RID: 462
		public const short FE_NET_BINDING_BUF_TOO_SMALL = 1688;

		// Token: 0x040001CF RID: 463
		public const short FE_WINSOCK_ERROR = 1693;

		// Token: 0x040001D0 RID: 464
		public const short FE_TCP_NAME = 1694;

		// Token: 0x040001D1 RID: 465
		public const short FE_NET_WAIT = 1695;

		// Token: 0x040001D2 RID: 466
		public const short FE_ILL_NETID = 1696;

		// Token: 0x040001D3 RID: 467
		public const short FE_TCP_CFG = 1697;

		// Token: 0x040001D4 RID: 468
		public const short FE_NET_CFG = 1698;

		// Token: 0x040001D5 RID: 469
		public const short FE_NETW_INVALID = 1699;

		// Token: 0x040001D6 RID: 470
		public const short FE_NO_DBB = 1701;

		// Token: 0x040001D7 RID: 471
		public const short FE_NO_SLOTS = 1702;

		// Token: 0x040001D8 RID: 472
		public const short FE_ILL_SCAN = 1703;

		// Token: 0x040001D9 RID: 473
		public const short FE_ILL_ISCAN = 1704;

		// Token: 0x040001DA RID: 474
		public const short FE_ILL_SMOOTH = 1705;

		// Token: 0x040001DB RID: 475
		public const short FE_BADEGU = 1706;

		// Token: 0x040001DC RID: 476
		public const short FE_RANGE = 1707;

		// Token: 0x040001DD RID: 477
		public const short FE_DECIMAL = 1708;

		// Token: 0x040001DE RID: 478
		public const short FE_RW_UNALLOC = 1709;

		// Token: 0x040001DF RID: 479
		public const short FE_RW_LOCK = 1710;

		// Token: 0x040001E0 RID: 480
		public const short FE_ILL_ADI = 1711;

		// Token: 0x040001E1 RID: 481
		public const short FE_ILL_PRIO = 1712;

		// Token: 0x040001E2 RID: 482
		public const short FE_ILL_ALM = 1713;

		// Token: 0x040001E3 RID: 483
		public const short FE_NO_IO_SLOTS = 1714;

		// Token: 0x040001E4 RID: 484
		public const short FE_TAG_DUP = 1715;

		// Token: 0x040001E5 RID: 485
		public const short FE_OUT_INV = 1716;

		// Token: 0x040001E6 RID: 486
		public const short FE_INV_COLD = 1717;

		// Token: 0x040001E7 RID: 487
		public const short FE_FLD_SIZ = 1718;

		// Token: 0x040001E8 RID: 488
		public const short FE_INV_DCT = 1719;

		// Token: 0x040001E9 RID: 489
		public const short FE_INV_DIG = 1720;

		// Token: 0x040001EA RID: 490
		public const short FE_NO_PDATA_PUT = 1721;

		// Token: 0x040001EB RID: 491
		public const short FE_NO_REMOTE_SP = 1722;

		// Token: 0x040001EC RID: 492
		public const short FE_NOT_IN_LOCAL = 1723;

		// Token: 0x040001ED RID: 493
		public const short FE_MSG_LONG = 1724;

		// Token: 0x040001EE RID: 494
		public const short FE_TWA_OR_DIG = 1725;

		// Token: 0x040001EF RID: 495
		public const short FE_BAD_VAL = 1726;

		// Token: 0x040001F0 RID: 496
		public const short FE_PG_NOWAIT = 1727;

		// Token: 0x040001F1 RID: 497
		public const short FE_BAD_STMT = 1728;

		// Token: 0x040001F2 RID: 498
		public const short FE_BAD_OPER = 1729;

		// Token: 0x040001F3 RID: 499
		public const short FE_BAD_RAMP = 1730;

		// Token: 0x040001F4 RID: 500
		public const short FE_BAD_SELMODE = 1731;

		// Token: 0x040001F5 RID: 501
		public const short FE_NO_EXP = 1732;

		// Token: 0x040001F6 RID: 502
		public const short FE_TWA_OR_TAG = 1733;

		// Token: 0x040001F7 RID: 503
		public const short FE_BAD_EXP = 1734;

		// Token: 0x040001F8 RID: 504
		public const short FE_INV_DAY = 1735;

		// Token: 0x040001F9 RID: 505
		public const short FE_INV_OPCODE = 1736;

		// Token: 0x040001FA RID: 506
		public const short FE_INV_VAR = 1737;

		// Token: 0x040001FB RID: 507
		public const short FE_MONTH = 1738;

		// Token: 0x040001FC RID: 508
		public const short FE_DATE = 1739;

		// Token: 0x040001FD RID: 509
		public const short FE_YEAR = 1740;

		// Token: 0x040001FE RID: 510
		public const short FE_BAD_TWA = 1741;

		// Token: 0x040001FF RID: 511
		public const short FE_BAD_TIME = 1742;

		// Token: 0x04000200 RID: 512
		public const short FE_NOT_IN_MANL = 1743;

		// Token: 0x04000201 RID: 513
		public const short FE_NOT_YET = 1744;

		// Token: 0x04000202 RID: 514
		public const short FE_NOT_ENOUGH = 1745;

		// Token: 0x04000203 RID: 515
		public const short FE_TAG_ADD = 1747;

		// Token: 0x04000204 RID: 516
		public const short FE_CANT_ALLOC = 1748;

		// Token: 0x04000205 RID: 517
		public const short FE_NOPATH = 1749;

		// Token: 0x04000206 RID: 518
		public const short FE_TAG_NOTF = 1750;

		// Token: 0x04000207 RID: 519
		public const short FE_NEED_O_OR_C = 1751;

		// Token: 0x04000208 RID: 520
		public const short FE_BAD_BIT = 1752;

		// Token: 0x04000209 RID: 521
		public const short FE_BIT_REPEAT = 1753;

		// Token: 0x0400020A RID: 522
		public const short FE_NOT_PRIM = 1754;

		// Token: 0x0400020B RID: 523
		public const short FE_BAD_KEY = 1755;

		// Token: 0x0400020C RID: 524
		public const short FE_NOT_IN_CHAIN = 1756;

		// Token: 0x0400020D RID: 525
		public const short FE_IN_NCHNS = 1757;

		// Token: 0x0400020E RID: 526
		public const short FE_CHAIN_MAX = 1758;

		// Token: 0x0400020F RID: 527
		public const short FE_SPAN = 1759;

		// Token: 0x04000210 RID: 528
		public const short FE_BADINTERVAL = 1760;

		// Token: 0x04000211 RID: 529
		public const short FE_NOT_AT_ALL = 1761;

		// Token: 0x04000212 RID: 530
		public const short FE_PDB_SN = 1762;

		// Token: 0x04000213 RID: 531
		public const short FE_DBASE_BIG = 1763;

		// Token: 0x04000214 RID: 532
		public const short FE_WAIT_RELOAD = 1764;

		// Token: 0x04000215 RID: 533
		public const short FE_BAD_OPTION = 1765;

		// Token: 0x04000216 RID: 534
		public const short FE_BAD_CHAIN = 1766;

		// Token: 0x04000217 RID: 535
		public const short FE_INVLASCII = 1767;

		// Token: 0x04000218 RID: 536
		public const short FE_BITOVF = 1768;

		// Token: 0x04000219 RID: 537
		public const short FE_INVPREFIX = 1769;

		// Token: 0x0400021A RID: 538
		public const short FE_CHAROVF = 1770;

		// Token: 0x0400021B RID: 539
		public const short FE_INVTYPE = 1771;

		// Token: 0x0400021C RID: 540
		public const short FE_WILD_CARD = 1773;

		// Token: 0x0400021D RID: 541
		public const short FE_OUT_NOT_ALLOWED = 1774;

		// Token: 0x0400021E RID: 542
		public const short FE_VALID_MSG = 1775;

		// Token: 0x0400021F RID: 543
		public const short FE_ILL_OPER = 1776;

		// Token: 0x04000220 RID: 544
		public const short FE_BAD_PDB = 1777;

		// Token: 0x04000221 RID: 545
		public const short FE_DBB_SAVE_RELOAD = 1778;

		// Token: 0x04000222 RID: 546
		public const short FE_NO_ALM_VAL = 1779;

		// Token: 0x04000223 RID: 547
		public const short FE_NO_ALM_TYPE = 1780;

		// Token: 0x04000224 RID: 548
		public const short FE_NO_CONTACT = 1781;

		// Token: 0x04000225 RID: 549
		public const short FE_SIGNIF_FDIGITS = 1782;

		// Token: 0x04000226 RID: 550
		public const short FE_SIGNIF_DDIGITS = 1783;

		// Token: 0x04000227 RID: 551
		public const short FE_NOT_ON_SCAN = 1784;

		// Token: 0x04000228 RID: 552
		public const short FE_IN_CHAIN = 1785;

		// Token: 0x04000229 RID: 553
		public const short FE_BAD_RELOAD_STATE = 1786;

		// Token: 0x0400022A RID: 554
		public const short FE_BAD_SAVE_STATE = 1787;

		// Token: 0x0400022B RID: 555
		public const short FE_BAD_STOP_STATE = 1788;

		// Token: 0x0400022C RID: 556
		public const short FE_REM_BLK_TYP = 1789;

		// Token: 0x0400022D RID: 557
		public const short FE_BTK_CONFIG = 1790;

		// Token: 0x0400022E RID: 558
		public const short FE_BTK_CONFIG_ERR = 1791;

		// Token: 0x0400022F RID: 559
		public const short FE_BTK_BLOCK_MISMATCH = 1792;

		// Token: 0x04000230 RID: 560
		public const short FE_REMAP_THREAD = 1793;

		// Token: 0x04000231 RID: 561
		public const short FE_BLOCK_NO_10_CHAR = 1794;

		// Token: 0x04000232 RID: 562
		public const short FE_DELBLK_FAILED = 1795;

		// Token: 0x04000233 RID: 563
		public const short FM_WARN_NOTSIGNED = 1796;

		// Token: 0x04000234 RID: 564
		public const short FM_ERROR_NOTSIGNED = 1797;

		// Token: 0x04000235 RID: 565
		public const short FE_NOT_SIGNED = 1798;

		// Token: 0x04000236 RID: 566
		public const short FM_WARN_BYPASSED = 1799;

		// Token: 0x04000237 RID: 567
		public const short FM_WARN_BYPASSED2 = 1800;

		// Token: 0x04000238 RID: 568
		public const short FE_SES_HANDLELIST_FULL = 1802;

		// Token: 0x04000239 RID: 569
		public const short FE_SES_NO_SESS = 1803;

		// Token: 0x0400023A RID: 570
		public const short FE_SES_SESS_OK = 1804;

		// Token: 0x0400023B RID: 571
		public const short FE_SES_LOC_NAME = 1806;

		// Token: 0x0400023C RID: 572
		public const short FE_SES_TMO = 1807;

		// Token: 0x0400023D RID: 573
		public const short FE_SES_PLEN = 1808;

		// Token: 0x0400023E RID: 574
		public const short FE_SES_OPEN = 1809;

		// Token: 0x0400023F RID: 575
		public const short FE_SES_CFG_FORM = 1810;

		// Token: 0x04000240 RID: 576
		public const short FE_SM_RES = 1901;

		// Token: 0x04000241 RID: 577
		public const short FE_SM_LOC_NODE = 1903;

		// Token: 0x04000242 RID: 578
		public const short FE_SM_NO_LOAD = 1904;

		// Token: 0x04000243 RID: 579
		public const short FE_SM_VERS = 1905;

		// Token: 0x04000244 RID: 580
		public const short FE_SM_STATE = 1906;

		// Token: 0x04000245 RID: 581
		public const short FE_SM_NO_ENTRY = 1907;

		// Token: 0x04000246 RID: 582
		public const short FE_SM_HOST_REG = 1908;

		// Token: 0x04000247 RID: 583
		public const short FE_SM_LOADED = 1909;

		// Token: 0x04000248 RID: 584
		public const short FE_SM_REQ_OUT = 1910;

		// Token: 0x04000249 RID: 585
		public const short FE_SM_WAIT = 1911;

		// Token: 0x0400024A RID: 586
		public const short FE_SM_STAT_REQ = 1912;

		// Token: 0x0400024B RID: 587
		public const short FE_SM_NO_RSP = 1913;

		// Token: 0x0400024C RID: 588
		public const short FE_SM_NO_SESS = 1914;

		// Token: 0x0400024D RID: 589
		public const short FE_SM_SESS_DOWN = 1915;

		// Token: 0x0400024E RID: 590
		public const short FE_SM_RUNNING = 1916;

		// Token: 0x0400024F RID: 591
		public const short FE_SM_NOT_RUNNING = 1917;

		// Token: 0x04000250 RID: 592
		public const short FE_SM_OK_SESS = 1918;

		// Token: 0x04000251 RID: 593
		public const short FE_SM_NO_SESS2 = 1919;

		// Token: 0x04000252 RID: 594
		public const short FM_SM_NETINI = 1920;

		// Token: 0x04000253 RID: 595
		public const short FE_SM_SEND_VAL = 1921;

		// Token: 0x04000254 RID: 596
		public const short FE_SM_RCV_VAL = 1922;

		// Token: 0x04000255 RID: 597
		public const short FE_SM_SEND_LT_RCV = 1923;

		// Token: 0x04000256 RID: 598
		public const short FE_SM_TMO_MOD = 1924;

		// Token: 0x04000257 RID: 599
		public const short FE_SM_TIMERS = 1925;

		// Token: 0x04000258 RID: 600
		public const short FE_CFM_CORRECTED = 1951;

		// Token: 0x04000259 RID: 601
		public const short FE_CFM_NO_MEM = 1952;

		// Token: 0x0400025A RID: 602
		public const short FE_CFM_NO_OWN = 1953;

		// Token: 0x0400025B RID: 603
		public const short FE_CM_DYNAMIC_CONNECT = 1960;

		// Token: 0x0400025C RID: 604
		public const short FE_CM_PENDING_CONNECT = 1961;

		// Token: 0x0400025D RID: 605
		public const short FE_CM_DISABLED_CONNECT = 1962;

		// Token: 0x0400025E RID: 606
		public const short FE_CM_FAILED_CONNECT = 1963;

		// Token: 0x0400025F RID: 607
		public const short FE_CM_SHUTDOWN = 1964;

		// Token: 0x04000260 RID: 608
		public const short FE_CM_INVALID_LOGICAL = 1965;

		// Token: 0x04000261 RID: 609
		public const short FE_NNT_ACCESS_QUOTA = 2000;

		// Token: 0x04000262 RID: 610
		public const short FE_NNT_FULL = 2001;

		// Token: 0x04000263 RID: 611
		public const short FE_NNT_DUP_NAME = 2002;

		// Token: 0x04000264 RID: 612
		public const short FE_NNT_BAD_ID = 2003;

		// Token: 0x04000265 RID: 613
		public const short FE_NNT_ENTRY_IN_USE = 2004;

		// Token: 0x04000266 RID: 614
		public const short FE_NNT_NO_NAME = 2005;

		// Token: 0x04000267 RID: 615
		public const short FE_NNT_INITIALIZED = 2006;

		// Token: 0x04000268 RID: 616
		public const short FE_NR_TIMEOUT = 2007;

		// Token: 0x04000269 RID: 617
		public const short FE_NR_HANDLELIST_EMPTY = 2008;

		// Token: 0x0400026A RID: 618
		public const short FE_NNT_NO_LOAD = 2009;

		// Token: 0x0400026B RID: 619
		public const short FE_NNT_VERS = 2010;

		// Token: 0x0400026C RID: 620
		public const short FE_NNT_LOADED = 2011;

		// Token: 0x0400026D RID: 621
		public const short FE_NR_PLEN = 2012;

		// Token: 0x0400026E RID: 622
		public const short FE_NR_OPEN = 2013;

		// Token: 0x0400026F RID: 623
		public const short FE_NR_CFG_FORM = 2014;

		// Token: 0x04000270 RID: 624
		public const short FE_NNT_NO_FN = 2015;

		// Token: 0x04000271 RID: 625
		public const short FE_NBT_NO_LOAD = 2016;

		// Token: 0x04000272 RID: 626
		public const short FE_NBT_BAD_ID = 2017;

		// Token: 0x04000273 RID: 627
		public const short FE_LCT_NO_LOAD = 2019;

		// Token: 0x04000274 RID: 628
		public const short FM_NR_WAIT = 2020;

		// Token: 0x04000275 RID: 629
		public const short FE_LCT_BAD_ID = 2021;

		// Token: 0x04000276 RID: 630
		public const short FE_SKT_NO_LOAD = 2022;

		// Token: 0x04000277 RID: 631
		public const short FE_SKT_BAD_ID = 2023;

		// Token: 0x04000278 RID: 632
		public const short FE_BAD_NODENAME = 2040;

		// Token: 0x04000279 RID: 633
		public const short FE_NNT_EXTPTR_IN_USE = 2041;

		// Token: 0x0400027A RID: 634
		public const short FE_NDK_RES_ALREADY_OPEN = 2050;

		// Token: 0x0400027B RID: 635
		public const short FE_NDK_RES_NOT_OPEN = 2051;

		// Token: 0x0400027C RID: 636
		public const short FE_NDK_NCT_FULL = 2052;

		// Token: 0x0400027D RID: 637
		public const short FE_NDK_NCT_EMPTY = 2053;

		// Token: 0x0400027E RID: 638
		public const short FE_NDK_BUF_SIZE_OVERLAP = 2054;

		// Token: 0x0400027F RID: 639
		public const short FE_NDK_CALLBACK_NULL = 2055;

		// Token: 0x04000280 RID: 640
		public const short FE_NDK_INVALID_SERVICE = 2056;

		// Token: 0x04000281 RID: 641
		public const short FE_NDK_INVALID_MOD_ID = 2057;

		// Token: 0x04000282 RID: 642
		public const short FE_NDK_NETWORK_NOT_CFG = 2058;

		// Token: 0x04000283 RID: 643
		public const short FE_NDK_APPL_NOT_OPEN = 2059;

		// Token: 0x04000284 RID: 644
		public const short FE_NDK_INTERFACE_ERR = 2060;

		// Token: 0x04000285 RID: 645
		public const short FE_NDK_INVALID_OPTION = 2061;

		// Token: 0x04000286 RID: 646
		public const short FE_NDK_MULT_RECEIVES = 2062;

		// Token: 0x04000287 RID: 647
		public const short FE_NDK_INVALID_PROTO = 2063;

		// Token: 0x04000288 RID: 648
		public const short FE_NDK_NO_KEY_INFO = 2064;

		// Token: 0x04000289 RID: 649
		public const short FE_NDK_MULT_SYNCH_SEND = 2065;

		// Token: 0x0400028A RID: 650
		public const short FE_NDK_RECV_BAD_SEQ = 2066;

		// Token: 0x0400028B RID: 651
		public const short FE_NDK_RES_CLOSED_NETW = 2067;

		// Token: 0x0400028C RID: 652
		public const short FE_NDK_FAILOVER_NOT_ENABLED = 2068;

		// Token: 0x0400028D RID: 653
		public const short FE_NDK_FAILOVER_OCCURRED = 2069;

		// Token: 0x0400028E RID: 654
		public const short FE_NDK_FAILOVER_MANUAL = 2070;

		// Token: 0x0400028F RID: 655
		public const short FE_NDK_FAILOVER_DISABLED = 2071;

		// Token: 0x04000290 RID: 656
		public const short FE_NDK_FAILOVER_ENABLED = 2072;

		// Token: 0x04000291 RID: 657
		public const short FE_RDB_TOO_SMALL = 2101;

		// Token: 0x04000292 RID: 658
		public const short FE_RDB_RES = 2102;

		// Token: 0x04000293 RID: 659
		public const short FE_RDB_HANDLE = 2103;

		// Token: 0x04000294 RID: 660
		public const short FE_RDB_FORMAT = 2104;

		// Token: 0x04000295 RID: 661
		public const short FE_RDB_WAIT = 2105;

		// Token: 0x04000296 RID: 662
		public const short FE_RDB_NO_VARS_REQ = 2107;

		// Token: 0x04000297 RID: 663
		public const short FE_RDB_DONE = 2108;

		// Token: 0x04000298 RID: 664
		public const short FE_RDB_BAD_SIZE = 2109;

		// Token: 0x04000299 RID: 665
		public const short FE_RDB_CFG_FORM = 2110;

		// Token: 0x0400029A RID: 666
		public const short FE_RDB_BAD_VSP = 2111;

		// Token: 0x0400029B RID: 667
		public const short FE_RDB_LONG_TAG = 2112;

		// Token: 0x0400029C RID: 668
		public const short FE_RDB_LONG_FIELD = 2113;

		// Token: 0x0400029D RID: 669
		public const short FE_NDB_MAX_CALL = 2120;

		// Token: 0x0400029E RID: 670
		public const short FE_NO_MODEM_CONNECTION = 2151;

		// Token: 0x0400029F RID: 671
		public const short FE_REMOIU_NOT_OUTGOING = 2152;

		// Token: 0x040002A0 RID: 672
		public const short FE_MDB_RES = 2153;

		// Token: 0x040002A1 RID: 673
		public const short FE_MDB_QUEUE_FULL = 2154;

		// Token: 0x040002A2 RID: 674
		public const short FE_MDB_HANDLE = 2155;

		// Token: 0x040002A3 RID: 675
		public const short FE_MDB_FORMAT = 2156;

		// Token: 0x040002A4 RID: 676
		public const short FE_MDB_WAIT = 2157;

		// Token: 0x040002A5 RID: 677
		public const short FE_REMOIU_NOT_ACTIVE = 2158;

		// Token: 0x040002A6 RID: 678
		public const short FE_MDB_ALREADY_CONNECTED = 2159;

		// Token: 0x040002A7 RID: 679
		public const short FE_ILLEGAL_STATUS_TYPE = 2160;

		// Token: 0x040002A8 RID: 680
		public const short FE_MDB_BAD_SIZE = 2161;

		// Token: 0x040002A9 RID: 681
		public const short FE_ILLEGAL_MNT_VERSION = 2162;

		// Token: 0x040002AA RID: 682
		public const short FE_RECEIVE_COMPLETE = 2163;

		// Token: 0x040002AB RID: 683
		public const short FE_SRV_FORMAT = 2164;

		// Token: 0x040002AC RID: 684
		public const short FE_RESPONSE_BUFFER_SMALL = 2165;

		// Token: 0x040002AD RID: 685
		public const short FE_XMIT_MSG_TOO_LARGE = 2166;

		// Token: 0x040002AE RID: 686
		public const short FE_ILLEGAL_PASSWORD = 2167;

		// Token: 0x040002AF RID: 687
		public const short FE_PASSWRD_READ_ERR = 2168;

		// Token: 0x040002B0 RID: 688
		public const short FE_INVALID_USER_ID = 2169;

		// Token: 0x040002B1 RID: 689
		public const short FE_READ_ONLY_ACCESS = 2170;

		// Token: 0x040002B2 RID: 690
		public const short FE_DEVICE_DISABLED = 2171;

		// Token: 0x040002B3 RID: 691
		public const short FE_MDBA_LIMIT = 2172;

		// Token: 0x040002B4 RID: 692
		public const short FE_SRV_CHDR = 2201;

		// Token: 0x040002B5 RID: 693
		public const short FE_SRV_SIZE = 2202;

		// Token: 0x040002B6 RID: 694
		public const short FE_SRV_VAP_TYPE = 2203;

		// Token: 0x040002B7 RID: 695
		public const short FE_SRV_PROT = 2204;

		// Token: 0x040002B8 RID: 696
		public const short FE_SRV_SES_DOWN = 2205;

		// Token: 0x040002B9 RID: 697
		public const short FE_SRV_UPL_TYPE = 2206;

		// Token: 0x040002BA RID: 698
		public const short FE_SRV_RSP2BIG = 2207;

		// Token: 0x040002BB RID: 699
		public const short FE_SRV_SESS_ESTAB = 2208;

		// Token: 0x040002BC RID: 700
		public const short FE_SRV_NO_LDBA = 2209;

		// Token: 0x040002BD RID: 701
		public const short FE_SRV_NO_MEM = 2210;

		// Token: 0x040002BE RID: 702
		public const short FE_SRV_NO_NODES = 2211;

		// Token: 0x040002BF RID: 703
		public const short FE_SRV_NODE_DIS = 2212;

		// Token: 0x040002C0 RID: 704
		public const short FE_SRV_NOT_RUNNING = 2213;

		// Token: 0x040002C1 RID: 705
		public const short FM_SRV_NODE_DIS = 2230;

		// Token: 0x040002C2 RID: 706
		public const short FE_SRV_SES_PROB = 2231;

		// Token: 0x040002C3 RID: 707
		public const short FE_SRV_BAD_READ = 2232;

		// Token: 0x040002C4 RID: 708
		public const short FE_SRV_ACC_WRITE = 2233;

		// Token: 0x040002C5 RID: 709
		public const short FE_SRV_ACC_DEF_VAR = 2234;

		// Token: 0x040002C6 RID: 710
		public const short FE_SRV_ACC_DEL_VAR = 2235;

		// Token: 0x040002C7 RID: 711
		public const short FE_SRV_ACC_SAV_DB = 2236;

		// Token: 0x040002C8 RID: 712
		public const short FE_SRV_ACC_REL_INI = 2237;

		// Token: 0x040002C9 RID: 713
		public const short FE_SRV_ACC_REL_TER = 2238;

		// Token: 0x040002CA RID: 714
		public const short FE_FMS_RES = 2301;

		// Token: 0x040002CB RID: 715
		public const short FE_FMS_WAIT = 2302;

		// Token: 0x040002CC RID: 716
		public const short FE_FMS_HANDLE = 2303;

		// Token: 0x040002CD RID: 717
		public const short FE_FMS_NO_FN = 2304;

		// Token: 0x040002CE RID: 718
		public const short FE_FMS_TIMEOUT = 2305;

		// Token: 0x040002CF RID: 719
		public const short FE_FMS_HANDLELIST_EMPTY = 2306;

		// Token: 0x040002D0 RID: 720
		public const short FE_FMS_TOO_SMALL = 2307;

		// Token: 0x040002D1 RID: 721
		public const short FE_FMS_BAD_VSP = 2308;

		// Token: 0x040002D2 RID: 722
		public const short FE_FMS_LIMIT = 2309;

		// Token: 0x040002D3 RID: 723
		public const short FE_FMS_NO_RES = 2310;

		// Token: 0x040002D4 RID: 724
		public const short FE_FMS_NO_NODE = 2311;

		// Token: 0x040002D5 RID: 725
		public const short FE_FMS_MINIVIEW = 2312;

		// Token: 0x040002D6 RID: 726
		public const short FE_INCOMPLETE = 2313;

		// Token: 0x040002D7 RID: 727
		public const short FE_DDE_OP_PENDING = 2314;

		// Token: 0x040002D8 RID: 728
		public const short FE_RAH_RES = 2401;

		// Token: 0x040002D9 RID: 729
		public const short FE_HLA_HANDLE = 2501;

		// Token: 0x040002DA RID: 730
		public const short FE_HLA_ACCESS = 2502;

		// Token: 0x040002DB RID: 731
		public const short FE_HLA_BUFFER = 2503;

		// Token: 0x040002DC RID: 732
		public const short FE_LOK_LOGIN = 2601;

		// Token: 0x040002DD RID: 733
		public const short FE_LOK_PASSWORD = 2602;

		// Token: 0x040002DE RID: 734
		public const short FE_LOK_NOTLOAD = 2603;

		// Token: 0x040002DF RID: 735
		public const short FM_LOGIN = 2604;

		// Token: 0x040002E0 RID: 736
		public const short FM_LOGOUT = 2605;

		// Token: 0x040002E1 RID: 737
		public const short FM_AREA_MSG = 2606;

		// Token: 0x040002E2 RID: 738
		public const short FM_APPL_MSG = 2607;

		// Token: 0x040002E3 RID: 739
		public const short FM_SPPAS_MSG = 2608;

		// Token: 0x040002E4 RID: 740
		public const short FM_PASS_PROMPT = 2609;

		// Token: 0x040002E5 RID: 741
		public const short FE_LOCK_NOMATCH = 2610;

		// Token: 0x040002E6 RID: 742
		public const short FE_LOCK_PASSWORD = 2611;

		// Token: 0x040002E7 RID: 743
		public const short FE_LOCK_NOKEY = 2612;

		// Token: 0x040002E8 RID: 744
		public const short FM_TRY_LOGIN = 2613;

		// Token: 0x040002E9 RID: 745
		public const short FM_TGE_PROMPT = 2701;

		// Token: 0x040002EA RID: 746
		public const short FM_TGE_DIR = 2702;

		// Token: 0x040002EB RID: 747
		public const short FM_EDIT = 2703;

		// Token: 0x040002EC RID: 748
		public const short FE_INPUT = 2704;

		// Token: 0x040002ED RID: 749
		public const short FM_TGE_NEW = 2705;

		// Token: 0x040002EE RID: 750
		public const short FE_TG_VERSION_MISMATCH = 2706;

		// Token: 0x040002EF RID: 751
		public const short FE_TG_SYM_NOT_FOUND = 2707;

		// Token: 0x040002F0 RID: 752
		public const short FE_TG_SYM_EXISTS = 2708;

		// Token: 0x040002F1 RID: 753
		public const short FE_TG_SYMBOL_TOO_LONG = 2709;

		// Token: 0x040002F2 RID: 754
		public const short FE_TG_SUBST_TOO_LONG = 2710;

		// Token: 0x040002F3 RID: 755
		public const short FE_TG_DESC_TOO_LONG = 2711;

		// Token: 0x040002F4 RID: 756
		public const short FE_TG_CANT_READ = 2712;

		// Token: 0x040002F5 RID: 757
		public const short FE_TG_NEED_TG = 2713;

		// Token: 0x040002F6 RID: 758
		public const short FE_ALM_INVALID_FORMAT = 2750;

		// Token: 0x040002F7 RID: 759
		public const short FE_ALM_NO_DESTINATION = 2751;

		// Token: 0x040002F8 RID: 760
		public const short FE_ALM_QUEUE_OVERFLOW = 2752;

		// Token: 0x040002F9 RID: 761
		public const short FE_ALM_INVALID_API_PARAM = 2753;

		// Token: 0x040002FA RID: 762
		public const short FE_FTYPER_NOT_LOADED = 2775;

		// Token: 0x040002FB RID: 763
		public const short FE_CANT_OPEN_ALMFILE = 2776;

		// Token: 0x040002FC RID: 764
		public const short FE_CANT_CLOSE_ALMFILE = 2777;

		// Token: 0x040002FD RID: 765
		public const short FE_FTYPER_DISABLED = 2778;

		// Token: 0x040002FE RID: 766
		public const short FE_ALM_BAD_TIMESTAMP = 2779;

		// Token: 0x040002FF RID: 767
		public const short FE_AAD_BACKUP_WRITE = 2780;

		// Token: 0x04000300 RID: 768
		public const short FE_AAD_FILE_NOT_FOUND = 2781;

		// Token: 0x04000301 RID: 769
		public const short FE_AAD_INVALID_FORMAT = 2782;

		// Token: 0x04000302 RID: 770
		public const short FE_AAD_BAD_VERSION = 2783;

		// Token: 0x04000303 RID: 771
		public const short FE_AAD_READ_ERROR = 2784;

		// Token: 0x04000304 RID: 772
		public const short FE_AAD_WRITE_ERROR = 2785;

		// Token: 0x04000305 RID: 773
		public const short FE_AAD_FILE_NOT_OPEN = 2786;

		// Token: 0x04000306 RID: 774
		public const short FE_AAD_INVALID_AREA_HAN = 2787;

		// Token: 0x04000307 RID: 775
		public const short FE_AAD_INVALID_AREA_SIZE = 2788;

		// Token: 0x04000308 RID: 776
		public const short FE_AAD_AREA_NOT_FOUND = 2789;

		// Token: 0x04000309 RID: 777
		public const short FE_AAD_AREA_WAS_DELETED = 2790;

		// Token: 0x0400030A RID: 778
		public const short FE_AAD_WRONG_FILE_ID = 2791;

		// Token: 0x0400030B RID: 779
		public const short FE_AAD_INVALID_AREA_SYNTAX = 2792;

		// Token: 0x0400030C RID: 780
		public const short FE_SUM_ADI = 2800;

		// Token: 0x0400030D RID: 781
		public const short FE_SUM_COLOR = 2801;

		// Token: 0x0400030E RID: 782
		public const short FE_SUM_FIL = 2802;

		// Token: 0x0400030F RID: 783
		public const short FE_SUM_POS = 2803;

		// Token: 0x04000310 RID: 784
		public const short FE_SUM_LEN = 2804;

		// Token: 0x04000311 RID: 785
		public const short FM_SUM_DATE_IN = 2810;

		// Token: 0x04000312 RID: 786
		public const short FM_SUM_TIME_IN = 2811;

		// Token: 0x04000313 RID: 787
		public const short FM_SUM_DATE_LST = 2812;

		// Token: 0x04000314 RID: 788
		public const short FM_SUM_TIME_LST = 2813;

		// Token: 0x04000315 RID: 789
		public const short FM_SUM_SNODE = 2814;

		// Token: 0x04000316 RID: 790
		public const short FM_SUM_TAG = 2815;

		// Token: 0x04000317 RID: 791
		public const short FM_SUM_STAT = 2816;

		// Token: 0x04000318 RID: 792
		public const short FM_SUM_VALUE = 2817;

		// Token: 0x04000319 RID: 793
		public const short FM_SUM_DESC = 2818;

		// Token: 0x0400031A RID: 794
		public const short FM_SUM_PAGE = 2819;

		// Token: 0x0400031B RID: 795
		public const short FM_SUM_TIME = 2820;

		// Token: 0x0400031C RID: 796
		public const short FM_SUM_PRIORITY = 2821;

		// Token: 0x0400031D RID: 797
		public const short FM_SUM_NODE = 2822;

		// Token: 0x0400031E RID: 798
		public const short FM_SUM_TYPE = 2823;

		// Token: 0x0400031F RID: 799
		public const short FE_SUM_VERERR = 2824;

		// Token: 0x04000320 RID: 800
		public const short FM_SUM_ALMACK = 2825;

		// Token: 0x04000321 RID: 801
		public const short FM_SUM_STATUS = 2826;

		// Token: 0x04000322 RID: 802
		public const short FM_SUM_ENABLE = 2827;

		// Token: 0x04000323 RID: 803
		public const short FM_SUM_DISABLE = 2828;

		// Token: 0x04000324 RID: 804
		public const short FM_SUM_NEWALM = 2829;

		// Token: 0x04000325 RID: 805
		public const short FM_SUM_TOP = 2830;

		// Token: 0x04000326 RID: 806
		public const short FM_SUM_END = 2831;

		// Token: 0x04000327 RID: 807
		public const short FM_SUM_DELALM = 2832;

		// Token: 0x04000328 RID: 808
		public const short FM_SUM_CLRALM = 2833;

		// Token: 0x04000329 RID: 809
		public const short FM_SUM_ALMDEL = 2834;

		// Token: 0x0400032A RID: 810
		public const short FM_SUM_ALMCLR = 2835;

		// Token: 0x0400032B RID: 811
		public const short FE_SUM_ACK = 2836;

		// Token: 0x0400032C RID: 812
		public const short FM_SUM_MENU_TITLE = 2837;

		// Token: 0x0400032D RID: 813
		public const short FM_SUM_CURRENT_TIME = 2838;

		// Token: 0x0400032E RID: 814
		public const short FM_SUM_FSTATUS = 2839;

		// Token: 0x0400032F RID: 815
		public const short FM_SUM_ALARM_ACK = 2840;

		// Token: 0x04000330 RID: 816
		public const short FE_ALMSUM_INVALID_FILTER = 2841;

		// Token: 0x04000331 RID: 817
		public const short FE_ALMSUM_TAGNOTF = 2842;

		// Token: 0x04000332 RID: 818
		public const short FM_KME_PROMPT = 2900;

		// Token: 0x04000333 RID: 819
		public const short FM_KME_DIR = 2901;

		// Token: 0x04000334 RID: 820
		public const short FM_KME_PAGE = 2902;

		// Token: 0x04000335 RID: 821
		public const short FE_KME_KEYDEF = 2903;

		// Token: 0x04000336 RID: 822
		public const short FE_KME_KEYINV = 2904;

		// Token: 0x04000337 RID: 823
		public const short FE_KME_KEYFULL = 2905;

		// Token: 0x04000338 RID: 824
		public const short FM_KME_KEYINFO = 2906;

		// Token: 0x04000339 RID: 825
		public const short FM_KME_NEW = 2907;

		// Token: 0x0400033A RID: 826
		public const short FE_EMAN_NOT_RUNNING = 3000;

		// Token: 0x0400033B RID: 827
		public const short FE_EMAN_MEMORY = 3001;

		// Token: 0x0400033C RID: 828
		public const short FE_EMAN_EVENT_DESC = 3002;

		// Token: 0x0400033D RID: 829
		public const short FE_EMAN_SCANTIME_DESC = 3003;

		// Token: 0x0400033E RID: 830
		public const short FE_EMAN_ACTIVATION_FLAG = 3004;

		// Token: 0x0400033F RID: 831
		public const short FE_EMAN_DETECTION_FLAG = 3005;

		// Token: 0x04000340 RID: 832
		public const short FE_EMAN_HANDLE = 3006;

		// Token: 0x04000341 RID: 833
		public const short FE_EMAN_CALLBACK_ID = 3007;

		// Token: 0x04000342 RID: 834
		public const short FE_EMAN_PARENTHESIS = 3010;

		// Token: 0x04000343 RID: 835
		public const short FE_EMAN_UNBALANCED = 3011;

		// Token: 0x04000344 RID: 836
		public const short FE_EMAN_LOGIC = 3012;

		// Token: 0x04000345 RID: 837
		public const short FE_EMAN_TIME = 3013;

		// Token: 0x04000346 RID: 838
		public const short FE_EMAN_WEEKDAY = 3014;

		// Token: 0x04000347 RID: 839
		public const short FE_EMAN_DATE = 3015;

		// Token: 0x04000348 RID: 840
		public const short FE_EMAN_MONITOR = 3016;

		// Token: 0x04000349 RID: 841
		public const short FE_EMAN_TAG_VALUE = 3017;

		// Token: 0x0400034A RID: 842
		public const short FE_EMAN_COMPARATOR = 3018;

		// Token: 0x0400034B RID: 843
		public const short FE_EMAN_INCOMPATIBLE = 3019;

		// Token: 0x0400034C RID: 844
		public const short FE_EMAN_ALARM_EVENT = 3020;

		// Token: 0x0400034D RID: 845
		public const short FE_EMAN_ALARM_VALUE = 3021;

		// Token: 0x0400034E RID: 846
		public const short FE_EMAN_CHANGE_EVENT = 3022;

		// Token: 0x0400034F RID: 847
		public const short FE_EMAN_NODE_TAG_FIELD = 3023;

		// Token: 0x04000350 RID: 848
		public const short FE_EMAN_DEADBAND = 3024;

		// Token: 0x04000351 RID: 849
		public const short FE_EMAN_TIME_EXPRESSION = 3025;

		// Token: 0x04000352 RID: 850
		public const short FE_EMAN_WEEKDAY_VALUE = 3026;

		// Token: 0x04000353 RID: 851
		public const short FE_EMAN_DATE_EXPRESSION = 3027;

		// Token: 0x04000354 RID: 852
		public const short FE_EMAN_YEAR_VALUE = 3028;

		// Token: 0x04000355 RID: 853
		public const short FE_EMAN_DATE_RANGE = 3029;

		// Token: 0x04000356 RID: 854
		public const short FE_EMAN_LEAP_YEAR = 3030;

		// Token: 0x04000357 RID: 855
		public const short FE_EMAN_NODE_TAG = 3031;

		// Token: 0x04000358 RID: 856
		public const short FM_EMAN_STARTUP = 3050;

		// Token: 0x04000359 RID: 857
		public const short FM_EMAN_CLOSE = 3051;

		// Token: 0x0400035A RID: 858
		public const short FM_EMAN_SYS_FAIL = 3052;

		// Token: 0x0400035B RID: 859
		public const short FM_DCON_EM = 3053;

		// Token: 0x0400035C RID: 860
		public const short FE_PROG_NAME_PVIEW = 3100;

		// Token: 0x0400035D RID: 861
		public const short FE_PROG_NAME_VIEW = 3101;

		// Token: 0x0400035E RID: 862
		public const short FE_PROG_NAME_PDRAW = 3102;

		// Token: 0x0400035F RID: 863
		public const short FE_PROG_NAME_DRAW = 3103;

		// Token: 0x04000360 RID: 864
		public const short FE_PROG_NAME_DBM = 3104;

		// Token: 0x04000361 RID: 865
		public const short FE_PROG_NAME_DBB = 3105;

		// Token: 0x04000362 RID: 866
		public const short FE_PROG_NAME_MENU = 3106;

		// Token: 0x04000363 RID: 867
		public const short FE_PROG_NAME_NSD = 3107;

		// Token: 0x04000364 RID: 868
		public const short FE_PROG_NAME_SUM = 3108;

		// Token: 0x04000365 RID: 869
		public const short FE_PROG_NAME_SC = 3109;

		// Token: 0x04000366 RID: 870
		public const short FE_PROG_NAME_DC = 3110;

		// Token: 0x04000367 RID: 871
		public const short FE_PROG_NAME_REP = 3111;

		// Token: 0x04000368 RID: 872
		public const short FE_PROG_NAME_RCP = 3112;

		// Token: 0x04000369 RID: 873
		public const short FE_PROG_NAME_TGE = 3113;

		// Token: 0x0400036A RID: 874
		public const short FE_PROG_NAME_KME = 3114;

		// Token: 0x0400036B RID: 875
		public const short FE_PROG_NAME_SCU = 3115;

		// Token: 0x0400036C RID: 876
		public const short FE_PROG_NAME_NCU = 3116;

		// Token: 0x0400036D RID: 877
		public const short FE_PROG_NAME_PGU = 3117;

		// Token: 0x0400036E RID: 878
		public const short FE_PROG_NAME_LOG = 3118;

		// Token: 0x0400036F RID: 879
		public const short FE_PROG_NAME_SAC = 3119;

		// Token: 0x04000370 RID: 880
		public const short FE_PROG_NAME_SC1 = 3120;

		// Token: 0x04000371 RID: 881
		public const short FE_PROG_NAME_SC2 = 3121;

		// Token: 0x04000372 RID: 882
		public const short FE_PROG_NAME_ALM = 3122;

		// Token: 0x04000373 RID: 883
		public const short FE_PROG_NAME_DBS = 3123;

		// Token: 0x04000374 RID: 884
		public const short FE_PROG_NAME_SMON = 3124;

		// Token: 0x04000375 RID: 885
		public const short FE_PROG_NAME_SCHD = 3125;

		// Token: 0x04000376 RID: 886
		public const short FE_PROG_NAME_EM = 3127;

		// Token: 0x04000377 RID: 887
		public const short FE_PROG_NAME_HTA = 3128;

		// Token: 0x04000378 RID: 888
		public const short FE_PROG_NAME_HTC = 3129;

		// Token: 0x04000379 RID: 889
		public const short FE_PROG_NAME_HTD = 3130;

		// Token: 0x0400037A RID: 890
		public const short FE_PROG_NAME_DTU = 3131;

		// Token: 0x0400037B RID: 891
		public const short FE_PROG_TITLE_PVIEW = 3200;

		// Token: 0x0400037C RID: 892
		public const short FE_PROG_TITLE_VIEW = 3201;

		// Token: 0x0400037D RID: 893
		public const short FE_PROG_TITLE_PDRAW = 3202;

		// Token: 0x0400037E RID: 894
		public const short FE_PROG_TITLE_DRAW = 3203;

		// Token: 0x0400037F RID: 895
		public const short FE_PROG_TITLE_DBM = 3204;

		// Token: 0x04000380 RID: 896
		public const short FE_PROG_TITLE_DBB = 3205;

		// Token: 0x04000381 RID: 897
		public const short FE_PROG_TITLE_MENU = 3206;

		// Token: 0x04000382 RID: 898
		public const short FE_PROG_TITLE_NSD = 3207;

		// Token: 0x04000383 RID: 899
		public const short FE_PROG_TITLE_SUM = 3208;

		// Token: 0x04000384 RID: 900
		public const short FE_PROG_TITLE_SC = 3209;

		// Token: 0x04000385 RID: 901
		public const short FE_PROG_TITLE_DC = 3210;

		// Token: 0x04000386 RID: 902
		public const short FE_PROG_TITLE_REP = 3211;

		// Token: 0x04000387 RID: 903
		public const short FE_PROG_TITLE_RCP = 3212;

		// Token: 0x04000388 RID: 904
		public const short FE_PROG_TITLE_TGE = 3213;

		// Token: 0x04000389 RID: 905
		public const short FE_PROG_TITLE_KME = 3214;

		// Token: 0x0400038A RID: 906
		public const short FE_PROG_TITLE_SCU = 3215;

		// Token: 0x0400038B RID: 907
		public const short FE_PROG_TITLE_NCU = 3216;

		// Token: 0x0400038C RID: 908
		public const short FE_PROG_TITLE_PGU = 3217;

		// Token: 0x0400038D RID: 909
		public const short FE_PROG_TITLE_LOG = 3218;

		// Token: 0x0400038E RID: 910
		public const short FE_PROG_TITLE_SAC = 3219;

		// Token: 0x0400038F RID: 911
		public const short FE_PROG_TITLE_SC1 = 3220;

		// Token: 0x04000390 RID: 912
		public const short FE_PROG_TITLE_SC2 = 3221;

		// Token: 0x04000391 RID: 913
		public const short FE_PROG_TITLE_ALM = 3222;

		// Token: 0x04000392 RID: 914
		public const short FE_PROG_TITLE_DBS = 3223;

		// Token: 0x04000393 RID: 915
		public const short FE_PROG_TITLE_SMON = 3224;

		// Token: 0x04000394 RID: 916
		public const short FE_PROG_TITLE_SCHD = 3225;

		// Token: 0x04000395 RID: 917
		public const short FE_PROG_TITLE_EM = 3227;

		// Token: 0x04000396 RID: 918
		public const short FE_PROG_TITLE_HTA = 3228;

		// Token: 0x04000397 RID: 919
		public const short FE_PROG_TITLE_HTC = 3229;

		// Token: 0x04000398 RID: 920
		public const short FE_PROG_TITLE_HTD = 3230;

		// Token: 0x04000399 RID: 921
		public const short FE_PROG_TITLE_DTU = 3231;

		// Token: 0x0400039A RID: 922
		public const short FE_INIT_NOLOG = 3300;

		// Token: 0x0400039B RID: 923
		public const short FE_INIT_REENTER = 3301;

		// Token: 0x0400039C RID: 924
		public const short FE_OSX_TYPE = 3400;

		// Token: 0x0400039D RID: 925
		public const short FE_OSX_CREATE = 3401;

		// Token: 0x0400039E RID: 926
		public const short FE_OSX_NOWAY = 3402;

		// Token: 0x0400039F RID: 927
		public const short FE_OSX_NOTFOUND = 3403;

		// Token: 0x040003A0 RID: 928
		public const short FE_OSX_BADCLOSE = 3404;

		// Token: 0x040003A1 RID: 929
		public const short FE_OSX_NOTOURS = 3405;

		// Token: 0x040003A2 RID: 930
		public const short FE_OSX_QCREATE = 3406;

		// Token: 0x040003A3 RID: 931
		public const short FE_OSX_QMUTEX = 3407;

		// Token: 0x040003A4 RID: 932
		public const short FE_OSX_QDOUBLE = 3408;

		// Token: 0x040003A5 RID: 933
		public const short FE_OSX_QMAP = 3409;

		// Token: 0x040003A6 RID: 934
		public const short FE_OSX_QFULL = 3410;

		// Token: 0x040003A7 RID: 935
		public const short FE_OSX_QEMPTY = 3411;

		// Token: 0x040003A8 RID: 936
		public const short FE_OSX_INDEX = 3412;

		// Token: 0x040003A9 RID: 937
		public const short FE_OSX_TIMO = 3413;

		// Token: 0x040003AA RID: 938
		public const short FE_OSX_NAMESIZE = 3414;

		// Token: 0x040003AB RID: 939
		public const short FE_OSX_OBJSECURITY = 3415;

		// Token: 0x040003AC RID: 940
		public const short FE_ART_LOWERR = 3500;

		// Token: 0x040003AD RID: 941
		public const short FE_ART_STAT_TYPE = 3501;

		// Token: 0x040003AE RID: 942
		public const short FE_ART_UNKNOWN = 3502;

		// Token: 0x040003AF RID: 943
		public const short FE_ART_STAT_ID = 3503;

		// Token: 0x040003B0 RID: 944
		public const short FE_ART_NAME_ID = 3504;

		// Token: 0x040003B1 RID: 945
		public const short FE_ART_RES = 3505;

		// Token: 0x040003B2 RID: 946
		public const short FE_ART_QCF_RES = 3506;

		// Token: 0x040003B3 RID: 947
		public const short FE_NO_ARTIC_CFG = 3507;

		// Token: 0x040003B4 RID: 948
		public const short FE_ART_HANDLE = 3508;

		// Token: 0x040003B5 RID: 949
		public const short FE_ART_WAIT = 3509;

		// Token: 0x040003B6 RID: 950
		public const short FE_ART_NOT_INUSE = 3510;

		// Token: 0x040003B7 RID: 951
		public const short FE_ART_ATTACH = 3511;

		// Token: 0x040003B8 RID: 952
		public const short FE_ART_LISTEN = 3512;

		// Token: 0x040003B9 RID: 953
		public const short FE_ART_BAD_SESSION = 3513;

		// Token: 0x040003BA RID: 954
		public const short FE_ART_NO_RCV_OUT = 3514;

		// Token: 0x040003BB RID: 955
		public const short FE_ART_BAD_MSG = 3515;

		// Token: 0x040003BC RID: 956
		public const short FE_ART_RCV_STAT_CFG = 3516;

		// Token: 0x040003BD RID: 957
		public const short FE_ART_BUFSIZ = 3517;

		// Token: 0x040003BE RID: 958
		public const short FM_ART_SUPPORT = 3518;

		// Token: 0x040003BF RID: 959
		public const short FE_ART_NO_MORE_STREAMS = 3601;

		// Token: 0x040003C0 RID: 960
		public const short FE_ERR_NO_MEM = 3602;

		// Token: 0x040003C1 RID: 961
		public const short FE_ART_NO_STREAM = 3603;

		// Token: 0x040003C2 RID: 962
		public const short FE_ART_STN_ALRDY_EXTS = 3604;

		// Token: 0x040003C3 RID: 963
		public const short FE_ART_STRM_ALRDY_EXTS = 3605;

		// Token: 0x040003C4 RID: 964
		public const short FE_ART_NO_STATION = 3606;

		// Token: 0x040003C5 RID: 965
		public const short FE_ART_BADTYPE = 3607;

		// Token: 0x040003C6 RID: 966
		public const short FE_ART_OPTION = 3609;

		// Token: 0x040003C7 RID: 967
		public const short FE_ART_STAT = 3610;

		// Token: 0x040003C8 RID: 968
		public const short FE_ART_GET_STAT = 3611;

		// Token: 0x040003C9 RID: 969
		public const short FE_ART_ALREADY_OPEN = 3612;

		// Token: 0x040003CA RID: 970
		public const short FE_ART_NO_MORE_MBX = 3614;

		// Token: 0x040003CB RID: 971
		public const short FE_ART_NO_RICMON = 3615;

		// Token: 0x040003CC RID: 972
		public const short FE_ART_NOT_OPEN = 3617;

		// Token: 0x040003CD RID: 973
		public const short FE_ART_STATION_ABSENT = 3618;

		// Token: 0x040003CE RID: 974
		public const short FE_ART_NO_WRITE_BUFFER = 3619;

		// Token: 0x040003CF RID: 975
		public const short FE_ART_WRITE_TOO_BIG = 3620;

		// Token: 0x040003D0 RID: 976
		public const short FE_ART_TOO_MANY_WRITE = 3621;

		// Token: 0x040003D1 RID: 977
		public const short FE_ART_NO_MESSAGE = 3622;

		// Token: 0x040003D2 RID: 978
		public const short FE_ART_TOO_MANY_READ = 3623;

		// Token: 0x040003D3 RID: 979
		public const short FE_ART_READ_TOO_SMALL = 3624;

		// Token: 0x040003D4 RID: 980
		public const short FE_ART_TIMEOUT = 3625;

		// Token: 0x040003D5 RID: 981
		public const short FE_ART_NAME_IN_USE = 3626;

		// Token: 0x040003D6 RID: 982
		public const short FE_ART_SVC_TIMEOUT = 3627;

		// Token: 0x040003D7 RID: 983
		public const short FE_ART_CANT_QECB = 3628;

		// Token: 0x040003D8 RID: 984
		public const short FE_ART_BAD_STRM_NAME = 3629;

		// Token: 0x040003D9 RID: 985
		public const short FE_ART_NO_TIMER = 3630;

		// Token: 0x040003DA RID: 986
		public const short FE_ART_BAD_STN_NAME = 3631;

		// Token: 0x040003DB RID: 987
		public const short FE_ART_POOL_ALRDY_EXTS = 3632;

		// Token: 0x040003DC RID: 988
		public const short FE_ART_NO_MORE_POOLS = 3633;

		// Token: 0x040003DD RID: 989
		public const short FE_ART_NO_POOL = 3634;

		// Token: 0x040003DE RID: 990
		public const short FE_ART_NO_POOL_MEM = 3635;

		// Token: 0x040003DF RID: 991
		public const short FE_ART_POOL_SIZE = 3636;

		// Token: 0x040003E0 RID: 992
		public const short FE_ART_TYPE_MISMATCH = 3637;

		// Token: 0x040003E1 RID: 993
		public const short FE_ART_CANT_WAKE_SU = 3638;

		// Token: 0x040003E2 RID: 994
		public const short FE_ART_BAD_POOL_BUF = 3639;

		// Token: 0x040003E3 RID: 995
		public const short ILL_FLDTYP = 4038;

		// Token: 0x040003E4 RID: 996
		public const short FE_FLOAT_ERR = 4039;

		// Token: 0x040003E5 RID: 997
		public const short FE_FFR_NO_REPORT = 4100;

		// Token: 0x040003E6 RID: 998
		public const short FE_FFR_FINISHED_MSG = 4101;

		// Token: 0x040003E7 RID: 999
		public const short FE_FFR_FINISHED_TER = 4102;

		// Token: 0x040003E8 RID: 1000
		public const short FE_FFR_FINISHED_ERR = 4103;

		// Token: 0x040003E9 RID: 1001
		public const short FE_FFR_OPERATOR_ABORT = 4104;

		// Token: 0x040003EA RID: 1002
		public const short FE_FFR_SHELL_CON = 4105;

		// Token: 0x040003EB RID: 1003
		public const short FE_FFR_ERR_HEADER = 4120;

		// Token: 0x040003EC RID: 1004
		public const short FE_FFR_NOERR_HEADER = 4121;

		// Token: 0x040003ED RID: 1005
		public const short FE_FFR_ERR_ACC = 4122;

		// Token: 0x040003EE RID: 1006
		public const short FE_FFR_ERR_FMT = 4123;

		// Token: 0x040003EF RID: 1007
		public const short FE_FFR_NODE = 4124;

		// Token: 0x040003F0 RID: 1008
		public const short FE_FFR_TAG = 4125;

		// Token: 0x040003F1 RID: 1009
		public const short FE_FFR_FIELD = 4126;

		// Token: 0x040003F2 RID: 1010
		public const short FE_FFR_BAD_LINK = 4127;

		// Token: 0x040003F3 RID: 1011
		public const short FE_FFR_VAL_LONG = 4128;

		// Token: 0x040003F4 RID: 1012
		public const short FE_FFR_OPTION_LONG = 4129;

		// Token: 0x040003F5 RID: 1013
		public const short FE_NO_ALM_RESP = 4300;

		// Token: 0x040003F6 RID: 1014
		public const short FE_BAD_DDEA_ITEM = 4301;

		// Token: 0x040003F7 RID: 1015
		public const short FE_DDE_NO_SERVER = 4302;

		// Token: 0x040003F8 RID: 1016
		public const short FE_ART_STRM_MISMATCH = 4500;

		// Token: 0x040003F9 RID: 1017
		public const short FE_ART_ACK = 4501;

		// Token: 0x040003FA RID: 1018
		public const short FE_ART_BAD_MBX = 4502;

		// Token: 0x040003FB RID: 1019
		public const short FE_ART_MBX_ERR = 4503;

		// Token: 0x040003FC RID: 1020
		public const short FE_ART_EMM_CTXNUM = 4504;

		// Token: 0x040003FD RID: 1021
		public const short FE_ART_INTERNAL = 4505;

		// Token: 0x040003FE RID: 1022
		public const short FE_FIO_DISK_SPACE_ERR = 4800;

		// Token: 0x040003FF RID: 1023
		public const short FE_FIO_BACKUP_DEL_FAILED = 4801;

		// Token: 0x04000400 RID: 1024
		public const short FE_FIO_SAVE_AND_BACKUP_FAILED = 4802;

		// Token: 0x04000401 RID: 1025
		public const short FE_NET_INIT = 4900;

		// Token: 0x04000402 RID: 1026
		public const short FE_FMS_INIT = 4901;

		// Token: 0x04000403 RID: 1027
		public const short FE_RDB_INIT = 4902;

		// Token: 0x04000404 RID: 1028
		public const short FE_ART_INIT = 4903;

		// Token: 0x04000405 RID: 1029
		public const short FE_IPC_WAIT = 5000;

		// Token: 0x04000406 RID: 1030
		public const short FE_IPC_TIMEOUT = 5001;

		// Token: 0x04000407 RID: 1031
		public const short FE_IPC_UNKNOWN = 5002;

		// Token: 0x04000408 RID: 1032
		public const short FE_IPC_PROTOCOL = 5003;

		// Token: 0x04000409 RID: 1033
		public const short FM_NETDMACS_STARTUP = 5100;

		// Token: 0x0400040A RID: 1034
		public const short FE_NETDMACS_SESS_OK = 5101;

		// Token: 0x0400040B RID: 1035
		public const short FE_NETDMACS_SESS_FAIL = 5102;

		// Token: 0x0400040C RID: 1036
		public const short FM_NETDMACS_CLOSE = 5103;

		// Token: 0x0400040D RID: 1037
		public const short FM_NETDMACS_SYS_FAIL = 5104;

		// Token: 0x0400040E RID: 1038
		public const short FE_NETDMACS_NOT_RUNNING = 5105;

		// Token: 0x0400040F RID: 1039
		public const short FE_NETDMACS_ERR_MSG_1 = 5106;

		// Token: 0x04000410 RID: 1040
		public const short FE_NETDMACS_ERR_MSG_2 = 5107;

		// Token: 0x04000411 RID: 1041
		public const short FE_NETDMACS_ERR_MSG_3 = 5108;

		// Token: 0x04000412 RID: 1042
		public const short FE_NETDMACS_ERR_MSG_4 = 5109;

		// Token: 0x04000413 RID: 1043
		public const short FM_SAVE_CHANGES = 5210;

		// Token: 0x04000414 RID: 1044
		public const short FE_FILE_VERSION = 5211;

		// Token: 0x04000415 RID: 1045
		public const short FE_FILE_TYPE = 5212;

		// Token: 0x04000416 RID: 1046
		public const short FE_FILE_READ = 5213;

		// Token: 0x04000417 RID: 1047
		public const short FE_FILE_WRITE = 5214;

		// Token: 0x04000418 RID: 1048
		public const short FE_FILE_OPEN = 5215;

		// Token: 0x04000419 RID: 1049
		public const short FM_END_OF_LIST = 5220;

		// Token: 0x0400041A RID: 1050
		public const short FM_YES = 5221;

		// Token: 0x0400041B RID: 1051
		public const short FM_NO = 5222;

		// Token: 0x0400041C RID: 1052
		public const short FM_CANCEL = 5223;

		// Token: 0x0400041D RID: 1053
		public const short FE_NOMATCH = 5224;

		// Token: 0x0400041E RID: 1054
		public const short FM_DISMISS = 5225;

		// Token: 0x0400041F RID: 1055
		public const short FM_ASK_DELETE_FILE = 5226;

		// Token: 0x04000420 RID: 1056
		public const short FM_ASK_OVERWRITE_FILE = 5227;

		// Token: 0x04000421 RID: 1057
		public const short FM_FILE_SAVED = 5228;

		// Token: 0x04000422 RID: 1058
		public const short FM_OUTFILE_NAME = 5240;

		// Token: 0x04000423 RID: 1059
		public const short FM_OUTFILE_PACKAGE = 5241;

		// Token: 0x04000424 RID: 1060
		public const short FM_OUTFILE_TIME = 5242;

		// Token: 0x04000425 RID: 1061
		public const short FE_NO_INPUT = 5250;

		// Token: 0x04000426 RID: 1062
		public const short FM_NODE_FILTER = 5300;

		// Token: 0x04000427 RID: 1063
		public const short FM_NODENAME = 5301;

		// Token: 0x04000428 RID: 1064
		public const short FM_TAGNAME = 5302;

		// Token: 0x04000429 RID: 1065
		public const short FM_FILTER = 5303;

		// Token: 0x0400042A RID: 1066
		public const short FE_FORMAT_NO_JUST = 5304;

		// Token: 0x0400042B RID: 1067
		public const short FE_FORMAT_NO_NUM = 5305;

		// Token: 0x0400042C RID: 1068
		public const short FE_FORMAT_BAD_LENGTH = 5306;

		// Token: 0x0400042D RID: 1069
		public const short FE_FORMAT_NO_TYPE = 5307;

		// Token: 0x0400042E RID: 1070
		public const short FE_FORMAT_VAL2SMALL = 5308;

		// Token: 0x0400042F RID: 1071
		public const short FE_FORMAT_LEN2BIG = 5309;

		// Token: 0x04000430 RID: 1072
		public const short FM_OVERVIEW = 5400;

		// Token: 0x04000431 RID: 1073
		public const short FM_LCU_HEADING1 = 5500;

		// Token: 0x04000432 RID: 1074
		public const short FM_LCU_HEADING2 = 5501;

		// Token: 0x04000433 RID: 1075
		public const short FM_LCU_HEADING3 = 5502;

		// Token: 0x04000434 RID: 1076
		public const short FM_LCU_HEADING4 = 5503;

		// Token: 0x04000435 RID: 1077
		public const short FM_LCU_HEADING5 = 5504;

		// Token: 0x04000436 RID: 1078
		public const short FM_LCU_HEADING6 = 5505;

		// Token: 0x04000437 RID: 1079
		public const short FM_LCU_HEADING8 = 5506;

		// Token: 0x04000438 RID: 1080
		public const short FM_LCU_QUERY1 = 5507;

		// Token: 0x04000439 RID: 1081
		public const short FM_LCU_QUERY2 = 5508;

		// Token: 0x0400043A RID: 1082
		public const short FM_LCU_QUERY3 = 5509;

		// Token: 0x0400043B RID: 1083
		public const short FM_LCU_QUERY4 = 5510;

		// Token: 0x0400043C RID: 1084
		public const short FM_LCU_ISSUE = 5511;

		// Token: 0x0400043D RID: 1085
		public const short FM_LCU_CUSTOMER = 5512;

		// Token: 0x0400043E RID: 1086
		public const short FM_LCU_ID = 5513;

		// Token: 0x0400043F RID: 1087
		public const short FM_LCU_VERSION = 5514;

		// Token: 0x04000440 RID: 1088
		public const short FM_LCU_CPU = 5515;

		// Token: 0x04000441 RID: 1089
		public const short FM_LCU_UNITS = 5516;

		// Token: 0x04000442 RID: 1090
		public const short FM_LCU_TERMINATION = 5517;

		// Token: 0x04000443 RID: 1091
		public const short FM_LCU_SYSTEM = 5518;

		// Token: 0x04000444 RID: 1092
		public const short FM_LCU_HARDWARE = 5519;

		// Token: 0x04000445 RID: 1093
		public const short FM_LCU_CHECKSUM = 5520;

		// Token: 0x04000446 RID: 1094
		public const short FE_LICENSE_INVALID = 5521;

		// Token: 0x04000447 RID: 1095
		public const short FE_LICENSE_TERMINATED = 5522;

		// Token: 0x04000448 RID: 1096
		public const short FE_LICENSE_CPU = 5523;

		// Token: 0x04000449 RID: 1097
		public const short FE_LICENSE_DATE = 5524;

		// Token: 0x0400044A RID: 1098
		public const short FE_LICENSE_COMMAND = 5525;

		// Token: 0x0400044B RID: 1099
		public const short FE_LICENSE_NOT_LOADED = 5526;

		// Token: 0x0400044C RID: 1100
		public const short FE_LCU_OK1 = 5527;

		// Token: 0x0400044D RID: 1101
		public const short FM_LCU_LIST = 5528;

		// Token: 0x0400044E RID: 1102
		public const short FM_LCU_ADD = 5529;

		// Token: 0x0400044F RID: 1103
		public const short FM_LCU_MODIFY = 5530;

		// Token: 0x04000450 RID: 1104
		public const short FM_LCU_REMOVE = 5531;

		// Token: 0x04000451 RID: 1105
		public const short FM_LCU_SHOW = 5532;

		// Token: 0x04000452 RID: 1106
		public const short FM_LCU_ENTER = 5533;

		// Token: 0x04000453 RID: 1107
		public const short FM_LCU_HEADINGA = 5534;

		// Token: 0x04000454 RID: 1108
		public const short FM_LCU_HEADINGB = 5535;

		// Token: 0x04000455 RID: 1109
		public const short FM_LCU_HEADINGC = 5536;

		// Token: 0x04000456 RID: 1110
		public const short FE_LICENSE_UNITS = 5537;

		// Token: 0x04000457 RID: 1111
		public const short FM_LCU_QUERY5 = 5538;

		// Token: 0x04000458 RID: 1112
		public const short FM_LCU_REMOVE1 = 5539;

		// Token: 0x04000459 RID: 1113
		public const short FE_LICENSE_RELEASE = 5540;

		// Token: 0x0400045A RID: 1114
		public const short FE_LICENSE_ALTERED = 5541;

		// Token: 0x0400045B RID: 1115
		public const short FM_LCU_STARTUP = 5542;

		// Token: 0x0400045C RID: 1116
		public const short FM_LCU_SERIAL = 5543;

		// Token: 0x0400045D RID: 1117
		public const short FE_LICENSE_LOADED = 5544;

		// Token: 0x0400045E RID: 1118
		public const short FM_LCU_HEADING7 = 5545;

		// Token: 0x0400045F RID: 1119
		public const short FE_LCU_TIMEOUT = 5546;

		// Token: 0x04000460 RID: 1120
		public const short FE_LICENSE_ISSUE = 5547;

		// Token: 0x04000461 RID: 1121
		public const short FM_LCU_OPTIONS = 5548;

		// Token: 0x04000462 RID: 1122
		public const short FE_LCU_PRODUCTNAME = 5549;

		// Token: 0x04000463 RID: 1123
		public const short FM_XSCHD_NAME = 5600;

		// Token: 0x04000464 RID: 1124
		public const short FE_XSCHD_COMMAND = 5601;

		// Token: 0x04000465 RID: 1125
		public const short FE_XSCHD_TIME = 5602;

		// Token: 0x04000466 RID: 1126
		public const short FE_XSCHD_DATE = 5603;

		// Token: 0x04000467 RID: 1127
		public const short FE_XSCHD_NODE = 5604;

		// Token: 0x04000468 RID: 1128
		public const short FE_XSCHD_TAG = 5605;

		// Token: 0x04000469 RID: 1129
		public const short FM_XDCON_NAME = 5700;

		// Token: 0x0400046A RID: 1130
		public const short FM_XDCON_SUMMARY_REMARK = 5701;

		// Token: 0x0400046B RID: 1131
		public const short FM_XDCON_SNAPSHOT_REMARK = 5702;

		// Token: 0x0400046C RID: 1132
		public const short FM_XDCON_SETPOINT_REMARK = 5703;

		// Token: 0x0400046D RID: 1133
		public const short FM_XDCON_MANUAL_REMARK = 5704;

		// Token: 0x0400046E RID: 1134
		public const short FM_XDCON_BATCH_ID = 5705;

		// Token: 0x0400046F RID: 1135
		public const short FM_XDCON_BATCH_START = 5706;

		// Token: 0x04000470 RID: 1136
		public const short FM_XDCON_BATCH_STOP = 5707;

		// Token: 0x04000471 RID: 1137
		public const short FM_XDCON_BATCH_INTERVAL = 5708;

		// Token: 0x04000472 RID: 1138
		public const short FM_XDCON_ACTION_NUM = 5709;

		// Token: 0x04000473 RID: 1139
		public const short FM_XDCON_EVENT_NUM = 5710;

		// Token: 0x04000474 RID: 1140
		public const short FM_XDCON_ACTION_REMARK = 5711;

		// Token: 0x04000475 RID: 1141
		public const short FM_XDCON_EVENT_REMARK = 5712;

		// Token: 0x04000476 RID: 1142
		public const short FM_XDCON_SUM_MODE = 5713;

		// Token: 0x04000477 RID: 1143
		public const short FM_XDCON_SUM_FILE_SEL = 5714;

		// Token: 0x04000478 RID: 1144
		public const short FM_XDCON_SNAP_FILE_SEL = 5715;

		// Token: 0x04000479 RID: 1145
		public const short FM_XDCON_SET_FILE_SEL = 5716;

		// Token: 0x0400047A RID: 1146
		public const short FM_XDCON_MANUAL_FILE_SEL = 5717;

		// Token: 0x0400047B RID: 1147
		public const short FM_XDCON_EVENT_NAME_SEL = 5718;

		// Token: 0x0400047C RID: 1148
		public const short FM_XDCON_EVENT_NAME_UNDEF = 5719;

		// Token: 0x0400047D RID: 1149
		public const short FM_XDCON_SUMMARY_FILE_UNDEF = 5720;

		// Token: 0x0400047E RID: 1150
		public const short FM_XDCON_SNAPSHOT_FILE_UNDEF = 5721;

		// Token: 0x0400047F RID: 1151
		public const short FM_XDCON_SETPOINT_FILE_UNDEF = 5722;

		// Token: 0x04000480 RID: 1152
		public const short FM_XDCON_MANUAL_FILE_UNDEF = 5723;

		// Token: 0x04000481 RID: 1153
		public const short FM_XDCON_BATCH_FILE_UNDEF = 5724;

		// Token: 0x04000482 RID: 1154
		public const short FM_XDCON_BATCH_FILE_SEL = 5725;

		// Token: 0x04000483 RID: 1155
		public const short FM_XDCON_BATCH_EXISTS = 5726;

		// Token: 0x04000484 RID: 1156
		public const short FM_XDCON_SUM_OUT_FILE_SEL = 5727;

		// Token: 0x04000485 RID: 1157
		public const short FM_XDCON_SNAP_OUT_FILE_SEL = 5728;

		// Token: 0x04000486 RID: 1158
		public const short FM_XDCON_SET_OUT_FILE_SEL = 5729;

		// Token: 0x04000487 RID: 1159
		public const short FM_XDCON_MANUAL_OUT_FILE_SEL = 5730;

		// Token: 0x04000488 RID: 1160
		public const short FM_XDCON_CONFIG_FILE_SEL = 5731;

		// Token: 0x04000489 RID: 1161
		public const short FM_XDCON_BATCH_SUFFIX = 5732;

		// Token: 0x0400048A RID: 1162
		public const short FM_XDCON_BATCH_SUFFIX_TAG = 5733;

		// Token: 0x0400048B RID: 1163
		public const short FM_XDCON_BATCH_SUSPEND = 5734;

		// Token: 0x0400048C RID: 1164
		public const short FM_XDCON_BATCH_RESUME = 5735;

		// Token: 0x0400048D RID: 1165
		public const short FM_XDCON_ACTION_MSG1 = 5736;

		// Token: 0x0400048E RID: 1166
		public const short FM_XDCON_ACTION_MSG2 = 5737;

		// Token: 0x0400048F RID: 1167
		public const short FM_XDCON_ACTION_MSG3 = 5738;

		// Token: 0x04000490 RID: 1168
		public const short FM_XDCON_ACTION_MSG4 = 5739;

		// Token: 0x04000491 RID: 1169
		public const short FE_XDCON_DUPLICATE_EVENT = 5740;

		// Token: 0x04000492 RID: 1170
		public const short FE_XDCON_INPUT_EVENT_DEF = 5741;

		// Token: 0x04000493 RID: 1171
		public const short FE_XDCON_INPUT_SUMMARY_MODE = 5742;

		// Token: 0x04000494 RID: 1172
		public const short FE_XDCON_INPUT_NODE_TAG_FIELD = 5743;

		// Token: 0x04000495 RID: 1173
		public const short FE_XDCON_INPUT_FORMAT = 5744;

		// Token: 0x04000496 RID: 1174
		public const short FE_XDCON_INPUT_FILE_NAME = 5745;

		// Token: 0x04000497 RID: 1175
		public const short FE_XDCON_INPUT_BSTART_EVENT = 5746;

		// Token: 0x04000498 RID: 1176
		public const short FE_XDCON_INPUT_BSTOP_EVENT = 5747;

		// Token: 0x04000499 RID: 1177
		public const short FE_XDCON_INPUT_BSCAN_INTERVAL = 5748;

		// Token: 0x0400049A RID: 1178
		public const short FE_XDCON_INPUT_ASTART_EVENT = 5749;

		// Token: 0x0400049B RID: 1179
		public const short FE_XDCON_INPUT_ASTOP_EVENT = 5750;

		// Token: 0x0400049C RID: 1180
		public const short FE_XDCON_INPUT_ASCAN_INTERVAL = 5751;

		// Token: 0x0400049D RID: 1181
		public const short FE_XDCON_INPUT_EVENT_NAME = 5752;

		// Token: 0x0400049E RID: 1182
		public const short FE_XDCON_INPUT_BATCH_ID = 5753;

		// Token: 0x0400049F RID: 1183
		public const short FE_XDCON_INVALID_SUMMARY_MODE = 5761;

		// Token: 0x040004A0 RID: 1184
		public const short FE_XDCON_INVALID_FORMAT = 5762;

		// Token: 0x040004A1 RID: 1185
		public const short FE_XDCON_INVALID_NODE_TAG_FIELD = 5763;

		// Token: 0x040004A2 RID: 1186
		public const short FE_XDCON_INVALID_BSCAN_INTERVAL = 5764;

		// Token: 0x040004A3 RID: 1187
		public const short FE_XDCON_INVALID_ASCAN_INTERVAL = 5765;

		// Token: 0x040004A4 RID: 1188
		public const short FE_XDCON_INVALID_BSTART_EVENT = 5766;

		// Token: 0x040004A5 RID: 1189
		public const short FE_XDCON_INVALID_BSTOP_EVENT = 5767;

		// Token: 0x040004A6 RID: 1190
		public const short FE_XDCON_INVALID_BSUSPEND_EVENT = 5768;

		// Token: 0x040004A7 RID: 1191
		public const short FE_XDCON_INVALID_BRESUME_EVENT = 5769;

		// Token: 0x040004A8 RID: 1192
		public const short FE_XDCON_INVALID_ASTART_EVENT = 5770;

		// Token: 0x040004A9 RID: 1193
		public const short FE_XDCON_INVALID_ASTOP_EVENT = 5771;

		// Token: 0x040004AA RID: 1194
		public const short FE_XDCON_NOTDEF_OUTFILE = 5772;

		// Token: 0x040004AB RID: 1195
		public const short FM_XDCON_REMOVE_BATCH = 5773;

		// Token: 0x040004AC RID: 1196
		public const short FM_XDCON_EXE_FILE_SEL = 5774;

		// Token: 0x040004AD RID: 1197
		public const short FE_XDCON_SUM_TYPE_MISMATCH = 5775;

		// Token: 0x040004AE RID: 1198
		public const short FM_XDCON_QUIT_XDCON = 5776;

		// Token: 0x040004AF RID: 1199
		public const short FE_XDCON_FIELD_FORMAT_MISMATCH = 5777;

		// Token: 0x040004B0 RID: 1200
		public const short FM_XDCON_FILE_MISMATCH = 5778;

		// Token: 0x040004B1 RID: 1201
		public const short FM_XDCON_FILE_MISMATCH1 = 5779;

		// Token: 0x040004B2 RID: 1202
		public const short FM_XDCON_RENAME = 5780;

		// Token: 0x040004B3 RID: 1203
		public const short FM_XDCON_DELETE = 5781;

		// Token: 0x040004B4 RID: 1204
		public const short FE_XDCON_FILE_RENAME_EXISTS = 5782;

		// Token: 0x040004B5 RID: 1205
		public const short FE_XDCON_NO_ACTIONS = 5783;

		// Token: 0x040004B6 RID: 1206
		public const short FM_XFVIEW_NAME = 5800;

		// Token: 0x040004B7 RID: 1207
		public const short FM_XFVIEW_ID = 5801;

		// Token: 0x040004B8 RID: 1208
		public const short FM_XFVIEW_START_DATE = 5802;

		// Token: 0x040004B9 RID: 1209
		public const short FM_XFVIEW_START_TIME = 5803;

		// Token: 0x040004BA RID: 1210
		public const short FM_XFVIEW_STOP_DATE = 5804;

		// Token: 0x040004BB RID: 1211
		public const short FM_XFVIEW_STOP_TIME = 5805;

		// Token: 0x040004BC RID: 1212
		public const short FE_XFVIEW_FORMAT = 5806;

		// Token: 0x040004BD RID: 1213
		public const short FM_XDEMO_NAME = 5900;

		// Token: 0x040004BE RID: 1214
		public const short FM_XNSD_NAME = 6030;

		// Token: 0x040004BF RID: 1215
		public const short FM_XNSD_ESTABLISHED = 6031;

		// Token: 0x040004C0 RID: 1216
		public const short FM_XNSD_INACTIVE = 6032;

		// Token: 0x040004C1 RID: 1217
		public const short FE_BM_MAX_LIST_FULL = 6100;

		// Token: 0x040004C2 RID: 1218
		public const short FE_BM_MALLOC_FAIL = 6101;

		// Token: 0x040004C3 RID: 1219
		public const short FE_BM_TIMEOUT = 6102;

		// Token: 0x040004C4 RID: 1220
		public const short FE_BM_INSTALL = 6103;

		// Token: 0x040004C5 RID: 1221
		public const short FE_BM_UNKNOWN_BATCH = 6104;

		// Token: 0x040004C6 RID: 1222
		public const short FE_BM_MATCH = 6105;

		// Token: 0x040004C7 RID: 1223
		public const short FE_BM_START_ERROR = 6106;

		// Token: 0x040004C8 RID: 1224
		public const short FE_BM_STOP_ERROR = 6107;

		// Token: 0x040004C9 RID: 1225
		public const short FE_BM_SUSPEND_ERROR = 6108;

		// Token: 0x040004CA RID: 1226
		public const short FE_BM_RESUME_ERROR = 6109;

		// Token: 0x040004CB RID: 1227
		public const short FE_BM_NOT_RUNNING = 6110;

		// Token: 0x040004CC RID: 1228
		public const short FE_BM_APPL_FORMAT = 6111;

		// Token: 0x040004CD RID: 1229
		public const short FE_BM_SYSTEM_ERROR = 6112;

		// Token: 0x040004CE RID: 1230
		public const short FE_BM_OPEN_ACT_FILE = 6113;

		// Token: 0x040004CF RID: 1231
		public const short FE_BM_OUTPUT_FILE_TYPE = 6114;

		// Token: 0x040004D0 RID: 1232
		public const short FE_BM_OPEN_DEF_FILE = 6115;

		// Token: 0x040004D1 RID: 1233
		public const short FE_BM_ACT_TYPE = 6116;

		// Token: 0x040004D2 RID: 1234
		public const short FE_BM_READ_EVENT = 6117;

		// Token: 0x040004D3 RID: 1235
		public const short FE_BM_READ_ACTION = 6118;

		// Token: 0x040004D4 RID: 1236
		public const short FE_BM_IS_RUNNING = 6119;

		// Token: 0x040004D5 RID: 1237
		public const short FE_BM_MULTIPLE_BATCHES = 6120;

		// Token: 0x040004D6 RID: 1238
		public const short FE_BM_EXECUTE = 6121;

		// Token: 0x040004D7 RID: 1239
		public const short FE_BM_EXECUTE_TMP = 6122;

		// Token: 0x040004D8 RID: 1240
		public const short FE_BM_EXECUTE_PROC = 6123;

		// Token: 0x040004D9 RID: 1241
		public const short FE_BM_UNKNOWN_ACTION = 6124;

		// Token: 0x040004DA RID: 1242
		public const short FE_BM_ACTION_STATE = 6125;

		// Token: 0x040004DB RID: 1243
		public const short FE_BM_CONTROL_REC_MAPPING = 6126;

		// Token: 0x040004DC RID: 1244
		public const short FE_FTB_NAME_TOO_BIG = 6127;

		// Token: 0x040004DD RID: 1245
		public const short FE_FTB_NO_SLOTS = 6128;

		// Token: 0x040004DE RID: 1246
		public const short FE_FTB_FILE_OPEN_ERROR = 6129;

		// Token: 0x040004DF RID: 1247
		public const short FE_BM_BADBACKUP = 6130;

		// Token: 0x040004E0 RID: 1248
		public const short FE_BM_NOBACKUPPATH = 6131;

		// Token: 0x040004E1 RID: 1249
		public const short FM_BATCH_START = 6132;

		// Token: 0x040004E2 RID: 1250
		public const short FE_BM_OPEN_FILE = 6133;

		// Token: 0x040004E3 RID: 1251
		public const short FE_BM_INVALID_EVT = 6134;

		// Token: 0x040004E4 RID: 1252
		public const short FM_BM_INSTALL = 6135;

		// Token: 0x040004E5 RID: 1253
		public const short FM_BM_SUSPEND = 6136;

		// Token: 0x040004E6 RID: 1254
		public const short FM_BM_RESUME = 6137;

		// Token: 0x040004E7 RID: 1255
		public const short FE_BM_ACT_EVT = 6138;

		// Token: 0x040004E8 RID: 1256
		public const short FM_BM_START = 6139;

		// Token: 0x040004E9 RID: 1257
		public const short FM_BM_DELETE = 6140;

		// Token: 0x040004EA RID: 1258
		public const short FE_BM_SUS_EVT = 6141;

		// Token: 0x040004EB RID: 1259
		public const short FM_BM_STOP = 6142;

		// Token: 0x040004EC RID: 1260
		public const short FM_BM_MODIFY = 6143;

		// Token: 0x040004ED RID: 1261
		public const short FE_BM_ALLOC_EVT = 6144;

		// Token: 0x040004EE RID: 1262
		public const short FE_BM_ALLOC_CHG = 6145;

		// Token: 0x040004EF RID: 1263
		public const short FE_BM_EVT_CALL = 6146;

		// Token: 0x040004F0 RID: 1264
		public const short FE_BM_CHG_EVT = 6147;

		// Token: 0x040004F1 RID: 1265
		public const short FE_BM_OVERRUN = 6148;

		// Token: 0x040004F2 RID: 1266
		public const short FM_BM_ACT_START = 6149;

		// Token: 0x040004F3 RID: 1267
		public const short FM_BM_ACT_STOP = 6150;

		// Token: 0x040004F4 RID: 1268
		public const short FE_BM_SCAN_OVERRUN = 6151;

		// Token: 0x040004F5 RID: 1269
		public const short FE_BM_DQP_ERR = 6152;

		// Token: 0x040004F6 RID: 1270
		public const short FM_BM_DQP_OK = 6153;

		// Token: 0x040004F7 RID: 1271
		public const short FE_BM_VMS_ERR = 6154;

		// Token: 0x040004F8 RID: 1272
		public const short FE_BM_NO_SPACE = 6155;

		// Token: 0x040004F9 RID: 1273
		public const short FM_BM_BACKUP_FULL = 6156;

		// Token: 0x040004FA RID: 1274
		public const short FE_BM_IO_ERR = 6157;

		// Token: 0x040004FB RID: 1275
		public const short FE_BM_ALLOC_DATA = 6158;

		// Token: 0x040004FC RID: 1276
		public const short FE_DECNET_OTHERS = 6200;

		// Token: 0x040004FD RID: 1277
		public const short FE_DECNET_INSFMEM = 6201;

		// Token: 0x040004FE RID: 1278
		public const short FE_DECNET_NOPRIV = 6202;

		// Token: 0x040004FF RID: 1279
		public const short FE_DECNET_NOSUCHDEV = 6203;

		// Token: 0x04000500 RID: 1280
		public const short FE_DECNET_CONNECFAIL = 6204;

		// Token: 0x04000501 RID: 1281
		public const short FE_DECNET_DEVOFFLINE = 6205;

		// Token: 0x04000502 RID: 1282
		public const short FE_DECNET_FILALRACC = 6206;

		// Token: 0x04000503 RID: 1283
		public const short FE_DECNET_INVLOGIN = 6207;

		// Token: 0x04000504 RID: 1284
		public const short FE_DECNET_IVDEVNAM = 6208;

		// Token: 0x04000505 RID: 1285
		public const short FE_DECNET_LINKEXIT = 6209;

		// Token: 0x04000506 RID: 1286
		public const short FE_DECNET_NOLINKS = 6210;

		// Token: 0x04000507 RID: 1287
		public const short FE_DECNET_NOSUCHNODE = 6211;

		// Token: 0x04000508 RID: 1288
		public const short FE_DECNET_NOSUCHOBJ = 6212;

		// Token: 0x04000509 RID: 1289
		public const short FE_DECNET_NOSUCHUSER = 6213;

		// Token: 0x0400050A RID: 1290
		public const short FE_DECNET_PROTOCOL = 6214;

		// Token: 0x0400050B RID: 1291
		public const short FE_DECNET_REJECT = 6215;

		// Token: 0x0400050C RID: 1292
		public const short FE_DECNET_REMRSRC = 6216;

		// Token: 0x0400050D RID: 1293
		public const short FE_DECNET_SHUT = 6217;

		// Token: 0x0400050E RID: 1294
		public const short FE_DECNET_THIRDPARTY = 6218;

		// Token: 0x0400050F RID: 1295
		public const short FE_DECNET_TOOMUCHDATA = 6219;

		// Token: 0x04000510 RID: 1296
		public const short FE_DECNET_UNREACHABLE = 6220;

		// Token: 0x04000511 RID: 1297
		public const short FE_DECNET_DEVALLOC = 6221;

		// Token: 0x04000512 RID: 1298
		public const short FE_DECNET_EXQUOTA = 6222;

		// Token: 0x04000513 RID: 1299
		public const short FE_DECNET_LINKABORT = 6223;

		// Token: 0x04000514 RID: 1300
		public const short FE_DECNET_LINKDISCON = 6224;

		// Token: 0x04000515 RID: 1301
		public const short FE_DECNET_PATHLOST = 6225;

		// Token: 0x04000516 RID: 1302
		public const short FE_DECNET_TIMEOUT = 6226;

		// Token: 0x04000517 RID: 1303
		public const short FE_DECNET_FILNOTACC = 6227;

		// Token: 0x04000518 RID: 1304
		public const short FE_DECNET_NOSOLICIT = 6228;

		// Token: 0x04000519 RID: 1305
		public const short FE_DECNET_BADPARAM = 6229;

		// Token: 0x0400051A RID: 1306
		public const short FE_DECNET_ILLCNTRFUNC = 6230;

		// Token: 0x0400051B RID: 1307
		public const short FE_DECNET_NOMBX = 6231;

		// Token: 0x0400051C RID: 1308
		public const short FM_XDBM_NAME = 6300;

		// Token: 0x0400051D RID: 1309
		public const short FM_XDBM_QUIT_XDBM = 6301;

		// Token: 0x0400051E RID: 1310
		public const short FM_XDBM_PDB_VERS = 6302;

		// Token: 0x0400051F RID: 1311
		public const short FM_XDBM_BLK_VERS = 6303;

		// Token: 0x04000520 RID: 1312
		public const short FM_XDBM_BLK_LESS = 6304;

		// Token: 0x04000521 RID: 1313
		public const short FE_XDB_CREATEVA = 6305;

		// Token: 0x04000522 RID: 1314
		public const short FE_XDB_DELETEVA = 6306;

		// Token: 0x04000523 RID: 1315
		public const short FE_XDB_GETVM = 6307;

		// Token: 0x04000524 RID: 1316
		public const short FE_XDB_FREEVM = 6308;

		// Token: 0x04000525 RID: 1317
		public const short FE_XDB_MAPSEC = 6309;

		// Token: 0x04000526 RID: 1318
		public const short FE_XDB_DELSEC = 6310;

		// Token: 0x04000527 RID: 1319
		public const short FE_XDB_CRESEC = 6311;

		// Token: 0x04000528 RID: 1320
		public const short FE_XDB_NOLIC = 6312;

		// Token: 0x04000529 RID: 1321
		public const short FE_MALLOC_VMS = 6313;

		// Token: 0x0400052A RID: 1322
		public const short FE_TIME_VMS = 6314;

		// Token: 0x0400052B RID: 1323
		public const short FE_BLKMEM_EXPANSION = 6315;

		// Token: 0x0400052C RID: 1324
		public const short FE_RDB_DCPATH = 6400;

		// Token: 0x0400052D RID: 1325
		public const short FE_RDB_DCOPATH = 6401;

		// Token: 0x0400052E RID: 1326
		public const short FE_RDB_DMACS = 6402;

		// Token: 0x0400052F RID: 1327
		public const short FE_RDB_DATABASE = 6403;

		// Token: 0x04000530 RID: 1328
		public const short FE_RDB_ARGS = 6404;

		// Token: 0x04000531 RID: 1329
		public const short FE_RDB_QUALIFIER = 6405;

		// Token: 0x04000532 RID: 1330
		public const short FE_RDB_DELIMETER1 = 6406;

		// Token: 0x04000533 RID: 1331
		public const short FE_RDB_DELIMETER2 = 6407;

		// Token: 0x04000534 RID: 1332
		public const short FE_RDB_IPC_SETUP = 6408;

		// Token: 0x04000535 RID: 1333
		public const short FE_RDB_MALLOC_FAIL = 6409;

		// Token: 0x04000536 RID: 1334
		public const short FE_RDB_RENAME = 6410;

		// Token: 0x04000537 RID: 1335
		public const short FE_RDB_SCHEMA = 6411;

		// Token: 0x04000538 RID: 1336
		public const short FE_RDB_TABLE = 6412;

		// Token: 0x04000539 RID: 1337
		public const short FE_RDB_INSERT = 6413;

		// Token: 0x0400053A RID: 1338
		public const short FE_RDB_COMMIT = 6414;

		// Token: 0x0400053B RID: 1339
		public const short FE_RDB_IMMEDIATE = 6415;

		// Token: 0x0400053C RID: 1340
		public const short FE_RDB_PREPARE = 6416;

		// Token: 0x0400053D RID: 1341
		public const short FE_RDB_DESCRIBE = 6417;

		// Token: 0x0400053E RID: 1342
		public const short FE_RDB_RELEASE = 6418;

		// Token: 0x0400053F RID: 1343
		public const short FE_RDB_DATE = 6419;

		// Token: 0x04000540 RID: 1344
		public const short FM_RDB_INSERT = 6420;

		// Token: 0x04000541 RID: 1345
		public const short FM_RDB_COMMIT = 6421;

		// Token: 0x04000542 RID: 1346
		public const short FM_RDB_BEGIN = 6423;

		// Token: 0x04000543 RID: 1347
		public const short FM_RDB_SCHEMA = 6424;

		// Token: 0x04000544 RID: 1348
		public const short FM_RDB_CREATE_TABLE = 6425;

		// Token: 0x04000545 RID: 1349
		public const short FE_Q2_EXCEEDING_MAX_QUES = 6500;

		// Token: 0x04000546 RID: 1350
		public const short FE_Q2_NO_HANDLE_AVAILABLE = 6501;

		// Token: 0x04000547 RID: 1351
		public const short FE_Q2_NO_MEMORY_FOR_QUE = 6502;

		// Token: 0x04000548 RID: 1352
		public const short FE_Q2_ITEM_SIZE_TOO_LARGE = 6503;

		// Token: 0x04000549 RID: 1353
		public const short FE_Q2_BAD_ITEM_SIZE = 6504;

		// Token: 0x0400054A RID: 1354
		public const short FE_Q2_BAD_ITEM_COUNT = 6505;

		// Token: 0x0400054B RID: 1355
		public const short FE_Q2_MISMATCH_HANDLE_NAME = 6506;

		// Token: 0x0400054C RID: 1356
		public const short FE_Q2_NO_MATCHING_HANDLE = 6507;

		// Token: 0x0400054D RID: 1357
		public const short FE_Q2_INVALID_HANDLE = 6508;

		// Token: 0x0400054E RID: 1358
		public const short FE_Q2_QUE_EXISTS = 6509;

		// Token: 0x0400054F RID: 1359
		public const short FE_Q2_QUE_EMPTY = 6510;

		// Token: 0x04000550 RID: 1360
		public const short FE_Q2_EMS_INITIALIZATION = 6511;

		// Token: 0x04000551 RID: 1361
		public const short FE_Q2_NO_EMS = 6512;

		// Token: 0x04000552 RID: 1362
		public const short FE_Q2_NO_EMS_MEMORY = 6513;

		// Token: 0x04000553 RID: 1363
		public const short FE_Q2_NO_EMS_32K_WINDOW = 6514;

		// Token: 0x04000554 RID: 1364
		public const short FE_Q2_NO_FCA_TABLE = 6515;

		// Token: 0x04000555 RID: 1365
		public const short FE_Q2_SIZE_EMS_CONTEXT = 6516;

		// Token: 0x04000556 RID: 1366
		public const short FE_Q2_MEMORY_TO_SAVE_CONTEXT = 6517;

		// Token: 0x04000557 RID: 1367
		public const short FE_Q2_SAVE_CONTEXT = 6518;

		// Token: 0x04000558 RID: 1368
		public const short FE_Q2_MAP_EMS_WINDOW = 6519;

		// Token: 0x04000559 RID: 1369
		public const short FE_Q2_RESTORE_CONTEXT = 6520;

		// Token: 0x0400055A RID: 1370
		public const short FE_Q2_FULL = 6521;

		// Token: 0x0400055B RID: 1371
		public const short FE_Q2_LOCKED = 6522;

		// Token: 0x0400055C RID: 1372
		public const short FE_LOCK_FAIL = 6600;

		// Token: 0x0400055D RID: 1373
		public const short FE_UNLOCK_FAIL = 6601;

		// Token: 0x0400055E RID: 1374
		public const short FE_SEC_NO_USER = 6700;

		// Token: 0x0400055F RID: 1375
		public const short FE_SEC_BAD_PSWD = 6701;

		// Token: 0x04000560 RID: 1376
		public const short FE_SEC_ACCESS = 6702;

		// Token: 0x04000561 RID: 1377
		public const short FE_SEC_BADBASE = 6703;

		// Token: 0x04000562 RID: 1378
		public const short FE_SEC_BAD_CAT = 6704;

		// Token: 0x04000563 RID: 1379
		public const short FE_SEC_NO_REMOTE_USER = 6705;

		// Token: 0x04000564 RID: 1380
		public const short FE_SEC_BAD_AREA = 6706;

		// Token: 0x04000565 RID: 1381
		public const short FE_SEC_SHUT_MSG = 6707;

		// Token: 0x04000566 RID: 1382
		public const short FE_SEC_LOGON_FAILURE = 6711;

		// Token: 0x04000567 RID: 1383
		public const short FE_SEC_ACCOUNT_RESTRICTION = 6712;

		// Token: 0x04000568 RID: 1384
		public const short FE_SEC_INVALID_LOGON_HOURS = 6713;

		// Token: 0x04000569 RID: 1385
		public const short FE_SEC_INVALID_WORKSTATION = 6714;

		// Token: 0x0400056A RID: 1386
		public const short FE_SEC_PASSWORD_EXPIRED = 6715;

		// Token: 0x0400056B RID: 1387
		public const short FE_SEC_ACCOUNT_DISABLED = 6716;

		// Token: 0x0400056C RID: 1388
		public const short FE_SEC_KEY_ERROR = 6717;

		// Token: 0x0400056D RID: 1389
		public const short FE_SEC_ACCOUNT_EXPIRED = 6718;

		// Token: 0x0400056E RID: 1390
		public const short FE_SEC_ACCOUNT_LOCKED_OUT = 6719;

		// Token: 0x0400056F RID: 1391
		public const short FE_SEC_ERROR_PRIVILEGE_NOT_HELD = 6720;

		// Token: 0x04000570 RID: 1392
		public const short FE_XNTF_SYNTAX_BAD = 6800;

		// Token: 0x04000571 RID: 1393
		public const short FE_XNTF_SYNTAX_NO_NODE = 6801;

		// Token: 0x04000572 RID: 1394
		public const short FE_XNTF_SYNTAX_BAD_NODESIZE = 6802;

		// Token: 0x04000573 RID: 1395
		public const short FE_XNTF_SYNTAX_NO_TAG = 6803;

		// Token: 0x04000574 RID: 1396
		public const short FE_XNTF_SYNTAX_BAD_TAGSIZE = 6804;

		// Token: 0x04000575 RID: 1397
		public const short FE_XNTF_SYNTAX_NO_FIELD = 6805;

		// Token: 0x04000576 RID: 1398
		public const short FE_XNTF_SYNTAX_BAD_FIELDSIZE = 6806;

		// Token: 0x04000577 RID: 1399
		public const short FE_XNTF_SYNTAX_NT_BAD = 6807;

		// Token: 0x04000578 RID: 1400
		public const short FE_XNTF_SYNTAX_NT_NO_NODE = 6808;

		// Token: 0x04000579 RID: 1401
		public const short FE_XNTF_SYNTAX_NT_NO_TAG = 6809;

		// Token: 0x0400057A RID: 1402
		public const short FE_XNTF_SYNTAX_NT_BAD_TAGSIZE = 6810;

		// Token: 0x0400057B RID: 1403
		public const short FE_XNTF_SYNTAX_NF_BAD = 6811;

		// Token: 0x0400057C RID: 1404
		public const short FE_XNTF_SYNTAX_NF_NO_NODE = 6812;

		// Token: 0x0400057D RID: 1405
		public const short FE_XNTF_SYNTAX_NF_NO_FIELD = 6813;

		// Token: 0x0400057E RID: 1406
		public const short FE_XNTF_SYNTAX_N_BAD = 6814;

		// Token: 0x0400057F RID: 1407
		public const short FE_XNTF_SYNTAX_N_NO_NODE = 6815;

		// Token: 0x04000580 RID: 1408
		public const short FE_IM_A_TAGGROUP = 6816;

		// Token: 0x04000581 RID: 1409
		public const short FE_ASCII_DATA_MISMATCH = 6817;

		// Token: 0x04000582 RID: 1410
		public const short FE_FLOAT_DATA_MISMATCH = 6818;

		// Token: 0x04000583 RID: 1411
		public const short FE_GRAPH_DATA_MISMATCH = 6819;

		// Token: 0x04000584 RID: 1412
		public const short FE_BINARY_DATA_MISMATCH = 6820;

		// Token: 0x04000585 RID: 1413
		public const short FE_IM_A_VARIABLE = 6821;

		// Token: 0x04000586 RID: 1414
		public const short FE_FIELD_SELECT = 6822;

		// Token: 0x04000587 RID: 1415
		public const short FE_ALLOC_NODE_LIST = 6823;

		// Token: 0x04000588 RID: 1416
		public const short FE_ALLOC_FIELD_LIST = 6824;

		// Token: 0x04000589 RID: 1417
		public const short FE_GETTING_NODE_LIST = 6825;

		// Token: 0x0400058A RID: 1418
		public const short FE_RETRIEVING_TAGS_FROM = 6826;

		// Token: 0x0400058B RID: 1419
		public const short FE_NO_TAGS_IN_DATABASE = 6827;

		// Token: 0x0400058C RID: 1420
		public const short FE_ALLOC_TAG_LIST = 6828;

		// Token: 0x0400058D RID: 1421
		public const short FE_GETTING_TAG_LIST = 6829;

		// Token: 0x0400058E RID: 1422
		public const short FE_TAGNAME_VERIFY_PROBLEM = 6830;

		// Token: 0x0400058F RID: 1423
		public const short FE_SELECTED_NODE_TEXT = 6831;

		// Token: 0x04000590 RID: 1424
		public const short FE_GETTING_FIELD_LIST = 6832;

		// Token: 0x04000591 RID: 1425
		public const short FE_SELECTED_TAG_TEXT = 6833;

		// Token: 0x04000592 RID: 1426
		public const short FE_GETTING_BLOCK_TYPE = 6834;

		// Token: 0x04000593 RID: 1427
		public const short FE_TAG_SELECT = 6835;

		// Token: 0x04000594 RID: 1428
		public const short FE_NEED_TO_SELECT_NODE = 6836;

		// Token: 0x04000595 RID: 1429
		public const short FE_NO_BOS_TAGS_IN_DATABASE = 6837;

		// Token: 0x04000596 RID: 1430
		public const short FE_XNTF_NODE_SELECT = 6840;

		// Token: 0x04000597 RID: 1431
		public const short FE_IM_A_DDETAG = 6842;

		// Token: 0x04000598 RID: 1432
		public const short FE_ICOUNTRY = 6845;

		// Token: 0x04000599 RID: 1433
		public const short FE_XNTF_NO_NODES_MATCH = 6850;

		// Token: 0x0400059A RID: 1434
		public const short FE_XNTF_NO_TAGS_MATCH = 6851;

		// Token: 0x0400059B RID: 1435
		public const short FE_XNTF_NO_FIELDS_MATCH = 6852;

		// Token: 0x0400059C RID: 1436
		public const short FE_XNTF_FILL_LISTBOX_NOTE = 6853;

		// Token: 0x0400059D RID: 1437
		public const short FE_XNTF_ZERO_ELEMENTS = 6854;

		// Token: 0x0400059E RID: 1438
		public const short FE_XNTF_ADD_TO_LISTBOX = 6855;

		// Token: 0x0400059F RID: 1439
		public const short FE_TAG_PREFIX = 6860;

		// Token: 0x040005A0 RID: 1440
		public const short FE_DDE_APPL_CLIPPED = 6861;

		// Token: 0x040005A1 RID: 1441
		public const short FE_DDE_TOPIC_CLIPPED = 6862;

		// Token: 0x040005A2 RID: 1442
		public const short FE_DDE_ITEM_CLIPPED = 6863;

		// Token: 0x040005A3 RID: 1443
		public const short FE_VARIABLE_NAME = 6864;

		// Token: 0x040005A4 RID: 1444
		public const short FE_REFRESHING_TAG_LIST = 6870;

		// Token: 0x040005A5 RID: 1445
		public const short FE_REFRESH_READ = 6871;

		// Token: 0x040005A6 RID: 1446
		public const short FE_REFRESH_BAD_BYTE_CNT = 6872;

		// Token: 0x040005A7 RID: 1447
		public const short FE_REFRESH_WRITE_HEADER = 6873;

		// Token: 0x040005A8 RID: 1448
		public const short FE_REFRESH_WRITE_TMP = 6874;

		// Token: 0x040005A9 RID: 1449
		public const short FE_REFRESH_REMOVE_TMP = 6875;

		// Token: 0x040005AA RID: 1450
		public const short FE_REFRESH_RENAME_TMP = 6876;

		// Token: 0x040005AB RID: 1451
		public const short FE_XNTF_30CHAR_OVERFLOW = 6880;

		// Token: 0x040005AC RID: 1452
		public const short FE_XNTF_VALIDATE_NODE = 6941;

		// Token: 0x040005AD RID: 1453
		public const short FE_CS2_INIT = 7000;

		// Token: 0x040005AE RID: 1454
		public const short FE_ADD_CCB = 7001;

		// Token: 0x040005AF RID: 1455
		public const short FE_DAE_RDB_QUE_WAIT = 7010;

		// Token: 0x040005B0 RID: 1456
		public const short FE_DAE_RDB_QUE = 7011;

		// Token: 0x040005B1 RID: 1457
		public const short FE_DAE_RDB_REQUEST = 7012;

		// Token: 0x040005B2 RID: 1458
		public const short FE_DAE_RDB_RESPONSE = 7013;

		// Token: 0x040005B3 RID: 1459
		public const short FE_DAE_RDBEXT_MEM = 7014;

		// Token: 0x040005B4 RID: 1460
		public const short FE_DAE_INV_ADAPTER = 7015;

		// Token: 0x040005B5 RID: 1461
		public const short FE_DDM_NOT_CONNECT = 7020;

		// Token: 0x040005B6 RID: 1462
		public const short FE_DDM_VAR_NOTDEF = 7021;

		// Token: 0x040005B7 RID: 1463
		public const short FE_DDM_VAR_NOTPRIME = 7022;

		// Token: 0x040005B8 RID: 1464
		public const short FE_DDM_BAD_INDEX = 7023;

		// Token: 0x040005B9 RID: 1465
		public const short FE_DDM_BAD_OFFSET = 7024;

		// Token: 0x040005BA RID: 1466
		public const short FE_DDM_NOT_PHYSADDR = 7025;

		// Token: 0x040005BB RID: 1467
		public const short FE_UDCS_QUE_FULL = 7030;

		// Token: 0x040005BC RID: 1468
		public const short FE_UDCS_BAD_FUNC = 7031;

		// Token: 0x040005BD RID: 1469
		public const short FE_UDCS_BUF_SIZE = 7032;

		// Token: 0x040005BE RID: 1470
		public const short FE_READING_INTL = 7039;

		// Token: 0x040005BF RID: 1471
		public const short FE_INTERNATIONAL = 7040;

		// Token: 0x040005C0 RID: 1472
		public const short FE_TIMESEPARATOR = 7041;

		// Token: 0x040005C1 RID: 1473
		public const short FE_TIMEFORMAT = 7042;

		// Token: 0x040005C2 RID: 1474
		public const short FE_WIN2DATEFORMAT = 7043;

		// Token: 0x040005C3 RID: 1475
		public const short FE_WIN2DATESEPARATOR = 7044;

		// Token: 0x040005C4 RID: 1476
		public const short FE_SHORTDATEFORMAT = 7045;

		// Token: 0x040005C5 RID: 1477
		public const short FE_TIMELEADINGZERO = 7046;

		// Token: 0x040005C6 RID: 1478
		public const short FE_AMSTRING = 7047;

		// Token: 0x040005C7 RID: 1479
		public const short FE_PMSTRING = 7048;

		// Token: 0x040005C8 RID: 1480
		public const short FE_SEP_THOUS = 7049;

		// Token: 0x040005C9 RID: 1481
		public const short FE_SEP_DECIMAL = 7050;

		// Token: 0x040005CA RID: 1482
		public const short FE_SEP_LIST = 7051;

		// Token: 0x040005CB RID: 1483
		public const short FE_MEASURE_SYS = 7052;

		// Token: 0x040005CC RID: 1484
		public const short FE_COUNTRY_NAME = 7053;

		// Token: 0x040005CD RID: 1485
		public const short FE_LANG_NAME = 7054;

		// Token: 0x040005CE RID: 1486
		public const short FE_COUNTRY_CODE = 7055;

		// Token: 0x040005CF RID: 1487
		public const short FE_VARIABLE = 8000;

		// Token: 0x040005D0 RID: 1488
		public const short FE_UNIT = 8001;

		// Token: 0x040005D1 RID: 1489
		public const short FE_SQ_NOTFND = 8002;

		// Token: 0x040005D2 RID: 1490
		public const short FE_QFULL = 8003;

		// Token: 0x040005D3 RID: 1491
		public const short FE_QEMPTY = 8004;

		// Token: 0x040005D4 RID: 1492
		public const short FE_UTAG = 8005;

		// Token: 0x040005D5 RID: 1493
		public const short FE_NOT_DIGITAL = 8006;

		// Token: 0x040005D6 RID: 1494
		public const short FE_INDEX = 8007;

		// Token: 0x040005D7 RID: 1495
		public const short FE_TOMANY_SEQ = 8008;

		// Token: 0x040005D8 RID: 1496
		public const short FE_TOMANY_LINE = 8009;

		// Token: 0x040005D9 RID: 1497
		public const short FE_VAR_DUP = 8010;

		// Token: 0x040005DA RID: 1498
		public const short FE_ONSCAN = 8011;

		// Token: 0x040005DB RID: 1499
		public const short FE_ONAUTO = 8012;

		// Token: 0x040005DC RID: 1500
		public const short FE_SQ_FOUND = 8013;

		// Token: 0x040005DD RID: 1501
		public const short FE_NO_SQ_ACT = 8014;

		// Token: 0x040005DE RID: 1502
		public const short FE_NOPHASE_STAT = 8015;

		// Token: 0x040005DF RID: 1503
		public const short FE_SPAWN_DNLDR = 8016;

		// Token: 0x040005E0 RID: 1504
		public const short FE_NOSEQ_STAT = 8017;

		// Token: 0x040005E1 RID: 1505
		public const short FE_INV_CMD = 8018;

		// Token: 0x040005E2 RID: 1506
		public const short FE_NO_PH_ACT = 8019;

		// Token: 0x040005E3 RID: 1507
		public const short FE_BATCH_STARTING = 8020;

		// Token: 0x040005E4 RID: 1508
		public const short FE_UNIT_DUP = 8021;

		// Token: 0x040005E5 RID: 1509
		public const short FE_S2S_NOLOGIN = 8022;

		// Token: 0x040005E6 RID: 1510
		public const short FE_NO_SEQ_SIZE = 8023;

		// Token: 0x040005E7 RID: 1511
		public const short FE_NO_SEQ_DATA = 8024;

		// Token: 0x040005E8 RID: 1512
		public const short FE_NO_RT_SIZE = 8025;

		// Token: 0x040005E9 RID: 1513
		public const short FE_NO_RT_DATA = 8026;

		// Token: 0x040005EA RID: 1514
		public const short FE_VAR_NORES = 8027;

		// Token: 0x040005EB RID: 1515
		public const short FE_UVAR_NORES = 8028;

		// Token: 0x040005EC RID: 1516
		public const short FE_S2S_NOT_RUNNING = 8029;

		// Token: 0x040005ED RID: 1517
		public const short FE_BATCH_ON_LINE = 8030;

		// Token: 0x040005EE RID: 1518
		public const short FE_NOT_DI = 8031;

		// Token: 0x040005EF RID: 1519
		public const short FE_NOT_DO = 8032;

		// Token: 0x040005F0 RID: 1520
		public const short FE_VAR_BAD = 8033;

		// Token: 0x040005F1 RID: 1521
		public const short FE_UVAR_BAD = 8034;

		// Token: 0x040005F2 RID: 1522
		public const short FE_NO_PROC_IN_SLOT = 8035;

		// Token: 0x040005F3 RID: 1523
		public const short FE_SAC_NOT_RUNNING = 8036;

		// Token: 0x040005F4 RID: 1524
		public const short FE_UNIT_INUSE = 8037;

		// Token: 0x040005F5 RID: 1525
		public const short FE_LINE_INUSE = 8038;

		// Token: 0x040005F6 RID: 1526
		public const short FE_DEL_TRANSIENT = 8039;

		// Token: 0x040005F7 RID: 1527
		public const short FE_MOD_TRANSIENT = 8040;

		// Token: 0x040005F8 RID: 1528
		public const short FE_EXPORT_TYPE = 8041;

		// Token: 0x040005F9 RID: 1529
		public const short FE_BATCH_STOPPED = 8042;

		// Token: 0x040005FA RID: 1530
		public const short FE_PROC_HOLD = 8043;

		// Token: 0x040005FB RID: 1531
		public const short FE_PHASE_HOLD = 8044;

		// Token: 0x040005FC RID: 1532
		public const short FE_ABORT_UPDATE = 8045;

		// Token: 0x040005FD RID: 1533
		public const short FE_COMPLETE_UPDATE = 8046;

		// Token: 0x040005FE RID: 1534
		public const short FE_OPERATE_DATA = 8047;

		// Token: 0x040005FF RID: 1535
		public const short FE_UNIT_NOUPD = 8048;

		// Token: 0x04000600 RID: 1536
		public const short FE_RERESOLVE = 8049;

		// Token: 0x04000601 RID: 1537
		public const short FE_OPER_TRACE = 8050;

		// Token: 0x04000602 RID: 1538
		public const short FE_INPROC_STAT = 8051;

		// Token: 0x04000603 RID: 1539
		public const short FE_PRODNUM_RPT = 8052;

		// Token: 0x04000604 RID: 1540
		public const short FE_BATCH_COMPLETE = 8053;

		// Token: 0x04000605 RID: 1541
		public const short FE_NOLINE_STAT = 8054;

		// Token: 0x04000606 RID: 1542
		public const short FE_PRESTART_UPDT = 8055;

		// Token: 0x04000607 RID: 1543
		public const short FE_PROC_ROW = 8056;

		// Token: 0x04000608 RID: 1544
		public const short FE_OPERATION = 8057;

		// Token: 0x04000609 RID: 1545
		public const short FE_RECORD_STAT = 8058;

		// Token: 0x0400060A RID: 1546
		public const short FE_INV_LOT = 8059;

		// Token: 0x0400060B RID: 1547
		public const short FE_WEIGH_STAT = 8060;

		// Token: 0x0400060C RID: 1548
		public const short FE_INGR_USED = 8061;

		// Token: 0x0400060D RID: 1549
		public const short FE_IF_EXPRESS = 8062;

		// Token: 0x0400060E RID: 1550
		public const short FE_TERMINATE_PHS = 8063;

		// Token: 0x0400060F RID: 1551
		public const short FE_ON_HOLD = 8064;

		// Token: 0x04000610 RID: 1552
		public const short FE_RUN_IT = 8065;

		// Token: 0x04000611 RID: 1553
		public const short FE_TYPE_MISMAT = 8100;

		// Token: 0x04000612 RID: 1554
		public const short FE_BAD_PARM = 8101;

		// Token: 0x04000613 RID: 1555
		public const short FE_NUM_PARM = 8102;

		// Token: 0x04000614 RID: 1556
		public const short FE_INV_PARM = 8103;

		// Token: 0x04000615 RID: 1557
		public const short FE_INV_BATKEY = 8104;

		// Token: 0x04000616 RID: 1558
		public const short FE_NUM_BKPARM = 8105;

		// Token: 0x04000617 RID: 1559
		public const short FE_TYPE_BKPARM = 8106;

		// Token: 0x04000618 RID: 1560
		public const short FE_INV_INGRNO = 8107;

		// Token: 0x04000619 RID: 1561
		public const short FE_INV_IVARNO = 8108;

		// Token: 0x0400061A RID: 1562
		public const short FE_INV_PTLNO = 8109;

		// Token: 0x0400061B RID: 1563
		public const short FE_TASK_NORES = 8110;

		// Token: 0x0400061C RID: 1564
		public const short FE_FBLK_TYPE = 8111;

		// Token: 0x0400061D RID: 1565
		public const short FE_TAG_TYPE = 8112;

		// Token: 0x0400061E RID: 1566
		public const short FE_LOOP_PREST = 8113;

		// Token: 0x0400061F RID: 1567
		public const short FE_INV_CLASS = 8114;

		// Token: 0x04000620 RID: 1568
		public const short FE_INV_UOM = 8115;

		// Token: 0x04000621 RID: 1569
		public const short FE_VAR_SIZE4 = 8116;

		// Token: 0x04000622 RID: 1570
		public const short FE_NEED_QUOTES = 8117;

		// Token: 0x04000623 RID: 1571
		public const short FE_TASK_INPROC = 8118;

		// Token: 0x04000624 RID: 1572
		public const short FE_INV_EXPR = 8119;

		// Token: 0x04000625 RID: 1573
		public const short FE_INV_LKW = 8150;

		// Token: 0x04000626 RID: 1574
		public const short FE_NO_LBLDATA = 8151;

		// Token: 0x04000627 RID: 1575
		public const short FE_INV_EXPDATE = 8152;

		// Token: 0x04000628 RID: 1576
		public const short FE_INV_LOTNO = 8153;

		// Token: 0x04000629 RID: 1577
		public const short FE_INV_LOC = 8154;

		// Token: 0x0400062A RID: 1578
		public const short FE_INV_SETI = 8155;

		// Token: 0x0400062B RID: 1579
		public const short FE_LBL_SIZE = 8156;

		// Token: 0x0400062C RID: 1580
		public const short FE_TK_UNALL = 8172;

		// Token: 0x0400062D RID: 1581
		public const short FE_TK_XFER = 8173;

		// Token: 0x0400062E RID: 1582
		public const short FE_TK_BKWD = 8174;

		// Token: 0x0400062F RID: 1583
		public const short FE_TK_RES = 8175;

		// Token: 0x04000630 RID: 1584
		public const short FE_TK_EVALPARM = 8176;

		// Token: 0x04000631 RID: 1585
		public const short FE_END_BLK = 8177;

		// Token: 0x04000632 RID: 1586
		public const short FE_SQ_ERROR = 8178;

		// Token: 0x04000633 RID: 1587
		public const short FE_TK_UNDAL = 8179;

		// Token: 0x04000634 RID: 1588
		public const short FE_INGRDATA_NOTF = 8180;

		// Token: 0x04000635 RID: 1589
		public const short FE_SEQ_REPORT = 8181;

		// Token: 0x04000636 RID: 1590
		public const short FE_LINE_POPQ = 8182;

		// Token: 0x04000637 RID: 1591
		public const short FE_LN_BSTART = 8183;

		// Token: 0x04000638 RID: 1592
		public const short FE_UNUSED_UNIT = 8184;

		// Token: 0x04000639 RID: 1593
		public const short FE_TK_ERROR = 8185;

		// Token: 0x0400063A RID: 1594
		public const short FE_TK_STEP = 8186;

		// Token: 0x0400063B RID: 1595
		public const short FE_TK_OPEN = 8187;

		// Token: 0x0400063C RID: 1596
		public const short FE_TK_CLOSE = 8188;

		// Token: 0x0400063D RID: 1597
		public const short FE_TK_AUTO = 8189;

		// Token: 0x0400063E RID: 1598
		public const short FE_TK_MANL = 8190;

		// Token: 0x0400063F RID: 1599
		public const short FE_INV_LINE = 8200;

		// Token: 0x04000640 RID: 1600
		public const short FE_INV_UNIT = 8201;

		// Token: 0x04000641 RID: 1601
		public const short FE_INV_TASK = 8202;

		// Token: 0x04000642 RID: 1602
		public const short FE_SEQ_NORES = 8203;

		// Token: 0x04000643 RID: 1603
		public const short FE_MAX_SEQXFER = 8204;

		// Token: 0x04000644 RID: 1604
		public const short FE_INV_PHASE = 8205;

		// Token: 0x04000645 RID: 1605
		public const short FE_PHASE_ACTIVE = 8206;

		// Token: 0x04000646 RID: 1606
		public const short FE_PHASE_INACTIVE = 8207;

		// Token: 0x04000647 RID: 1607
		public const short FE_OPER_HOLD = 8208;

		// Token: 0x04000648 RID: 1608
		public const short FE_SEQ_HOLD = 8209;

		// Token: 0x04000649 RID: 1609
		public const short FE_TASK_HOLD = 8210;

		// Token: 0x0400064A RID: 1610
		public const short FE_INV_ACTION = 8211;

		// Token: 0x0400064B RID: 1611
		public const short FE_INV_SEQ = 8212;

		// Token: 0x0400064C RID: 1612
		public const short FE_UNIT_HRA = 8213;

		// Token: 0x0400064D RID: 1613
		public const short FE_PH_HOLD_AGAIN = 8214;

		// Token: 0x0400064E RID: 1614
		public const short FE_PH_RESUME_AGAIN = 8215;

		// Token: 0x0400064F RID: 1615
		public const short FE_SQ_HOLD_AGAIN = 8216;

		// Token: 0x04000650 RID: 1616
		public const short FE_SQ_RESUME_AGAIN = 8217;

		// Token: 0x04000651 RID: 1617
		public const short FE_PH_TK_HELD = 8218;

		// Token: 0x04000652 RID: 1618
		public const short FE_PH_HOLD_RES = 8219;

		// Token: 0x04000653 RID: 1619
		public const short FE_SEQ_ACCESS = 8220;

		// Token: 0x04000654 RID: 1620
		public const short FE_UNIT_CMD = 8221;

		// Token: 0x04000655 RID: 1621
		public const short FE_NO_BATCH = 8222;

		// Token: 0x04000656 RID: 1622
		public const short FE_CANT_CHAIN_LNBLK = 8223;

		// Token: 0x04000657 RID: 1623
		public const short FE_CANT_CHAIN_UNBLK = 8224;

		// Token: 0x04000658 RID: 1624
		public const short FE_CANT_CHAIN_PHBLK = 8225;

		// Token: 0x04000659 RID: 1625
		public const short FE_DUP_PROC_TAG = 8226;

		// Token: 0x0400065A RID: 1626
		public const short FE_PROC_VERSION = 8227;

		// Token: 0x0400065B RID: 1627
		public const short FE_RCP_FILETYPE = 8300;

		// Token: 0x0400065C RID: 1628
		public const short FE_RCP_NOMATCH = 8301;

		// Token: 0x0400065D RID: 1629
		public const short FE_RCP_ITEMUNDEF = 8302;

		// Token: 0x0400065E RID: 1630
		public const short FE_RCP_TAGCORRUPT = 8303;

		// Token: 0x0400065F RID: 1631
		public const short FE_RCP_BADSTRING = 8304;

		// Token: 0x04000660 RID: 1632
		public const short FE_RCP_DINTERLOCK = 8305;

		// Token: 0x04000661 RID: 1633
		public const short FE_RCP_UINTERLOCK = 8306;

		// Token: 0x04000662 RID: 1634
		public const short FE_RCP_COMPSTATUS = 8307;

		// Token: 0x04000663 RID: 1635
		public const short FE_ILLEGAL_RESPONSE = 8400;

		// Token: 0x04000664 RID: 1636
		public const short FE_MSG_TMO = 8401;

		// Token: 0x04000665 RID: 1637
		public const short FE_HARDWARE_ERROR = 8402;

		// Token: 0x04000666 RID: 1638
		public const short FE_ML_COMMAND_ERROR = 8403;

		// Token: 0x04000667 RID: 1639
		public const short FE_MODEM_FEATURE_UNSUPPORTED = 8404;

		// Token: 0x04000668 RID: 1640
		public const short FE_MODEM_INITIALIZED = 8405;

		// Token: 0x04000669 RID: 1641
		public const short FE_PORT_INITIALIZED = 8406;

		// Token: 0x0400066A RID: 1642
		public const short FE_MODEM_NOT_INITIALIZED = 8407;

		// Token: 0x0400066B RID: 1643
		public const short FE_PORT_NOT_INITIALIZED = 8408;

		// Token: 0x0400066C RID: 1644
		public const short FE_NO_PHONE_NUMBER = 8409;

		// Token: 0x0400066D RID: 1645
		public const short FE_MODEM_OK = 8420;

		// Token: 0x0400066E RID: 1646
		public const short FE_CONNECT = 8421;

		// Token: 0x0400066F RID: 1647
		public const short FE_RING = 8422;

		// Token: 0x04000670 RID: 1648
		public const short FE_NO_CARRIER = 8423;

		// Token: 0x04000671 RID: 1649
		public const short FE_MODEM_ERROR = 8424;

		// Token: 0x04000672 RID: 1650
		public const short FE_CONNECT_1200 = 8425;

		// Token: 0x04000673 RID: 1651
		public const short FE_NO_DIALTONE = 8426;

		// Token: 0x04000674 RID: 1652
		public const short FE_BUSY = 8427;

		// Token: 0x04000675 RID: 1653
		public const short FE_NO_ANSWER = 8428;

		// Token: 0x04000676 RID: 1654
		public const short FE_CONNECT_2400 = 8430;

		// Token: 0x04000677 RID: 1655
		public const short FE_CONNECT_4800 = 8431;

		// Token: 0x04000678 RID: 1656
		public const short FE_CONNECT_9600 = 8432;

		// Token: 0x04000679 RID: 1657
		public const short FE_CONNECT_19200 = 8434;

		// Token: 0x0400067A RID: 1658
		public const short FE_CONNECT_1200_75 = 8442;

		// Token: 0x0400067B RID: 1659
		public const short FE_CONNECT_75_1200 = 8443;

		// Token: 0x0400067C RID: 1660
		public const short FE_CONNECT_38400 = 8448;

		// Token: 0x0400067D RID: 1661
		public const short FE_LAST_MODEM_RESPONSE = 8460;

		// Token: 0x0400067E RID: 1662
		public const short FE_ACCESS = 8470;

		// Token: 0x0400067F RID: 1663
		public const short FE_LAB_CONNECT = 8471;

		// Token: 0x04000680 RID: 1664
		public const short FE_BAD_API = 8472;

		// Token: 0x04000681 RID: 1665
		public const short FE_BAD_GROUP = 8473;

		// Token: 0x04000682 RID: 1666
		public const short FE_BAD_EGUTAG = 8474;

		// Token: 0x04000683 RID: 1667
		public const short FE_BAD_DESC = 8475;

		// Token: 0x04000684 RID: 1668
		public const short FE_BAD_LABDATA = 8476;

		// Token: 0x04000685 RID: 1669
		public const short FE_BAD_USER = 8477;

		// Token: 0x04000686 RID: 1670
		public const short FE_BAD_PARAM = 8478;

		// Token: 0x04000687 RID: 1671
		public const short FE_INVALID_AALRM_BUF = 8480;

		// Token: 0x04000688 RID: 1672
		public const short FE_NO_BYTES_RECEIVED = 8481;

		// Token: 0x04000689 RID: 1673
		public const short FE_RECEIVED_ACK = 8482;

		// Token: 0x0400068A RID: 1674
		public const short FE_INVALID_AALARM_CFG_FILE = 8483;

		// Token: 0x0400068B RID: 1675
		public const short FE_PLANT3_PORT_TIMEOUT = 8484;

		// Token: 0x0400068C RID: 1676
		public const short FE_PLANT3_PORT_ERROR = 8485;

		// Token: 0x0400068D RID: 1677
		public const short FE_DISCONNECT_REMOIU_MSG = 8486;

		// Token: 0x0400068E RID: 1678
		public const short FE_DISABLE_REMOIU_MSG = 8487;

		// Token: 0x0400068F RID: 1679
		public const short FE_DISABLE_REMOIU_ERROR = 8488;

		// Token: 0x04000690 RID: 1680
		public const short FE_REMOIU_NOT_RUNNING = 8489;

		// Token: 0x04000691 RID: 1681
		public const short FE_XFER_TYPE = 8490;

		// Token: 0x04000692 RID: 1682
		public const short FE_WRONG_NODE = 8491;

		// Token: 0x04000693 RID: 1683
		public const short FE_XFER_ERROR = 8492;

		// Token: 0x04000694 RID: 1684
		public const short FE_FTU_BUSY = 8493;

		// Token: 0x04000695 RID: 1685
		public const short FE_BAD_REQ = 8494;

		// Token: 0x04000696 RID: 1686
		public const short FE_TCP_NETW_NOT_READY = 8501;

		// Token: 0x04000697 RID: 1687
		public const short FE_TCP_NOT_INIT = 8502;

		// Token: 0x04000698 RID: 1688
		public const short FE_TCP_VER_MISMATCH = 8503;

		// Token: 0x04000699 RID: 1689
		public const short FE_INVALID_PARAMETER = 8504;

		// Token: 0x0400069A RID: 1690
		public const short FE_NETWORK_DOWN = 8505;

		// Token: 0x0400069B RID: 1691
		public const short FE_BLOCK_IN_PROG = 8506;

		// Token: 0x0400069C RID: 1692
		public const short FE_MAX_OUTSTANDING_CONN = 8507;

		// Token: 0x0400069D RID: 1693
		public const short FE_MAX_OUTSTANDING_SEND = 8508;

		// Token: 0x0400069E RID: 1694
		public const short FE_MAX_CONNECTIONS = 8509;

		// Token: 0x0400069F RID: 1695
		public const short FE_TABLE_EMPTY = 8510;

		// Token: 0x040006A0 RID: 1696
		public const short FE_ADDNAME_FAILED = 8511;

		// Token: 0x040006A1 RID: 1697
		public const short FE_CONN_OUTSTANDING = 8512;

		// Token: 0x040006A2 RID: 1698
		public const short FE_NO_RECV_ALLOC_BUF = 8513;

		// Token: 0x040006A3 RID: 1699
		public const short FE_MAX_SENDBUF = 8514;

		// Token: 0x040006A4 RID: 1700
		public const short FE_TCP_NO_RES = 8515;

		// Token: 0x040006A5 RID: 1701
		public const short FE_TCP_NO_NETW_RCVBUF = 8516;

		// Token: 0x040006A6 RID: 1702
		public const short FE_TCP_NODE_NOTF = 8517;

		// Token: 0x040006A7 RID: 1703
		public const short FE_TCP_NODE_NOTF_W95 = 8518;

		// Token: 0x040006A8 RID: 1704
		public const short FE_LAN_FAILOVER_NOT_ENABLED = 8519;

		// Token: 0x040006A9 RID: 1705
		public const short FE_TCP_LAN_FAILOVER_OCCURRED = 8520;

		// Token: 0x040006AA RID: 1706
		public const short FE_NB_LAN_FAILOVER_OCCURRED = 8521;

		// Token: 0x040006AB RID: 1707
		public const short FE_LAN_FAILOVER_DISABLED = 8522;

		// Token: 0x040006AC RID: 1708
		public const short FE_LAN_FAILOVER_ENABLED = 8523;

		// Token: 0x040006AD RID: 1709
		public const short FE_TCP_PROTOCOL_NAME = 8524;

		// Token: 0x040006AE RID: 1710
		public const short FE_TABLE_SIZE = 8525;

		// Token: 0x040006AF RID: 1711
		public const short FE_ABORT_STARTUP = 8600;

		// Token: 0x040006B0 RID: 1712
		public const short FE_LOCAL_NODE_ALIAS = 8650;

		// Token: 0x040006B1 RID: 1713
		public const short FE_BAD_PARAMETER = 8700;

		// Token: 0x040006B2 RID: 1714
		public const short FE_VAR = 8750;

		// Token: 0x040006B3 RID: 1715
		public const short FE_KWIN_NOTOPEN = 8800;

		// Token: 0x040006B4 RID: 1716
		public const short FE_KWIN_NOARGUMENT = 8801;

		// Token: 0x040006B5 RID: 1717
		public const short FE_ESIG_INIT = 8850;

		// Token: 0x040006B6 RID: 1718
		public const short FE_ESIG_FIX32 = 8851;

		// Token: 0x040006B7 RID: 1719
		public const short FE_ESIG_CANCEL = 8852;

		// Token: 0x040006B8 RID: 1720
		public const short FE_ESIG_CV_CHANGE = 8853;

		// Token: 0x040006B9 RID: 1721
		public const short FE_ESIG_ACTION = 8854;

		// Token: 0x040006BA RID: 1722
		public const short FE_ESIG_FIELD_TYPE = 8855;

		// Token: 0x040006BB RID: 1723
		public const short FE_ESIG_READ = 8856;

		// Token: 0x040006BC RID: 1724
		public const short FE_ESIG_NOT_ENAB = 8857;

		// Token: 0x040006BD RID: 1725
		public const short FTK_OK = 11000;

		// Token: 0x040006BE RID: 1726
		public const short FTK_MEMORY = 11001;

		// Token: 0x040006BF RID: 1727
		public const short FTK_BAD_HDAGROUP = 11002;

		// Token: 0x040006C0 RID: 1728
		public const short FTK_BAD_HDANTF = 11003;

		// Token: 0x040006C1 RID: 1729
		public const short FTK_BAD_DATE = 11004;

		// Token: 0x040006C2 RID: 1730
		public const short FTK_BAD_TIME = 11005;

		// Token: 0x040006C3 RID: 1731
		public const short FTK_RANGE = 11006;

		// Token: 0x040006C4 RID: 1732
		public const short FTK_OPTION = 11007;

		// Token: 0x040006C5 RID: 1733
		public const short FTK_BADNTF = 11008;

		// Token: 0x040006C6 RID: 1734
		public const short FTK_BAD_LENGTH = 11009;

		// Token: 0x040006C7 RID: 1735
		public const short FTK_GROUP_FULL = 11010;

		// Token: 0x040006C8 RID: 1736
		public const short FTK_BAD_MHANDLE = 11011;

		// Token: 0x040006C9 RID: 1737
		public const short FTK_MORE_SAMPLES = 11012;

		// Token: 0x040006CA RID: 1738
		public const short FTK_NO_NTFS = 11013;

		// Token: 0x040006CB RID: 1739
		public const short FTK_NODENAME_NOT_DEFINED = 11014;

		// Token: 0x040006CC RID: 1740
		public const short FTK_NOT_REGISTERED = 11015;

		// Token: 0x040006CD RID: 1741
		public const short FTK_BAD_ORDER = 11016;

		// Token: 0x040006CE RID: 1742
		public const short FTK_NO_MESSAGE = 11017;

		// Token: 0x040006CF RID: 1743
		public const short FTK_FIX_NOT_RUNNING = 11018;

		// Token: 0x040006D0 RID: 1744
		public const short FTK_BAD_NAME = 11019;

		// Token: 0x040006D1 RID: 1745
		public const short FTK_BAD_PATH = 11020;

		// Token: 0x040006D2 RID: 1746
		public const short FTK_BAD_NODENAME = 11022;

		// Token: 0x040006D3 RID: 1747
		public const short FTK_NO_DATA = 11023;

		// Token: 0x040006D4 RID: 1748
		public const short FTK_ALREADY_REGISTERED = 11024;

		// Token: 0x040006D5 RID: 1749
		public const short FTK_CANNOT_REGISTER = 11025;

		// Token: 0x040006D6 RID: 1750
		public const short FTK_NO_USER = 11026;

		// Token: 0x040006D7 RID: 1751
		public const short FTK_NO_SECURITY = 11027;

		// Token: 0x040006D8 RID: 1752
		public const short FTK_INTERNAL_ERROR = 11028;

		// Token: 0x040006D9 RID: 1753
		public const short FTK_NO_FIX32 = 11029;

		// Token: 0x040006DA RID: 1754
		public const short FTK_FIX32_PRIOR6 = 11030;

		// Token: 0x040006DB RID: 1755
		public const short FTK_SEC_ACCESS = 11031;

		// Token: 0x040006DC RID: 1756
		public const short FTK_SEC_LOGGED_IN = 11032;

		// Token: 0x040006DD RID: 1757
		public const short FTK_SEC_BAD_AREA = 11033;

		// Token: 0x040006DE RID: 1758
		public const short FTK_NAC_NORUN = 11034;

		// Token: 0x040006DF RID: 1759
		public const short FTK_NAM_NORUN = 11035;

		// Token: 0x040006E0 RID: 1760
		public const short FTK_NAM_NOPAUSE = 11036;

		// Token: 0x040006E1 RID: 1761
		public const short FTK_IHSTATUS_API_TIMEOUT = 11037;

		// Token: 0x040006E2 RID: 1762
		public const short FTK_IHIST_ERRORCODES_SECTION = 11038;

		// Token: 0x040006E3 RID: 1763
		public const short FTK_IHIST_ERRORCODES_COUNT = 11039;

		// Token: 0x040006E4 RID: 1764
		public const short FTK_IHIST_ERRORCODES_VALUE = 11040;

		// Token: 0x040006E5 RID: 1765
		public const short FTK_CANT_FIND_IHIST = 11041;
	}
}

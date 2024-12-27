using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200045E RID: 1118
	[Flags]
	[ComVisible(true)]
	[TypeConverter(typeof(KeysConverter))]
	[Editor("System.Windows.Forms.Design.ShortcutKeysEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public enum Keys
	{
		// Token: 0x04001FB4 RID: 8116
		KeyCode = 65535,
		// Token: 0x04001FB5 RID: 8117
		Modifiers = -65536,
		// Token: 0x04001FB6 RID: 8118
		None = 0,
		// Token: 0x04001FB7 RID: 8119
		LButton = 1,
		// Token: 0x04001FB8 RID: 8120
		RButton = 2,
		// Token: 0x04001FB9 RID: 8121
		Cancel = 3,
		// Token: 0x04001FBA RID: 8122
		MButton = 4,
		// Token: 0x04001FBB RID: 8123
		XButton1 = 5,
		// Token: 0x04001FBC RID: 8124
		XButton2 = 6,
		// Token: 0x04001FBD RID: 8125
		Back = 8,
		// Token: 0x04001FBE RID: 8126
		Tab = 9,
		// Token: 0x04001FBF RID: 8127
		LineFeed = 10,
		// Token: 0x04001FC0 RID: 8128
		Clear = 12,
		// Token: 0x04001FC1 RID: 8129
		Return = 13,
		// Token: 0x04001FC2 RID: 8130
		Enter = 13,
		// Token: 0x04001FC3 RID: 8131
		ShiftKey = 16,
		// Token: 0x04001FC4 RID: 8132
		ControlKey = 17,
		// Token: 0x04001FC5 RID: 8133
		Menu = 18,
		// Token: 0x04001FC6 RID: 8134
		Pause = 19,
		// Token: 0x04001FC7 RID: 8135
		Capital = 20,
		// Token: 0x04001FC8 RID: 8136
		CapsLock = 20,
		// Token: 0x04001FC9 RID: 8137
		KanaMode = 21,
		// Token: 0x04001FCA RID: 8138
		HanguelMode = 21,
		// Token: 0x04001FCB RID: 8139
		HangulMode = 21,
		// Token: 0x04001FCC RID: 8140
		JunjaMode = 23,
		// Token: 0x04001FCD RID: 8141
		FinalMode = 24,
		// Token: 0x04001FCE RID: 8142
		HanjaMode = 25,
		// Token: 0x04001FCF RID: 8143
		KanjiMode = 25,
		// Token: 0x04001FD0 RID: 8144
		Escape = 27,
		// Token: 0x04001FD1 RID: 8145
		IMEConvert = 28,
		// Token: 0x04001FD2 RID: 8146
		IMENonconvert = 29,
		// Token: 0x04001FD3 RID: 8147
		IMEAccept = 30,
		// Token: 0x04001FD4 RID: 8148
		IMEAceept = 30,
		// Token: 0x04001FD5 RID: 8149
		IMEModeChange = 31,
		// Token: 0x04001FD6 RID: 8150
		Space = 32,
		// Token: 0x04001FD7 RID: 8151
		Prior = 33,
		// Token: 0x04001FD8 RID: 8152
		PageUp = 33,
		// Token: 0x04001FD9 RID: 8153
		Next = 34,
		// Token: 0x04001FDA RID: 8154
		PageDown = 34,
		// Token: 0x04001FDB RID: 8155
		End = 35,
		// Token: 0x04001FDC RID: 8156
		Home = 36,
		// Token: 0x04001FDD RID: 8157
		Left = 37,
		// Token: 0x04001FDE RID: 8158
		Up = 38,
		// Token: 0x04001FDF RID: 8159
		Right = 39,
		// Token: 0x04001FE0 RID: 8160
		Down = 40,
		// Token: 0x04001FE1 RID: 8161
		Select = 41,
		// Token: 0x04001FE2 RID: 8162
		Print = 42,
		// Token: 0x04001FE3 RID: 8163
		Execute = 43,
		// Token: 0x04001FE4 RID: 8164
		Snapshot = 44,
		// Token: 0x04001FE5 RID: 8165
		PrintScreen = 44,
		// Token: 0x04001FE6 RID: 8166
		Insert = 45,
		// Token: 0x04001FE7 RID: 8167
		Delete = 46,
		// Token: 0x04001FE8 RID: 8168
		Help = 47,
		// Token: 0x04001FE9 RID: 8169
		D0 = 48,
		// Token: 0x04001FEA RID: 8170
		D1 = 49,
		// Token: 0x04001FEB RID: 8171
		D2 = 50,
		// Token: 0x04001FEC RID: 8172
		D3 = 51,
		// Token: 0x04001FED RID: 8173
		D4 = 52,
		// Token: 0x04001FEE RID: 8174
		D5 = 53,
		// Token: 0x04001FEF RID: 8175
		D6 = 54,
		// Token: 0x04001FF0 RID: 8176
		D7 = 55,
		// Token: 0x04001FF1 RID: 8177
		D8 = 56,
		// Token: 0x04001FF2 RID: 8178
		D9 = 57,
		// Token: 0x04001FF3 RID: 8179
		A = 65,
		// Token: 0x04001FF4 RID: 8180
		B = 66,
		// Token: 0x04001FF5 RID: 8181
		C = 67,
		// Token: 0x04001FF6 RID: 8182
		D = 68,
		// Token: 0x04001FF7 RID: 8183
		E = 69,
		// Token: 0x04001FF8 RID: 8184
		F = 70,
		// Token: 0x04001FF9 RID: 8185
		G = 71,
		// Token: 0x04001FFA RID: 8186
		H = 72,
		// Token: 0x04001FFB RID: 8187
		I = 73,
		// Token: 0x04001FFC RID: 8188
		J = 74,
		// Token: 0x04001FFD RID: 8189
		K = 75,
		// Token: 0x04001FFE RID: 8190
		L = 76,
		// Token: 0x04001FFF RID: 8191
		M = 77,
		// Token: 0x04002000 RID: 8192
		N = 78,
		// Token: 0x04002001 RID: 8193
		O = 79,
		// Token: 0x04002002 RID: 8194
		P = 80,
		// Token: 0x04002003 RID: 8195
		Q = 81,
		// Token: 0x04002004 RID: 8196
		R = 82,
		// Token: 0x04002005 RID: 8197
		S = 83,
		// Token: 0x04002006 RID: 8198
		T = 84,
		// Token: 0x04002007 RID: 8199
		U = 85,
		// Token: 0x04002008 RID: 8200
		V = 86,
		// Token: 0x04002009 RID: 8201
		W = 87,
		// Token: 0x0400200A RID: 8202
		X = 88,
		// Token: 0x0400200B RID: 8203
		Y = 89,
		// Token: 0x0400200C RID: 8204
		Z = 90,
		// Token: 0x0400200D RID: 8205
		LWin = 91,
		// Token: 0x0400200E RID: 8206
		RWin = 92,
		// Token: 0x0400200F RID: 8207
		Apps = 93,
		// Token: 0x04002010 RID: 8208
		Sleep = 95,
		// Token: 0x04002011 RID: 8209
		NumPad0 = 96,
		// Token: 0x04002012 RID: 8210
		NumPad1 = 97,
		// Token: 0x04002013 RID: 8211
		NumPad2 = 98,
		// Token: 0x04002014 RID: 8212
		NumPad3 = 99,
		// Token: 0x04002015 RID: 8213
		NumPad4 = 100,
		// Token: 0x04002016 RID: 8214
		NumPad5 = 101,
		// Token: 0x04002017 RID: 8215
		NumPad6 = 102,
		// Token: 0x04002018 RID: 8216
		NumPad7 = 103,
		// Token: 0x04002019 RID: 8217
		NumPad8 = 104,
		// Token: 0x0400201A RID: 8218
		NumPad9 = 105,
		// Token: 0x0400201B RID: 8219
		Multiply = 106,
		// Token: 0x0400201C RID: 8220
		Add = 107,
		// Token: 0x0400201D RID: 8221
		Separator = 108,
		// Token: 0x0400201E RID: 8222
		Subtract = 109,
		// Token: 0x0400201F RID: 8223
		Decimal = 110,
		// Token: 0x04002020 RID: 8224
		Divide = 111,
		// Token: 0x04002021 RID: 8225
		F1 = 112,
		// Token: 0x04002022 RID: 8226
		F2 = 113,
		// Token: 0x04002023 RID: 8227
		F3 = 114,
		// Token: 0x04002024 RID: 8228
		F4 = 115,
		// Token: 0x04002025 RID: 8229
		F5 = 116,
		// Token: 0x04002026 RID: 8230
		F6 = 117,
		// Token: 0x04002027 RID: 8231
		F7 = 118,
		// Token: 0x04002028 RID: 8232
		F8 = 119,
		// Token: 0x04002029 RID: 8233
		F9 = 120,
		// Token: 0x0400202A RID: 8234
		F10 = 121,
		// Token: 0x0400202B RID: 8235
		F11 = 122,
		// Token: 0x0400202C RID: 8236
		F12 = 123,
		// Token: 0x0400202D RID: 8237
		F13 = 124,
		// Token: 0x0400202E RID: 8238
		F14 = 125,
		// Token: 0x0400202F RID: 8239
		F15 = 126,
		// Token: 0x04002030 RID: 8240
		F16 = 127,
		// Token: 0x04002031 RID: 8241
		F17 = 128,
		// Token: 0x04002032 RID: 8242
		F18 = 129,
		// Token: 0x04002033 RID: 8243
		F19 = 130,
		// Token: 0x04002034 RID: 8244
		F20 = 131,
		// Token: 0x04002035 RID: 8245
		F21 = 132,
		// Token: 0x04002036 RID: 8246
		F22 = 133,
		// Token: 0x04002037 RID: 8247
		F23 = 134,
		// Token: 0x04002038 RID: 8248
		F24 = 135,
		// Token: 0x04002039 RID: 8249
		NumLock = 144,
		// Token: 0x0400203A RID: 8250
		Scroll = 145,
		// Token: 0x0400203B RID: 8251
		LShiftKey = 160,
		// Token: 0x0400203C RID: 8252
		RShiftKey = 161,
		// Token: 0x0400203D RID: 8253
		LControlKey = 162,
		// Token: 0x0400203E RID: 8254
		RControlKey = 163,
		// Token: 0x0400203F RID: 8255
		LMenu = 164,
		// Token: 0x04002040 RID: 8256
		RMenu = 165,
		// Token: 0x04002041 RID: 8257
		BrowserBack = 166,
		// Token: 0x04002042 RID: 8258
		BrowserForward = 167,
		// Token: 0x04002043 RID: 8259
		BrowserRefresh = 168,
		// Token: 0x04002044 RID: 8260
		BrowserStop = 169,
		// Token: 0x04002045 RID: 8261
		BrowserSearch = 170,
		// Token: 0x04002046 RID: 8262
		BrowserFavorites = 171,
		// Token: 0x04002047 RID: 8263
		BrowserHome = 172,
		// Token: 0x04002048 RID: 8264
		VolumeMute = 173,
		// Token: 0x04002049 RID: 8265
		VolumeDown = 174,
		// Token: 0x0400204A RID: 8266
		VolumeUp = 175,
		// Token: 0x0400204B RID: 8267
		MediaNextTrack = 176,
		// Token: 0x0400204C RID: 8268
		MediaPreviousTrack = 177,
		// Token: 0x0400204D RID: 8269
		MediaStop = 178,
		// Token: 0x0400204E RID: 8270
		MediaPlayPause = 179,
		// Token: 0x0400204F RID: 8271
		LaunchMail = 180,
		// Token: 0x04002050 RID: 8272
		SelectMedia = 181,
		// Token: 0x04002051 RID: 8273
		LaunchApplication1 = 182,
		// Token: 0x04002052 RID: 8274
		LaunchApplication2 = 183,
		// Token: 0x04002053 RID: 8275
		OemSemicolon = 186,
		// Token: 0x04002054 RID: 8276
		Oem1 = 186,
		// Token: 0x04002055 RID: 8277
		Oemplus = 187,
		// Token: 0x04002056 RID: 8278
		Oemcomma = 188,
		// Token: 0x04002057 RID: 8279
		OemMinus = 189,
		// Token: 0x04002058 RID: 8280
		OemPeriod = 190,
		// Token: 0x04002059 RID: 8281
		OemQuestion = 191,
		// Token: 0x0400205A RID: 8282
		Oem2 = 191,
		// Token: 0x0400205B RID: 8283
		Oemtilde = 192,
		// Token: 0x0400205C RID: 8284
		Oem3 = 192,
		// Token: 0x0400205D RID: 8285
		OemOpenBrackets = 219,
		// Token: 0x0400205E RID: 8286
		Oem4 = 219,
		// Token: 0x0400205F RID: 8287
		OemPipe = 220,
		// Token: 0x04002060 RID: 8288
		Oem5 = 220,
		// Token: 0x04002061 RID: 8289
		OemCloseBrackets = 221,
		// Token: 0x04002062 RID: 8290
		Oem6 = 221,
		// Token: 0x04002063 RID: 8291
		OemQuotes = 222,
		// Token: 0x04002064 RID: 8292
		Oem7 = 222,
		// Token: 0x04002065 RID: 8293
		Oem8 = 223,
		// Token: 0x04002066 RID: 8294
		OemBackslash = 226,
		// Token: 0x04002067 RID: 8295
		Oem102 = 226,
		// Token: 0x04002068 RID: 8296
		ProcessKey = 229,
		// Token: 0x04002069 RID: 8297
		Packet = 231,
		// Token: 0x0400206A RID: 8298
		Attn = 246,
		// Token: 0x0400206B RID: 8299
		Crsel = 247,
		// Token: 0x0400206C RID: 8300
		Exsel = 248,
		// Token: 0x0400206D RID: 8301
		EraseEof = 249,
		// Token: 0x0400206E RID: 8302
		Play = 250,
		// Token: 0x0400206F RID: 8303
		Zoom = 251,
		// Token: 0x04002070 RID: 8304
		NoName = 252,
		// Token: 0x04002071 RID: 8305
		Pa1 = 253,
		// Token: 0x04002072 RID: 8306
		OemClear = 254,
		// Token: 0x04002073 RID: 8307
		Shift = 65536,
		// Token: 0x04002074 RID: 8308
		Control = 131072,
		// Token: 0x04002075 RID: 8309
		Alt = 262144
	}
}

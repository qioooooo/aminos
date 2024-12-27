using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x0200000A RID: 10
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct CultureTableHeader
	{
		// Token: 0x040000E8 RID: 232
		internal uint version;

		// Token: 0x040000E9 RID: 233
		internal ushort hash0;

		// Token: 0x040000EA RID: 234
		internal ushort hash1;

		// Token: 0x040000EB RID: 235
		internal ushort hash2;

		// Token: 0x040000EC RID: 236
		internal ushort hash3;

		// Token: 0x040000ED RID: 237
		internal ushort hash4;

		// Token: 0x040000EE RID: 238
		internal ushort hash5;

		// Token: 0x040000EF RID: 239
		internal ushort hash6;

		// Token: 0x040000F0 RID: 240
		internal ushort hash7;

		// Token: 0x040000F1 RID: 241
		internal ushort headerSize;

		// Token: 0x040000F2 RID: 242
		internal ushort numLcidItems;

		// Token: 0x040000F3 RID: 243
		internal ushort numCultureItems;

		// Token: 0x040000F4 RID: 244
		internal ushort sizeCultureItem;

		// Token: 0x040000F5 RID: 245
		internal uint offsetToCultureItemData;

		// Token: 0x040000F6 RID: 246
		internal ushort numCultureNames;

		// Token: 0x040000F7 RID: 247
		internal ushort numRegionNames;

		// Token: 0x040000F8 RID: 248
		internal uint cultureIDTableOffset;

		// Token: 0x040000F9 RID: 249
		internal uint cultureNameTableOffset;

		// Token: 0x040000FA RID: 250
		internal uint regionNameTableOffset;

		// Token: 0x040000FB RID: 251
		internal ushort numCalendarItems;

		// Token: 0x040000FC RID: 252
		internal ushort sizeCalendarItem;

		// Token: 0x040000FD RID: 253
		internal uint offsetToCalendarItemData;

		// Token: 0x040000FE RID: 254
		internal uint offsetToDataPool;

		// Token: 0x040000FF RID: 255
		internal ushort Unused_numIetfNames;

		// Token: 0x04000100 RID: 256
		internal ushort Unused_Padding;

		// Token: 0x04000101 RID: 257
		internal uint Unused_ietfNameTableOffset;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003C5 RID: 965
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct CultureTableHeader
	{
		// Token: 0x040012DA RID: 4826
		internal uint version;

		// Token: 0x040012DB RID: 4827
		internal ushort hash0;

		// Token: 0x040012DC RID: 4828
		internal ushort hash1;

		// Token: 0x040012DD RID: 4829
		internal ushort hash2;

		// Token: 0x040012DE RID: 4830
		internal ushort hash3;

		// Token: 0x040012DF RID: 4831
		internal ushort hash4;

		// Token: 0x040012E0 RID: 4832
		internal ushort hash5;

		// Token: 0x040012E1 RID: 4833
		internal ushort hash6;

		// Token: 0x040012E2 RID: 4834
		internal ushort hash7;

		// Token: 0x040012E3 RID: 4835
		internal ushort headerSize;

		// Token: 0x040012E4 RID: 4836
		internal ushort numLcidItems;

		// Token: 0x040012E5 RID: 4837
		internal ushort numCultureItems;

		// Token: 0x040012E6 RID: 4838
		internal ushort sizeCultureItem;

		// Token: 0x040012E7 RID: 4839
		internal uint offsetToCultureItemData;

		// Token: 0x040012E8 RID: 4840
		internal ushort numCultureNames;

		// Token: 0x040012E9 RID: 4841
		internal ushort numRegionNames;

		// Token: 0x040012EA RID: 4842
		internal uint cultureIDTableOffset;

		// Token: 0x040012EB RID: 4843
		internal uint cultureNameTableOffset;

		// Token: 0x040012EC RID: 4844
		internal uint regionNameTableOffset;

		// Token: 0x040012ED RID: 4845
		internal ushort numCalendarItems;

		// Token: 0x040012EE RID: 4846
		internal ushort sizeCalendarItem;

		// Token: 0x040012EF RID: 4847
		internal uint offsetToCalendarItemData;

		// Token: 0x040012F0 RID: 4848
		internal uint offsetToDataPool;

		// Token: 0x040012F1 RID: 4849
		internal ushort Unused_numIetfNames;

		// Token: 0x040012F2 RID: 4850
		internal ushort Unused_Padding;

		// Token: 0x040012F3 RID: 4851
		internal uint Unused_ietfNameTableOffset;
	}
}

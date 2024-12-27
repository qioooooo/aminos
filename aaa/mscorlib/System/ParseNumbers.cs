using System;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x020000E7 RID: 231
	internal static class ParseNumbers
	{
		// Token: 0x06000C7D RID: 3197 RVA: 0x00025832 File Offset: 0x00024832
		public static long StringToLong(string s, int radix, int flags)
		{
			return ParseNumbers.StringToLong(s, radix, flags, null);
		}

		// Token: 0x06000C7E RID: 3198
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern long StringToLong(string s, int radix, int flags, int* currPos);

		// Token: 0x06000C7F RID: 3199 RVA: 0x00025840 File Offset: 0x00024840
		public unsafe static long StringToLong(string s, int radix, int flags, ref int currPos)
		{
			fixed (int* ptr = &currPos)
			{
				return ParseNumbers.StringToLong(s, radix, flags, ptr);
			}
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0002585D File Offset: 0x0002485D
		public static int StringToInt(string s, int radix, int flags)
		{
			return ParseNumbers.StringToInt(s, radix, flags, null);
		}

		// Token: 0x06000C81 RID: 3201
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern int StringToInt(string s, int radix, int flags, int* currPos);

		// Token: 0x06000C82 RID: 3202 RVA: 0x0002586C File Offset: 0x0002486C
		public unsafe static int StringToInt(string s, int radix, int flags, ref int currPos)
		{
			fixed (int* ptr = &currPos)
			{
				return ParseNumbers.StringToInt(s, radix, flags, ptr);
			}
		}

		// Token: 0x06000C83 RID: 3203
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string IntToString(int l, int radix, int width, char paddingChar, int flags);

		// Token: 0x06000C84 RID: 3204
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string LongToString(long l, int radix, int width, char paddingChar, int flags);

		// Token: 0x04000431 RID: 1073
		internal const int PrintAsI1 = 64;

		// Token: 0x04000432 RID: 1074
		internal const int PrintAsI2 = 128;

		// Token: 0x04000433 RID: 1075
		internal const int PrintAsI4 = 256;

		// Token: 0x04000434 RID: 1076
		internal const int TreatAsUnsigned = 512;

		// Token: 0x04000435 RID: 1077
		internal const int TreatAsI1 = 1024;

		// Token: 0x04000436 RID: 1078
		internal const int TreatAsI2 = 2048;

		// Token: 0x04000437 RID: 1079
		internal const int IsTight = 4096;

		// Token: 0x04000438 RID: 1080
		internal const int NoSpace = 8192;
	}
}

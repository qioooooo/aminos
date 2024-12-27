using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.Win32
{
	// Token: 0x0200045B RID: 1115
	internal sealed class OAVariantLib
	{
		// Token: 0x06002CF9 RID: 11513 RVA: 0x00096C16 File Offset: 0x00095C16
		private OAVariantLib()
		{
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x00096C20 File Offset: 0x00095C20
		internal static Variant ChangeType(Variant source, Type targetClass, short options, CultureInfo culture)
		{
			if (targetClass == null)
			{
				throw new ArgumentNullException("targetClass");
			}
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			Variant variant = default(Variant);
			OAVariantLib.ChangeTypeEx(ref variant, source, culture.LCID, OAVariantLib.GetCVTypeFromClass(targetClass), options);
			return variant;
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x00096C68 File Offset: 0x00095C68
		private static int GetCVTypeFromClass(Type ctype)
		{
			int num = -1;
			for (int i = 0; i < OAVariantLib.ClassTypes.Length; i++)
			{
				if (ctype.Equals(OAVariantLib.ClassTypes[i]))
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				num = 18;
			}
			return num;
		}

		// Token: 0x06002CFC RID: 11516
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ChangeTypeEx(ref Variant result, Variant source, int lcid, int cvType, short flags);

		// Token: 0x0400170D RID: 5901
		public const int NoValueProp = 1;

		// Token: 0x0400170E RID: 5902
		public const int AlphaBool = 2;

		// Token: 0x0400170F RID: 5903
		public const int NoUserOverride = 4;

		// Token: 0x04001710 RID: 5904
		public const int CalendarHijri = 8;

		// Token: 0x04001711 RID: 5905
		public const int LocalBool = 16;

		// Token: 0x04001712 RID: 5906
		private const int CV_OBJECT = 18;

		// Token: 0x04001713 RID: 5907
		internal static readonly Type[] ClassTypes = new Type[]
		{
			typeof(Empty),
			typeof(void),
			typeof(bool),
			typeof(char),
			typeof(sbyte),
			typeof(byte),
			typeof(short),
			typeof(ushort),
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(float),
			typeof(double),
			typeof(string),
			typeof(void),
			typeof(DateTime),
			typeof(TimeSpan),
			typeof(object),
			typeof(decimal),
			null,
			typeof(Missing),
			typeof(DBNull)
		};
	}
}

using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Specialized
{
	// Token: 0x02000041 RID: 65
	internal class BackCompatibleStringComparer : IEqualityComparer
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x00007022 File Offset: 0x00006022
		internal BackCompatibleStringComparer()
		{
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000702C File Offset: 0x0000602C
		public unsafe static int GetHashCode(string obj)
		{
			IntPtr intPtr2;
			IntPtr intPtr = (intPtr2 = obj);
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			int num = 5381;
			char* ptr2 = ptr;
			int num2;
			while ((num2 = (int)(*ptr2)) != 0)
			{
				num = ((num << 5) + num) ^ num2;
				ptr2++;
			}
			return num;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000706D File Offset: 0x0000606D
		bool IEqualityComparer.Equals(object a, object b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00007078 File Offset: 0x00006078
		public virtual int GetHashCode(object o)
		{
			string text = o as string;
			if (text == null)
			{
				return o.GetHashCode();
			}
			return BackCompatibleStringComparer.GetHashCode(text);
		}

		// Token: 0x04000B6D RID: 2925
		internal static IEqualityComparer Default = new BackCompatibleStringComparer();
	}
}

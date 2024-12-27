using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Specialized
{
	// Token: 0x020007A3 RID: 1955
	internal class BackCompatibleStringComparer : IEqualityComparer
	{
		// Token: 0x06003C1C RID: 15388 RVA: 0x00101027 File Offset: 0x00100027
		internal BackCompatibleStringComparer()
		{
		}

		// Token: 0x06003C1D RID: 15389 RVA: 0x00101030 File Offset: 0x00100030
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

		// Token: 0x06003C1E RID: 15390 RVA: 0x00101071 File Offset: 0x00100071
		bool IEqualityComparer.Equals(object a, object b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x0010107C File Offset: 0x0010007C
		public virtual int GetHashCode(object o)
		{
			string text = o as string;
			if (text == null)
			{
				return o.GetHashCode();
			}
			return BackCompatibleStringComparer.GetHashCode(text);
		}

		// Token: 0x04003509 RID: 13577
		internal static IEqualityComparer Default = new BackCompatibleStringComparer();
	}
}

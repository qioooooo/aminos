using System;

namespace System
{
	// Token: 0x02000388 RID: 904
	internal struct DTSubString
	{
		// Token: 0x17000670 RID: 1648
		internal char this[int relativeIndex]
		{
			get
			{
				return this.s[this.index + relativeIndex];
			}
		}

		// Token: 0x04000FB2 RID: 4018
		internal string s;

		// Token: 0x04000FB3 RID: 4019
		internal int index;

		// Token: 0x04000FB4 RID: 4020
		internal int length;

		// Token: 0x04000FB5 RID: 4021
		internal DTSubStringType type;

		// Token: 0x04000FB6 RID: 4022
		internal int value;
	}
}

using System;

namespace System.Reflection
{
	// Token: 0x0200030B RID: 779
	[Serializable]
	internal struct ConstArray
	{
		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001E56 RID: 7766 RVA: 0x0004D227 File Offset: 0x0004C227
		public IntPtr Signature
		{
			get
			{
				return this.m_constArray;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001E57 RID: 7767 RVA: 0x0004D22F File Offset: 0x0004C22F
		public int Length
		{
			get
			{
				return this.m_length;
			}
		}

		// Token: 0x17000507 RID: 1287
		public unsafe byte this[int index]
		{
			get
			{
				if (index < 0 || index >= this.m_length)
				{
					throw new IndexOutOfRangeException();
				}
				return ((byte*)this.m_constArray.ToPointer())[index];
			}
		}

		// Token: 0x04000CB9 RID: 3257
		internal int m_length;

		// Token: 0x04000CBA RID: 3258
		internal IntPtr m_constArray;
	}
}

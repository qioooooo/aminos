using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008D1 RID: 2257
	internal struct Triple<T1, T2, T3>
	{
		// Token: 0x06005277 RID: 21111 RVA: 0x0012A2DC File Offset: 0x001292DC
		internal Triple(T1 first, T2 second, T3 third)
		{
			this._first = first;
			this._second = second;
			this._third = third;
		}

		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x06005278 RID: 21112 RVA: 0x0012A2F3 File Offset: 0x001292F3
		public T1 Item1
		{
			get
			{
				return this._first;
			}
		}

		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x06005279 RID: 21113 RVA: 0x0012A2FB File Offset: 0x001292FB
		public T2 Item2
		{
			get
			{
				return this._second;
			}
		}

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x0600527A RID: 21114 RVA: 0x0012A303 File Offset: 0x00129303
		public T3 Item3
		{
			get
			{
				return this._third;
			}
		}

		// Token: 0x04002A5A RID: 10842
		private readonly T1 _first;

		// Token: 0x04002A5B RID: 10843
		private readonly T2 _second;

		// Token: 0x04002A5C RID: 10844
		private readonly T3 _third;
	}
}

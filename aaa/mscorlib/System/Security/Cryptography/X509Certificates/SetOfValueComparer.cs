using System;
using System.Collections.Generic;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008D3 RID: 2259
	internal sealed class SetOfValueComparer : IComparer<ReadOnlyMemory<byte>>
	{
		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x06005283 RID: 21123 RVA: 0x0012A7AE File Offset: 0x001297AE
		internal static SetOfValueComparer Instance
		{
			get
			{
				return SetOfValueComparer._instance;
			}
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x0012A7B5 File Offset: 0x001297B5
		public int Compare(ReadOnlyMemory<byte> x, ReadOnlyMemory<byte> y)
		{
			return SetOfValueComparer.Compare(x.Span, y.Span);
		}

		// Token: 0x06005285 RID: 21125 RVA: 0x0012A7CC File Offset: 0x001297CC
		internal static int Compare(ReadOnlySpan<byte> x, ReadOnlySpan<byte> y)
		{
			int num = Math.Min(x.Length, y.Length);
			for (int i = 0; i < num; i++)
			{
				int num2 = (int)x[i];
				byte b = y[i];
				int num3 = num2 - (int)b;
				if (num3 != 0)
				{
					return num3;
				}
			}
			return x.Length - y.Length;
		}

		// Token: 0x04002A61 RID: 10849
		private static SetOfValueComparer _instance = new SetOfValueComparer();
	}
}

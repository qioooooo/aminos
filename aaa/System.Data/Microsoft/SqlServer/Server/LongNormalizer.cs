using System;
using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000292 RID: 658
	internal sealed class LongNormalizer : Normalizer
	{
		// Token: 0x06002255 RID: 8789 RVA: 0x0026D438 File Offset: 0x0026C838
		internal override void Normalize(FieldInfo fi, object obj, Stream s)
		{
			byte[] bytes = BitConverter.GetBytes((long)base.GetValue(fi, obj));
			if (!this.m_skipNormalize)
			{
				Array.Reverse(bytes);
				byte[] array = bytes;
				int num = 0;
				array[num] ^= 128;
			}
			s.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x0026D48C File Offset: 0x0026C88C
		internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
		{
			byte[] array = new byte[8];
			s.Read(array, 0, array.Length);
			if (!this.m_skipNormalize)
			{
				byte[] array2 = array;
				int num = 0;
				array2[num] ^= 128;
				Array.Reverse(array);
			}
			base.SetValue(fi, recvr, BitConverter.ToInt64(array, 0));
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x0026D4E8 File Offset: 0x0026C8E8
		internal override int Size
		{
			get
			{
				return 8;
			}
		}
	}
}

using System;
using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200028E RID: 654
	internal sealed class ShortNormalizer : Normalizer
	{
		// Token: 0x06002245 RID: 8773 RVA: 0x0026D148 File Offset: 0x0026C548
		internal override void Normalize(FieldInfo fi, object obj, Stream s)
		{
			byte[] bytes = BitConverter.GetBytes((short)base.GetValue(fi, obj));
			if (!this.m_skipNormalize)
			{
				Array.Reverse(bytes);
				byte[] array = bytes;
				int num = 0;
				array[num] ^= 128;
			}
			s.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x0026D19C File Offset: 0x0026C59C
		internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
		{
			byte[] array = new byte[2];
			s.Read(array, 0, array.Length);
			if (!this.m_skipNormalize)
			{
				byte[] array2 = array;
				int num = 0;
				array2[num] ^= 128;
				Array.Reverse(array);
			}
			base.SetValue(fi, recvr, BitConverter.ToInt16(array, 0));
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06002247 RID: 8775 RVA: 0x0026D1F8 File Offset: 0x0026C5F8
		internal override int Size
		{
			get
			{
				return 2;
			}
		}
	}
}

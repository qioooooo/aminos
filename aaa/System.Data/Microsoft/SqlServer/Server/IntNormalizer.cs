using System;
using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000290 RID: 656
	internal sealed class IntNormalizer : Normalizer
	{
		// Token: 0x0600224D RID: 8781 RVA: 0x0026D2C0 File Offset: 0x0026C6C0
		internal override void Normalize(FieldInfo fi, object obj, Stream s)
		{
			byte[] bytes = BitConverter.GetBytes((int)base.GetValue(fi, obj));
			if (!this.m_skipNormalize)
			{
				Array.Reverse(bytes);
				byte[] array = bytes;
				int num = 0;
				array[num] ^= 128;
			}
			s.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x0026D314 File Offset: 0x0026C714
		internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
		{
			byte[] array = new byte[4];
			s.Read(array, 0, array.Length);
			if (!this.m_skipNormalize)
			{
				byte[] array2 = array;
				int num = 0;
				array2[num] ^= 128;
				Array.Reverse(array);
			}
			base.SetValue(fi, recvr, BitConverter.ToInt32(array, 0));
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x0600224F RID: 8783 RVA: 0x0026D370 File Offset: 0x0026C770
		internal override int Size
		{
			get
			{
				return 4;
			}
		}
	}
}

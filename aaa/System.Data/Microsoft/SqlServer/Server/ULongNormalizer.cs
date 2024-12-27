using System;
using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000293 RID: 659
	internal sealed class ULongNormalizer : Normalizer
	{
		// Token: 0x06002259 RID: 8793 RVA: 0x0026D50C File Offset: 0x0026C90C
		internal override void Normalize(FieldInfo fi, object obj, Stream s)
		{
			byte[] bytes = BitConverter.GetBytes((ulong)base.GetValue(fi, obj));
			if (!this.m_skipNormalize)
			{
				Array.Reverse(bytes);
			}
			s.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x0026D548 File Offset: 0x0026C948
		internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
		{
			byte[] array = new byte[8];
			s.Read(array, 0, array.Length);
			if (!this.m_skipNormalize)
			{
				Array.Reverse(array);
			}
			base.SetValue(fi, recvr, BitConverter.ToUInt64(array, 0));
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x0600225B RID: 8795 RVA: 0x0026D58C File Offset: 0x0026C98C
		internal override int Size
		{
			get
			{
				return 8;
			}
		}
	}
}

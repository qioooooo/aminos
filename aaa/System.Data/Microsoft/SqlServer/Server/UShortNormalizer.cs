using System;
using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200028F RID: 655
	internal sealed class UShortNormalizer : Normalizer
	{
		// Token: 0x06002249 RID: 8777 RVA: 0x0026D21C File Offset: 0x0026C61C
		internal override void Normalize(FieldInfo fi, object obj, Stream s)
		{
			byte[] bytes = BitConverter.GetBytes((ushort)base.GetValue(fi, obj));
			if (!this.m_skipNormalize)
			{
				Array.Reverse(bytes);
			}
			s.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x0026D258 File Offset: 0x0026C658
		internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
		{
			byte[] array = new byte[2];
			s.Read(array, 0, array.Length);
			if (!this.m_skipNormalize)
			{
				Array.Reverse(array);
			}
			base.SetValue(fi, recvr, BitConverter.ToUInt16(array, 0));
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x0026D29C File Offset: 0x0026C69C
		internal override int Size
		{
			get
			{
				return 2;
			}
		}
	}
}

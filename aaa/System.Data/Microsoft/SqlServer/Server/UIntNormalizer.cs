using System;
using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000291 RID: 657
	internal sealed class UIntNormalizer : Normalizer
	{
		// Token: 0x06002251 RID: 8785 RVA: 0x0026D394 File Offset: 0x0026C794
		internal override void Normalize(FieldInfo fi, object obj, Stream s)
		{
			byte[] bytes = BitConverter.GetBytes((uint)base.GetValue(fi, obj));
			if (!this.m_skipNormalize)
			{
				Array.Reverse(bytes);
			}
			s.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x0026D3D0 File Offset: 0x0026C7D0
		internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
		{
			byte[] array = new byte[4];
			s.Read(array, 0, array.Length);
			if (!this.m_skipNormalize)
			{
				Array.Reverse(array);
			}
			base.SetValue(fi, recvr, BitConverter.ToUInt32(array, 0));
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06002253 RID: 8787 RVA: 0x0026D414 File Offset: 0x0026C814
		internal override int Size
		{
			get
			{
				return 4;
			}
		}
	}
}

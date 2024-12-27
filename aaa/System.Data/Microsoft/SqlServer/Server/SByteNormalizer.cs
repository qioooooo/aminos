using System;
using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200028C RID: 652
	internal sealed class SByteNormalizer : Normalizer
	{
		// Token: 0x0600223D RID: 8765 RVA: 0x0026D048 File Offset: 0x0026C448
		internal override void Normalize(FieldInfo fi, object obj, Stream s)
		{
			sbyte b = (sbyte)base.GetValue(fi, obj);
			byte b2 = (byte)b;
			if (!this.m_skipNormalize)
			{
				b2 ^= 128;
			}
			s.WriteByte(b2);
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x0026D080 File Offset: 0x0026C480
		internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
		{
			byte b = (byte)s.ReadByte();
			if (!this.m_skipNormalize)
			{
				b ^= 128;
			}
			sbyte b2 = (sbyte)b;
			base.SetValue(fi, recvr, b2);
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x0600223F RID: 8767 RVA: 0x0026D0B8 File Offset: 0x0026C4B8
		internal override int Size
		{
			get
			{
				return 1;
			}
		}
	}
}

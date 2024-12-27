using System;
using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200028D RID: 653
	internal sealed class ByteNormalizer : Normalizer
	{
		// Token: 0x06002241 RID: 8769 RVA: 0x0026D0DC File Offset: 0x0026C4DC
		internal override void Normalize(FieldInfo fi, object obj, Stream s)
		{
			byte b = (byte)base.GetValue(fi, obj);
			s.WriteByte(b);
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x0026D100 File Offset: 0x0026C500
		internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
		{
			byte b = (byte)s.ReadByte();
			base.SetValue(fi, recvr, b);
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06002243 RID: 8771 RVA: 0x0026D124 File Offset: 0x0026C524
		internal override int Size
		{
			get
			{
				return 1;
			}
		}
	}
}

using System;
using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200028B RID: 651
	internal sealed class BooleanNormalizer : Normalizer
	{
		// Token: 0x06002239 RID: 8761 RVA: 0x0026CFD0 File Offset: 0x0026C3D0
		internal override void Normalize(FieldInfo fi, object obj, Stream s)
		{
			s.WriteByte(((bool)base.GetValue(fi, obj)) ? 1 : 0);
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x0026CFFC File Offset: 0x0026C3FC
		internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
		{
			byte b = (byte)s.ReadByte();
			base.SetValue(fi, recvr, b == 1);
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x0600223B RID: 8763 RVA: 0x0026D024 File Offset: 0x0026C424
		internal override int Size
		{
			get
			{
				return 1;
			}
		}
	}
}

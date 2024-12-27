using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007CA RID: 1994
	internal sealed class BinaryCrossAppDomainMap : IStreamable
	{
		// Token: 0x0600470F RID: 18191 RVA: 0x000F46A0 File Offset: 0x000F36A0
		internal BinaryCrossAppDomainMap()
		{
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x000F46A8 File Offset: 0x000F36A8
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(18);
			sout.WriteInt32(this.crossAppDomainArrayIndex);
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x000F46BE File Offset: 0x000F36BE
		public void Read(__BinaryParser input)
		{
			this.crossAppDomainArrayIndex = input.ReadInt32();
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x000F46CC File Offset: 0x000F36CC
		public void Dump()
		{
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x000F46CE File Offset: 0x000F36CE
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x040023CA RID: 9162
		internal int crossAppDomainArrayIndex;
	}
}

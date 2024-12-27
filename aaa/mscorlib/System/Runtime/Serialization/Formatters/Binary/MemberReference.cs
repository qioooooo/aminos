using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D0 RID: 2000
	internal sealed class MemberReference : IStreamable
	{
		// Token: 0x06004732 RID: 18226 RVA: 0x000F4F8C File Offset: 0x000F3F8C
		internal MemberReference()
		{
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x000F4F94 File Offset: 0x000F3F94
		internal void Set(int idRef)
		{
			this.idRef = idRef;
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x000F4F9D File Offset: 0x000F3F9D
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(9);
			sout.WriteInt32(this.idRef);
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x000F4FB3 File Offset: 0x000F3FB3
		public void Read(__BinaryParser input)
		{
			this.idRef = input.ReadInt32();
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x000F4FC1 File Offset: 0x000F3FC1
		public void Dump()
		{
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x000F4FC3 File Offset: 0x000F3FC3
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x040023E7 RID: 9191
		internal int idRef;
	}
}

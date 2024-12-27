using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007C9 RID: 1993
	internal sealed class BinaryCrossAppDomainString : IStreamable
	{
		// Token: 0x0600470A RID: 18186 RVA: 0x000F464D File Offset: 0x000F364D
		internal BinaryCrossAppDomainString()
		{
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x000F4655 File Offset: 0x000F3655
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(19);
			sout.WriteInt32(this.objectId);
			sout.WriteInt32(this.value);
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x000F4677 File Offset: 0x000F3677
		public void Read(__BinaryParser input)
		{
			this.objectId = input.ReadInt32();
			this.value = input.ReadInt32();
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x000F4691 File Offset: 0x000F3691
		public void Dump()
		{
		}

		// Token: 0x0600470E RID: 18190 RVA: 0x000F4693 File Offset: 0x000F3693
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x040023C8 RID: 9160
		internal int objectId;

		// Token: 0x040023C9 RID: 9161
		internal int value;
	}
}

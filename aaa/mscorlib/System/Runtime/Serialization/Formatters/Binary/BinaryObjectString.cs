using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007C8 RID: 1992
	internal sealed class BinaryObjectString : IStreamable
	{
		// Token: 0x06004704 RID: 18180 RVA: 0x000F45EB File Offset: 0x000F35EB
		internal BinaryObjectString()
		{
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x000F45F3 File Offset: 0x000F35F3
		internal void Set(int objectId, string value)
		{
			this.objectId = objectId;
			this.value = value;
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x000F4603 File Offset: 0x000F3603
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(6);
			sout.WriteInt32(this.objectId);
			sout.WriteString(this.value);
		}

		// Token: 0x06004707 RID: 18183 RVA: 0x000F4624 File Offset: 0x000F3624
		public void Read(__BinaryParser input)
		{
			this.objectId = input.ReadInt32();
			this.value = input.ReadString();
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x000F463E File Offset: 0x000F363E
		public void Dump()
		{
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x000F4640 File Offset: 0x000F3640
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x040023C6 RID: 9158
		internal int objectId;

		// Token: 0x040023C7 RID: 9159
		internal string value;
	}
}

using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007C5 RID: 1989
	internal sealed class BinaryObject : IStreamable
	{
		// Token: 0x060046EF RID: 18159 RVA: 0x000F3963 File Offset: 0x000F2963
		internal BinaryObject()
		{
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x000F396B File Offset: 0x000F296B
		internal void Set(int objectId, int mapId)
		{
			this.objectId = objectId;
			this.mapId = mapId;
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x000F397B File Offset: 0x000F297B
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(1);
			sout.WriteInt32(this.objectId);
			sout.WriteInt32(this.mapId);
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x000F399C File Offset: 0x000F299C
		public void Read(__BinaryParser input)
		{
			this.objectId = input.ReadInt32();
			this.mapId = input.ReadInt32();
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x000F39B6 File Offset: 0x000F29B6
		public void Dump()
		{
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x000F39B8 File Offset: 0x000F29B8
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x040023AB RID: 9131
		internal int objectId;

		// Token: 0x040023AC RID: 9132
		internal int mapId;
	}
}

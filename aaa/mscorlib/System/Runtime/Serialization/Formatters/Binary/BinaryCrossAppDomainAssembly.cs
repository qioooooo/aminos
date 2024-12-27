using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007C4 RID: 1988
	internal sealed class BinaryCrossAppDomainAssembly : IStreamable
	{
		// Token: 0x060046EA RID: 18154 RVA: 0x000F3910 File Offset: 0x000F2910
		internal BinaryCrossAppDomainAssembly()
		{
		}

		// Token: 0x060046EB RID: 18155 RVA: 0x000F3918 File Offset: 0x000F2918
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(20);
			sout.WriteInt32(this.assemId);
			sout.WriteInt32(this.assemblyIndex);
		}

		// Token: 0x060046EC RID: 18156 RVA: 0x000F393A File Offset: 0x000F293A
		public void Read(__BinaryParser input)
		{
			this.assemId = input.ReadInt32();
			this.assemblyIndex = input.ReadInt32();
		}

		// Token: 0x060046ED RID: 18157 RVA: 0x000F3954 File Offset: 0x000F2954
		public void Dump()
		{
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x000F3956 File Offset: 0x000F2956
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x040023A9 RID: 9129
		internal int assemId;

		// Token: 0x040023AA RID: 9130
		internal int assemblyIndex;
	}
}

using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007C3 RID: 1987
	internal sealed class BinaryAssembly : IStreamable
	{
		// Token: 0x060046E4 RID: 18148 RVA: 0x000F38AD File Offset: 0x000F28AD
		internal BinaryAssembly()
		{
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x000F38B5 File Offset: 0x000F28B5
		internal void Set(int assemId, string assemblyString)
		{
			this.assemId = assemId;
			this.assemblyString = assemblyString;
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x000F38C5 File Offset: 0x000F28C5
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(12);
			sout.WriteInt32(this.assemId);
			sout.WriteString(this.assemblyString);
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x000F38E7 File Offset: 0x000F28E7
		public void Read(__BinaryParser input)
		{
			this.assemId = input.ReadInt32();
			this.assemblyString = input.ReadString();
		}

		// Token: 0x060046E8 RID: 18152 RVA: 0x000F3901 File Offset: 0x000F2901
		public void Dump()
		{
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x000F3903 File Offset: 0x000F2903
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x040023A7 RID: 9127
		internal int assemId;

		// Token: 0x040023A8 RID: 9128
		internal string assemblyString;
	}
}

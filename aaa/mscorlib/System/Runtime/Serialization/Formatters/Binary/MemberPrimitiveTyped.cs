using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007CB RID: 1995
	internal sealed class MemberPrimitiveTyped : IStreamable
	{
		// Token: 0x06004714 RID: 18196 RVA: 0x000F46DB File Offset: 0x000F36DB
		internal MemberPrimitiveTyped()
		{
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x000F46E3 File Offset: 0x000F36E3
		internal void Set(InternalPrimitiveTypeE primitiveTypeEnum, object value)
		{
			this.primitiveTypeEnum = primitiveTypeEnum;
			this.value = value;
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x000F46F3 File Offset: 0x000F36F3
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(8);
			sout.WriteByte((byte)this.primitiveTypeEnum);
			sout.WriteValue(this.primitiveTypeEnum, this.value);
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x000F471B File Offset: 0x000F371B
		public void Read(__BinaryParser input)
		{
			this.primitiveTypeEnum = (InternalPrimitiveTypeE)input.ReadByte();
			this.value = input.ReadValue(this.primitiveTypeEnum);
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x000F473B File Offset: 0x000F373B
		public void Dump()
		{
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x000F473D File Offset: 0x000F373D
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x040023CB RID: 9163
		internal InternalPrimitiveTypeE primitiveTypeEnum;

		// Token: 0x040023CC RID: 9164
		internal object value;
	}
}

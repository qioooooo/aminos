using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007CF RID: 1999
	internal sealed class MemberPrimitiveUnTyped : IStreamable
	{
		// Token: 0x0600472B RID: 18219 RVA: 0x000F4F27 File Offset: 0x000F3F27
		internal MemberPrimitiveUnTyped()
		{
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x000F4F2F File Offset: 0x000F3F2F
		internal void Set(InternalPrimitiveTypeE typeInformation, object value)
		{
			this.typeInformation = typeInformation;
			this.value = value;
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x000F4F3F File Offset: 0x000F3F3F
		internal void Set(InternalPrimitiveTypeE typeInformation)
		{
			this.typeInformation = typeInformation;
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x000F4F48 File Offset: 0x000F3F48
		public void Write(__BinaryWriter sout)
		{
			sout.WriteValue(this.typeInformation, this.value);
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x000F4F5C File Offset: 0x000F3F5C
		public void Read(__BinaryParser input)
		{
			this.value = input.ReadValue(this.typeInformation);
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x000F4F70 File Offset: 0x000F3F70
		public void Dump()
		{
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x000F4F72 File Offset: 0x000F3F72
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			if (BCLDebug.CheckEnabled("BINARY"))
			{
				Converter.ToComType(this.typeInformation);
			}
		}

		// Token: 0x040023E5 RID: 9189
		internal InternalPrimitiveTypeE typeInformation;

		// Token: 0x040023E6 RID: 9190
		internal object value;
	}
}

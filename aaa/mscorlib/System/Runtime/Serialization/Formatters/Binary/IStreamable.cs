using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007C0 RID: 1984
	internal interface IStreamable
	{
		// Token: 0x060046D8 RID: 18136
		void Read(__BinaryParser input);

		// Token: 0x060046D9 RID: 18137
		void Write(__BinaryWriter sout);
	}
}

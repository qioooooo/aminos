using System;
using System.Runtime.Serialization;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000031 RID: 49
	[Serializable]
	public class BerConversionException : DirectoryException
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00005375 File Offset: 0x00004375
		protected BerConversionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000537F File Offset: 0x0000437F
		public BerConversionException()
			: base(Res.GetString("BerConversionError"))
		{
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005391 File Offset: 0x00004391
		public BerConversionException(string message)
			: base(message)
		{
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000539A File Offset: 0x0000439A
		public BerConversionException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}

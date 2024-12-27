using System;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.MetadataServices
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	public class SUDSGeneratorException : Exception
	{
		// Token: 0x0600038D RID: 909 RVA: 0x00010DD0 File Offset: 0x0000FDD0
		internal SUDSGeneratorException(string msg)
			: base(msg)
		{
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00010DD9 File Offset: 0x0000FDD9
		protected SUDSGeneratorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}

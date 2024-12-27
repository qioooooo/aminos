using System;
using System.Runtime.Serialization;

namespace <CrtImplementationDetails>
{
	// Token: 0x020000AF RID: 175
	[Serializable]
	internal class OpenMPWithMultipleAppdomainsException : Exception
	{
		// Token: 0x06000120 RID: 288 RVA: 0x00006568 File Offset: 0x00005968
		protected OpenMPWithMultipleAppdomainsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006554 File Offset: 0x00005954
		public OpenMPWithMultipleAppdomainsException()
		{
		}
	}
}

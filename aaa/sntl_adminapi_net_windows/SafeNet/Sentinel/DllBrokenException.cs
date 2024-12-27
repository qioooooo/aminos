using System;
using System.Runtime.Serialization;

namespace SafeNet.Sentinel
{
	// Token: 0x02000002 RID: 2
	public class DllBrokenException : Exception
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		public DllBrokenException()
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020DB File Offset: 0x000002DB
		public DllBrokenException(string message)
			: base(message)
		{
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020E7 File Offset: 0x000002E7
		public DllBrokenException(string message, Exception ex)
			: base(message, ex)
		{
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020F4 File Offset: 0x000002F4
		protected DllBrokenException(SerializationInfo serInfo, StreamingContext streamContext)
			: base(serInfo, streamContext)
		{
		}
	}
}

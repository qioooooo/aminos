using System;
using System.Runtime.Serialization;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200002F RID: 47
	[Serializable]
	public class DirectoryException : Exception
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x000052D0 File Offset: 0x000042D0
		protected DirectoryException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000052DA File Offset: 0x000042DA
		public DirectoryException(string message, Exception inner)
			: base(message, inner)
		{
			Utility.CheckOSVersion();
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000052E9 File Offset: 0x000042E9
		public DirectoryException(string message)
			: base(message)
		{
			Utility.CheckOSVersion();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000052F7 File Offset: 0x000042F7
		public DirectoryException()
		{
			Utility.CheckOSVersion();
		}
	}
}

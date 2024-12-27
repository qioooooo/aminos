using System;
using System.Runtime.Serialization;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000A2 RID: 162
	[Serializable]
	public class ActiveDirectoryObjectExistsException : Exception
	{
		// Token: 0x06000557 RID: 1367 RVA: 0x0001E7E4 File Offset: 0x0001D7E4
		public ActiveDirectoryObjectExistsException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001E7EE File Offset: 0x0001D7EE
		public ActiveDirectoryObjectExistsException(string message)
			: base(message)
		{
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001E7F7 File Offset: 0x0001D7F7
		public ActiveDirectoryObjectExistsException()
		{
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001E7FF File Offset: 0x0001D7FF
		protected ActiveDirectoryObjectExistsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}

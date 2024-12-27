using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000A0 RID: 160
	[Serializable]
	public class ActiveDirectoryOperationException : Exception, ISerializable
	{
		// Token: 0x06000545 RID: 1349 RVA: 0x0001E6BB File Offset: 0x0001D6BB
		public ActiveDirectoryOperationException(string message, Exception inner, int errorCode)
			: base(message, inner)
		{
			this.errorCode = errorCode;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001E6CC File Offset: 0x0001D6CC
		public ActiveDirectoryOperationException(string message, int errorCode)
			: base(message)
		{
			this.errorCode = errorCode;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001E6DC File Offset: 0x0001D6DC
		public ActiveDirectoryOperationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001E6E6 File Offset: 0x0001D6E6
		public ActiveDirectoryOperationException(string message)
			: base(message)
		{
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001E6EF File Offset: 0x0001D6EF
		public ActiveDirectoryOperationException()
			: base(Res.GetString("DSUnknownFailure"))
		{
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001E701 File Offset: 0x0001D701
		protected ActiveDirectoryOperationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x0001E70B File Offset: 0x0001D70B
		public int ErrorCode
		{
			get
			{
				return this.errorCode;
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001E713 File Offset: 0x0001D713
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x04000438 RID: 1080
		private int errorCode;
	}
}

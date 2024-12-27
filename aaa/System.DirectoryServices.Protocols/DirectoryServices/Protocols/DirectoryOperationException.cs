using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000030 RID: 48
	[Serializable]
	public class DirectoryOperationException : DirectoryException, ISerializable
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x00005304 File Offset: 0x00004304
		protected DirectoryOperationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000530E File Offset: 0x0000430E
		public DirectoryOperationException()
		{
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005316 File Offset: 0x00004316
		public DirectoryOperationException(string message)
			: base(message)
		{
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000531F File Offset: 0x0000431F
		public DirectoryOperationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005329 File Offset: 0x00004329
		public DirectoryOperationException(DirectoryResponse response)
			: base(Res.GetString("DefaultOperationsError"))
		{
			this.response = response;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005342 File Offset: 0x00004342
		public DirectoryOperationException(DirectoryResponse response, string message)
			: base(message)
		{
			this.response = response;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005352 File Offset: 0x00004352
		public DirectoryOperationException(DirectoryResponse response, string message, Exception inner)
			: base(message, inner)
		{
			this.response = response;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00005363 File Offset: 0x00004363
		public DirectoryResponse Response
		{
			get
			{
				return this.response;
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000536B File Offset: 0x0000436B
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x04000101 RID: 257
		internal DirectoryResponse response;
	}
}

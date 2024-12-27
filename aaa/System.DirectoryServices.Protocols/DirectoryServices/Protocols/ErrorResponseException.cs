using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000065 RID: 101
	[Serializable]
	public class ErrorResponseException : DirectoryException, ISerializable
	{
		// Token: 0x060001F8 RID: 504 RVA: 0x00008721 File Offset: 0x00007721
		protected ErrorResponseException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000872B File Offset: 0x0000772B
		public ErrorResponseException()
		{
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00008733 File Offset: 0x00007733
		public ErrorResponseException(string message)
			: base(message)
		{
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000873C File Offset: 0x0000773C
		public ErrorResponseException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00008746 File Offset: 0x00007746
		public ErrorResponseException(DsmlErrorResponse response)
			: this(response, Res.GetString("ErrorResponse"), null)
		{
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000875A File Offset: 0x0000775A
		public ErrorResponseException(DsmlErrorResponse response, string message)
			: this(response, message, null)
		{
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00008765 File Offset: 0x00007765
		public ErrorResponseException(DsmlErrorResponse response, string message, Exception inner)
			: base(message, inner)
		{
			this.errorResponse = response;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00008776 File Offset: 0x00007776
		public DsmlErrorResponse Response
		{
			get
			{
				return this.errorResponse;
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000877E File Offset: 0x0000777E
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x040001FA RID: 506
		private DsmlErrorResponse errorResponse;
	}
}

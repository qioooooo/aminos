using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000078 RID: 120
	[Serializable]
	public class LdapException : DirectoryException, ISerializable
	{
		// Token: 0x06000282 RID: 642 RVA: 0x0000D582 File Offset: 0x0000C582
		protected LdapException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000D597 File Offset: 0x0000C597
		public LdapException()
		{
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000D5AA File Offset: 0x0000C5AA
		public LdapException(string message)
			: base(message)
		{
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000D5BE File Offset: 0x0000C5BE
		public LdapException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000D5D3 File Offset: 0x0000C5D3
		public LdapException(int errorCode)
			: base(Res.GetString("DefaultLdapError"))
		{
			this.errorCode = errorCode;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000D5F7 File Offset: 0x0000C5F7
		public LdapException(int errorCode, string message)
			: base(message)
		{
			this.errorCode = errorCode;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000D612 File Offset: 0x0000C612
		public LdapException(int errorCode, string message, string serverErrorMessage)
			: base(message)
		{
			this.errorCode = errorCode;
			this.serverErrorMessage = serverErrorMessage;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000D634 File Offset: 0x0000C634
		public LdapException(int errorCode, string message, Exception inner)
			: base(message, inner)
		{
			this.errorCode = errorCode;
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000D650 File Offset: 0x0000C650
		public int ErrorCode
		{
			get
			{
				return this.errorCode;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000D658 File Offset: 0x0000C658
		public string ServerErrorMessage
		{
			get
			{
				return this.serverErrorMessage;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000D660 File Offset: 0x0000C660
		public PartialResultsCollection PartialResults
		{
			get
			{
				return this.results;
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000D668 File Offset: 0x0000C668
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x0400026C RID: 620
		private int errorCode;

		// Token: 0x0400026D RID: 621
		private string serverErrorMessage;

		// Token: 0x0400026E RID: 622
		internal PartialResultsCollection results = new PartialResultsCollection();
	}
}

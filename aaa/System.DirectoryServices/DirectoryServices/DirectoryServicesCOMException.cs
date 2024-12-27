using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.DirectoryServices
{
	// Token: 0x02000046 RID: 70
	[Serializable]
	public class DirectoryServicesCOMException : COMException, ISerializable
	{
		// Token: 0x060001E4 RID: 484 RVA: 0x00007CDD File Offset: 0x00006CDD
		public DirectoryServicesCOMException()
		{
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00007CF0 File Offset: 0x00006CF0
		public DirectoryServicesCOMException(string message)
			: base(message)
		{
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00007D04 File Offset: 0x00006D04
		public DirectoryServicesCOMException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00007D19 File Offset: 0x00006D19
		protected DirectoryServicesCOMException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00007D2E File Offset: 0x00006D2E
		internal DirectoryServicesCOMException(string extendedMessage, int extendedError, COMException e)
			: base(e.Message, e.ErrorCode)
		{
			this.extendederror = extendedError;
			this.extendedmessage = extendedMessage;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00007D5B File Offset: 0x00006D5B
		public int ExtendedError
		{
			get
			{
				return this.extendederror;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00007D63 File Offset: 0x00006D63
		public string ExtendedErrorMessage
		{
			get
			{
				return this.extendedmessage;
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00007D6B File Offset: 0x00006D6B
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x04000202 RID: 514
		private int extendederror;

		// Token: 0x04000203 RID: 515
		private string extendedmessage = "";
	}
}

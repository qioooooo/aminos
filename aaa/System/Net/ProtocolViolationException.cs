using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x0200042B RID: 1067
	[Serializable]
	public class ProtocolViolationException : InvalidOperationException, ISerializable
	{
		// Token: 0x0600215F RID: 8543 RVA: 0x000841D9 File Offset: 0x000831D9
		public ProtocolViolationException()
		{
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x000841E1 File Offset: 0x000831E1
		public ProtocolViolationException(string message)
			: base(message)
		{
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x000841EA File Offset: 0x000831EA
		protected ProtocolViolationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x000841F4 File Offset: 0x000831F4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x000841FE File Offset: 0x000831FE
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}
	}
}

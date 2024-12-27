using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x02000398 RID: 920
	[Serializable]
	public class CookieException : FormatException, ISerializable
	{
		// Token: 0x06001CBA RID: 7354 RVA: 0x0006D874 File Offset: 0x0006C874
		public CookieException()
		{
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x0006D87C File Offset: 0x0006C87C
		internal CookieException(string message)
			: base(message)
		{
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x0006D885 File Offset: 0x0006C885
		internal CookieException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x0006D88F File Offset: 0x0006C88F
		protected CookieException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x0006D899 File Offset: 0x0006C899
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x0006D8A3 File Offset: 0x0006C8A3
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}
	}
}

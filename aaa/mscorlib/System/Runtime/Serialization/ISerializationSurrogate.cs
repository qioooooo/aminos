using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Serialization
{
	// Token: 0x0200034A RID: 842
	[ComVisible(true)]
	public interface ISerializationSurrogate
	{
		// Token: 0x060021B5 RID: 8629
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void GetObjectData(object obj, SerializationInfo info, StreamingContext context);

		// Token: 0x060021B6 RID: 8630
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector);
	}
}

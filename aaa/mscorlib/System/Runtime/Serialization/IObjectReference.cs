using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Serialization
{
	// Token: 0x020000A5 RID: 165
	[ComVisible(true)]
	public interface IObjectReference
	{
		// Token: 0x06000A16 RID: 2582
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		object GetRealObject(StreamingContext context);
	}
}

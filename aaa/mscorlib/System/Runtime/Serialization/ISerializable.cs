using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Serialization
{
	// Token: 0x0200002D RID: 45
	[ComVisible(true)]
	public interface ISerializable
	{
		// Token: 0x0600021C RID: 540
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void GetObjectData(SerializationInfo info, StreamingContext context);
	}
}

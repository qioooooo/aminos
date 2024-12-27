using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000705 RID: 1797
	internal interface ISerializationRootObject
	{
		// Token: 0x060040AF RID: 16559
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void RootSetObjectData(SerializationInfo info, StreamingContext ctx);
	}
}

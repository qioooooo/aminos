using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Serialization
{
	// Token: 0x0200034B RID: 843
	[ComVisible(true)]
	public interface ISurrogateSelector
	{
		// Token: 0x060021B7 RID: 8631
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ChainSelector(ISurrogateSelector selector);

		// Token: 0x060021B8 RID: 8632
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector);

		// Token: 0x060021B9 RID: 8633
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		ISurrogateSelector GetNextSelector();
	}
}

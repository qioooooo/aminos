using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006EE RID: 1774
	[ComVisible(true)]
	public interface IRemotingFormatter : IFormatter
	{
		// Token: 0x06003F9A RID: 16282
		object Deserialize(Stream serializationStream, HeaderHandler handler);

		// Token: 0x06003F9B RID: 16283
		void Serialize(Stream serializationStream, object graph, Header[] headers);
	}
}

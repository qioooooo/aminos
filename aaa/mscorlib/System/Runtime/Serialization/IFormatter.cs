using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000345 RID: 837
	[ComVisible(true)]
	public interface IFormatter
	{
		// Token: 0x06002155 RID: 8533
		object Deserialize(Stream serializationStream);

		// Token: 0x06002156 RID: 8534
		void Serialize(Stream serializationStream, object graph);

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06002157 RID: 8535
		// (set) Token: 0x06002158 RID: 8536
		ISurrogateSelector SurrogateSelector { get; set; }

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06002159 RID: 8537
		// (set) Token: 0x0600215A RID: 8538
		SerializationBinder Binder { get; set; }

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x0600215B RID: 8539
		// (set) Token: 0x0600215C RID: 8540
		StreamingContext Context { get; set; }
	}
}

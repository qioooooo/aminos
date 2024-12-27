using System;
using System.Collections;

namespace System.Runtime.Serialization
{
	// Token: 0x02000360 RID: 864
	internal static class SerializationEventsCache
	{
		// Token: 0x06002249 RID: 8777 RVA: 0x0005729C File Offset: 0x0005629C
		internal static SerializationEvents GetSerializationEventsForType(Type t)
		{
			SerializationEvents serializationEvents;
			if ((serializationEvents = (SerializationEvents)SerializationEventsCache.cache[t]) == null)
			{
				lock (SerializationEventsCache.cache.SyncRoot)
				{
					if ((serializationEvents = (SerializationEvents)SerializationEventsCache.cache[t]) == null)
					{
						serializationEvents = new SerializationEvents(t);
						SerializationEventsCache.cache[t] = serializationEvents;
					}
				}
			}
			return serializationEvents;
		}

		// Token: 0x04000E43 RID: 3651
		private static Hashtable cache = new Hashtable();
	}
}

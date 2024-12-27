using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Runtime.Serialization
{
	// Token: 0x02000357 RID: 855
	public sealed class SerializationObjectManager
	{
		// Token: 0x06002231 RID: 8753 RVA: 0x00056D89 File Offset: 0x00055D89
		public SerializationObjectManager(StreamingContext context)
		{
			this.m_context = context;
			this.m_objectSeenTable = new Hashtable();
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x00056DB0 File Offset: 0x00055DB0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void RegisterObject(object obj)
		{
			SerializationEvents serializationEventsForType = SerializationEventsCache.GetSerializationEventsForType(obj.GetType());
			if (serializationEventsForType.HasOnSerializingEvents && this.m_objectSeenTable[obj] == null)
			{
				this.m_objectSeenTable[obj] = true;
				serializationEventsForType.InvokeOnSerializing(obj, this.m_context);
				this.AddOnSerialized(obj);
			}
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x00056E05 File Offset: 0x00055E05
		public void RaiseOnSerializedEvent()
		{
			if (this.m_onSerializedHandler != null)
			{
				this.m_onSerializedHandler(this.m_context);
			}
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x00056E20 File Offset: 0x00055E20
		private void AddOnSerialized(object obj)
		{
			SerializationEvents serializationEventsForType = SerializationEventsCache.GetSerializationEventsForType(obj.GetType());
			this.m_onSerializedHandler = serializationEventsForType.AddOnSerialized(obj, this.m_onSerializedHandler);
		}

		// Token: 0x04000E38 RID: 3640
		private Hashtable m_objectSeenTable = new Hashtable();

		// Token: 0x04000E39 RID: 3641
		private SerializationEventHandler m_onSerializedHandler;

		// Token: 0x04000E3A RID: 3642
		private StreamingContext m_context;
	}
}

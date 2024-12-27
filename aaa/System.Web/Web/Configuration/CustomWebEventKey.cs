using System;

namespace System.Web.Configuration
{
	// Token: 0x020001D2 RID: 466
	internal class CustomWebEventKey
	{
		// Token: 0x06001A40 RID: 6720 RVA: 0x0007B328 File Offset: 0x0007A328
		internal CustomWebEventKey(Type eventType, int eventCode)
		{
			this._type = eventType;
			this._eventCode = eventCode;
		}

		// Token: 0x040017CB RID: 6091
		internal Type _type;

		// Token: 0x040017CC RID: 6092
		internal int _eventCode;
	}
}

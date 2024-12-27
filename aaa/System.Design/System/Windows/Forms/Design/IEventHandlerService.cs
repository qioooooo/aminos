using System;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200022F RID: 559
	internal interface IEventHandlerService
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x0600155C RID: 5468
		// (remove) Token: 0x0600155D RID: 5469
		event EventHandler EventHandlerChanged;

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x0600155E RID: 5470
		Control FocusWindow { get; }

		// Token: 0x0600155F RID: 5471
		object GetHandler(Type handlerType);

		// Token: 0x06001560 RID: 5472
		void PopHandler(object handler);

		// Token: 0x06001561 RID: 5473
		void PushHandler(object handler);
	}
}

using System;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000230 RID: 560
	public sealed class EventHandlerService : IEventHandlerService
	{
		// Token: 0x06001562 RID: 5474 RVA: 0x0006F3D7 File Offset: 0x0006E3D7
		public EventHandlerService(Control focusWnd)
		{
			this.focusWnd = focusWnd;
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06001563 RID: 5475 RVA: 0x0006F3E6 File Offset: 0x0006E3E6
		// (remove) Token: 0x06001564 RID: 5476 RVA: 0x0006F3FF File Offset: 0x0006E3FF
		public event EventHandler EventHandlerChanged
		{
			add
			{
				this.changedEvent = (EventHandler)Delegate.Combine(this.changedEvent, value);
			}
			remove
			{
				this.changedEvent = (EventHandler)Delegate.Remove(this.changedEvent, value);
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001565 RID: 5477 RVA: 0x0006F418 File Offset: 0x0006E418
		public Control FocusWindow
		{
			get
			{
				return this.focusWnd;
			}
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0006F420 File Offset: 0x0006E420
		public object GetHandler(Type handlerType)
		{
			if (handlerType == this.lastHandlerType)
			{
				return this.lastHandler;
			}
			for (EventHandlerService.HandlerEntry next = this.handlerHead; next != null; next = next.next)
			{
				if (next.handler != null && handlerType.IsInstanceOfType(next.handler))
				{
					this.lastHandlerType = handlerType;
					this.lastHandler = next.handler;
					return next.handler;
				}
			}
			return null;
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0006F481 File Offset: 0x0006E481
		private void OnEventHandlerChanged(EventArgs e)
		{
			if (this.changedEvent != null)
			{
				this.changedEvent(this, e);
			}
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0006F498 File Offset: 0x0006E498
		public void PopHandler(object handler)
		{
			for (EventHandlerService.HandlerEntry next = this.handlerHead; next != null; next = next.next)
			{
				if (next.handler == handler)
				{
					this.handlerHead = next.next;
					this.lastHandler = null;
					this.lastHandlerType = null;
					this.OnEventHandlerChanged(EventArgs.Empty);
					return;
				}
			}
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0006F4E7 File Offset: 0x0006E4E7
		public void PushHandler(object handler)
		{
			this.handlerHead = new EventHandlerService.HandlerEntry(handler, this.handlerHead);
			this.lastHandlerType = handler.GetType();
			this.lastHandler = this.handlerHead.handler;
			this.OnEventHandlerChanged(EventArgs.Empty);
		}

		// Token: 0x04001281 RID: 4737
		private object lastHandler;

		// Token: 0x04001282 RID: 4738
		private Type lastHandlerType;

		// Token: 0x04001283 RID: 4739
		private EventHandlerService.HandlerEntry handlerHead;

		// Token: 0x04001284 RID: 4740
		private EventHandler changedEvent;

		// Token: 0x04001285 RID: 4741
		private readonly Control focusWnd;

		// Token: 0x02000231 RID: 561
		private sealed class HandlerEntry
		{
			// Token: 0x0600156A RID: 5482 RVA: 0x0006F523 File Offset: 0x0006E523
			public HandlerEntry(object handler, EventHandlerService.HandlerEntry next)
			{
				this.handler = handler;
				this.next = next;
			}

			// Token: 0x04001286 RID: 4742
			public object handler;

			// Token: 0x04001287 RID: 4743
			public EventHandlerService.HandlerEntry next;
		}
	}
}

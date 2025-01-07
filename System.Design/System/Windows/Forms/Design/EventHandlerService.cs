using System;

namespace System.Windows.Forms.Design
{
	public sealed class EventHandlerService : IEventHandlerService
	{
		public EventHandlerService(Control focusWnd)
		{
			this.focusWnd = focusWnd;
		}

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

		public Control FocusWindow
		{
			get
			{
				return this.focusWnd;
			}
		}

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

		private void OnEventHandlerChanged(EventArgs e)
		{
			if (this.changedEvent != null)
			{
				this.changedEvent(this, e);
			}
		}

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

		public void PushHandler(object handler)
		{
			this.handlerHead = new EventHandlerService.HandlerEntry(handler, this.handlerHead);
			this.lastHandlerType = handler.GetType();
			this.lastHandler = this.handlerHead.handler;
			this.OnEventHandlerChanged(EventArgs.Empty);
		}

		private object lastHandler;

		private Type lastHandlerType;

		private EventHandlerService.HandlerEntry handlerHead;

		private EventHandler changedEvent;

		private readonly Control focusWnd;

		private sealed class HandlerEntry
		{
			public HandlerEntry(object handler, EventHandlerService.HandlerEntry next)
			{
				this.handler = handler;
				this.next = next;
			}

			public object handler;

			public EventHandlerService.HandlerEntry next;
		}
	}
}

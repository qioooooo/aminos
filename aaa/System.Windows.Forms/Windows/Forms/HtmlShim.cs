using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200042C RID: 1068
	internal abstract class HtmlShim : IDisposable
	{
		// Token: 0x06003FB1 RID: 16305 RVA: 0x000E6DD4 File Offset: 0x000E5DD4
		~HtmlShim()
		{
			this.Dispose(false);
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06003FB2 RID: 16306 RVA: 0x000E6E04 File Offset: 0x000E5E04
		private EventHandlerList Events
		{
			get
			{
				if (this.events == null)
				{
					this.events = new EventHandlerList();
				}
				return this.events;
			}
		}

		// Token: 0x06003FB3 RID: 16307
		public abstract void AttachEventHandler(string eventName, EventHandler eventHandler);

		// Token: 0x06003FB4 RID: 16308 RVA: 0x000E6E1F File Offset: 0x000E5E1F
		public void AddHandler(object key, Delegate value)
		{
			this.eventCount++;
			this.Events.AddHandler(key, value);
			this.OnEventHandlerAdded();
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x000E6E44 File Offset: 0x000E5E44
		protected HtmlToClrEventProxy AddEventProxy(string eventName, EventHandler eventHandler)
		{
			if (this.attachedEventList == null)
			{
				this.attachedEventList = new Dictionary<EventHandler, HtmlToClrEventProxy>();
			}
			HtmlToClrEventProxy htmlToClrEventProxy = new HtmlToClrEventProxy(this, eventName, eventHandler);
			this.attachedEventList[eventHandler] = htmlToClrEventProxy;
			return htmlToClrEventProxy;
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06003FB6 RID: 16310
		public abstract UnsafeNativeMethods.IHTMLWindow2 AssociatedWindow { get; }

		// Token: 0x06003FB7 RID: 16311
		public abstract void ConnectToEvents();

		// Token: 0x06003FB8 RID: 16312
		public abstract void DetachEventHandler(string eventName, EventHandler eventHandler);

		// Token: 0x06003FB9 RID: 16313 RVA: 0x000E6E7C File Offset: 0x000E5E7C
		public virtual void DisconnectFromEvents()
		{
			if (this.attachedEventList != null)
			{
				EventHandler[] array = new EventHandler[this.attachedEventList.Count];
				this.attachedEventList.Keys.CopyTo(array, 0);
				foreach (EventHandler eventHandler in array)
				{
					HtmlToClrEventProxy htmlToClrEventProxy = this.attachedEventList[eventHandler];
					this.DetachEventHandler(htmlToClrEventProxy.EventName, eventHandler);
				}
			}
		}

		// Token: 0x06003FBA RID: 16314
		protected abstract object GetEventSender();

		// Token: 0x06003FBB RID: 16315 RVA: 0x000E6EE7 File Offset: 0x000E5EE7
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x000E6EF6 File Offset: 0x000E5EF6
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.DisconnectFromEvents();
				if (this.events != null)
				{
					this.events.Dispose();
					this.events = null;
				}
			}
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x000E6F1C File Offset: 0x000E5F1C
		public void FireEvent(object key, EventArgs e)
		{
			Delegate @delegate = this.Events[key];
			if (@delegate != null)
			{
				try
				{
					@delegate.DynamicInvoke(new object[]
					{
						this.GetEventSender(),
						e
					});
				}
				catch (Exception ex)
				{
					if (NativeWindow.WndProcShouldBeDebuggable)
					{
						throw;
					}
					Application.OnThreadException(ex);
				}
			}
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x000E6F7C File Offset: 0x000E5F7C
		protected virtual void OnEventHandlerAdded()
		{
			this.ConnectToEvents();
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x000E6F84 File Offset: 0x000E5F84
		protected virtual void OnEventHandlerRemoved()
		{
			if (this.eventCount <= 0)
			{
				this.DisconnectFromEvents();
				this.eventCount = 0;
			}
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x000E6F9C File Offset: 0x000E5F9C
		public void RemoveHandler(object key, Delegate value)
		{
			this.eventCount--;
			this.Events.RemoveHandler(key, value);
			this.OnEventHandlerRemoved();
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x000E6FC0 File Offset: 0x000E5FC0
		protected HtmlToClrEventProxy RemoveEventProxy(EventHandler eventHandler)
		{
			if (this.attachedEventList == null)
			{
				return null;
			}
			if (this.attachedEventList.ContainsKey(eventHandler))
			{
				HtmlToClrEventProxy htmlToClrEventProxy = this.attachedEventList[eventHandler];
				this.attachedEventList.Remove(eventHandler);
				return htmlToClrEventProxy;
			}
			return null;
		}

		// Token: 0x04001F34 RID: 7988
		private EventHandlerList events;

		// Token: 0x04001F35 RID: 7989
		private int eventCount;

		// Token: 0x04001F36 RID: 7990
		private Dictionary<EventHandler, HtmlToClrEventProxy> attachedEventList;
	}
}

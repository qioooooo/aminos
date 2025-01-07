using System;

namespace System.Web.UI.Design
{
	public class ViewEventArgs : EventArgs
	{
		public ViewEventArgs(ViewEvent eventType, DesignerRegion region, EventArgs eventArgs)
		{
			this._eventType = eventType;
			this._region = region;
			this._eventArgs = eventArgs;
		}

		public EventArgs EventArgs
		{
			get
			{
				return this._eventArgs;
			}
		}

		public ViewEvent EventType
		{
			get
			{
				return this._eventType;
			}
		}

		public DesignerRegion Region
		{
			get
			{
				return this._region;
			}
		}

		private DesignerRegion _region;

		private EventArgs _eventArgs;

		private ViewEvent _eventType;
	}
}

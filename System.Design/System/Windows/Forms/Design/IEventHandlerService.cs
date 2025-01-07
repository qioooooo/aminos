using System;

namespace System.Windows.Forms.Design
{
	internal interface IEventHandlerService
	{
		event EventHandler EventHandlerChanged;

		Control FocusWindow { get; }

		object GetHandler(Type handlerType);

		void PopHandler(object handler);

		void PushHandler(object handler);
	}
}

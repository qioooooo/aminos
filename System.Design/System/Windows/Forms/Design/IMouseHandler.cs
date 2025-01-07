using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	internal interface IMouseHandler
	{
		void OnMouseDoubleClick(IComponent component);

		void OnMouseDown(IComponent component, MouseButtons button, int x, int y);

		void OnMouseHover(IComponent component);

		void OnMouseMove(IComponent component, int x, int y);

		void OnMouseUp(IComponent component, MouseButtons button);

		void OnSetCursor(IComponent component);
	}
}

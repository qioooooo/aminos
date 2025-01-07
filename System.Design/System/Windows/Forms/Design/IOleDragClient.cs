using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	internal interface IOleDragClient
	{
		IComponent Component { get; }

		bool AddComponent(IComponent component, string name, bool firstAdd);

		bool CanModifyComponents { get; }

		bool IsDropOk(IComponent component);

		Control GetDesignerControl();

		Control GetControlForComponent(object component);
	}
}

using System;

namespace System.Windows.Forms.Design
{
	internal interface ISplitWindowService
	{
		void AddSplitWindow(Control window);

		void RemoveSplitWindow(Control window);
	}
}

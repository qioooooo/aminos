using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Design
{
	[ComVisible(true)]
	internal interface ISupportInSituService
	{
		bool IgnoreMessages { get; }

		void HandleKeyChar();

		IntPtr GetEditWindow();
	}
}

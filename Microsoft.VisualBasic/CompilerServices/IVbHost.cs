using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IVbHost
	{
		IWin32Window GetParentWindow();

		string GetWindowTitle();
	}
}

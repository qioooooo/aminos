using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public delegate void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e);
}

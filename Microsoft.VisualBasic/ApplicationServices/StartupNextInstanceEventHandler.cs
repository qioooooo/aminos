using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public delegate void StartupNextInstanceEventHandler(object sender, StartupNextInstanceEventArgs e);
}

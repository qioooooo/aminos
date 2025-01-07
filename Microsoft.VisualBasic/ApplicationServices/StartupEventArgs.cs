using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[ComVisible(false)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public class StartupEventArgs : CancelEventArgs
	{
		public StartupEventArgs(ReadOnlyCollection<string> args)
		{
			if (args == null)
			{
				args = new ReadOnlyCollection<string>(null);
			}
			this.m_CommandLine = args;
		}

		public ReadOnlyCollection<string> CommandLine
		{
			get
			{
				return this.m_CommandLine;
			}
		}

		private ReadOnlyCollection<string> m_CommandLine;
	}
}

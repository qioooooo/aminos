using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public class StartupNextInstanceEventArgs : EventArgs
	{
		public StartupNextInstanceEventArgs(ReadOnlyCollection<string> args, bool bringToForegroundFlag)
		{
			if (args == null)
			{
				args = new ReadOnlyCollection<string>(null);
			}
			this.m_CommandLine = args;
			this.m_BringToForeground = bringToForegroundFlag;
		}

		public bool BringToForeground
		{
			get
			{
				return this.m_BringToForeground;
			}
			set
			{
				this.m_BringToForeground = value;
			}
		}

		public ReadOnlyCollection<string> CommandLine
		{
			get
			{
				return this.m_CommandLine;
			}
		}

		private bool m_BringToForeground;

		private ReadOnlyCollection<string> m_CommandLine;
	}
}

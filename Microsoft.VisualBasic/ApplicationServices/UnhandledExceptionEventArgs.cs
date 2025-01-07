using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[ComVisible(false)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public class UnhandledExceptionEventArgs : ThreadExceptionEventArgs
	{
		public UnhandledExceptionEventArgs(bool exitApplication, Exception exception)
			: base(exception)
		{
			this.m_ExitApplication = exitApplication;
		}

		public bool ExitApplication
		{
			get
			{
				return this.m_ExitApplication;
			}
			set
			{
				this.m_ExitApplication = value;
			}
		}

		private bool m_ExitApplication;
	}
}

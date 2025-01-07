using System;

namespace Microsoft.VisualBasic.Devices
{
	public class NetworkAvailableEventArgs : EventArgs
	{
		public NetworkAvailableEventArgs(bool networkAvailable)
		{
			this.m_NetworkAvailable = networkAvailable;
		}

		public bool IsNetworkAvailable
		{
			get
			{
				return this.m_NetworkAvailable;
			}
		}

		private bool m_NetworkAvailable;
	}
}

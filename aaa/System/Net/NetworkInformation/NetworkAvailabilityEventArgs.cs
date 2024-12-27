using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000612 RID: 1554
	public class NetworkAvailabilityEventArgs : EventArgs
	{
		// Token: 0x06002FF2 RID: 12274 RVA: 0x000CF3E3 File Offset: 0x000CE3E3
		internal NetworkAvailabilityEventArgs(bool isAvailable)
		{
			this.isAvailable = isAvailable;
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06002FF3 RID: 12275 RVA: 0x000CF3F2 File Offset: 0x000CE3F2
		public bool IsAvailable
		{
			get
			{
				return this.isAvailable;
			}
		}

		// Token: 0x04002DC4 RID: 11716
		private bool isAvailable;
	}
}

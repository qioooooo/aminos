using System;
using System.Net;

namespace Microsoft.VisualBasic.Devices
{
	internal class WebClientExtended : WebClient
	{
		public WebClientExtended()
		{
			this.m_Timeout = 100000;
		}

		public int Timeout
		{
			set
			{
				this.m_Timeout = value;
			}
		}

		public bool UseNonPassiveFtp
		{
			set
			{
				this.m_UseNonPassiveFtp = value;
			}
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest webRequest = base.GetWebRequest(address);
			if (webRequest != null)
			{
				webRequest.Timeout = this.m_Timeout;
				if (this.m_UseNonPassiveFtp)
				{
					FtpWebRequest ftpWebRequest = webRequest as FtpWebRequest;
					if (ftpWebRequest != null)
					{
						ftpWebRequest.UsePassive = false;
					}
				}
				HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
				if (httpWebRequest != null)
				{
					httpWebRequest.AllowAutoRedirect = false;
				}
			}
			return webRequest;
		}

		private int m_Timeout;

		private bool m_UseNonPassiveFtp;
	}
}

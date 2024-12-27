using System;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x0200003D RID: 61
	internal static class TcpChannelHelper
	{
		// Token: 0x060001FD RID: 509 RVA: 0x0000A0AC File Offset: 0x000090AC
		internal static string ParseURL(string url, out string objectURI)
		{
			objectURI = null;
			if (!StringHelper.StartsWithAsciiIgnoreCasePrefixLower(url, "tcp://"))
			{
				return null;
			}
			int num = "tcp://".Length;
			num = url.IndexOf('/', num);
			if (-1 == num)
			{
				return url;
			}
			string text = url.Substring(0, num);
			objectURI = url.Substring(num);
			return text;
		}

		// Token: 0x04000164 RID: 356
		private const string _tcp = "tcp://";
	}
}

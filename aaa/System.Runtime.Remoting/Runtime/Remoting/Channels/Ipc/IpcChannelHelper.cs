using System;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x02000056 RID: 86
	internal static class IpcChannelHelper
	{
		// Token: 0x060002BA RID: 698 RVA: 0x0000D744 File Offset: 0x0000C744
		internal static bool StartsWithIpc(string url)
		{
			return StringHelper.StartsWithAsciiIgnoreCasePrefixLower(url, "ipc://");
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000D754 File Offset: 0x0000C754
		internal static string ParseURL(string url, out string objectURI)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			objectURI = null;
			if (!IpcChannelHelper.StartsWithIpc(url))
			{
				return null;
			}
			int num = "ipc://".Length;
			num = url.IndexOf('/', num);
			if (-1 == num)
			{
				return url;
			}
			string text = url.Substring(0, num);
			objectURI = url.Substring(num);
			return text;
		}

		// Token: 0x040001F4 RID: 500
		private const string _ipc = "ipc://";
	}
}

using System;
using System.DirectoryServices;
using System.Web;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000011 RID: 17
	internal static class IisHelper
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00003C74 File Offset: 0x00002C74
		internal static void Initialize()
		{
			try
			{
				HttpRequest request = HttpContext.Current.Request;
				string text = request.ServerVariables["APPL_MD_PATH"];
				bool flag = false;
				if (text.StartsWith("/LM/", StringComparison.Ordinal))
				{
					text = "IIS://localhost/" + text.Substring(4);
					DirectoryEntry directoryEntry = new DirectoryEntry(text);
					flag = (bool)directoryEntry.Properties["AccessSSL"][0];
				}
				IisHelper._bIsSslRequired = flag;
			}
			catch
			{
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003D00 File Offset: 0x00002D00
		internal static bool IsSslRequired
		{
			get
			{
				return IisHelper._bIsSslRequired;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003D07 File Offset: 0x00002D07
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00003D0E File Offset: 0x00002D0E
		internal static string ApplicationUrl
		{
			get
			{
				return IisHelper._iisAppUrl;
			}
			set
			{
				IisHelper._iisAppUrl = value;
			}
		}

		// Token: 0x04000079 RID: 121
		private static bool _bIsSslRequired;

		// Token: 0x0400007A RID: 122
		private static string _iisAppUrl;
	}
}

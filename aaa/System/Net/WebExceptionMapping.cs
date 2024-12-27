using System;

namespace System.Net
{
	// Token: 0x020004A1 RID: 1185
	internal static class WebExceptionMapping
	{
		// Token: 0x0600241C RID: 9244 RVA: 0x0008D384 File Offset: 0x0008C384
		internal static string GetWebStatusString(WebExceptionStatus status)
		{
			if (status >= (WebExceptionStatus)WebExceptionMapping.s_Mapping.Length || status < WebExceptionStatus.Success)
			{
				throw new InternalException();
			}
			string text = WebExceptionMapping.s_Mapping[(int)status];
			if (text == null)
			{
				text = "net_webstatus_" + status.ToString();
				WebExceptionMapping.s_Mapping[(int)status] = text;
			}
			return text;
		}

		// Token: 0x04002485 RID: 9349
		private static readonly string[] s_Mapping = new string[21];
	}
}

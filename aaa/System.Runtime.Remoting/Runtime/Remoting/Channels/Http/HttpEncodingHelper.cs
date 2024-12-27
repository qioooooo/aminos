using System;
using System.Text;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000027 RID: 39
	internal static class HttpEncodingHelper
	{
		// Token: 0x06000125 RID: 293 RVA: 0x0000612C File Offset: 0x0000512C
		internal static string EncodeUriAsXLinkHref(string uri)
		{
			if (uri == null)
			{
				return null;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(uri);
			StringBuilder stringBuilder = new StringBuilder(uri.Length);
			foreach (byte b in bytes)
			{
				if (!HttpEncodingHelper.EscapeInXLinkHref(b))
				{
					stringBuilder.Append((char)b);
				}
				else
				{
					stringBuilder.Append('%');
					stringBuilder.Append(HttpChannelHelper.DecimalToCharacterHexDigit(b >> 4));
					stringBuilder.Append(HttpChannelHelper.DecimalToCharacterHexDigit((int)(b & 15)));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000061AF File Offset: 0x000051AF
		internal static bool EscapeInXLinkHref(byte ch)
		{
			return ch <= 32 || ch >= 128 || ch == 60 || ch == 62 || ch == 34;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000061D0 File Offset: 0x000051D0
		internal static string DecodeUri(string uri)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(uri);
			int num;
			HttpChannelHelper.DecodeUriInPlace(bytes, out num);
			return Encoding.UTF8.GetString(bytes, 0, num);
		}
	}
}

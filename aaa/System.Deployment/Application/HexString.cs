using System;
using System.Globalization;
using System.Text;

namespace System.Deployment.Application
{
	// Token: 0x020000E5 RID: 229
	internal static class HexString
	{
		// Token: 0x060005DE RID: 1502 RVA: 0x0001E614 File Offset: 0x0001D614
		public static string FromBytes(byte[] bytes)
		{
			StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);
			for (int i = 0; i < bytes.Length; i++)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", new object[] { bytes[i] });
			}
			return stringBuilder.ToString();
		}
	}
}

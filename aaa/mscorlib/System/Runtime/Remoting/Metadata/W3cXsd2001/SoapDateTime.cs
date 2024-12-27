using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000764 RID: 1892
	[ComVisible(true)]
	public sealed class SoapDateTime
	{
		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x060043E4 RID: 17380 RVA: 0x000E991B File Offset: 0x000E891B
		public static string XsdType
		{
			get
			{
				return "dateTime";
			}
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x000E9922 File Offset: 0x000E8922
		public static string ToString(DateTime value)
		{
			return value.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffffzzz", CultureInfo.InvariantCulture);
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x000E9938 File Offset: 0x000E8938
		public static DateTime Parse(string value)
		{
			DateTime dateTime;
			try
			{
				if (value == null)
				{
					dateTime = DateTime.MinValue;
				}
				else
				{
					string text = value;
					if (value.EndsWith("Z", StringComparison.Ordinal))
					{
						text = value.Substring(0, value.Length - 1) + "-00:00";
					}
					dateTime = DateTime.ParseExact(text, SoapDateTime.formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
				}
			}
			catch (Exception)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:dateTime", value }));
			}
			return dateTime;
		}

		// Token: 0x040021F0 RID: 8688
		private static string[] formats = new string[]
		{
			"yyyy-MM-dd'T'HH:mm:ss.fffffffzzz", "yyyy-MM-dd'T'HH:mm:ss.ffff", "yyyy-MM-dd'T'HH:mm:ss.ffffzzz", "yyyy-MM-dd'T'HH:mm:ss.fff", "yyyy-MM-dd'T'HH:mm:ss.fffzzz", "yyyy-MM-dd'T'HH:mm:ss.ff", "yyyy-MM-dd'T'HH:mm:ss.ffzzz", "yyyy-MM-dd'T'HH:mm:ss.f", "yyyy-MM-dd'T'HH:mm:ss.fzzz", "yyyy-MM-dd'T'HH:mm:ss",
			"yyyy-MM-dd'T'HH:mm:sszzz", "yyyy-MM-dd'T'HH:mm:ss.fffff", "yyyy-MM-dd'T'HH:mm:ss.fffffzzz", "yyyy-MM-dd'T'HH:mm:ss.ffffff", "yyyy-MM-dd'T'HH:mm:ss.ffffffzzz", "yyyy-MM-dd'T'HH:mm:ss.fffffff", "yyyy-MM-dd'T'HH:mm:ss.ffffffff", "yyyy-MM-dd'T'HH:mm:ss.ffffffffzzz", "yyyy-MM-dd'T'HH:mm:ss.fffffffff", "yyyy-MM-dd'T'HH:mm:ss.fffffffffzzz",
			"yyyy-MM-dd'T'HH:mm:ss.ffffffffff", "yyyy-MM-dd'T'HH:mm:ss.ffffffffffzzz"
		};
	}
}

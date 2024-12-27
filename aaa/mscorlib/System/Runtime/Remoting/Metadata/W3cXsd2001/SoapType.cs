using System;
using System.Text;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000762 RID: 1890
	internal static class SoapType
	{
		// Token: 0x060043DF RID: 17375 RVA: 0x000E952C File Offset: 0x000E852C
		internal static string FilterBin64(string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] != ' ' && value[i] != '\n' && value[i] != '\r')
				{
					stringBuilder.Append(value[i]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x000E9588 File Offset: 0x000E8588
		internal static string LineFeedsBin64(string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < value.Length; i++)
			{
				if (i % 76 == 0)
				{
					stringBuilder.Append('\n');
				}
				stringBuilder.Append(value[i]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x000E95D0 File Offset: 0x000E85D0
		internal static string Escape(string value)
		{
			if (value == null || value.Length == 0)
			{
				return value;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = value.IndexOf('&');
			if (num > -1)
			{
				stringBuilder.Append(value);
				stringBuilder.Replace("&", "&#38;", num, stringBuilder.Length - num);
			}
			num = value.IndexOf('"');
			if (num > -1)
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(value);
				}
				stringBuilder.Replace("\"", "&#34;", num, stringBuilder.Length - num);
			}
			num = value.IndexOf('\'');
			if (num > -1)
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(value);
				}
				stringBuilder.Replace("'", "&#39;", num, stringBuilder.Length - num);
			}
			num = value.IndexOf('<');
			if (num > -1)
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(value);
				}
				stringBuilder.Replace("<", "&#60;", num, stringBuilder.Length - num);
			}
			num = value.IndexOf('>');
			if (num > -1)
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(value);
				}
				stringBuilder.Replace(">", "&#62;", num, stringBuilder.Length - num);
			}
			num = value.IndexOf('\0');
			if (num > -1)
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(value);
				}
				stringBuilder.Replace('\0'.ToString(), "&#0;", num, stringBuilder.Length - num);
			}
			string text;
			if (stringBuilder.Length > 0)
			{
				text = stringBuilder.ToString();
			}
			else
			{
				text = value;
			}
			return text;
		}

		// Token: 0x040021D2 RID: 8658
		internal static Type typeofSoapTime = typeof(SoapTime);

		// Token: 0x040021D3 RID: 8659
		internal static Type typeofSoapDate = typeof(SoapDate);

		// Token: 0x040021D4 RID: 8660
		internal static Type typeofSoapYearMonth = typeof(SoapYearMonth);

		// Token: 0x040021D5 RID: 8661
		internal static Type typeofSoapYear = typeof(SoapYear);

		// Token: 0x040021D6 RID: 8662
		internal static Type typeofSoapMonthDay = typeof(SoapMonthDay);

		// Token: 0x040021D7 RID: 8663
		internal static Type typeofSoapDay = typeof(SoapDay);

		// Token: 0x040021D8 RID: 8664
		internal static Type typeofSoapMonth = typeof(SoapMonth);

		// Token: 0x040021D9 RID: 8665
		internal static Type typeofSoapHexBinary = typeof(SoapHexBinary);

		// Token: 0x040021DA RID: 8666
		internal static Type typeofSoapBase64Binary = typeof(SoapBase64Binary);

		// Token: 0x040021DB RID: 8667
		internal static Type typeofSoapInteger = typeof(SoapInteger);

		// Token: 0x040021DC RID: 8668
		internal static Type typeofSoapPositiveInteger = typeof(SoapPositiveInteger);

		// Token: 0x040021DD RID: 8669
		internal static Type typeofSoapNonPositiveInteger = typeof(SoapNonPositiveInteger);

		// Token: 0x040021DE RID: 8670
		internal static Type typeofSoapNonNegativeInteger = typeof(SoapNonNegativeInteger);

		// Token: 0x040021DF RID: 8671
		internal static Type typeofSoapNegativeInteger = typeof(SoapNegativeInteger);

		// Token: 0x040021E0 RID: 8672
		internal static Type typeofSoapAnyUri = typeof(SoapAnyUri);

		// Token: 0x040021E1 RID: 8673
		internal static Type typeofSoapQName = typeof(SoapQName);

		// Token: 0x040021E2 RID: 8674
		internal static Type typeofSoapNotation = typeof(SoapNotation);

		// Token: 0x040021E3 RID: 8675
		internal static Type typeofSoapNormalizedString = typeof(SoapNormalizedString);

		// Token: 0x040021E4 RID: 8676
		internal static Type typeofSoapToken = typeof(SoapToken);

		// Token: 0x040021E5 RID: 8677
		internal static Type typeofSoapLanguage = typeof(SoapLanguage);

		// Token: 0x040021E6 RID: 8678
		internal static Type typeofSoapName = typeof(SoapName);

		// Token: 0x040021E7 RID: 8679
		internal static Type typeofSoapIdrefs = typeof(SoapIdrefs);

		// Token: 0x040021E8 RID: 8680
		internal static Type typeofSoapEntities = typeof(SoapEntities);

		// Token: 0x040021E9 RID: 8681
		internal static Type typeofSoapNmtoken = typeof(SoapNmtoken);

		// Token: 0x040021EA RID: 8682
		internal static Type typeofSoapNmtokens = typeof(SoapNmtokens);

		// Token: 0x040021EB RID: 8683
		internal static Type typeofSoapNcName = typeof(SoapNcName);

		// Token: 0x040021EC RID: 8684
		internal static Type typeofSoapId = typeof(SoapId);

		// Token: 0x040021ED RID: 8685
		internal static Type typeofSoapIdref = typeof(SoapIdref);

		// Token: 0x040021EE RID: 8686
		internal static Type typeofSoapEntity = typeof(SoapEntity);

		// Token: 0x040021EF RID: 8687
		internal static Type typeofISoapXsd = typeof(ISoapXsd);
	}
}

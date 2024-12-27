using System;
using System.Collections.Specialized;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x0200079D RID: 1949
	public class AppSettingsReader
	{
		// Token: 0x06003C08 RID: 15368 RVA: 0x00100B9E File Offset: 0x000FFB9E
		public AppSettingsReader()
		{
			this.map = ConfigurationManager.AppSettings;
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x00100BB4 File Offset: 0x000FFBB4
		public object GetValue(string key, Type type)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			string text = this.map[key];
			if (text == null)
			{
				throw new InvalidOperationException(SR.GetString("AppSettingsReaderNoKey", new object[] { key }));
			}
			if (type != AppSettingsReader.stringType)
			{
				object obj;
				try
				{
					obj = Convert.ChangeType(text, type, CultureInfo.InvariantCulture);
				}
				catch (Exception)
				{
					string text2 = ((text.Length == 0) ? "AppSettingsReaderEmptyString" : text);
					throw new InvalidOperationException(SR.GetString("AppSettingsReaderCantParse", new object[]
					{
						text2,
						key,
						type.ToString()
					}));
				}
				return obj;
			}
			int noneNesting = this.GetNoneNesting(text);
			if (noneNesting == 0)
			{
				return text;
			}
			if (noneNesting == 1)
			{
				return null;
			}
			return text.Substring(1, text.Length - 2);
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x00100C98 File Offset: 0x000FFC98
		private int GetNoneNesting(string val)
		{
			int num = 0;
			int length = val.Length;
			if (length > 1)
			{
				while (val[num] == '(' && val[length - num - 1] == ')')
				{
					num++;
				}
				if (num > 0 && string.Compare(AppSettingsReader.NullString, 0, val, num, length - 2 * num, StringComparison.Ordinal) != 0)
				{
					num = 0;
				}
			}
			return num;
		}

		// Token: 0x040034BA RID: 13498
		private NameValueCollection map;

		// Token: 0x040034BB RID: 13499
		private static Type stringType = typeof(string);

		// Token: 0x040034BC RID: 13500
		private static Type[] paramsArray = new Type[] { AppSettingsReader.stringType };

		// Token: 0x040034BD RID: 13501
		private static string NullString = "None";
	}
}

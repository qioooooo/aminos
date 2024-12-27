using System;
using System.Collections.Specialized;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x0200077A RID: 1914
	internal static class EnvironmentBlock
	{
		// Token: 0x06003B26 RID: 15142 RVA: 0x000FBC40 File Offset: 0x000FAC40
		public static byte[] ToByteArray(StringDictionary sd, bool unicode)
		{
			string[] array = new string[sd.Count];
			sd.Keys.CopyTo(array, 0);
			string[] array2 = new string[sd.Count];
			sd.Values.CopyTo(array2, 0);
			Array.Sort(array, array2, OrdinalCaseInsensitiveComparer.Default);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < sd.Count; i++)
			{
				stringBuilder.Append(array[i]);
				stringBuilder.Append('=');
				stringBuilder.Append(array2[i]);
				stringBuilder.Append('\0');
			}
			stringBuilder.Append('\0');
			byte[] array3;
			if (unicode)
			{
				array3 = Encoding.Unicode.GetBytes(stringBuilder.ToString());
			}
			else
			{
				array3 = Encoding.Default.GetBytes(stringBuilder.ToString());
			}
			if (array3.Length > 65535)
			{
				throw new InvalidOperationException(SR.GetString("EnvironmentBlockTooLong", new object[] { array3.Length }));
			}
			return array3;
		}
	}
}

using System;

namespace Microsoft.JScript
{
	// Token: 0x0200003E RID: 62
	public class CmdLineOptionParser
	{
		// Token: 0x06000293 RID: 659 RVA: 0x000155AD File Offset: 0x000145AD
		public static bool IsSimpleOption(string option, string prefix)
		{
			return string.Compare(option, prefix, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000155BC File Offset: 0x000145BC
		public static string IsArgumentOption(string option, string prefix)
		{
			int length = prefix.Length;
			if (option.Length < length || string.Compare(option, 0, prefix, 0, length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				return null;
			}
			if (option.Length == length)
			{
				return "";
			}
			if (':' != option[length])
			{
				return null;
			}
			return option.Substring(length + 1);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00015610 File Offset: 0x00014610
		public static string IsArgumentOption(string option, string shortPrefix, string longPrefix)
		{
			string text = CmdLineOptionParser.IsArgumentOption(option, shortPrefix);
			if (text == null)
			{
				text = CmdLineOptionParser.IsArgumentOption(option, longPrefix);
			}
			return text;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00015634 File Offset: 0x00014634
		public static object IsBooleanOption(string option, string prefix)
		{
			int length = prefix.Length;
			if (option.Length < prefix.Length || string.Compare(option, 0, prefix, 0, length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				return null;
			}
			if (option.Length == length)
			{
				return true;
			}
			if (option.Length != length + 1)
			{
				return null;
			}
			if ('-' == option[length])
			{
				return false;
			}
			if ('+' == option[length])
			{
				return true;
			}
			return null;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000156A8 File Offset: 0x000146A8
		public static object IsBooleanOption(string option, string shortPrefix, string longPrefix)
		{
			object obj = CmdLineOptionParser.IsBooleanOption(option, shortPrefix);
			if (obj == null)
			{
				obj = CmdLineOptionParser.IsBooleanOption(option, longPrefix);
			}
			return obj;
		}
	}
}

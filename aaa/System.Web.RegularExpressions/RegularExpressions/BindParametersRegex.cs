using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000037 RID: 55
	internal class BindParametersRegex : Regex
	{
		// Token: 0x0600007E RID: 126 RVA: 0x0000CC2C File Offset: 0x0000BC2C
		public BindParametersRegex()
		{
			this.pattern = "\\s*((\"(?<fieldName>(([\\w\\.]+)|(\\[.+\\])))\")|('(?<fieldName>(([\\w\\.]+)|(\\[.+\\])))'))\\s*(,\\s*((\"(?<formatString>.*)\")|('(?<formatString>.*)'))\\s*)?\\s*\\z";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new BindParametersRegexFactory18();
			this.capnames = new Hashtable();
			this.capnames.Add("10", 10);
			this.capnames.Add("8", 8);
			this.capnames.Add("9", 9);
			this.capnames.Add("13", 13);
			this.capnames.Add("formatString", 15);
			this.capnames.Add("fieldName", 14);
			this.capnames.Add("0", 0);
			this.capnames.Add("1", 1);
			this.capnames.Add("2", 2);
			this.capnames.Add("3", 3);
			this.capnames.Add("4", 4);
			this.capnames.Add("5", 5);
			this.capnames.Add("6", 6);
			this.capnames.Add("7", 7);
			this.capnames.Add("11", 11);
			this.capnames.Add("12", 12);
			this.capslist = new string[16];
			this.capslist[0] = "0";
			this.capslist[1] = "1";
			this.capslist[2] = "2";
			this.capslist[3] = "3";
			this.capslist[4] = "4";
			this.capslist[5] = "5";
			this.capslist[6] = "6";
			this.capslist[7] = "7";
			this.capslist[8] = "8";
			this.capslist[9] = "9";
			this.capslist[10] = "10";
			this.capslist[11] = "11";
			this.capslist[12] = "12";
			this.capslist[13] = "13";
			this.capslist[14] = "fieldName";
			this.capslist[15] = "formatString";
			this.capsize = 16;
			base.InitializeReferences();
		}
	}
}

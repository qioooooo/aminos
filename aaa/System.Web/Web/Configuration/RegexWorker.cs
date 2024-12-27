using System;
using System.Collections;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.RegularExpressions;

namespace System.Web.Configuration
{
	// Token: 0x02000236 RID: 566
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class RegexWorker
	{
		// Token: 0x06001E41 RID: 7745 RVA: 0x000878FC File Offset: 0x000868FC
		public RegexWorker(HttpBrowserCapabilities browserCaps)
		{
			this._browserCaps = browserCaps;
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x0008790C File Offset: 0x0008690C
		private string Lookup(string from)
		{
			MatchCollection matchCollection = RegexWorker.RefPat.Matches(from);
			if (matchCollection.Count == 0)
			{
				return from;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				int num2 = match.Index - num;
				stringBuilder.Append(from.Substring(num, num2));
				num = match.Index + match.Length;
				string value = match.Groups["name"].Value;
				string text = null;
				if (this._groups != null)
				{
					text = (string)this._groups[value];
				}
				if (text == null)
				{
					text = this._browserCaps[value];
				}
				stringBuilder.Append(text);
			}
			stringBuilder.Append(from, num, from.Length - num);
			string text2 = stringBuilder.ToString();
			if (text2.Length == 0)
			{
				return null;
			}
			return text2;
		}

		// Token: 0x1700062F RID: 1583
		public string this[string key]
		{
			get
			{
				return this.Lookup(key);
			}
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x00087A30 File Offset: 0x00086A30
		public bool ProcessRegex(string target, string regexExpression)
		{
			if (target == null)
			{
				target = string.Empty;
			}
			Regex regex = new Regex(regexExpression, RegexOptions.ExplicitCapture);
			Match match = regex.Match(target);
			if (!match.Success)
			{
				return false;
			}
			string[] groupNames = regex.GetGroupNames();
			if (groupNames.Length > 0)
			{
				if (this._groups == null)
				{
					this._groups = new Hashtable();
				}
				for (int i = 0; i < groupNames.Length; i++)
				{
					this._groups[groupNames[i]] = match.Groups[i].Value;
				}
			}
			return true;
		}

		// Token: 0x040019B4 RID: 6580
		internal static readonly Regex RefPat = new BrowserCapsRefRegex();

		// Token: 0x040019B5 RID: 6581
		private Hashtable _groups;

		// Token: 0x040019B6 RID: 6582
		private HttpBrowserCapabilities _browserCaps;
	}
}

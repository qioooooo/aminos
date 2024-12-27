using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000187 RID: 391
	internal sealed class PrefixQName
	{
		// Token: 0x06001062 RID: 4194 RVA: 0x0004FAC0 File Offset: 0x0004EAC0
		internal void ClearPrefix()
		{
			this.Prefix = string.Empty;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0004FACD File Offset: 0x0004EACD
		internal void SetQName(string qname)
		{
			PrefixQName.ParseQualifiedName(qname, out this.Prefix, out this.Name);
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0004FAE4 File Offset: 0x0004EAE4
		private static string ParseNCName(string qname, ref int position)
		{
			int length = qname.Length;
			int num = position;
			XmlCharType instance = XmlCharType.Instance;
			if (length == position || !instance.IsStartNCNameChar(qname[position]))
			{
				throw XsltException.Create("Xslt_InvalidQName", new string[] { qname });
			}
			position++;
			while (position < length && instance.IsNCNameChar(qname[position]))
			{
				position++;
			}
			return qname.Substring(num, position - num);
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0004FB60 File Offset: 0x0004EB60
		public static void ParseQualifiedName(string qname, out string prefix, out string local)
		{
			prefix = string.Empty;
			local = string.Empty;
			int num = 0;
			local = PrefixQName.ParseNCName(qname, ref num);
			if (num < qname.Length)
			{
				if (qname[num] == ':')
				{
					num++;
					prefix = local;
					local = PrefixQName.ParseNCName(qname, ref num);
				}
				if (num < qname.Length)
				{
					throw XsltException.Create("Xslt_InvalidQName", new string[] { qname });
				}
			}
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0004FBD0 File Offset: 0x0004EBD0
		public static bool ValidatePrefix(string prefix)
		{
			if (prefix.Length == 0)
			{
				return false;
			}
			XmlCharType instance = XmlCharType.Instance;
			if (!instance.IsStartNCNameChar(prefix[0]))
			{
				return false;
			}
			for (int i = 1; i < prefix.Length; i++)
			{
				if (!instance.IsNCNameChar(prefix[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000AFC RID: 2812
		public string Prefix;

		// Token: 0x04000AFD RID: 2813
		public string Name;

		// Token: 0x04000AFE RID: 2814
		public string Namespace;
	}
}

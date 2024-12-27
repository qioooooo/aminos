using System;
using System.Collections;
using System.Globalization;

namespace System.Security.Util
{
	// Token: 0x02000470 RID: 1136
	[Serializable]
	internal class SiteString
	{
		// Token: 0x06002D88 RID: 11656 RVA: 0x00098F4C File Offset: 0x00097F4C
		protected internal SiteString()
		{
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x00098F54 File Offset: 0x00097F54
		public SiteString(string site)
		{
			this.m_separatedSite = SiteString.CreateSeparatedSite(site);
			this.m_site = site;
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x00098F6F File Offset: 0x00097F6F
		private SiteString(string site, ArrayList separatedSite)
		{
			this.m_separatedSite = separatedSite;
			this.m_site = site;
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x00098F88 File Offset: 0x00097F88
		private static ArrayList CreateSeparatedSite(string site)
		{
			ArrayList arrayList = new ArrayList();
			if (site == null || site.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSite"));
			}
			int num = -1;
			int num2 = site.IndexOf('[');
			if (num2 == 0)
			{
				num = site.IndexOf(']', num2 + 1);
			}
			if (num != -1)
			{
				string text = site.Substring(num2 + 1, num - num2 - 1);
				arrayList.Add(text);
				return arrayList;
			}
			string[] array = site.Split(SiteString.m_separators);
			for (int i = array.Length - 1; i > -1; i--)
			{
				if (array[i] == null)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSite"));
				}
				if (array[i].Equals(""))
				{
					if (i != array.Length - 1)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSite"));
					}
				}
				else if (array[i].Equals("*"))
				{
					if (i != 0)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSite"));
					}
					arrayList.Add(array[i]);
				}
				else
				{
					if (!SiteString.AllLegalCharacters(array[i]))
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSite"));
					}
					arrayList.Add(array[i]);
				}
			}
			return arrayList;
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000990B8 File Offset: 0x000980B8
		private static bool AllLegalCharacters(string str)
		{
			foreach (char c in str)
			{
				if (!SiteString.IsLegalDNSChar(c) && !SiteString.IsNetbiosSplChar(c))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x000990F1 File Offset: 0x000980F1
		private static bool IsLegalDNSChar(char c)
		{
			return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '-';
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x0009911C File Offset: 0x0009811C
		private static bool IsNetbiosSplChar(char c)
		{
			if (c <= '@')
			{
				switch (c)
				{
				case '!':
				case '#':
				case '$':
				case '%':
				case '&':
				case '\'':
				case '(':
				case ')':
				case '-':
				case '.':
					break;
				case '"':
				case '*':
				case '+':
				case ',':
					return false;
				default:
					if (c != '@')
					{
						return false;
					}
					break;
				}
			}
			else
			{
				switch (c)
				{
				case '^':
				case '_':
					break;
				default:
					switch (c)
					{
					case '{':
					case '}':
					case '~':
						break;
					case '|':
						return false;
					default:
						return false;
					}
					break;
				}
			}
			return true;
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x000991A7 File Offset: 0x000981A7
		public override string ToString()
		{
			return this.m_site;
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x000991AF File Offset: 0x000981AF
		public override bool Equals(object o)
		{
			return o != null && o is SiteString && this.Equals((SiteString)o, true);
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x000991CC File Offset: 0x000981CC
		public override int GetHashCode()
		{
			TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
			return textInfo.GetCaseInsensitiveHashCode(this.m_site);
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000991F0 File Offset: 0x000981F0
		internal bool Equals(SiteString ss, bool ignoreCase)
		{
			if (this.m_site == null)
			{
				return ss.m_site == null;
			}
			return ss.m_site != null && this.IsSubsetOf(ss, ignoreCase) && ss.IsSubsetOf(this, ignoreCase);
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x00099222 File Offset: 0x00098222
		public virtual SiteString Copy()
		{
			return new SiteString(this.m_site, this.m_separatedSite);
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x00099235 File Offset: 0x00098235
		public virtual bool IsSubsetOf(SiteString operand)
		{
			return this.IsSubsetOf(operand, true);
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x00099240 File Offset: 0x00098240
		public virtual bool IsSubsetOf(SiteString operand, bool ignoreCase)
		{
			StringComparison stringComparison = (ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
			if (operand == null)
			{
				return false;
			}
			if (this.m_separatedSite.Count == operand.m_separatedSite.Count && this.m_separatedSite.Count == 0)
			{
				return true;
			}
			if (this.m_separatedSite.Count < operand.m_separatedSite.Count - 1)
			{
				return false;
			}
			if (this.m_separatedSite.Count > operand.m_separatedSite.Count && operand.m_separatedSite.Count > 0 && !operand.m_separatedSite[operand.m_separatedSite.Count - 1].Equals("*"))
			{
				return false;
			}
			if (string.Compare(this.m_site, operand.m_site, stringComparison) == 0)
			{
				return true;
			}
			for (int i = 0; i < operand.m_separatedSite.Count - 1; i++)
			{
				if (string.Compare((string)this.m_separatedSite[i], (string)operand.m_separatedSite[i], stringComparison) != 0)
				{
					return false;
				}
			}
			if (this.m_separatedSite.Count < operand.m_separatedSite.Count)
			{
				return operand.m_separatedSite[operand.m_separatedSite.Count - 1].Equals("*");
			}
			return this.m_separatedSite.Count != operand.m_separatedSite.Count || string.Compare((string)this.m_separatedSite[this.m_separatedSite.Count - 1], (string)operand.m_separatedSite[this.m_separatedSite.Count - 1], stringComparison) == 0 || operand.m_separatedSite[operand.m_separatedSite.Count - 1].Equals("*");
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x000993FE File Offset: 0x000983FE
		public virtual SiteString Intersect(SiteString operand)
		{
			if (operand == null)
			{
				return null;
			}
			if (this.IsSubsetOf(operand))
			{
				return this.Copy();
			}
			if (operand.IsSubsetOf(this))
			{
				return operand.Copy();
			}
			return null;
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x00099426 File Offset: 0x00098426
		public virtual SiteString Union(SiteString operand)
		{
			if (operand == null)
			{
				return this;
			}
			if (this.IsSubsetOf(operand))
			{
				return operand.Copy();
			}
			if (operand.IsSubsetOf(this))
			{
				return this.Copy();
			}
			return null;
		}

		// Token: 0x04001753 RID: 5971
		protected string m_site;

		// Token: 0x04001754 RID: 5972
		protected ArrayList m_separatedSite;

		// Token: 0x04001755 RID: 5973
		protected static char[] m_separators = new char[] { '.' };
	}
}

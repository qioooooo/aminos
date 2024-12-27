using System;
using System.Collections;

namespace System.Security.Util
{
	// Token: 0x02000475 RID: 1141
	[Serializable]
	internal class DirectoryString : SiteString
	{
		// Token: 0x06002DF3 RID: 11763 RVA: 0x0009B94C File Offset: 0x0009A94C
		public DirectoryString()
		{
			this.m_site = "";
			this.m_separatedSite = new ArrayList();
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x0009B96A File Offset: 0x0009A96A
		public DirectoryString(string directory, bool checkForIllegalChars)
		{
			this.m_site = directory;
			this.m_checkForIllegalChars = checkForIllegalChars;
			this.m_separatedSite = this.CreateSeparatedString(directory);
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x0009B990 File Offset: 0x0009A990
		private ArrayList CreateSeparatedString(string directory)
		{
			ArrayList arrayList = new ArrayList();
			if (directory == null || directory.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDirectoryOnUrl"));
			}
			string[] array = directory.Split(DirectoryString.m_separators);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && !array[i].Equals(""))
				{
					if (array[i].Equals("*"))
					{
						if (i != array.Length - 1)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDirectoryOnUrl"));
						}
						arrayList.Add(array[i]);
					}
					else
					{
						if (this.m_checkForIllegalChars && array[i].IndexOfAny(DirectoryString.m_illegalDirectoryCharacters) != -1)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDirectoryOnUrl"));
						}
						arrayList.Add(array[i]);
					}
				}
			}
			return arrayList;
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x0009BA55 File Offset: 0x0009AA55
		public virtual bool IsSubsetOf(DirectoryString operand)
		{
			return this.IsSubsetOf(operand, true);
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x0009BA60 File Offset: 0x0009AA60
		public virtual bool IsSubsetOf(DirectoryString operand, bool ignoreCase)
		{
			if (operand == null)
			{
				return false;
			}
			if (operand.m_separatedSite.Count == 0)
			{
				return this.m_separatedSite.Count == 0 || (this.m_separatedSite.Count > 0 && string.Compare((string)this.m_separatedSite[0], "*", StringComparison.Ordinal) == 0);
			}
			if (this.m_separatedSite.Count == 0)
			{
				return string.Compare((string)operand.m_separatedSite[0], "*", StringComparison.Ordinal) == 0;
			}
			return base.IsSubsetOf(operand, ignoreCase);
		}

		// Token: 0x04001774 RID: 6004
		private bool m_checkForIllegalChars;

		// Token: 0x04001775 RID: 6005
		private new static char[] m_separators = new char[] { '/' };

		// Token: 0x04001776 RID: 6006
		protected static char[] m_illegalDirectoryCharacters = new char[] { '\\', ':', '*', '?', '"', '<', '>', '|' };
	}
}

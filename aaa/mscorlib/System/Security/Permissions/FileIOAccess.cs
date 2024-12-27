using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Util;

namespace System.Security.Permissions
{
	// Token: 0x02000618 RID: 1560
	[Serializable]
	internal sealed class FileIOAccess
	{
		// Token: 0x060038BF RID: 14527 RVA: 0x000C08D8 File Offset: 0x000BF8D8
		public FileIOAccess()
		{
			this.m_set = new StringExpressionSet(this.m_ignoreCase, true);
			this.m_allFiles = false;
			this.m_allLocalFiles = false;
			this.m_pathDiscovery = false;
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x000C090E File Offset: 0x000BF90E
		public FileIOAccess(bool pathDiscovery)
		{
			this.m_set = new StringExpressionSet(this.m_ignoreCase, true);
			this.m_allFiles = false;
			this.m_allLocalFiles = false;
			this.m_pathDiscovery = pathDiscovery;
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x000C0944 File Offset: 0x000BF944
		public FileIOAccess(string value)
		{
			if (value == null)
			{
				this.m_set = new StringExpressionSet(this.m_ignoreCase, true);
				this.m_allFiles = false;
				this.m_allLocalFiles = false;
			}
			else if (value.Length >= "*AllFiles*".Length && string.Compare("*AllFiles*", value, StringComparison.Ordinal) == 0)
			{
				this.m_set = new StringExpressionSet(this.m_ignoreCase, true);
				this.m_allFiles = true;
				this.m_allLocalFiles = false;
			}
			else if (value.Length >= "*AllLocalFiles*".Length && string.Compare("*AllLocalFiles*", 0, value, 0, "*AllLocalFiles*".Length, StringComparison.Ordinal) == 0)
			{
				this.m_set = new StringExpressionSet(this.m_ignoreCase, value.Substring("*AllLocalFiles*".Length), true);
				this.m_allFiles = false;
				this.m_allLocalFiles = true;
			}
			else
			{
				this.m_set = new StringExpressionSet(this.m_ignoreCase, value, true);
				this.m_allFiles = false;
				this.m_allLocalFiles = false;
			}
			this.m_pathDiscovery = false;
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x000C0A4E File Offset: 0x000BFA4E
		public FileIOAccess(bool allFiles, bool allLocalFiles, bool pathDiscovery)
		{
			this.m_set = new StringExpressionSet(this.m_ignoreCase, true);
			this.m_allFiles = allFiles;
			this.m_allLocalFiles = allLocalFiles;
			this.m_pathDiscovery = pathDiscovery;
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x000C0A84 File Offset: 0x000BFA84
		public FileIOAccess(StringExpressionSet set, bool allFiles, bool allLocalFiles, bool pathDiscovery)
		{
			this.m_set = set;
			this.m_set.SetThrowOnRelative(true);
			this.m_allFiles = allFiles;
			this.m_allLocalFiles = allLocalFiles;
			this.m_pathDiscovery = pathDiscovery;
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x000C0ABC File Offset: 0x000BFABC
		private FileIOAccess(FileIOAccess operand)
		{
			this.m_set = operand.m_set.Copy();
			this.m_allFiles = operand.m_allFiles;
			this.m_allLocalFiles = operand.m_allLocalFiles;
			this.m_pathDiscovery = operand.m_pathDiscovery;
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x000C0B0B File Offset: 0x000BFB0B
		public void AddExpressions(ArrayList values, bool checkForDuplicates)
		{
			this.m_allFiles = false;
			this.m_set.AddExpressions(values, checkForDuplicates);
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x060038C6 RID: 14534 RVA: 0x000C0B21 File Offset: 0x000BFB21
		// (set) Token: 0x060038C7 RID: 14535 RVA: 0x000C0B29 File Offset: 0x000BFB29
		public bool AllFiles
		{
			get
			{
				return this.m_allFiles;
			}
			set
			{
				this.m_allFiles = value;
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x060038C8 RID: 14536 RVA: 0x000C0B32 File Offset: 0x000BFB32
		// (set) Token: 0x060038C9 RID: 14537 RVA: 0x000C0B3A File Offset: 0x000BFB3A
		public bool AllLocalFiles
		{
			get
			{
				return this.m_allLocalFiles;
			}
			set
			{
				this.m_allLocalFiles = value;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (set) Token: 0x060038CA RID: 14538 RVA: 0x000C0B43 File Offset: 0x000BFB43
		public bool PathDiscovery
		{
			set
			{
				this.m_pathDiscovery = value;
			}
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x000C0B4C File Offset: 0x000BFB4C
		public bool IsEmpty()
		{
			return !this.m_allFiles && !this.m_allLocalFiles && (this.m_set == null || this.m_set.IsEmpty());
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x000C0B75 File Offset: 0x000BFB75
		public FileIOAccess Copy()
		{
			return new FileIOAccess(this);
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x000C0B80 File Offset: 0x000BFB80
		public FileIOAccess Union(FileIOAccess operand)
		{
			if (operand == null)
			{
				if (!this.IsEmpty())
				{
					return this.Copy();
				}
				return null;
			}
			else
			{
				if (this.m_allFiles || operand.m_allFiles)
				{
					return new FileIOAccess(true, false, this.m_pathDiscovery);
				}
				return new FileIOAccess(this.m_set.Union(operand.m_set), false, this.m_allLocalFiles || operand.m_allLocalFiles, this.m_pathDiscovery);
			}
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x000C0BF0 File Offset: 0x000BFBF0
		public FileIOAccess Intersect(FileIOAccess operand)
		{
			if (operand == null)
			{
				return null;
			}
			if (this.m_allFiles)
			{
				if (operand.m_allFiles)
				{
					return new FileIOAccess(true, false, this.m_pathDiscovery);
				}
				return new FileIOAccess(operand.m_set.Copy(), false, operand.m_allLocalFiles, this.m_pathDiscovery);
			}
			else
			{
				if (operand.m_allFiles)
				{
					return new FileIOAccess(this.m_set.Copy(), false, this.m_allLocalFiles, this.m_pathDiscovery);
				}
				StringExpressionSet stringExpressionSet = new StringExpressionSet(this.m_ignoreCase, true);
				if (this.m_allLocalFiles)
				{
					string[] array = operand.m_set.ToStringArray();
					if (array != null)
					{
						for (int i = 0; i < array.Length; i++)
						{
							string root = FileIOAccess.GetRoot(array[i]);
							if (root != null && FileIOAccess._LocalDrive(FileIOAccess.GetRoot(root)))
							{
								stringExpressionSet.AddExpressions(new string[] { array[i] }, true, false);
							}
						}
					}
				}
				if (operand.m_allLocalFiles)
				{
					string[] array2 = this.m_set.ToStringArray();
					if (array2 != null)
					{
						for (int j = 0; j < array2.Length; j++)
						{
							string root2 = FileIOAccess.GetRoot(array2[j]);
							if (root2 != null && FileIOAccess._LocalDrive(FileIOAccess.GetRoot(root2)))
							{
								stringExpressionSet.AddExpressions(new string[] { array2[j] }, true, false);
							}
						}
					}
				}
				string[] array3 = this.m_set.Intersect(operand.m_set).ToStringArray();
				if (array3 != null)
				{
					stringExpressionSet.AddExpressions(array3, !stringExpressionSet.IsEmpty(), false);
				}
				return new FileIOAccess(stringExpressionSet, false, this.m_allLocalFiles && operand.m_allLocalFiles, this.m_pathDiscovery);
			}
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x000C0D7C File Offset: 0x000BFD7C
		public bool IsSubsetOf(FileIOAccess operand)
		{
			if (operand == null)
			{
				return this.IsEmpty();
			}
			if (operand.m_allFiles)
			{
				return true;
			}
			if ((!this.m_pathDiscovery || !this.m_set.IsSubsetOfPathDiscovery(operand.m_set)) && !this.m_set.IsSubsetOf(operand.m_set))
			{
				if (!operand.m_allLocalFiles)
				{
					return false;
				}
				string[] array = this.m_set.ToStringArray();
				for (int i = 0; i < array.Length; i++)
				{
					string root = FileIOAccess.GetRoot(array[i]);
					if (root == null || !FileIOAccess._LocalDrive(FileIOAccess.GetRoot(root)))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x000C0E10 File Offset: 0x000BFE10
		private static string GetRoot(string path)
		{
			string text = path.Substring(0, 3);
			if (text.EndsWith(":\\", StringComparison.Ordinal))
			{
				return text;
			}
			return null;
		}

		// Token: 0x060038D1 RID: 14545 RVA: 0x000C0E38 File Offset: 0x000BFE38
		public override string ToString()
		{
			if (this.m_allFiles)
			{
				return "*AllFiles*";
			}
			if (this.m_allLocalFiles)
			{
				string text = "*AllLocalFiles*";
				string text2 = this.m_set.ToString();
				if (text2 != null && text2.Length > 0)
				{
					text = text + ";" + text2;
				}
				return text;
			}
			return this.m_set.ToString();
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x000C0E93 File Offset: 0x000BFE93
		public string[] ToStringArray()
		{
			return this.m_set.ToStringArray();
		}

		// Token: 0x060038D3 RID: 14547
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _LocalDrive(string path);

		// Token: 0x060038D4 RID: 14548 RVA: 0x000C0EA0 File Offset: 0x000BFEA0
		public override bool Equals(object obj)
		{
			FileIOAccess fileIOAccess = obj as FileIOAccess;
			if (fileIOAccess == null)
			{
				return this.IsEmpty() && obj == null;
			}
			if (this.m_pathDiscovery)
			{
				return (this.m_allFiles && fileIOAccess.m_allFiles) || (this.m_allLocalFiles == fileIOAccess.m_allLocalFiles && this.m_set.IsSubsetOf(fileIOAccess.m_set) && fileIOAccess.m_set.IsSubsetOf(this.m_set));
			}
			return this.IsSubsetOf(fileIOAccess) && fileIOAccess.IsSubsetOf(this);
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x000C0F2F File Offset: 0x000BFF2F
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04001D4A RID: 7498
		private const string m_strAllFiles = "*AllFiles*";

		// Token: 0x04001D4B RID: 7499
		private const string m_strAllLocalFiles = "*AllLocalFiles*";

		// Token: 0x04001D4C RID: 7500
		private bool m_ignoreCase = true;

		// Token: 0x04001D4D RID: 7501
		private StringExpressionSet m_set;

		// Token: 0x04001D4E RID: 7502
		private bool m_allFiles;

		// Token: 0x04001D4F RID: 7503
		private bool m_allLocalFiles;

		// Token: 0x04001D50 RID: 7504
		private bool m_pathDiscovery;
	}
}

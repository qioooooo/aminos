using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Security.Util
{
	// Token: 0x02000471 RID: 1137
	[Serializable]
	internal class StringExpressionSet
	{
		// Token: 0x06002D99 RID: 11673 RVA: 0x0009946F File Offset: 0x0009846F
		public StringExpressionSet()
			: this(true, null, false)
		{
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x0009947A File Offset: 0x0009847A
		public StringExpressionSet(string str)
			: this(true, str, false)
		{
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x00099485 File Offset: 0x00098485
		public StringExpressionSet(bool ignoreCase, bool throwOnRelative)
			: this(ignoreCase, null, throwOnRelative)
		{
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x00099490 File Offset: 0x00098490
		public StringExpressionSet(bool ignoreCase, string str, bool throwOnRelative)
		{
			this.m_list = null;
			this.m_ignoreCase = ignoreCase;
			this.m_throwOnRelative = throwOnRelative;
			if (str == null)
			{
				this.m_expressions = null;
				return;
			}
			this.AddExpressions(str);
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x000994BF File Offset: 0x000984BF
		protected virtual StringExpressionSet CreateNewEmpty()
		{
			return new StringExpressionSet();
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x000994C8 File Offset: 0x000984C8
		public virtual StringExpressionSet Copy()
		{
			StringExpressionSet stringExpressionSet = this.CreateNewEmpty();
			if (this.m_list != null)
			{
				stringExpressionSet.m_list = new ArrayList(this.m_list);
			}
			stringExpressionSet.m_expressions = this.m_expressions;
			stringExpressionSet.m_ignoreCase = this.m_ignoreCase;
			stringExpressionSet.m_throwOnRelative = this.m_throwOnRelative;
			return stringExpressionSet;
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x0009951A File Offset: 0x0009851A
		public void SetThrowOnRelative(bool throwOnRelative)
		{
			this.m_throwOnRelative = throwOnRelative;
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x00099523 File Offset: 0x00098523
		private static string StaticProcessWholeString(string str)
		{
			return str.Replace(StringExpressionSet.m_alternateDirectorySeparator, StringExpressionSet.m_directorySeparator);
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x00099535 File Offset: 0x00098535
		private static string StaticProcessSingleString(string str)
		{
			return str.Trim(StringExpressionSet.m_trimChars);
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x00099542 File Offset: 0x00098542
		protected virtual string ProcessWholeString(string str)
		{
			return StringExpressionSet.StaticProcessWholeString(str);
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x0009954A File Offset: 0x0009854A
		protected virtual string ProcessSingleString(string str)
		{
			return StringExpressionSet.StaticProcessSingleString(str);
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x00099554 File Offset: 0x00098554
		public void AddExpressions(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (str.Length == 0)
			{
				return;
			}
			str = this.ProcessWholeString(str);
			if (this.m_expressions == null)
			{
				this.m_expressions = str;
			}
			else
			{
				this.m_expressions = this.m_expressions + StringExpressionSet.m_separators[0] + str;
			}
			this.m_expressionsArray = null;
			string[] array = this.Split(str);
			if (this.m_list == null)
			{
				this.m_list = new ArrayList();
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && !array[i].Equals(""))
				{
					string text = this.ProcessSingleString(array[i]);
					int num = text.IndexOf('\0');
					if (num != -1)
					{
						text = text.Substring(0, num);
					}
					if (text != null && !text.Equals(""))
					{
						if (this.m_throwOnRelative)
						{
							if ((text.Length < 3 || text[1] != ':' || text[2] != '\\' || ((text[0] < 'a' || text[0] > 'z') && (text[0] < 'A' || text[0] > 'Z'))) && (text.Length < 2 || text[0] != '\\' || text[1] != '\\'))
							{
								throw new ArgumentException(Environment.GetResourceString("Argument_AbsolutePathRequired"));
							}
							text = StringExpressionSet.CanonicalizePath(text);
						}
						this.m_list.Add(text);
					}
				}
			}
			this.Reduce();
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x000996D4 File Offset: 0x000986D4
		public void AddExpressions(string[] str, bool checkForDuplicates, bool needFullPath)
		{
			this.AddExpressions(StringExpressionSet.CreateListFromExpressions(str, needFullPath), checkForDuplicates);
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000996E4 File Offset: 0x000986E4
		public void AddExpressions(ArrayList exprArrayList, bool checkForDuplicates)
		{
			this.m_expressionsArray = null;
			this.m_expressions = null;
			if (this.m_list != null)
			{
				this.m_list.AddRange(exprArrayList);
			}
			else
			{
				this.m_list = new ArrayList(exprArrayList);
			}
			if (checkForDuplicates)
			{
				this.Reduce();
			}
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x00099720 File Offset: 0x00098720
		internal static ArrayList CreateListFromExpressions(string[] str, bool needFullPath)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == null)
				{
					throw new ArgumentNullException("str");
				}
				string text = StringExpressionSet.StaticProcessWholeString(str[i]);
				if (text != null && text.Length != 0)
				{
					string text2 = StringExpressionSet.StaticProcessSingleString(text);
					int num = text2.IndexOf('\0');
					if (num != -1)
					{
						text2 = text2.Substring(0, num);
					}
					if (text2 != null && text2.Length != 0)
					{
						if ((text2.Length < 3 || text2[1] != ':' || text2[2] != '\\' || ((text2[0] < 'a' || text2[0] > 'z') && (text2[0] < 'A' || text2[0] > 'Z'))) && (text2.Length < 2 || text2[0] != '\\' || text2[1] != '\\'))
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_AbsolutePathRequired"));
						}
						text2 = StringExpressionSet.CanonicalizePath(text2, needFullPath);
						arrayList.Add(text2);
					}
				}
			}
			return arrayList;
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x0009983A File Offset: 0x0009883A
		protected void CheckList()
		{
			if (this.m_list == null && this.m_expressions != null)
			{
				this.CreateList();
			}
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x00099854 File Offset: 0x00098854
		protected string[] Split(string expressions)
		{
			if (this.m_throwOnRelative)
			{
				ArrayList arrayList = new ArrayList();
				string[] array = expressions.Split(new char[] { '"' });
				for (int i = 0; i < array.Length; i++)
				{
					if (i % 2 == 0)
					{
						string[] array2 = array[i].Split(new char[] { ';' });
						for (int j = 0; j < array2.Length; j++)
						{
							if (array2[j] != null && !array2[j].Equals(""))
							{
								arrayList.Add(array2[j]);
							}
						}
					}
					else
					{
						arrayList.Add(array[i]);
					}
				}
				string[] array3 = new string[arrayList.Count];
				IEnumerator enumerator = arrayList.GetEnumerator();
				int num = 0;
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					array3[num++] = (string)obj;
				}
				return array3;
			}
			return expressions.Split(StringExpressionSet.m_separators);
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x0009993C File Offset: 0x0009893C
		protected void CreateList()
		{
			string[] array = this.Split(this.m_expressions);
			this.m_list = new ArrayList();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && !array[i].Equals(""))
				{
					string text = this.ProcessSingleString(array[i]);
					int num = text.IndexOf('\0');
					if (num != -1)
					{
						text = text.Substring(0, num);
					}
					if (text != null && !text.Equals(""))
					{
						if (this.m_throwOnRelative)
						{
							if ((text.Length < 3 || text[1] != ':' || text[2] != '\\' || ((text[0] < 'a' || text[0] > 'z') && (text[0] < 'A' || text[0] > 'Z'))) && (text.Length < 2 || text[0] != '\\' || text[1] != '\\'))
							{
								throw new ArgumentException(Environment.GetResourceString("Argument_AbsolutePathRequired"));
							}
							text = StringExpressionSet.CanonicalizePath(text);
						}
						this.m_list.Add(text);
					}
				}
			}
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x00099A5D File Offset: 0x00098A5D
		public bool IsEmpty()
		{
			if (this.m_list == null)
			{
				return this.m_expressions == null;
			}
			return this.m_list.Count == 0;
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x00099A80 File Offset: 0x00098A80
		public bool IsSubsetOf(StringExpressionSet ses)
		{
			if (this.IsEmpty())
			{
				return true;
			}
			if (ses == null || ses.IsEmpty())
			{
				return false;
			}
			this.CheckList();
			ses.CheckList();
			for (int i = 0; i < this.m_list.Count; i++)
			{
				if (!this.StringSubsetStringExpression((string)this.m_list[i], ses, this.m_ignoreCase))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x00099AEC File Offset: 0x00098AEC
		public bool IsSubsetOfPathDiscovery(StringExpressionSet ses)
		{
			if (this.IsEmpty())
			{
				return true;
			}
			if (ses == null || ses.IsEmpty())
			{
				return false;
			}
			this.CheckList();
			ses.CheckList();
			for (int i = 0; i < this.m_list.Count; i++)
			{
				if (!StringExpressionSet.StringSubsetStringExpressionPathDiscovery((string)this.m_list[i], ses, this.m_ignoreCase))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x00099B54 File Offset: 0x00098B54
		public StringExpressionSet Union(StringExpressionSet ses)
		{
			if (ses == null || ses.IsEmpty())
			{
				return this.Copy();
			}
			if (this.IsEmpty())
			{
				return ses.Copy();
			}
			this.CheckList();
			ses.CheckList();
			StringExpressionSet stringExpressionSet = ((ses.m_list.Count > this.m_list.Count) ? ses : this);
			StringExpressionSet stringExpressionSet2 = ((ses.m_list.Count <= this.m_list.Count) ? ses : this);
			StringExpressionSet stringExpressionSet3 = stringExpressionSet.Copy();
			stringExpressionSet3.Reduce();
			for (int i = 0; i < stringExpressionSet2.m_list.Count; i++)
			{
				stringExpressionSet3.AddSingleExpressionNoDuplicates((string)stringExpressionSet2.m_list[i]);
			}
			stringExpressionSet3.GenerateString();
			return stringExpressionSet3;
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x00099C0C File Offset: 0x00098C0C
		public StringExpressionSet Intersect(StringExpressionSet ses)
		{
			if (this.IsEmpty() || ses == null || ses.IsEmpty())
			{
				return this.CreateNewEmpty();
			}
			this.CheckList();
			ses.CheckList();
			StringExpressionSet stringExpressionSet = this.CreateNewEmpty();
			for (int i = 0; i < this.m_list.Count; i++)
			{
				for (int j = 0; j < ses.m_list.Count; j++)
				{
					if (this.StringSubsetString((string)this.m_list[i], (string)ses.m_list[j], this.m_ignoreCase))
					{
						if (stringExpressionSet.m_list == null)
						{
							stringExpressionSet.m_list = new ArrayList();
						}
						stringExpressionSet.AddSingleExpressionNoDuplicates((string)this.m_list[i]);
					}
					else if (this.StringSubsetString((string)ses.m_list[j], (string)this.m_list[i], this.m_ignoreCase))
					{
						if (stringExpressionSet.m_list == null)
						{
							stringExpressionSet.m_list = new ArrayList();
						}
						stringExpressionSet.AddSingleExpressionNoDuplicates((string)ses.m_list[j]);
					}
				}
			}
			stringExpressionSet.GenerateString();
			return stringExpressionSet;
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x00099D3C File Offset: 0x00098D3C
		protected void GenerateString()
		{
			if (this.m_list != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				IEnumerator enumerator = this.m_list.GetEnumerator();
				bool flag = true;
				while (enumerator.MoveNext())
				{
					if (!flag)
					{
						stringBuilder.Append(StringExpressionSet.m_separators[0]);
					}
					else
					{
						flag = false;
					}
					string text = (string)enumerator.Current;
					if (text != null)
					{
						int num = text.IndexOf(StringExpressionSet.m_separators[0]);
						if (num != -1)
						{
							stringBuilder.Append('"');
						}
						stringBuilder.Append(text);
						if (num != -1)
						{
							stringBuilder.Append('"');
						}
					}
				}
				this.m_expressions = stringBuilder.ToString();
				return;
			}
			this.m_expressions = null;
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x00099DDD File Offset: 0x00098DDD
		public override string ToString()
		{
			this.CheckList();
			this.Reduce();
			this.GenerateString();
			return this.m_expressions;
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x00099DF7 File Offset: 0x00098DF7
		public string[] ToStringArray()
		{
			if (this.m_expressionsArray == null && this.m_list != null)
			{
				this.m_expressionsArray = (string[])this.m_list.ToArray(typeof(string));
			}
			return this.m_expressionsArray;
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x00099E30 File Offset: 0x00098E30
		protected bool StringSubsetStringExpression(string left, StringExpressionSet right, bool ignoreCase)
		{
			for (int i = 0; i < right.m_list.Count; i++)
			{
				if (this.StringSubsetString(left, (string)right.m_list[i], ignoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x00099E74 File Offset: 0x00098E74
		protected static bool StringSubsetStringExpressionPathDiscovery(string left, StringExpressionSet right, bool ignoreCase)
		{
			for (int i = 0; i < right.m_list.Count; i++)
			{
				if (StringExpressionSet.StringSubsetStringPathDiscovery(left, (string)right.m_list[i], ignoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x00099EB4 File Offset: 0x00098EB4
		protected virtual bool StringSubsetString(string left, string right, bool ignoreCase)
		{
			StringComparison stringComparison = (ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
			if (right == null || left == null || right.Length == 0 || left.Length == 0 || right.Length > left.Length)
			{
				return false;
			}
			if (right.Length == left.Length)
			{
				return string.Compare(right, left, stringComparison) == 0;
			}
			if (left.Length - right.Length == 1 && left[left.Length - 1] == StringExpressionSet.m_directorySeparator)
			{
				return string.Compare(left, 0, right, 0, right.Length, stringComparison) == 0;
			}
			if (right[right.Length - 1] == StringExpressionSet.m_directorySeparator)
			{
				return string.Compare(right, 0, left, 0, right.Length, stringComparison) == 0;
			}
			return left[right.Length] == StringExpressionSet.m_directorySeparator && string.Compare(right, 0, left, 0, right.Length, stringComparison) == 0;
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x00099F94 File Offset: 0x00098F94
		protected static bool StringSubsetStringPathDiscovery(string left, string right, bool ignoreCase)
		{
			StringComparison stringComparison = (ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
			if (right == null || left == null || right.Length == 0 || left.Length == 0)
			{
				return false;
			}
			if (right.Length == left.Length)
			{
				return string.Compare(right, left, stringComparison) == 0;
			}
			string text;
			string text2;
			if (right.Length < left.Length)
			{
				text = right;
				text2 = left;
			}
			else
			{
				text = left;
				text2 = right;
			}
			return string.Compare(text, 0, text2, 0, text.Length, stringComparison) == 0 && ((text.Length == 3 && text.EndsWith(":\\", StringComparison.Ordinal) && ((text[0] >= 'A' && text[0] <= 'Z') || (text[0] >= 'a' && text[0] <= 'z'))) || text2[text.Length] == StringExpressionSet.m_directorySeparator);
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x0009A060 File Offset: 0x00099060
		protected void AddSingleExpressionNoDuplicates(string expression)
		{
			int i = 0;
			this.m_expressionsArray = null;
			this.m_expressions = null;
			if (this.m_list == null)
			{
				this.m_list = new ArrayList();
			}
			while (i < this.m_list.Count)
			{
				if (this.StringSubsetString((string)this.m_list[i], expression, this.m_ignoreCase))
				{
					this.m_list.RemoveAt(i);
				}
				else
				{
					if (this.StringSubsetString(expression, (string)this.m_list[i], this.m_ignoreCase))
					{
						return;
					}
					i++;
				}
			}
			this.m_list.Add(expression);
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x0009A100 File Offset: 0x00099100
		protected void Reduce()
		{
			this.CheckList();
			if (this.m_list == null)
			{
				return;
			}
			for (int i = 0; i < this.m_list.Count - 1; i++)
			{
				int j = i + 1;
				while (j < this.m_list.Count)
				{
					if (this.StringSubsetString((string)this.m_list[j], (string)this.m_list[i], this.m_ignoreCase))
					{
						this.m_list.RemoveAt(j);
					}
					else if (this.StringSubsetString((string)this.m_list[i], (string)this.m_list[j], this.m_ignoreCase))
					{
						this.m_list[i] = this.m_list[j];
						this.m_list.RemoveAt(j);
						j = i + 1;
					}
					else
					{
						j++;
					}
				}
			}
		}

		// Token: 0x06002DB9 RID: 11705
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetLongPathName(string path);

		// Token: 0x06002DBA RID: 11706 RVA: 0x0009A1F0 File Offset: 0x000991F0
		internal static string CanonicalizePath(string path)
		{
			return StringExpressionSet.CanonicalizePath(path, true);
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x0009A1FC File Offset: 0x000991FC
		internal static string CanonicalizePath(string path, bool needFullPath)
		{
			if (path.IndexOf('~') != -1)
			{
				path = StringExpressionSet.GetLongPathName(path);
			}
			if (path.IndexOf(':', 2) != -1)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_PathFormatNotSupported"));
			}
			if (needFullPath)
			{
				string text = Path.GetFullPathInternal(path);
				if (path.EndsWith("\\.", StringComparison.Ordinal))
				{
					if (text.EndsWith("\\", StringComparison.Ordinal))
					{
						text += ".";
					}
					else
					{
						text += "\\.";
					}
				}
				return text;
			}
			return path;
		}

		// Token: 0x04001756 RID: 5974
		protected ArrayList m_list;

		// Token: 0x04001757 RID: 5975
		protected bool m_ignoreCase;

		// Token: 0x04001758 RID: 5976
		protected string m_expressions;

		// Token: 0x04001759 RID: 5977
		protected string[] m_expressionsArray;

		// Token: 0x0400175A RID: 5978
		protected bool m_throwOnRelative;

		// Token: 0x0400175B RID: 5979
		protected static readonly char[] m_separators = new char[] { ';' };

		// Token: 0x0400175C RID: 5980
		protected static readonly char[] m_trimChars = new char[] { ' ' };

		// Token: 0x0400175D RID: 5981
		protected static readonly char m_directorySeparator = '\\';

		// Token: 0x0400175E RID: 5982
		protected static readonly char m_alternateDirectorySeparator = '/';
	}
}

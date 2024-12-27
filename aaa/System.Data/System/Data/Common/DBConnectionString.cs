using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace System.Data.Common
{
	// Token: 0x0200012C RID: 300
	[Serializable]
	internal sealed class DBConnectionString
	{
		// Token: 0x060013AA RID: 5034 RVA: 0x00223920 File Offset: 0x00222D20
		internal DBConnectionString(string value, string restrictions, KeyRestrictionBehavior behavior, Hashtable synonyms, bool useOdbcRules)
			: this(new DbConnectionOptions(value, synonyms, useOdbcRules), restrictions, behavior, synonyms, false)
		{
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00223944 File Offset: 0x00222D44
		internal DBConnectionString(DbConnectionOptions connectionOptions)
			: this(connectionOptions, null, KeyRestrictionBehavior.AllowOnly, null, true)
		{
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0022395C File Offset: 0x00222D5C
		private DBConnectionString(DbConnectionOptions connectionOptions, string restrictions, KeyRestrictionBehavior behavior, Hashtable synonyms, bool mustCloneDictionary)
		{
			switch (behavior)
			{
			case KeyRestrictionBehavior.AllowOnly:
			case KeyRestrictionBehavior.PreventUsage:
				this._behavior = behavior;
				this._encryptedUsersConnectionString = connectionOptions.UsersConnectionString(false);
				this._hasPassword = connectionOptions.HasPasswordKeyword;
				this._parsetable = connectionOptions.Parsetable;
				this._keychain = connectionOptions.KeyChain;
				if (this._hasPassword && !connectionOptions.HasPersistablePassword)
				{
					if (mustCloneDictionary)
					{
						this._parsetable = (Hashtable)this._parsetable.Clone();
					}
					if (this._parsetable.ContainsKey("password"))
					{
						this._parsetable["password"] = "*";
					}
					if (this._parsetable.ContainsKey("pwd"))
					{
						this._parsetable["pwd"] = "*";
					}
					this._keychain = connectionOptions.ReplacePasswordPwd(out this._encryptedUsersConnectionString, true);
				}
				if (!ADP.IsEmpty(restrictions))
				{
					this._restrictionValues = DBConnectionString.ParseRestrictions(restrictions, synonyms);
					this._restrictions = restrictions;
				}
				return;
			default:
				throw ADP.InvalidKeyRestrictionBehavior(behavior);
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00223A70 File Offset: 0x00222E70
		private DBConnectionString(DBConnectionString connectionString, string[] restrictionValues, KeyRestrictionBehavior behavior)
		{
			this._encryptedUsersConnectionString = connectionString._encryptedUsersConnectionString;
			this._parsetable = connectionString._parsetable;
			this._keychain = connectionString._keychain;
			this._hasPassword = connectionString._hasPassword;
			this._restrictionValues = restrictionValues;
			this._restrictions = null;
			this._behavior = behavior;
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060013AE RID: 5038 RVA: 0x00223AC8 File Offset: 0x00222EC8
		internal KeyRestrictionBehavior Behavior
		{
			get
			{
				return this._behavior;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060013AF RID: 5039 RVA: 0x00223ADC File Offset: 0x00222EDC
		internal string ConnectionString
		{
			get
			{
				return this._encryptedUsersConnectionString;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x00223AF0 File Offset: 0x00222EF0
		internal bool IsEmpty
		{
			get
			{
				return null == this._keychain;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x00223B08 File Offset: 0x00222F08
		internal NameValuePair KeyChain
		{
			get
			{
				return this._keychain;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060013B2 RID: 5042 RVA: 0x00223B1C File Offset: 0x00222F1C
		internal string Restrictions
		{
			get
			{
				string text = this._restrictions;
				if (text == null)
				{
					string[] restrictionValues = this._restrictionValues;
					if (restrictionValues != null && 0 < restrictionValues.Length)
					{
						StringBuilder stringBuilder = new StringBuilder();
						for (int i = 0; i < restrictionValues.Length; i++)
						{
							if (!ADP.IsEmpty(restrictionValues[i]))
							{
								stringBuilder.Append(restrictionValues[i]);
								stringBuilder.Append("=;");
							}
						}
						text = stringBuilder.ToString();
					}
				}
				if (text == null)
				{
					return "";
				}
				return text;
			}
		}

		// Token: 0x170002A7 RID: 679
		internal string this[string keyword]
		{
			get
			{
				return (string)this._parsetable[keyword];
			}
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00223BA8 File Offset: 0x00222FA8
		internal bool ContainsKey(string keyword)
		{
			return this._parsetable.ContainsKey(keyword);
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00223BC4 File Offset: 0x00222FC4
		internal DBConnectionString Intersect(DBConnectionString entry)
		{
			KeyRestrictionBehavior keyRestrictionBehavior = this._behavior;
			string[] array = null;
			if (entry == null)
			{
				keyRestrictionBehavior = KeyRestrictionBehavior.AllowOnly;
			}
			else if (this._behavior != entry._behavior)
			{
				keyRestrictionBehavior = KeyRestrictionBehavior.AllowOnly;
				if (entry._behavior == KeyRestrictionBehavior.AllowOnly)
				{
					if (!ADP.IsEmptyArray(this._restrictionValues))
					{
						if (!ADP.IsEmptyArray(entry._restrictionValues))
						{
							array = DBConnectionString.NewRestrictionAllowOnly(entry._restrictionValues, this._restrictionValues);
						}
					}
					else
					{
						array = entry._restrictionValues;
					}
				}
				else if (!ADP.IsEmptyArray(this._restrictionValues))
				{
					if (!ADP.IsEmptyArray(entry._restrictionValues))
					{
						array = DBConnectionString.NewRestrictionAllowOnly(this._restrictionValues, entry._restrictionValues);
					}
					else
					{
						array = this._restrictionValues;
					}
				}
			}
			else if (KeyRestrictionBehavior.PreventUsage == this._behavior)
			{
				if (ADP.IsEmptyArray(this._restrictionValues))
				{
					array = entry._restrictionValues;
				}
				else if (ADP.IsEmptyArray(entry._restrictionValues))
				{
					array = this._restrictionValues;
				}
				else
				{
					array = DBConnectionString.NoDuplicateUnion(this._restrictionValues, entry._restrictionValues);
				}
			}
			else if (!ADP.IsEmptyArray(this._restrictionValues) && !ADP.IsEmptyArray(entry._restrictionValues))
			{
				if (this._restrictionValues.Length <= entry._restrictionValues.Length)
				{
					array = DBConnectionString.NewRestrictionIntersect(this._restrictionValues, entry._restrictionValues);
				}
				else
				{
					array = DBConnectionString.NewRestrictionIntersect(entry._restrictionValues, this._restrictionValues);
				}
			}
			return new DBConnectionString(this, array, keyRestrictionBehavior);
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00223D28 File Offset: 0x00223128
		private bool IsRestrictedKeyword(string key)
		{
			return this._restrictionValues == null || 0 > Array.BinarySearch<string>(this._restrictionValues, key, StringComparer.Ordinal);
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00223D54 File Offset: 0x00223154
		internal bool IsSupersetOf(DBConnectionString entry)
		{
			switch (this._behavior)
			{
			case KeyRestrictionBehavior.AllowOnly:
			{
				for (NameValuePair nameValuePair = entry.KeyChain; nameValuePair != null; nameValuePair = nameValuePair.Next)
				{
					if (!this.ContainsKey(nameValuePair.Name) && this.IsRestrictedKeyword(nameValuePair.Name))
					{
						return false;
					}
				}
				break;
			}
			case KeyRestrictionBehavior.PreventUsage:
				if (this._restrictionValues != null)
				{
					foreach (string text in this._restrictionValues)
					{
						if (entry.ContainsKey(text))
						{
							return false;
						}
					}
				}
				break;
			default:
				throw ADP.InvalidKeyRestrictionBehavior(this._behavior);
			}
			return true;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00223DF0 File Offset: 0x002231F0
		private static string[] NewRestrictionAllowOnly(string[] allowonly, string[] preventusage)
		{
			List<string> list = null;
			for (int i = 0; i < allowonly.Length; i++)
			{
				if (0 > Array.BinarySearch<string>(preventusage, allowonly[i], StringComparer.Ordinal))
				{
					if (list == null)
					{
						list = new List<string>();
					}
					list.Add(allowonly[i]);
				}
			}
			string[] array = null;
			if (list != null)
			{
				array = list.ToArray();
			}
			return array;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00223E40 File Offset: 0x00223240
		private static string[] NewRestrictionIntersect(string[] a, string[] b)
		{
			List<string> list = null;
			for (int i = 0; i < a.Length; i++)
			{
				if (0 <= Array.BinarySearch<string>(b, a[i], StringComparer.Ordinal))
				{
					if (list == null)
					{
						list = new List<string>();
					}
					list.Add(a[i]);
				}
			}
			string[] array = null;
			if (list != null)
			{
				array = list.ToArray();
			}
			return array;
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x00223E90 File Offset: 0x00223290
		private static string[] NoDuplicateUnion(string[] a, string[] b)
		{
			List<string> list = new List<string>(a.Length + b.Length);
			for (int i = 0; i < a.Length; i++)
			{
				list.Add(a[i]);
			}
			for (int j = 0; j < b.Length; j++)
			{
				if (0 > Array.BinarySearch<string>(a, b[j], StringComparer.Ordinal))
				{
					list.Add(b[j]);
				}
			}
			string[] array = list.ToArray();
			Array.Sort<string>(array, StringComparer.Ordinal);
			return array;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00223EFC File Offset: 0x002232FC
		private static string[] ParseRestrictions(string restrictions, Hashtable synonyms)
		{
			List<string> list = new List<string>();
			StringBuilder stringBuilder = new StringBuilder(restrictions.Length);
			int i = 0;
			int length = restrictions.Length;
			while (i < length)
			{
				int num = i;
				string text;
				string text2;
				i = DbConnectionOptions.GetKeyValuePair(restrictions, num, stringBuilder, false, out text, out text2);
				if (!ADP.IsEmpty(text))
				{
					string text3 = ((synonyms != null) ? ((string)synonyms[text]) : text);
					if (ADP.IsEmpty(text3))
					{
						throw ADP.KeywordNotSupported(text);
					}
					list.Add(text3);
				}
			}
			return DBConnectionString.RemoveDuplicates(list.ToArray());
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x00223F7C File Offset: 0x0022337C
		internal static string[] RemoveDuplicates(string[] restrictions)
		{
			int num = restrictions.Length;
			if (0 < num)
			{
				Array.Sort<string>(restrictions, StringComparer.Ordinal);
				for (int i = 1; i < restrictions.Length; i++)
				{
					string text = restrictions[i - 1];
					if (text.Length == 0 || text == restrictions[i])
					{
						restrictions[i - 1] = null;
						num--;
					}
				}
				if (restrictions[restrictions.Length - 1].Length == 0)
				{
					restrictions[restrictions.Length - 1] = null;
					num--;
				}
				if (num != restrictions.Length)
				{
					string[] array = new string[num];
					num = 0;
					for (int j = 0; j < restrictions.Length; j++)
					{
						if (restrictions[j] != null)
						{
							array[num++] = restrictions[j];
						}
					}
					restrictions = array;
				}
			}
			return restrictions;
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00224020 File Offset: 0x00223420
		[Conditional("DEBUG")]
		private static void Verify(string[] restrictionValues)
		{
			if (restrictionValues != null)
			{
				for (int i = 1; i < restrictionValues.Length; i++)
				{
				}
			}
		}

		// Token: 0x04000C15 RID: 3093
		private readonly string _encryptedUsersConnectionString;

		// Token: 0x04000C16 RID: 3094
		private readonly Hashtable _parsetable;

		// Token: 0x04000C17 RID: 3095
		private readonly NameValuePair _keychain;

		// Token: 0x04000C18 RID: 3096
		private readonly bool _hasPassword;

		// Token: 0x04000C19 RID: 3097
		private readonly string[] _restrictionValues;

		// Token: 0x04000C1A RID: 3098
		private readonly string _restrictions;

		// Token: 0x04000C1B RID: 3099
		private readonly KeyRestrictionBehavior _behavior;

		// Token: 0x04000C1C RID: 3100
		private readonly string _encryptedActualConnectionString;
	}
}

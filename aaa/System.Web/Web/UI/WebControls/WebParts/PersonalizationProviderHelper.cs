using System;
using System.Collections;
using System.Globalization;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006E2 RID: 1762
	internal static class PersonalizationProviderHelper
	{
		// Token: 0x0600567D RID: 22141 RVA: 0x0015D0C8 File Offset: 0x0015C0C8
		internal static string[] CheckAndTrimNonEmptyStringEntries(string[] array, string paramName, bool throwIfArrayIsNull, bool checkCommas, int lengthToCheck)
		{
			if (array == null)
			{
				if (throwIfArrayIsNull)
				{
					throw new ArgumentNullException(paramName);
				}
				return null;
			}
			else
			{
				if (array.Length == 0)
				{
					throw new ArgumentException(SR.GetString("PersonalizationProviderHelper_Empty_Collection", new object[] { paramName }));
				}
				string[] array2 = null;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					string text2 = ((text == null) ? null : text.Trim());
					if (string.IsNullOrEmpty(text2))
					{
						throw new ArgumentException(SR.GetString("PersonalizationProviderHelper_Null_Or_Empty_String_Entries", new object[] { paramName }));
					}
					if (checkCommas && text2.IndexOf(',') != -1)
					{
						throw new ArgumentException(SR.GetString("PersonalizationProviderHelper_CannotHaveCommaInString", new object[] { paramName, text }));
					}
					if (lengthToCheck > -1 && text2.Length > lengthToCheck)
					{
						throw new ArgumentException(SR.GetString("PersonalizationProviderHelper_Trimmed_Entry_Value_Exceed_Maximum_Length", new object[]
						{
							text,
							paramName,
							lengthToCheck.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (text.Length != text2.Length && array2 == null)
					{
						array2 = new string[array.Length];
						Array.Copy(array, array2, i);
					}
					if (array2 != null)
					{
						array2[i] = text2;
					}
				}
				if (array2 == null)
				{
					return array;
				}
				return array2;
			}
		}

		// Token: 0x0600567E RID: 22142 RVA: 0x0015D1FC File Offset: 0x0015C1FC
		internal static string CheckAndTrimStringWithoutCommas(string paramValue, string paramName)
		{
			string text = StringUtil.CheckAndTrimString(paramValue, paramName);
			if (text.IndexOf(',') != -1)
			{
				throw new ArgumentException(SR.GetString("PersonalizationProviderHelper_CannotHaveCommaInString", new object[] { paramName, paramValue }));
			}
			return text;
		}

		// Token: 0x0600567F RID: 22143 RVA: 0x0015D240 File Offset: 0x0015C240
		internal static void CheckOnlyOnePathWithUsers(string[] paths, string[] usernames)
		{
			if (usernames != null && usernames.Length > 0 && paths != null && paths.Length > 1)
			{
				throw new ArgumentException(SR.GetString("PersonalizationProviderHelper_More_Than_One_Path", new object[] { "paths", "usernames" }));
			}
		}

		// Token: 0x06005680 RID: 22144 RVA: 0x0015D287 File Offset: 0x0015C287
		internal static void CheckNegativeInteger(int paramValue, string paramName)
		{
			if (paramValue < 0)
			{
				throw new ArgumentException(SR.GetString("PersonalizationProviderHelper_Negative_Integer"), paramName);
			}
		}

		// Token: 0x06005681 RID: 22145 RVA: 0x0015D2A0 File Offset: 0x0015C2A0
		internal static void CheckNegativeReturnedInteger(int returnedValue, string methodName)
		{
			if (returnedValue < 0)
			{
				throw new HttpException(SR.GetString("PersonalizationAdmin_UnexpectedPersonalizationProviderReturnValue", new object[]
				{
					returnedValue.ToString(CultureInfo.CurrentCulture),
					methodName
				}));
			}
		}

		// Token: 0x06005682 RID: 22146 RVA: 0x0015D2DC File Offset: 0x0015C2DC
		internal static void CheckNullEntries(ICollection array, string paramName)
		{
			if (array == null)
			{
				throw new ArgumentNullException(paramName);
			}
			if (array.Count == 0)
			{
				throw new ArgumentException(SR.GetString("PersonalizationProviderHelper_Empty_Collection", new object[] { paramName }));
			}
			using (IEnumerator enumerator = array.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == null)
					{
						throw new ArgumentException(SR.GetString("PersonalizationProviderHelper_Null_Entries", new object[] { paramName }));
					}
				}
			}
		}

		// Token: 0x06005683 RID: 22147 RVA: 0x0015D374 File Offset: 0x0015C374
		internal static void CheckPageIndexAndSize(int pageIndex, int pageSize)
		{
			if (pageIndex < 0)
			{
				throw new ArgumentException(SR.GetString("PersonalizationProviderHelper_Invalid_Less_Than_Parameter", new object[] { "pageIndex", "0" }));
			}
			if (pageSize < 1)
			{
				throw new ArgumentException(SR.GetString("PersonalizationProviderHelper_Invalid_Less_Than_Parameter", new object[] { "pageSize", "1" }));
			}
			long num = (long)pageIndex * (long)pageSize + (long)pageSize - 1L;
			if (num > 2147483647L)
			{
				throw new ArgumentException(SR.GetString("PageIndex_PageSize_bad"));
			}
		}

		// Token: 0x06005684 RID: 22148 RVA: 0x0015D3FE File Offset: 0x0015C3FE
		internal static void CheckPersonalizationScope(PersonalizationScope scope)
		{
			if (scope < PersonalizationScope.User || scope > PersonalizationScope.Shared)
			{
				throw new ArgumentOutOfRangeException("scope");
			}
		}

		// Token: 0x06005685 RID: 22149 RVA: 0x0015D414 File Offset: 0x0015C414
		internal static void CheckUsernamesInSharedScope(string[] usernames)
		{
			if (usernames != null)
			{
				throw new ArgumentException(SR.GetString("PersonalizationProviderHelper_No_Usernames_Set_In_Shared_Scope", new object[]
				{
					"usernames",
					"scope",
					PersonalizationScope.Shared.ToString()
				}));
			}
		}
	}
}

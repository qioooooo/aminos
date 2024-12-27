using System;
using System.Collections;
using System.Data.Common;
using System.Globalization;

namespace System.Data.ProviderBase
{
	// Token: 0x02000084 RID: 132
	internal sealed class FieldNameLookup
	{
		// Token: 0x06000798 RID: 1944 RVA: 0x00071698 File Offset: 0x00070A98
		public FieldNameLookup(IDataReader reader, int defaultLocaleID)
		{
			int fieldCount = reader.FieldCount;
			string[] array = new string[fieldCount];
			for (int i = 0; i < fieldCount; i++)
			{
				array[i] = reader.GetName(i);
			}
			this._fieldNames = array;
			this._defaultLocaleID = defaultLocaleID;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x000716E0 File Offset: 0x00070AE0
		public int GetOrdinal(string fieldName)
		{
			if (fieldName == null)
			{
				throw ADP.ArgumentNull("fieldName");
			}
			int num = this.IndexOf(fieldName);
			if (-1 == num)
			{
				throw ADP.IndexOutOfRange(fieldName);
			}
			return num;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00071710 File Offset: 0x00070B10
		public int IndexOf(string fieldName)
		{
			if (this._fieldNameLookup == null)
			{
				this.GenerateLookup();
			}
			object obj = this._fieldNameLookup[fieldName];
			int num;
			if (obj != null)
			{
				num = (int)obj;
			}
			else
			{
				num = this.LinearIndexOf(fieldName, CompareOptions.IgnoreCase);
				if (-1 == num)
				{
					num = this.LinearIndexOf(fieldName, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
				}
			}
			return num;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0007175C File Offset: 0x00070B5C
		private int LinearIndexOf(string fieldName, CompareOptions compareOptions)
		{
			CompareInfo compareInfo = this._compareInfo;
			if (compareInfo == null)
			{
				if (-1 != this._defaultLocaleID)
				{
					compareInfo = CompareInfo.GetCompareInfo(this._defaultLocaleID);
				}
				if (compareInfo == null)
				{
					compareInfo = CultureInfo.InvariantCulture.CompareInfo;
				}
				this._compareInfo = compareInfo;
			}
			int num = this._fieldNames.Length;
			for (int i = 0; i < num; i++)
			{
				if (compareInfo.Compare(fieldName, this._fieldNames[i], compareOptions) == 0)
				{
					this._fieldNameLookup[fieldName] = i;
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x000717DC File Offset: 0x00070BDC
		private void GenerateLookup()
		{
			int num = this._fieldNames.Length;
			Hashtable hashtable = new Hashtable(num);
			int num2 = num - 1;
			while (0 <= num2)
			{
				string text = this._fieldNames[num2];
				hashtable[text] = num2;
				num2--;
			}
			this._fieldNameLookup = hashtable;
		}

		// Token: 0x040004F1 RID: 1265
		private Hashtable _fieldNameLookup;

		// Token: 0x040004F2 RID: 1266
		private string[] _fieldNames;

		// Token: 0x040004F3 RID: 1267
		private CompareInfo _compareInfo;

		// Token: 0x040004F4 RID: 1268
		private int _defaultLocaleID;
	}
}

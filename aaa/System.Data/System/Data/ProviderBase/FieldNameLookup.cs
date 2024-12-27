using System;
using System.Collections;
using System.Data.Common;
using System.Globalization;

namespace System.Data.ProviderBase
{
	// Token: 0x0200014A RID: 330
	internal sealed class FieldNameLookup
	{
		// Token: 0x06001550 RID: 5456 RVA: 0x0022A364 File Offset: 0x00229764
		public FieldNameLookup(string[] fieldNames, int defaultLocaleID)
		{
			if (fieldNames == null)
			{
				throw ADP.ArgumentNull("fieldNames");
			}
			this._fieldNames = fieldNames;
			this._defaultLocaleID = defaultLocaleID;
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x0022A394 File Offset: 0x00229794
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

		// Token: 0x06001552 RID: 5458 RVA: 0x0022A3DC File Offset: 0x002297DC
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

		// Token: 0x06001553 RID: 5459 RVA: 0x0022A40C File Offset: 0x0022980C
		public int IndexOfName(string fieldName)
		{
			if (this._fieldNameLookup == null)
			{
				this.GenerateLookup();
			}
			object obj = this._fieldNameLookup[fieldName];
			if (obj == null)
			{
				return -1;
			}
			return (int)obj;
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x0022A440 File Offset: 0x00229840
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

		// Token: 0x06001555 RID: 5461 RVA: 0x0022A48C File Offset: 0x0022988C
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

		// Token: 0x06001556 RID: 5462 RVA: 0x0022A50C File Offset: 0x0022990C
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

		// Token: 0x04000C7B RID: 3195
		private Hashtable _fieldNameLookup;

		// Token: 0x04000C7C RID: 3196
		private string[] _fieldNames;

		// Token: 0x04000C7D RID: 3197
		private CompareInfo _compareInfo;

		// Token: 0x04000C7E RID: 3198
		private int _defaultLocaleID;
	}
}

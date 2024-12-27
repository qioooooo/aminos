using System;
using System.Collections.Specialized;
using System.Text;

namespace System.Configuration
{
	// Token: 0x0200009D RID: 157
	public sealed class CommaDelimitedStringCollection : StringCollection
	{
		// Token: 0x06000626 RID: 1574 RVA: 0x0001C9AE File Offset: 0x0001B9AE
		public CommaDelimitedStringCollection()
		{
			this._ReadOnly = false;
			this._Modified = false;
			this._OriginalString = this.ToString();
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001C9D0 File Offset: 0x0001B9D0
		internal void FromString(string list)
		{
			char[] array = new char[] { ',' };
			if (list != null)
			{
				string[] array2 = list.Split(array);
				foreach (string text in array2)
				{
					string text2 = text.Trim();
					if (text2.Length != 0)
					{
						this.Add(text.Trim());
					}
				}
			}
			this._OriginalString = this.ToString();
			this._ReadOnly = false;
			this._Modified = false;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001CA4C File Offset: 0x0001BA4C
		public override string ToString()
		{
			string text = null;
			if (base.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text2 in this)
				{
					this.ThrowIfContainsDelimiter(text2);
					stringBuilder.Append(text2.Trim());
					stringBuilder.Append(',');
				}
				text = stringBuilder.ToString();
				if (text.Length > 0)
				{
					text = text.Substring(0, text.Length - 1);
				}
				if (text.Length == 0)
				{
					text = null;
				}
			}
			return text;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001CAF0 File Offset: 0x0001BAF0
		private void ThrowIfReadOnly()
		{
			if (this.IsReadOnly)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_read_only"));
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001CB0C File Offset: 0x0001BB0C
		private void ThrowIfContainsDelimiter(string value)
		{
			if (value.Contains(","))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_value_cannot_contain", new object[] { "," }));
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001CB46 File Offset: 0x0001BB46
		public void SetReadOnly()
		{
			this._ReadOnly = true;
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x0001CB4F File Offset: 0x0001BB4F
		public bool IsModified
		{
			get
			{
				return this._Modified || this.ToString() != this._OriginalString;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x0001CB6C File Offset: 0x0001BB6C
		public new bool IsReadOnly
		{
			get
			{
				return this._ReadOnly;
			}
		}

		// Token: 0x170001E7 RID: 487
		public new string this[int index]
		{
			get
			{
				return base[index];
			}
			set
			{
				this.ThrowIfReadOnly();
				this.ThrowIfContainsDelimiter(value);
				this._Modified = true;
				base[index] = value.Trim();
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0001CBA0 File Offset: 0x0001BBA0
		public new void Add(string value)
		{
			this.ThrowIfReadOnly();
			this.ThrowIfContainsDelimiter(value);
			this._Modified = true;
			base.Add(value.Trim());
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0001CBC4 File Offset: 0x0001BBC4
		public new void AddRange(string[] range)
		{
			this.ThrowIfReadOnly();
			this._Modified = true;
			foreach (string text in range)
			{
				this.ThrowIfContainsDelimiter(text);
				base.Add(text.Trim());
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0001CC06 File Offset: 0x0001BC06
		public new void Clear()
		{
			this.ThrowIfReadOnly();
			this._Modified = true;
			base.Clear();
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0001CC1B File Offset: 0x0001BC1B
		public new void Insert(int index, string value)
		{
			this.ThrowIfReadOnly();
			this.ThrowIfContainsDelimiter(value);
			this._Modified = true;
			base.Insert(index, value.Trim());
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0001CC3E File Offset: 0x0001BC3E
		public new void Remove(string value)
		{
			this.ThrowIfReadOnly();
			this.ThrowIfContainsDelimiter(value);
			this._Modified = true;
			base.Remove(value.Trim());
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001CC60 File Offset: 0x0001BC60
		public CommaDelimitedStringCollection Clone()
		{
			CommaDelimitedStringCollection commaDelimitedStringCollection = new CommaDelimitedStringCollection();
			foreach (string text in this)
			{
				commaDelimitedStringCollection.Add(text);
			}
			commaDelimitedStringCollection._Modified = false;
			commaDelimitedStringCollection._ReadOnly = this._ReadOnly;
			commaDelimitedStringCollection._OriginalString = this._OriginalString;
			return commaDelimitedStringCollection;
		}

		// Token: 0x040003E3 RID: 995
		private bool _Modified;

		// Token: 0x040003E4 RID: 996
		private bool _ReadOnly;

		// Token: 0x040003E5 RID: 997
		private string _OriginalString;
	}
}

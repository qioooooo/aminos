using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000769 RID: 1897
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapYear : ISoapXsd
	{
		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x0600440F RID: 17423 RVA: 0x000EA342 File Offset: 0x000E9342
		public static string XsdType
		{
			get
			{
				return "gYear";
			}
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x000EA349 File Offset: 0x000E9349
		public string GetXsdType()
		{
			return SoapYear.XsdType;
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x000EA350 File Offset: 0x000E9350
		public SoapYear()
		{
		}

		// Token: 0x06004412 RID: 17426 RVA: 0x000EA363 File Offset: 0x000E9363
		public SoapYear(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x000EA37D File Offset: 0x000E937D
		public SoapYear(DateTime value, int sign)
		{
			this._value = value;
			this._sign = sign;
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06004414 RID: 17428 RVA: 0x000EA39E File Offset: 0x000E939E
		// (set) Token: 0x06004415 RID: 17429 RVA: 0x000EA3A6 File Offset: 0x000E93A6
		public DateTime Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06004416 RID: 17430 RVA: 0x000EA3AF File Offset: 0x000E93AF
		// (set) Token: 0x06004417 RID: 17431 RVA: 0x000EA3B7 File Offset: 0x000E93B7
		public int Sign
		{
			get
			{
				return this._sign;
			}
			set
			{
				this._sign = value;
			}
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x000EA3C0 File Offset: 0x000E93C0
		public override string ToString()
		{
			if (this._sign < 0)
			{
				return this._value.ToString("'-'yyyy", CultureInfo.InvariantCulture);
			}
			return this._value.ToString("yyyy", CultureInfo.InvariantCulture);
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x000EA3F8 File Offset: 0x000E93F8
		public static SoapYear Parse(string value)
		{
			int num = 0;
			if (value[0] == '-')
			{
				num = -1;
			}
			return new SoapYear(DateTime.ParseExact(value, SoapYear.formats, CultureInfo.InvariantCulture, DateTimeStyles.None), num);
		}

		// Token: 0x040021F9 RID: 8697
		private DateTime _value = DateTime.MinValue;

		// Token: 0x040021FA RID: 8698
		private int _sign;

		// Token: 0x040021FB RID: 8699
		private static string[] formats = new string[] { "yyyy", "'+'yyyy", "'-'yyyy", "yyyyzzz", "'+'yyyyzzz", "'-'yyyyzzz" };
	}
}

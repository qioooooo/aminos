using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000767 RID: 1895
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapDate : ISoapXsd
	{
		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x060043F7 RID: 17399 RVA: 0x000EA094 File Offset: 0x000E9094
		public static string XsdType
		{
			get
			{
				return "date";
			}
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x000EA09B File Offset: 0x000E909B
		public string GetXsdType()
		{
			return SoapDate.XsdType;
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x000EA0A4 File Offset: 0x000E90A4
		public SoapDate()
		{
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x000EA0CC File Offset: 0x000E90CC
		public SoapDate(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x000EA0FC File Offset: 0x000E90FC
		public SoapDate(DateTime value, int sign)
		{
			this._value = value;
			this._sign = sign;
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x060043FC RID: 17404 RVA: 0x000EA130 File Offset: 0x000E9130
		// (set) Token: 0x060043FD RID: 17405 RVA: 0x000EA138 File Offset: 0x000E9138
		public DateTime Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value.Date;
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x060043FE RID: 17406 RVA: 0x000EA147 File Offset: 0x000E9147
		// (set) Token: 0x060043FF RID: 17407 RVA: 0x000EA14F File Offset: 0x000E914F
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

		// Token: 0x06004400 RID: 17408 RVA: 0x000EA158 File Offset: 0x000E9158
		public override string ToString()
		{
			if (this._sign < 0)
			{
				return this._value.ToString("'-'yyyy-MM-dd", CultureInfo.InvariantCulture);
			}
			return this._value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x000EA190 File Offset: 0x000E9190
		public static SoapDate Parse(string value)
		{
			int num = 0;
			if (value[0] == '-')
			{
				num = -1;
			}
			return new SoapDate(DateTime.ParseExact(value, SoapDate.formats, CultureInfo.InvariantCulture, DateTimeStyles.None), num);
		}

		// Token: 0x040021F3 RID: 8691
		private DateTime _value = DateTime.MinValue.Date;

		// Token: 0x040021F4 RID: 8692
		private int _sign;

		// Token: 0x040021F5 RID: 8693
		private static string[] formats = new string[] { "yyyy-MM-dd", "'+'yyyy-MM-dd", "'-'yyyy-MM-dd", "yyyy-MM-ddzzz", "'+'yyyy-MM-ddzzz", "'-'yyyy-MM-ddzzz" };
	}
}

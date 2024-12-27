using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000768 RID: 1896
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapYearMonth : ISoapXsd
	{
		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06004403 RID: 17411 RVA: 0x000EA20E File Offset: 0x000E920E
		public static string XsdType
		{
			get
			{
				return "gYearMonth";
			}
		}

		// Token: 0x06004404 RID: 17412 RVA: 0x000EA215 File Offset: 0x000E9215
		public string GetXsdType()
		{
			return SoapYearMonth.XsdType;
		}

		// Token: 0x06004405 RID: 17413 RVA: 0x000EA21C File Offset: 0x000E921C
		public SoapYearMonth()
		{
		}

		// Token: 0x06004406 RID: 17414 RVA: 0x000EA22F File Offset: 0x000E922F
		public SoapYearMonth(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x06004407 RID: 17415 RVA: 0x000EA249 File Offset: 0x000E9249
		public SoapYearMonth(DateTime value, int sign)
		{
			this._value = value;
			this._sign = sign;
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06004408 RID: 17416 RVA: 0x000EA26A File Offset: 0x000E926A
		// (set) Token: 0x06004409 RID: 17417 RVA: 0x000EA272 File Offset: 0x000E9272
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

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x0600440A RID: 17418 RVA: 0x000EA27B File Offset: 0x000E927B
		// (set) Token: 0x0600440B RID: 17419 RVA: 0x000EA283 File Offset: 0x000E9283
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

		// Token: 0x0600440C RID: 17420 RVA: 0x000EA28C File Offset: 0x000E928C
		public override string ToString()
		{
			if (this._sign < 0)
			{
				return this._value.ToString("'-'yyyy-MM", CultureInfo.InvariantCulture);
			}
			return this._value.ToString("yyyy-MM", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x000EA2C4 File Offset: 0x000E92C4
		public static SoapYearMonth Parse(string value)
		{
			int num = 0;
			if (value[0] == '-')
			{
				num = -1;
			}
			return new SoapYearMonth(DateTime.ParseExact(value, SoapYearMonth.formats, CultureInfo.InvariantCulture, DateTimeStyles.None), num);
		}

		// Token: 0x040021F6 RID: 8694
		private DateTime _value = DateTime.MinValue;

		// Token: 0x040021F7 RID: 8695
		private int _sign;

		// Token: 0x040021F8 RID: 8696
		private static string[] formats = new string[] { "yyyy-MM", "'+'yyyy-MM", "'-'yyyy-MM", "yyyy-MMzzz", "'+'yyyy-MMzzz", "'-'yyyy-MMzzz" };
	}
}

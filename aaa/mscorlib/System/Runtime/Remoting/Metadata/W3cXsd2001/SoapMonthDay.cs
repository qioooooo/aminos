using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200076A RID: 1898
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapMonthDay : ISoapXsd
	{
		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x0600441B RID: 17435 RVA: 0x000EA476 File Offset: 0x000E9476
		public static string XsdType
		{
			get
			{
				return "gMonthDay";
			}
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x000EA47D File Offset: 0x000E947D
		public string GetXsdType()
		{
			return SoapMonthDay.XsdType;
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x000EA484 File Offset: 0x000E9484
		public SoapMonthDay()
		{
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x000EA497 File Offset: 0x000E9497
		public SoapMonthDay(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x0600441F RID: 17439 RVA: 0x000EA4B1 File Offset: 0x000E94B1
		// (set) Token: 0x06004420 RID: 17440 RVA: 0x000EA4B9 File Offset: 0x000E94B9
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

		// Token: 0x06004421 RID: 17441 RVA: 0x000EA4C2 File Offset: 0x000E94C2
		public override string ToString()
		{
			return this._value.ToString("'--'MM'-'dd", CultureInfo.InvariantCulture);
		}

		// Token: 0x06004422 RID: 17442 RVA: 0x000EA4D9 File Offset: 0x000E94D9
		public static SoapMonthDay Parse(string value)
		{
			return new SoapMonthDay(DateTime.ParseExact(value, SoapMonthDay.formats, CultureInfo.InvariantCulture, DateTimeStyles.None));
		}

		// Token: 0x040021FC RID: 8700
		private DateTime _value = DateTime.MinValue;

		// Token: 0x040021FD RID: 8701
		private static string[] formats = new string[] { "--MM-dd", "--MM-ddzzz" };
	}
}

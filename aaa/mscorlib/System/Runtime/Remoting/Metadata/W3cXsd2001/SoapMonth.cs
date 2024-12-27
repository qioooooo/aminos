using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200076C RID: 1900
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapMonth : ISoapXsd
	{
		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x0600442D RID: 17453 RVA: 0x000EA5C6 File Offset: 0x000E95C6
		public static string XsdType
		{
			get
			{
				return "gMonth";
			}
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x000EA5CD File Offset: 0x000E95CD
		public string GetXsdType()
		{
			return SoapMonth.XsdType;
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x000EA5D4 File Offset: 0x000E95D4
		public SoapMonth()
		{
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x000EA5E7 File Offset: 0x000E95E7
		public SoapMonth(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06004431 RID: 17457 RVA: 0x000EA601 File Offset: 0x000E9601
		// (set) Token: 0x06004432 RID: 17458 RVA: 0x000EA609 File Offset: 0x000E9609
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

		// Token: 0x06004433 RID: 17459 RVA: 0x000EA612 File Offset: 0x000E9612
		public override string ToString()
		{
			return this._value.ToString("--MM--", CultureInfo.InvariantCulture);
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x000EA629 File Offset: 0x000E9629
		public static SoapMonth Parse(string value)
		{
			return new SoapMonth(DateTime.ParseExact(value, SoapMonth.formats, CultureInfo.InvariantCulture, DateTimeStyles.None));
		}

		// Token: 0x04002200 RID: 8704
		private DateTime _value = DateTime.MinValue;

		// Token: 0x04002201 RID: 8705
		private static string[] formats = new string[] { "--MM--", "--MM--zzz" };
	}
}

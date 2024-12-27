using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200076B RID: 1899
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapDay : ISoapXsd
	{
		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06004424 RID: 17444 RVA: 0x000EA51E File Offset: 0x000E951E
		public static string XsdType
		{
			get
			{
				return "gDay";
			}
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x000EA525 File Offset: 0x000E9525
		public string GetXsdType()
		{
			return SoapDay.XsdType;
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x000EA52C File Offset: 0x000E952C
		public SoapDay()
		{
		}

		// Token: 0x06004427 RID: 17447 RVA: 0x000EA53F File Offset: 0x000E953F
		public SoapDay(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06004428 RID: 17448 RVA: 0x000EA559 File Offset: 0x000E9559
		// (set) Token: 0x06004429 RID: 17449 RVA: 0x000EA561 File Offset: 0x000E9561
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

		// Token: 0x0600442A RID: 17450 RVA: 0x000EA56A File Offset: 0x000E956A
		public override string ToString()
		{
			return this._value.ToString("---dd", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x000EA581 File Offset: 0x000E9581
		public static SoapDay Parse(string value)
		{
			return new SoapDay(DateTime.ParseExact(value, SoapDay.formats, CultureInfo.InvariantCulture, DateTimeStyles.None));
		}

		// Token: 0x040021FE RID: 8702
		private DateTime _value = DateTime.MinValue;

		// Token: 0x040021FF RID: 8703
		private static string[] formats = new string[] { "---dd", "---ddzzz" };
	}
}

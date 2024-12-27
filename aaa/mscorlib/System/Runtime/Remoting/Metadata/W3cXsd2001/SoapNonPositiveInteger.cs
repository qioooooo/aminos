using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000771 RID: 1905
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNonPositiveInteger : ISoapXsd
	{
		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06004458 RID: 17496 RVA: 0x000EAA84 File Offset: 0x000E9A84
		public static string XsdType
		{
			get
			{
				return "nonPositiveInteger";
			}
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x000EAA8B File Offset: 0x000E9A8B
		public string GetXsdType()
		{
			return SoapNonPositiveInteger.XsdType;
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x000EAA92 File Offset: 0x000E9A92
		public SoapNonPositiveInteger()
		{
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x000EAA9C File Offset: 0x000E9A9C
		public SoapNonPositiveInteger(decimal value)
		{
			this._value = decimal.Truncate(value);
			if (this._value > 0m)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:nonPositiveInteger", value }));
			}
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x0600445C RID: 17500 RVA: 0x000EAB01 File Offset: 0x000E9B01
		// (set) Token: 0x0600445D RID: 17501 RVA: 0x000EAB0C File Offset: 0x000E9B0C
		public decimal Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = decimal.Truncate(value);
				if (this._value > 0m)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:nonPositiveInteger", value }));
				}
			}
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x000EAB6B File Offset: 0x000E9B6B
		public override string ToString()
		{
			return this._value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x000EAB7D File Offset: 0x000E9B7D
		public static SoapNonPositiveInteger Parse(string value)
		{
			return new SoapNonPositiveInteger(decimal.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture));
		}

		// Token: 0x04002207 RID: 8711
		private decimal _value;
	}
}

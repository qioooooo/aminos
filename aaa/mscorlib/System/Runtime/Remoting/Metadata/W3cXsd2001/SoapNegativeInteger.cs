using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000773 RID: 1907
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNegativeInteger : ISoapXsd
	{
		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06004468 RID: 17512 RVA: 0x000EAC9C File Offset: 0x000E9C9C
		public static string XsdType
		{
			get
			{
				return "negativeInteger";
			}
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x000EACA3 File Offset: 0x000E9CA3
		public string GetXsdType()
		{
			return SoapNegativeInteger.XsdType;
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x000EACAA File Offset: 0x000E9CAA
		public SoapNegativeInteger()
		{
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x000EACB4 File Offset: 0x000E9CB4
		public SoapNegativeInteger(decimal value)
		{
			this._value = decimal.Truncate(value);
			if (value > -1m)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:negativeInteger", value }));
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x0600446C RID: 17516 RVA: 0x000EAD14 File Offset: 0x000E9D14
		// (set) Token: 0x0600446D RID: 17517 RVA: 0x000EAD1C File Offset: 0x000E9D1C
		public decimal Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = decimal.Truncate(value);
				if (this._value > -1m)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:negativeInteger", value }));
				}
			}
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x000EAD7B File Offset: 0x000E9D7B
		public override string ToString()
		{
			return this._value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x000EAD8D File Offset: 0x000E9D8D
		public static SoapNegativeInteger Parse(string value)
		{
			return new SoapNegativeInteger(decimal.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture));
		}

		// Token: 0x04002209 RID: 8713
		private decimal _value;
	}
}

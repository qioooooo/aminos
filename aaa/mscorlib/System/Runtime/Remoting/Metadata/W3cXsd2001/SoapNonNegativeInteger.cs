using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000772 RID: 1906
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNonNegativeInteger : ISoapXsd
	{
		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x06004460 RID: 17504 RVA: 0x000EAB90 File Offset: 0x000E9B90
		public static string XsdType
		{
			get
			{
				return "nonNegativeInteger";
			}
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x000EAB97 File Offset: 0x000E9B97
		public string GetXsdType()
		{
			return SoapNonNegativeInteger.XsdType;
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x000EAB9E File Offset: 0x000E9B9E
		public SoapNonNegativeInteger()
		{
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x000EABA8 File Offset: 0x000E9BA8
		public SoapNonNegativeInteger(decimal value)
		{
			this._value = decimal.Truncate(value);
			if (this._value < 0m)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:nonNegativeInteger", value }));
			}
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x06004464 RID: 17508 RVA: 0x000EAC0D File Offset: 0x000E9C0D
		// (set) Token: 0x06004465 RID: 17509 RVA: 0x000EAC18 File Offset: 0x000E9C18
		public decimal Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = decimal.Truncate(value);
				if (this._value < 0m)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:nonNegativeInteger", value }));
				}
			}
		}

		// Token: 0x06004466 RID: 17510 RVA: 0x000EAC77 File Offset: 0x000E9C77
		public override string ToString()
		{
			return this._value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x000EAC89 File Offset: 0x000E9C89
		public static SoapNonNegativeInteger Parse(string value)
		{
			return new SoapNonNegativeInteger(decimal.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture));
		}

		// Token: 0x04002208 RID: 8712
		private decimal _value;
	}
}

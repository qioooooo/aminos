using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000770 RID: 1904
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapPositiveInteger : ISoapXsd
	{
		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06004450 RID: 17488 RVA: 0x000EA979 File Offset: 0x000E9979
		public static string XsdType
		{
			get
			{
				return "positiveInteger";
			}
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x000EA980 File Offset: 0x000E9980
		public string GetXsdType()
		{
			return SoapPositiveInteger.XsdType;
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x000EA987 File Offset: 0x000E9987
		public SoapPositiveInteger()
		{
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x000EA990 File Offset: 0x000E9990
		public SoapPositiveInteger(decimal value)
		{
			this._value = decimal.Truncate(value);
			if (this._value < 1m)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:positiveInteger", value }));
			}
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06004454 RID: 17492 RVA: 0x000EA9F5 File Offset: 0x000E99F5
		// (set) Token: 0x06004455 RID: 17493 RVA: 0x000EAA00 File Offset: 0x000E9A00
		public decimal Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = decimal.Truncate(value);
				if (this._value < 1m)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:positiveInteger", value }));
				}
			}
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x000EAA5F File Offset: 0x000E9A5F
		public override string ToString()
		{
			return this._value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x000EAA71 File Offset: 0x000E9A71
		public static SoapPositiveInteger Parse(string value)
		{
			return new SoapPositiveInteger(decimal.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture));
		}

		// Token: 0x04002206 RID: 8710
		private decimal _value;
	}
}

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200076F RID: 1903
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapInteger : ISoapXsd
	{
		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06004448 RID: 17480 RVA: 0x000EA914 File Offset: 0x000E9914
		public static string XsdType
		{
			get
			{
				return "integer";
			}
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x000EA91B File Offset: 0x000E991B
		public string GetXsdType()
		{
			return SoapInteger.XsdType;
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x000EA922 File Offset: 0x000E9922
		public SoapInteger()
		{
		}

		// Token: 0x0600444B RID: 17483 RVA: 0x000EA92A File Offset: 0x000E992A
		public SoapInteger(decimal value)
		{
			this._value = decimal.Truncate(value);
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x0600444C RID: 17484 RVA: 0x000EA93E File Offset: 0x000E993E
		// (set) Token: 0x0600444D RID: 17485 RVA: 0x000EA946 File Offset: 0x000E9946
		public decimal Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = decimal.Truncate(value);
			}
		}

		// Token: 0x0600444E RID: 17486 RVA: 0x000EA954 File Offset: 0x000E9954
		public override string ToString()
		{
			return this._value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x000EA966 File Offset: 0x000E9966
		public static SoapInteger Parse(string value)
		{
			return new SoapInteger(decimal.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture));
		}

		// Token: 0x04002205 RID: 8709
		private decimal _value;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000776 RID: 1910
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNotation : ISoapXsd
	{
		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06004486 RID: 17542 RVA: 0x000EAEED File Offset: 0x000E9EED
		public static string XsdType
		{
			get
			{
				return "NOTATION";
			}
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x000EAEF4 File Offset: 0x000E9EF4
		public string GetXsdType()
		{
			return SoapNotation.XsdType;
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x000EAEFB File Offset: 0x000E9EFB
		public SoapNotation()
		{
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x000EAF03 File Offset: 0x000E9F03
		public SoapNotation(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x0600448A RID: 17546 RVA: 0x000EAF12 File Offset: 0x000E9F12
		// (set) Token: 0x0600448B RID: 17547 RVA: 0x000EAF1A File Offset: 0x000E9F1A
		public string Value
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

		// Token: 0x0600448C RID: 17548 RVA: 0x000EAF23 File Offset: 0x000E9F23
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x000EAF2B File Offset: 0x000E9F2B
		public static SoapNotation Parse(string value)
		{
			return new SoapNotation(value);
		}

		// Token: 0x0400220E RID: 8718
		private string _value;
	}
}

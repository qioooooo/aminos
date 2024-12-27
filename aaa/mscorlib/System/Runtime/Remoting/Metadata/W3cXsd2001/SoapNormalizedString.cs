using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000777 RID: 1911
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNormalizedString : ISoapXsd
	{
		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x0600448E RID: 17550 RVA: 0x000EAF33 File Offset: 0x000E9F33
		public static string XsdType
		{
			get
			{
				return "normalizedString";
			}
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x000EAF3A File Offset: 0x000E9F3A
		public string GetXsdType()
		{
			return SoapNormalizedString.XsdType;
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x000EAF41 File Offset: 0x000E9F41
		public SoapNormalizedString()
		{
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x000EAF49 File Offset: 0x000E9F49
		public SoapNormalizedString(string value)
		{
			this._value = this.Validate(value);
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06004492 RID: 17554 RVA: 0x000EAF5E File Offset: 0x000E9F5E
		// (set) Token: 0x06004493 RID: 17555 RVA: 0x000EAF66 File Offset: 0x000E9F66
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = this.Validate(value);
			}
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x000EAF75 File Offset: 0x000E9F75
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x000EAF82 File Offset: 0x000E9F82
		public static SoapNormalizedString Parse(string value)
		{
			return new SoapNormalizedString(value);
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x000EAF98 File Offset: 0x000E9F98
		private string Validate(string value)
		{
			if (value == null || value.Length == 0)
			{
				return value;
			}
			char[] array = new char[] { '\r', '\n', '\t' };
			int num = value.LastIndexOfAny(array);
			if (num > -1)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:normalizedString", value }));
			}
			return value;
		}

		// Token: 0x0400220F RID: 8719
		private string _value;
	}
}

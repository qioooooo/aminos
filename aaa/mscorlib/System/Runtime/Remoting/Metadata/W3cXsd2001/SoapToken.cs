using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000778 RID: 1912
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapToken : ISoapXsd
	{
		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06004497 RID: 17559 RVA: 0x000EAFFF File Offset: 0x000E9FFF
		public static string XsdType
		{
			get
			{
				return "token";
			}
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x000EB006 File Offset: 0x000EA006
		public string GetXsdType()
		{
			return SoapToken.XsdType;
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x000EB00D File Offset: 0x000EA00D
		public SoapToken()
		{
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x000EB015 File Offset: 0x000EA015
		public SoapToken(string value)
		{
			this._value = this.Validate(value);
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x0600449B RID: 17563 RVA: 0x000EB02A File Offset: 0x000EA02A
		// (set) Token: 0x0600449C RID: 17564 RVA: 0x000EB032 File Offset: 0x000EA032
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

		// Token: 0x0600449D RID: 17565 RVA: 0x000EB041 File Offset: 0x000EA041
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x000EB04E File Offset: 0x000EA04E
		public static SoapToken Parse(string value)
		{
			return new SoapToken(value);
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x000EB058 File Offset: 0x000EA058
		private string Validate(string value)
		{
			if (value == null || value.Length == 0)
			{
				return value;
			}
			char[] array = new char[] { '\r', '\t' };
			int num = value.LastIndexOfAny(array);
			if (num > -1)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:token", value }));
			}
			if (value.Length > 0 && (char.IsWhiteSpace(value[0]) || char.IsWhiteSpace(value[value.Length - 1])))
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:token", value }));
			}
			num = value.IndexOf("  ");
			if (num > -1)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:token", value }));
			}
			return value;
		}

		// Token: 0x04002210 RID: 8720
		private string _value;
	}
}

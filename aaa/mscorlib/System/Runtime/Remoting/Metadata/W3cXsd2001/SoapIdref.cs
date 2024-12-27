using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000781 RID: 1921
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapIdref : ISoapXsd
	{
		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x060044E0 RID: 17632 RVA: 0x000EB3B8 File Offset: 0x000EA3B8
		public static string XsdType
		{
			get
			{
				return "IDREF";
			}
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x000EB3BF File Offset: 0x000EA3BF
		public string GetXsdType()
		{
			return SoapIdref.XsdType;
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x000EB3C6 File Offset: 0x000EA3C6
		public SoapIdref()
		{
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x000EB3CE File Offset: 0x000EA3CE
		public SoapIdref(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x060044E4 RID: 17636 RVA: 0x000EB3DD File Offset: 0x000EA3DD
		// (set) Token: 0x060044E5 RID: 17637 RVA: 0x000EB3E5 File Offset: 0x000EA3E5
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

		// Token: 0x060044E6 RID: 17638 RVA: 0x000EB3EE File Offset: 0x000EA3EE
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x000EB3FB File Offset: 0x000EA3FB
		public static SoapIdref Parse(string value)
		{
			return new SoapIdref(value);
		}

		// Token: 0x04002219 RID: 8729
		private string _value;
	}
}

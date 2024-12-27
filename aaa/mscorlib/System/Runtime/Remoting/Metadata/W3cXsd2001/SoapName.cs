using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200077A RID: 1914
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapName : ISoapXsd
	{
		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x060044A8 RID: 17576 RVA: 0x000EB1AB File Offset: 0x000EA1AB
		public static string XsdType
		{
			get
			{
				return "Name";
			}
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x000EB1B2 File Offset: 0x000EA1B2
		public string GetXsdType()
		{
			return SoapName.XsdType;
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x000EB1B9 File Offset: 0x000EA1B9
		public SoapName()
		{
		}

		// Token: 0x060044AB RID: 17579 RVA: 0x000EB1C1 File Offset: 0x000EA1C1
		public SoapName(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x060044AC RID: 17580 RVA: 0x000EB1D0 File Offset: 0x000EA1D0
		// (set) Token: 0x060044AD RID: 17581 RVA: 0x000EB1D8 File Offset: 0x000EA1D8
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

		// Token: 0x060044AE RID: 17582 RVA: 0x000EB1E1 File Offset: 0x000EA1E1
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x000EB1EE File Offset: 0x000EA1EE
		public static SoapName Parse(string value)
		{
			return new SoapName(value);
		}

		// Token: 0x04002212 RID: 8722
		private string _value;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200077F RID: 1919
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNcName : ISoapXsd
	{
		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x060044D0 RID: 17616 RVA: 0x000EB322 File Offset: 0x000EA322
		public static string XsdType
		{
			get
			{
				return "NCName";
			}
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x000EB329 File Offset: 0x000EA329
		public string GetXsdType()
		{
			return SoapNcName.XsdType;
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x000EB330 File Offset: 0x000EA330
		public SoapNcName()
		{
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x000EB338 File Offset: 0x000EA338
		public SoapNcName(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x060044D4 RID: 17620 RVA: 0x000EB347 File Offset: 0x000EA347
		// (set) Token: 0x060044D5 RID: 17621 RVA: 0x000EB34F File Offset: 0x000EA34F
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

		// Token: 0x060044D6 RID: 17622 RVA: 0x000EB358 File Offset: 0x000EA358
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044D7 RID: 17623 RVA: 0x000EB365 File Offset: 0x000EA365
		public static SoapNcName Parse(string value)
		{
			return new SoapNcName(value);
		}

		// Token: 0x04002217 RID: 8727
		private string _value;
	}
}

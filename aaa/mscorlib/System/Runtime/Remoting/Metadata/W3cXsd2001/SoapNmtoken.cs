using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200077D RID: 1917
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNmtoken : ISoapXsd
	{
		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x060044C0 RID: 17600 RVA: 0x000EB28C File Offset: 0x000EA28C
		public static string XsdType
		{
			get
			{
				return "NMTOKEN";
			}
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x000EB293 File Offset: 0x000EA293
		public string GetXsdType()
		{
			return SoapNmtoken.XsdType;
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x000EB29A File Offset: 0x000EA29A
		public SoapNmtoken()
		{
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x000EB2A2 File Offset: 0x000EA2A2
		public SoapNmtoken(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x060044C4 RID: 17604 RVA: 0x000EB2B1 File Offset: 0x000EA2B1
		// (set) Token: 0x060044C5 RID: 17605 RVA: 0x000EB2B9 File Offset: 0x000EA2B9
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

		// Token: 0x060044C6 RID: 17606 RVA: 0x000EB2C2 File Offset: 0x000EA2C2
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x000EB2CF File Offset: 0x000EA2CF
		public static SoapNmtoken Parse(string value)
		{
			return new SoapNmtoken(value);
		}

		// Token: 0x04002215 RID: 8725
		private string _value;
	}
}

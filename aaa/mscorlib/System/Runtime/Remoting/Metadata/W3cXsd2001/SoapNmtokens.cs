using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200077E RID: 1918
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNmtokens : ISoapXsd
	{
		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x060044C8 RID: 17608 RVA: 0x000EB2D7 File Offset: 0x000EA2D7
		public static string XsdType
		{
			get
			{
				return "NMTOKENS";
			}
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x000EB2DE File Offset: 0x000EA2DE
		public string GetXsdType()
		{
			return SoapNmtokens.XsdType;
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x000EB2E5 File Offset: 0x000EA2E5
		public SoapNmtokens()
		{
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x000EB2ED File Offset: 0x000EA2ED
		public SoapNmtokens(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x060044CC RID: 17612 RVA: 0x000EB2FC File Offset: 0x000EA2FC
		// (set) Token: 0x060044CD RID: 17613 RVA: 0x000EB304 File Offset: 0x000EA304
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

		// Token: 0x060044CE RID: 17614 RVA: 0x000EB30D File Offset: 0x000EA30D
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x000EB31A File Offset: 0x000EA31A
		public static SoapNmtokens Parse(string value)
		{
			return new SoapNmtokens(value);
		}

		// Token: 0x04002216 RID: 8726
		private string _value;
	}
}

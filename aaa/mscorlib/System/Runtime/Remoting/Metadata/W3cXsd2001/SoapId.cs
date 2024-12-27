using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000780 RID: 1920
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapId : ISoapXsd
	{
		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x060044D8 RID: 17624 RVA: 0x000EB36D File Offset: 0x000EA36D
		public static string XsdType
		{
			get
			{
				return "ID";
			}
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x000EB374 File Offset: 0x000EA374
		public string GetXsdType()
		{
			return SoapId.XsdType;
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x000EB37B File Offset: 0x000EA37B
		public SoapId()
		{
		}

		// Token: 0x060044DB RID: 17627 RVA: 0x000EB383 File Offset: 0x000EA383
		public SoapId(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x060044DC RID: 17628 RVA: 0x000EB392 File Offset: 0x000EA392
		// (set) Token: 0x060044DD RID: 17629 RVA: 0x000EB39A File Offset: 0x000EA39A
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

		// Token: 0x060044DE RID: 17630 RVA: 0x000EB3A3 File Offset: 0x000EA3A3
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x000EB3B0 File Offset: 0x000EA3B0
		public static SoapId Parse(string value)
		{
			return new SoapId(value);
		}

		// Token: 0x04002218 RID: 8728
		private string _value;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200077B RID: 1915
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapIdrefs : ISoapXsd
	{
		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x060044B0 RID: 17584 RVA: 0x000EB1F6 File Offset: 0x000EA1F6
		public static string XsdType
		{
			get
			{
				return "IDREFS";
			}
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x000EB1FD File Offset: 0x000EA1FD
		public string GetXsdType()
		{
			return SoapIdrefs.XsdType;
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x000EB204 File Offset: 0x000EA204
		public SoapIdrefs()
		{
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x000EB20C File Offset: 0x000EA20C
		public SoapIdrefs(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x060044B4 RID: 17588 RVA: 0x000EB21B File Offset: 0x000EA21B
		// (set) Token: 0x060044B5 RID: 17589 RVA: 0x000EB223 File Offset: 0x000EA223
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

		// Token: 0x060044B6 RID: 17590 RVA: 0x000EB22C File Offset: 0x000EA22C
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x000EB239 File Offset: 0x000EA239
		public static SoapIdrefs Parse(string value)
		{
			return new SoapIdrefs(value);
		}

		// Token: 0x04002213 RID: 8723
		private string _value;
	}
}

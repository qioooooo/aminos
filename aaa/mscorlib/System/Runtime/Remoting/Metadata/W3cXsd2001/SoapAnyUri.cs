using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000774 RID: 1908
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapAnyUri : ISoapXsd
	{
		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x06004470 RID: 17520 RVA: 0x000EADA0 File Offset: 0x000E9DA0
		public static string XsdType
		{
			get
			{
				return "anyURI";
			}
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x000EADA7 File Offset: 0x000E9DA7
		public string GetXsdType()
		{
			return SoapAnyUri.XsdType;
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x000EADAE File Offset: 0x000E9DAE
		public SoapAnyUri()
		{
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x000EADB6 File Offset: 0x000E9DB6
		public SoapAnyUri(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06004474 RID: 17524 RVA: 0x000EADC5 File Offset: 0x000E9DC5
		// (set) Token: 0x06004475 RID: 17525 RVA: 0x000EADCD File Offset: 0x000E9DCD
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

		// Token: 0x06004476 RID: 17526 RVA: 0x000EADD6 File Offset: 0x000E9DD6
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x000EADDE File Offset: 0x000E9DDE
		public static SoapAnyUri Parse(string value)
		{
			return new SoapAnyUri(value);
		}

		// Token: 0x0400220A RID: 8714
		private string _value;
	}
}

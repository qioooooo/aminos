using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200077C RID: 1916
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapEntities : ISoapXsd
	{
		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x060044B8 RID: 17592 RVA: 0x000EB241 File Offset: 0x000EA241
		public static string XsdType
		{
			get
			{
				return "ENTITIES";
			}
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x000EB248 File Offset: 0x000EA248
		public string GetXsdType()
		{
			return SoapEntities.XsdType;
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x000EB24F File Offset: 0x000EA24F
		public SoapEntities()
		{
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x000EB257 File Offset: 0x000EA257
		public SoapEntities(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x060044BC RID: 17596 RVA: 0x000EB266 File Offset: 0x000EA266
		// (set) Token: 0x060044BD RID: 17597 RVA: 0x000EB26E File Offset: 0x000EA26E
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

		// Token: 0x060044BE RID: 17598 RVA: 0x000EB277 File Offset: 0x000EA277
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044BF RID: 17599 RVA: 0x000EB284 File Offset: 0x000EA284
		public static SoapEntities Parse(string value)
		{
			return new SoapEntities(value);
		}

		// Token: 0x04002214 RID: 8724
		private string _value;
	}
}

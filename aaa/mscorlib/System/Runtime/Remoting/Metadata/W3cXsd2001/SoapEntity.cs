using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000782 RID: 1922
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapEntity : ISoapXsd
	{
		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x060044E8 RID: 17640 RVA: 0x000EB403 File Offset: 0x000EA403
		public static string XsdType
		{
			get
			{
				return "ENTITY";
			}
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x000EB40A File Offset: 0x000EA40A
		public string GetXsdType()
		{
			return SoapEntity.XsdType;
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x000EB411 File Offset: 0x000EA411
		public SoapEntity()
		{
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x000EB419 File Offset: 0x000EA419
		public SoapEntity(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x060044EC RID: 17644 RVA: 0x000EB428 File Offset: 0x000EA428
		// (set) Token: 0x060044ED RID: 17645 RVA: 0x000EB430 File Offset: 0x000EA430
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

		// Token: 0x060044EE RID: 17646 RVA: 0x000EB439 File Offset: 0x000EA439
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x000EB446 File Offset: 0x000EA446
		public static SoapEntity Parse(string value)
		{
			return new SoapEntity(value);
		}

		// Token: 0x0400221A RID: 8730
		private string _value;
	}
}

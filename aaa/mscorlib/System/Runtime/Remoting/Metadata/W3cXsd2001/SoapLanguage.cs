using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000779 RID: 1913
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapLanguage : ISoapXsd
	{
		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x060044A0 RID: 17568 RVA: 0x000EB160 File Offset: 0x000EA160
		public static string XsdType
		{
			get
			{
				return "language";
			}
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x000EB167 File Offset: 0x000EA167
		public string GetXsdType()
		{
			return SoapLanguage.XsdType;
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x000EB16E File Offset: 0x000EA16E
		public SoapLanguage()
		{
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x000EB176 File Offset: 0x000EA176
		public SoapLanguage(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x060044A4 RID: 17572 RVA: 0x000EB185 File Offset: 0x000EA185
		// (set) Token: 0x060044A5 RID: 17573 RVA: 0x000EB18D File Offset: 0x000EA18D
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

		// Token: 0x060044A6 RID: 17574 RVA: 0x000EB196 File Offset: 0x000EA196
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044A7 RID: 17575 RVA: 0x000EB1A3 File Offset: 0x000EA1A3
		public static SoapLanguage Parse(string value)
		{
			return new SoapLanguage(value);
		}

		// Token: 0x04002211 RID: 8721
		private string _value;
	}
}

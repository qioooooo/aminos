using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002D2 RID: 722
	internal class SpecialMapping : TypeMapping
	{
		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06002227 RID: 8743 RVA: 0x0009FEF0 File Offset: 0x0009EEF0
		// (set) Token: 0x06002228 RID: 8744 RVA: 0x0009FEF8 File Offset: 0x0009EEF8
		internal bool NamedAny
		{
			get
			{
				return this.namedAny;
			}
			set
			{
				this.namedAny = value;
			}
		}

		// Token: 0x0400149E RID: 5278
		private bool namedAny;
	}
}

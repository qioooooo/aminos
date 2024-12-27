using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002CA RID: 714
	internal class ConstantMapping : Mapping
	{
		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x0009F365 File Offset: 0x0009E365
		// (set) Token: 0x060021CB RID: 8651 RVA: 0x0009F37B File Offset: 0x0009E37B
		internal string XmlName
		{
			get
			{
				if (this.xmlName != null)
				{
					return this.xmlName;
				}
				return string.Empty;
			}
			set
			{
				this.xmlName = value;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x060021CC RID: 8652 RVA: 0x0009F384 File Offset: 0x0009E384
		// (set) Token: 0x060021CD RID: 8653 RVA: 0x0009F39A File Offset: 0x0009E39A
		internal string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x060021CE RID: 8654 RVA: 0x0009F3A3 File Offset: 0x0009E3A3
		// (set) Token: 0x060021CF RID: 8655 RVA: 0x0009F3AB File Offset: 0x0009E3AB
		internal long Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x0400147D RID: 5245
		private string xmlName;

		// Token: 0x0400147E RID: 5246
		private string name;

		// Token: 0x0400147F RID: 5247
		private long value;
	}
}

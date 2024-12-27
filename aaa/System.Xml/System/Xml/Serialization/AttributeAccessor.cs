using System;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x020002C3 RID: 707
	internal class AttributeAccessor : Accessor
	{
		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x0600219B RID: 8603 RVA: 0x0009F096 File Offset: 0x0009E096
		internal bool IsSpecialXmlNamespace
		{
			get
			{
				return this.isSpecial;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x0600219C RID: 8604 RVA: 0x0009F09E File Offset: 0x0009E09E
		// (set) Token: 0x0600219D RID: 8605 RVA: 0x0009F0A6 File Offset: 0x0009E0A6
		internal bool IsList
		{
			get
			{
				return this.isList;
			}
			set
			{
				this.isList = value;
			}
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x0009F0B0 File Offset: 0x0009E0B0
		internal void CheckSpecial()
		{
			int num = this.Name.LastIndexOf(':');
			if (num >= 0)
			{
				if (!this.Name.StartsWith("xml:", StringComparison.Ordinal))
				{
					throw new InvalidOperationException(Res.GetString("Xml_InvalidNameChars", new object[] { this.Name }));
				}
				this.Name = this.Name.Substring("xml:".Length);
				base.Namespace = "http://www.w3.org/XML/1998/namespace";
				this.isSpecial = true;
			}
			else if (base.Namespace == "http://www.w3.org/XML/1998/namespace")
			{
				this.isSpecial = true;
			}
			else
			{
				this.isSpecial = false;
			}
			if (this.isSpecial)
			{
				base.Form = XmlSchemaForm.Qualified;
			}
		}

		// Token: 0x0400146B RID: 5227
		private bool isSpecial;

		// Token: 0x0400146C RID: 5228
		private bool isList;
	}
}

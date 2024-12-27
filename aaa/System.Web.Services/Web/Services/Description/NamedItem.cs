using System;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000DE RID: 222
	public abstract class NamedItem : DocumentableItem
	{
		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0001CA9E File Offset: 0x0001BA9E
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x0001CAA6 File Offset: 0x0001BAA6
		[XmlAttribute("name")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x0400043A RID: 1082
		private string name;
	}
}

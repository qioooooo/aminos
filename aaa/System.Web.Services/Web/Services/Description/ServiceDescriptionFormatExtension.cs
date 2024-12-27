using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000B5 RID: 181
	public abstract class ServiceDescriptionFormatExtension
	{
		// Token: 0x060004C2 RID: 1218 RVA: 0x00017DA7 File Offset: 0x00016DA7
		internal void SetParent(object parent)
		{
			this.parent = parent;
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00017DB0 File Offset: 0x00016DB0
		public object Parent
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00017DB8 File Offset: 0x00016DB8
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x00017DC0 File Offset: 0x00016DC0
		[DefaultValue(false)]
		[XmlAttribute("required", Namespace = "http://schemas.xmlsoap.org/wsdl/")]
		public bool Required
		{
			get
			{
				return this.required;
			}
			set
			{
				this.required = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00017DC9 File Offset: 0x00016DC9
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x00017DD1 File Offset: 0x00016DD1
		[XmlIgnore]
		public bool Handled
		{
			get
			{
				return this.handled;
			}
			set
			{
				this.handled = value;
			}
		}

		// Token: 0x040003DE RID: 990
		private object parent;

		// Token: 0x040003DF RID: 991
		private bool required;

		// Token: 0x040003E0 RID: 992
		private bool handled;
	}
}

using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000B8 RID: 184
	[XmlFormatExtension("operation", "http://schemas.xmlsoap.org/wsdl/http/", typeof(OperationBinding))]
	public sealed class HttpOperationBinding : ServiceDescriptionFormatExtension
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00017E22 File Offset: 0x00016E22
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x00017E38 File Offset: 0x00016E38
		[XmlAttribute("location")]
		public string Location
		{
			get
			{
				if (this.location != null)
				{
					return this.location;
				}
				return string.Empty;
			}
			set
			{
				this.location = value;
			}
		}

		// Token: 0x040003E4 RID: 996
		private string location;
	}
}

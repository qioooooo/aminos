using System;

namespace System.Web.Management
{
	// Token: 0x020002E0 RID: 736
	internal class WebEventFieldData
	{
		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06002533 RID: 9523 RVA: 0x0009FDF5 File Offset: 0x0009EDF5
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06002534 RID: 9524 RVA: 0x0009FDFD File Offset: 0x0009EDFD
		public string Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06002535 RID: 9525 RVA: 0x0009FE05 File Offset: 0x0009EE05
		public WebEventFieldType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x0009FE0D File Offset: 0x0009EE0D
		public WebEventFieldData(string name, string data, WebEventFieldType type)
		{
			this._name = name;
			this._data = data;
			this._type = type;
		}

		// Token: 0x04001D3D RID: 7485
		private string _name;

		// Token: 0x04001D3E RID: 7486
		private string _data;

		// Token: 0x04001D3F RID: 7487
		private WebEventFieldType _type;
	}
}

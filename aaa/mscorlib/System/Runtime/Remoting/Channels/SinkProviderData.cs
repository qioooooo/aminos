using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006DA RID: 1754
	[ComVisible(true)]
	public class SinkProviderData
	{
		// Token: 0x06003F11 RID: 16145 RVA: 0x000D82DB File Offset: 0x000D72DB
		public SinkProviderData(string name)
		{
			this._name = name;
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06003F12 RID: 16146 RVA: 0x000D8305 File Offset: 0x000D7305
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06003F13 RID: 16147 RVA: 0x000D830D File Offset: 0x000D730D
		public IDictionary Properties
		{
			get
			{
				return this._properties;
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06003F14 RID: 16148 RVA: 0x000D8315 File Offset: 0x000D7315
		public IList Children
		{
			get
			{
				return this._children;
			}
		}

		// Token: 0x04001FCB RID: 8139
		private string _name;

		// Token: 0x04001FCC RID: 8140
		private Hashtable _properties = new Hashtable(StringComparer.InvariantCultureIgnoreCase);

		// Token: 0x04001FCD RID: 8141
		private ArrayList _children = new ArrayList();
	}
}

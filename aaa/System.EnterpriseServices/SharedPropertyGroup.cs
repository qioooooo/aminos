using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200001F RID: 31
	[ComVisible(false)]
	public sealed class SharedPropertyGroup
	{
		// Token: 0x0600005E RID: 94 RVA: 0x000020FA File Offset: 0x000010FA
		internal SharedPropertyGroup(ISharedPropertyGroup grp)
		{
			this._x = grp;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002109 File Offset: 0x00001109
		public SharedProperty CreatePropertyByPosition(int position, out bool fExists)
		{
			return new SharedProperty(this._x.CreatePropertyByPosition(position, out fExists));
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000211D File Offset: 0x0000111D
		public SharedProperty PropertyByPosition(int position)
		{
			return new SharedProperty(this._x.PropertyByPosition(position));
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002130 File Offset: 0x00001130
		public SharedProperty CreateProperty(string name, out bool fExists)
		{
			return new SharedProperty(this._x.CreateProperty(name, out fExists));
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002144 File Offset: 0x00001144
		public SharedProperty Property(string name)
		{
			return new SharedProperty(this._x.Property(name));
		}

		// Token: 0x0400001D RID: 29
		private ISharedPropertyGroup _x;
	}
}

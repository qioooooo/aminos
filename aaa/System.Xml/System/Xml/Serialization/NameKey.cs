using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002DE RID: 734
	internal class NameKey
	{
		// Token: 0x06002263 RID: 8803 RVA: 0x000A0F48 File Offset: 0x0009FF48
		internal NameKey(string name, string ns)
		{
			this.name = name;
			this.ns = ns;
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x000A0F60 File Offset: 0x0009FF60
		public override bool Equals(object other)
		{
			if (!(other is NameKey))
			{
				return false;
			}
			NameKey nameKey = (NameKey)other;
			return this.name == nameKey.name && this.ns == nameKey.ns;
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x000A0FA4 File Offset: 0x0009FFA4
		public override int GetHashCode()
		{
			return ((this.ns == null) ? "<null>".GetHashCode() : this.ns.GetHashCode()) ^ ((this.name == null) ? 0 : this.name.GetHashCode());
		}

		// Token: 0x040014C0 RID: 5312
		private string ns;

		// Token: 0x040014C1 RID: 5313
		private string name;
	}
}

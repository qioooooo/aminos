using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x02000303 RID: 771
	public class XmlAttributeOverrides
	{
		// Token: 0x06002419 RID: 9241 RVA: 0x000AA62D File Offset: 0x000A962D
		public void Add(Type type, XmlAttributes attributes)
		{
			this.Add(type, string.Empty, attributes);
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x000AA63C File Offset: 0x000A963C
		public void Add(Type type, string member, XmlAttributes attributes)
		{
			Hashtable hashtable = (Hashtable)this.types[type];
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				this.types.Add(type, hashtable);
			}
			else if (hashtable[member] != null)
			{
				throw new InvalidOperationException(Res.GetString("XmlAttributeSetAgain", new object[] { type.FullName, member }));
			}
			hashtable.Add(member, attributes);
		}

		// Token: 0x170008DC RID: 2268
		public XmlAttributes this[Type type]
		{
			get
			{
				return this[type, string.Empty];
			}
		}

		// Token: 0x170008DD RID: 2269
		public XmlAttributes this[Type type, string member]
		{
			get
			{
				Hashtable hashtable = (Hashtable)this.types[type];
				if (hashtable == null)
				{
					return null;
				}
				return (XmlAttributes)hashtable[member];
			}
		}

		// Token: 0x04001552 RID: 5458
		private Hashtable types = new Hashtable();
	}
}

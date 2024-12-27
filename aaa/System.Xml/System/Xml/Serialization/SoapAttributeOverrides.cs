using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x020002E7 RID: 743
	public class SoapAttributeOverrides
	{
		// Token: 0x060022CA RID: 8906 RVA: 0x000A3AA9 File Offset: 0x000A2AA9
		public void Add(Type type, SoapAttributes attributes)
		{
			this.Add(type, string.Empty, attributes);
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x000A3AB8 File Offset: 0x000A2AB8
		public void Add(Type type, string member, SoapAttributes attributes)
		{
			Hashtable hashtable = (Hashtable)this.types[type];
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				this.types.Add(type, hashtable);
			}
			else if (hashtable[member] != null)
			{
				throw new InvalidOperationException(Res.GetString("XmlMultipleAttributeOverrides", new object[] { type.FullName, member }));
			}
			hashtable.Add(member, attributes);
		}

		// Token: 0x1700087D RID: 2173
		public SoapAttributes this[Type type]
		{
			get
			{
				return this[type, string.Empty];
			}
		}

		// Token: 0x1700087E RID: 2174
		public SoapAttributes this[Type type, string member]
		{
			get
			{
				Hashtable hashtable = (Hashtable)this.types[type];
				if (hashtable == null)
				{
					return null;
				}
				return (SoapAttributes)hashtable[member];
			}
		}

		// Token: 0x040014D3 RID: 5331
		private Hashtable types = new Hashtable();
	}
}

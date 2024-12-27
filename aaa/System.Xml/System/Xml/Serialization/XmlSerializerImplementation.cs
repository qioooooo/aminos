using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x02000336 RID: 822
	public abstract class XmlSerializerImplementation
	{
		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x0600282F RID: 10287 RVA: 0x000D0548 File Offset: 0x000CF548
		public virtual XmlSerializationReader Reader
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06002830 RID: 10288 RVA: 0x000D054F File Offset: 0x000CF54F
		public virtual XmlSerializationWriter Writer
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06002831 RID: 10289 RVA: 0x000D0556 File Offset: 0x000CF556
		public virtual Hashtable ReadMethods
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06002832 RID: 10290 RVA: 0x000D055D File Offset: 0x000CF55D
		public virtual Hashtable WriteMethods
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06002833 RID: 10291 RVA: 0x000D0564 File Offset: 0x000CF564
		public virtual Hashtable TypedSerializers
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x000D056B File Offset: 0x000CF56B
		public virtual bool CanSerialize(Type type)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x000D0572 File Offset: 0x000CF572
		public virtual XmlSerializer GetSerializer(Type type)
		{
			throw new NotSupportedException();
		}
	}
}

using System;
using System.Collections;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000693 RID: 1683
	internal class IllogicalCallContext : ICloneable
	{
		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06003D42 RID: 15682 RVA: 0x000D23BB File Offset: 0x000D13BB
		private Hashtable Datastore
		{
			get
			{
				if (this.m_Datastore == null)
				{
					this.m_Datastore = new Hashtable();
				}
				return this.m_Datastore;
			}
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06003D43 RID: 15683 RVA: 0x000D23D6 File Offset: 0x000D13D6
		// (set) Token: 0x06003D44 RID: 15684 RVA: 0x000D23DE File Offset: 0x000D13DE
		internal object HostContext
		{
			get
			{
				return this.m_HostContext;
			}
			set
			{
				this.m_HostContext = value;
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06003D45 RID: 15685 RVA: 0x000D23E7 File Offset: 0x000D13E7
		internal bool HasUserData
		{
			get
			{
				return this.m_Datastore != null && this.m_Datastore.Count > 0;
			}
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x000D2401 File Offset: 0x000D1401
		public void FreeNamedDataSlot(string name)
		{
			this.Datastore.Remove(name);
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x000D240F File Offset: 0x000D140F
		public object GetData(string name)
		{
			return this.Datastore[name];
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x000D241D File Offset: 0x000D141D
		public void SetData(string name, object data)
		{
			this.Datastore[name] = data;
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x000D242C File Offset: 0x000D142C
		public object Clone()
		{
			IllogicalCallContext illogicalCallContext = new IllogicalCallContext();
			if (this.HasUserData)
			{
				IDictionaryEnumerator enumerator = this.m_Datastore.GetEnumerator();
				while (enumerator.MoveNext())
				{
					illogicalCallContext.Datastore[(string)enumerator.Key] = enumerator.Value;
				}
			}
			return illogicalCallContext;
		}

		// Token: 0x04001F27 RID: 7975
		private Hashtable m_Datastore;

		// Token: 0x04001F28 RID: 7976
		private object m_HostContext;
	}
}

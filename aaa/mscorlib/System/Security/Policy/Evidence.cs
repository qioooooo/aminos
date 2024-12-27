using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x0200048A RID: 1162
	[ComVisible(true)]
	[Serializable]
	public sealed class Evidence : ICollection, IEnumerable
	{
		// Token: 0x06002EA8 RID: 11944 RVA: 0x0009EB01 File Offset: 0x0009DB01
		public Evidence()
		{
			this.m_hostList = null;
			this.m_assemblyList = null;
			this.m_locked = false;
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x0009EB1E File Offset: 0x0009DB1E
		public Evidence(Evidence evidence)
		{
			if (evidence == null)
			{
				return;
			}
			this.m_locked = false;
			this.Merge(evidence);
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x0009EB38 File Offset: 0x0009DB38
		public Evidence(object[] hostEvidence, object[] assemblyEvidence)
		{
			this.m_locked = false;
			if (hostEvidence != null)
			{
				this.m_hostList = ArrayList.Synchronized(new ArrayList(hostEvidence));
			}
			if (assemblyEvidence != null)
			{
				this.m_assemblyList = ArrayList.Synchronized(new ArrayList(assemblyEvidence));
			}
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x0009EB70 File Offset: 0x0009DB70
		internal Evidence(char[] buffer)
		{
			int i = 0;
			while (i < buffer.Length)
			{
				switch (buffer[i++])
				{
				case '\0':
				{
					IBuiltInEvidence builtInEvidence = new ApplicationDirectory();
					i = builtInEvidence.InitFromBuffer(buffer, i);
					this.AddAssembly(builtInEvidence);
					continue;
				}
				case '\u0001':
				{
					IBuiltInEvidence builtInEvidence2 = new Publisher();
					i = builtInEvidence2.InitFromBuffer(buffer, i);
					this.AddHost(builtInEvidence2);
					continue;
				}
				case '\u0002':
				{
					IBuiltInEvidence builtInEvidence3 = new StrongName();
					i = builtInEvidence3.InitFromBuffer(buffer, i);
					this.AddHost(builtInEvidence3);
					continue;
				}
				case '\u0003':
				{
					IBuiltInEvidence builtInEvidence4 = new Zone();
					i = builtInEvidence4.InitFromBuffer(buffer, i);
					this.AddHost(builtInEvidence4);
					continue;
				}
				case '\u0004':
				{
					IBuiltInEvidence builtInEvidence5 = new Url();
					i = builtInEvidence5.InitFromBuffer(buffer, i);
					this.AddHost(builtInEvidence5);
					continue;
				}
				case '\u0006':
				{
					IBuiltInEvidence builtInEvidence6 = new Site();
					i = builtInEvidence6.InitFromBuffer(buffer, i);
					this.AddHost(builtInEvidence6);
					continue;
				}
				case '\a':
				{
					IBuiltInEvidence builtInEvidence7 = new PermissionRequestEvidence();
					i = builtInEvidence7.InitFromBuffer(buffer, i);
					this.AddHost(builtInEvidence7);
					continue;
				}
				case '\b':
				{
					IBuiltInEvidence builtInEvidence8 = new Hash();
					i = builtInEvidence8.InitFromBuffer(buffer, i);
					this.AddHost(builtInEvidence8);
					continue;
				}
				case '\t':
				{
					IBuiltInEvidence builtInEvidence9 = new GacInstalled();
					i = builtInEvidence9.InitFromBuffer(buffer, i);
					this.AddHost(builtInEvidence9);
					continue;
				}
				}
				throw new SerializationException(Environment.GetResourceString("Serialization_UnableToFixup"));
			}
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x0009ECD6 File Offset: 0x0009DCD6
		public void AddHost(object id)
		{
			if (this.m_hostList == null)
			{
				this.m_hostList = ArrayList.Synchronized(new ArrayList());
			}
			if (this.m_locked)
			{
				new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Demand();
			}
			this.m_hostList.Add(id);
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x0009ED11 File Offset: 0x0009DD11
		public void AddAssembly(object id)
		{
			if (this.m_assemblyList == null)
			{
				this.m_assemblyList = ArrayList.Synchronized(new ArrayList());
			}
			this.m_assemblyList.Add(id);
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06002EAE RID: 11950 RVA: 0x0009ED38 File Offset: 0x0009DD38
		// (set) Token: 0x06002EAF RID: 11951 RVA: 0x0009ED40 File Offset: 0x0009DD40
		public bool Locked
		{
			get
			{
				return this.m_locked;
			}
			set
			{
				if (!value)
				{
					new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Demand();
					this.m_locked = false;
					return;
				}
				this.m_locked = true;
			}
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x0009ED60 File Offset: 0x0009DD60
		public void Merge(Evidence evidence)
		{
			if (evidence == null)
			{
				return;
			}
			if (evidence.m_hostList != null)
			{
				if (this.m_hostList == null)
				{
					this.m_hostList = ArrayList.Synchronized(new ArrayList());
				}
				if (evidence.m_hostList.Count != 0 && this.m_locked)
				{
					new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Demand();
				}
				foreach (object obj in evidence.m_hostList)
				{
					this.m_hostList.Add(obj);
				}
			}
			if (evidence.m_assemblyList != null)
			{
				if (this.m_assemblyList == null)
				{
					this.m_assemblyList = ArrayList.Synchronized(new ArrayList());
				}
				foreach (object obj2 in evidence.m_assemblyList)
				{
					this.m_assemblyList.Add(obj2);
				}
			}
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x0009EE24 File Offset: 0x0009DE24
		internal void MergeWithNoDuplicates(Evidence evidence)
		{
			if (evidence == null)
			{
				return;
			}
			IEnumerator enumerator;
			if (evidence.m_hostList != null)
			{
				if (this.m_hostList == null)
				{
					this.m_hostList = ArrayList.Synchronized(new ArrayList());
				}
				foreach (object obj in evidence.m_hostList)
				{
					Type type = obj.GetType();
					IEnumerator enumerator2 = this.m_hostList.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.GetType() == type)
						{
							this.m_hostList.Remove(enumerator2.Current);
							break;
						}
					}
					this.m_hostList.Add(enumerator.Current);
				}
			}
			if (evidence.m_assemblyList != null)
			{
				if (this.m_assemblyList == null)
				{
					this.m_assemblyList = ArrayList.Synchronized(new ArrayList());
				}
				foreach (object obj2 in evidence.m_assemblyList)
				{
					Type type2 = obj2.GetType();
					IEnumerator enumerator2 = this.m_assemblyList.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.GetType() == type2)
						{
							this.m_assemblyList.Remove(enumerator2.Current);
							break;
						}
					}
					this.m_assemblyList.Add(enumerator.Current);
				}
			}
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x0009EF54 File Offset: 0x0009DF54
		public void CopyTo(Array array, int index)
		{
			int num = index;
			if (this.m_hostList != null)
			{
				this.m_hostList.CopyTo(array, num);
				num += this.m_hostList.Count;
			}
			if (this.m_assemblyList != null)
			{
				this.m_assemblyList.CopyTo(array, num);
			}
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x0009EF9B File Offset: 0x0009DF9B
		public IEnumerator GetHostEnumerator()
		{
			if (this.m_hostList == null)
			{
				this.m_hostList = ArrayList.Synchronized(new ArrayList());
			}
			return this.m_hostList.GetEnumerator();
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x0009EFC0 File Offset: 0x0009DFC0
		public IEnumerator GetAssemblyEnumerator()
		{
			if (this.m_assemblyList == null)
			{
				this.m_assemblyList = ArrayList.Synchronized(new ArrayList());
			}
			return this.m_assemblyList.GetEnumerator();
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x0009EFE5 File Offset: 0x0009DFE5
		public IEnumerator GetEnumerator()
		{
			return new EvidenceEnumerator(this);
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06002EB6 RID: 11958 RVA: 0x0009EFED File Offset: 0x0009DFED
		public int Count
		{
			get
			{
				return ((this.m_hostList != null) ? this.m_hostList.Count : 0) + ((this.m_assemblyList != null) ? this.m_assemblyList.Count : 0);
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06002EB7 RID: 11959 RVA: 0x0009F01C File Offset: 0x0009E01C
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06002EB8 RID: 11960 RVA: 0x0009F01F File Offset: 0x0009E01F
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06002EB9 RID: 11961 RVA: 0x0009F022 File Offset: 0x0009E022
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x0009F028 File Offset: 0x0009E028
		internal Evidence Copy()
		{
			char[] array = PolicyManager.MakeEvidenceArray(this, true);
			if (array != null)
			{
				return new Evidence(array);
			}
			new PermissionSet(true).Assert();
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (Evidence)binaryFormatter.Deserialize(memoryStream);
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x0009F07C File Offset: 0x0009E07C
		internal Evidence ShallowCopy()
		{
			Evidence evidence = new Evidence();
			IEnumerator enumerator = this.GetHostEnumerator();
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				evidence.AddHost(obj);
			}
			enumerator = this.GetAssemblyEnumerator();
			while (enumerator.MoveNext())
			{
				object obj2 = enumerator.Current;
				evidence.AddAssembly(obj2);
			}
			return evidence;
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x0009F0CA File Offset: 0x0009E0CA
		[ComVisible(false)]
		public void Clear()
		{
			this.m_hostList = null;
			this.m_assemblyList = null;
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x0009F0DC File Offset: 0x0009E0DC
		[ComVisible(false)]
		public void RemoveType(Type t)
		{
			for (int i = 0; i < ((this.m_hostList == null) ? 0 : this.m_hostList.Count); i++)
			{
				if (this.m_hostList[i].GetType() == t)
				{
					this.m_hostList.RemoveAt(i--);
				}
			}
			for (int i = 0; i < ((this.m_assemblyList == null) ? 0 : this.m_assemblyList.Count); i++)
			{
				if (this.m_assemblyList[i].GetType() == t)
				{
					this.m_assemblyList.RemoveAt(i--);
				}
			}
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x0009F174 File Offset: 0x0009E174
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			Evidence evidence = obj as Evidence;
			if (evidence == null)
			{
				return false;
			}
			if (this.m_hostList != null && evidence.m_hostList != null)
			{
				if (this.m_hostList.Count != evidence.m_hostList.Count)
				{
					return false;
				}
				int count = this.m_hostList.Count;
				for (int i = 0; i < count; i++)
				{
					bool flag = false;
					for (int j = 0; j < count; j++)
					{
						if (object.Equals(this.m_hostList[i], evidence.m_hostList[j]))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			else if (this.m_hostList != null || evidence.m_hostList != null)
			{
				return false;
			}
			if (this.m_assemblyList != null && evidence.m_assemblyList != null)
			{
				if (this.m_assemblyList.Count != evidence.m_assemblyList.Count)
				{
					return false;
				}
				int count2 = this.m_assemblyList.Count;
				for (int k = 0; k < count2; k++)
				{
					bool flag2 = false;
					for (int l = 0; l < count2; l++)
					{
						if (object.Equals(this.m_assemblyList[k], evidence.m_assemblyList[l]))
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						return false;
					}
				}
			}
			else if (this.m_assemblyList != null || evidence.m_assemblyList != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x0009F2C0 File Offset: 0x0009E2C0
		[ComVisible(false)]
		public override int GetHashCode()
		{
			int num = 0;
			if (this.m_hostList != null)
			{
				int count = this.m_hostList.Count;
				for (int i = 0; i < count; i++)
				{
					num ^= this.m_hostList[i].GetHashCode();
				}
			}
			if (this.m_assemblyList != null)
			{
				int count2 = this.m_assemblyList.Count;
				for (int j = 0; j < count2; j++)
				{
					num ^= this.m_assemblyList[j].GetHashCode();
				}
			}
			return num;
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x0009F340 File Offset: 0x0009E340
		internal object FindType(Type t)
		{
			for (int i = 0; i < ((this.m_hostList == null) ? 0 : this.m_hostList.Count); i++)
			{
				if (this.m_hostList[i].GetType() == t)
				{
					return this.m_hostList[i];
				}
			}
			for (int i = 0; i < ((this.m_assemblyList == null) ? 0 : this.m_assemblyList.Count); i++)
			{
				if (this.m_assemblyList[i].GetType() == t)
				{
					return this.m_hostList[i];
				}
			}
			return null;
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x0009F3D4 File Offset: 0x0009E3D4
		internal void MarkAllEvidenceAsUsed()
		{
			foreach (object obj in this)
			{
				IDelayEvaluatedEvidence delayEvaluatedEvidence = obj as IDelayEvaluatedEvidence;
				if (delayEvaluatedEvidence != null)
				{
					delayEvaluatedEvidence.MarkUsed();
				}
			}
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x0009F42C File Offset: 0x0009E42C
		private bool WasStrongNameEvidenceUsed()
		{
			IDelayEvaluatedEvidence delayEvaluatedEvidence = this.FindType(typeof(StrongName)) as IDelayEvaluatedEvidence;
			return delayEvaluatedEvidence != null && delayEvaluatedEvidence.WasUsed;
		}

		// Token: 0x040017A4 RID: 6052
		private IList m_hostList;

		// Token: 0x040017A5 RID: 6053
		private IList m_assemblyList;

		// Token: 0x040017A6 RID: 6054
		private bool m_locked;
	}
}

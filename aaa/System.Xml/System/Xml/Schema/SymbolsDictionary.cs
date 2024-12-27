using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x02000192 RID: 402
	internal class SymbolsDictionary
	{
		// Token: 0x06001535 RID: 5429 RVA: 0x0005E926 File Offset: 0x0005D926
		public SymbolsDictionary()
		{
			this.names = new Hashtable();
			this.particles = new ArrayList();
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x0005E94B File Offset: 0x0005D94B
		public int Count
		{
			get
			{
				return this.last + 1;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001537 RID: 5431 RVA: 0x0005E955 File Offset: 0x0005D955
		public int CountOfNames
		{
			get
			{
				return this.names.Count;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001538 RID: 5432 RVA: 0x0005E962 File Offset: 0x0005D962
		// (set) Token: 0x06001539 RID: 5433 RVA: 0x0005E96A File Offset: 0x0005D96A
		public bool IsUpaEnforced
		{
			get
			{
				return this.isUpaEnforced;
			}
			set
			{
				this.isUpaEnforced = value;
			}
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0005E974 File Offset: 0x0005D974
		public int AddName(XmlQualifiedName name, object particle)
		{
			object obj = this.names[name];
			if (obj != null)
			{
				int num = (int)obj;
				if (this.particles[num] != particle)
				{
					this.isUpaEnforced = false;
				}
				return num;
			}
			this.names.Add(name, this.last);
			this.particles.Add(particle);
			return this.last++;
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x0005E9E8 File Offset: 0x0005D9E8
		public void AddNamespaceList(NamespaceList list, object particle, bool allowLocal)
		{
			switch (list.Type)
			{
			case NamespaceList.ListType.Any:
				this.particleLast = particle;
				return;
			case NamespaceList.ListType.Other:
				this.AddWildcard(list.Excluded, null);
				if (!allowLocal)
				{
					this.AddWildcard(string.Empty, null);
					return;
				}
				break;
			case NamespaceList.ListType.Set:
				foreach (object obj in list.Enumerate)
				{
					string text = (string)obj;
					this.AddWildcard(text, particle);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x0005EA84 File Offset: 0x0005DA84
		private void AddWildcard(string wildcard, object particle)
		{
			if (this.wildcards == null)
			{
				this.wildcards = new Hashtable();
			}
			object obj = this.wildcards[wildcard];
			if (obj == null)
			{
				this.wildcards.Add(wildcard, this.last);
				this.particles.Add(particle);
				this.last++;
				return;
			}
			if (particle != null)
			{
				this.particles[(int)obj] = particle;
			}
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0005EAFC File Offset: 0x0005DAFC
		public ICollection GetNamespaceListSymbols(NamespaceList list)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.names.Keys)
			{
				XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)obj;
				if (xmlQualifiedName != XmlQualifiedName.Empty && list.Allows(xmlQualifiedName))
				{
					arrayList.Add(this.names[xmlQualifiedName]);
				}
			}
			if (this.wildcards != null)
			{
				foreach (object obj2 in this.wildcards.Keys)
				{
					string text = (string)obj2;
					if (list.Allows(text))
					{
						arrayList.Add(this.wildcards[text]);
					}
				}
			}
			if (list.Type == NamespaceList.ListType.Any || list.Type == NamespaceList.ListType.Other)
			{
				arrayList.Add(this.last);
			}
			return arrayList;
		}

		// Token: 0x17000519 RID: 1305
		public int this[XmlQualifiedName name]
		{
			get
			{
				object obj = this.names[name];
				if (obj != null)
				{
					return (int)obj;
				}
				if (this.wildcards != null)
				{
					obj = this.wildcards[name.Namespace];
					if (obj != null)
					{
						return (int)obj;
					}
				}
				return this.last;
			}
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0005EC6C File Offset: 0x0005DC6C
		public bool Exists(XmlQualifiedName name)
		{
			return this.names[name] != null;
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0005EC8C File Offset: 0x0005DC8C
		public object GetParticle(int symbol)
		{
			if (symbol != this.last)
			{
				return this.particles[symbol];
			}
			return this.particleLast;
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x0005ECAC File Offset: 0x0005DCAC
		public string NameOf(int symbol)
		{
			foreach (object obj in this.names)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if ((int)dictionaryEntry.Value == symbol)
				{
					return ((XmlQualifiedName)dictionaryEntry.Key).ToString();
				}
			}
			if (this.wildcards != null)
			{
				foreach (object obj2 in this.wildcards)
				{
					DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
					if ((int)dictionaryEntry2.Value == symbol)
					{
						return (string)dictionaryEntry2.Key + ":*";
					}
				}
			}
			return "##other:*";
		}

		// Token: 0x04000CBB RID: 3259
		private int last;

		// Token: 0x04000CBC RID: 3260
		private Hashtable names;

		// Token: 0x04000CBD RID: 3261
		private Hashtable wildcards;

		// Token: 0x04000CBE RID: 3262
		private ArrayList particles;

		// Token: 0x04000CBF RID: 3263
		private object particleLast;

		// Token: 0x04000CC0 RID: 3264
		private bool isUpaEnforced = true;
	}
}

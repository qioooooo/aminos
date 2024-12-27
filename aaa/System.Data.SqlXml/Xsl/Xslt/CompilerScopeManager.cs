using System;
using System.Collections;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000E7 RID: 231
	internal sealed class CompilerScopeManager<V> : IEnumerable where V : class
	{
		// Token: 0x06000A9B RID: 2715 RVA: 0x00033380 File Offset: 0x00032380
		public CompilerScopeManager()
		{
			this.Reset();
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0003339B File Offset: 0x0003239B
		private void Reset()
		{
			this.records[0].ncName = "xml";
			this.records[0].nsUri = "http://www.w3.org/XML/1998/namespace";
			this.lastRecord = 0;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x000333D0 File Offset: 0x000323D0
		public void PushScope()
		{
			this.lastScopes++;
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x000333E0 File Offset: 0x000323E0
		public void PopScope()
		{
			if (0 < this.lastScopes)
			{
				this.lastScopes--;
				return;
			}
			while (this.records[--this.lastRecord].scopeCount == 0)
			{
			}
			this.lastScopes = this.records[this.lastRecord].scopeCount;
			this.lastScopes--;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00033454 File Offset: 0x00032454
		private void AddRecord(string ncName, string uri, V value)
		{
			this.records[this.lastRecord].scopeCount = this.lastScopes;
			if (++this.lastRecord == this.records.Length)
			{
				CompilerScopeManager<V>.ScopeRecord[] array = new CompilerScopeManager<V>.ScopeRecord[this.lastRecord * 2];
				Array.Copy(this.records, 0, array, 0, this.lastRecord);
				this.records = array;
			}
			this.lastScopes = 0;
			this.records[this.lastRecord].ncName = ncName;
			this.records[this.lastRecord].nsUri = uri;
			this.records[this.lastRecord].value = value;
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00033510 File Offset: 0x00032510
		public void AddNamespace(string prefix, string uri)
		{
			this.AddRecord(prefix, uri, default(V));
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0003352E File Offset: 0x0003252E
		public void AddVariable(QilName varName, V value)
		{
			this.AddRecord(varName.LocalName, varName.NamespaceUri, value);
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00033544 File Offset: 0x00032544
		private string LookupNamespace(string prefix, int from, int to)
		{
			int num = from;
			while (to <= num)
			{
				if (this.records[num].IsNamespace && this.records[num].ncName == prefix)
				{
					return this.records[num].nsUri;
				}
				num--;
			}
			return null;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0003359C File Offset: 0x0003259C
		public string LookupNamespace(string prefix)
		{
			return this.LookupNamespace(prefix, this.lastRecord, 0);
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x000335AC File Offset: 0x000325AC
		public bool IsExNamespace(string nsUri)
		{
			int num = this.lastRecord;
			while (0 <= num)
			{
				if (this.records[num].IsExNamespace && this.records[num].nsUri == nsUri)
				{
					return true;
				}
				num--;
			}
			return false;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x000335FC File Offset: 0x000325FC
		private int SearchVariable(string localName, string uri)
		{
			int num = this.lastRecord;
			while (0 <= num)
			{
				if (this.records[num].IsVariable && this.records[num].ncName == localName && this.records[num].nsUri == uri)
				{
					return num;
				}
				num--;
			}
			return -1;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00033664 File Offset: 0x00032664
		public V LookupVariable(string localName, string uri)
		{
			int num = this.SearchVariable(localName, uri);
			if (num >= 0)
			{
				return this.records[num].value;
			}
			return default(V);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0003369C File Offset: 0x0003269C
		public bool IsLocalVariable(string localName, string uri)
		{
			int num = this.SearchVariable(localName, uri);
			while (0 <= --num)
			{
				if (this.records[num].scopeCount != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x000336D2 File Offset: 0x000326D2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CompilerScopeManager<V>.NamespaceEnumerator(this);
		}

		// Token: 0x04000717 RID: 1815
		private const int LastPredefRecord = 0;

		// Token: 0x04000718 RID: 1816
		private CompilerScopeManager<V>.ScopeRecord[] records = new CompilerScopeManager<V>.ScopeRecord[32];

		// Token: 0x04000719 RID: 1817
		private int lastRecord;

		// Token: 0x0400071A RID: 1818
		private int lastScopes;

		// Token: 0x020000E8 RID: 232
		public struct ScopeRecord
		{
			// Token: 0x17000175 RID: 373
			// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x000336DC File Offset: 0x000326DC
			public bool IsVariable
			{
				get
				{
					return this.value != default(V);
				}
			}

			// Token: 0x17000176 RID: 374
			// (get) Token: 0x06000AAA RID: 2730 RVA: 0x00033708 File Offset: 0x00032708
			public bool IsNamespace
			{
				get
				{
					return this.value == default(V) && this.ncName != null;
				}
			}

			// Token: 0x17000177 RID: 375
			// (get) Token: 0x06000AAB RID: 2731 RVA: 0x00033740 File Offset: 0x00032740
			public bool IsExNamespace
			{
				get
				{
					return this.value == default(V) && this.ncName == null;
				}
			}

			// Token: 0x0400071B RID: 1819
			public int scopeCount;

			// Token: 0x0400071C RID: 1820
			public string ncName;

			// Token: 0x0400071D RID: 1821
			public string nsUri;

			// Token: 0x0400071E RID: 1822
			public V value;
		}

		// Token: 0x020000E9 RID: 233
		private sealed class NamespaceEnumerator : IEnumerator
		{
			// Token: 0x06000AAC RID: 2732 RVA: 0x00033773 File Offset: 0x00032773
			public NamespaceEnumerator(CompilerScopeManager<V> scope)
			{
				this.scope = scope;
				this.lastRecord = scope.lastRecord;
				this.Reset();
			}

			// Token: 0x06000AAD RID: 2733 RVA: 0x00033794 File Offset: 0x00032794
			public void Reset()
			{
				this.currentRecord = this.lastRecord + 1;
			}

			// Token: 0x06000AAE RID: 2734 RVA: 0x000337A4 File Offset: 0x000327A4
			public bool MoveNext()
			{
				while (0 < --this.currentRecord)
				{
					if (this.scope.records[this.currentRecord].IsNamespace && this.scope.LookupNamespace(this.scope.records[this.currentRecord].ncName, this.lastRecord, this.currentRecord + 1) == null)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0003381D File Offset: 0x0003281D
			public object Current
			{
				get
				{
					return this.scope.records[this.currentRecord];
				}
			}

			// Token: 0x0400071F RID: 1823
			private CompilerScopeManager<V> scope;

			// Token: 0x04000720 RID: 1824
			private int lastRecord;

			// Token: 0x04000721 RID: 1825
			private int currentRecord;
		}
	}
}

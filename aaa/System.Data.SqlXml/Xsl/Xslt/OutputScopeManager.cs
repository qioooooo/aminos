using System;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000FA RID: 250
	internal class OutputScopeManager
	{
		// Token: 0x06000AFA RID: 2810 RVA: 0x00035644 File Offset: 0x00034644
		public OutputScopeManager()
		{
			this.Reset();
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0003565F File Offset: 0x0003465F
		public void Reset()
		{
			this.records[0].prefix = null;
			this.records[0].nsUri = null;
			this.PushScope();
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0003568B File Offset: 0x0003468B
		public void PushScope()
		{
			this.lastScopes++;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0003569C File Offset: 0x0003469C
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

		// Token: 0x06000AFE RID: 2814 RVA: 0x0003570E File Offset: 0x0003470E
		public void AddNamespace(string prefix, string uri)
		{
			this.AddRecord(prefix, uri);
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00035718 File Offset: 0x00034718
		private void AddRecord(string prefix, string uri)
		{
			this.records[this.lastRecord].scopeCount = this.lastScopes;
			this.lastRecord++;
			if (this.lastRecord == this.records.Length)
			{
				OutputScopeManager.ScopeReord[] array = new OutputScopeManager.ScopeReord[this.lastRecord * 2];
				Array.Copy(this.records, 0, array, 0, this.lastRecord);
				this.records = array;
			}
			this.lastScopes = 0;
			this.records[this.lastRecord].prefix = prefix;
			this.records[this.lastRecord].nsUri = uri;
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x000357BD File Offset: 0x000347BD
		public void InvalidateAllPrefixes()
		{
			if (this.records[this.lastRecord].prefix == null)
			{
				return;
			}
			this.AddRecord(null, null);
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x000357E0 File Offset: 0x000347E0
		public void InvalidateNonDefaultPrefixes()
		{
			string text = this.LookupNamespace(string.Empty);
			if (text == null)
			{
				this.InvalidateAllPrefixes();
				return;
			}
			if (this.records[this.lastRecord].prefix.Length == 0 && this.records[this.lastRecord - 1].prefix == null)
			{
				return;
			}
			this.AddRecord(null, null);
			this.AddRecord(string.Empty, text);
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x00035850 File Offset: 0x00034850
		public string LookupNamespace(string prefix)
		{
			int num = this.lastRecord;
			while (this.records[num].prefix != null)
			{
				if (this.records[num].prefix == prefix)
				{
					return this.records[num].nsUri;
				}
				num--;
			}
			return null;
		}

		// Token: 0x040007B9 RID: 1977
		private OutputScopeManager.ScopeReord[] records = new OutputScopeManager.ScopeReord[32];

		// Token: 0x040007BA RID: 1978
		private int lastRecord;

		// Token: 0x040007BB RID: 1979
		private int lastScopes;

		// Token: 0x020000FB RID: 251
		public struct ScopeReord
		{
			// Token: 0x040007BC RID: 1980
			public int scopeCount;

			// Token: 0x040007BD RID: 1981
			public string prefix;

			// Token: 0x040007BE RID: 1982
			public string nsUri;
		}
	}
}

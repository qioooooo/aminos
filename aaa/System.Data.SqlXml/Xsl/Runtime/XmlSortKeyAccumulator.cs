using System;
using System.ComponentModel;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000C8 RID: 200
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct XmlSortKeyAccumulator
	{
		// Token: 0x0600096C RID: 2412 RVA: 0x0002C68D File Offset: 0x0002B68D
		public void Create()
		{
			if (this.keys == null)
			{
				this.keys = new XmlSortKey[64];
			}
			this.pos = 0;
			this.keys[0] = null;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0002C6B4 File Offset: 0x0002B6B4
		public void AddStringSortKey(XmlCollation collation, string value)
		{
			this.AppendSortKey(collation.CreateSortKey(value));
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0002C6C3 File Offset: 0x0002B6C3
		public void AddDecimalSortKey(XmlCollation collation, decimal value)
		{
			this.AppendSortKey(new XmlDecimalSortKey(value, collation));
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0002C6D2 File Offset: 0x0002B6D2
		public void AddIntegerSortKey(XmlCollation collation, long value)
		{
			this.AppendSortKey(new XmlIntegerSortKey(value, collation));
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0002C6E1 File Offset: 0x0002B6E1
		public void AddIntSortKey(XmlCollation collation, int value)
		{
			this.AppendSortKey(new XmlIntSortKey(value, collation));
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0002C6F0 File Offset: 0x0002B6F0
		public void AddDoubleSortKey(XmlCollation collation, double value)
		{
			this.AppendSortKey(new XmlDoubleSortKey(value, collation));
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0002C6FF File Offset: 0x0002B6FF
		public void AddDateTimeSortKey(XmlCollation collation, DateTime value)
		{
			this.AppendSortKey(new XmlDateTimeSortKey(value, collation));
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0002C70E File Offset: 0x0002B70E
		public void AddEmptySortKey(XmlCollation collation)
		{
			this.AppendSortKey(new XmlEmptySortKey(collation));
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0002C71C File Offset: 0x0002B71C
		public void FinishSortKeys()
		{
			this.pos++;
			if (this.pos >= this.keys.Length)
			{
				XmlSortKey[] array = new XmlSortKey[this.pos * 2];
				Array.Copy(this.keys, 0, array, 0, this.keys.Length);
				this.keys = array;
			}
			this.keys[this.pos] = null;
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0002C780 File Offset: 0x0002B780
		private void AppendSortKey(XmlSortKey key)
		{
			key.Priority = this.pos;
			if (this.keys[this.pos] == null)
			{
				this.keys[this.pos] = key;
				return;
			}
			this.keys[this.pos].AddSortKey(key);
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x0002C7C0 File Offset: 0x0002B7C0
		public Array Keys
		{
			get
			{
				return this.keys;
			}
		}

		// Token: 0x040005CA RID: 1482
		private const int DefaultSortKeyCount = 64;

		// Token: 0x040005CB RID: 1483
		private XmlSortKey[] keys;

		// Token: 0x040005CC RID: 1484
		private int pos;
	}
}

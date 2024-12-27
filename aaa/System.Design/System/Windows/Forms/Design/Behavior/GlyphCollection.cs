using System;
using System.Collections;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002F7 RID: 759
	public class GlyphCollection : CollectionBase
	{
		// Token: 0x06001D6C RID: 7532 RVA: 0x000A5EEE File Offset: 0x000A4EEE
		public GlyphCollection()
		{
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x000A5EF6 File Offset: 0x000A4EF6
		public GlyphCollection(GlyphCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x000A5F05 File Offset: 0x000A4F05
		public GlyphCollection(Glyph[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x1700051C RID: 1308
		public Glyph this[int index]
		{
			get
			{
				return (Glyph)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x000A5F36 File Offset: 0x000A4F36
		public int Add(Glyph value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x000A5F44 File Offset: 0x000A4F44
		public void AddRange(Glyph[] value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x000A5F6C File Offset: 0x000A4F6C
		public void AddRange(GlyphCollection value)
		{
			for (int i = 0; i < value.Count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x000A5F98 File Offset: 0x000A4F98
		public bool Contains(Glyph value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x000A5FA6 File Offset: 0x000A4FA6
		public void CopyTo(Glyph[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x000A5FB5 File Offset: 0x000A4FB5
		public int IndexOf(Glyph value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x000A5FC3 File Offset: 0x000A4FC3
		public void Insert(int index, Glyph value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x000A5FD2 File Offset: 0x000A4FD2
		public void Remove(Glyph value)
		{
			base.List.Remove(value);
		}
	}
}

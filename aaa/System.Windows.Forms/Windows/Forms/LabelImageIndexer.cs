using System;

namespace System.Windows.Forms
{
	// Token: 0x02000463 RID: 1123
	internal class LabelImageIndexer : ImageList.Indexer
	{
		// Token: 0x06004254 RID: 16980 RVA: 0x000ED2E1 File Offset: 0x000EC2E1
		public LabelImageIndexer(Label owner)
		{
			this.owner = owner;
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06004255 RID: 16981 RVA: 0x000ED2F7 File Offset: 0x000EC2F7
		// (set) Token: 0x06004256 RID: 16982 RVA: 0x000ED30E File Offset: 0x000EC30E
		public override ImageList ImageList
		{
			get
			{
				if (this.owner != null)
				{
					return this.owner.ImageList;
				}
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06004257 RID: 16983 RVA: 0x000ED310 File Offset: 0x000EC310
		// (set) Token: 0x06004258 RID: 16984 RVA: 0x000ED318 File Offset: 0x000EC318
		public override string Key
		{
			get
			{
				return base.Key;
			}
			set
			{
				base.Key = value;
				this.useIntegerIndex = false;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06004259 RID: 16985 RVA: 0x000ED328 File Offset: 0x000EC328
		// (set) Token: 0x0600425A RID: 16986 RVA: 0x000ED330 File Offset: 0x000EC330
		public override int Index
		{
			get
			{
				return base.Index;
			}
			set
			{
				base.Index = value;
				this.useIntegerIndex = true;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x0600425B RID: 16987 RVA: 0x000ED340 File Offset: 0x000EC340
		public override int ActualIndex
		{
			get
			{
				if (this.useIntegerIndex)
				{
					if (this.Index >= this.ImageList.Images.Count)
					{
						return this.ImageList.Images.Count - 1;
					}
					return this.Index;
				}
				else
				{
					if (this.ImageList != null)
					{
						return this.ImageList.Images.IndexOfKey(this.Key);
					}
					return -1;
				}
			}
		}

		// Token: 0x04002096 RID: 8342
		private Label owner;

		// Token: 0x04002097 RID: 8343
		private bool useIntegerIndex = true;
	}
}

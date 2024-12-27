using System;
using System.ComponentModel;

namespace System.DirectoryServices
{
	// Token: 0x0200002A RID: 42
	public class DirectoryVirtualListView
	{
		// Token: 0x0600012A RID: 298 RVA: 0x00005E17 File Offset: 0x00004E17
		public DirectoryVirtualListView()
		{
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005E2A File Offset: 0x00004E2A
		public DirectoryVirtualListView(int afterCount)
		{
			this.AfterCount = afterCount;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005E44 File Offset: 0x00004E44
		public DirectoryVirtualListView(int beforeCount, int afterCount, int offset)
		{
			this.BeforeCount = beforeCount;
			this.AfterCount = afterCount;
			this.Offset = offset;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005E6C File Offset: 0x00004E6C
		public DirectoryVirtualListView(int beforeCount, int afterCount, string target)
		{
			this.BeforeCount = beforeCount;
			this.AfterCount = afterCount;
			this.Target = target;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005E94 File Offset: 0x00004E94
		public DirectoryVirtualListView(int beforeCount, int afterCount, int offset, DirectoryVirtualListViewContext context)
		{
			this.BeforeCount = beforeCount;
			this.AfterCount = afterCount;
			this.Offset = offset;
			this.context = context;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005EC4 File Offset: 0x00004EC4
		public DirectoryVirtualListView(int beforeCount, int afterCount, string target, DirectoryVirtualListViewContext context)
		{
			this.BeforeCount = beforeCount;
			this.AfterCount = afterCount;
			this.Target = target;
			this.context = context;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00005EF4 File Offset: 0x00004EF4
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00005EFC File Offset: 0x00004EFC
		[DefaultValue(0)]
		[DSDescription("DSBeforeCount")]
		public int BeforeCount
		{
			get
			{
				return this.beforeCount;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("DSBadBeforeCount"));
				}
				this.beforeCount = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00005F19 File Offset: 0x00004F19
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00005F21 File Offset: 0x00004F21
		[DSDescription("DSAfterCount")]
		[DefaultValue(0)]
		public int AfterCount
		{
			get
			{
				return this.afterCount;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("DSBadAfterCount"));
				}
				this.afterCount = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00005F3E File Offset: 0x00004F3E
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00005F48 File Offset: 0x00004F48
		[DefaultValue(0)]
		[DSDescription("DSOffset")]
		public int Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("DSBadOffset"));
				}
				this.offset = value;
				if (this.approximateTotal != 0)
				{
					this.targetPercentage = (int)((double)this.offset / (double)this.approximateTotal * 100.0);
					return;
				}
				this.targetPercentage = 0;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00005FA0 File Offset: 0x00004FA0
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00005FA8 File Offset: 0x00004FA8
		[DSDescription("DSTargetPercentage")]
		[DefaultValue(0)]
		public int TargetPercentage
		{
			get
			{
				return this.targetPercentage;
			}
			set
			{
				if (value > 100 || value < 0)
				{
					throw new ArgumentException(Res.GetString("DSBadTargetPercentage"));
				}
				this.targetPercentage = value;
				this.offset = this.approximateTotal * this.targetPercentage / 100;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00005FE0 File Offset: 0x00004FE0
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00005FE8 File Offset: 0x00004FE8
		[DSDescription("DSTarget")]
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string Target
		{
			get
			{
				return this.target;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				this.target = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00005FFB File Offset: 0x00004FFB
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00006003 File Offset: 0x00005003
		[DSDescription("DSApproximateTotal")]
		[DefaultValue(0)]
		public int ApproximateTotal
		{
			get
			{
				return this.approximateTotal;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("DSBadApproximateTotal"));
				}
				this.approximateTotal = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00006020 File Offset: 0x00005020
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00006028 File Offset: 0x00005028
		[DSDescription("DSDirectoryVirtualListViewContext")]
		[DefaultValue(null)]
		public DirectoryVirtualListViewContext DirectoryVirtualListViewContext
		{
			get
			{
				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

		// Token: 0x040001B3 RID: 435
		private int beforeCount;

		// Token: 0x040001B4 RID: 436
		private int afterCount;

		// Token: 0x040001B5 RID: 437
		private int offset;

		// Token: 0x040001B6 RID: 438
		private string target = "";

		// Token: 0x040001B7 RID: 439
		private int approximateTotal;

		// Token: 0x040001B8 RID: 440
		private int targetPercentage;

		// Token: 0x040001B9 RID: 441
		private DirectoryVirtualListViewContext context;
	}
}

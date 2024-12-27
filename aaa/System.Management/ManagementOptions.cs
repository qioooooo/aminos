using System;
using System.ComponentModel;

namespace System.Management
{
	// Token: 0x0200002D RID: 45
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public abstract class ManagementOptions : ICloneable
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600014D RID: 333 RVA: 0x00008348 File Offset: 0x00007348
		// (remove) Token: 0x0600014E RID: 334 RVA: 0x00008361 File Offset: 0x00007361
		internal event IdentifierChangedEventHandler IdentifierChanged;

		// Token: 0x0600014F RID: 335 RVA: 0x0000837A File Offset: 0x0000737A
		internal void FireIdentifierChanged()
		{
			if (this.IdentifierChanged != null)
			{
				this.IdentifierChanged(this, null);
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00008391 File Offset: 0x00007391
		internal void HandleIdentifierChange(object sender, IdentifierChangedEventArgs args)
		{
			this.FireIdentifierChanged();
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00008399 File Offset: 0x00007399
		// (set) Token: 0x06000152 RID: 338 RVA: 0x000083A1 File Offset: 0x000073A1
		internal int Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000083AC File Offset: 0x000073AC
		// (set) Token: 0x06000154 RID: 340 RVA: 0x000083D8 File Offset: 0x000073D8
		public ManagementNamedValueCollection Context
		{
			get
			{
				if (this.context == null)
				{
					return this.context = new ManagementNamedValueCollection();
				}
				return this.context;
			}
			set
			{
				ManagementNamedValueCollection managementNamedValueCollection = this.context;
				if (value != null)
				{
					this.context = value.Clone();
				}
				else
				{
					this.context = new ManagementNamedValueCollection();
				}
				if (managementNamedValueCollection != null)
				{
					managementNamedValueCollection.IdentifierChanged -= this.HandleIdentifierChange;
				}
				this.context.IdentifierChanged += this.HandleIdentifierChange;
				this.HandleIdentifierChange(this, null);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000843C File Offset: 0x0000743C
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00008444 File Offset: 0x00007444
		public TimeSpan Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				if (value.Ticks < 0L)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.timeout = value;
				this.FireIdentifierChanged();
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00008469 File Offset: 0x00007469
		internal ManagementOptions()
			: this(null, ManagementOptions.InfiniteTimeout)
		{
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00008477 File Offset: 0x00007477
		internal ManagementOptions(ManagementNamedValueCollection context, TimeSpan timeout)
			: this(context, timeout, 0)
		{
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00008482 File Offset: 0x00007482
		internal ManagementOptions(ManagementNamedValueCollection context, TimeSpan timeout, int flags)
		{
			this.flags = flags;
			if (context != null)
			{
				this.Context = context;
			}
			else
			{
				this.context = null;
			}
			this.Timeout = timeout;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000084AB File Offset: 0x000074AB
		internal IWbemContext GetContext()
		{
			if (this.context != null)
			{
				return this.context.GetContext();
			}
			return null;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600015B RID: 347 RVA: 0x000084C2 File Offset: 0x000074C2
		// (set) Token: 0x0600015C RID: 348 RVA: 0x000084D5 File Offset: 0x000074D5
		internal bool SendStatus
		{
			get
			{
				return (this.Flags & 128) != 0;
			}
			set
			{
				this.Flags = ((!value) ? (this.Flags & -129) : (this.Flags | 128));
			}
		}

		// Token: 0x0600015D RID: 349
		public abstract object Clone();

		// Token: 0x04000138 RID: 312
		public static readonly TimeSpan InfiniteTimeout = TimeSpan.MaxValue;

		// Token: 0x04000139 RID: 313
		internal int flags;

		// Token: 0x0400013A RID: 314
		internal ManagementNamedValueCollection context;

		// Token: 0x0400013B RID: 315
		internal TimeSpan timeout;
	}
}

using System;

namespace System.Management
{
	// Token: 0x0200002E RID: 46
	public class EnumerationOptions : ManagementOptions
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00008506 File Offset: 0x00007506
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00008516 File Offset: 0x00007516
		public bool ReturnImmediately
		{
			get
			{
				return (base.Flags & 16) != 0;
			}
			set
			{
				base.Flags = ((!value) ? (base.Flags & -17) : (base.Flags | 16));
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00008535 File Offset: 0x00007535
		// (set) Token: 0x06000162 RID: 354 RVA: 0x0000853D File Offset: 0x0000753D
		public int BlockSize
		{
			get
			{
				return this.blockSize;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.blockSize = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00008555 File Offset: 0x00007555
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00008565 File Offset: 0x00007565
		public bool Rewindable
		{
			get
			{
				return (base.Flags & 32) == 0;
			}
			set
			{
				base.Flags = (value ? (base.Flags & -33) : (base.Flags | 32));
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00008584 File Offset: 0x00007584
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00008597 File Offset: 0x00007597
		public bool UseAmendedQualifiers
		{
			get
			{
				return (base.Flags & 131072) != 0;
			}
			set
			{
				base.Flags = (value ? (base.Flags | 131072) : (base.Flags & -131073));
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000167 RID: 359 RVA: 0x000085BC File Offset: 0x000075BC
		// (set) Token: 0x06000168 RID: 360 RVA: 0x000085CF File Offset: 0x000075CF
		public bool EnsureLocatable
		{
			get
			{
				return (base.Flags & 256) != 0;
			}
			set
			{
				base.Flags = (value ? (base.Flags | 256) : (base.Flags & -257));
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000169 RID: 361 RVA: 0x000085F4 File Offset: 0x000075F4
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00008603 File Offset: 0x00007603
		public bool PrototypeOnly
		{
			get
			{
				return (base.Flags & 2) != 0;
			}
			set
			{
				base.Flags = (value ? (base.Flags | 2) : (base.Flags & -3));
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00008621 File Offset: 0x00007621
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00008634 File Offset: 0x00007634
		public bool DirectRead
		{
			get
			{
				return (base.Flags & 512) != 0;
			}
			set
			{
				base.Flags = (value ? (base.Flags | 512) : (base.Flags & -513));
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00008659 File Offset: 0x00007659
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00008668 File Offset: 0x00007668
		public bool EnumerateDeep
		{
			get
			{
				return (base.Flags & 1) == 0;
			}
			set
			{
				base.Flags = ((!value) ? (base.Flags | 1) : (base.Flags & -2));
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00008688 File Offset: 0x00007688
		public EnumerationOptions()
			: this(null, ManagementOptions.InfiniteTimeout, 1, true, true, false, false, false, false, false)
		{
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000086AC File Offset: 0x000076AC
		public EnumerationOptions(ManagementNamedValueCollection context, TimeSpan timeout, int blockSize, bool rewindable, bool returnImmediatley, bool useAmendedQualifiers, bool ensureLocatable, bool prototypeOnly, bool directRead, bool enumerateDeep)
			: base(context, timeout)
		{
			this.BlockSize = blockSize;
			this.Rewindable = rewindable;
			this.ReturnImmediately = returnImmediatley;
			this.UseAmendedQualifiers = useAmendedQualifiers;
			this.EnsureLocatable = ensureLocatable;
			this.PrototypeOnly = prototypeOnly;
			this.DirectRead = directRead;
			this.EnumerateDeep = enumerateDeep;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00008700 File Offset: 0x00007700
		public override object Clone()
		{
			ManagementNamedValueCollection managementNamedValueCollection = null;
			if (base.Context != null)
			{
				managementNamedValueCollection = base.Context.Clone();
			}
			return new EnumerationOptions(managementNamedValueCollection, base.Timeout, this.blockSize, this.Rewindable, this.ReturnImmediately, this.UseAmendedQualifiers, this.EnsureLocatable, this.PrototypeOnly, this.DirectRead, this.EnumerateDeep);
		}

		// Token: 0x0400013D RID: 317
		private int blockSize;
	}
}

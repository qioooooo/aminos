using System;

namespace System.Management
{
	// Token: 0x02000030 RID: 48
	public class ObjectGetOptions : ManagementOptions
	{
		// Token: 0x06000177 RID: 375 RVA: 0x000087DD File Offset: 0x000077DD
		internal static ObjectGetOptions _Clone(ObjectGetOptions options)
		{
			return ObjectGetOptions._Clone(options, null);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000087E8 File Offset: 0x000077E8
		internal static ObjectGetOptions _Clone(ObjectGetOptions options, IdentifierChangedEventHandler handler)
		{
			ObjectGetOptions objectGetOptions;
			if (options != null)
			{
				objectGetOptions = new ObjectGetOptions(options.context, options.timeout, options.UseAmendedQualifiers);
			}
			else
			{
				objectGetOptions = new ObjectGetOptions();
			}
			if (handler != null)
			{
				objectGetOptions.IdentifierChanged += handler;
			}
			else if (options != null)
			{
				objectGetOptions.IdentifierChanged += options.HandleIdentifierChange;
			}
			return objectGetOptions;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000179 RID: 377 RVA: 0x0000883A File Offset: 0x0000783A
		// (set) Token: 0x0600017A RID: 378 RVA: 0x0000884D File Offset: 0x0000784D
		public bool UseAmendedQualifiers
		{
			get
			{
				return (base.Flags & 131072) != 0;
			}
			set
			{
				base.Flags = (value ? (base.Flags | 131072) : (base.Flags & -131073));
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00008878 File Offset: 0x00007878
		public ObjectGetOptions()
			: this(null, ManagementOptions.InfiniteTimeout, false)
		{
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00008887 File Offset: 0x00007887
		public ObjectGetOptions(ManagementNamedValueCollection context)
			: this(context, ManagementOptions.InfiniteTimeout, false)
		{
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00008896 File Offset: 0x00007896
		public ObjectGetOptions(ManagementNamedValueCollection context, TimeSpan timeout, bool useAmendedQualifiers)
			: base(context, timeout)
		{
			this.UseAmendedQualifiers = useAmendedQualifiers;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000088A8 File Offset: 0x000078A8
		public override object Clone()
		{
			ManagementNamedValueCollection managementNamedValueCollection = null;
			if (base.Context != null)
			{
				managementNamedValueCollection = base.Context.Clone();
			}
			return new ObjectGetOptions(managementNamedValueCollection, base.Timeout, this.UseAmendedQualifiers);
		}
	}
}

using System;

namespace System.Management
{
	// Token: 0x02000031 RID: 49
	public class PutOptions : ManagementOptions
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600017F RID: 383 RVA: 0x000088DD File Offset: 0x000078DD
		// (set) Token: 0x06000180 RID: 384 RVA: 0x000088F0 File Offset: 0x000078F0
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

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00008915 File Offset: 0x00007915
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00008930 File Offset: 0x00007930
		public PutType Type
		{
			get
			{
				if ((base.Flags & 1) != 0)
				{
					return PutType.UpdateOnly;
				}
				if ((base.Flags & 2) == 0)
				{
					return PutType.UpdateOrCreate;
				}
				return PutType.CreateOnly;
			}
			set
			{
				switch (value)
				{
				case PutType.UpdateOnly:
					base.Flags |= 1;
					return;
				case PutType.CreateOnly:
					base.Flags |= 2;
					return;
				case PutType.UpdateOrCreate:
					base.Flags = base.Flags;
					return;
				default:
					throw new ArgumentException(null, "Type");
				}
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000898B File Offset: 0x0000798B
		public PutOptions()
			: this(null, ManagementOptions.InfiniteTimeout, false, PutType.UpdateOrCreate)
		{
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000899B File Offset: 0x0000799B
		public PutOptions(ManagementNamedValueCollection context)
			: this(context, ManagementOptions.InfiniteTimeout, false, PutType.UpdateOrCreate)
		{
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000089AB File Offset: 0x000079AB
		public PutOptions(ManagementNamedValueCollection context, TimeSpan timeout, bool useAmendedQualifiers, PutType putType)
			: base(context, timeout)
		{
			this.UseAmendedQualifiers = useAmendedQualifiers;
			this.Type = putType;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000089C4 File Offset: 0x000079C4
		public override object Clone()
		{
			ManagementNamedValueCollection managementNamedValueCollection = null;
			if (base.Context != null)
			{
				managementNamedValueCollection = base.Context.Clone();
			}
			return new PutOptions(managementNamedValueCollection, base.Timeout, this.UseAmendedQualifiers, this.Type);
		}
	}
}

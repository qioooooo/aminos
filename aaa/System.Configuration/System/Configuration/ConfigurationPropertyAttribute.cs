using System;

namespace System.Configuration
{
	// Token: 0x02000038 RID: 56
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class ConfigurationPropertyAttribute : Attribute
	{
		// Token: 0x060002AF RID: 687 RVA: 0x00010EFD File Offset: 0x0000FEFD
		public ConfigurationPropertyAttribute(string name)
		{
			this._Name = name;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x00010F17 File Offset: 0x0000FF17
		public string Name
		{
			get
			{
				return this._Name;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00010F1F File Offset: 0x0000FF1F
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x00010F27 File Offset: 0x0000FF27
		public object DefaultValue
		{
			get
			{
				return this._DefaultValue;
			}
			set
			{
				this._DefaultValue = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x00010F30 File Offset: 0x0000FF30
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x00010F38 File Offset: 0x0000FF38
		public ConfigurationPropertyOptions Options
		{
			get
			{
				return this._Flags;
			}
			set
			{
				this._Flags = value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x00010F41 File Offset: 0x0000FF41
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x00010F51 File Offset: 0x0000FF51
		public bool IsDefaultCollection
		{
			get
			{
				return (this.Options & ConfigurationPropertyOptions.IsDefaultCollection) != ConfigurationPropertyOptions.None;
			}
			set
			{
				if (value)
				{
					this.Options |= ConfigurationPropertyOptions.IsDefaultCollection;
					return;
				}
				this.Options &= ~ConfigurationPropertyOptions.IsDefaultCollection;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x00010F74 File Offset: 0x0000FF74
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x00010F84 File Offset: 0x0000FF84
		public bool IsRequired
		{
			get
			{
				return (this.Options & ConfigurationPropertyOptions.IsRequired) != ConfigurationPropertyOptions.None;
			}
			set
			{
				if (value)
				{
					this.Options |= ConfigurationPropertyOptions.IsRequired;
					return;
				}
				this.Options &= ~ConfigurationPropertyOptions.IsRequired;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x00010FA7 File Offset: 0x0000FFA7
		// (set) Token: 0x060002BA RID: 698 RVA: 0x00010FB7 File Offset: 0x0000FFB7
		public bool IsKey
		{
			get
			{
				return (this.Options & ConfigurationPropertyOptions.IsKey) != ConfigurationPropertyOptions.None;
			}
			set
			{
				if (value)
				{
					this.Options |= ConfigurationPropertyOptions.IsKey;
					return;
				}
				this.Options &= ~ConfigurationPropertyOptions.IsKey;
			}
		}

		// Token: 0x04000285 RID: 645
		internal static readonly string DefaultCollectionPropertyName = "";

		// Token: 0x04000286 RID: 646
		private string _Name;

		// Token: 0x04000287 RID: 647
		private object _DefaultValue = ConfigurationElement.s_nullPropertyValue;

		// Token: 0x04000288 RID: 648
		private ConfigurationPropertyOptions _Flags;
	}
}

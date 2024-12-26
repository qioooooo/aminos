using System;
using System.Security.Permissions;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x0200013D RID: 317
	public abstract class VsaItem : IVsaItem
	{
		// Token: 0x06000E74 RID: 3700 RVA: 0x000622E7 File Offset: 0x000612E7
		internal VsaItem(VsaEngine engine, string itemName, VsaItemType type, VsaItemFlag flag)
		{
			this.engine = engine;
			this.type = type;
			this.name = itemName;
			this.flag = flag;
			this.codebase = null;
			this.isDirty = true;
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0006231A File Offset: 0x0006131A
		internal virtual void CheckForErrors()
		{
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x0006231C File Offset: 0x0006131C
		internal virtual void Close()
		{
			this.engine = null;
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00062325 File Offset: 0x00061325
		internal virtual void Compile()
		{
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00062327 File Offset: 0x00061327
		internal virtual Type GetCompiledType()
		{
			return null;
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000E79 RID: 3705 RVA: 0x0006232A File Offset: 0x0006132A
		// (set) Token: 0x06000E7A RID: 3706 RVA: 0x00062345 File Offset: 0x00061345
		public virtual bool IsDirty
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				return this.isDirty;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				this.isDirty = value;
			}
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00062361 File Offset: 0x00061361
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual object GetOption(string name)
		{
			if (this.engine == null)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			if (string.Compare(name, "codebase", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.codebase;
			}
			throw new VsaException(VsaError.OptionNotSupported);
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x00062395 File Offset: 0x00061395
		// (set) Token: 0x06000E7D RID: 3709 RVA: 0x000623B0 File Offset: 0x000613B0
		public virtual string Name
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				return this.name;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				if (this.name == value)
				{
					return;
				}
				if (!this.engine.IsValidIdentifier(value))
				{
					throw new VsaException(VsaError.ItemNameInvalid);
				}
				foreach (object obj in this.engine.Items)
				{
					IVsaItem vsaItem = (IVsaItem)obj;
					if (vsaItem.Name.Equals(value))
					{
						throw new VsaException(VsaError.ItemNameInUse);
					}
				}
				this.name = value;
				this.isDirty = true;
				this.engine.IsDirty = true;
			}
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00062478 File Offset: 0x00061478
		internal virtual void Remove()
		{
			this.engine = null;
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00062481 File Offset: 0x00061481
		internal virtual void Reset()
		{
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00062483 File Offset: 0x00061483
		internal virtual void Run()
		{
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00062488 File Offset: 0x00061488
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual void SetOption(string name, object value)
		{
			if (this.engine == null)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			if (string.Compare(name, "codebase", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.codebase = (string)value;
				this.isDirty = true;
				this.engine.IsDirty = true;
				return;
			}
			throw new VsaException(VsaError.OptionNotSupported);
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x000624E2 File Offset: 0x000614E2
		public VsaItemType ItemType
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				return this.type;
			}
		}

		// Token: 0x040007C7 RID: 1991
		protected string name;

		// Token: 0x040007C8 RID: 1992
		internal string codebase;

		// Token: 0x040007C9 RID: 1993
		internal VsaEngine engine;

		// Token: 0x040007CA RID: 1994
		protected VsaItemType type;

		// Token: 0x040007CB RID: 1995
		protected VsaItemFlag flag;

		// Token: 0x040007CC RID: 1996
		protected bool isDirty;
	}
}

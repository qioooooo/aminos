using System;
using System.Collections;
using System.Security.Permissions;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x0200013F RID: 319
	public sealed class VsaItems : IVsaItems, IEnumerable
	{
		// Token: 0x06000E95 RID: 3733 RVA: 0x00062915 File Offset: 0x00061915
		public VsaItems(VsaEngine engine)
		{
			this.engine = engine;
			this.staticCodeBlockCount = 0;
			this.items = new ArrayList(10);
		}

		// Token: 0x170003C4 RID: 964
		public IVsaItem this[int index]
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				if (this.isClosed)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				if (index < 0 || index >= this.items.Count)
				{
					throw new VsaException(VsaError.ItemNotFound);
				}
				return (IVsaItem)this.items[index];
			}
		}

		// Token: 0x170003C5 RID: 965
		public IVsaItem this[string itemName]
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				if (this.isClosed)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				if (itemName != null)
				{
					int i = 0;
					int count = this.items.Count;
					while (i < count)
					{
						IVsaItem vsaItem = (IVsaItem)this.items[i];
						if (vsaItem.Name.Equals(itemName))
						{
							return vsaItem;
						}
						i++;
					}
				}
				throw new VsaException(VsaError.ItemNotFound);
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x000629EF File Offset: 0x000619EF
		public int Count
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				if (this.isClosed)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				if (this.items != null)
				{
					return this.items.Count;
				}
				return 0;
			}
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00062A1C File Offset: 0x00061A1C
		public void Close()
		{
			if (this.isClosed)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			this.TryObtainLock();
			try
			{
				this.isClosed = true;
				foreach (object obj in this.items)
				{
					((VsaItem)obj).Close();
				}
				this.items = null;
			}
			finally
			{
				this.ReleaseLock();
				this.engine = null;
			}
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00062AB8 File Offset: 0x00061AB8
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public IVsaItem CreateItem(string name, VsaItemType itemType, VsaItemFlag itemFlag)
		{
			if (this.isClosed)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			if (this.engine.IsRunning)
			{
				throw new VsaException(VsaError.EngineRunning);
			}
			this.TryObtainLock();
			IVsaItem vsaItem2;
			try
			{
				if (itemType != VsaItemType.Reference && !this.engine.IsValidIdentifier(name))
				{
					throw new VsaException(VsaError.ItemNameInvalid);
				}
				foreach (object obj in this.items)
				{
					if (((VsaItem)obj).Name.Equals(name))
					{
						throw new VsaException(VsaError.ItemNameInUse);
					}
				}
				IVsaItem vsaItem = null;
				switch (itemType)
				{
				case VsaItemType.Reference:
					if (itemFlag != VsaItemFlag.None)
					{
						throw new VsaException(VsaError.ItemFlagNotSupported);
					}
					vsaItem = new VsaReference(this.engine, name);
					break;
				case VsaItemType.AppGlobal:
					if (itemFlag != VsaItemFlag.None)
					{
						throw new VsaException(VsaError.ItemFlagNotSupported);
					}
					vsaItem = new VsaHostObject(this.engine, name, VsaItemType.AppGlobal);
					((VsaHostObject)vsaItem).isVisible = true;
					break;
				case VsaItemType.Code:
					if (itemFlag == VsaItemFlag.Class)
					{
						throw new VsaException(VsaError.ItemFlagNotSupported);
					}
					vsaItem = new VsaStaticCode(this.engine, name, itemFlag);
					this.staticCodeBlockCount++;
					break;
				}
				if (vsaItem == null)
				{
					throw new VsaException(VsaError.ItemTypeNotSupported);
				}
				this.items.Add(vsaItem);
				this.engine.IsDirty = true;
				vsaItem2 = vsaItem;
			}
			finally
			{
				this.ReleaseLock();
			}
			return vsaItem2;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00062C58 File Offset: 0x00061C58
		public IEnumerator GetEnumerator()
		{
			if (this.isClosed)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			return this.items.GetEnumerator();
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00062C78 File Offset: 0x00061C78
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public void Remove(string itemName)
		{
			if (this.isClosed)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			this.TryObtainLock();
			try
			{
				if (itemName == null)
				{
					throw new ArgumentNullException("itemName");
				}
				int i = 0;
				int count = this.items.Count;
				while (i < count)
				{
					IVsaItem vsaItem = (IVsaItem)this.items[i];
					if (vsaItem.Name.Equals(itemName))
					{
						((VsaItem)vsaItem).Remove();
						this.items.RemoveAt(i);
						this.engine.IsDirty = true;
						if (vsaItem is VsaStaticCode)
						{
							this.staticCodeBlockCount--;
						}
						return;
					}
					i++;
				}
				throw new VsaException(VsaError.ItemNotFound);
			}
			finally
			{
				this.ReleaseLock();
			}
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00062D44 File Offset: 0x00061D44
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public void Remove(int itemIndex)
		{
			if (this.isClosed)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			this.TryObtainLock();
			try
			{
				if (0 > itemIndex || itemIndex >= this.items.Count)
				{
					throw new VsaException(VsaError.ItemNotFound);
				}
				VsaItem vsaItem = (VsaItem)this.items[itemIndex];
				vsaItem.Remove();
				this.items.RemoveAt(itemIndex);
				if (vsaItem is VsaStaticCode)
				{
					this.staticCodeBlockCount--;
				}
			}
			finally
			{
				this.ReleaseLock();
			}
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00062DDC File Offset: 0x00061DDC
		private void TryObtainLock()
		{
			this.engine.TryObtainLock();
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00062DE9 File Offset: 0x00061DE9
		private void ReleaseLock()
		{
			this.engine.ReleaseLock();
		}

		// Token: 0x040007D5 RID: 2005
		private ArrayList items;

		// Token: 0x040007D6 RID: 2006
		private bool isClosed;

		// Token: 0x040007D7 RID: 2007
		private VsaEngine engine;

		// Token: 0x040007D8 RID: 2008
		internal int staticCodeBlockCount;
	}
}

using System;
using System.Collections;
using System.Security.Permissions;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000143 RID: 323
	internal class VsaScriptScope : VsaItem, IVsaScriptScope, IVsaItem, IDebugScriptScope
	{
		// Token: 0x06000ECE RID: 3790 RVA: 0x00063E24 File Offset: 0x00062E24
		internal VsaScriptScope(VsaEngine engine, string itemName, VsaScriptScope parent)
			: base(engine, itemName, (VsaItemType)19, VsaItemFlag.None)
		{
			this.parent = parent;
			this.scope = null;
			this.items = new ArrayList(8);
			this.isCompiled = false;
			this.isClosed = false;
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00063E5C File Offset: 0x00062E5C
		public virtual object GetObject()
		{
			if (this.scope == null)
			{
				if (this.parent != null)
				{
					this.scope = new GlobalScope((GlobalScope)this.parent.GetObject(), this.engine, false);
				}
				else
				{
					this.scope = new GlobalScope(null, this.engine);
				}
			}
			return this.scope;
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x00063EB5 File Offset: 0x00062EB5
		public IVsaScriptScope Parent
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00063EC0 File Offset: 0x00062EC0
		public virtual IVsaItem AddItem(string itemName, VsaItemType type)
		{
			VsaItem vsaItem = null;
			if (this.isClosed)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			if (this.GetItem(itemName) != null)
			{
				throw new VsaException(VsaError.ItemNameInUse);
			}
			switch (type)
			{
			case (VsaItemType)16:
			case (VsaItemType)17:
			case (VsaItemType)18:
				vsaItem = new VsaHostObject(this.engine, itemName, type, this);
				if (type == (VsaItemType)17 || type == (VsaItemType)18)
				{
					((VsaHostObject)vsaItem).exposeMembers = true;
				}
				if (type == (VsaItemType)16 || type == (VsaItemType)18)
				{
					((VsaHostObject)vsaItem).isVisible = true;
				}
				if (this.engine.IsRunning)
				{
					((VsaHostObject)vsaItem).Compile();
					((VsaHostObject)vsaItem).Run();
				}
				break;
			case (VsaItemType)19:
				vsaItem = new VsaScriptScope(this.engine, itemName, this);
				break;
			case (VsaItemType)20:
				vsaItem = new VsaScriptCode(this.engine, itemName, type, this);
				break;
			case (VsaItemType)21:
				if (!this.engine.IsRunning)
				{
					throw new VsaException(VsaError.EngineNotRunning);
				}
				vsaItem = new VsaScriptCode(this.engine, itemName, type, this);
				break;
			case (VsaItemType)22:
				if (!this.engine.IsRunning)
				{
					throw new VsaException(VsaError.EngineNotRunning);
				}
				vsaItem = new VsaScriptCode(this.engine, itemName, type, this);
				break;
			}
			if (vsaItem != null)
			{
				this.items.Add(vsaItem);
				return vsaItem;
			}
			throw new VsaException(VsaError.ItemTypeNotSupported);
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00064018 File Offset: 0x00063018
		public virtual IVsaItem GetItem(string itemName)
		{
			int i = 0;
			int count = this.items.Count;
			while (i < count)
			{
				VsaItem vsaItem = (VsaItem)this.items[i];
				if ((vsaItem.Name == null && itemName == null) || (vsaItem.Name != null && vsaItem.Name.Equals(itemName)))
				{
					return (IVsaItem)this.items[i];
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00064084 File Offset: 0x00063084
		public virtual void RemoveItem(string itemName)
		{
			int i = 0;
			int count = this.items.Count;
			while (i < count)
			{
				VsaItem vsaItem = (VsaItem)this.items[i];
				if ((vsaItem.Name == null && itemName == null) || (vsaItem.Name != null && vsaItem.Name.Equals(itemName)))
				{
					vsaItem.Remove();
					this.items.RemoveAt(i);
					return;
				}
				i++;
			}
			throw new VsaException(VsaError.ItemNotFound);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x000640FC File Offset: 0x000630FC
		public virtual void RemoveItem(IVsaItem item)
		{
			int i = 0;
			int count = this.items.Count;
			while (i < count)
			{
				VsaItem vsaItem = (VsaItem)this.items[i];
				if (vsaItem == item)
				{
					vsaItem.Remove();
					this.items.RemoveAt(i);
					return;
				}
				i++;
			}
			throw new VsaException(VsaError.ItemNotFound);
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x00064154 File Offset: 0x00063154
		public virtual int GetItemCount()
		{
			return this.items.Count;
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00064161 File Offset: 0x00063161
		public virtual IVsaItem GetItemAtIndex(int index)
		{
			if (index < this.items.Count)
			{
				return (IVsaItem)this.items[index];
			}
			throw new VsaException(VsaError.ItemNotFound);
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0006418D File Offset: 0x0006318D
		public virtual void RemoveItemAtIndex(int index)
		{
			if (index < this.items.Count)
			{
				((VsaItem)this.items[index]).Remove();
				this.items.RemoveAt(index);
				return;
			}
			throw new VsaException(VsaError.ItemNotFound);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x000641CA File Offset: 0x000631CA
		public virtual IVsaItem CreateDynamicItem(string itemName, VsaItemType type)
		{
			if (this.engine.IsRunning)
			{
				return this.AddItem(itemName, type);
			}
			throw new VsaException(VsaError.EngineNotRunning);
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x000641EC File Offset: 0x000631EC
		internal override void CheckForErrors()
		{
			if (this.items.Count == 0)
			{
				return;
			}
			try
			{
				this.engine.Globals.ScopeStack.Push((ScriptObject)this.GetObject());
				foreach (object obj in this.items)
				{
					((VsaItem)obj).CheckForErrors();
				}
			}
			finally
			{
				this.engine.Globals.ScopeStack.Pop();
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x00064298 File Offset: 0x00063298
		internal override void Compile()
		{
			if (this.items.Count == 0)
			{
				return;
			}
			if (!this.isCompiled)
			{
				this.isCompiled = true;
				try
				{
					this.engine.Globals.ScopeStack.Push((ScriptObject)this.GetObject());
					try
					{
						foreach (object obj in this.items)
						{
							((VsaItem)obj).Compile();
						}
					}
					finally
					{
						this.engine.Globals.ScopeStack.Pop();
					}
				}
				catch
				{
					this.isCompiled = false;
					throw;
				}
			}
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x00064370 File Offset: 0x00063370
		internal override void Reset()
		{
			foreach (object obj in this.items)
			{
				((VsaItem)obj).Reset();
			}
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x000643C8 File Offset: 0x000633C8
		internal void ReRun(GlobalScope scope)
		{
			foreach (object obj in this.items)
			{
				if (obj is VsaHostObject)
				{
					((VsaHostObject)obj).ReRun(scope);
				}
			}
			if (this.parent != null)
			{
				this.parent.ReRun(scope);
			}
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x00064440 File Offset: 0x00063440
		internal override void Run()
		{
			if (this.items.Count == 0)
			{
				return;
			}
			try
			{
				this.engine.Globals.ScopeStack.Push((ScriptObject)this.GetObject());
				foreach (object obj in this.items)
				{
					((VsaItem)obj).Run();
				}
			}
			finally
			{
				this.engine.Globals.ScopeStack.Pop();
			}
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x000644EC File Offset: 0x000634EC
		internal override void Close()
		{
			foreach (object obj in this.items)
			{
				((VsaItem)obj).Close();
			}
			this.items = null;
			this.parent = null;
			this.scope = null;
			this.isClosed = true;
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x00064560 File Offset: 0x00063560
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual void SetThisValue(object thisValue)
		{
			if (this.scope != null)
			{
				this.scope.thisObject = thisValue;
			}
		}

		// Token: 0x040007E7 RID: 2023
		private VsaScriptScope parent;

		// Token: 0x040007E8 RID: 2024
		private GlobalScope scope;

		// Token: 0x040007E9 RID: 2025
		private ArrayList items;

		// Token: 0x040007EA RID: 2026
		private bool isCompiled;

		// Token: 0x040007EB RID: 2027
		private bool isClosed;
	}
}

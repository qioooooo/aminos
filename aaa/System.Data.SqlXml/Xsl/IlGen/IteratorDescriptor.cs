using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.XPath;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x02000026 RID: 38
	internal class IteratorDescriptor
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x0000CE65 File Offset: 0x0000BE65
		public IteratorDescriptor(GenerateHelper helper)
		{
			this.Init(null, helper);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000CE75 File Offset: 0x0000BE75
		public IteratorDescriptor(IteratorDescriptor iterParent)
		{
			this.Init(iterParent, iterParent.helper);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000CE8A File Offset: 0x0000BE8A
		private void Init(IteratorDescriptor iterParent, GenerateHelper helper)
		{
			this.helper = helper;
			this.iterParent = iterParent;
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000CE9A File Offset: 0x0000BE9A
		public IteratorDescriptor ParentIterator
		{
			get
			{
				return this.iterParent;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000CEA2 File Offset: 0x0000BEA2
		public bool HasLabelNext
		{
			get
			{
				return this.hasNext;
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000CEAA File Offset: 0x0000BEAA
		public Label GetLabelNext()
		{
			return this.lblNext;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000CEB2 File Offset: 0x0000BEB2
		public void SetIterator(Label lblNext, StorageDescriptor storage)
		{
			this.lblNext = lblNext;
			this.hasNext = true;
			this.storage = storage;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000CEC9 File Offset: 0x0000BEC9
		public void SetIterator(IteratorDescriptor iterInfo)
		{
			if (iterInfo.HasLabelNext)
			{
				this.lblNext = iterInfo.GetLabelNext();
				this.hasNext = true;
			}
			this.storage = iterInfo.Storage;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000CEF2 File Offset: 0x0000BEF2
		public void LoopToEnd(Label lblOnEnd)
		{
			if (this.hasNext)
			{
				this.helper.BranchAndMark(this.lblNext, lblOnEnd);
				this.hasNext = false;
			}
			this.storage = StorageDescriptor.None();
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000CF20 File Offset: 0x0000BF20
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x0000CF28 File Offset: 0x0000BF28
		public LocalBuilder LocalPosition
		{
			get
			{
				return this.locPos;
			}
			set
			{
				this.locPos = value;
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000CF31 File Offset: 0x0000BF31
		public void CacheCount()
		{
			this.PushValue();
			this.helper.CallCacheCount(this.storage.ItemStorageType);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000CF50 File Offset: 0x0000BF50
		public void EnsureNoCache()
		{
			if (this.storage.IsCached)
			{
				if (!this.HasLabelNext)
				{
					this.EnsureStack();
					this.helper.LoadInteger(0);
					this.helper.CallCacheItem(this.storage.ItemStorageType);
					this.storage = StorageDescriptor.Stack(this.storage.ItemStorageType, false);
					return;
				}
				LocalBuilder localBuilder = this.helper.DeclareLocal("$$$idx", typeof(int));
				this.EnsureNoStack("$$$cache");
				this.helper.LoadInteger(-1);
				this.helper.Emit(OpCodes.Stloc, localBuilder);
				Label label = this.helper.DefineLabel();
				this.helper.MarkLabel(label);
				this.helper.Emit(OpCodes.Ldloc, localBuilder);
				this.helper.LoadInteger(1);
				this.helper.Emit(OpCodes.Add);
				this.helper.Emit(OpCodes.Stloc, localBuilder);
				this.helper.Emit(OpCodes.Ldloc, localBuilder);
				this.CacheCount();
				this.helper.Emit(OpCodes.Bge, this.GetLabelNext());
				this.PushValue();
				this.helper.Emit(OpCodes.Ldloc, localBuilder);
				this.helper.CallCacheItem(this.storage.ItemStorageType);
				this.SetIterator(label, StorageDescriptor.Stack(this.storage.ItemStorageType, false));
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000D0C0 File Offset: 0x0000C0C0
		public void SetBranching(BranchingContext brctxt, Label lblBranch)
		{
			this.brctxt = brctxt;
			this.lblBranch = lblBranch;
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000D0D0 File Offset: 0x0000C0D0
		public bool IsBranching
		{
			get
			{
				return this.brctxt != BranchingContext.None;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000D0DE File Offset: 0x0000C0DE
		public Label LabelBranch
		{
			get
			{
				return this.lblBranch;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000D0E6 File Offset: 0x0000C0E6
		public BranchingContext CurrentBranchingContext
		{
			get
			{
				return this.brctxt;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000D0EE File Offset: 0x0000C0EE
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x0000D0F6 File Offset: 0x0000C0F6
		public StorageDescriptor Storage
		{
			get
			{
				return this.storage;
			}
			set
			{
				this.storage = value;
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000D100 File Offset: 0x0000C100
		public void PushValue()
		{
			switch (this.storage.Location)
			{
			case ItemLocation.Stack:
				this.helper.Emit(OpCodes.Dup);
				return;
			case ItemLocation.Parameter:
				this.helper.LoadParameter(this.storage.ParameterLocation);
				return;
			case ItemLocation.Local:
				this.helper.Emit(OpCodes.Ldloc, this.storage.LocalLocation);
				return;
			case ItemLocation.Current:
				this.helper.Emit(OpCodes.Ldloca, this.storage.CurrentLocation);
				this.helper.Call(this.storage.CurrentLocation.LocalType.GetMethod("get_Current"));
				return;
			default:
				return;
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000D1B8 File Offset: 0x0000C1B8
		public void EnsureStack()
		{
			switch (this.storage.Location)
			{
			case ItemLocation.Stack:
				return;
			case ItemLocation.Parameter:
			case ItemLocation.Local:
			case ItemLocation.Current:
				this.PushValue();
				break;
			case ItemLocation.Global:
				this.helper.LoadQueryRuntime();
				this.helper.Call(this.storage.GlobalLocation);
				break;
			}
			this.storage = this.storage.ToStack();
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000D22A File Offset: 0x0000C22A
		public void EnsureNoStack(string locName)
		{
			if (this.storage.Location == ItemLocation.Stack)
			{
				this.EnsureLocal(locName);
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000D244 File Offset: 0x0000C244
		public void EnsureLocal(string locName)
		{
			if (this.storage.Location != ItemLocation.Local)
			{
				if (this.storage.IsCached)
				{
					this.EnsureLocal(this.helper.DeclareLocal(locName, typeof(IList<>).MakeGenericType(new Type[] { this.storage.ItemStorageType })));
					return;
				}
				this.EnsureLocal(this.helper.DeclareLocal(locName, this.storage.ItemStorageType));
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000D2C1 File Offset: 0x0000C2C1
		public void EnsureLocal(LocalBuilder bldr)
		{
			if (this.storage.LocalLocation != bldr)
			{
				this.EnsureStack();
				this.helper.Emit(OpCodes.Stloc, bldr);
				this.storage = this.storage.ToLocal(bldr);
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000D2FA File Offset: 0x0000C2FA
		public void DiscardStack()
		{
			if (this.storage.Location == ItemLocation.Stack)
			{
				this.helper.Emit(OpCodes.Pop);
				this.storage = StorageDescriptor.None();
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000D325 File Offset: 0x0000C325
		public void EnsureStackNoCache()
		{
			this.EnsureNoCache();
			this.EnsureStack();
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000D333 File Offset: 0x0000C333
		public void EnsureNoStackNoCache(string locName)
		{
			this.EnsureNoCache();
			this.EnsureNoStack(locName);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000D342 File Offset: 0x0000C342
		public void EnsureLocalNoCache(string locName)
		{
			this.EnsureNoCache();
			this.EnsureLocal(locName);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000D351 File Offset: 0x0000C351
		public void EnsureLocalNoCache(LocalBuilder bldr)
		{
			this.EnsureNoCache();
			this.EnsureLocal(bldr);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000D360 File Offset: 0x0000C360
		public void EnsureItemStorageType(XmlQueryType xmlType, Type storageTypeDest)
		{
			if (this.storage.ItemStorageType != storageTypeDest)
			{
				if (this.storage.IsCached)
				{
					if (this.storage.ItemStorageType == typeof(XPathNavigator))
					{
						this.EnsureStack();
						this.helper.Call(XmlILMethods.NavsToItems);
						goto IL_012F;
					}
					if (storageTypeDest == typeof(XPathNavigator))
					{
						this.EnsureStack();
						this.helper.Call(XmlILMethods.ItemsToNavs);
						goto IL_012F;
					}
				}
				this.EnsureStackNoCache();
				if (this.storage.ItemStorageType == typeof(XPathItem))
				{
					if (storageTypeDest == typeof(XPathNavigator))
					{
						this.helper.Emit(OpCodes.Castclass, typeof(XPathNavigator));
					}
					else
					{
						this.helper.CallValueAs(storageTypeDest);
					}
				}
				else if (this.storage.ItemStorageType != typeof(XPathNavigator))
				{
					this.helper.LoadInteger(this.helper.StaticData.DeclareXmlType(xmlType));
					this.helper.LoadQueryRuntime();
					this.helper.Call(XmlILMethods.StorageMethods[this.storage.ItemStorageType].ToAtomicValue);
				}
			}
			IL_012F:
			this.storage = this.storage.ToStorageType(storageTypeDest);
		}

		// Token: 0x04000259 RID: 601
		private GenerateHelper helper;

		// Token: 0x0400025A RID: 602
		private IteratorDescriptor iterParent;

		// Token: 0x0400025B RID: 603
		private Label lblNext;

		// Token: 0x0400025C RID: 604
		private bool hasNext;

		// Token: 0x0400025D RID: 605
		private LocalBuilder locPos;

		// Token: 0x0400025E RID: 606
		private BranchingContext brctxt;

		// Token: 0x0400025F RID: 607
		private Label lblBranch;

		// Token: 0x04000260 RID: 608
		private StorageDescriptor storage;
	}
}

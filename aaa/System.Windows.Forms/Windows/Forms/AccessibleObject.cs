using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Accessibility;

namespace System.Windows.Forms
{
	// Token: 0x020001D4 RID: 468
	[ComVisible(true)]
	public class AccessibleObject : StandardOleMarshalObject, IReflect, IAccessible, UnsafeNativeMethods.IEnumVariant, UnsafeNativeMethods.IOleWindow
	{
		// Token: 0x060011FA RID: 4602 RVA: 0x0000F228 File Offset: 0x0000E228
		public AccessibleObject()
		{
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0000F238 File Offset: 0x0000E238
		private AccessibleObject(IAccessible iAcc)
		{
			this.systemIAccessible = iAcc;
			this.systemWrapper = true;
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x0000F258 File Offset: 0x0000E258
		public virtual Rectangle Bounds
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					try
					{
						this.systemIAccessible.accLocation(out num, out num2, out num3, out num4, 0);
						return new Rectangle(num, num2, num3, num4);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return Rectangle.Empty;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060011FD RID: 4605 RVA: 0x0000F2C8 File Offset: 0x0000E2C8
		public virtual string DefaultAction
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.get_accDefaultAction(0);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x0000F318 File Offset: 0x0000E318
		public virtual string Description
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.get_accDescription(0);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060011FF RID: 4607 RVA: 0x0000F368 File Offset: 0x0000E368
		private UnsafeNativeMethods.IEnumVariant EnumVariant
		{
			get
			{
				if (this.enumVariant == null)
				{
					this.enumVariant = new AccessibleObject.EnumVariantObject(this);
				}
				return this.enumVariant;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06001200 RID: 4608 RVA: 0x0000F384 File Offset: 0x0000E384
		public virtual string Help
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.get_accHelp(0);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06001201 RID: 4609 RVA: 0x0000F3D4 File Offset: 0x0000E3D4
		public virtual string KeyboardShortcut
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.get_accKeyboardShortcut(0);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06001202 RID: 4610 RVA: 0x0000F424 File Offset: 0x0000E424
		// (set) Token: 0x06001203 RID: 4611 RVA: 0x0000F474 File Offset: 0x0000E474
		public virtual string Name
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.get_accName(0);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
			set
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						this.systemIAccessible.set_accName(0, value);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x0000F4C0 File Offset: 0x0000E4C0
		public virtual AccessibleObject Parent
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (this.systemIAccessible != null)
				{
					return this.WrapIAccessible(this.systemIAccessible.accParent);
				}
				return null;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x0000F4DD File Offset: 0x0000E4DD
		public virtual AccessibleRole Role
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					return (AccessibleRole)this.systemIAccessible.get_accRole(0);
				}
				return AccessibleRole.None;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x0000F4FF File Offset: 0x0000E4FF
		public virtual AccessibleStates State
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					return (AccessibleStates)this.systemIAccessible.get_accState(0);
				}
				return AccessibleStates.None;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x0000F524 File Offset: 0x0000E524
		// (set) Token: 0x06001208 RID: 4616 RVA: 0x0000F578 File Offset: 0x0000E578
		public virtual string Value
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.get_accValue(0);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return "";
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						this.systemIAccessible.set_accValue(0, value);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
			}
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x0000F5C4 File Offset: 0x0000E5C4
		public virtual AccessibleObject GetChild(int index)
		{
			return null;
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x0000F5C7 File Offset: 0x0000E5C7
		public virtual int GetChildCount()
		{
			return -1;
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x0000F5CA File Offset: 0x0000E5CA
		internal virtual int[] GetSysChildOrder()
		{
			return null;
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0000F5CD File Offset: 0x0000E5CD
		internal virtual bool GetSysChild(AccessibleNavigation navdir, out AccessibleObject accessibleObject)
		{
			accessibleObject = null;
			return false;
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x0000F5D4 File Offset: 0x0000E5D4
		public virtual AccessibleObject GetFocused()
		{
			if (this.GetChildCount() < 0)
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.WrapIAccessible(this.systemIAccessible.accFocus);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
			int childCount = this.GetChildCount();
			for (int i = 0; i < childCount; i++)
			{
				AccessibleObject child = this.GetChild(i);
				if (child != null && (child.State & AccessibleStates.Focused) != AccessibleStates.None)
				{
					return child;
				}
			}
			if ((this.State & AccessibleStates.Focused) != AccessibleStates.None)
			{
				return this;
			}
			return null;
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0000F668 File Offset: 0x0000E668
		public virtual int GetHelpTopic(out string fileName)
		{
			if (this.systemIAccessible != null)
			{
				try
				{
					int num = this.systemIAccessible.get_accHelpTopic(out fileName, 0);
					if (fileName != null && fileName.Length > 0)
					{
						IntSecurity.DemandFileIO(FileIOPermissionAccess.PathDiscovery, fileName);
					}
					return num;
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			fileName = null;
			return -1;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0000F6D4 File Offset: 0x0000E6D4
		public virtual AccessibleObject GetSelected()
		{
			if (this.GetChildCount() < 0)
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.WrapIAccessible(this.systemIAccessible.accSelection);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
			int childCount = this.GetChildCount();
			for (int i = 0; i < childCount; i++)
			{
				AccessibleObject child = this.GetChild(i);
				if (child != null && (child.State & AccessibleStates.Selected) != AccessibleStates.None)
				{
					return child;
				}
			}
			if ((this.State & AccessibleStates.Selected) != AccessibleStates.None)
			{
				return this;
			}
			return null;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0000F768 File Offset: 0x0000E768
		public virtual AccessibleObject HitTest(int x, int y)
		{
			if (this.GetChildCount() >= 0)
			{
				int childCount = this.GetChildCount();
				for (int i = 0; i < childCount; i++)
				{
					AccessibleObject child = this.GetChild(i);
					if (child != null && child.Bounds.Contains(x, y))
					{
						return child;
					}
				}
				return this;
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.WrapIAccessible(this.systemIAccessible.accHitTest(x, y));
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			if (this.Bounds.Contains(x, y))
			{
				return this;
			}
			return null;
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x0000F810 File Offset: 0x0000E810
		void IAccessible.accDoDefaultAction(object childID)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					this.DoDefaultAction();
					return;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					accessibleChild.DoDefaultAction();
					return;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					this.systemIAccessible.accDoDefaultAction(childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0000F898 File Offset: 0x0000E898
		object IAccessible.accHitTest(int xLeft, int yTop)
		{
			if (this.IsClientObject)
			{
				AccessibleObject accessibleObject = this.HitTest(xLeft, yTop);
				if (accessibleObject != null)
				{
					return this.AsVariant(accessibleObject);
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.accHitTest(xLeft, yTop);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0000F900 File Offset: 0x0000E900
		void IAccessible.accLocation(out int pxLeft, out int pyTop, out int pcxWidth, out int pcyHeight, object childID)
		{
			pxLeft = 0;
			pyTop = 0;
			pcxWidth = 0;
			pcyHeight = 0;
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					Rectangle bounds = this.Bounds;
					pxLeft = bounds.X;
					pyTop = bounds.Y;
					pcxWidth = bounds.Width;
					pcyHeight = bounds.Height;
					return;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					Rectangle bounds2 = accessibleChild.Bounds;
					pxLeft = bounds2.X;
					pyTop = bounds2.Y;
					pcxWidth = bounds2.Width;
					pcyHeight = bounds2.Height;
					return;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					this.systemIAccessible.accLocation(out pxLeft, out pyTop, out pcxWidth, out pcyHeight, childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0000F9E0 File Offset: 0x0000E9E0
		object IAccessible.accNavigate(int navDir, object childID)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					AccessibleObject accessibleObject = this.Navigate((AccessibleNavigation)navDir);
					if (accessibleObject != null)
					{
						return this.AsVariant(accessibleObject);
					}
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return this.AsVariant(accessibleChild.Navigate((AccessibleNavigation)navDir));
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					object obj;
					if (!this.SysNavigate(navDir, childID, out obj))
					{
						obj = this.systemIAccessible.accNavigate(navDir, childID);
					}
					return obj;
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0000FA90 File Offset: 0x0000EA90
		void IAccessible.accSelect(int flagsSelect, object childID)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					this.Select((AccessibleSelection)flagsSelect);
					return;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					accessibleChild.Select((AccessibleSelection)flagsSelect);
					return;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					this.systemIAccessible.accSelect(flagsSelect, childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0000FB1C File Offset: 0x0000EB1C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual void DoDefaultAction()
		{
			if (this.systemIAccessible != null)
			{
				try
				{
					this.systemIAccessible.accDoDefaultAction(0);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0000FB68 File Offset: 0x0000EB68
		object IAccessible.get_accChild(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.AsIAccessible(this);
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					if (accessibleChild == this)
					{
						return null;
					}
					return this.AsIAccessible(accessibleChild);
				}
			}
			if (this.systemIAccessible != null)
			{
				return this.systemIAccessible.get_accChild(childID);
			}
			return null;
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06001218 RID: 4632 RVA: 0x0000FBCC File Offset: 0x0000EBCC
		int IAccessible.accChildCount
		{
			get
			{
				int num = -1;
				if (this.IsClientObject)
				{
					num = this.GetChildCount();
				}
				if (num == -1)
				{
					if (this.systemIAccessible != null)
					{
						num = this.systemIAccessible.accChildCount;
					}
					else
					{
						num = 0;
					}
				}
				return num;
			}
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x0000FC08 File Offset: 0x0000EC08
		string IAccessible.get_accDefaultAction(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.DefaultAction;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.DefaultAction;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.get_accDefaultAction(childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0000FC88 File Offset: 0x0000EC88
		string IAccessible.get_accDescription(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.Description;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.Description;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.get_accDescription(childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0000FD08 File Offset: 0x0000ED08
		private AccessibleObject GetAccessibleChild(object childID)
		{
			if (!childID.Equals(0))
			{
				int num = (int)childID - 1;
				if (num >= 0 && num < this.GetChildCount())
				{
					return this.GetChild(num);
				}
			}
			return null;
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600121C RID: 4636 RVA: 0x0000FD44 File Offset: 0x0000ED44
		object IAccessible.accFocus
		{
			get
			{
				if (this.IsClientObject)
				{
					AccessibleObject focused = this.GetFocused();
					if (focused != null)
					{
						return this.AsVariant(focused);
					}
				}
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.accFocus;
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0000FDA8 File Offset: 0x0000EDA8
		string IAccessible.get_accHelp(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.Help;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.Help;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.get_accHelp(childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0000FE28 File Offset: 0x0000EE28
		int IAccessible.get_accHelpTopic(out string pszHelpFile, object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.GetHelpTopic(out pszHelpFile);
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.GetHelpTopic(out pszHelpFile);
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.get_accHelpTopic(out pszHelpFile, childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			pszHelpFile = null;
			return -1;
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0000FEB0 File Offset: 0x0000EEB0
		string IAccessible.get_accKeyboardShortcut(object childID)
		{
			return this.get_accKeyboardShortcutInternal(childID);
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0000FEBC File Offset: 0x0000EEBC
		internal virtual string get_accKeyboardShortcutInternal(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.KeyboardShortcut;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.KeyboardShortcut;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.get_accKeyboardShortcut(childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0000FF3C File Offset: 0x0000EF3C
		string IAccessible.get_accName(object childID)
		{
			return this.get_accNameInternal(childID);
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0000FF48 File Offset: 0x0000EF48
		internal virtual string get_accNameInternal(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.Name;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.Name;
				}
			}
			if (this.systemIAccessible != null)
			{
				string text = this.systemIAccessible.get_accName(childID);
				if (this.IsClientObject && (text == null || text.Length == 0))
				{
					text = this.Name;
				}
				return text;
			}
			return null;
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x0000FFC0 File Offset: 0x0000EFC0
		object IAccessible.accParent
		{
			get
			{
				IntSecurity.UnmanagedCode.Demand();
				AccessibleObject accessibleObject = this.Parent;
				if (accessibleObject != null && accessibleObject == this)
				{
					accessibleObject = null;
				}
				return this.AsIAccessible(accessibleObject);
			}
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0000FFF0 File Offset: 0x0000EFF0
		object IAccessible.get_accRole(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return (int)this.Role;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return (int)accessibleChild.Role;
				}
			}
			if (this.systemIAccessible != null)
			{
				return this.systemIAccessible.get_accRole(childID);
			}
			return null;
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x00010054 File Offset: 0x0000F054
		object IAccessible.accSelection
		{
			get
			{
				if (this.IsClientObject)
				{
					AccessibleObject selected = this.GetSelected();
					if (selected != null)
					{
						return this.AsVariant(selected);
					}
				}
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.accSelection;
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x000100B8 File Offset: 0x0000F0B8
		object IAccessible.get_accState(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return (int)this.State;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return (int)accessibleChild.State;
				}
			}
			if (this.systemIAccessible != null)
			{
				return this.systemIAccessible.get_accState(childID);
			}
			return null;
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0001011C File Offset: 0x0000F11C
		string IAccessible.get_accValue(object childID)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.Value;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.Value;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.get_accValue(childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x000101A8 File Offset: 0x0000F1A8
		void IAccessible.set_accName(object childID, string newName)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					this.Name = newName;
					return;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					accessibleChild.Name = newName;
					return;
				}
			}
			if (this.systemIAccessible != null)
			{
				this.systemIAccessible.set_accName(childID, newName);
			}
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00010204 File Offset: 0x0000F204
		void IAccessible.set_accValue(object childID, string newValue)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					this.Value = newValue;
					return;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					accessibleChild.Value = newValue;
					return;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					this.systemIAccessible.set_accValue(childID, newValue);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x00010290 File Offset: 0x0000F290
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int UnsafeNativeMethods.IOleWindow.GetWindow(out IntPtr hwnd)
		{
			if (this.systemIOleWindow != null)
			{
				return this.systemIOleWindow.GetWindow(out hwnd);
			}
			AccessibleObject parent = this.Parent;
			if (parent != null)
			{
				return ((UnsafeNativeMethods.IOleWindow)parent).GetWindow(out hwnd);
			}
			hwnd = IntPtr.Zero;
			return -2147467259;
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x000102D4 File Offset: 0x0000F2D4
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void UnsafeNativeMethods.IOleWindow.ContextSensitiveHelp(int fEnterMode)
		{
			if (this.systemIOleWindow != null)
			{
				this.systemIOleWindow.ContextSensitiveHelp(fEnterMode);
				return;
			}
			AccessibleObject parent = this.Parent;
			if (parent != null)
			{
				((UnsafeNativeMethods.IOleWindow)parent).ContextSensitiveHelp(fEnterMode);
			}
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00010307 File Offset: 0x0000F307
		void UnsafeNativeMethods.IEnumVariant.Clone(UnsafeNativeMethods.IEnumVariant[] v)
		{
			this.EnumVariant.Clone(v);
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00010315 File Offset: 0x0000F315
		int UnsafeNativeMethods.IEnumVariant.Next(int n, IntPtr rgvar, int[] ns)
		{
			return this.EnumVariant.Next(n, rgvar, ns);
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00010325 File Offset: 0x0000F325
		void UnsafeNativeMethods.IEnumVariant.Reset()
		{
			this.EnumVariant.Reset();
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00010332 File Offset: 0x0000F332
		void UnsafeNativeMethods.IEnumVariant.Skip(int n)
		{
			this.EnumVariant.Skip(n);
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00010340 File Offset: 0x0000F340
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual AccessibleObject Navigate(AccessibleNavigation navdir)
		{
			if (this.GetChildCount() >= 0)
			{
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					if (this.Parent.GetChildCount() > 0)
					{
						return null;
					}
					break;
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					if (this.Parent.GetChildCount() > 0)
					{
						return null;
					}
					break;
				case AccessibleNavigation.FirstChild:
					return this.GetChild(0);
				case AccessibleNavigation.LastChild:
					return this.GetChild(this.GetChildCount() - 1);
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					object obj = null;
					if (!this.SysNavigate((int)navdir, 0, out obj))
					{
						obj = this.systemIAccessible.accNavigate((int)navdir, 0);
					}
					return this.WrapIAccessible(obj);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00010418 File Offset: 0x0000F418
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual void Select(AccessibleSelection flags)
		{
			if (this.systemIAccessible != null)
			{
				try
				{
					this.systemIAccessible.accSelect((int)flags, 0);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00010464 File Offset: 0x0000F464
		private object AsVariant(AccessibleObject obj)
		{
			if (obj == this)
			{
				return 0;
			}
			return this.AsIAccessible(obj);
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x00010478 File Offset: 0x0000F478
		private IAccessible AsIAccessible(AccessibleObject obj)
		{
			if (obj != null && obj.systemWrapper)
			{
				return obj.systemIAccessible;
			}
			return obj;
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x0001048D File Offset: 0x0000F48D
		// (set) Token: 0x06001235 RID: 4661 RVA: 0x00010495 File Offset: 0x0000F495
		internal int AccessibleObjectId
		{
			get
			{
				return this.accObjId;
			}
			set
			{
				this.accObjId = value;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x0001049E File Offset: 0x0000F49E
		internal bool IsClientObject
		{
			get
			{
				return this.AccessibleObjectId == -4;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06001237 RID: 4663 RVA: 0x000104AA File Offset: 0x0000F4AA
		internal bool IsNonClientObject
		{
			get
			{
				return this.AccessibleObjectId == 0;
			}
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x000104B5 File Offset: 0x0000F4B5
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal IAccessible GetSystemIAccessibleInternal()
		{
			return this.systemIAccessible;
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x000104BD File Offset: 0x0000F4BD
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected void UseStdAccessibleObjects(IntPtr handle)
		{
			this.UseStdAccessibleObjects(handle, this.AccessibleObjectId);
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x000104CC File Offset: 0x0000F4CC
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected void UseStdAccessibleObjects(IntPtr handle, int objid)
		{
			Guid guid = new Guid("{618736E0-3C3D-11CF-810C-00AA00389B71}");
			object obj = null;
			UnsafeNativeMethods.CreateStdAccessibleObject(new HandleRef(this, handle), objid, ref guid, ref obj);
			Guid guid2 = new Guid("{00020404-0000-0000-C000-000000000046}");
			object obj2 = null;
			UnsafeNativeMethods.CreateStdAccessibleObject(new HandleRef(this, handle), objid, ref guid2, ref obj2);
			if (obj != null || obj2 != null)
			{
				this.systemIAccessible = (IAccessible)obj;
				this.systemIEnumVariant = (UnsafeNativeMethods.IEnumVariant)obj2;
				this.systemIOleWindow = obj as UnsafeNativeMethods.IOleWindow;
			}
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00010550 File Offset: 0x0000F550
		private bool SysNavigate(int navDir, object childID, out object retObject)
		{
			retObject = null;
			if (!childID.Equals(0))
			{
				return false;
			}
			AccessibleObject accessibleObject;
			if (!this.GetSysChild((AccessibleNavigation)navDir, out accessibleObject))
			{
				return false;
			}
			retObject = ((accessibleObject == null) ? null : this.AsVariant(accessibleObject));
			return true;
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0001058D File Offset: 0x0000F58D
		internal void ValidateChildID(ref object childID)
		{
			if (childID == null)
			{
				childID = 0;
				return;
			}
			if (childID.Equals(-2147352572))
			{
				childID = 0;
				return;
			}
			if (!(childID is int))
			{
				childID = 0;
			}
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x000105CC File Offset: 0x0000F5CC
		private AccessibleObject WrapIAccessible(object iacc)
		{
			IAccessible accessible = iacc as IAccessible;
			if (accessible == null)
			{
				return null;
			}
			if (this.systemIAccessible == iacc)
			{
				return this;
			}
			return new AccessibleObject(accessible);
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x000105F6 File Offset: 0x0000F5F6
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return typeof(IAccessible).GetMethod(name, bindingAttr, binder, types, modifiers);
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x0001060E File Offset: 0x0000F60E
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetMethod(name, bindingAttr);
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00010621 File Offset: 0x0000F621
		MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetMethods(bindingAttr);
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00010633 File Offset: 0x0000F633
		FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetField(name, bindingAttr);
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00010646 File Offset: 0x0000F646
		FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetFields(bindingAttr);
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x00010658 File Offset: 0x0000F658
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetProperty(name, bindingAttr);
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0001066B File Offset: 0x0000F66B
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			return typeof(IAccessible).GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00010685 File Offset: 0x0000F685
		PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetProperties(bindingAttr);
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00010697 File Offset: 0x0000F697
		MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetMember(name, bindingAttr);
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x000106AA File Offset: 0x0000F6AA
		MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetMembers(bindingAttr);
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x000106BC File Offset: 0x0000F6BC
		object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			if (args.Length == 0)
			{
				MemberInfo[] member = typeof(IAccessible).GetMember(name);
				if (member != null && member.Length > 0 && member[0] is PropertyInfo)
				{
					MethodInfo getMethod = ((PropertyInfo)member[0]).GetGetMethod();
					if (getMethod != null && getMethod.GetParameters().Length > 0)
					{
						args = new object[getMethod.GetParameters().Length];
						for (int i = 0; i < args.Length; i++)
						{
							args[i] = 0;
						}
					}
				}
			}
			return typeof(IAccessible).InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06001249 RID: 4681 RVA: 0x00010753 File Offset: 0x0000F753
		Type IReflect.UnderlyingSystemType
		{
			get
			{
				return typeof(IAccessible);
			}
		}

		// Token: 0x04000FA2 RID: 4002
		private IAccessible systemIAccessible;

		// Token: 0x04000FA3 RID: 4003
		private UnsafeNativeMethods.IEnumVariant systemIEnumVariant;

		// Token: 0x04000FA4 RID: 4004
		private UnsafeNativeMethods.IEnumVariant enumVariant;

		// Token: 0x04000FA5 RID: 4005
		private UnsafeNativeMethods.IOleWindow systemIOleWindow;

		// Token: 0x04000FA6 RID: 4006
		private bool systemWrapper;

		// Token: 0x04000FA7 RID: 4007
		private int accObjId = -4;

		// Token: 0x020001D5 RID: 469
		private class EnumVariantObject : UnsafeNativeMethods.IEnumVariant
		{
			// Token: 0x0600124A RID: 4682 RVA: 0x0001075F File Offset: 0x0000F75F
			public EnumVariantObject(AccessibleObject owner)
			{
				this.owner = owner;
			}

			// Token: 0x0600124B RID: 4683 RVA: 0x0001076E File Offset: 0x0000F76E
			public EnumVariantObject(AccessibleObject owner, int currentChild)
			{
				this.owner = owner;
				this.currentChild = currentChild;
			}

			// Token: 0x0600124C RID: 4684 RVA: 0x00010784 File Offset: 0x0000F784
			void UnsafeNativeMethods.IEnumVariant.Clone(UnsafeNativeMethods.IEnumVariant[] v)
			{
				v[0] = new AccessibleObject.EnumVariantObject(this.owner, this.currentChild);
			}

			// Token: 0x0600124D RID: 4685 RVA: 0x0001079C File Offset: 0x0000F79C
			void UnsafeNativeMethods.IEnumVariant.Reset()
			{
				this.currentChild = 0;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					if (this.owner.systemIEnumVariant != null)
					{
						this.owner.systemIEnumVariant.Reset();
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}

			// Token: 0x0600124E RID: 4686 RVA: 0x000107F0 File Offset: 0x0000F7F0
			void UnsafeNativeMethods.IEnumVariant.Skip(int n)
			{
				this.currentChild += n;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					if (this.owner.systemIEnumVariant != null)
					{
						this.owner.systemIEnumVariant.Skip(n);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}

			// Token: 0x0600124F RID: 4687 RVA: 0x0001084C File Offset: 0x0000F84C
			int UnsafeNativeMethods.IEnumVariant.Next(int n, IntPtr rgvar, int[] ns)
			{
				if (this.owner.IsClientObject)
				{
					int childCount;
					int[] sysChildOrder;
					if ((childCount = this.owner.GetChildCount()) >= 0)
					{
						this.NextFromChildCollection(n, rgvar, ns, childCount);
					}
					else if (this.owner.systemIEnumVariant == null)
					{
						this.NextEmpty(n, rgvar, ns);
					}
					else if ((sysChildOrder = this.owner.GetSysChildOrder()) != null)
					{
						this.NextFromSystemReordered(n, rgvar, ns, sysChildOrder);
					}
					else
					{
						this.NextFromSystem(n, rgvar, ns);
					}
				}
				else
				{
					this.NextFromSystem(n, rgvar, ns);
				}
				if (ns[0] != n)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x06001250 RID: 4688 RVA: 0x000108D4 File Offset: 0x0000F8D4
			private void NextFromSystem(int n, IntPtr rgvar, int[] ns)
			{
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					this.owner.systemIEnumVariant.Next(n, rgvar, ns);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				this.currentChild += ns[0];
			}

			// Token: 0x06001251 RID: 4689 RVA: 0x00010928 File Offset: 0x0000F928
			private void NextFromSystemReordered(int n, IntPtr rgvar, int[] ns, int[] newOrder)
			{
				int num = 0;
				while (num < n && this.currentChild < newOrder.Length && AccessibleObject.EnumVariantObject.GotoItem(this.owner.systemIEnumVariant, newOrder[this.currentChild], AccessibleObject.EnumVariantObject.GetAddressOfVariantAtIndex(rgvar, num)))
				{
					this.currentChild++;
					num++;
				}
				ns[0] = num;
			}

			// Token: 0x06001252 RID: 4690 RVA: 0x00010984 File Offset: 0x0000F984
			private void NextFromChildCollection(int n, IntPtr rgvar, int[] ns, int childCount)
			{
				int num = 0;
				while (num < n && this.currentChild < childCount)
				{
					this.currentChild++;
					Marshal.GetNativeVariantForObject(this.currentChild, AccessibleObject.EnumVariantObject.GetAddressOfVariantAtIndex(rgvar, num));
					num++;
				}
				ns[0] = num;
			}

			// Token: 0x06001253 RID: 4691 RVA: 0x000109D0 File Offset: 0x0000F9D0
			private void NextEmpty(int n, IntPtr rgvar, int[] ns)
			{
				ns[0] = 0;
			}

			// Token: 0x06001254 RID: 4692 RVA: 0x000109D8 File Offset: 0x0000F9D8
			private static bool GotoItem(UnsafeNativeMethods.IEnumVariant iev, int index, IntPtr variantPtr)
			{
				int[] array = new int[1];
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					iev.Reset();
					iev.Skip(index);
					iev.Next(1, variantPtr, array);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return array[0] == 1;
			}

			// Token: 0x06001255 RID: 4693 RVA: 0x00010A2C File Offset: 0x0000FA2C
			private static IntPtr GetAddressOfVariantAtIndex(IntPtr variantArrayPtr, int index)
			{
				int num = 8 + IntPtr.Size * 2;
				return (IntPtr)((long)variantArrayPtr + (long)index * (long)num);
			}

			// Token: 0x04000FA8 RID: 4008
			private int currentChild;

			// Token: 0x04000FA9 RID: 4009
			private AccessibleObject owner;
		}
	}
}

using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Accessibility;

namespace System.Windows.Forms
{
	// Token: 0x020001D6 RID: 470
	internal sealed class InternalAccessibleObject : StandardOleMarshalObject, UnsafeNativeMethods.IAccessibleInternal, IReflect, UnsafeNativeMethods.IEnumVariant, UnsafeNativeMethods.IOleWindow
	{
		// Token: 0x06001256 RID: 4694 RVA: 0x00010A54 File Offset: 0x0000FA54
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal InternalAccessibleObject(AccessibleObject accessibleImplemention)
		{
			this.publicIAccessible = accessibleImplemention;
			this.publicIEnumVariant = accessibleImplemention;
			this.publicIOleWindow = accessibleImplemention;
			this.publicIReflect = accessibleImplemention;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00010A78 File Offset: 0x0000FA78
		private object AsNativeAccessible(object accObject)
		{
			if (accObject is AccessibleObject)
			{
				return new InternalAccessibleObject(accObject as AccessibleObject);
			}
			return accObject;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00010A8F File Offset: 0x0000FA8F
		void UnsafeNativeMethods.IAccessibleInternal.accDoDefaultAction(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIAccessible.accDoDefaultAction(childID);
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00010AA7 File Offset: 0x0000FAA7
		object UnsafeNativeMethods.IAccessibleInternal.accHitTest(int xLeft, int yTop)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIAccessible.accHitTest(xLeft, yTop));
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x00010AC6 File Offset: 0x0000FAC6
		void UnsafeNativeMethods.IAccessibleInternal.accLocation(out int l, out int t, out int w, out int h, object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIAccessible.accLocation(out l, out t, out w, out h, childID);
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x00010AE4 File Offset: 0x0000FAE4
		object UnsafeNativeMethods.IAccessibleInternal.accNavigate(int navDir, object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIAccessible.accNavigate(navDir, childID));
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00010B03 File Offset: 0x0000FB03
		void UnsafeNativeMethods.IAccessibleInternal.accSelect(int flagsSelect, object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIAccessible.accSelect(flagsSelect, childID);
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00010B1C File Offset: 0x0000FB1C
		object UnsafeNativeMethods.IAccessibleInternal.get_accChild(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIAccessible.get_accChild(childID));
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00010B3A File Offset: 0x0000FB3A
		int UnsafeNativeMethods.IAccessibleInternal.get_accChildCount()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.accChildCount;
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x00010B51 File Offset: 0x0000FB51
		string UnsafeNativeMethods.IAccessibleInternal.get_accDefaultAction(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accDefaultAction(childID);
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00010B69 File Offset: 0x0000FB69
		string UnsafeNativeMethods.IAccessibleInternal.get_accDescription(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accDescription(childID);
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00010B81 File Offset: 0x0000FB81
		object UnsafeNativeMethods.IAccessibleInternal.get_accFocus()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIAccessible.accFocus);
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x00010B9E File Offset: 0x0000FB9E
		string UnsafeNativeMethods.IAccessibleInternal.get_accHelp(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accHelp(childID);
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x00010BB6 File Offset: 0x0000FBB6
		int UnsafeNativeMethods.IAccessibleInternal.get_accHelpTopic(out string pszHelpFile, object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accHelpTopic(out pszHelpFile, childID);
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00010BCF File Offset: 0x0000FBCF
		string UnsafeNativeMethods.IAccessibleInternal.get_accKeyboardShortcut(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accKeyboardShortcut(childID);
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00010BE7 File Offset: 0x0000FBE7
		string UnsafeNativeMethods.IAccessibleInternal.get_accName(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accName(childID);
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00010BFF File Offset: 0x0000FBFF
		object UnsafeNativeMethods.IAccessibleInternal.get_accParent()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIAccessible.accParent);
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00010C1C File Offset: 0x0000FC1C
		object UnsafeNativeMethods.IAccessibleInternal.get_accRole(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accRole(childID);
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x00010C34 File Offset: 0x0000FC34
		object UnsafeNativeMethods.IAccessibleInternal.get_accSelection()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIAccessible.accSelection);
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x00010C51 File Offset: 0x0000FC51
		object UnsafeNativeMethods.IAccessibleInternal.get_accState(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accState(childID);
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x00010C69 File Offset: 0x0000FC69
		string UnsafeNativeMethods.IAccessibleInternal.get_accValue(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accValue(childID);
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00010C81 File Offset: 0x0000FC81
		void UnsafeNativeMethods.IAccessibleInternal.set_accName(object childID, string newName)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIAccessible.set_accName(childID, newName);
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00010C9A File Offset: 0x0000FC9A
		void UnsafeNativeMethods.IAccessibleInternal.set_accValue(object childID, string newValue)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIAccessible.set_accValue(childID, newValue);
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00010CB3 File Offset: 0x0000FCB3
		void UnsafeNativeMethods.IEnumVariant.Clone(UnsafeNativeMethods.IEnumVariant[] v)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIEnumVariant.Clone(v);
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00010CCB File Offset: 0x0000FCCB
		int UnsafeNativeMethods.IEnumVariant.Next(int n, IntPtr rgvar, int[] ns)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIEnumVariant.Next(n, rgvar, ns);
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x00010CE5 File Offset: 0x0000FCE5
		void UnsafeNativeMethods.IEnumVariant.Reset()
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIEnumVariant.Reset();
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00010CFC File Offset: 0x0000FCFC
		void UnsafeNativeMethods.IEnumVariant.Skip(int n)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIEnumVariant.Skip(n);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00010D14 File Offset: 0x0000FD14
		int UnsafeNativeMethods.IOleWindow.GetWindow(out IntPtr hwnd)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIOleWindow.GetWindow(out hwnd);
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x00010D2C File Offset: 0x0000FD2C
		void UnsafeNativeMethods.IOleWindow.ContextSensitiveHelp(int fEnterMode)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIOleWindow.ContextSensitiveHelp(fEnterMode);
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x00010D44 File Offset: 0x0000FD44
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return this.publicIReflect.GetMethod(name, bindingAttr, binder, types, modifiers);
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x00010D58 File Offset: 0x0000FD58
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetMethod(name, bindingAttr);
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00010D67 File Offset: 0x0000FD67
		MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetMethods(bindingAttr);
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x00010D75 File Offset: 0x0000FD75
		FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetField(name, bindingAttr);
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00010D84 File Offset: 0x0000FD84
		FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetFields(bindingAttr);
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00010D92 File Offset: 0x0000FD92
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetProperty(name, bindingAttr);
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00010DA1 File Offset: 0x0000FDA1
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			return this.publicIReflect.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00010DB7 File Offset: 0x0000FDB7
		PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetProperties(bindingAttr);
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00010DC5 File Offset: 0x0000FDC5
		MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetMember(name, bindingAttr);
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00010DD4 File Offset: 0x0000FDD4
		MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetMembers(bindingAttr);
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00010DE4 File Offset: 0x0000FDE4
		object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			IntSecurity.UnmanagedCode.Demand();
			return this.publicIReflect.InvokeMember(name, invokeAttr, binder, this.publicIAccessible, args, modifiers, culture, namedParameters);
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600127E RID: 4734 RVA: 0x00010E17 File Offset: 0x0000FE17
		Type IReflect.UnderlyingSystemType
		{
			get
			{
				return this.publicIReflect.UnderlyingSystemType;
			}
		}

		// Token: 0x04000FAA RID: 4010
		private IAccessible publicIAccessible;

		// Token: 0x04000FAB RID: 4011
		private UnsafeNativeMethods.IEnumVariant publicIEnumVariant;

		// Token: 0x04000FAC RID: 4012
		private UnsafeNativeMethods.IOleWindow publicIOleWindow;

		// Token: 0x04000FAD RID: 4013
		private IReflect publicIReflect;
	}
}

using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;

namespace System
{
	// Token: 0x0200003E RID: 62
	[ComVisible(true)]
	[Serializable]
	public abstract class MulticastDelegate : Delegate
	{
		// Token: 0x060003B3 RID: 947 RVA: 0x0000ED08 File Offset: 0x0000DD08
		protected MulticastDelegate(object target, string method)
			: base(target, method)
		{
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000ED12 File Offset: 0x0000DD12
		protected MulticastDelegate(Type target, string method)
			: base(target, method)
		{
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000ED1C File Offset: 0x0000DD1C
		internal bool IsUnmanagedFunctionPtr()
		{
			return this._invocationCount == (IntPtr)(-1);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000ED30 File Offset: 0x0000DD30
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			int num = 0;
			object[] array = this._invocationList as object[];
			if (array == null)
			{
				MethodInfo method = base.Method;
				if (method is DynamicMethod || method is DynamicMethod.RTDynamicMethod || this.IsUnmanagedFunctionPtr())
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_InvalidDelegateType"));
				}
				if (this._invocationList != null && !this._invocationCount.IsNull())
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_InvalidDelegateType"));
				}
				DelegateSerializationHolder.GetDelegateSerializationInfo(info, base.GetType(), base.Target, method, num);
				return;
			}
			else
			{
				DelegateSerializationHolder.DelegateEntry delegateEntry = null;
				int num2 = (int)this._invocationCount;
				int num3 = num2;
				while (--num3 >= 0)
				{
					MulticastDelegate multicastDelegate = (MulticastDelegate)array[num3];
					MethodInfo method2 = multicastDelegate.Method;
					if (!(method2 is DynamicMethod) && !(method2 is DynamicMethod.RTDynamicMethod) && !this.IsUnmanagedFunctionPtr() && (multicastDelegate._invocationList == null || multicastDelegate._invocationCount.IsNull()))
					{
						DelegateSerializationHolder.DelegateEntry delegateSerializationInfo = DelegateSerializationHolder.GetDelegateSerializationInfo(info, multicastDelegate.GetType(), multicastDelegate.Target, method2, num++);
						if (delegateEntry != null)
						{
							delegateEntry.Entry = delegateSerializationInfo;
						}
						delegateEntry = delegateSerializationInfo;
					}
				}
				if (delegateEntry == null)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_InvalidDelegateType"));
				}
				return;
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000EE5C File Offset: 0x0000DE5C
		public sealed override bool Equals(object obj)
		{
			if (obj == null || !Delegate.InternalEqualTypes(this, obj))
			{
				return false;
			}
			MulticastDelegate multicastDelegate = obj as MulticastDelegate;
			if (multicastDelegate == null)
			{
				return false;
			}
			if (this._invocationCount != (IntPtr)0)
			{
				if (this._invocationList == null)
				{
					if (this.IsUnmanagedFunctionPtr())
					{
						return multicastDelegate.IsUnmanagedFunctionPtr() && !(this._methodPtr != multicastDelegate._methodPtr) && !(base.GetUnmanagedCallSite() != multicastDelegate.GetUnmanagedCallSite());
					}
					return base.Equals(obj);
				}
				else
				{
					if (this._invocationList is Delegate)
					{
						return this._invocationList.Equals(obj);
					}
					return this.InvocationListEquals(multicastDelegate);
				}
			}
			else
			{
				if (this._invocationList != null)
				{
					return this._invocationList.Equals(multicastDelegate._invocationList) && base.Equals(multicastDelegate);
				}
				if (multicastDelegate._invocationList != null || multicastDelegate._invocationCount != (IntPtr)0)
				{
					return multicastDelegate._invocationList is Delegate && (multicastDelegate._invocationList as Delegate).Equals(this);
				}
				return base.Equals(multicastDelegate);
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000EF70 File Offset: 0x0000DF70
		private bool InvocationListEquals(MulticastDelegate d)
		{
			object[] array = this._invocationList as object[];
			if (d._invocationCount != this._invocationCount)
			{
				return false;
			}
			int num = (int)this._invocationCount;
			for (int i = 0; i < num; i++)
			{
				Delegate @delegate = (Delegate)array[i];
				object[] array2 = d._invocationList as object[];
				if (!@delegate.Equals(array2[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000EFDC File Offset: 0x0000DFDC
		private bool TrySetSlot(object[] a, int index, object o)
		{
			if (a[index] == null && Interlocked.CompareExchange(ref a[index], o, null) == null)
			{
				return true;
			}
			if (a[index] != null)
			{
				MulticastDelegate multicastDelegate = (MulticastDelegate)o;
				MulticastDelegate multicastDelegate2 = (MulticastDelegate)a[index];
				if (multicastDelegate2._methodPtr == multicastDelegate._methodPtr && multicastDelegate2._target == multicastDelegate._target && multicastDelegate2._methodPtrAux == multicastDelegate._methodPtrAux)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000F04C File Offset: 0x0000E04C
		internal MulticastDelegate NewMulticastDelegate(object[] invocationList, int invocationCount, bool thisIsMultiCastAlready)
		{
			MulticastDelegate multicastDelegate = Delegate.InternalAllocLike(this);
			if (thisIsMultiCastAlready)
			{
				multicastDelegate._methodPtr = this._methodPtr;
				multicastDelegate._methodPtrAux = this._methodPtrAux;
			}
			else
			{
				multicastDelegate._methodPtr = base.GetMulticastInvoke();
				multicastDelegate._methodPtrAux = base.GetInvokeMethod();
			}
			multicastDelegate._target = multicastDelegate;
			multicastDelegate._invocationList = invocationList;
			multicastDelegate._invocationCount = (IntPtr)invocationCount;
			return multicastDelegate;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000F0B0 File Offset: 0x0000E0B0
		internal MulticastDelegate NewMulticastDelegate(object[] invocationList, int invocationCount)
		{
			return this.NewMulticastDelegate(invocationList, invocationCount, false);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000F0BC File Offset: 0x0000E0BC
		internal void StoreDynamicMethod(MethodInfo dynamicMethod)
		{
			if (this._invocationCount != (IntPtr)0)
			{
				MulticastDelegate multicastDelegate = (MulticastDelegate)this._invocationList;
				multicastDelegate._methodBase = dynamicMethod;
				return;
			}
			this._methodBase = dynamicMethod;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000F0F8 File Offset: 0x0000E0F8
		protected sealed override Delegate CombineImpl(Delegate follow)
		{
			if (follow == null)
			{
				return this;
			}
			if (!Delegate.InternalEqualTypes(this, follow))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_DlgtTypeMis"));
			}
			MulticastDelegate multicastDelegate = (MulticastDelegate)follow;
			int num = 1;
			object[] array = multicastDelegate._invocationList as object[];
			if (array != null)
			{
				num = (int)multicastDelegate._invocationCount;
			}
			object[] array2 = this._invocationList as object[];
			int num2;
			object[] array3;
			if (array2 == null)
			{
				num2 = 1 + num;
				array3 = new object[num2];
				array3[0] = this;
				if (array == null)
				{
					array3[1] = multicastDelegate;
				}
				else
				{
					for (int i = 0; i < num; i++)
					{
						array3[1 + i] = array[i];
					}
				}
				return this.NewMulticastDelegate(array3, num2);
			}
			int num3 = (int)this._invocationCount;
			num2 = num3 + num;
			array3 = null;
			if (num2 <= array2.Length)
			{
				array3 = array2;
				if (array == null)
				{
					if (!this.TrySetSlot(array3, num3, multicastDelegate))
					{
						array3 = null;
					}
				}
				else
				{
					for (int j = 0; j < num; j++)
					{
						if (!this.TrySetSlot(array3, num3 + j, array[j]))
						{
							array3 = null;
							break;
						}
					}
				}
			}
			if (array3 == null)
			{
				int k;
				for (k = array2.Length; k < num2; k *= 2)
				{
				}
				array3 = new object[k];
				for (int l = 0; l < num3; l++)
				{
					array3[l] = array2[l];
				}
				if (array == null)
				{
					array3[num3] = multicastDelegate;
				}
				else
				{
					for (int m = 0; m < num; m++)
					{
						array3[num3 + m] = array[m];
					}
				}
			}
			return this.NewMulticastDelegate(array3, num2, true);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000F258 File Offset: 0x0000E258
		private object[] DeleteFromInvocationList(object[] invocationList, int invocationCount, int deleteIndex, int deleteCount)
		{
			object[] array = this._invocationList as object[];
			int num = array.Length;
			while (num / 2 >= invocationCount - deleteCount)
			{
				num /= 2;
			}
			object[] array2 = new object[num];
			for (int i = 0; i < deleteIndex; i++)
			{
				array2[i] = invocationList[i];
			}
			for (int j = deleteIndex + deleteCount; j < invocationCount; j++)
			{
				array2[j - deleteCount] = invocationList[j];
			}
			return array2;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000F2BC File Offset: 0x0000E2BC
		private bool EqualInvocationLists(object[] a, object[] b, int start, int count)
		{
			for (int i = 0; i < count; i++)
			{
				if (!a[start + i].Equals(b[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000F2E8 File Offset: 0x0000E2E8
		protected sealed override Delegate RemoveImpl(Delegate value)
		{
			MulticastDelegate multicastDelegate = value as MulticastDelegate;
			if (multicastDelegate == null)
			{
				return this;
			}
			if (!(multicastDelegate._invocationList is object[]))
			{
				object[] array = this._invocationList as object[];
				if (array == null)
				{
					if (this.Equals(value))
					{
						return null;
					}
				}
				else
				{
					int num = (int)this._invocationCount;
					int num2 = num;
					while (--num2 >= 0)
					{
						if (value.Equals(array[num2]))
						{
							if (num == 2)
							{
								return (Delegate)array[1 - num2];
							}
							object[] array2 = this.DeleteFromInvocationList(array, num, num2, 1);
							return this.NewMulticastDelegate(array2, num - 1, true);
						}
					}
				}
			}
			else
			{
				object[] array3 = this._invocationList as object[];
				if (array3 != null)
				{
					int num3 = (int)this._invocationCount;
					int num4 = (int)multicastDelegate._invocationCount;
					int i = num3 - num4;
					while (i >= 0)
					{
						if (this.EqualInvocationLists(array3, multicastDelegate._invocationList as object[], i, num4))
						{
							if (num3 - num4 == 0)
							{
								return null;
							}
							if (num3 - num4 == 1)
							{
								return (Delegate)array3[(i != 0) ? 0 : (num3 - 1)];
							}
							object[] array4 = this.DeleteFromInvocationList(array3, num3, i, num4);
							return this.NewMulticastDelegate(array4, num3 - num4, true);
						}
						else
						{
							i--;
						}
					}
				}
			}
			return this;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000F41C File Offset: 0x0000E41C
		public sealed override Delegate[] GetInvocationList()
		{
			object[] array = this._invocationList as object[];
			Delegate[] array2;
			if (array == null)
			{
				array2 = new Delegate[] { this };
			}
			else
			{
				int num = (int)this._invocationCount;
				array2 = new Delegate[num];
				for (int i = 0; i < num; i++)
				{
					array2[i] = (Delegate)array[i];
				}
			}
			return array2;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000F470 File Offset: 0x0000E470
		public static bool operator ==(MulticastDelegate d1, MulticastDelegate d2)
		{
			if (d1 == null)
			{
				return d2 == null;
			}
			return d1.Equals(d2);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000F481 File Offset: 0x0000E481
		public static bool operator !=(MulticastDelegate d1, MulticastDelegate d2)
		{
			if (d1 == null)
			{
				return d2 != null;
			}
			return !d1.Equals(d2);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000F498 File Offset: 0x0000E498
		public sealed override int GetHashCode()
		{
			if (this.IsUnmanagedFunctionPtr())
			{
				return (int)(long)this._methodPtr;
			}
			object[] array = this._invocationList as object[];
			if (array == null)
			{
				return base.GetHashCode();
			}
			int num = 0;
			for (int i = 0; i < (int)this._invocationCount; i++)
			{
				num = num * 33 + array[i].GetHashCode();
			}
			return num;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000F4F8 File Offset: 0x0000E4F8
		internal override object GetTarget()
		{
			if (this._invocationCount != (IntPtr)0)
			{
				if (this._invocationList == null)
				{
					return null;
				}
				object[] array = this._invocationList as object[];
				if (array != null)
				{
					int num = (int)this._invocationCount;
					return ((Delegate)array[num - 1]).GetTarget();
				}
				Delegate @delegate = this._invocationList as Delegate;
				if (@delegate != null)
				{
					return @delegate.GetTarget();
				}
			}
			return base.GetTarget();
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000F56C File Offset: 0x0000E56C
		protected override MethodInfo GetMethodImpl()
		{
			if (!(this._invocationCount != (IntPtr)0) || this._invocationList == null)
			{
				return base.GetMethodImpl();
			}
			object[] array = this._invocationList as object[];
			if (array != null)
			{
				int num = (int)this._invocationCount - 1;
				return ((Delegate)array[num]).Method;
			}
			return ((MulticastDelegate)this._invocationList).GetMethodImpl();
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000F5D6 File Offset: 0x0000E5D6
		[DebuggerNonUserCode]
		private void ThrowNullThisInDelegateToInstance()
		{
			throw new ArgumentException(Environment.GetResourceString("Arg_DlgtNullInst"));
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000F5E7 File Offset: 0x0000E5E7
		[DebuggerNonUserCode]
		private void CtorClosed(object target, IntPtr methodPtr)
		{
			if (target == null)
			{
				this.ThrowNullThisInDelegateToInstance();
			}
			this._target = target;
			this._methodPtr = methodPtr;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000F600 File Offset: 0x0000E600
		[DebuggerNonUserCode]
		private void CtorClosedStatic(object target, IntPtr methodPtr)
		{
			this._target = target;
			this._methodPtr = methodPtr;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000F610 File Offset: 0x0000E610
		[DebuggerNonUserCode]
		private void CtorRTClosed(object target, IntPtr methodPtr)
		{
			this._target = target;
			this._methodPtr = base.AdjustTarget(target, methodPtr);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000F627 File Offset: 0x0000E627
		[DebuggerNonUserCode]
		private void CtorOpened(object target, IntPtr methodPtr, IntPtr shuffleThunk)
		{
			this._target = this;
			this._methodPtr = shuffleThunk;
			this._methodPtrAux = methodPtr;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000F640 File Offset: 0x0000E640
		[DebuggerNonUserCode]
		private void CtorSecureClosed(object target, IntPtr methodPtr, IntPtr callThunk, IntPtr assembly)
		{
			MulticastDelegate multicastDelegate = Delegate.InternalAlloc(Type.GetTypeHandle(this));
			multicastDelegate.CtorClosed(target, methodPtr);
			this._invocationList = multicastDelegate;
			this._target = this;
			this._methodPtr = callThunk;
			this._methodPtrAux = assembly;
			this._invocationCount = base.GetInvokeMethod();
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000F68C File Offset: 0x0000E68C
		[DebuggerNonUserCode]
		private void CtorSecureClosedStatic(object target, IntPtr methodPtr, IntPtr callThunk, IntPtr assembly)
		{
			MulticastDelegate multicastDelegate = Delegate.InternalAlloc(Type.GetTypeHandle(this));
			multicastDelegate.CtorClosedStatic(target, methodPtr);
			this._invocationList = multicastDelegate;
			this._target = this;
			this._methodPtr = callThunk;
			this._methodPtrAux = assembly;
			this._invocationCount = base.GetInvokeMethod();
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000F6D8 File Offset: 0x0000E6D8
		[DebuggerNonUserCode]
		private void CtorSecureRTClosed(object target, IntPtr methodPtr, IntPtr callThunk, IntPtr assembly)
		{
			MulticastDelegate multicastDelegate = Delegate.InternalAlloc(Type.GetTypeHandle(this));
			multicastDelegate.CtorRTClosed(target, methodPtr);
			this._invocationList = multicastDelegate;
			this._target = this;
			this._methodPtr = callThunk;
			this._methodPtrAux = assembly;
			this._invocationCount = base.GetInvokeMethod();
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000F724 File Offset: 0x0000E724
		[DebuggerNonUserCode]
		private void CtorSecureOpened(object target, IntPtr methodPtr, IntPtr shuffleThunk, IntPtr callThunk, IntPtr assembly)
		{
			MulticastDelegate multicastDelegate = Delegate.InternalAlloc(Type.GetTypeHandle(this));
			multicastDelegate.CtorOpened(target, methodPtr, shuffleThunk);
			this._invocationList = multicastDelegate;
			this._target = this;
			this._methodPtr = callThunk;
			this._methodPtrAux = assembly;
			this._invocationCount = base.GetInvokeMethod();
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000F770 File Offset: 0x0000E770
		[DebuggerNonUserCode]
		private void CtorVirtualDispatch(object target, IntPtr methodPtr, IntPtr shuffleThunk)
		{
			this._target = this;
			this._methodPtr = shuffleThunk;
			this._methodPtrAux = base.GetCallStub(methodPtr);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000F790 File Offset: 0x0000E790
		[DebuggerNonUserCode]
		private void CtorSecureVirtualDispatch(object target, IntPtr methodPtr, IntPtr shuffleThunk, IntPtr callThunk, IntPtr assembly)
		{
			MulticastDelegate multicastDelegate = Delegate.InternalAlloc(Type.GetTypeHandle(this));
			multicastDelegate.CtorVirtualDispatch(target, methodPtr, shuffleThunk);
			this._invocationList = multicastDelegate;
			this._target = this;
			this._methodPtr = callThunk;
			this._methodPtrAux = assembly;
			this._invocationCount = base.GetInvokeMethod();
		}

		// Token: 0x04000116 RID: 278
		private object _invocationList;

		// Token: 0x04000117 RID: 279
		private IntPtr _invocationCount;
	}
}

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x0200033A RID: 826
	[Serializable]
	internal sealed class RtFieldInfo : RuntimeFieldInfo, ISerializable
	{
		// Token: 0x060020C3 RID: 8387
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PerformVisibilityCheckOnField(IntPtr field, object target, IntPtr declaringType, FieldAttributes attr, uint invocationFlags);

		// Token: 0x060020C4 RID: 8388 RVA: 0x00051D83 File Offset: 0x00050D83
		internal RtFieldInfo()
		{
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x00051D8B File Offset: 0x00050D8B
		internal RtFieldInfo(RuntimeFieldHandle handle, RuntimeType declaringType, RuntimeType.RuntimeTypeCache reflectedTypeCache, BindingFlags bindingFlags)
			: base(reflectedTypeCache, declaringType, bindingFlags)
		{
			this.m_fieldHandle = handle;
			this.m_fieldAttributes = this.m_fieldHandle.GetAttributes();
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x00051DB0 File Offset: 0x00050DB0
		private void GetOneTimeFlags()
		{
			Type declaringType = this.DeclaringType;
			uint num = 0U;
			if ((declaringType != null && declaringType.ContainsGenericParameters) || (declaringType == null && this.Module.Assembly.ReflectionOnly) || declaringType is ReflectionOnlyType)
			{
				num |= 2U;
			}
			else
			{
				AssemblyBuilderData assemblyData = this.Module.Assembly.m_assemblyData;
				if (assemblyData != null && (assemblyData.m_access & AssemblyBuilderAccess.Run) == (AssemblyBuilderAccess)0)
				{
					num |= 2U;
				}
			}
			if (num == 0U)
			{
				if ((this.m_fieldAttributes & FieldAttributes.InitOnly) != FieldAttributes.PrivateScope)
				{
					num |= 16U;
				}
				if ((this.m_fieldAttributes & FieldAttributes.HasFieldRVA) != FieldAttributes.PrivateScope)
				{
					num |= 16U;
				}
				if ((this.m_fieldAttributes & FieldAttributes.FieldAccessMask) != FieldAttributes.Public || (declaringType != null && !declaringType.IsVisible))
				{
					num |= 4U;
				}
				Type fieldType = this.FieldType;
				if (fieldType.IsPointer || fieldType.IsEnum || fieldType.IsPrimitive)
				{
					num |= 32U;
				}
			}
			num |= 1U;
			this.m_invocationFlags = num;
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x00051E88 File Offset: 0x00050E88
		private void CheckConsistency(object target)
		{
			if ((this.m_fieldAttributes & FieldAttributes.Static) == FieldAttributes.Static || this.m_declaringType.IsInstanceOfType(target))
			{
				return;
			}
			if (target == null)
			{
				throw new TargetException(Environment.GetResourceString("RFLCT.Targ_StatFldReqTarg"));
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Arg_FieldDeclTarget"), new object[]
			{
				this.Name,
				this.m_declaringType,
				target.GetType()
			}));
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x00051F00 File Offset: 0x00050F00
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			RtFieldInfo rtFieldInfo = o as RtFieldInfo;
			return rtFieldInfo != null && rtFieldInfo.m_fieldHandle.Equals(this.m_fieldHandle);
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x00051F2A File Offset: 0x00050F2A
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal void InternalSetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture, bool doVisibilityCheck)
		{
			this.InternalSetValue(obj, value, invokeAttr, binder, culture, doVisibilityCheck, true);
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x00051F3C File Offset: 0x00050F3C
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal void InternalSetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture, bool doVisibilityCheck, bool doCheckConsistency)
		{
			RuntimeType runtimeType = this.DeclaringType as RuntimeType;
			if ((this.m_invocationFlags & 1U) == 0U)
			{
				this.GetOneTimeFlags();
			}
			if ((this.m_invocationFlags & 2U) != 0U)
			{
				if (runtimeType != null && runtimeType.ContainsGenericParameters)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_UnboundGenField"));
				}
				if ((runtimeType == null && this.Module.Assembly.ReflectionOnly) || runtimeType is ReflectionOnlyType)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_ReflectionOnlyField"));
				}
				throw new FieldAccessException();
			}
			else
			{
				if (doCheckConsistency)
				{
					this.CheckConsistency(obj);
				}
				value = ((RuntimeType)this.FieldType).CheckValue(value, binder, culture, invokeAttr);
				if (doVisibilityCheck && (this.m_invocationFlags & 20U) != 0U)
				{
					RtFieldInfo.PerformVisibilityCheckOnField(this.m_fieldHandle.Value, obj, this.m_declaringType.TypeHandle.Value, this.m_fieldAttributes, this.m_invocationFlags);
				}
				bool flag = false;
				if (runtimeType == null)
				{
					this.m_fieldHandle.SetValue(obj, value, this.FieldType.TypeHandle, this.m_fieldAttributes, RuntimeTypeHandle.EmptyHandle, ref flag);
					return;
				}
				flag = runtimeType.DomainInitialized;
				this.m_fieldHandle.SetValue(obj, value, this.FieldType.TypeHandle, this.m_fieldAttributes, this.DeclaringType.TypeHandle, ref flag);
				runtimeType.DomainInitialized = flag;
				return;
			}
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x00052083 File Offset: 0x00051083
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal object InternalGetValue(object obj, bool doVisibilityCheck)
		{
			return this.InternalGetValue(obj, doVisibilityCheck, true);
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x00052090 File Offset: 0x00051090
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal object InternalGetValue(object obj, bool doVisibilityCheck, bool doCheckConsistency)
		{
			RuntimeType runtimeType = this.DeclaringType as RuntimeType;
			if ((this.m_invocationFlags & 1U) == 0U)
			{
				this.GetOneTimeFlags();
			}
			if ((this.m_invocationFlags & 2U) != 0U)
			{
				if (runtimeType != null && this.DeclaringType.ContainsGenericParameters)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_UnboundGenField"));
				}
				if ((runtimeType == null && this.Module.Assembly.ReflectionOnly) || runtimeType is ReflectionOnlyType)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_ReflectionOnlyField"));
				}
				throw new FieldAccessException();
			}
			else
			{
				if (doCheckConsistency)
				{
					this.CheckConsistency(obj);
				}
				RuntimeTypeHandle typeHandle = this.FieldType.TypeHandle;
				if (doVisibilityCheck && (this.m_invocationFlags & 4U) != 0U)
				{
					RtFieldInfo.PerformVisibilityCheckOnField(this.m_fieldHandle.Value, obj, this.m_declaringType.TypeHandle.Value, this.m_fieldAttributes, this.m_invocationFlags & 4294967279U);
				}
				bool flag = false;
				if (runtimeType == null)
				{
					return this.m_fieldHandle.GetValue(obj, typeHandle, RuntimeTypeHandle.EmptyHandle, ref flag);
				}
				flag = runtimeType.DomainInitialized;
				object value = this.m_fieldHandle.GetValue(obj, typeHandle, this.DeclaringType.TypeHandle, ref flag);
				runtimeType.DomainInitialized = flag;
				return value;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x060020CD RID: 8397 RVA: 0x000521B1 File Offset: 0x000511B1
		public override string Name
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = this.m_fieldHandle.GetName();
				}
				return this.m_name;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x060020CE RID: 8398 RVA: 0x000521D2 File Offset: 0x000511D2
		public override int MetadataToken
		{
			get
			{
				return this.m_fieldHandle.GetToken();
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x060020CF RID: 8399 RVA: 0x000521E0 File Offset: 0x000511E0
		public override Module Module
		{
			get
			{
				return this.m_fieldHandle.GetApproxDeclaringType().GetModuleHandle().GetModule();
			}
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00052208 File Offset: 0x00051208
		public override object GetValue(object obj)
		{
			return this.InternalGetValue(obj, true);
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x00052212 File Offset: 0x00051212
		public override object GetRawConstantValue()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x0005221C File Offset: 0x0005121C
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override object GetValueDirect(TypedReference obj)
		{
			if (obj.IsNull)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_TypedReference_Null"));
			}
			return this.m_fieldHandle.GetValueDirect(this.FieldType.TypeHandle, obj, (this.DeclaringType == null) ? RuntimeTypeHandle.EmptyHandle : this.DeclaringType.TypeHandle);
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x00052273 File Offset: 0x00051273
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			this.InternalSetValue(obj, value, invokeAttr, binder, culture, true);
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x00052284 File Offset: 0x00051284
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override void SetValueDirect(TypedReference obj, object value)
		{
			if (obj.IsNull)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_TypedReference_Null"));
			}
			this.m_fieldHandle.SetValueDirect(this.FieldType.TypeHandle, obj, value, (this.DeclaringType == null) ? RuntimeTypeHandle.EmptyHandle : this.DeclaringType.TypeHandle);
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x060020D5 RID: 8405 RVA: 0x000522DC File Offset: 0x000512DC
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				Type declaringType = this.DeclaringType;
				if ((declaringType == null && this.Module.Assembly.ReflectionOnly) || declaringType is ReflectionOnlyType)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAllowedInReflectionOnly"));
				}
				return this.m_fieldHandle;
			}
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x00052323 File Offset: 0x00051323
		internal override RuntimeFieldHandle GetFieldHandle()
		{
			return this.m_fieldHandle;
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x060020D7 RID: 8407 RVA: 0x0005232B File Offset: 0x0005132B
		public override FieldAttributes Attributes
		{
			get
			{
				return this.m_fieldAttributes;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x060020D8 RID: 8408 RVA: 0x00052334 File Offset: 0x00051334
		public override Type FieldType
		{
			get
			{
				if (this.m_fieldType == null)
				{
					this.m_fieldType = new Signature(this.m_fieldHandle, base.DeclaringTypeHandle).FieldTypeHandle.GetRuntimeType();
				}
				return this.m_fieldType;
			}
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x00052373 File Offset: 0x00051373
		public override Type[] GetRequiredCustomModifiers()
		{
			return new Signature(this.m_fieldHandle, base.DeclaringTypeHandle).GetCustomModifiers(1, true);
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x0005238D File Offset: 0x0005138D
		public override Type[] GetOptionalCustomModifiers()
		{
			return new Signature(this.m_fieldHandle, base.DeclaringTypeHandle).GetCustomModifiers(1, false);
		}

		// Token: 0x04000DB0 RID: 3504
		private RuntimeFieldHandle m_fieldHandle;

		// Token: 0x04000DB1 RID: 3505
		private FieldAttributes m_fieldAttributes;

		// Token: 0x04000DB2 RID: 3506
		private string m_name;

		// Token: 0x04000DB3 RID: 3507
		private RuntimeType m_fieldType;

		// Token: 0x04000DB4 RID: 3508
		private uint m_invocationFlags;
	}
}

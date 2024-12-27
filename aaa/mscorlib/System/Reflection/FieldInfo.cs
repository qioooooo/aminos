using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000333 RID: 819
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_FieldInfo))]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public abstract class FieldInfo : MemberInfo, _FieldInfo
	{
		// Token: 0x06002011 RID: 8209 RVA: 0x00050404 File Offset: 0x0004F404
		public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle)
		{
			if (handle.IsNullHandle())
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidHandle"));
			}
			FieldInfo fieldInfo = RuntimeType.GetFieldInfo(handle);
			if (fieldInfo.DeclaringType != null && fieldInfo.DeclaringType.IsGenericType)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_FieldDeclaringTypeGeneric"), new object[]
				{
					fieldInfo.Name,
					fieldInfo.DeclaringType.GetGenericTypeDefinition()
				}));
			}
			return fieldInfo;
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x00050480 File Offset: 0x0004F480
		[ComVisible(false)]
		public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle, RuntimeTypeHandle declaringType)
		{
			if (handle.IsNullHandle())
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidHandle"));
			}
			return RuntimeType.GetFieldInfo(declaringType, handle);
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06002014 RID: 8212 RVA: 0x000504AA File Offset: 0x0004F4AA
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Field;
			}
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x000504AD File Offset: 0x0004F4AD
		public virtual Type[] GetRequiredCustomModifiers()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x000504B4 File Offset: 0x0004F4B4
		public virtual Type[] GetOptionalCustomModifiers()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x000504BB File Offset: 0x0004F4BB
		[CLSCompliant(false)]
		public virtual void SetValueDirect(TypedReference obj, object value)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_AbstractNonCLS"));
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x000504CC File Offset: 0x0004F4CC
		[CLSCompliant(false)]
		public virtual object GetValueDirect(TypedReference obj)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_AbstractNonCLS"));
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06002019 RID: 8217
		public abstract RuntimeFieldHandle FieldHandle { get; }

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x0600201A RID: 8218
		public abstract Type FieldType { get; }

		// Token: 0x0600201B RID: 8219
		public abstract object GetValue(object obj);

		// Token: 0x0600201C RID: 8220 RVA: 0x000504DD File Offset: 0x0004F4DD
		public virtual object GetRawConstantValue()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_AbstractNonCLS"));
		}

		// Token: 0x0600201D RID: 8221
		public abstract void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture);

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x0600201E RID: 8222
		public abstract FieldAttributes Attributes { get; }

		// Token: 0x0600201F RID: 8223 RVA: 0x000504EE File Offset: 0x0004F4EE
		[DebuggerHidden]
		[DebuggerStepThrough]
		public void SetValue(object obj, object value)
		{
			this.SetValue(obj, value, BindingFlags.Default, Type.DefaultBinder, null);
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06002020 RID: 8224 RVA: 0x000504FF File Offset: 0x0004F4FF
		public bool IsPublic
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06002021 RID: 8225 RVA: 0x0005050C File Offset: 0x0004F50C
		public bool IsPrivate
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Private;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x00050519 File Offset: 0x0004F519
		public bool IsFamily
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Family;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06002023 RID: 8227 RVA: 0x00050526 File Offset: 0x0004F526
		public bool IsAssembly
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly;
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06002024 RID: 8228 RVA: 0x00050533 File Offset: 0x0004F533
		public bool IsFamilyAndAssembly
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamANDAssem;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06002025 RID: 8229 RVA: 0x00050540 File Offset: 0x0004F540
		public bool IsFamilyOrAssembly
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamORAssem;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x0005054D File Offset: 0x0004F54D
		public bool IsStatic
		{
			get
			{
				return (this.Attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06002027 RID: 8231 RVA: 0x0005055E File Offset: 0x0004F55E
		public bool IsInitOnly
		{
			get
			{
				return (this.Attributes & FieldAttributes.InitOnly) != FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06002028 RID: 8232 RVA: 0x0005056F File Offset: 0x0004F56F
		public bool IsLiteral
		{
			get
			{
				return (this.Attributes & FieldAttributes.Literal) != FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06002029 RID: 8233 RVA: 0x00050580 File Offset: 0x0004F580
		public bool IsNotSerialized
		{
			get
			{
				return (this.Attributes & FieldAttributes.NotSerialized) != FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600202A RID: 8234 RVA: 0x00050594 File Offset: 0x0004F594
		public bool IsSpecialName
		{
			get
			{
				return (this.Attributes & FieldAttributes.SpecialName) != FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600202B RID: 8235 RVA: 0x000505A8 File Offset: 0x0004F5A8
		public bool IsPinvokeImpl
		{
			get
			{
				return (this.Attributes & FieldAttributes.PinvokeImpl) != FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x000505BC File Offset: 0x0004F5BC
		Type _FieldInfo.GetType()
		{
			return base.GetType();
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x000505C4 File Offset: 0x0004F5C4
		void _FieldInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x000505CB File Offset: 0x0004F5CB
		void _FieldInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x000505D2 File Offset: 0x0004F5D2
		void _FieldInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x000505D9 File Offset: 0x0004F5D9
		void _FieldInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}
	}
}

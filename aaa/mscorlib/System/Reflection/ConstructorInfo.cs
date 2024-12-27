using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000331 RID: 817
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_ConstructorInfo))]
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public abstract class ConstructorInfo : MethodBase, _ConstructorInfo
	{
		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001FF0 RID: 8176 RVA: 0x000502FC File Offset: 0x0004F2FC
		[ComVisible(true)]
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Constructor;
			}
		}

		// Token: 0x06001FF1 RID: 8177
		public abstract object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x06001FF2 RID: 8178 RVA: 0x000502FF File Offset: 0x0004F2FF
		internal override Type GetReturnType()
		{
			return this.DeclaringType;
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x00050307 File Offset: 0x0004F307
		[DebuggerHidden]
		[DebuggerStepThrough]
		public object Invoke(object[] parameters)
		{
			return this.Invoke(BindingFlags.Default, null, parameters, null);
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x00050313 File Offset: 0x0004F313
		Type _ConstructorInfo.GetType()
		{
			return base.GetType();
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x0005031B File Offset: 0x0004F31B
		object _ConstructorInfo.Invoke_2(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.Invoke(obj, invokeAttr, binder, parameters, culture);
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x0005032A File Offset: 0x0004F32A
		object _ConstructorInfo.Invoke_3(object obj, object[] parameters)
		{
			return base.Invoke(obj, parameters);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x00050334 File Offset: 0x0004F334
		object _ConstructorInfo.Invoke_4(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.Invoke(invokeAttr, binder, parameters, culture);
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x00050341 File Offset: 0x0004F341
		object _ConstructorInfo.Invoke_5(object[] parameters)
		{
			return this.Invoke(parameters);
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x0005034A File Offset: 0x0004F34A
		void _ConstructorInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x00050351 File Offset: 0x0004F351
		void _ConstructorInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x00050358 File Offset: 0x0004F358
		void _ConstructorInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x0005035F File Offset: 0x0004F35F
		void _ConstructorInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000D92 RID: 3474
		[ComVisible(true)]
		public static readonly string ConstructorName = ".ctor";

		// Token: 0x04000D93 RID: 3475
		[ComVisible(true)]
		public static readonly string TypeConstructorName = ".cctor";
	}
}

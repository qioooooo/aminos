using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000335 RID: 821
	[ComDefaultInterface(typeof(_PropertyInfo))]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public abstract class PropertyInfo : MemberInfo, _PropertyInfo
	{
		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06002047 RID: 8263 RVA: 0x00050746 File Offset: 0x0004F746
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Property;
			}
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x0005074A File Offset: 0x0004F74A
		public virtual object GetConstantValue()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x00050751 File Offset: 0x0004F751
		public virtual object GetRawConstantValue()
		{
			throw new NotImplementedException();
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x0600204A RID: 8266
		public abstract Type PropertyType { get; }

		// Token: 0x0600204B RID: 8267
		public abstract void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		// Token: 0x0600204C RID: 8268
		public abstract MethodInfo[] GetAccessors(bool nonPublic);

		// Token: 0x0600204D RID: 8269
		public abstract MethodInfo GetGetMethod(bool nonPublic);

		// Token: 0x0600204E RID: 8270
		public abstract MethodInfo GetSetMethod(bool nonPublic);

		// Token: 0x0600204F RID: 8271
		public abstract ParameterInfo[] GetIndexParameters();

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06002050 RID: 8272
		public abstract PropertyAttributes Attributes { get; }

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06002051 RID: 8273
		public abstract bool CanRead { get; }

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06002052 RID: 8274
		public abstract bool CanWrite { get; }

		// Token: 0x06002053 RID: 8275 RVA: 0x00050758 File Offset: 0x0004F758
		[DebuggerHidden]
		[DebuggerStepThrough]
		public virtual object GetValue(object obj, object[] index)
		{
			return this.GetValue(obj, BindingFlags.Default, null, index, null);
		}

		// Token: 0x06002054 RID: 8276
		public abstract object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		// Token: 0x06002055 RID: 8277 RVA: 0x00050765 File Offset: 0x0004F765
		[DebuggerHidden]
		[DebuggerStepThrough]
		public virtual void SetValue(object obj, object value, object[] index)
		{
			this.SetValue(obj, value, BindingFlags.Default, null, index, null);
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x00050773 File Offset: 0x0004F773
		public virtual Type[] GetRequiredCustomModifiers()
		{
			return new Type[0];
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x0005077B File Offset: 0x0004F77B
		public virtual Type[] GetOptionalCustomModifiers()
		{
			return new Type[0];
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x00050783 File Offset: 0x0004F783
		public MethodInfo[] GetAccessors()
		{
			return this.GetAccessors(false);
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x0005078C File Offset: 0x0004F78C
		public MethodInfo GetGetMethod()
		{
			return this.GetGetMethod(false);
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x00050795 File Offset: 0x0004F795
		public MethodInfo GetSetMethod()
		{
			return this.GetSetMethod(false);
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x0600205B RID: 8283 RVA: 0x0005079E File Offset: 0x0004F79E
		public bool IsSpecialName
		{
			get
			{
				return (this.Attributes & PropertyAttributes.SpecialName) != PropertyAttributes.None;
			}
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x000507B2 File Offset: 0x0004F7B2
		Type _PropertyInfo.GetType()
		{
			return base.GetType();
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000507BA File Offset: 0x0004F7BA
		void _PropertyInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x000507C1 File Offset: 0x0004F7C1
		void _PropertyInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x000507C8 File Offset: 0x0004F7C8
		void _PropertyInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x000507CF File Offset: 0x0004F7CF
		void _PropertyInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}
	}
}

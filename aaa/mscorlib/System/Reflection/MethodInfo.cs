using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000332 RID: 818
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_MethodInfo))]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public abstract class MethodInfo : MethodBase, _MethodInfo
	{
		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001FFF RID: 8191 RVA: 0x00050384 File Offset: 0x0004F384
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Method;
			}
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x00050387 File Offset: 0x0004F387
		internal virtual MethodInfo GetParentDefinition()
		{
			return null;
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06002001 RID: 8193 RVA: 0x0005038A File Offset: 0x0004F38A
		public virtual Type ReturnType
		{
			get
			{
				return this.GetReturnType();
			}
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x00050392 File Offset: 0x0004F392
		internal override Type GetReturnType()
		{
			return this.ReturnType;
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06002003 RID: 8195 RVA: 0x0005039A File Offset: 0x0004F39A
		public virtual ParameterInfo ReturnParameter
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06002004 RID: 8196
		public abstract ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

		// Token: 0x06002005 RID: 8197
		public abstract MethodInfo GetBaseDefinition();

		// Token: 0x06002006 RID: 8198 RVA: 0x000503A1 File Offset: 0x0004F3A1
		[ComVisible(true)]
		public override Type[] GetGenericArguments()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x000503B2 File Offset: 0x0004F3B2
		[ComVisible(true)]
		public virtual MethodInfo GetGenericMethodDefinition()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06002008 RID: 8200 RVA: 0x000503C3 File Offset: 0x0004F3C3
		public override bool IsGenericMethodDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06002009 RID: 8201 RVA: 0x000503C6 File Offset: 0x0004F3C6
		public override bool ContainsGenericParameters
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x000503C9 File Offset: 0x0004F3C9
		public virtual MethodInfo MakeGenericMethod(params Type[] typeArguments)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x0600200B RID: 8203 RVA: 0x000503DA File Offset: 0x0004F3DA
		public override bool IsGenericMethod
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x000503DD File Offset: 0x0004F3DD
		Type _MethodInfo.GetType()
		{
			return base.GetType();
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x000503E5 File Offset: 0x0004F3E5
		void _MethodInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x000503EC File Offset: 0x0004F3EC
		void _MethodInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x000503F3 File Offset: 0x0004F3F3
		void _MethodInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x000503FA File Offset: 0x0004F3FA
		void _MethodInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}
	}
}

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x020007FF RID: 2047
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_ConstructorBuilder))]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class ConstructorBuilder : ConstructorInfo, _ConstructorBuilder
	{
		// Token: 0x060048BF RID: 18623 RVA: 0x000FD49F File Offset: 0x000FC49F
		private ConstructorBuilder()
		{
		}

		// Token: 0x060048C0 RID: 18624 RVA: 0x000FD4A8 File Offset: 0x000FC4A8
		internal ConstructorBuilder(string name, MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers, Module mod, TypeBuilder type)
		{
			this.m_methodBuilder = new MethodBuilder(name, attributes, callingConvention, null, null, null, parameterTypes, requiredCustomModifiers, optionalCustomModifiers, mod, type, false);
			type.m_listMethods.Add(this.m_methodBuilder);
			int num;
			this.m_methodBuilder.GetMethodSignature().InternalGetSignature(out num);
			this.m_methodBuilder.GetToken();
			this.m_ReturnILGen = true;
		}

		// Token: 0x060048C1 RID: 18625 RVA: 0x000FD510 File Offset: 0x000FC510
		internal ConstructorBuilder(string name, MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Module mod, TypeBuilder type)
			: this(name, attributes, callingConvention, parameterTypes, null, null, mod, type)
		{
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x000FD52E File Offset: 0x000FC52E
		internal override Type[] GetParameterTypes()
		{
			return this.m_methodBuilder.GetParameterTypes();
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x000FD53B File Offset: 0x000FC53B
		public override string ToString()
		{
			return this.m_methodBuilder.ToString();
		}

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x060048C4 RID: 18628 RVA: 0x000FD548 File Offset: 0x000FC548
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_methodBuilder.MetadataTokenInternal;
			}
		}

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x060048C5 RID: 18629 RVA: 0x000FD555 File Offset: 0x000FC555
		public override Module Module
		{
			get
			{
				return this.m_methodBuilder.Module;
			}
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x060048C6 RID: 18630 RVA: 0x000FD562 File Offset: 0x000FC562
		public override Type ReflectedType
		{
			get
			{
				return this.m_methodBuilder.ReflectedType;
			}
		}

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x060048C7 RID: 18631 RVA: 0x000FD56F File Offset: 0x000FC56F
		public override Type DeclaringType
		{
			get
			{
				return this.m_methodBuilder.DeclaringType;
			}
		}

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x060048C8 RID: 18632 RVA: 0x000FD57C File Offset: 0x000FC57C
		public override string Name
		{
			get
			{
				return this.m_methodBuilder.Name;
			}
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x000FD589 File Offset: 0x000FC589
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x060048CA RID: 18634 RVA: 0x000FD59C File Offset: 0x000FC59C
		public override ParameterInfo[] GetParameters()
		{
			if (!this.m_methodBuilder.m_bIsBaked)
			{
				throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_TypeNotCreated"));
			}
			Type runtimeType = this.m_methodBuilder.GetTypeBuilder().m_runtimeType;
			ConstructorInfo constructor = runtimeType.GetConstructor(this.m_methodBuilder.m_parameterTypes);
			return constructor.GetParameters();
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x060048CB RID: 18635 RVA: 0x000FD5EF File Offset: 0x000FC5EF
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_methodBuilder.Attributes;
			}
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x000FD5FC File Offset: 0x000FC5FC
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_methodBuilder.GetMethodImplementationFlags();
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x060048CD RID: 18637 RVA: 0x000FD609 File Offset: 0x000FC609
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				return this.m_methodBuilder.MethodHandle;
			}
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x000FD616 File Offset: 0x000FC616
		public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x060048CF RID: 18639 RVA: 0x000FD627 File Offset: 0x000FC627
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_methodBuilder.GetCustomAttributes(inherit);
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x000FD635 File Offset: 0x000FC635
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_methodBuilder.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x060048D1 RID: 18641 RVA: 0x000FD644 File Offset: 0x000FC644
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_methodBuilder.IsDefined(attributeType, inherit);
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x000FD653 File Offset: 0x000FC653
		public MethodToken GetToken()
		{
			return this.m_methodBuilder.GetToken();
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x000FD660 File Offset: 0x000FC660
		public ParameterBuilder DefineParameter(int iSequence, ParameterAttributes attributes, string strParamName)
		{
			attributes &= ~ParameterAttributes.ReservedMask;
			return this.m_methodBuilder.DefineParameter(iSequence, attributes, strParamName);
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x000FD679 File Offset: 0x000FC679
		public void SetSymCustomAttribute(string name, byte[] data)
		{
			this.m_methodBuilder.SetSymCustomAttribute(name, data);
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x000FD688 File Offset: 0x000FC688
		public ILGenerator GetILGenerator()
		{
			if (!this.m_ReturnILGen)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DefaultConstructorILGen"));
			}
			return this.m_methodBuilder.GetILGenerator();
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x000FD6AD File Offset: 0x000FC6AD
		public ILGenerator GetILGenerator(int streamSize)
		{
			if (!this.m_ReturnILGen)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DefaultConstructorILGen"));
			}
			return this.m_methodBuilder.GetILGenerator(streamSize);
		}

		// Token: 0x060048D7 RID: 18647 RVA: 0x000FD6D4 File Offset: 0x000FC6D4
		public void AddDeclarativeSecurity(SecurityAction action, PermissionSet pset)
		{
			if (pset == null)
			{
				throw new ArgumentNullException("pset");
			}
			if (!Enum.IsDefined(typeof(SecurityAction), action) || action == SecurityAction.RequestMinimum || action == SecurityAction.RequestOptional || action == SecurityAction.RequestRefuse)
			{
				throw new ArgumentOutOfRangeException("action");
			}
			if (this.m_methodBuilder.IsTypeCreated())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TypeHasBeenCreated"));
			}
			byte[] array = pset.EncodeXml();
			TypeBuilder.InternalAddDeclarativeSecurity(this.GetModule(), this.GetToken().Token, action, array);
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x060048D8 RID: 18648 RVA: 0x000FD75E File Offset: 0x000FC75E
		public override CallingConventions CallingConvention
		{
			get
			{
				if (this.DeclaringType.IsGenericType)
				{
					return CallingConventions.HasThis;
				}
				return CallingConventions.Standard;
			}
		}

		// Token: 0x060048D9 RID: 18649 RVA: 0x000FD771 File Offset: 0x000FC771
		public Module GetModule()
		{
			return this.m_methodBuilder.GetModule();
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x060048DA RID: 18650 RVA: 0x000FD77E File Offset: 0x000FC77E
		public Type ReturnType
		{
			get
			{
				return this.m_methodBuilder.GetReturnType();
			}
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x000FD78B File Offset: 0x000FC78B
		internal override Type GetReturnType()
		{
			return this.m_methodBuilder.GetReturnType();
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x060048DC RID: 18652 RVA: 0x000FD798 File Offset: 0x000FC798
		public string Signature
		{
			get
			{
				return this.m_methodBuilder.Signature;
			}
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x000FD7A5 File Offset: 0x000FC7A5
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			this.m_methodBuilder.SetCustomAttribute(con, binaryAttribute);
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x000FD7B4 File Offset: 0x000FC7B4
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			this.m_methodBuilder.SetCustomAttribute(customBuilder);
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x000FD7C2 File Offset: 0x000FC7C2
		public void SetImplementationFlags(MethodImplAttributes attributes)
		{
			this.m_methodBuilder.SetImplementationFlags(attributes);
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x060048E0 RID: 18656 RVA: 0x000FD7D0 File Offset: 0x000FC7D0
		// (set) Token: 0x060048E1 RID: 18657 RVA: 0x000FD7DD File Offset: 0x000FC7DD
		public bool InitLocals
		{
			get
			{
				return this.m_methodBuilder.InitLocals;
			}
			set
			{
				this.m_methodBuilder.InitLocals = value;
			}
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x000FD7EB File Offset: 0x000FC7EB
		void _ConstructorBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x000FD7F2 File Offset: 0x000FC7F2
		void _ConstructorBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x000FD7F9 File Offset: 0x000FC7F9
		void _ConstructorBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x000FD800 File Offset: 0x000FC800
		void _ConstructorBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400253F RID: 9535
		internal MethodBuilder m_methodBuilder;

		// Token: 0x04002540 RID: 9536
		internal bool m_ReturnILGen;
	}
}

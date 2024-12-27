using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x0200082F RID: 2095
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_PropertyBuilder))]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class PropertyBuilder : PropertyInfo, _PropertyBuilder
	{
		// Token: 0x06004BAE RID: 19374 RVA: 0x0010A16D File Offset: 0x0010916D
		private PropertyBuilder()
		{
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x0010A178 File Offset: 0x00109178
		internal PropertyBuilder(Module mod, string name, SignatureHelper sig, PropertyAttributes attr, Type returnType, PropertyToken prToken, TypeBuilder containingType)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (name[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IllegalName"), "name");
			}
			this.m_name = name;
			this.m_module = mod;
			this.m_signature = sig;
			this.m_attributes = attr;
			this.m_returnType = returnType;
			this.m_prToken = prToken;
			this.m_tkProperty = prToken.Token;
			this.m_getMethod = null;
			this.m_setMethod = null;
			this.m_containingType = containingType;
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x0010A224 File Offset: 0x00109224
		public void SetConstant(object defaultValue)
		{
			this.m_containingType.ThrowIfCreated();
			TypeBuilder.SetConstantValue(this.m_module, this.m_prToken.Token, this.m_returnType, defaultValue);
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06004BB1 RID: 19377 RVA: 0x0010A24E File Offset: 0x0010924E
		public PropertyToken PropertyToken
		{
			get
			{
				return this.m_prToken;
			}
		}

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06004BB2 RID: 19378 RVA: 0x0010A256 File Offset: 0x00109256
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_tkProperty;
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06004BB3 RID: 19379 RVA: 0x0010A25E File Offset: 0x0010925E
		public override Module Module
		{
			get
			{
				return this.m_containingType.Module;
			}
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x0010A26C File Offset: 0x0010926C
		public void SetGetMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.m_containingType.ThrowIfCreated();
			TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_prToken.Token, MethodSemanticsAttributes.Getter, mdBuilder.GetToken().Token);
			this.m_getMethod = mdBuilder;
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x0010A2C0 File Offset: 0x001092C0
		public void SetSetMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.m_containingType.ThrowIfCreated();
			TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_prToken.Token, MethodSemanticsAttributes.Setter, mdBuilder.GetToken().Token);
			this.m_setMethod = mdBuilder;
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x0010A314 File Offset: 0x00109314
		public void AddOtherMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.m_containingType.ThrowIfCreated();
			TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_prToken.Token, MethodSemanticsAttributes.Other, mdBuilder.GetToken().Token);
		}

		// Token: 0x06004BB7 RID: 19383 RVA: 0x0010A360 File Offset: 0x00109360
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			this.m_containingType.ThrowIfCreated();
			TypeBuilder.InternalCreateCustomAttribute(this.m_prToken.Token, ((ModuleBuilder)this.m_module).GetConstructorToken(con).Token, binaryAttribute, this.m_module, false);
		}

		// Token: 0x06004BB8 RID: 19384 RVA: 0x0010A3C5 File Offset: 0x001093C5
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			this.m_containingType.ThrowIfCreated();
			customBuilder.CreateCustomAttribute((ModuleBuilder)this.m_module, this.m_prToken.Token);
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x0010A3FC File Offset: 0x001093FC
		public override object GetValue(object obj, object[] index)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x0010A40D File Offset: 0x0010940D
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x0010A41E File Offset: 0x0010941E
		public override void SetValue(object obj, object value, object[] index)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x0010A42F File Offset: 0x0010942F
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004BBD RID: 19389 RVA: 0x0010A440 File Offset: 0x00109440
		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004BBE RID: 19390 RVA: 0x0010A451 File Offset: 0x00109451
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			if (nonPublic || this.m_getMethod == null)
			{
				return this.m_getMethod;
			}
			if ((this.m_getMethod.Attributes & MethodAttributes.Public) == MethodAttributes.Public)
			{
				return this.m_getMethod;
			}
			return null;
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x0010A47D File Offset: 0x0010947D
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			if (nonPublic || this.m_setMethod == null)
			{
				return this.m_setMethod;
			}
			if ((this.m_setMethod.Attributes & MethodAttributes.Public) == MethodAttributes.Public)
			{
				return this.m_setMethod;
			}
			return null;
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x0010A4A9 File Offset: 0x001094A9
		public override ParameterInfo[] GetIndexParameters()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06004BC1 RID: 19393 RVA: 0x0010A4BA File Offset: 0x001094BA
		public override Type PropertyType
		{
			get
			{
				return this.m_returnType;
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06004BC2 RID: 19394 RVA: 0x0010A4C2 File Offset: 0x001094C2
		public override PropertyAttributes Attributes
		{
			get
			{
				return this.m_attributes;
			}
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x06004BC3 RID: 19395 RVA: 0x0010A4CA File Offset: 0x001094CA
		public override bool CanRead
		{
			get
			{
				return this.m_getMethod != null;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x06004BC4 RID: 19396 RVA: 0x0010A4D7 File Offset: 0x001094D7
		public override bool CanWrite
		{
			get
			{
				return this.m_setMethod != null;
			}
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x0010A4E4 File Offset: 0x001094E4
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x0010A4F5 File Offset: 0x001094F5
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x0010A506 File Offset: 0x00109506
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x0010A517 File Offset: 0x00109517
		void _PropertyBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x0010A51E File Offset: 0x0010951E
		void _PropertyBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x0010A525 File Offset: 0x00109525
		void _PropertyBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x0010A52C File Offset: 0x0010952C
		void _PropertyBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x06004BCC RID: 19404 RVA: 0x0010A533 File Offset: 0x00109533
		public override string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x06004BCD RID: 19405 RVA: 0x0010A53B File Offset: 0x0010953B
		public override Type DeclaringType
		{
			get
			{
				return this.m_containingType;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x06004BCE RID: 19406 RVA: 0x0010A543 File Offset: 0x00109543
		public override Type ReflectedType
		{
			get
			{
				return this.m_containingType;
			}
		}

		// Token: 0x04002780 RID: 10112
		private string m_name;

		// Token: 0x04002781 RID: 10113
		private PropertyToken m_prToken;

		// Token: 0x04002782 RID: 10114
		private int m_tkProperty;

		// Token: 0x04002783 RID: 10115
		private Module m_module;

		// Token: 0x04002784 RID: 10116
		private SignatureHelper m_signature;

		// Token: 0x04002785 RID: 10117
		private PropertyAttributes m_attributes;

		// Token: 0x04002786 RID: 10118
		private Type m_returnType;

		// Token: 0x04002787 RID: 10119
		private MethodInfo m_getMethod;

		// Token: 0x04002788 RID: 10120
		private MethodInfo m_setMethod;

		// Token: 0x04002789 RID: 10121
		private TypeBuilder m_containingType;
	}
}

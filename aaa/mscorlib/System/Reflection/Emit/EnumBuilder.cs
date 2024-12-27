using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x02000839 RID: 2105
	[ComDefaultInterface(typeof(_EnumBuilder))]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class EnumBuilder : Type, _EnumBuilder
	{
		// Token: 0x06004D61 RID: 19809 RVA: 0x0010ED04 File Offset: 0x0010DD04
		public FieldBuilder DefineLiteral(string literalName, object literalValue)
		{
			FieldBuilder fieldBuilder = this.m_typeBuilder.DefineField(literalName, this, FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static | FieldAttributes.Literal);
			fieldBuilder.SetConstant(literalValue);
			return fieldBuilder;
		}

		// Token: 0x06004D62 RID: 19810 RVA: 0x0010ED29 File Offset: 0x0010DD29
		public Type CreateType()
		{
			return this.m_typeBuilder.CreateType();
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x06004D63 RID: 19811 RVA: 0x0010ED36 File Offset: 0x0010DD36
		public TypeToken TypeToken
		{
			get
			{
				return this.m_typeBuilder.TypeToken;
			}
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x06004D64 RID: 19812 RVA: 0x0010ED43 File Offset: 0x0010DD43
		public FieldBuilder UnderlyingField
		{
			get
			{
				return this.m_underlyingField;
			}
		}

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x06004D65 RID: 19813 RVA: 0x0010ED4B File Offset: 0x0010DD4B
		public override string Name
		{
			get
			{
				return this.m_typeBuilder.Name;
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x06004D66 RID: 19814 RVA: 0x0010ED58 File Offset: 0x0010DD58
		public override Guid GUID
		{
			get
			{
				return this.m_typeBuilder.GUID;
			}
		}

		// Token: 0x06004D67 RID: 19815 RVA: 0x0010ED68 File Offset: 0x0010DD68
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			return this.m_typeBuilder.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x06004D68 RID: 19816 RVA: 0x0010ED8D File Offset: 0x0010DD8D
		public override Module Module
		{
			get
			{
				return this.m_typeBuilder.Module;
			}
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06004D69 RID: 19817 RVA: 0x0010ED9A File Offset: 0x0010DD9A
		public override Assembly Assembly
		{
			get
			{
				return this.m_typeBuilder.Assembly;
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x06004D6A RID: 19818 RVA: 0x0010EDA7 File Offset: 0x0010DDA7
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				return this.m_typeBuilder.TypeHandle;
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x06004D6B RID: 19819 RVA: 0x0010EDB4 File Offset: 0x0010DDB4
		public override string FullName
		{
			get
			{
				return this.m_typeBuilder.FullName;
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x06004D6C RID: 19820 RVA: 0x0010EDC1 File Offset: 0x0010DDC1
		public override string AssemblyQualifiedName
		{
			get
			{
				return this.m_typeBuilder.AssemblyQualifiedName;
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06004D6D RID: 19821 RVA: 0x0010EDCE File Offset: 0x0010DDCE
		public override string Namespace
		{
			get
			{
				return this.m_typeBuilder.Namespace;
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06004D6E RID: 19822 RVA: 0x0010EDDB File Offset: 0x0010DDDB
		public override Type BaseType
		{
			get
			{
				return this.m_typeBuilder.BaseType;
			}
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x0010EDE8 File Offset: 0x0010DDE8
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			return this.m_typeBuilder.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06004D70 RID: 19824 RVA: 0x0010EDFC File Offset: 0x0010DDFC
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			return this.m_typeBuilder.GetConstructors(bindingAttr);
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x0010EE0A File Offset: 0x0010DE0A
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (types == null)
			{
				return this.m_typeBuilder.GetMethod(name, bindingAttr);
			}
			return this.m_typeBuilder.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x0010EE32 File Offset: 0x0010DE32
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return this.m_typeBuilder.GetMethods(bindingAttr);
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x0010EE40 File Offset: 0x0010DE40
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			return this.m_typeBuilder.GetField(name, bindingAttr);
		}

		// Token: 0x06004D74 RID: 19828 RVA: 0x0010EE4F File Offset: 0x0010DE4F
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			return this.m_typeBuilder.GetFields(bindingAttr);
		}

		// Token: 0x06004D75 RID: 19829 RVA: 0x0010EE5D File Offset: 0x0010DE5D
		public override Type GetInterface(string name, bool ignoreCase)
		{
			return this.m_typeBuilder.GetInterface(name, ignoreCase);
		}

		// Token: 0x06004D76 RID: 19830 RVA: 0x0010EE6C File Offset: 0x0010DE6C
		public override Type[] GetInterfaces()
		{
			return this.m_typeBuilder.GetInterfaces();
		}

		// Token: 0x06004D77 RID: 19831 RVA: 0x0010EE79 File Offset: 0x0010DE79
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			return this.m_typeBuilder.GetEvent(name, bindingAttr);
		}

		// Token: 0x06004D78 RID: 19832 RVA: 0x0010EE88 File Offset: 0x0010DE88
		public override EventInfo[] GetEvents()
		{
			return this.m_typeBuilder.GetEvents();
		}

		// Token: 0x06004D79 RID: 19833 RVA: 0x0010EE95 File Offset: 0x0010DE95
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x0010EEA6 File Offset: 0x0010DEA6
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			return this.m_typeBuilder.GetProperties(bindingAttr);
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x0010EEB4 File Offset: 0x0010DEB4
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			return this.m_typeBuilder.GetNestedTypes(bindingAttr);
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x0010EEC2 File Offset: 0x0010DEC2
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			return this.m_typeBuilder.GetNestedType(name, bindingAttr);
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x0010EED1 File Offset: 0x0010DED1
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			return this.m_typeBuilder.GetMember(name, type, bindingAttr);
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x0010EEE1 File Offset: 0x0010DEE1
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			return this.m_typeBuilder.GetMembers(bindingAttr);
		}

		// Token: 0x06004D7F RID: 19839 RVA: 0x0010EEEF File Offset: 0x0010DEEF
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			return this.m_typeBuilder.GetInterfaceMap(interfaceType);
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x0010EEFD File Offset: 0x0010DEFD
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			return this.m_typeBuilder.GetEvents(bindingAttr);
		}

		// Token: 0x06004D81 RID: 19841 RVA: 0x0010EF0B File Offset: 0x0010DF0B
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.m_typeBuilder.m_iAttr;
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x0010EF18 File Offset: 0x0010DF18
		protected override bool IsArrayImpl()
		{
			return false;
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x0010EF1B File Offset: 0x0010DF1B
		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x0010EF1E File Offset: 0x0010DF1E
		protected override bool IsValueTypeImpl()
		{
			return true;
		}

		// Token: 0x06004D85 RID: 19845 RVA: 0x0010EF21 File Offset: 0x0010DF21
		protected override bool IsByRefImpl()
		{
			return false;
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x0010EF24 File Offset: 0x0010DF24
		protected override bool IsPointerImpl()
		{
			return false;
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x0010EF27 File Offset: 0x0010DF27
		protected override bool IsCOMObjectImpl()
		{
			return false;
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x0010EF2A File Offset: 0x0010DF2A
		public override Type GetElementType()
		{
			return this.m_typeBuilder.GetElementType();
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x0010EF37 File Offset: 0x0010DF37
		protected override bool HasElementTypeImpl()
		{
			return this.m_typeBuilder.HasElementType;
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06004D8A RID: 19850 RVA: 0x0010EF44 File Offset: 0x0010DF44
		public override Type UnderlyingSystemType
		{
			get
			{
				return this.m_underlyingType;
			}
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x0010EF4C File Offset: 0x0010DF4C
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_typeBuilder.GetCustomAttributes(inherit);
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x0010EF5A File Offset: 0x0010DF5A
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_typeBuilder.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x0010EF69 File Offset: 0x0010DF69
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			this.m_typeBuilder.SetCustomAttribute(con, binaryAttribute);
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x0010EF78 File Offset: 0x0010DF78
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			this.m_typeBuilder.SetCustomAttribute(customBuilder);
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06004D8F RID: 19855 RVA: 0x0010EF86 File Offset: 0x0010DF86
		public override Type DeclaringType
		{
			get
			{
				return this.m_typeBuilder.DeclaringType;
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06004D90 RID: 19856 RVA: 0x0010EF93 File Offset: 0x0010DF93
		public override Type ReflectedType
		{
			get
			{
				return this.m_typeBuilder.ReflectedType;
			}
		}

		// Token: 0x06004D91 RID: 19857 RVA: 0x0010EFA0 File Offset: 0x0010DFA0
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_typeBuilder.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06004D92 RID: 19858 RVA: 0x0010EFAF File Offset: 0x0010DFAF
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_typeBuilder.MetadataTokenInternal;
			}
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x0010EFBC File Offset: 0x0010DFBC
		private EnumBuilder()
		{
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x0010EFC4 File Offset: 0x0010DFC4
		public override Type MakePointerType()
		{
			return SymbolType.FormCompoundType("*".ToCharArray(), this, 0);
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x0010EFD7 File Offset: 0x0010DFD7
		public override Type MakeByRefType()
		{
			return SymbolType.FormCompoundType("&".ToCharArray(), this, 0);
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x0010EFEA File Offset: 0x0010DFEA
		public override Type MakeArrayType()
		{
			return SymbolType.FormCompoundType("[]".ToCharArray(), this, 0);
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x0010F000 File Offset: 0x0010E000
		public override Type MakeArrayType(int rank)
		{
			if (rank <= 0)
			{
				throw new IndexOutOfRangeException();
			}
			string text = "";
			if (rank == 1)
			{
				text = "*";
			}
			else
			{
				for (int i = 1; i < rank; i++)
				{
					text += ",";
				}
			}
			string text2 = string.Format(CultureInfo.InvariantCulture, "[{0}]", new object[] { text });
			return SymbolType.FormCompoundType(text2.ToCharArray(), this, 0);
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x0010F06C File Offset: 0x0010E06C
		internal EnumBuilder(string name, Type underlyingType, TypeAttributes visibility, Module module)
		{
			if ((visibility & ~TypeAttributes.VisibilityMask) != TypeAttributes.NotPublic)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ShouldOnlySetVisibilityFlags"), "name");
			}
			this.m_typeBuilder = new TypeBuilder(name, visibility | TypeAttributes.Sealed, typeof(Enum), null, module, PackingSize.Unspecified, null);
			this.m_underlyingType = underlyingType;
			this.m_underlyingField = this.m_typeBuilder.DefineField("value__", underlyingType, FieldAttributes.Private | FieldAttributes.SpecialName);
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x0010F0DF File Offset: 0x0010E0DF
		void _EnumBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x0010F0E6 File Offset: 0x0010E0E6
		void _EnumBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004D9B RID: 19867 RVA: 0x0010F0ED File Offset: 0x0010E0ED
		void _EnumBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004D9C RID: 19868 RVA: 0x0010F0F4 File Offset: 0x0010E0F4
		void _EnumBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040027FA RID: 10234
		private Type m_underlyingType;

		// Token: 0x040027FB RID: 10235
		internal TypeBuilder m_typeBuilder;

		// Token: 0x040027FC RID: 10236
		private FieldBuilder m_underlyingField;

		// Token: 0x040027FD RID: 10237
		internal Type m_runtimeType;
	}
}

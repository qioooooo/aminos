using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000838 RID: 2104
	[ComVisible(true)]
	public sealed class GenericTypeParameterBuilder : Type
	{
		// Token: 0x06004D1D RID: 19741 RVA: 0x0010E9AD File Offset: 0x0010D9AD
		internal GenericTypeParameterBuilder(TypeBuilder type)
		{
			this.m_type = type;
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x0010E9BC File Offset: 0x0010D9BC
		public override string ToString()
		{
			return this.m_type.Name;
		}

		// Token: 0x06004D1F RID: 19743 RVA: 0x0010E9CC File Offset: 0x0010D9CC
		public override bool Equals(object o)
		{
			GenericTypeParameterBuilder genericTypeParameterBuilder = o as GenericTypeParameterBuilder;
			return genericTypeParameterBuilder != null && genericTypeParameterBuilder.m_type == this.m_type;
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x0010E9F3 File Offset: 0x0010D9F3
		public override int GetHashCode()
		{
			return this.m_type.GetHashCode();
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06004D21 RID: 19745 RVA: 0x0010EA00 File Offset: 0x0010DA00
		public override Type DeclaringType
		{
			get
			{
				return this.m_type.DeclaringType;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06004D22 RID: 19746 RVA: 0x0010EA0D File Offset: 0x0010DA0D
		public override Type ReflectedType
		{
			get
			{
				return this.m_type.ReflectedType;
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06004D23 RID: 19747 RVA: 0x0010EA1A File Offset: 0x0010DA1A
		public override string Name
		{
			get
			{
				return this.m_type.Name;
			}
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06004D24 RID: 19748 RVA: 0x0010EA27 File Offset: 0x0010DA27
		public override Module Module
		{
			get
			{
				return this.m_type.Module;
			}
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06004D25 RID: 19749 RVA: 0x0010EA34 File Offset: 0x0010DA34
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_type.MetadataTokenInternal;
			}
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x0010EA41 File Offset: 0x0010DA41
		public override Type MakePointerType()
		{
			return SymbolType.FormCompoundType("*".ToCharArray(), this, 0);
		}

		// Token: 0x06004D27 RID: 19751 RVA: 0x0010EA54 File Offset: 0x0010DA54
		public override Type MakeByRefType()
		{
			return SymbolType.FormCompoundType("&".ToCharArray(), this, 0);
		}

		// Token: 0x06004D28 RID: 19752 RVA: 0x0010EA67 File Offset: 0x0010DA67
		public override Type MakeArrayType()
		{
			return SymbolType.FormCompoundType("[]".ToCharArray(), this, 0);
		}

		// Token: 0x06004D29 RID: 19753 RVA: 0x0010EA7C File Offset: 0x0010DA7C
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
			return SymbolType.FormCompoundType(text2.ToCharArray(), this, 0) as SymbolType;
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06004D2A RID: 19754 RVA: 0x0010EAF0 File Offset: 0x0010DAF0
		public override Guid GUID
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06004D2B RID: 19755 RVA: 0x0010EAF7 File Offset: 0x0010DAF7
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06004D2C RID: 19756 RVA: 0x0010EAFE File Offset: 0x0010DAFE
		public override Assembly Assembly
		{
			get
			{
				return this.m_type.Assembly;
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06004D2D RID: 19757 RVA: 0x0010EB0B File Offset: 0x0010DB0B
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06004D2E RID: 19758 RVA: 0x0010EB12 File Offset: 0x0010DB12
		public override string FullName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06004D2F RID: 19759 RVA: 0x0010EB15 File Offset: 0x0010DB15
		public override string Namespace
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06004D30 RID: 19760 RVA: 0x0010EB18 File Offset: 0x0010DB18
		public override string AssemblyQualifiedName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06004D31 RID: 19761 RVA: 0x0010EB1B File Offset: 0x0010DB1B
		public override Type BaseType
		{
			get
			{
				return this.m_type.BaseType;
			}
		}

		// Token: 0x06004D32 RID: 19762 RVA: 0x0010EB28 File Offset: 0x0010DB28
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D33 RID: 19763 RVA: 0x0010EB2F File Offset: 0x0010DB2F
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D34 RID: 19764 RVA: 0x0010EB36 File Offset: 0x0010DB36
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x0010EB3D File Offset: 0x0010DB3D
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x0010EB44 File Offset: 0x0010DB44
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x0010EB4B File Offset: 0x0010DB4B
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D38 RID: 19768 RVA: 0x0010EB52 File Offset: 0x0010DB52
		public override Type GetInterface(string name, bool ignoreCase)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x0010EB59 File Offset: 0x0010DB59
		public override Type[] GetInterfaces()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D3A RID: 19770 RVA: 0x0010EB60 File Offset: 0x0010DB60
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D3B RID: 19771 RVA: 0x0010EB67 File Offset: 0x0010DB67
		public override EventInfo[] GetEvents()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D3C RID: 19772 RVA: 0x0010EB6E File Offset: 0x0010DB6E
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x0010EB75 File Offset: 0x0010DB75
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x0010EB7C File Offset: 0x0010DB7C
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D3F RID: 19775 RVA: 0x0010EB83 File Offset: 0x0010DB83
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D40 RID: 19776 RVA: 0x0010EB8A File Offset: 0x0010DB8A
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D41 RID: 19777 RVA: 0x0010EB91 File Offset: 0x0010DB91
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D42 RID: 19778 RVA: 0x0010EB98 File Offset: 0x0010DB98
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x0010EB9F File Offset: 0x0010DB9F
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x0010EBA6 File Offset: 0x0010DBA6
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x0010EBAD File Offset: 0x0010DBAD
		protected override bool IsArrayImpl()
		{
			return false;
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x0010EBB0 File Offset: 0x0010DBB0
		protected override bool IsByRefImpl()
		{
			return false;
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x0010EBB3 File Offset: 0x0010DBB3
		protected override bool IsPointerImpl()
		{
			return false;
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x0010EBB6 File Offset: 0x0010DBB6
		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x0010EBB9 File Offset: 0x0010DBB9
		protected override bool IsCOMObjectImpl()
		{
			return false;
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x0010EBBC File Offset: 0x0010DBBC
		public override Type GetElementType()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x0010EBC3 File Offset: 0x0010DBC3
		protected override bool HasElementTypeImpl()
		{
			return false;
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06004D4C RID: 19788 RVA: 0x0010EBC6 File Offset: 0x0010DBC6
		public override Type UnderlyingSystemType
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x0010EBC9 File Offset: 0x0010DBC9
		public override Type[] GetGenericArguments()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06004D4E RID: 19790 RVA: 0x0010EBD0 File Offset: 0x0010DBD0
		public override bool IsGenericTypeDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x06004D4F RID: 19791 RVA: 0x0010EBD3 File Offset: 0x0010DBD3
		public override bool IsGenericType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06004D50 RID: 19792 RVA: 0x0010EBD6 File Offset: 0x0010DBD6
		public override bool IsGenericParameter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06004D51 RID: 19793 RVA: 0x0010EBD9 File Offset: 0x0010DBD9
		public override int GenericParameterPosition
		{
			get
			{
				return this.m_type.GenericParameterPosition;
			}
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06004D52 RID: 19794 RVA: 0x0010EBE6 File Offset: 0x0010DBE6
		public override bool ContainsGenericParameters
		{
			get
			{
				return this.m_type.ContainsGenericParameters;
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x06004D53 RID: 19795 RVA: 0x0010EBF3 File Offset: 0x0010DBF3
		public override MethodBase DeclaringMethod
		{
			get
			{
				return this.m_type.DeclaringMethod;
			}
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x0010EC00 File Offset: 0x0010DC00
		public override Type GetGenericTypeDefinition()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x0010EC07 File Offset: 0x0010DC07
		public override Type MakeGenericType(params Type[] typeArguments)
		{
			throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericTypeDefinition"));
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x0010EC18 File Offset: 0x0010DC18
		protected override bool IsValueTypeImpl()
		{
			return false;
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x0010EC1B File Offset: 0x0010DC1B
		public override bool IsAssignableFrom(Type c)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x0010EC22 File Offset: 0x0010DC22
		[ComVisible(true)]
		public override bool IsSubclassOf(Type c)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x0010EC29 File Offset: 0x0010DC29
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x0010EC30 File Offset: 0x0010DC30
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x0010EC37 File Offset: 0x0010DC37
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D5C RID: 19804 RVA: 0x0010EC3E File Offset: 0x0010DC3E
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (this.m_type.m_ca == null)
			{
				this.m_type.m_ca = new ArrayList();
			}
			this.m_type.m_ca.Add(new TypeBuilder.CustAttr(con, binaryAttribute));
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x0010EC75 File Offset: 0x0010DC75
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (this.m_type.m_ca == null)
			{
				this.m_type.m_ca = new ArrayList();
			}
			this.m_type.m_ca.Add(new TypeBuilder.CustAttr(customBuilder));
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x0010ECAC File Offset: 0x0010DCAC
		public void SetBaseTypeConstraint(Type baseTypeConstraint)
		{
			this.m_type.CheckContext(new Type[] { baseTypeConstraint });
			this.m_type.SetParent(baseTypeConstraint);
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x0010ECDC File Offset: 0x0010DCDC
		[ComVisible(true)]
		public void SetInterfaceConstraints(params Type[] interfaceConstraints)
		{
			this.m_type.CheckContext(interfaceConstraints);
			this.m_type.SetInterfaces(interfaceConstraints);
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x0010ECF6 File Offset: 0x0010DCF6
		public void SetGenericParameterAttributes(GenericParameterAttributes genericParameterAttributes)
		{
			this.m_type.m_genParamAttributes = genericParameterAttributes;
		}

		// Token: 0x040027F9 RID: 10233
		internal TypeBuilder m_type;
	}
}

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000837 RID: 2103
	internal sealed class TypeBuilderInstantiation : Type
	{
		// Token: 0x06004CDF RID: 19679 RVA: 0x0010E681 File Offset: 0x0010D681
		internal TypeBuilderInstantiation(Type type, Type[] inst)
		{
			this.m_type = type;
			this.m_inst = inst;
		}

		// Token: 0x06004CE0 RID: 19680 RVA: 0x0010E697 File Offset: 0x0010D697
		public override string ToString()
		{
			return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.ToString);
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06004CE1 RID: 19681 RVA: 0x0010E6A0 File Offset: 0x0010D6A0
		public override Type DeclaringType
		{
			get
			{
				return this.m_type.DeclaringType;
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06004CE2 RID: 19682 RVA: 0x0010E6AD File Offset: 0x0010D6AD
		public override Type ReflectedType
		{
			get
			{
				return this.m_type.ReflectedType;
			}
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06004CE3 RID: 19683 RVA: 0x0010E6BA File Offset: 0x0010D6BA
		public override string Name
		{
			get
			{
				return this.m_type.Name;
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06004CE4 RID: 19684 RVA: 0x0010E6C7 File Offset: 0x0010D6C7
		public override Module Module
		{
			get
			{
				return this.m_type.Module;
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06004CE5 RID: 19685 RVA: 0x0010E6D4 File Offset: 0x0010D6D4
		internal override int MetadataTokenInternal
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06004CE6 RID: 19686 RVA: 0x0010E6DB File Offset: 0x0010D6DB
		public override Type MakePointerType()
		{
			return SymbolType.FormCompoundType("*".ToCharArray(), this, 0);
		}

		// Token: 0x06004CE7 RID: 19687 RVA: 0x0010E6EE File Offset: 0x0010D6EE
		public override Type MakeByRefType()
		{
			return SymbolType.FormCompoundType("&".ToCharArray(), this, 0);
		}

		// Token: 0x06004CE8 RID: 19688 RVA: 0x0010E701 File Offset: 0x0010D701
		public override Type MakeArrayType()
		{
			return SymbolType.FormCompoundType("[]".ToCharArray(), this, 0);
		}

		// Token: 0x06004CE9 RID: 19689 RVA: 0x0010E714 File Offset: 0x0010D714
		public override Type MakeArrayType(int rank)
		{
			if (rank <= 0)
			{
				throw new IndexOutOfRangeException();
			}
			string text = "";
			for (int i = 1; i < rank; i++)
			{
				text += ",";
			}
			string text2 = string.Format(CultureInfo.InvariantCulture, "[{0}]", new object[] { text });
			return SymbolType.FormCompoundType(text2.ToCharArray(), this, 0);
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06004CEA RID: 19690 RVA: 0x0010E772 File Offset: 0x0010D772
		public override Guid GUID
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06004CEB RID: 19691 RVA: 0x0010E779 File Offset: 0x0010D779
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x06004CEC RID: 19692 RVA: 0x0010E780 File Offset: 0x0010D780
		public override Assembly Assembly
		{
			get
			{
				return this.m_type.Assembly;
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x06004CED RID: 19693 RVA: 0x0010E78D File Offset: 0x0010D78D
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x06004CEE RID: 19694 RVA: 0x0010E794 File Offset: 0x0010D794
		public override string FullName
		{
			get
			{
				if (this.m_strFullQualName == null)
				{
					this.m_strFullQualName = TypeNameBuilder.ToString(this, TypeNameBuilder.Format.FullName);
				}
				return this.m_strFullQualName;
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06004CEF RID: 19695 RVA: 0x0010E7B1 File Offset: 0x0010D7B1
		public override string Namespace
		{
			get
			{
				return this.m_type.Namespace;
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06004CF0 RID: 19696 RVA: 0x0010E7BE File Offset: 0x0010D7BE
		public override string AssemblyQualifiedName
		{
			get
			{
				return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.AssemblyQualifiedName);
			}
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x0010E7C8 File Offset: 0x0010D7C8
		internal Type Substitute(Type[] substitutes)
		{
			Type[] genericArguments = this.GetGenericArguments();
			Type[] array = new Type[genericArguments.Length];
			for (int i = 0; i < array.Length; i++)
			{
				Type type = genericArguments[i];
				if (type is TypeBuilderInstantiation)
				{
					array[i] = (type as TypeBuilderInstantiation).Substitute(substitutes);
				}
				else if (type is GenericTypeParameterBuilder)
				{
					array[i] = substitutes[type.GenericParameterPosition];
				}
				else
				{
					array[i] = type;
				}
			}
			return this.GetGenericTypeDefinition().MakeGenericType(array);
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06004CF2 RID: 19698 RVA: 0x0010E838 File Offset: 0x0010D838
		public override Type BaseType
		{
			get
			{
				Type baseType = this.m_type.BaseType;
				if (baseType == null)
				{
					return null;
				}
				TypeBuilderInstantiation typeBuilderInstantiation = baseType as TypeBuilderInstantiation;
				if (typeBuilderInstantiation == null)
				{
					return baseType;
				}
				return typeBuilderInstantiation.Substitute(this.GetGenericArguments());
			}
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x0010E86E File Offset: 0x0010D86E
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x0010E875 File Offset: 0x0010D875
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x0010E87C File Offset: 0x0010D87C
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x0010E883 File Offset: 0x0010D883
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x0010E88A File Offset: 0x0010D88A
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x0010E891 File Offset: 0x0010D891
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004CF9 RID: 19705 RVA: 0x0010E898 File Offset: 0x0010D898
		public override Type GetInterface(string name, bool ignoreCase)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x0010E89F File Offset: 0x0010D89F
		public override Type[] GetInterfaces()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x0010E8A6 File Offset: 0x0010D8A6
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x0010E8AD File Offset: 0x0010D8AD
		public override EventInfo[] GetEvents()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x0010E8B4 File Offset: 0x0010D8B4
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x0010E8BB File Offset: 0x0010D8BB
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x0010E8C2 File Offset: 0x0010D8C2
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x0010E8C9 File Offset: 0x0010D8C9
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x0010E8D0 File Offset: 0x0010D8D0
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x0010E8D7 File Offset: 0x0010D8D7
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x0010E8DE File Offset: 0x0010D8DE
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x0010E8E5 File Offset: 0x0010D8E5
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x0010E8EC File Offset: 0x0010D8EC
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.m_type.Attributes;
		}

		// Token: 0x06004D06 RID: 19718 RVA: 0x0010E8F9 File Offset: 0x0010D8F9
		protected override bool IsArrayImpl()
		{
			return false;
		}

		// Token: 0x06004D07 RID: 19719 RVA: 0x0010E8FC File Offset: 0x0010D8FC
		protected override bool IsByRefImpl()
		{
			return false;
		}

		// Token: 0x06004D08 RID: 19720 RVA: 0x0010E8FF File Offset: 0x0010D8FF
		protected override bool IsPointerImpl()
		{
			return false;
		}

		// Token: 0x06004D09 RID: 19721 RVA: 0x0010E902 File Offset: 0x0010D902
		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		// Token: 0x06004D0A RID: 19722 RVA: 0x0010E905 File Offset: 0x0010D905
		protected override bool IsCOMObjectImpl()
		{
			return false;
		}

		// Token: 0x06004D0B RID: 19723 RVA: 0x0010E908 File Offset: 0x0010D908
		public override Type GetElementType()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D0C RID: 19724 RVA: 0x0010E90F File Offset: 0x0010D90F
		protected override bool HasElementTypeImpl()
		{
			return false;
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06004D0D RID: 19725 RVA: 0x0010E912 File Offset: 0x0010D912
		public override Type UnderlyingSystemType
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06004D0E RID: 19726 RVA: 0x0010E915 File Offset: 0x0010D915
		public override Type[] GetGenericArguments()
		{
			return this.m_inst;
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06004D0F RID: 19727 RVA: 0x0010E91D File Offset: 0x0010D91D
		public override bool IsGenericTypeDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06004D10 RID: 19728 RVA: 0x0010E920 File Offset: 0x0010D920
		public override bool IsGenericType
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06004D11 RID: 19729 RVA: 0x0010E923 File Offset: 0x0010D923
		public override bool IsGenericParameter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06004D12 RID: 19730 RVA: 0x0010E926 File Offset: 0x0010D926
		public override int GenericParameterPosition
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06004D13 RID: 19731 RVA: 0x0010E92D File Offset: 0x0010D92D
		protected override bool IsValueTypeImpl()
		{
			return this.m_type.IsValueType;
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06004D14 RID: 19732 RVA: 0x0010E93C File Offset: 0x0010D93C
		public override bool ContainsGenericParameters
		{
			get
			{
				for (int i = 0; i < this.m_inst.Length; i++)
				{
					if (this.m_inst[i].ContainsGenericParameters)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06004D15 RID: 19733 RVA: 0x0010E96E File Offset: 0x0010D96E
		public override MethodBase DeclaringMethod
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004D16 RID: 19734 RVA: 0x0010E971 File Offset: 0x0010D971
		public override Type GetGenericTypeDefinition()
		{
			return this.m_type;
		}

		// Token: 0x06004D17 RID: 19735 RVA: 0x0010E979 File Offset: 0x0010D979
		public override Type MakeGenericType(params Type[] inst)
		{
			throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericTypeDefinition"));
		}

		// Token: 0x06004D18 RID: 19736 RVA: 0x0010E98A File Offset: 0x0010D98A
		public override bool IsAssignableFrom(Type c)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D19 RID: 19737 RVA: 0x0010E991 File Offset: 0x0010D991
		[ComVisible(true)]
		public override bool IsSubclassOf(Type c)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D1A RID: 19738 RVA: 0x0010E998 File Offset: 0x0010D998
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x0010E99F File Offset: 0x0010D99F
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D1C RID: 19740 RVA: 0x0010E9A6 File Offset: 0x0010D9A6
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040027F6 RID: 10230
		private Type m_type;

		// Token: 0x040027F7 RID: 10231
		private Type[] m_inst;

		// Token: 0x040027F8 RID: 10232
		private string m_strFullQualName;
	}
}

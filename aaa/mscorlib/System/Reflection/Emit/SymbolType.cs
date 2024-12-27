using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200081F RID: 2079
	internal sealed class SymbolType : Type
	{
		// Token: 0x06004AAE RID: 19118 RVA: 0x00104028 File Offset: 0x00103028
		internal static Type FormCompoundType(char[] bFormat, Type baseType, int curIndex)
		{
			if (bFormat == null || curIndex == bFormat.Length)
			{
				return baseType;
			}
			if (bFormat[curIndex] == '&')
			{
				SymbolType symbolType = new SymbolType(TypeKind.IsByRef);
				symbolType.SetFormat(bFormat, curIndex, 1);
				curIndex++;
				if (curIndex != bFormat.Length)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_BadSigFormat"));
				}
				symbolType.SetElementType(baseType);
				return symbolType;
			}
			else
			{
				if (bFormat[curIndex] == '[')
				{
					SymbolType symbolType = new SymbolType(TypeKind.IsArray);
					int num = curIndex;
					curIndex++;
					int num2 = 0;
					int num3 = -1;
					while (bFormat[curIndex] != ']')
					{
						if (bFormat[curIndex] == '*')
						{
							symbolType.m_isSzArray = false;
							curIndex++;
						}
						if ((bFormat[curIndex] >= '0' && bFormat[curIndex] <= '9') || bFormat[curIndex] == '-')
						{
							bool flag = false;
							if (bFormat[curIndex] == '-')
							{
								flag = true;
								curIndex++;
							}
							while (bFormat[curIndex] >= '0' && bFormat[curIndex] <= '9')
							{
								num2 *= 10;
								num2 += (int)(bFormat[curIndex] - '0');
								curIndex++;
							}
							if (flag)
							{
								num2 = -num2;
							}
							num3 = num2 - 1;
						}
						if (bFormat[curIndex] == '.')
						{
							curIndex++;
							if (bFormat[curIndex] != '.')
							{
								throw new ArgumentException(Environment.GetResourceString("Argument_BadSigFormat"));
							}
							curIndex++;
							if ((bFormat[curIndex] >= '0' && bFormat[curIndex] <= '9') || bFormat[curIndex] == '-')
							{
								bool flag2 = false;
								num3 = 0;
								if (bFormat[curIndex] == '-')
								{
									flag2 = true;
									curIndex++;
								}
								while (bFormat[curIndex] >= '0' && bFormat[curIndex] <= '9')
								{
									num3 *= 10;
									num3 += (int)(bFormat[curIndex] - '0');
									curIndex++;
								}
								if (flag2)
								{
									num3 = -num3;
								}
								if (num3 < num2)
								{
									throw new ArgumentException(Environment.GetResourceString("Argument_BadSigFormat"));
								}
							}
						}
						if (bFormat[curIndex] == ',')
						{
							curIndex++;
							symbolType.SetBounds(num2, num3);
							num2 = 0;
							num3 = -1;
						}
						else if (bFormat[curIndex] != ']')
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_BadSigFormat"));
						}
					}
					symbolType.SetBounds(num2, num3);
					curIndex++;
					symbolType.SetFormat(bFormat, num, curIndex - num);
					symbolType.SetElementType(baseType);
					return SymbolType.FormCompoundType(bFormat, symbolType, curIndex);
				}
				if (bFormat[curIndex] == '*')
				{
					SymbolType symbolType = new SymbolType(TypeKind.IsPointer);
					symbolType.SetFormat(bFormat, curIndex, 1);
					curIndex++;
					symbolType.SetElementType(baseType);
					return SymbolType.FormCompoundType(bFormat, symbolType, curIndex);
				}
				return null;
			}
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x00104226 File Offset: 0x00103226
		internal SymbolType(TypeKind typeKind)
		{
			this.m_typeKind = typeKind;
			this.m_iaLowerBound = new int[4];
			this.m_iaUpperBound = new int[4];
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x00104254 File Offset: 0x00103254
		internal void SetElementType(Type baseType)
		{
			if (baseType == null)
			{
				throw new ArgumentNullException("baseType");
			}
			this.m_baseType = baseType;
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x0010426C File Offset: 0x0010326C
		internal void SetBounds(int lower, int upper)
		{
			if (lower != 0 || upper != -1)
			{
				this.m_isSzArray = false;
			}
			if (this.m_iaLowerBound.Length <= this.m_cRank)
			{
				int[] array = new int[this.m_cRank * 2];
				Array.Copy(this.m_iaLowerBound, array, this.m_cRank);
				this.m_iaLowerBound = array;
				Array.Copy(this.m_iaUpperBound, array, this.m_cRank);
				this.m_iaUpperBound = array;
			}
			this.m_iaLowerBound[this.m_cRank] = lower;
			this.m_iaUpperBound[this.m_cRank] = upper;
			this.m_cRank++;
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x00104304 File Offset: 0x00103304
		internal void SetFormat(char[] bFormat, int curIndex, int length)
		{
			char[] array = new char[length];
			Array.Copy(bFormat, curIndex, array, 0, length);
			this.m_bFormat = array;
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06004AB3 RID: 19123 RVA: 0x00104329 File Offset: 0x00103329
		internal override bool IsSzArray
		{
			get
			{
				return this.m_cRank <= 1 && this.m_isSzArray;
			}
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x0010433C File Offset: 0x0010333C
		public override Type MakePointerType()
		{
			return SymbolType.FormCompoundType((new string(this.m_bFormat) + "*").ToCharArray(), this.m_baseType, 0);
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x00104364 File Offset: 0x00103364
		public override Type MakeByRefType()
		{
			return SymbolType.FormCompoundType((new string(this.m_bFormat) + "&").ToCharArray(), this.m_baseType, 0);
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x0010438C File Offset: 0x0010338C
		public override Type MakeArrayType()
		{
			return SymbolType.FormCompoundType((new string(this.m_bFormat) + "[]").ToCharArray(), this.m_baseType, 0);
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x001043B4 File Offset: 0x001033B4
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
			return SymbolType.FormCompoundType((new string(this.m_bFormat) + text2).ToCharArray(), this.m_baseType, 0) as SymbolType;
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x0010443D File Offset: 0x0010343D
		public override int GetArrayRank()
		{
			if (!base.IsArray)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
			}
			return this.m_cRank;
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06004AB9 RID: 19129 RVA: 0x0010445D File Offset: 0x0010345D
		internal override int MetadataTokenInternal
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06004ABA RID: 19130 RVA: 0x0010446E File Offset: 0x0010346E
		public override Guid GUID
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
			}
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x0010447F File Offset: 0x0010347F
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06004ABC RID: 19132 RVA: 0x00104490 File Offset: 0x00103490
		public override Module Module
		{
			get
			{
				Type type = this.m_baseType;
				while (type is SymbolType)
				{
					type = ((SymbolType)type).m_baseType;
				}
				return type.Module;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06004ABD RID: 19133 RVA: 0x001044C0 File Offset: 0x001034C0
		public override Assembly Assembly
		{
			get
			{
				Type type = this.m_baseType;
				while (type is SymbolType)
				{
					type = ((SymbolType)type).m_baseType;
				}
				return type.Assembly;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06004ABE RID: 19134 RVA: 0x001044F0 File Offset: 0x001034F0
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06004ABF RID: 19135 RVA: 0x00104504 File Offset: 0x00103504
		public override string Name
		{
			get
			{
				string text = new string(this.m_bFormat);
				Type type = this.m_baseType;
				while (type is SymbolType)
				{
					text = new string(((SymbolType)type).m_bFormat) + text;
					type = ((SymbolType)type).m_baseType;
				}
				return type.Name + text;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06004AC0 RID: 19136 RVA: 0x0010455D File Offset: 0x0010355D
		public override string FullName
		{
			get
			{
				return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.FullName);
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06004AC1 RID: 19137 RVA: 0x00104566 File Offset: 0x00103566
		public override string AssemblyQualifiedName
		{
			get
			{
				return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.AssemblyQualifiedName);
			}
		}

		// Token: 0x06004AC2 RID: 19138 RVA: 0x0010456F File Offset: 0x0010356F
		public override string ToString()
		{
			return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.ToString);
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06004AC3 RID: 19139 RVA: 0x00104578 File Offset: 0x00103578
		public override string Namespace
		{
			get
			{
				return this.m_baseType.Namespace;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06004AC4 RID: 19140 RVA: 0x00104585 File Offset: 0x00103585
		public override Type BaseType
		{
			get
			{
				return typeof(Array);
			}
		}

		// Token: 0x06004AC5 RID: 19141 RVA: 0x00104591 File Offset: 0x00103591
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AC6 RID: 19142 RVA: 0x001045A2 File Offset: 0x001035A2
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AC7 RID: 19143 RVA: 0x001045B3 File Offset: 0x001035B3
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AC8 RID: 19144 RVA: 0x001045C4 File Offset: 0x001035C4
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AC9 RID: 19145 RVA: 0x001045D5 File Offset: 0x001035D5
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004ACA RID: 19146 RVA: 0x001045E6 File Offset: 0x001035E6
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004ACB RID: 19147 RVA: 0x001045F7 File Offset: 0x001035F7
		public override Type GetInterface(string name, bool ignoreCase)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004ACC RID: 19148 RVA: 0x00104608 File Offset: 0x00103608
		public override Type[] GetInterfaces()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004ACD RID: 19149 RVA: 0x00104619 File Offset: 0x00103619
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x0010462A File Offset: 0x0010362A
		public override EventInfo[] GetEvents()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004ACF RID: 19151 RVA: 0x0010463B File Offset: 0x0010363B
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x0010464C File Offset: 0x0010364C
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AD1 RID: 19153 RVA: 0x0010465D File Offset: 0x0010365D
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AD2 RID: 19154 RVA: 0x0010466E File Offset: 0x0010366E
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x0010467F File Offset: 0x0010367F
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x00104690 File Offset: 0x00103690
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x001046A1 File Offset: 0x001036A1
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x001046B2 File Offset: 0x001036B2
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x001046C4 File Offset: 0x001036C4
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			Type type = this.m_baseType;
			while (type is SymbolType)
			{
				type = ((SymbolType)type).m_baseType;
			}
			return type.Attributes;
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x001046F4 File Offset: 0x001036F4
		protected override bool IsArrayImpl()
		{
			return this.m_typeKind == TypeKind.IsArray;
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x001046FF File Offset: 0x001036FF
		protected override bool IsPointerImpl()
		{
			return this.m_typeKind == TypeKind.IsPointer;
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x0010470A File Offset: 0x0010370A
		protected override bool IsByRefImpl()
		{
			return this.m_typeKind == TypeKind.IsByRef;
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x00104715 File Offset: 0x00103715
		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x00104718 File Offset: 0x00103718
		protected override bool IsValueTypeImpl()
		{
			return false;
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x0010471B File Offset: 0x0010371B
		protected override bool IsCOMObjectImpl()
		{
			return false;
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x0010471E File Offset: 0x0010371E
		public override Type GetElementType()
		{
			return this.m_baseType;
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x00104726 File Offset: 0x00103726
		protected override bool HasElementTypeImpl()
		{
			return this.m_baseType != null;
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06004AE0 RID: 19168 RVA: 0x00104734 File Offset: 0x00103734
		public override Type UnderlyingSystemType
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06004AE1 RID: 19169 RVA: 0x00104737 File Offset: 0x00103737
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x00104748 File Offset: 0x00103748
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x00104759 File Offset: 0x00103759
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonReflectedType"));
		}

		// Token: 0x0400260C RID: 9740
		internal TypeKind m_typeKind;

		// Token: 0x0400260D RID: 9741
		internal Type m_baseType;

		// Token: 0x0400260E RID: 9742
		internal int m_cRank;

		// Token: 0x0400260F RID: 9743
		internal int[] m_iaLowerBound;

		// Token: 0x04002610 RID: 9744
		internal int[] m_iaUpperBound;

		// Token: 0x04002611 RID: 9745
		private char[] m_bFormat;

		// Token: 0x04002612 RID: 9746
		private bool m_isSzArray = true;
	}
}

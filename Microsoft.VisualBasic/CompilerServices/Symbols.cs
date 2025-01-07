using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting;

namespace Microsoft.VisualBasic.CompilerServices
{
	internal class Symbols
	{
		private Symbols()
		{
		}

		static Symbols()
		{
			Symbols.OperatorCLSNames[1] = "op_Explicit";
			Symbols.OperatorCLSNames[2] = "op_Implicit";
			Symbols.OperatorCLSNames[3] = "op_True";
			Symbols.OperatorCLSNames[4] = "op_False";
			Symbols.OperatorCLSNames[5] = "op_UnaryNegation";
			Symbols.OperatorCLSNames[6] = "op_OnesComplement";
			Symbols.OperatorCLSNames[7] = "op_UnaryPlus";
			Symbols.OperatorCLSNames[8] = "op_Addition";
			Symbols.OperatorCLSNames[9] = "op_Subtraction";
			Symbols.OperatorCLSNames[10] = "op_Multiply";
			Symbols.OperatorCLSNames[11] = "op_Division";
			Symbols.OperatorCLSNames[12] = "op_Exponent";
			Symbols.OperatorCLSNames[13] = "op_IntegerDivision";
			Symbols.OperatorCLSNames[14] = "op_Concatenate";
			Symbols.OperatorCLSNames[15] = "op_LeftShift";
			Symbols.OperatorCLSNames[16] = "op_RightShift";
			Symbols.OperatorCLSNames[17] = "op_Modulus";
			Symbols.OperatorCLSNames[18] = "op_BitwiseOr";
			Symbols.OperatorCLSNames[19] = "op_ExclusiveOr";
			Symbols.OperatorCLSNames[20] = "op_BitwiseAnd";
			Symbols.OperatorCLSNames[21] = "op_Like";
			Symbols.OperatorCLSNames[22] = "op_Equality";
			Symbols.OperatorCLSNames[23] = "op_Inequality";
			Symbols.OperatorCLSNames[24] = "op_LessThan";
			Symbols.OperatorCLSNames[25] = "op_LessThanOrEqual";
			Symbols.OperatorCLSNames[26] = "op_GreaterThanOrEqual";
			Symbols.OperatorCLSNames[27] = "op_GreaterThan";
			Symbols.OperatorNames = new string[28];
			Symbols.OperatorNames[1] = "CType";
			Symbols.OperatorNames[2] = "CType";
			Symbols.OperatorNames[3] = "IsTrue";
			Symbols.OperatorNames[4] = "IsFalse";
			Symbols.OperatorNames[5] = "-";
			Symbols.OperatorNames[6] = "Not";
			Symbols.OperatorNames[7] = "+";
			Symbols.OperatorNames[8] = "+";
			Symbols.OperatorNames[9] = "-";
			Symbols.OperatorNames[10] = "*";
			Symbols.OperatorNames[11] = "/";
			Symbols.OperatorNames[12] = "^";
			Symbols.OperatorNames[13] = "\\";
			Symbols.OperatorNames[14] = "&";
			Symbols.OperatorNames[15] = "<<";
			Symbols.OperatorNames[16] = ">>";
			Symbols.OperatorNames[17] = "Mod";
			Symbols.OperatorNames[18] = "Or";
			Symbols.OperatorNames[19] = "Xor";
			Symbols.OperatorNames[20] = "And";
			Symbols.OperatorNames[21] = "Like";
			Symbols.OperatorNames[22] = "=";
			Symbols.OperatorNames[23] = "<>";
			Symbols.OperatorNames[24] = "<";
			Symbols.OperatorNames[25] = "<=";
			Symbols.OperatorNames[26] = ">=";
			Symbols.OperatorNames[27] = ">";
		}

		internal static bool IsUnaryOperator(Symbols.UserDefinedOperator Op)
		{
			switch (Op)
			{
			case Symbols.UserDefinedOperator.Narrow:
			case Symbols.UserDefinedOperator.Widen:
			case Symbols.UserDefinedOperator.IsTrue:
			case Symbols.UserDefinedOperator.IsFalse:
			case Symbols.UserDefinedOperator.Negate:
			case Symbols.UserDefinedOperator.Not:
			case Symbols.UserDefinedOperator.UnaryPlus:
				return true;
			default:
				return false;
			}
		}

		internal static bool IsBinaryOperator(Symbols.UserDefinedOperator Op)
		{
			switch (Op)
			{
			case Symbols.UserDefinedOperator.Plus:
			case Symbols.UserDefinedOperator.Minus:
			case Symbols.UserDefinedOperator.Multiply:
			case Symbols.UserDefinedOperator.Divide:
			case Symbols.UserDefinedOperator.Power:
			case Symbols.UserDefinedOperator.IntegralDivide:
			case Symbols.UserDefinedOperator.Concatenate:
			case Symbols.UserDefinedOperator.ShiftLeft:
			case Symbols.UserDefinedOperator.ShiftRight:
			case Symbols.UserDefinedOperator.Modulus:
			case Symbols.UserDefinedOperator.Or:
			case Symbols.UserDefinedOperator.Xor:
			case Symbols.UserDefinedOperator.And:
			case Symbols.UserDefinedOperator.Like:
			case Symbols.UserDefinedOperator.Equal:
			case Symbols.UserDefinedOperator.NotEqual:
			case Symbols.UserDefinedOperator.Less:
			case Symbols.UserDefinedOperator.LessEqual:
			case Symbols.UserDefinedOperator.GreaterEqual:
			case Symbols.UserDefinedOperator.Greater:
				return true;
			default:
				return false;
			}
		}

		internal static bool IsUserDefinedOperator(MethodBase Method)
		{
			return Method.IsSpecialName && Method.Name.StartsWith("op_", StringComparison.Ordinal);
		}

		internal static bool IsNarrowingConversionOperator(MethodBase Method)
		{
			return Method.IsSpecialName && Method.Name.Equals(Symbols.OperatorCLSNames[1]);
		}

		internal static Symbols.UserDefinedOperator MapToUserDefinedOperator(MethodBase Method)
		{
			int num = 1;
			checked
			{
				Symbols.UserDefinedOperator userDefinedOperator;
				for (;;)
				{
					if (Method.Name.Equals(Symbols.OperatorCLSNames[num]))
					{
						int num2 = Method.GetParameters().Length;
						userDefinedOperator = (Symbols.UserDefinedOperator)num;
						if (num2 == 1 && Symbols.IsUnaryOperator(userDefinedOperator))
						{
							break;
						}
						if (num2 == 2 && Symbols.IsBinaryOperator(userDefinedOperator))
						{
							break;
						}
					}
					num++;
					if (num > 27)
					{
						return Symbols.UserDefinedOperator.UNDEF;
					}
				}
				return userDefinedOperator;
			}
		}

		internal static TypeCode GetTypeCode(Type Type)
		{
			return Type.GetTypeCode(Type);
		}

		internal static Type MapTypeCodeToType(TypeCode TypeCode)
		{
			switch (TypeCode)
			{
			case TypeCode.Object:
				return typeof(object);
			case TypeCode.DBNull:
				return typeof(DBNull);
			case TypeCode.Boolean:
				return typeof(bool);
			case TypeCode.Char:
				return typeof(char);
			case TypeCode.SByte:
				return typeof(sbyte);
			case TypeCode.Byte:
				return typeof(byte);
			case TypeCode.Int16:
				return typeof(short);
			case TypeCode.UInt16:
				return typeof(ushort);
			case TypeCode.Int32:
				return typeof(int);
			case TypeCode.UInt32:
				return typeof(uint);
			case TypeCode.Int64:
				return typeof(long);
			case TypeCode.UInt64:
				return typeof(ulong);
			case TypeCode.Single:
				return typeof(float);
			case TypeCode.Double:
				return typeof(double);
			case TypeCode.Decimal:
				return typeof(decimal);
			case TypeCode.DateTime:
				return typeof(DateTime);
			case TypeCode.String:
				return typeof(string);
			}
			return null;
		}

		internal static bool IsRootObjectType(Type Type)
		{
			return Type == typeof(object);
		}

		internal static bool IsRootEnumType(Type Type)
		{
			return Type == typeof(Enum);
		}

		internal static bool IsValueType(Type Type)
		{
			return Type.IsValueType;
		}

		internal static bool IsEnum(Type Type)
		{
			return Type.IsEnum;
		}

		internal static bool IsArrayType(Type Type)
		{
			return Type.IsArray;
		}

		internal static bool IsStringType(Type Type)
		{
			return Type == typeof(string);
		}

		internal static bool IsCharArrayRankOne(Type Type)
		{
			return Type == typeof(char[]);
		}

		internal static bool IsIntegralType(TypeCode TypeCode)
		{
			switch (TypeCode)
			{
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
				return true;
			}
			return false;
		}

		internal static bool IsNumericType(TypeCode TypeCode)
		{
			switch (TypeCode)
			{
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return true;
			}
			return false;
		}

		internal static bool IsNumericType(Type Type)
		{
			return Symbols.IsNumericType(Symbols.GetTypeCode(Type));
		}

		internal static bool IsIntrinsicType(TypeCode TypeCode)
		{
			switch (TypeCode)
			{
			case TypeCode.Boolean:
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
			case TypeCode.DateTime:
			case TypeCode.String:
				return true;
			}
			return false;
		}

		internal static bool IsIntrinsicType(Type Type)
		{
			return Symbols.IsIntrinsicType(Symbols.GetTypeCode(Type)) && !Symbols.IsEnum(Type);
		}

		internal static bool IsClass(Type Type)
		{
			return Type.IsClass || Symbols.IsRootEnumType(Type);
		}

		internal static bool IsClassOrValueType(Type Type)
		{
			return Symbols.IsValueType(Type) || Symbols.IsClass(Type);
		}

		internal static bool IsInterface(Type Type)
		{
			return Type.IsInterface;
		}

		internal static bool IsClassOrInterface(Type Type)
		{
			return Symbols.IsClass(Type) || Symbols.IsInterface(Type);
		}

		internal static bool IsReferenceType(Type Type)
		{
			return Symbols.IsClass(Type) || Symbols.IsInterface(Type);
		}

		internal static bool IsGenericParameter(Type Type)
		{
			return Type.IsGenericParameter;
		}

		internal static bool Implements(Type Implementor, Type Interface)
		{
			foreach (Type type in Implementor.GetInterfaces())
			{
				if (type == Interface)
				{
					return true;
				}
			}
			return false;
		}

		internal static bool IsOrInheritsFrom(Type Derived, Type Base)
		{
			if (Derived == Base)
			{
				return true;
			}
			if (Derived.IsGenericParameter)
			{
				if (Symbols.IsClass(Base) && (Derived.GenericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) > GenericParameterAttributes.None && Symbols.IsOrInheritsFrom(typeof(ValueType), Base))
				{
					return true;
				}
				foreach (Type type in Derived.GetGenericParameterConstraints())
				{
					if (Symbols.IsOrInheritsFrom(type, Base))
					{
						return true;
					}
				}
			}
			else if (Symbols.IsInterface(Derived))
			{
				if (Symbols.IsInterface(Base))
				{
					foreach (Type type2 in Derived.GetInterfaces())
					{
						if (type2 == Base)
						{
							return true;
						}
					}
				}
			}
			else if (Symbols.IsClass(Base) && Symbols.IsClassOrValueType(Derived))
			{
				return Derived.IsSubclassOf(Base);
			}
			return false;
		}

		internal static bool IsGeneric(Type Type)
		{
			return Type.IsGenericType;
		}

		internal static bool IsInstantiatedGeneric(Type Type)
		{
			return Type.IsGenericType && !Type.IsGenericTypeDefinition;
		}

		internal static bool IsGeneric(MethodBase Method)
		{
			return Method.IsGenericMethod;
		}

		internal static bool IsGeneric(MemberInfo Member)
		{
			MethodBase methodBase = Member as MethodBase;
			return methodBase != null && Symbols.IsGeneric(methodBase);
		}

		internal static bool IsRawGeneric(MethodBase Method)
		{
			return Method.IsGenericMethod && Method.IsGenericMethodDefinition;
		}

		internal static Type[] GetTypeParameters(MemberInfo Member)
		{
			MethodBase methodBase = Member as MethodBase;
			if (methodBase == null)
			{
				return Symbols.NoTypeParameters;
			}
			return methodBase.GetGenericArguments();
		}

		internal static Type[] GetTypeParameters(Type Type)
		{
			return Type.GetGenericArguments();
		}

		internal static Type[] GetTypeArguments(Type Type)
		{
			return Type.GetGenericArguments();
		}

		internal static Type[] GetInterfaceConstraints(Type GenericParameter)
		{
			return GenericParameter.GetInterfaces();
		}

		internal static Type GetClassConstraint(Type GenericParameter)
		{
			Type baseType = GenericParameter.BaseType;
			if (Symbols.IsRootObjectType(baseType))
			{
				return null;
			}
			return baseType;
		}

		internal static int IndexIn(Type PossibleGenericParameter, MethodBase GenericMethodDef)
		{
			if (Symbols.IsGenericParameter(PossibleGenericParameter) && PossibleGenericParameter.DeclaringMethod != null && Symbols.AreGenericMethodDefsEqual(PossibleGenericParameter.DeclaringMethod, GenericMethodDef))
			{
				return PossibleGenericParameter.GenericParameterPosition;
			}
			return -1;
		}

		internal static bool RefersToGenericParameter(Type ReferringType, MethodBase Method)
		{
			if (!Symbols.IsRawGeneric(Method))
			{
				return false;
			}
			if (ReferringType.IsByRef)
			{
				ReferringType = Symbols.GetElementType(ReferringType);
			}
			if (Symbols.IsGenericParameter(ReferringType))
			{
				if (Symbols.AreGenericMethodDefsEqual(ReferringType.DeclaringMethod, Method))
				{
					return true;
				}
			}
			else if (Symbols.IsGeneric(ReferringType))
			{
				foreach (Type type in Symbols.GetTypeArguments(ReferringType))
				{
					if (Symbols.RefersToGenericParameter(type, Method))
					{
						return true;
					}
				}
			}
			else if (Symbols.IsArrayType(ReferringType))
			{
				return Symbols.RefersToGenericParameter(ReferringType.GetElementType(), Method);
			}
			return false;
		}

		internal static bool RefersToGenericParameterCLRSemantics(Type ReferringType, Type Typ)
		{
			if (ReferringType.IsByRef)
			{
				ReferringType = Symbols.GetElementType(ReferringType);
			}
			if (Symbols.IsGenericParameter(ReferringType))
			{
				if (ReferringType.DeclaringType == Typ)
				{
					return true;
				}
			}
			else if (Symbols.IsGeneric(ReferringType))
			{
				foreach (Type type in Symbols.GetTypeArguments(ReferringType))
				{
					if (Symbols.RefersToGenericParameterCLRSemantics(type, Typ))
					{
						return true;
					}
				}
			}
			else if (Symbols.IsArrayType(ReferringType))
			{
				return Symbols.RefersToGenericParameterCLRSemantics(ReferringType.GetElementType(), Typ);
			}
			return false;
		}

		internal static bool AreGenericMethodDefsEqual(MethodBase Method1, MethodBase Method2)
		{
			return Method1 == Method2 || Method1.MetadataToken == Method2.MetadataToken;
		}

		internal static bool IsShadows(MethodBase Method)
		{
			return !Method.IsHideBySig && (!Method.IsVirtual || (Method.Attributes & MethodAttributes.VtableLayoutMask) != MethodAttributes.PrivateScope || (((MethodInfo)Method).GetBaseDefinition().Attributes & MethodAttributes.VtableLayoutMask) != MethodAttributes.PrivateScope);
		}

		internal static bool IsShared(MemberInfo Member)
		{
			switch (Member.MemberType)
			{
			case MemberTypes.Constructor:
				return ((ConstructorInfo)Member).IsStatic;
			case MemberTypes.Field:
				return ((FieldInfo)Member).IsStatic;
			case MemberTypes.Method:
				return ((MethodInfo)Member).IsStatic;
			case MemberTypes.Property:
				return ((PropertyInfo)Member).GetGetMethod().IsStatic;
			}
			return false;
		}

		internal static bool IsParamArray(ParameterInfo Parameter)
		{
			return Symbols.IsArrayType(Parameter.ParameterType) && Parameter.IsDefined(typeof(ParamArrayAttribute), false);
		}

		internal static Type GetElementType(Type Type)
		{
			return Type.GetElementType();
		}

		internal static bool AreParametersAndReturnTypesValid(ParameterInfo[] Parameters, Type ReturnType)
		{
			if (ReturnType != null && (ReturnType.IsPointer || ReturnType.IsByRef))
			{
				return false;
			}
			if (Parameters != null)
			{
				foreach (ParameterInfo parameterInfo in Parameters)
				{
					if (parameterInfo.ParameterType.IsPointer)
					{
						return false;
					}
				}
			}
			return true;
		}

		internal static void GetAllParameterCounts(ParameterInfo[] Parameters, ref int RequiredParameterCount, ref int MaximumParameterCount, ref int ParamArrayIndex)
		{
			MaximumParameterCount = Parameters.Length;
			checked
			{
				for (int i = MaximumParameterCount - 1; i >= 0; i += -1)
				{
					if (!Parameters[i].IsOptional)
					{
						RequiredParameterCount = i + 1;
						break;
					}
				}
				if (MaximumParameterCount != 0 && Symbols.IsParamArray(Parameters[MaximumParameterCount - 1]))
				{
					ParamArrayIndex = MaximumParameterCount - 1;
					RequiredParameterCount--;
				}
			}
		}

		internal static bool IsNonPublicRuntimeMember(MemberInfo Member)
		{
			Type declaringType = Member.DeclaringType;
			return !declaringType.IsPublic && declaringType.Assembly == Utils.VBRuntimeAssembly;
		}

		internal static bool HasFlag(BindingFlags Flags, BindingFlags FlagToTest)
		{
			return (Flags & FlagToTest) > BindingFlags.Default;
		}

		internal static readonly object[] NoArguments = new object[0];

		internal static readonly string[] NoArgumentNames = new string[0];

		internal static readonly Type[] NoTypeArguments = new Type[0];

		internal static readonly Type[] NoTypeParameters = new Type[0];

		internal static readonly string[] OperatorCLSNames = new string[28];

		internal static readonly string[] OperatorNames;

		internal enum UserDefinedOperator : sbyte
		{
			UNDEF,
			Narrow,
			Widen,
			IsTrue,
			IsFalse,
			Negate,
			Not,
			UnaryPlus,
			Plus,
			Minus,
			Multiply,
			Divide,
			Power,
			IntegralDivide,
			Concatenate,
			ShiftLeft,
			ShiftRight,
			Modulus,
			Or,
			Xor,
			And,
			Like,
			Equal,
			NotEqual,
			Less,
			LessEqual,
			GreaterEqual,
			Greater,
			MAX
		}

		internal sealed class Container
		{
			internal Container(object Instance)
			{
				if (Instance == null)
				{
					throw ExceptionUtils.VbMakeException(91);
				}
				this.m_Instance = Instance;
				this.m_Type = Instance.GetType();
				this.m_UseCustomReflection = false;
				if (!this.m_Type.IsCOMObject && !RemotingServices.IsTransparentProxy(Instance) && !(Instance is Type))
				{
					this.m_IReflect = Instance as IReflect;
					if (this.m_IReflect != null)
					{
						this.m_UseCustomReflection = true;
					}
				}
				if (!this.m_UseCustomReflection)
				{
					this.m_IReflect = this.m_Type;
				}
				this.CheckForClassExtendingCOMClass();
			}

			internal Container(Type Type)
			{
				if (Type == null)
				{
					throw ExceptionUtils.VbMakeException(91);
				}
				this.m_Instance = null;
				this.m_Type = Type;
				this.m_IReflect = Type;
				this.m_UseCustomReflection = false;
				this.CheckForClassExtendingCOMClass();
			}

			internal bool IsCOMObject
			{
				get
				{
					return this.m_Type.IsCOMObject;
				}
			}

			internal string VBFriendlyName
			{
				get
				{
					return Utils.VBFriendlyName(this.m_Type, this.m_Instance);
				}
			}

			internal bool IsArray
			{
				get
				{
					return Symbols.IsArrayType(this.m_Type) && this.m_Instance != null;
				}
			}

			internal bool IsValueType
			{
				get
				{
					return Symbols.IsValueType(this.m_Type) && this.m_Instance != null;
				}
			}

			private static MemberInfo[] FilterInvalidMembers(MemberInfo[] Members)
			{
				if (Members == null || Members.Length == 0)
				{
					return null;
				}
				int num = 0;
				int num2 = 0;
				checked
				{
					int num3 = Members.Length - 1;
					for (int i = num2; i <= num3; i++)
					{
						ParameterInfo[] array = null;
						Type type = null;
						switch (Members[i].MemberType)
						{
						case MemberTypes.Constructor:
						case MemberTypes.Method:
						{
							MethodInfo methodInfo = (MethodInfo)Members[i];
							array = methodInfo.GetParameters();
							type = methodInfo.ReturnType;
							break;
						}
						case MemberTypes.Field:
							type = ((FieldInfo)Members[i]).FieldType;
							break;
						case MemberTypes.Property:
						{
							PropertyInfo propertyInfo = (PropertyInfo)Members[i];
							MethodInfo getMethod = propertyInfo.GetGetMethod();
							if (getMethod != null)
							{
								array = getMethod.GetParameters();
							}
							else
							{
								MethodInfo setMethod = propertyInfo.GetSetMethod();
								ParameterInfo[] parameters = setMethod.GetParameters();
								array = new ParameterInfo[parameters.Length - 2 + 1];
								Array.Copy(parameters, array, array.Length);
							}
							type = propertyInfo.PropertyType;
							break;
						}
						}
						if (Symbols.AreParametersAndReturnTypesValid(array, type))
						{
							num++;
						}
						else
						{
							Members[i] = null;
						}
					}
					if (num == Members.Length)
					{
						return Members;
					}
					if (num > 0)
					{
						MemberInfo[] array2 = new MemberInfo[num - 1 + 1];
						int num4 = 0;
						int num5 = 0;
						int num6 = Members.Length - 1;
						for (int i = num5; i <= num6; i++)
						{
							if (Members[i] != null)
							{
								array2[num4] = Members[i];
								num4++;
							}
						}
						return array2;
					}
					return null;
				}
			}

			internal MemberInfo[] LookupNamedMembers(string MemberName)
			{
				MemberInfo[] array;
				if (Symbols.IsGenericParameter(this.m_Type))
				{
					Type classConstraint = Symbols.GetClassConstraint(this.m_Type);
					if (classConstraint != null)
					{
						array = classConstraint.GetMember(MemberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
					}
					else
					{
						array = null;
					}
				}
				else
				{
					array = this.m_IReflect.GetMember(MemberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				}
				array = Symbols.Container.FilterInvalidMembers(array);
				if (array == null)
				{
					array = Symbols.Container.NoMembers;
				}
				else if (array.Length > 1)
				{
					Array.Sort(array, Symbols.Container.InheritanceSorter.Instance);
				}
				return array;
			}

			private MemberInfo[] LookupDefaultMembers(ref string DefaultMemberName)
			{
				string text = null;
				Type type = this.m_Type;
				object[] customAttributes;
				for (;;)
				{
					customAttributes = type.GetCustomAttributes(typeof(DefaultMemberAttribute), false);
					if (customAttributes != null && customAttributes.Length > 0)
					{
						break;
					}
					type = type.BaseType;
					if (type == null || Symbols.IsRootObjectType(type))
					{
						goto IL_0046;
					}
				}
				text = ((DefaultMemberAttribute)customAttributes[0]).MemberName;
				IL_0046:
				if (text != null)
				{
					MemberInfo[] array = type.GetMember(text, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
					array = Symbols.Container.FilterInvalidMembers(array);
					if (array != null)
					{
						DefaultMemberName = text;
						if (array.Length > 1)
						{
							Array.Sort(array, Symbols.Container.InheritanceSorter.Instance);
						}
						return array;
					}
				}
				return Symbols.Container.NoMembers;
			}

			internal MemberInfo[] GetMembers(ref string MemberName, bool ReportErrors)
			{
				if (MemberName == null)
				{
					MemberName = "";
				}
				MemberInfo[] array;
				if (Operators.CompareString(MemberName, "", false) == 0)
				{
					if (this.m_UseCustomReflection)
					{
						array = this.LookupNamedMembers(MemberName);
					}
					else
					{
						array = this.LookupDefaultMembers(ref MemberName);
					}
					if (array.Length == 0)
					{
						if (ReportErrors)
						{
							throw new MissingMemberException(Utils.GetResourceString("MissingMember_NoDefaultMemberFound1", new string[] { this.VBFriendlyName }));
						}
						return array;
					}
					else if (this.m_UseCustomReflection)
					{
						MemberName = array[0].Name;
					}
				}
				else
				{
					array = this.LookupNamedMembers(MemberName);
					if (array.Length == 0)
					{
						if (ReportErrors)
						{
							throw new MissingMemberException(Utils.GetResourceString("MissingMember_MemberNotFoundOnType2", new string[] { MemberName, this.VBFriendlyName }));
						}
						return array;
					}
				}
				return array;
			}

			private void CheckForClassExtendingCOMClass()
			{
				if (this.IsCOMObject && Operators.CompareString(this.m_Type.FullName, "System.__ComObject", false) != 0 && Operators.CompareString(this.m_Type.BaseType.FullName, "System.__ComObject", false) != 0)
				{
					throw new InvalidOperationException(Utils.GetResourceString("LateboundCallToInheritedComClass"));
				}
			}

			internal object GetFieldValue(FieldInfo Field)
			{
				if (this.m_Instance == null && !Symbols.IsShared(Field))
				{
					throw new NullReferenceException(Utils.GetResourceString("NullReference_InstanceReqToAccessMember1", new string[] { Utils.FieldToString(Field) }));
				}
				if (Symbols.IsNonPublicRuntimeMember(Field))
				{
					throw new MissingMemberException();
				}
				return Field.GetValue(this.m_Instance);
			}

			internal void SetFieldValue(FieldInfo Field, object Value)
			{
				if (Field.IsInitOnly)
				{
					throw new MissingMemberException(Utils.GetResourceString("MissingMember_ReadOnlyField2", new string[] { Field.Name, this.VBFriendlyName }));
				}
				if (this.m_Instance == null && !Symbols.IsShared(Field))
				{
					throw new NullReferenceException(Utils.GetResourceString("NullReference_InstanceReqToAccessMember1", new string[] { Utils.FieldToString(Field) }));
				}
				if (Symbols.IsNonPublicRuntimeMember(Field))
				{
					throw new MissingMemberException();
				}
				Field.SetValue(this.m_Instance, Conversions.ChangeType(Value, Field.FieldType));
			}

			internal object GetArrayValue(object[] Indices)
			{
				Array array = (Array)this.m_Instance;
				int rank = array.Rank;
				if (Indices.Length != rank)
				{
					throw new RankException();
				}
				int num = (int)Conversions.ChangeType(Indices[0], typeof(int));
				if (rank == 1)
				{
					return array.GetValue(num);
				}
				int num2 = (int)Conversions.ChangeType(Indices[1], typeof(int));
				if (rank == 2)
				{
					return array.GetValue(num, num2);
				}
				int num3 = (int)Conversions.ChangeType(Indices[2], typeof(int));
				if (rank == 3)
				{
					return array.GetValue(num, num2, num3);
				}
				checked
				{
					int[] array2 = new int[rank - 1 + 1];
					array2[0] = num;
					array2[1] = num2;
					array2[2] = num3;
					int num4 = 3;
					int num5 = rank - 1;
					for (int i = num4; i <= num5; i++)
					{
						array2[i] = (int)Conversions.ChangeType(Indices[i], typeof(int));
					}
					return array.GetValue(array2);
				}
			}

			internal void SetArrayValue(object[] Arguments)
			{
				Array array = (Array)this.m_Instance;
				int rank = array.Rank;
				checked
				{
					if (Arguments.Length - 1 != rank)
					{
						throw new RankException();
					}
					object obj = Arguments[Arguments.Length - 1];
					Type elementType = this.m_Type.GetElementType();
					int num = (int)Conversions.ChangeType(Arguments[0], typeof(int));
					if (rank == 1)
					{
						array.SetValue(Conversions.ChangeType(obj, elementType), num);
						return;
					}
					int num2 = (int)Conversions.ChangeType(Arguments[1], typeof(int));
					if (rank == 2)
					{
						array.SetValue(Conversions.ChangeType(obj, elementType), num, num2);
						return;
					}
					int num3 = (int)Conversions.ChangeType(Arguments[2], typeof(int));
					if (rank == 3)
					{
						array.SetValue(Conversions.ChangeType(obj, elementType), num, num2, num3);
						return;
					}
					int[] array2 = new int[rank - 1 + 1];
					array2[0] = num;
					array2[1] = num2;
					array2[2] = num3;
					int num4 = 3;
					int num5 = rank - 1;
					for (int i = num4; i <= num5; i++)
					{
						array2[i] = (int)Conversions.ChangeType(Arguments[i], typeof(int));
					}
					array.SetValue(Conversions.ChangeType(obj, elementType), array2);
				}
			}

			internal object InvokeMethod(Symbols.Method TargetProcedure, object[] Arguments, bool[] CopyBack, BindingFlags Flags)
			{
				MethodBase callTarget = NewLateBinding.GetCallTarget(TargetProcedure, Flags);
				object[] array = NewLateBinding.ConstructCallArguments(TargetProcedure, Arguments, Flags);
				if (this.m_Instance == null && !Symbols.IsShared(callTarget))
				{
					throw new NullReferenceException(Utils.GetResourceString("NullReference_InstanceReqToAccessMember1", new string[] { TargetProcedure.ToString() }));
				}
				if (Symbols.IsNonPublicRuntimeMember(callTarget))
				{
					throw new MissingMemberException();
				}
				object obj;
				try
				{
					obj = callTarget.Invoke(this.m_Instance, array);
				}
				catch (TargetInvocationException ex) when (ex.InnerException != null)
				{
					throw ex.InnerException;
				}
				OverloadResolution.ReorderArgumentArray(TargetProcedure, array, Arguments, CopyBack, Flags);
				return obj;
			}

			private readonly object m_Instance;

			private readonly Type m_Type;

			private readonly IReflect m_IReflect;

			private readonly bool m_UseCustomReflection;

			private const BindingFlags DefaultLookupFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;

			private static readonly MemberInfo[] NoMembers = new MemberInfo[0];

			private class InheritanceSorter : IComparer
			{
				private InheritanceSorter()
				{
				}

				int IComparer.Compare(object Left, object Right)
				{
					Type declaringType = ((MemberInfo)Left).DeclaringType;
					Type declaringType2 = ((MemberInfo)Right).DeclaringType;
					if (declaringType == declaringType2)
					{
						return 0;
					}
					if (declaringType.IsSubclassOf(declaringType2))
					{
						return -1;
					}
					return 1;
				}

				internal static readonly Symbols.Container.InheritanceSorter Instance = new Symbols.Container.InheritanceSorter();
			}
		}

		internal sealed class Method
		{
			private Method(ParameterInfo[] Parameters, int ParamArrayIndex, bool ParamArrayExpanded)
			{
				this.m_Parameters = Parameters;
				this.m_RawParameters = Parameters;
				this.ParamArrayIndex = ParamArrayIndex;
				this.ParamArrayExpanded = ParamArrayExpanded;
				this.AllNarrowingIsFromObject = true;
			}

			internal Method(MethodBase Method, ParameterInfo[] Parameters, int ParamArrayIndex, bool ParamArrayExpanded)
				: this(Parameters, ParamArrayIndex, ParamArrayExpanded)
			{
				this.m_Item = Method;
				this.m_RawItem = Method;
			}

			internal Method(PropertyInfo Property, ParameterInfo[] Parameters, int ParamArrayIndex, bool ParamArrayExpanded)
				: this(Parameters, ParamArrayIndex, ParamArrayExpanded)
			{
				this.m_Item = Property;
			}

			internal ParameterInfo[] Parameters
			{
				get
				{
					return this.m_Parameters;
				}
			}

			internal ParameterInfo[] RawParameters
			{
				get
				{
					return this.m_RawParameters;
				}
			}

			internal ParameterInfo[] RawParametersFromType
			{
				get
				{
					if (this.m_RawParametersFromType == null)
					{
						if (!this.IsProperty)
						{
							int metadataToken = this.m_Item.MetadataToken;
							Type declaringType = this.m_Item.DeclaringType;
							MethodBase methodBase = declaringType.Module.ResolveMethod(metadataToken, null, null);
							this.m_RawParametersFromType = methodBase.GetParameters();
						}
						else
						{
							this.m_RawParametersFromType = this.m_RawParameters;
						}
					}
					return this.m_RawParametersFromType;
				}
			}

			internal Type DeclaringType
			{
				get
				{
					return this.m_Item.DeclaringType;
				}
			}

			internal Type RawDeclaringType
			{
				get
				{
					if (this.m_RawDeclaringType == null)
					{
						Type declaringType = this.m_Item.DeclaringType;
						int metadataToken = declaringType.MetadataToken;
						this.m_RawDeclaringType = declaringType.Module.ResolveType(metadataToken, null, null);
					}
					return this.m_RawDeclaringType;
				}
			}

			internal bool HasParamArray
			{
				get
				{
					return this.ParamArrayIndex > -1;
				}
			}

			internal bool HasByRefParameter
			{
				get
				{
					foreach (ParameterInfo parameterInfo in this.Parameters)
					{
						if (parameterInfo.ParameterType.IsByRef)
						{
							return true;
						}
					}
					return false;
				}
			}

			internal bool IsProperty
			{
				get
				{
					return this.m_Item.MemberType == MemberTypes.Property;
				}
			}

			internal bool IsMethod
			{
				get
				{
					return this.m_Item.MemberType == MemberTypes.Method || this.m_Item.MemberType == MemberTypes.Constructor;
				}
			}

			internal bool IsGeneric
			{
				get
				{
					return Symbols.IsGeneric(this.m_Item);
				}
			}

			internal Type[] TypeParameters
			{
				get
				{
					return Symbols.GetTypeParameters(this.m_Item);
				}
			}

			internal bool BindGenericArguments()
			{
				bool flag;
				try
				{
					this.m_Item = ((MethodInfo)this.m_RawItem).MakeGenericMethod(this.TypeArguments);
					this.m_Parameters = this.AsMethod().GetParameters();
					flag = true;
				}
				catch (ArgumentException ex)
				{
					flag = false;
				}
				return flag;
			}

			internal MethodBase AsMethod()
			{
				return this.m_Item as MethodBase;
			}

			internal PropertyInfo AsProperty()
			{
				return this.m_Item as PropertyInfo;
			}

			public static bool operator ==(Symbols.Method Left, Symbols.Method Right)
			{
				return Left.m_Item == Right.m_Item;
			}

			public static bool operator !=(Symbols.Method Left, Symbols.Method right)
			{
				return Left.m_Item != right.m_Item;
			}

			public static bool operator ==(MemberInfo Left, Symbols.Method Right)
			{
				return Left == Right.m_Item;
			}

			public static bool operator !=(MemberInfo Left, Symbols.Method Right)
			{
				return Left != Right.m_Item;
			}

			public override string ToString()
			{
				return Utils.MemberToString(this.m_Item);
			}

			private MemberInfo m_Item;

			private MethodBase m_RawItem;

			private ParameterInfo[] m_Parameters;

			private ParameterInfo[] m_RawParameters;

			private ParameterInfo[] m_RawParametersFromType;

			private Type m_RawDeclaringType;

			internal readonly int ParamArrayIndex;

			internal readonly bool ParamArrayExpanded;

			internal bool NotCallable;

			internal bool RequiresNarrowingConversion;

			internal bool AllNarrowingIsFromObject;

			internal bool LessSpecific;

			internal bool ArgumentsValidated;

			internal int[] NamedArgumentMapping;

			internal Type[] TypeArguments;

			internal bool ArgumentMatchingDone;
		}

		internal sealed class TypedNothing
		{
			internal TypedNothing(Type Type)
			{
				this.Type = Type;
			}

			internal readonly Type Type;
		}
	}
}

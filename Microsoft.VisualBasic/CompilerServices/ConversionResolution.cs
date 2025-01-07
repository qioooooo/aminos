using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Microsoft.VisualBasic.CompilerServices
{
	internal class ConversionResolution
	{
		private ConversionResolution()
		{
		}

		static ConversionResolution()
		{
			ConversionResolution.NumericSpecificityRank[6] = 1;
			ConversionResolution.NumericSpecificityRank[5] = 2;
			ConversionResolution.NumericSpecificityRank[7] = 3;
			ConversionResolution.NumericSpecificityRank[8] = 4;
			ConversionResolution.NumericSpecificityRank[9] = 5;
			ConversionResolution.NumericSpecificityRank[10] = 6;
			ConversionResolution.NumericSpecificityRank[11] = 7;
			ConversionResolution.NumericSpecificityRank[12] = 8;
			ConversionResolution.NumericSpecificityRank[15] = 9;
			ConversionResolution.NumericSpecificityRank[13] = 10;
			ConversionResolution.NumericSpecificityRank[14] = 11;
			ConversionResolution.ForLoopWidestTypeCode = new TypeCode[][]
			{
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Int16,
					TypeCode.Empty,
					TypeCode.SByte,
					TypeCode.Int16,
					TypeCode.Int16,
					TypeCode.Int32,
					TypeCode.Int32,
					TypeCode.Int64,
					TypeCode.Int64,
					TypeCode.Decimal,
					TypeCode.Single,
					TypeCode.Double,
					TypeCode.Decimal,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.SByte,
					TypeCode.Empty,
					TypeCode.SByte,
					TypeCode.Int16,
					TypeCode.Int16,
					TypeCode.Int32,
					TypeCode.Int32,
					TypeCode.Int64,
					TypeCode.Int64,
					TypeCode.Decimal,
					TypeCode.Single,
					TypeCode.Double,
					TypeCode.Decimal,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Int16,
					TypeCode.Empty,
					TypeCode.Int16,
					TypeCode.Byte,
					TypeCode.Int16,
					TypeCode.UInt16,
					TypeCode.Int32,
					TypeCode.UInt32,
					TypeCode.Int64,
					TypeCode.UInt64,
					TypeCode.Single,
					TypeCode.Double,
					TypeCode.Decimal,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Int16,
					TypeCode.Empty,
					TypeCode.Int16,
					TypeCode.Int16,
					TypeCode.Int16,
					TypeCode.Int32,
					TypeCode.Int32,
					TypeCode.Int64,
					TypeCode.Int64,
					TypeCode.Decimal,
					TypeCode.Single,
					TypeCode.Double,
					TypeCode.Decimal,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Int32,
					TypeCode.Empty,
					TypeCode.Int32,
					TypeCode.UInt16,
					TypeCode.Int32,
					TypeCode.UInt16,
					TypeCode.Int32,
					TypeCode.UInt32,
					TypeCode.Int64,
					TypeCode.UInt64,
					TypeCode.Single,
					TypeCode.Double,
					TypeCode.Decimal,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Int32,
					TypeCode.Empty,
					TypeCode.Int32,
					TypeCode.Int32,
					TypeCode.Int32,
					TypeCode.Int32,
					TypeCode.Int32,
					TypeCode.Int64,
					TypeCode.Int64,
					TypeCode.Decimal,
					TypeCode.Single,
					TypeCode.Double,
					TypeCode.Decimal,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Int64,
					TypeCode.Empty,
					TypeCode.Int64,
					TypeCode.UInt32,
					TypeCode.Int64,
					TypeCode.UInt32,
					TypeCode.Int64,
					TypeCode.UInt32,
					TypeCode.Int64,
					TypeCode.UInt64,
					TypeCode.Single,
					TypeCode.Double,
					TypeCode.Decimal,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Int64,
					TypeCode.Empty,
					TypeCode.Int64,
					TypeCode.Int64,
					TypeCode.Int64,
					TypeCode.Int64,
					TypeCode.Int64,
					TypeCode.Int64,
					TypeCode.Int64,
					TypeCode.Decimal,
					TypeCode.Single,
					TypeCode.Double,
					TypeCode.Decimal,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Decimal,
					TypeCode.Empty,
					TypeCode.Decimal,
					TypeCode.UInt64,
					TypeCode.Decimal,
					TypeCode.UInt64,
					TypeCode.Decimal,
					TypeCode.UInt64,
					TypeCode.Decimal,
					TypeCode.UInt64,
					TypeCode.Single,
					TypeCode.Double,
					TypeCode.Decimal,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Single,
					TypeCode.Empty,
					TypeCode.Single,
					TypeCode.Single,
					TypeCode.Single,
					TypeCode.Single,
					TypeCode.Single,
					TypeCode.Single,
					TypeCode.Single,
					TypeCode.Single,
					TypeCode.Single,
					TypeCode.Double,
					TypeCode.Single,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Double,
					TypeCode.Empty,
					TypeCode.Double,
					TypeCode.Double,
					TypeCode.Double,
					TypeCode.Double,
					TypeCode.Double,
					TypeCode.Double,
					TypeCode.Double,
					TypeCode.Double,
					TypeCode.Double,
					TypeCode.Double,
					TypeCode.Double,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Decimal,
					TypeCode.Empty,
					TypeCode.Decimal,
					TypeCode.Decimal,
					TypeCode.Decimal,
					TypeCode.Decimal,
					TypeCode.Decimal,
					TypeCode.Decimal,
					TypeCode.Decimal,
					TypeCode.Decimal,
					TypeCode.Single,
					TypeCode.Double,
					TypeCode.Decimal,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				},
				new TypeCode[]
				{
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty,
					TypeCode.Empty
				}
			};
		}

		[Conditional("DEBUG")]
		private static void VerifyTypeCodeEnum()
		{
		}

		internal static ConversionResolution.ConversionClass ClassifyConversion(Type TargetType, Type SourceType, ref Symbols.Method OperatorMethod)
		{
			ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyPredefinedConversion(TargetType, SourceType);
			if (conversionClass == ConversionResolution.ConversionClass.None && !Symbols.IsInterface(SourceType) && !Symbols.IsInterface(TargetType) && (Symbols.IsClassOrValueType(SourceType) || Symbols.IsClassOrValueType(TargetType)) && (!Symbols.IsIntrinsicType(SourceType) || !Symbols.IsIntrinsicType(TargetType)))
			{
				conversionClass = ConversionResolution.ClassifyUserDefinedConversion(TargetType, SourceType, ref OperatorMethod);
			}
			return conversionClass;
		}

		internal static ConversionResolution.ConversionClass ClassifyIntrinsicConversion(TypeCode TargetTypeCode, TypeCode SourceTypeCode)
		{
			return ConversionResolution.ConversionTable[(int)TargetTypeCode][(int)SourceTypeCode];
		}

		internal static ConversionResolution.ConversionClass ClassifyPredefinedCLRConversion(Type TargetType, Type SourceType)
		{
			if (TargetType == SourceType)
			{
				return ConversionResolution.ConversionClass.Identity;
			}
			if (Symbols.IsRootObjectType(TargetType) || Symbols.IsOrInheritsFrom(SourceType, TargetType))
			{
				return ConversionResolution.ConversionClass.Widening;
			}
			if (Symbols.IsRootObjectType(SourceType) || Symbols.IsOrInheritsFrom(TargetType, SourceType))
			{
				return ConversionResolution.ConversionClass.Narrowing;
			}
			if (Symbols.IsInterface(SourceType))
			{
				if (!Symbols.IsClass(TargetType))
				{
					if (!Symbols.IsArrayType(TargetType))
					{
						if (!Symbols.IsGenericParameter(TargetType))
						{
							if (Symbols.IsInterface(TargetType))
							{
								return ConversionResolution.ConversionClass.Narrowing;
							}
							if (!Symbols.IsValueType(TargetType))
							{
								return ConversionResolution.ConversionClass.Narrowing;
							}
							if (Symbols.Implements(TargetType, SourceType))
							{
								return ConversionResolution.ConversionClass.Narrowing;
							}
							return ConversionResolution.ConversionClass.None;
						}
					}
				}
				return ConversionResolution.ConversionClass.Narrowing;
			}
			if (Symbols.IsInterface(TargetType))
			{
				if (Symbols.IsArrayType(SourceType))
				{
					return ConversionResolution.ClassifyCLRArrayToInterfaceConversion(TargetType, SourceType);
				}
				if (Symbols.IsValueType(SourceType))
				{
					if (Symbols.Implements(SourceType, TargetType))
					{
						return ConversionResolution.ConversionClass.Widening;
					}
					return ConversionResolution.ConversionClass.None;
				}
				else if (Symbols.IsClass(SourceType))
				{
					if (Symbols.Implements(SourceType, TargetType))
					{
						return ConversionResolution.ConversionClass.Widening;
					}
					return ConversionResolution.ConversionClass.Narrowing;
				}
			}
			if (Symbols.IsEnum(SourceType) || Symbols.IsEnum(TargetType))
			{
				if (Symbols.GetTypeCode(SourceType) != Symbols.GetTypeCode(TargetType))
				{
					return ConversionResolution.ConversionClass.None;
				}
				if (Symbols.IsEnum(TargetType))
				{
					return ConversionResolution.ConversionClass.Narrowing;
				}
				return ConversionResolution.ConversionClass.Widening;
			}
			else if (Symbols.IsGenericParameter(SourceType))
			{
				if (!Symbols.IsClassOrInterface(TargetType))
				{
					return ConversionResolution.ConversionClass.None;
				}
				foreach (Type type in Symbols.GetInterfaceConstraints(SourceType))
				{
					ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyPredefinedConversion(TargetType, type);
					if (conversionClass == ConversionResolution.ConversionClass.Widening || conversionClass == ConversionResolution.ConversionClass.Identity)
					{
						return ConversionResolution.ConversionClass.Widening;
					}
				}
				Type classConstraint = Symbols.GetClassConstraint(SourceType);
				if (classConstraint != null)
				{
					ConversionResolution.ConversionClass conversionClass2 = ConversionResolution.ClassifyPredefinedConversion(TargetType, classConstraint);
					if (conversionClass2 == ConversionResolution.ConversionClass.Widening || conversionClass2 == ConversionResolution.ConversionClass.Identity)
					{
						return ConversionResolution.ConversionClass.Widening;
					}
				}
				return Interaction.IIf<ConversionResolution.ConversionClass>(Symbols.IsInterface(TargetType), ConversionResolution.ConversionClass.Narrowing, ConversionResolution.ConversionClass.None);
			}
			else if (Symbols.IsGenericParameter(TargetType))
			{
				Type classConstraint2 = Symbols.GetClassConstraint(TargetType);
				if (classConstraint2 != null && Symbols.IsOrInheritsFrom(classConstraint2, SourceType))
				{
					return ConversionResolution.ConversionClass.Narrowing;
				}
				return ConversionResolution.ConversionClass.None;
			}
			else
			{
				if (!Symbols.IsArrayType(SourceType) || !Symbols.IsArrayType(TargetType))
				{
					return ConversionResolution.ConversionClass.None;
				}
				if (SourceType.GetArrayRank() == TargetType.GetArrayRank())
				{
					return ConversionResolution.ClassifyCLRConversionForArrayElementTypes(TargetType.GetElementType(), SourceType.GetElementType());
				}
				return ConversionResolution.ConversionClass.None;
			}
		}

		private static ConversionResolution.ConversionClass ClassifyCLRArrayToInterfaceConversion(Type TargetInterface, Type SourceArrayType)
		{
			if (Symbols.Implements(SourceArrayType, TargetInterface))
			{
				return ConversionResolution.ConversionClass.Widening;
			}
			if (SourceArrayType.GetArrayRank() > 1)
			{
				return ConversionResolution.ConversionClass.Narrowing;
			}
			Type elementType = SourceArrayType.GetElementType();
			ConversionResolution.ConversionClass conversionClass = ConversionResolution.ConversionClass.None;
			if (TargetInterface.IsGenericType && !TargetInterface.IsGenericTypeDefinition)
			{
				Type genericTypeDefinition = TargetInterface.GetGenericTypeDefinition();
				if (genericTypeDefinition != typeof(IList<>))
				{
					if (genericTypeDefinition != typeof(ICollection<>))
					{
						if (genericTypeDefinition != typeof(IEnumerable<>))
						{
							goto IL_0095;
						}
					}
				}
				conversionClass = ConversionResolution.ClassifyCLRConversionForArrayElementTypes(TargetInterface.GetGenericArguments()[0], elementType);
			}
			else
			{
				conversionClass = ConversionResolution.ClassifyPredefinedCLRConversion(TargetInterface, typeof(IList<>).MakeGenericType(new Type[] { elementType }));
			}
			IL_0095:
			if (conversionClass == ConversionResolution.ConversionClass.Identity || conversionClass == ConversionResolution.ConversionClass.Widening)
			{
				return ConversionResolution.ConversionClass.Widening;
			}
			return ConversionResolution.ConversionClass.Narrowing;
		}

		private static ConversionResolution.ConversionClass ClassifyCLRConversionForArrayElementTypes(Type TargetElementType, Type SourceElementType)
		{
			if (Symbols.IsReferenceType(SourceElementType) && Symbols.IsReferenceType(TargetElementType))
			{
				return ConversionResolution.ClassifyPredefinedCLRConversion(TargetElementType, SourceElementType);
			}
			if (Symbols.IsValueType(SourceElementType) && Symbols.IsValueType(TargetElementType))
			{
				return ConversionResolution.ClassifyPredefinedCLRConversion(TargetElementType, SourceElementType);
			}
			if (Symbols.IsGenericParameter(SourceElementType) && Symbols.IsGenericParameter(TargetElementType))
			{
				if (SourceElementType == TargetElementType)
				{
					return ConversionResolution.ConversionClass.Identity;
				}
				if (Symbols.IsReferenceType(SourceElementType) && Symbols.IsOrInheritsFrom(SourceElementType, TargetElementType))
				{
					return ConversionResolution.ConversionClass.Widening;
				}
				if (Symbols.IsReferenceType(TargetElementType) && Symbols.IsOrInheritsFrom(TargetElementType, SourceElementType))
				{
					return ConversionResolution.ConversionClass.Narrowing;
				}
			}
			return ConversionResolution.ConversionClass.None;
		}

		internal static ConversionResolution.ConversionClass ClassifyPredefinedConversion(Type TargetType, Type SourceType)
		{
			if (TargetType == SourceType)
			{
				return ConversionResolution.ConversionClass.Identity;
			}
			TypeCode typeCode = Symbols.GetTypeCode(SourceType);
			TypeCode typeCode2 = Symbols.GetTypeCode(TargetType);
			if (Symbols.IsIntrinsicType(typeCode) && Symbols.IsIntrinsicType(typeCode2))
			{
				if (Symbols.IsEnum(TargetType) && Symbols.IsIntegralType(typeCode) && Symbols.IsIntegralType(typeCode2))
				{
					return ConversionResolution.ConversionClass.Narrowing;
				}
				if (typeCode == typeCode2 && Symbols.IsEnum(SourceType))
				{
					return ConversionResolution.ConversionClass.Widening;
				}
				return ConversionResolution.ClassifyIntrinsicConversion(typeCode2, typeCode);
			}
			else
			{
				if (Symbols.IsCharArrayRankOne(SourceType) && Symbols.IsStringType(TargetType))
				{
					return ConversionResolution.ConversionClass.Widening;
				}
				if (Symbols.IsCharArrayRankOne(TargetType) && Symbols.IsStringType(SourceType))
				{
					return ConversionResolution.ConversionClass.Narrowing;
				}
				return ConversionResolution.ClassifyPredefinedCLRConversion(TargetType, SourceType);
			}
		}

		private static List<Symbols.Method> CollectConversionOperators(Type TargetType, Type SourceType, ref bool FoundTargetTypeOperators, ref bool FoundSourceTypeOperators)
		{
			if (Symbols.IsIntrinsicType(TargetType))
			{
				TargetType = typeof(object);
			}
			if (Symbols.IsIntrinsicType(SourceType))
			{
				SourceType = typeof(object);
			}
			List<Symbols.Method> list = Operators.CollectOperators(Symbols.UserDefinedOperator.Widen, TargetType, SourceType, ref FoundTargetTypeOperators, ref FoundSourceTypeOperators);
			List<Symbols.Method> list2 = Operators.CollectOperators(Symbols.UserDefinedOperator.Narrow, TargetType, SourceType, ref FoundTargetTypeOperators, ref FoundSourceTypeOperators);
			list.AddRange(list2);
			return list;
		}

		private static bool Encompasses(Type Larger, Type Smaller)
		{
			ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyPredefinedConversion(Larger, Smaller);
			return conversionClass == ConversionResolution.ConversionClass.Widening || conversionClass == ConversionResolution.ConversionClass.Identity;
		}

		private static bool NotEncompasses(Type Larger, Type Smaller)
		{
			ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyPredefinedConversion(Larger, Smaller);
			return conversionClass == ConversionResolution.ConversionClass.Narrowing || conversionClass == ConversionResolution.ConversionClass.Identity;
		}

		private static Type MostEncompassing(List<Type> Types)
		{
			Type type = Types[0];
			int num = 1;
			checked
			{
				int num2 = Types.Count - 1;
				for (int i = num; i <= num2; i++)
				{
					Type type2 = Types[i];
					if (ConversionResolution.Encompasses(type2, type))
					{
						type = type2;
					}
					else if (!ConversionResolution.Encompasses(type, type2))
					{
						return null;
					}
				}
				return type;
			}
		}

		private static Type MostEncompassed(List<Type> Types)
		{
			Type type = Types[0];
			int num = 1;
			checked
			{
				int num2 = Types.Count - 1;
				for (int i = num; i <= num2; i++)
				{
					Type type2 = Types[i];
					if (ConversionResolution.Encompasses(type, type2))
					{
						type = type2;
					}
					else if (!ConversionResolution.Encompasses(type2, type))
					{
						return null;
					}
				}
				return type;
			}
		}

		private static void FindBestMatch(Type TargetType, Type SourceType, List<Symbols.Method> SearchList, List<Symbols.Method> ResultList, ref bool GenericMembersExistInList)
		{
			try
			{
				foreach (Symbols.Method method in SearchList)
				{
					MethodBase methodBase = method.AsMethod();
					Type parameterType = methodBase.GetParameters()[0].ParameterType;
					Type returnType = ((MethodInfo)methodBase).ReturnType;
					if (parameterType == SourceType && returnType == TargetType)
					{
						ConversionResolution.InsertInOperatorListIfLessGenericThanExisting(method, ResultList, ref GenericMembersExistInList);
					}
				}
			}
			finally
			{
				List<Symbols.Method>.Enumerator enumerator;
				((IDisposable)enumerator).Dispose();
			}
		}

		private static void InsertInOperatorListIfLessGenericThanExisting(Symbols.Method OperatorToInsert, List<Symbols.Method> OperatorList, ref bool GenericMembersExistInList)
		{
			if (Symbols.IsGeneric(OperatorToInsert.DeclaringType))
			{
				GenericMembersExistInList = true;
			}
			checked
			{
				if (GenericMembersExistInList)
				{
					for (int i = OperatorList.Count - 1; i >= 0; i += -1)
					{
						Symbols.Method method = OperatorList[i];
						Symbols.Method method2 = OverloadResolution.LeastGenericProcedure(method, OperatorToInsert);
						if (method2 == method)
						{
							return;
						}
						if (method2 != null)
						{
							OperatorList.Remove(method);
						}
					}
				}
				OperatorList.Add(OperatorToInsert);
			}
		}

		private static List<Symbols.Method> ResolveConversion(Type TargetType, Type SourceType, List<Symbols.Method> OperatorSet, bool WideningOnly, ref bool ResolutionIsAmbiguous)
		{
			ResolutionIsAmbiguous = false;
			Type type = null;
			Type type2 = null;
			bool flag = false;
			List<Symbols.Method> list = new List<Symbols.Method>(OperatorSet.Count);
			List<Symbols.Method> list2 = new List<Symbols.Method>(OperatorSet.Count);
			List<Type> list3 = new List<Type>(OperatorSet.Count);
			List<Type> list4 = new List<Type>(OperatorSet.Count);
			List<Type> list5 = null;
			List<Type> list6 = null;
			if (!WideningOnly)
			{
				list5 = new List<Type>(OperatorSet.Count);
				list6 = new List<Type>(OperatorSet.Count);
			}
			try
			{
				foreach (Symbols.Method method in OperatorSet)
				{
					MethodBase methodBase = method.AsMethod();
					if (WideningOnly && Symbols.IsNarrowingConversionOperator(methodBase))
					{
						break;
					}
					Type parameterType = methodBase.GetParameters()[0].ParameterType;
					Type returnType = ((MethodInfo)methodBase).ReturnType;
					if ((!Symbols.IsGeneric(methodBase) && !Symbols.IsGeneric(methodBase.DeclaringType)) || ConversionResolution.ClassifyPredefinedConversion(returnType, parameterType) == ConversionResolution.ConversionClass.None)
					{
						if (parameterType == SourceType && returnType == TargetType)
						{
							ConversionResolution.InsertInOperatorListIfLessGenericThanExisting(method, list, ref flag);
						}
						else if (list.Count == 0)
						{
							if (ConversionResolution.Encompasses(parameterType, SourceType) && ConversionResolution.Encompasses(TargetType, returnType))
							{
								list2.Add(method);
								if (parameterType == SourceType)
								{
									type = parameterType;
								}
								else
								{
									list3.Add(parameterType);
								}
								if (returnType == TargetType)
								{
									type2 = returnType;
								}
								else
								{
									list4.Add(returnType);
								}
							}
							else if (!WideningOnly && ConversionResolution.Encompasses(parameterType, SourceType) && ConversionResolution.NotEncompasses(TargetType, returnType))
							{
								list2.Add(method);
								if (parameterType == SourceType)
								{
									type = parameterType;
								}
								else
								{
									list3.Add(parameterType);
								}
								if (returnType == TargetType)
								{
									type2 = returnType;
								}
								else
								{
									list6.Add(returnType);
								}
							}
							else if (!WideningOnly && ConversionResolution.NotEncompasses(parameterType, SourceType) && ConversionResolution.NotEncompasses(TargetType, returnType))
							{
								list2.Add(method);
								if (parameterType == SourceType)
								{
									type = parameterType;
								}
								else
								{
									list5.Add(parameterType);
								}
								if (returnType == TargetType)
								{
									type2 = returnType;
								}
								else
								{
									list6.Add(returnType);
								}
							}
						}
					}
				}
			}
			finally
			{
				List<Symbols.Method>.Enumerator enumerator;
				((IDisposable)enumerator).Dispose();
			}
			if (list.Count == 0 && list2.Count > 0)
			{
				if (type == null)
				{
					if (list3.Count > 0)
					{
						type = ConversionResolution.MostEncompassed(list3);
					}
					else
					{
						type = ConversionResolution.MostEncompassing(list5);
					}
				}
				if (type2 == null)
				{
					if (list4.Count > 0)
					{
						type2 = ConversionResolution.MostEncompassing(list4);
					}
					else
					{
						type2 = ConversionResolution.MostEncompassed(list6);
					}
				}
				if (type == null || type2 == null)
				{
					ResolutionIsAmbiguous = true;
					return new List<Symbols.Method>();
				}
				ConversionResolution.FindBestMatch(type2, type, list2, list, ref flag);
			}
			if (list.Count > 1)
			{
				ResolutionIsAmbiguous = true;
			}
			return list;
		}

		internal static ConversionResolution.ConversionClass ClassifyUserDefinedConversion(Type TargetType, Type SourceType, ref Symbols.Method OperatorMethod)
		{
			OperatorCaches.FixedList conversionCache = OperatorCaches.ConversionCache;
			ConversionResolution.ConversionClass conversionClass;
			lock (conversionCache)
			{
				if (OperatorCaches.UnconvertibleTypeCache.Lookup(TargetType) && OperatorCaches.UnconvertibleTypeCache.Lookup(SourceType))
				{
					return ConversionResolution.ConversionClass.None;
				}
				if (OperatorCaches.ConversionCache.Lookup(TargetType, SourceType, ref conversionClass, ref OperatorMethod))
				{
					return conversionClass;
				}
			}
			bool flag = false;
			bool flag2 = false;
			conversionClass = ConversionResolution.DoClassifyUserDefinedConversion(TargetType, SourceType, ref OperatorMethod, ref flag, ref flag2);
			OperatorCaches.FixedList conversionCache2 = OperatorCaches.ConversionCache;
			lock (conversionCache2)
			{
				if (!flag)
				{
					OperatorCaches.UnconvertibleTypeCache.Insert(TargetType);
				}
				if (!flag2)
				{
					OperatorCaches.UnconvertibleTypeCache.Insert(SourceType);
				}
				if (flag || flag2)
				{
					OperatorCaches.ConversionCache.Insert(TargetType, SourceType, conversionClass, OperatorMethod);
				}
			}
			return conversionClass;
		}

		private static ConversionResolution.ConversionClass DoClassifyUserDefinedConversion(Type TargetType, Type SourceType, ref Symbols.Method OperatorMethod, ref bool FoundTargetTypeOperators, ref bool FoundSourceTypeOperators)
		{
			OperatorMethod = null;
			List<Symbols.Method> list = ConversionResolution.CollectConversionOperators(TargetType, SourceType, ref FoundTargetTypeOperators, ref FoundSourceTypeOperators);
			if (list.Count == 0)
			{
				return ConversionResolution.ConversionClass.None;
			}
			bool flag = false;
			List<Symbols.Method> list2 = ConversionResolution.ResolveConversion(TargetType, SourceType, list, true, ref flag);
			if (list2.Count == 1)
			{
				OperatorMethod = list2[0];
				OperatorMethod.ArgumentsValidated = true;
				return ConversionResolution.ConversionClass.Widening;
			}
			if (list2.Count == 0 && !flag)
			{
				list2 = ConversionResolution.ResolveConversion(TargetType, SourceType, list, false, ref flag);
				if (list2.Count == 1)
				{
					OperatorMethod = list2[0];
					OperatorMethod.ArgumentsValidated = true;
					return ConversionResolution.ConversionClass.Narrowing;
				}
				if (list2.Count == 0)
				{
					return ConversionResolution.ConversionClass.None;
				}
			}
			return ConversionResolution.ConversionClass.Ambiguous;
		}

		private static readonly ConversionResolution.ConversionClass[][] ConversionTable = new ConversionResolution.ConversionClass[][]
		{
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Widening
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.None,
				ConversionResolution.ConversionClass.Identity,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Bad
			},
			new ConversionResolution.ConversionClass[]
			{
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Widening,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Narrowing,
				ConversionResolution.ConversionClass.Bad,
				ConversionResolution.ConversionClass.Identity
			}
		};

		internal static readonly int[] NumericSpecificityRank = new int[19];

		internal static readonly TypeCode[][] ForLoopWidestTypeCode;

		internal enum ConversionClass : sbyte
		{
			Bad,
			Identity,
			Widening,
			Narrowing,
			None,
			Ambiguous
		}
	}
}

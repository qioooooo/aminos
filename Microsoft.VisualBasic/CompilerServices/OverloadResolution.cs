using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microsoft.VisualBasic.CompilerServices
{
	internal class OverloadResolution
	{
		private OverloadResolution()
		{
		}

		private static bool IsExactSignatureMatch(ParameterInfo[] LeftSignature, int LeftTypeParameterCount, ParameterInfo[] RightSignature, int RightTypeParameterCount)
		{
			ParameterInfo[] array;
			ParameterInfo[] array2;
			if (LeftSignature.Length >= RightSignature.Length)
			{
				array = LeftSignature;
				array2 = RightSignature;
			}
			else
			{
				array = RightSignature;
				array2 = LeftSignature;
			}
			int num = array2.Length;
			checked
			{
				int num2 = array.Length - 1;
				for (int i = num; i <= num2; i++)
				{
					if (!array[i].IsOptional)
					{
						return false;
					}
				}
				int num3 = 0;
				int num4 = array2.Length - 1;
				for (int j = num3; j <= num4; j++)
				{
					Type type = array2[j].ParameterType;
					Type type2 = array[j].ParameterType;
					if (type.IsByRef)
					{
						type = type.GetElementType();
					}
					if (type2.IsByRef)
					{
						type2 = type2.GetElementType();
					}
					if (type != type2 && (!array2[j].IsOptional || !array[j].IsOptional))
					{
						return false;
					}
				}
				return true;
			}
		}

		private static void CompareNumericTypeSpecificity(Type LeftType, Type RightType, ref bool LeftWins, ref bool RightWins)
		{
			if (LeftType == RightType)
			{
				return;
			}
			if (ConversionResolution.NumericSpecificityRank[(int)Symbols.GetTypeCode(LeftType)] < ConversionResolution.NumericSpecificityRank[(int)Symbols.GetTypeCode(RightType)])
			{
				LeftWins = true;
			}
			else
			{
				RightWins = true;
			}
		}

		private static void CompareParameterSpecificity(Type ArgumentType, ParameterInfo LeftParameter, MethodBase LeftProcedure, bool ExpandLeftParamArray, ParameterInfo RightParameter, MethodBase RightProcedure, bool ExpandRightParamArray, ref bool LeftWins, ref bool RightWins, ref bool BothLose)
		{
			BothLose = false;
			Type type = LeftParameter.ParameterType;
			Type type2 = RightParameter.ParameterType;
			if (type.IsByRef)
			{
				type = Symbols.GetElementType(type);
			}
			if (type2.IsByRef)
			{
				type2 = Symbols.GetElementType(type2);
			}
			if (ExpandLeftParamArray && Symbols.IsParamArray(LeftParameter))
			{
				type = Symbols.GetElementType(type);
			}
			if (ExpandRightParamArray && Symbols.IsParamArray(RightParameter))
			{
				type2 = Symbols.GetElementType(type2);
			}
			if (Symbols.IsNumericType(type) && Symbols.IsNumericType(type2) && !Symbols.IsEnum(type) && !Symbols.IsEnum(type2))
			{
				OverloadResolution.CompareNumericTypeSpecificity(type, type2, ref LeftWins, ref RightWins);
				return;
			}
			if (LeftProcedure != null && RightProcedure != null && Symbols.IsRawGeneric(LeftProcedure) && Symbols.IsRawGeneric(RightProcedure))
			{
				if (type == type2)
				{
					return;
				}
				int num = Symbols.IndexIn(type, LeftProcedure);
				int num2 = Symbols.IndexIn(type2, RightProcedure);
				if (num == num2 && num >= 0)
				{
					return;
				}
			}
			Symbols.Method method = null;
			ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyConversion(type2, type, ref method);
			if (conversionClass == ConversionResolution.ConversionClass.Identity)
			{
				return;
			}
			if (conversionClass == ConversionResolution.ConversionClass.Widening)
			{
				if (method == null || ConversionResolution.ClassifyConversion(type, type2, ref method) != ConversionResolution.ConversionClass.Widening)
				{
					LeftWins = true;
					return;
				}
				if (ArgumentType != null && ArgumentType == type)
				{
					LeftWins = true;
					return;
				}
				if (ArgumentType != null && ArgumentType == type2)
				{
					RightWins = true;
					return;
				}
				BothLose = true;
				return;
			}
			else
			{
				ConversionResolution.ConversionClass conversionClass2 = ConversionResolution.ClassifyConversion(type, type2, ref method);
				if (conversionClass2 == ConversionResolution.ConversionClass.Widening)
				{
					RightWins = true;
					return;
				}
				BothLose = true;
				return;
			}
		}

		private static void CompareGenericityBasedOnMethodGenericParams(ParameterInfo LeftParameter, ParameterInfo RawLeftParameter, Symbols.Method LeftMember, bool ExpandLeftParamArray, ParameterInfo RightParameter, ParameterInfo RawRightParameter, Symbols.Method RightMember, bool ExpandRightParamArray, ref bool LeftIsLessGeneric, ref bool RightIsLessGeneric, ref bool SignatureMismatch)
		{
			if (LeftMember.IsMethod)
			{
				if (!RightMember.IsMethod)
				{
					return;
				}
				Type type = LeftParameter.ParameterType;
				Type type2 = RightParameter.ParameterType;
				Type type3 = RawLeftParameter.ParameterType;
				Type type4 = RawRightParameter.ParameterType;
				if (type.IsByRef)
				{
					type = Symbols.GetElementType(type);
					type3 = Symbols.GetElementType(type3);
				}
				if (type2.IsByRef)
				{
					type2 = Symbols.GetElementType(type2);
					type4 = Symbols.GetElementType(type4);
				}
				if (ExpandLeftParamArray && Symbols.IsParamArray(LeftParameter))
				{
					type = Symbols.GetElementType(type);
					type3 = Symbols.GetElementType(type3);
				}
				if (ExpandRightParamArray && Symbols.IsParamArray(RightParameter))
				{
					type2 = Symbols.GetElementType(type2);
					type4 = Symbols.GetElementType(type4);
				}
				if (type != type2)
				{
					SignatureMismatch = true;
					return;
				}
				MethodBase methodBase = LeftMember.AsMethod();
				MethodBase methodBase2 = RightMember.AsMethod();
				if (Symbols.IsGeneric(methodBase))
				{
					methodBase = ((MethodInfo)methodBase).GetGenericMethodDefinition();
				}
				if (Symbols.IsGeneric(methodBase2))
				{
					methodBase2 = ((MethodInfo)methodBase2).GetGenericMethodDefinition();
				}
				if (Symbols.RefersToGenericParameter(type3, methodBase))
				{
					if (!Symbols.RefersToGenericParameter(type4, methodBase2))
					{
						RightIsLessGeneric = true;
					}
				}
				else if (Symbols.RefersToGenericParameter(type4, methodBase2) && !Symbols.RefersToGenericParameter(type3, methodBase))
				{
					LeftIsLessGeneric = true;
				}
			}
		}

		private static void CompareGenericityBasedOnTypeGenericParams(ParameterInfo LeftParameter, ParameterInfo RawLeftParameter, Symbols.Method LeftMember, bool ExpandLeftParamArray, ParameterInfo RightParameter, ParameterInfo RawRightParameter, Symbols.Method RightMember, bool ExpandRightParamArray, ref bool LeftIsLessGeneric, ref bool RightIsLessGeneric, ref bool SignatureMismatch)
		{
			Type type = LeftParameter.ParameterType;
			Type type2 = RightParameter.ParameterType;
			Type type3 = RawLeftParameter.ParameterType;
			Type type4 = RawRightParameter.ParameterType;
			if (type.IsByRef)
			{
				type = Symbols.GetElementType(type);
				type3 = Symbols.GetElementType(type3);
			}
			if (type2.IsByRef)
			{
				type2 = Symbols.GetElementType(type2);
				type4 = Symbols.GetElementType(type4);
			}
			if (ExpandLeftParamArray && Symbols.IsParamArray(LeftParameter))
			{
				type = Symbols.GetElementType(type);
				type3 = Symbols.GetElementType(type3);
			}
			if (ExpandRightParamArray && Symbols.IsParamArray(RightParameter))
			{
				type2 = Symbols.GetElementType(type2);
				type4 = Symbols.GetElementType(type4);
			}
			if (type != type2)
			{
				SignatureMismatch = true;
				return;
			}
			Type rawDeclaringType = LeftMember.RawDeclaringType;
			Type rawDeclaringType2 = RightMember.RawDeclaringType;
			if (Symbols.RefersToGenericParameterCLRSemantics(type3, rawDeclaringType))
			{
				if (!Symbols.RefersToGenericParameterCLRSemantics(type4, rawDeclaringType2))
				{
					RightIsLessGeneric = true;
				}
			}
			else if (Symbols.RefersToGenericParameterCLRSemantics(type4, rawDeclaringType2))
			{
				LeftIsLessGeneric = true;
			}
		}

		private static Symbols.Method LeastGenericProcedure(Symbols.Method Left, Symbols.Method Right, OverloadResolution.ComparisonType CompareGenericity, ref bool SignatureMismatch)
		{
			bool flag = false;
			bool flag2 = false;
			SignatureMismatch = false;
			if (!Left.IsMethod || !Right.IsMethod)
			{
				return null;
			}
			int num = 0;
			int num2 = Left.Parameters.Length;
			int num3 = Right.Parameters.Length;
			checked
			{
				while (num < num2 && num < num3)
				{
					switch (CompareGenericity)
					{
					case OverloadResolution.ComparisonType.GenericSpecificityBasedOnMethodGenericParams:
						OverloadResolution.CompareGenericityBasedOnMethodGenericParams(Left.Parameters[num], Left.RawParameters[num], Left, Left.ParamArrayExpanded, Right.Parameters[num], Right.RawParameters[num], Right, false, ref flag, ref flag2, ref SignatureMismatch);
						break;
					case OverloadResolution.ComparisonType.GenericSpecificityBasedOnTypeGenericParams:
						OverloadResolution.CompareGenericityBasedOnTypeGenericParams(Left.Parameters[num], Left.RawParameters[num], Left, Left.ParamArrayExpanded, Right.Parameters[num], Right.RawParameters[num], Right, false, ref flag, ref flag2, ref SignatureMismatch);
						break;
					}
					if (SignatureMismatch || (flag && flag2))
					{
						return null;
					}
					num++;
				}
				if (num < num2 || num < num3)
				{
					return null;
				}
				if (flag)
				{
					return Left;
				}
				if (flag2)
				{
					return Right;
				}
				return null;
			}
		}

		internal static Symbols.Method LeastGenericProcedure(Symbols.Method Left, Symbols.Method Right)
		{
			if (!Left.IsGeneric && !Right.IsGeneric && !Symbols.IsGeneric(Left.DeclaringType) && !Symbols.IsGeneric(Right.DeclaringType))
			{
				return null;
			}
			bool flag = false;
			Symbols.Method method = OverloadResolution.LeastGenericProcedure(Left, Right, OverloadResolution.ComparisonType.GenericSpecificityBasedOnMethodGenericParams, ref flag);
			if (method == null && !flag)
			{
				method = OverloadResolution.LeastGenericProcedure(Left, Right, OverloadResolution.ComparisonType.GenericSpecificityBasedOnTypeGenericParams, ref flag);
			}
			return method;
		}

		private static void InsertIfMethodAvailable(MemberInfo NewCandidate, ParameterInfo[] NewCandidateSignature, int NewCandidateParamArrayIndex, bool ExpandNewCandidateParamArray, object[] Arguments, int ArgumentCount, string[] ArgumentNames, Type[] TypeArguments, bool CollectOnlyOperators, List<Symbols.Method> Candidates)
		{
			Symbols.Method method = null;
			checked
			{
				if (!CollectOnlyOperators)
				{
					MethodBase methodBase = NewCandidate as MethodBase;
					bool flag = false;
					if (NewCandidate.MemberType == MemberTypes.Method && Symbols.IsRawGeneric(methodBase))
					{
						method = new Symbols.Method(methodBase, NewCandidateSignature, NewCandidateParamArrayIndex, ExpandNewCandidateParamArray);
						OverloadResolution.RejectUncallableProcedure(method, Arguments, ArgumentNames, TypeArguments);
						NewCandidate = method.AsMethod();
						NewCandidateSignature = method.Parameters;
					}
					if (NewCandidate != null && NewCandidate.MemberType == MemberTypes.Method && Symbols.IsRawGeneric(NewCandidate as MethodBase))
					{
						flag = true;
					}
					int num = 0;
					int num2 = Candidates.Count - 1;
					for (int i = num; i <= num2; i++)
					{
						Symbols.Method method2 = Candidates[i];
						if (method2 != null)
						{
							ParameterInfo[] parameters = method2.Parameters;
							MethodBase methodBase2;
							if (method2.IsMethod)
							{
								methodBase2 = method2.AsMethod();
							}
							else
							{
								methodBase2 = null;
							}
							if (!(NewCandidate == method2))
							{
								int num3 = 0;
								int num4 = 0;
								for (int j = 1; j <= ArgumentCount; j++)
								{
									bool flag2 = false;
									bool flag3 = false;
									bool flag4 = false;
									OverloadResolution.CompareParameterSpecificity(null, NewCandidateSignature[num3], methodBase, ExpandNewCandidateParamArray, parameters[num4], methodBase2, method2.ParamArrayExpanded, ref flag3, ref flag4, ref flag2);
									if (flag2 || flag3 || flag4)
									{
										goto IL_01D6;
									}
									if (num3 != NewCandidateParamArrayIndex || !ExpandNewCandidateParamArray)
									{
										num3++;
									}
									if (num4 != method2.ParamArrayIndex || !method2.ParamArrayExpanded)
									{
										num4++;
									}
								}
								if (!OverloadResolution.IsExactSignatureMatch(NewCandidateSignature, Symbols.GetTypeParameters(NewCandidate).Length, method2.Parameters, method2.TypeParameters.Length))
								{
									if (!flag)
									{
										if (methodBase2 == null || !Symbols.IsRawGeneric(methodBase2))
										{
											if (!ExpandNewCandidateParamArray && method2.ParamArrayExpanded)
											{
												Candidates[i] = null;
											}
											else
											{
												if (ExpandNewCandidateParamArray && !method2.ParamArrayExpanded)
												{
													return;
												}
												if (ExpandNewCandidateParamArray || method2.ParamArrayExpanded)
												{
													if (num3 > num4)
													{
														Candidates[i] = null;
													}
													else if (num4 > num3)
													{
														return;
													}
												}
											}
										}
									}
								}
								else
								{
									if (NewCandidate.DeclaringType == method2.DeclaringType)
									{
										break;
									}
									if (flag || methodBase2 == null || !Symbols.IsRawGeneric(methodBase2))
									{
										return;
									}
								}
							}
						}
						IL_01D6:;
					}
				}
				if (method != null)
				{
					Candidates.Add(method);
				}
				else if (NewCandidate.MemberType == MemberTypes.Property)
				{
					Candidates.Add(new Symbols.Method((PropertyInfo)NewCandidate, NewCandidateSignature, NewCandidateParamArrayIndex, ExpandNewCandidateParamArray));
				}
				else
				{
					Candidates.Add(new Symbols.Method((MethodBase)NewCandidate, NewCandidateSignature, NewCandidateParamArrayIndex, ExpandNewCandidateParamArray));
				}
			}
		}

		internal static List<Symbols.Method> CollectOverloadCandidates(MemberInfo[] Members, object[] Arguments, int ArgumentCount, string[] ArgumentNames, Type[] TypeArguments, bool CollectOnlyOperators, Type TerminatingScope, ref int RejectedForArgumentCount, ref int RejectedForTypeArgumentCount)
		{
			int num = 0;
			if (TypeArguments != null)
			{
				num = TypeArguments.Length;
			}
			List<Symbols.Method> list = new List<Symbols.Method>(Members.Length);
			if (Members.Length == 0)
			{
				return list;
			}
			bool flag = true;
			int i = 0;
			checked
			{
				do
				{
					Type declaringType = Members[i].DeclaringType;
					if (TerminatingScope != null && Symbols.IsOrInheritsFrom(TerminatingScope, declaringType))
					{
						break;
					}
					for (;;)
					{
						MemberInfo memberInfo = Members[i];
						int num2 = 0;
						MemberTypes memberType = memberInfo.MemberType;
						ParameterInfo[] array;
						if (memberType == MemberTypes.Constructor || memberType == MemberTypes.Method)
						{
							MethodBase methodBase = (MethodBase)memberInfo;
							if (!CollectOnlyOperators || Symbols.IsUserDefinedOperator(methodBase))
							{
								array = methodBase.GetParameters();
								num2 = Symbols.GetTypeParameters(methodBase).Length;
								if (Symbols.IsShadows(methodBase))
								{
									flag = false;
									goto IL_0146;
								}
								goto IL_0146;
							}
						}
						else if (memberType == MemberTypes.Property)
						{
							if (!CollectOnlyOperators)
							{
								PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
								MethodInfo getMethod = propertyInfo.GetGetMethod();
								if (getMethod == null)
								{
									MethodInfo setMethod = propertyInfo.GetSetMethod();
									ParameterInfo[] parameters = setMethod.GetParameters();
									array = new ParameterInfo[parameters.Length - 2 + 1];
									Array.Copy(parameters, array, array.Length);
									if (Symbols.IsShadows(setMethod))
									{
										flag = false;
									}
									goto IL_0146;
								}
								array = getMethod.GetParameters();
								if (Symbols.IsShadows(getMethod))
								{
									flag = false;
									goto IL_0146;
								}
								goto IL_0146;
							}
						}
						else if ((memberType == MemberTypes.Custom || memberType == MemberTypes.Event || memberType == MemberTypes.Field || memberType == MemberTypes.TypeInfo || memberType == MemberTypes.NestedType) && !CollectOnlyOperators)
						{
							flag = false;
						}
						IL_01C8:
						i++;
						if (i >= Members.Length || Members[i].DeclaringType != declaringType)
						{
							break;
						}
						continue;
						IL_0146:
						int num3 = 0;
						int num4 = 0;
						int num5 = -1;
						Symbols.GetAllParameterCounts(array, ref num3, ref num4, ref num5);
						bool flag2 = num5 >= 0;
						if (ArgumentCount < num3 || (!flag2 && ArgumentCount > num4))
						{
							RejectedForArgumentCount++;
							goto IL_01C8;
						}
						if (num > 0 && num != num2)
						{
							RejectedForTypeArgumentCount++;
							goto IL_01C8;
						}
						if (!flag2 || ArgumentCount == num4)
						{
							OverloadResolution.InsertIfMethodAvailable(memberInfo, array, num5, false, Arguments, ArgumentCount, ArgumentNames, TypeArguments, CollectOnlyOperators, list);
						}
						if (flag2)
						{
							OverloadResolution.InsertIfMethodAvailable(memberInfo, array, num5, true, Arguments, ArgumentCount, ArgumentNames, TypeArguments, CollectOnlyOperators, list);
							goto IL_01C8;
						}
						goto IL_01C8;
					}
				}
				while (flag && i < Members.Length);
				for (i = 0; i < list.Count; i++)
				{
					if (list[i] == null)
					{
						int num6 = i + 1;
						while (num6 < list.Count && list[num6] == null)
						{
							num6++;
						}
						list.RemoveRange(i, num6 - i);
					}
				}
				return list;
			}
		}

		private static bool CanConvert(Type TargetType, Type SourceType, bool RejectNarrowingConversion, List<string> Errors, string ParameterName, bool IsByRefCopyBackContext, ref bool RequiresNarrowingConversion, ref bool AllNarrowingIsFromObject)
		{
			Symbols.Method method = null;
			ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyConversion(TargetType, SourceType, ref method);
			switch (conversionClass)
			{
			case ConversionResolution.ConversionClass.Identity:
			case ConversionResolution.ConversionClass.Widening:
				return true;
			case ConversionResolution.ConversionClass.Narrowing:
				if (RejectNarrowingConversion)
				{
					if (Errors != null)
					{
						OverloadResolution.ReportError(Errors, Interaction.IIf<string>(IsByRefCopyBackContext, "ArgumentNarrowingCopyBack3", "ArgumentNarrowing3"), ParameterName, SourceType, TargetType);
					}
					return false;
				}
				RequiresNarrowingConversion = true;
				if (SourceType != typeof(object))
				{
					AllNarrowingIsFromObject = false;
				}
				return true;
			default:
				if (Errors != null)
				{
					OverloadResolution.ReportError(Errors, Interaction.IIf<string>(conversionClass == ConversionResolution.ConversionClass.Ambiguous, Interaction.IIf<string>(IsByRefCopyBackContext, "ArgumentMismatchAmbiguousCopyBack3", "ArgumentMismatchAmbiguous3"), Interaction.IIf<string>(IsByRefCopyBackContext, "ArgumentMismatchCopyBack3", "ArgumentMismatch3")), ParameterName, SourceType, TargetType);
				}
				return false;
			}
		}

		private static bool InferTypeArgumentsFromArgument(Type ArgumentType, Type ParameterType, Type[] TypeInferenceArguments, MethodBase TargetProcedure, bool DigThroughToBasesAndImplements)
		{
			bool flag = OverloadResolution.InferTypeArgumentsFromArgumentDirectly(ArgumentType, ParameterType, TypeInferenceArguments, TargetProcedure, DigThroughToBasesAndImplements);
			if (!flag)
			{
				if (DigThroughToBasesAndImplements)
				{
					if (Symbols.IsInstantiatedGeneric(ParameterType))
					{
						if (ParameterType.IsClass || ParameterType.IsInterface)
						{
							Type genericTypeDefinition = ParameterType.GetGenericTypeDefinition();
							if (Symbols.IsArrayType(ArgumentType))
							{
								if (ArgumentType.GetArrayRank() > 1 || ParameterType.IsClass)
								{
									return false;
								}
								ArgumentType = typeof(IList<>).MakeGenericType(new Type[] { ArgumentType.GetElementType() });
								if (typeof(IList<>) == genericTypeDefinition)
								{
									goto IL_0137;
								}
							}
							else
							{
								if (!ArgumentType.IsClass && !ArgumentType.IsInterface)
								{
									return false;
								}
								if (Symbols.IsInstantiatedGeneric(ArgumentType) && ArgumentType.GetGenericTypeDefinition() == genericTypeDefinition)
								{
									return false;
								}
							}
							if (ParameterType.IsClass)
							{
								if (!ArgumentType.IsClass)
								{
									return false;
								}
								Type type;
								for (type = ArgumentType.BaseType; type != null; type = type.BaseType)
								{
									if (Symbols.IsInstantiatedGeneric(type) && type.GetGenericTypeDefinition() == genericTypeDefinition)
									{
										break;
									}
								}
								ArgumentType = type;
							}
							else
							{
								Type type2 = null;
								foreach (Type type3 in ArgumentType.GetInterfaces())
								{
									if (Symbols.IsInstantiatedGeneric(type3) && type3.GetGenericTypeDefinition() == genericTypeDefinition)
									{
										if (type2 != null)
										{
											return false;
										}
										type2 = type3;
									}
								}
								ArgumentType = type2;
							}
							if (ArgumentType == null)
							{
								return false;
							}
							IL_0137:
							return OverloadResolution.InferTypeArgumentsFromArgumentDirectly(ArgumentType, ParameterType, TypeInferenceArguments, TargetProcedure, DigThroughToBasesAndImplements);
						}
					}
				}
			}
			return flag;
		}

		private static bool InferTypeArgumentsFromArgumentDirectly(Type ArgumentType, Type ParameterType, Type[] TypeInferenceArguments, MethodBase TargetProcedure, bool DigThroughToBasesAndImplements)
		{
			if (!Symbols.RefersToGenericParameter(ParameterType, TargetProcedure))
			{
				return true;
			}
			checked
			{
				if (Symbols.IsGenericParameter(ParameterType))
				{
					if (Symbols.AreGenericMethodDefsEqual(ParameterType.DeclaringMethod, TargetProcedure))
					{
						int genericParameterPosition = ParameterType.GenericParameterPosition;
						if (TypeInferenceArguments[genericParameterPosition] == null)
						{
							TypeInferenceArguments[genericParameterPosition] = ArgumentType;
						}
						else if (TypeInferenceArguments[genericParameterPosition] != ArgumentType)
						{
							return false;
						}
					}
				}
				else if (Symbols.IsInstantiatedGeneric(ParameterType))
				{
					Type type = null;
					if (Symbols.IsInstantiatedGeneric(ArgumentType) && ArgumentType.GetGenericTypeDefinition() == ParameterType.GetGenericTypeDefinition())
					{
						type = ArgumentType;
					}
					if (type == null && DigThroughToBasesAndImplements)
					{
						foreach (Type type2 in ArgumentType.GetInterfaces())
						{
							if (Symbols.IsInstantiatedGeneric(type2) && type2.GetGenericTypeDefinition() == ParameterType.GetGenericTypeDefinition())
							{
								if (type != null)
								{
									return false;
								}
								type = type2;
							}
						}
					}
					if (type != null)
					{
						Type[] typeArguments = Symbols.GetTypeArguments(ParameterType);
						Type[] typeArguments2 = Symbols.GetTypeArguments(type);
						int num = 0;
						int num2 = typeArguments2.Length - 1;
						for (int j = num; j <= num2; j++)
						{
							if (!OverloadResolution.InferTypeArgumentsFromArgument(typeArguments2[j], typeArguments[j], TypeInferenceArguments, TargetProcedure, false))
							{
								return false;
							}
						}
						return true;
					}
					return false;
				}
				else if (Symbols.IsArrayType(ParameterType))
				{
					return Symbols.IsArrayType(ArgumentType) && ParameterType.GetArrayRank() == ArgumentType.GetArrayRank() && OverloadResolution.InferTypeArgumentsFromArgument(Symbols.GetElementType(ArgumentType), Symbols.GetElementType(ParameterType), TypeInferenceArguments, TargetProcedure, DigThroughToBasesAndImplements);
				}
				return true;
			}
		}

		private static bool CanPassToParamArray(Symbols.Method TargetProcedure, object Argument, ParameterInfo Parameter)
		{
			if (Argument == null)
			{
				return true;
			}
			Type parameterType = Parameter.ParameterType;
			Type argumentType = OverloadResolution.GetArgumentType(Argument);
			Type type = parameterType;
			Type type2 = argumentType;
			Symbols.Method method = null;
			ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyConversion(type, type2, ref method);
			return conversionClass == ConversionResolution.ConversionClass.Widening || conversionClass == ConversionResolution.ConversionClass.Identity;
		}

		internal static bool CanPassToParameter(Symbols.Method TargetProcedure, object Argument, ParameterInfo Parameter, bool IsExpandedParamArray, bool RejectNarrowingConversions, List<string> Errors, ref bool RequiresNarrowingConversion, ref bool AllNarrowingIsFromObject)
		{
			if (Argument == null)
			{
				return true;
			}
			Type type = Parameter.ParameterType;
			bool isByRef = type.IsByRef;
			if (isByRef || IsExpandedParamArray)
			{
				type = Symbols.GetElementType(type);
			}
			Type argumentType = OverloadResolution.GetArgumentType(Argument);
			if (Argument == Missing.Value)
			{
				if (Parameter.IsOptional)
				{
					return true;
				}
				if (!Symbols.IsRootObjectType(type) || !IsExpandedParamArray)
				{
					if (Errors != null)
					{
						if (IsExpandedParamArray)
						{
							OverloadResolution.ReportError(Errors, "OmittedParamArrayArgument");
						}
						else
						{
							OverloadResolution.ReportError(Errors, "OmittedArgument1", Parameter.Name);
						}
					}
					return false;
				}
			}
			bool flag = OverloadResolution.CanConvert(type, argumentType, RejectNarrowingConversions, Errors, Parameter.Name, false, ref RequiresNarrowingConversion, ref AllNarrowingIsFromObject);
			if (!isByRef || !flag)
			{
				return flag;
			}
			return OverloadResolution.CanConvert(argumentType, type, RejectNarrowingConversions, Errors, Parameter.Name, true, ref RequiresNarrowingConversion, ref AllNarrowingIsFromObject);
		}

		internal static bool InferTypeArgumentsFromArgument(Symbols.Method TargetProcedure, object Argument, ParameterInfo Parameter, bool IsExpandedParamArray, List<string> Errors)
		{
			if (Argument == null)
			{
				return true;
			}
			Type type = Parameter.ParameterType;
			if (type.IsByRef || IsExpandedParamArray)
			{
				type = Symbols.GetElementType(type);
			}
			Type argumentType = OverloadResolution.GetArgumentType(Argument);
			if (!OverloadResolution.InferTypeArgumentsFromArgument(argumentType, type, TargetProcedure.TypeArguments, TargetProcedure.AsMethod(), true))
			{
				if (Errors != null)
				{
					OverloadResolution.ReportError(Errors, "TypeInferenceFails1", Parameter.Name);
				}
				return false;
			}
			return true;
		}

		internal static object PassToParameter(object Argument, ParameterInfo Parameter, Type ParameterType)
		{
			bool isByRef = ParameterType.IsByRef;
			if (isByRef)
			{
				ParameterType = ParameterType.GetElementType();
			}
			if (Argument is Symbols.TypedNothing)
			{
				Argument = null;
			}
			if (Argument == Missing.Value && Parameter.IsOptional)
			{
				Argument = Parameter.DefaultValue;
			}
			if (isByRef)
			{
				Type argumentType = OverloadResolution.GetArgumentType(Argument);
				if (argumentType != null && Symbols.IsValueType(argumentType))
				{
					Argument = Conversions.ForceValueCopy(Argument, argumentType);
				}
			}
			return Conversions.ChangeType(Argument, ParameterType);
		}

		private static bool FindParameterByName(ParameterInfo[] Parameters, string Name, ref int Index)
		{
			checked
			{
				for (int i = 0; i < Parameters.Length; i++)
				{
					if (Operators.CompareString(Name, Parameters[i].Name, true) == 0)
					{
						Index = i;
						return true;
					}
				}
				return false;
			}
		}

		private static bool[] CreateMatchTable(int Size, int LastPositionalMatchIndex)
		{
			checked
			{
				bool[] array = new bool[Size - 1 + 1];
				for (int i = 0; i <= LastPositionalMatchIndex; i++)
				{
					array[i] = true;
				}
				return array;
			}
		}

		internal static bool CanMatchArguments(Symbols.Method TargetProcedure, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, bool RejectNarrowingConversions, List<string> Errors)
		{
			bool flag = Errors != null;
			TargetProcedure.ArgumentsValidated = true;
			checked
			{
				if (TargetProcedure.IsMethod && Symbols.IsRawGeneric(TargetProcedure.AsMethod()))
				{
					if (TypeArguments.Length == 0)
					{
						TypeArguments = new Type[TargetProcedure.TypeParameters.Length - 1 + 1];
						TargetProcedure.TypeArguments = TypeArguments;
						if (!OverloadResolution.InferTypeArguments(TargetProcedure, Arguments, ArgumentNames, TypeArguments, Errors))
						{
							return false;
						}
					}
					else
					{
						TargetProcedure.TypeArguments = TypeArguments;
					}
					if (!OverloadResolution.InstantiateGenericMethod(TargetProcedure, TypeArguments, Errors))
					{
						return false;
					}
				}
				ParameterInfo[] parameters = TargetProcedure.Parameters;
				int i = ArgumentNames.Length;
				int num = 0;
				while (i < Arguments.Length && num != TargetProcedure.ParamArrayIndex)
				{
					if (!OverloadResolution.CanPassToParameter(TargetProcedure, Arguments[i], parameters[num], false, RejectNarrowingConversions, Errors, ref TargetProcedure.RequiresNarrowingConversion, ref TargetProcedure.AllNarrowingIsFromObject) && !flag)
					{
						return false;
					}
					i++;
					num++;
				}
				if (TargetProcedure.HasParamArray)
				{
					if (TargetProcedure.ParamArrayExpanded)
					{
						if (i == Arguments.Length - 1 && Arguments[i] == null)
						{
							return false;
						}
						while (i < Arguments.Length)
						{
							if (!OverloadResolution.CanPassToParameter(TargetProcedure, Arguments[i], parameters[num], true, RejectNarrowingConversions, Errors, ref TargetProcedure.RequiresNarrowingConversion, ref TargetProcedure.AllNarrowingIsFromObject) && !flag)
							{
								return false;
							}
							i++;
						}
					}
					else
					{
						if (Arguments.Length - i != 1)
						{
							return false;
						}
						if (!OverloadResolution.CanPassToParamArray(TargetProcedure, Arguments[i], parameters[num]))
						{
							if (flag)
							{
								OverloadResolution.ReportError(Errors, "ArgumentMismatch3", parameters[num].Name, OverloadResolution.GetArgumentType(Arguments[i]), parameters[num].ParameterType);
							}
							return false;
						}
					}
					num++;
				}
				bool[] array = null;
				if (ArgumentNames.Length > 0 || num < parameters.Length)
				{
					array = OverloadResolution.CreateMatchTable(parameters.Length, num - 1);
				}
				if (ArgumentNames.Length > 0)
				{
					int[] array2 = new int[ArgumentNames.Length - 1 + 1];
					for (i = 0; i < ArgumentNames.Length; i++)
					{
						if (!OverloadResolution.FindParameterByName(parameters, ArgumentNames[i], ref num))
						{
							if (!flag)
							{
								return false;
							}
							OverloadResolution.ReportError(Errors, "NamedParamNotFound2", ArgumentNames[i], TargetProcedure);
						}
						else if (num == TargetProcedure.ParamArrayIndex)
						{
							if (!flag)
							{
								return false;
							}
							OverloadResolution.ReportError(Errors, "NamedParamArrayArgument1", ArgumentNames[i]);
						}
						else if (array[num])
						{
							if (!flag)
							{
								return false;
							}
							OverloadResolution.ReportError(Errors, "NamedArgUsedTwice2", ArgumentNames[i], TargetProcedure);
						}
						else
						{
							if (!OverloadResolution.CanPassToParameter(TargetProcedure, Arguments[i], parameters[num], false, RejectNarrowingConversions, Errors, ref TargetProcedure.RequiresNarrowingConversion, ref TargetProcedure.AllNarrowingIsFromObject) && !flag)
							{
								return false;
							}
							array[num] = true;
							array2[i] = num;
						}
					}
					TargetProcedure.NamedArgumentMapping = array2;
				}
				if (array != null)
				{
					int num2 = 0;
					int num3 = array.Length - 1;
					for (int j = num2; j <= num3; j++)
					{
						if (!array[j] && !parameters[j].IsOptional)
						{
							if (!flag)
							{
								return false;
							}
							OverloadResolution.ReportError(Errors, "OmittedArgument1", parameters[j].Name);
						}
					}
				}
				return Errors == null || Errors.Count <= 0;
			}
		}

		private static bool InstantiateGenericMethod(Symbols.Method TargetProcedure, Type[] TypeArguments, List<string> Errors)
		{
			bool flag = Errors != null;
			int num = 0;
			checked
			{
				int num2 = TypeArguments.Length - 1;
				for (int i = num; i <= num2; i++)
				{
					if (TypeArguments[i] == null)
					{
						if (!flag)
						{
							return false;
						}
						OverloadResolution.ReportError(Errors, "UnboundTypeParam1", TargetProcedure.TypeParameters[i].Name);
					}
				}
				if ((Errors == null || Errors.Count == 0) && !TargetProcedure.BindGenericArguments())
				{
					if (!flag)
					{
						return false;
					}
					OverloadResolution.ReportError(Errors, "FailedTypeArgumentBinding");
				}
				return Errors == null || Errors.Count <= 0;
			}
		}

		internal static void MatchArguments(Symbols.Method TargetProcedure, object[] Arguments, object[] MatchedArguments)
		{
			ParameterInfo[] parameters = TargetProcedure.Parameters;
			int[] namedArgumentMapping = TargetProcedure.NamedArgumentMapping;
			int i = 0;
			if (namedArgumentMapping != null)
			{
				i = namedArgumentMapping.Length;
			}
			int num = 0;
			checked
			{
				while (i < Arguments.Length && num != TargetProcedure.ParamArrayIndex)
				{
					MatchedArguments[num] = OverloadResolution.PassToParameter(Arguments[i], parameters[num], parameters[num].ParameterType);
					i++;
					num++;
				}
				if (TargetProcedure.HasParamArray)
				{
					if (TargetProcedure.ParamArrayExpanded)
					{
						int num2 = Arguments.Length - i;
						ParameterInfo parameterInfo = parameters[num];
						Type elementType = parameterInfo.ParameterType.GetElementType();
						Array array = Array.CreateInstance(elementType, num2);
						int num3 = 0;
						while (i < Arguments.Length)
						{
							array.SetValue(OverloadResolution.PassToParameter(Arguments[i], parameterInfo, elementType), num3);
							i++;
							num3++;
						}
						MatchedArguments[num] = array;
					}
					else
					{
						MatchedArguments[num] = OverloadResolution.PassToParameter(Arguments[i], parameters[num], parameters[num].ParameterType);
					}
					num++;
				}
				bool[] array2 = null;
				if (namedArgumentMapping != null || num < parameters.Length)
				{
					array2 = OverloadResolution.CreateMatchTable(parameters.Length, num - 1);
				}
				if (namedArgumentMapping != null)
				{
					for (i = 0; i < namedArgumentMapping.Length; i++)
					{
						num = namedArgumentMapping[i];
						MatchedArguments[num] = OverloadResolution.PassToParameter(Arguments[i], parameters[num], parameters[num].ParameterType);
						array2[num] = true;
					}
				}
				if (array2 != null)
				{
					int num4 = 0;
					int num5 = array2.Length - 1;
					for (int j = num4; j <= num5; j++)
					{
						if (!array2[j])
						{
							MatchedArguments[j] = OverloadResolution.PassToParameter(Missing.Value, parameters[j], parameters[j].ParameterType);
						}
					}
					return;
				}
			}
		}

		private static bool InferTypeArguments(Symbols.Method TargetProcedure, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, List<string> Errors)
		{
			bool flag = Errors != null;
			ParameterInfo[] rawParameters = TargetProcedure.RawParameters;
			int i = ArgumentNames.Length;
			int num = 0;
			checked
			{
				while (i < Arguments.Length && num != TargetProcedure.ParamArrayIndex)
				{
					if (!OverloadResolution.InferTypeArgumentsFromArgument(TargetProcedure, Arguments[i], rawParameters[num], false, Errors) && !flag)
					{
						return false;
					}
					i++;
					num++;
				}
				if (TargetProcedure.HasParamArray)
				{
					if (TargetProcedure.ParamArrayExpanded)
					{
						while (i < Arguments.Length)
						{
							if (!OverloadResolution.InferTypeArgumentsFromArgument(TargetProcedure, Arguments[i], rawParameters[num], true, Errors) && !flag)
							{
								return false;
							}
							i++;
						}
					}
					else
					{
						if (Arguments.Length - i != 1)
						{
							return true;
						}
						if (!OverloadResolution.InferTypeArgumentsFromArgument(TargetProcedure, Arguments[i], rawParameters[num], false, Errors))
						{
							return false;
						}
					}
					num++;
				}
				if (ArgumentNames.Length > 0)
				{
					for (i = 0; i < ArgumentNames.Length; i++)
					{
						if (OverloadResolution.FindParameterByName(rawParameters, ArgumentNames[i], ref num))
						{
							if (num != TargetProcedure.ParamArrayIndex && !OverloadResolution.InferTypeArgumentsFromArgument(TargetProcedure, Arguments[i], rawParameters[num], false, Errors) && !flag)
							{
								return false;
							}
						}
					}
				}
				return Errors == null || Errors.Count <= 0;
			}
		}

		internal static void ReorderArgumentArray(Symbols.Method TargetProcedure, object[] ParameterResults, object[] Arguments, bool[] CopyBack, BindingFlags LookupFlags)
		{
			if (CopyBack == null)
			{
				return;
			}
			int num = 0;
			checked
			{
				int num2 = CopyBack.Length - 1;
				for (int i = num; i <= num2; i++)
				{
					CopyBack[i] = false;
				}
				if (!Symbols.HasFlag(LookupFlags, BindingFlags.SetProperty))
				{
					if (!TargetProcedure.HasByRefParameter)
					{
						return;
					}
					ParameterInfo[] parameters = TargetProcedure.Parameters;
					int[] namedArgumentMapping = TargetProcedure.NamedArgumentMapping;
					int j = 0;
					if (namedArgumentMapping != null)
					{
						j = namedArgumentMapping.Length;
					}
					int num3 = 0;
					while (j < Arguments.Length && num3 != TargetProcedure.ParamArrayIndex)
					{
						if (parameters[num3].ParameterType.IsByRef)
						{
							Arguments[j] = ParameterResults[num3];
							CopyBack[j] = true;
						}
						j++;
						num3++;
					}
					if (namedArgumentMapping != null)
					{
						for (j = 0; j < namedArgumentMapping.Length; j++)
						{
							num3 = namedArgumentMapping[j];
							if (parameters[num3].ParameterType.IsByRef)
							{
								Arguments[j] = ParameterResults[num3];
								CopyBack[j] = true;
							}
						}
						return;
					}
				}
			}
		}

		private static Symbols.Method RejectUncallableProcedures(List<Symbols.Method> Candidates, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, ref int CandidateCount, ref bool SomeCandidatesAreGeneric)
		{
			Symbols.Method method = null;
			int num = 0;
			checked
			{
				int num2 = Candidates.Count - 1;
				for (int i = num; i <= num2; i++)
				{
					Symbols.Method method2 = Candidates[i];
					if (!method2.ArgumentMatchingDone)
					{
						OverloadResolution.RejectUncallableProcedure(method2, Arguments, ArgumentNames, TypeArguments);
					}
					if (method2.NotCallable)
					{
						CandidateCount--;
					}
					else
					{
						method = method2;
						if (method2.IsGeneric || Symbols.IsGeneric(method2.DeclaringType))
						{
							SomeCandidatesAreGeneric = true;
						}
					}
				}
				return method;
			}
		}

		private static void RejectUncallableProcedure(Symbols.Method Candidate, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments)
		{
			if (!OverloadResolution.CanMatchArguments(Candidate, Arguments, ArgumentNames, TypeArguments, false, null))
			{
				Candidate.NotCallable = true;
			}
			Candidate.ArgumentMatchingDone = true;
		}

		private static Type GetArgumentType(object Argument)
		{
			if (Argument == null)
			{
				return null;
			}
			Symbols.TypedNothing typedNothing = Argument as Symbols.TypedNothing;
			if (typedNothing != null)
			{
				return typedNothing.Type;
			}
			return Argument.GetType();
		}

		private static Symbols.Method MoreSpecificProcedure(Symbols.Method Left, Symbols.Method Right, object[] Arguments, string[] ArgumentNames, OverloadResolution.ComparisonType CompareGenericity, ref bool BothLose = false, bool ContinueWhenBothLose = false)
		{
			BothLose = false;
			bool flag = false;
			bool flag2 = false;
			MethodBase methodBase;
			if (Left.IsMethod)
			{
				methodBase = Left.AsMethod();
			}
			else
			{
				methodBase = null;
			}
			MethodBase methodBase2;
			if (Right.IsMethod)
			{
				methodBase2 = Right.AsMethod();
			}
			else
			{
				methodBase2 = null;
			}
			int num = 0;
			int num2 = 0;
			int i = ArgumentNames.Length;
			checked
			{
				while (i < Arguments.Length)
				{
					Type argumentType = OverloadResolution.GetArgumentType(Arguments[i]);
					switch (CompareGenericity)
					{
					case OverloadResolution.ComparisonType.ParameterSpecificty:
						OverloadResolution.CompareParameterSpecificity(argumentType, Left.Parameters[num], methodBase, Left.ParamArrayExpanded, Right.Parameters[num2], methodBase2, Right.ParamArrayExpanded, ref flag, ref flag2, ref BothLose);
						break;
					case OverloadResolution.ComparisonType.GenericSpecificityBasedOnMethodGenericParams:
						OverloadResolution.CompareGenericityBasedOnMethodGenericParams(Left.Parameters[num], Left.RawParameters[num], Left, Left.ParamArrayExpanded, Right.Parameters[num2], Right.RawParameters[num2], Right, Right.ParamArrayExpanded, ref flag, ref flag2, ref BothLose);
						break;
					case OverloadResolution.ComparisonType.GenericSpecificityBasedOnTypeGenericParams:
						OverloadResolution.CompareGenericityBasedOnTypeGenericParams(Left.Parameters[num], Left.RawParametersFromType[num], Left, Left.ParamArrayExpanded, Right.Parameters[num2], Right.RawParametersFromType[num2], Right, Right.ParamArrayExpanded, ref flag, ref flag2, ref BothLose);
						break;
					}
					if (!BothLose || ContinueWhenBothLose)
					{
						if (!flag || !flag2)
						{
							if (num != Left.ParamArrayIndex)
							{
								num++;
							}
							if (num2 != Right.ParamArrayIndex)
							{
								num2++;
							}
							i++;
							continue;
						}
					}
					return null;
				}
				i = 0;
				while (i < ArgumentNames.Length)
				{
					bool flag3 = OverloadResolution.FindParameterByName(Left.Parameters, ArgumentNames[i], ref num);
					bool flag4 = OverloadResolution.FindParameterByName(Right.Parameters, ArgumentNames[i], ref num2);
					if (!flag3 || !flag4)
					{
						throw new InternalErrorException();
					}
					Type argumentType2 = OverloadResolution.GetArgumentType(Arguments[i]);
					switch (CompareGenericity)
					{
					case OverloadResolution.ComparisonType.ParameterSpecificty:
						OverloadResolution.CompareParameterSpecificity(argumentType2, Left.Parameters[num], methodBase, true, Right.Parameters[num2], methodBase2, true, ref flag, ref flag2, ref BothLose);
						break;
					case OverloadResolution.ComparisonType.GenericSpecificityBasedOnMethodGenericParams:
						OverloadResolution.CompareGenericityBasedOnMethodGenericParams(Left.Parameters[num], Left.RawParameters[num], Left, true, Right.Parameters[num2], Right.RawParameters[num2], Right, true, ref flag, ref flag2, ref BothLose);
						break;
					case OverloadResolution.ComparisonType.GenericSpecificityBasedOnTypeGenericParams:
						OverloadResolution.CompareGenericityBasedOnTypeGenericParams(Left.Parameters[num], Left.RawParameters[num], Left, true, Right.Parameters[num2], Right.RawParameters[num2], Right, true, ref flag, ref flag2, ref BothLose);
						break;
					}
					if (!BothLose || ContinueWhenBothLose)
					{
						if (!flag || !flag2)
						{
							i++;
							continue;
						}
					}
					return null;
				}
				if (flag)
				{
					return Left;
				}
				if (flag2)
				{
					return Right;
				}
				return null;
			}
		}

		private static Symbols.Method MostSpecificProcedure(List<Symbols.Method> Candidates, ref int CandidateCount, object[] Arguments, string[] ArgumentNames)
		{
			checked
			{
				try
				{
					foreach (Symbols.Method method in Candidates)
					{
						if (!method.NotCallable)
						{
							if (!method.RequiresNarrowingConversion)
							{
								bool flag = true;
								try
								{
									foreach (Symbols.Method method2 in Candidates)
									{
										if (!method2.NotCallable)
										{
											if (!method2.RequiresNarrowingConversion)
											{
												if (!(method2 == method) || method2.ParamArrayExpanded != method.ParamArrayExpanded)
												{
													Symbols.Method method3 = method;
													Symbols.Method method4 = method2;
													OverloadResolution.ComparisonType comparisonType = OverloadResolution.ComparisonType.ParameterSpecificty;
													bool flag2 = false;
													Symbols.Method method5 = OverloadResolution.MoreSpecificProcedure(method3, method4, Arguments, ArgumentNames, comparisonType, ref flag2, true);
													if (method5 == method)
													{
														if (!method2.LessSpecific)
														{
															method2.LessSpecific = true;
															CandidateCount--;
														}
													}
													else
													{
														flag = false;
														if (method5 == method2 && !method.LessSpecific)
														{
															method.LessSpecific = true;
															CandidateCount--;
														}
													}
												}
											}
										}
									}
								}
								finally
								{
									List<Symbols.Method>.Enumerator enumerator2;
									((IDisposable)enumerator2).Dispose();
								}
								if (flag)
								{
									return method;
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
				return null;
			}
		}

		private static Symbols.Method RemoveRedundantGenericProcedures(List<Symbols.Method> Candidates, ref int CandidateCount, object[] Arguments, string[] ArgumentNames)
		{
			int num = 0;
			checked
			{
				int num2 = Candidates.Count - 1;
				for (int i = num; i <= num2; i++)
				{
					Symbols.Method method = Candidates[i];
					if (!method.NotCallable)
					{
						int num3 = i + 1;
						int num4 = Candidates.Count - 1;
						for (int j = num3; j <= num4; j++)
						{
							Symbols.Method method2 = Candidates[j];
							if (!method2.NotCallable && method.RequiresNarrowingConversion == method2.RequiresNarrowingConversion)
							{
								Symbols.Method method3 = null;
								bool flag = false;
								if (method.IsGeneric || method2.IsGeneric)
								{
									method3 = OverloadResolution.MoreSpecificProcedure(method, method2, Arguments, ArgumentNames, OverloadResolution.ComparisonType.GenericSpecificityBasedOnMethodGenericParams, ref flag, false);
									if (method3 != null)
									{
										CandidateCount--;
										if (CandidateCount == 1)
										{
											return method3;
										}
										if (method3 != method)
										{
											method.NotCallable = true;
											break;
										}
										method2.NotCallable = true;
									}
								}
								if (!flag && method3 == null && (Symbols.IsGeneric(method.DeclaringType) || Symbols.IsGeneric(method2.DeclaringType)))
								{
									method3 = OverloadResolution.MoreSpecificProcedure(method, method2, Arguments, ArgumentNames, OverloadResolution.ComparisonType.GenericSpecificityBasedOnTypeGenericParams, ref flag, false);
									if (method3 != null)
									{
										CandidateCount--;
										if (CandidateCount == 1)
										{
											return method3;
										}
										if (method3 != method)
										{
											method.NotCallable = true;
											break;
										}
										method2.NotCallable = true;
									}
								}
							}
						}
					}
				}
				return null;
			}
		}

		private static void ReportError(List<string> Errors, string ResourceID, string Substitution1, Type Substitution2, Type Substitution3)
		{
			Errors.Add(Utils.GetResourceString(ResourceID, new string[]
			{
				Substitution1,
				Utils.VBFriendlyName(Substitution2),
				Utils.VBFriendlyName(Substitution3)
			}));
		}

		private static void ReportError(List<string> Errors, string ResourceID, string Substitution1, Symbols.Method Substitution2)
		{
			Errors.Add(Utils.GetResourceString(ResourceID, new string[]
			{
				Substitution1,
				Substitution2.ToString()
			}));
		}

		private static void ReportError(List<string> Errors, string ResourceID, string Substitution1)
		{
			Errors.Add(Utils.GetResourceString(ResourceID, new string[] { Substitution1 }));
		}

		private static void ReportError(List<string> Errors, string ResourceID)
		{
			Errors.Add(Utils.GetResourceString(ResourceID));
		}

		private static Exception ReportOverloadResolutionFailure(string OverloadedProcedureName, List<Symbols.Method> Candidates, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, string ErrorID, OverloadResolution.ResolutionFailure Failure, OverloadResolution.ArgumentDetector Detector, OverloadResolution.CandidateProperty CandidateFilter)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<string> list = new List<string>();
			int num = 0;
			int num2 = 0;
			checked
			{
				int num3 = Candidates.Count - 1;
				for (int i = num2; i <= num3; i++)
				{
					Symbols.Method method = Candidates[i];
					if (CandidateFilter(method))
					{
						if (method.HasParamArray)
						{
							for (int j = i + 1; j < Candidates.Count; j++)
							{
								if (CandidateFilter(Candidates[j]) && Candidates[j] == method)
								{
									goto IL_0101;
								}
							}
						}
						num++;
						list.Clear();
						bool flag = Detector(method, Arguments, ArgumentNames, TypeArguments, list);
						stringBuilder.Append("\r\n    '");
						stringBuilder.Append(method.ToString());
						stringBuilder.Append("':");
						try
						{
							foreach (string text in list)
							{
								stringBuilder.Append("\r\n        ");
								stringBuilder.Append(text);
							}
						}
						finally
						{
							List<string>.Enumerator enumerator;
							((IDisposable)enumerator).Dispose();
						}
					}
					IL_0101:;
				}
				string resourceString = Utils.GetResourceString(ErrorID, new string[]
				{
					OverloadedProcedureName,
					stringBuilder.ToString()
				});
				if (num == 1)
				{
					return new InvalidCastException(resourceString);
				}
				return new AmbiguousMatchException(resourceString);
			}
		}

		private static bool DetectArgumentErrors(Symbols.Method TargetProcedure, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, List<string> Errors)
		{
			return OverloadResolution.CanMatchArguments(TargetProcedure, Arguments, ArgumentNames, TypeArguments, false, Errors);
		}

		private static bool CandidateIsNotCallable(Symbols.Method Candidate)
		{
			return Candidate.NotCallable;
		}

		private static Exception ReportUncallableProcedures(string OverloadedProcedureName, List<Symbols.Method> Candidates, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, OverloadResolution.ResolutionFailure Failure)
		{
			return OverloadResolution.ReportOverloadResolutionFailure(OverloadedProcedureName, Candidates, Arguments, ArgumentNames, TypeArguments, "NoCallableOverloadCandidates2", Failure, new OverloadResolution.ArgumentDetector(OverloadResolution.DetectArgumentErrors), new OverloadResolution.CandidateProperty(OverloadResolution.CandidateIsNotCallable));
		}

		private static bool DetectArgumentNarrowing(Symbols.Method TargetProcedure, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, List<string> Errors)
		{
			return OverloadResolution.CanMatchArguments(TargetProcedure, Arguments, ArgumentNames, TypeArguments, true, Errors);
		}

		private static bool CandidateIsNarrowing(Symbols.Method Candidate)
		{
			return !Candidate.NotCallable && Candidate.RequiresNarrowingConversion;
		}

		private static Exception ReportNarrowingProcedures(string OverloadedProcedureName, List<Symbols.Method> Candidates, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, OverloadResolution.ResolutionFailure Failure)
		{
			return OverloadResolution.ReportOverloadResolutionFailure(OverloadedProcedureName, Candidates, Arguments, ArgumentNames, TypeArguments, "NoNonNarrowingOverloadCandidates2", Failure, new OverloadResolution.ArgumentDetector(OverloadResolution.DetectArgumentNarrowing), new OverloadResolution.CandidateProperty(OverloadResolution.CandidateIsNarrowing));
		}

		private static bool DetectUnspecificity(Symbols.Method TargetProcedure, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, List<string> Errors)
		{
			OverloadResolution.ReportError(Errors, "NotMostSpecificOverload");
			return false;
		}

		private static bool CandidateIsUnspecific(Symbols.Method Candidate)
		{
			return !Candidate.NotCallable && !Candidate.RequiresNarrowingConversion && !Candidate.LessSpecific;
		}

		private static Exception ReportUnspecificProcedures(string OverloadedProcedureName, List<Symbols.Method> Candidates, OverloadResolution.ResolutionFailure Failure)
		{
			return OverloadResolution.ReportOverloadResolutionFailure(OverloadedProcedureName, Candidates, null, null, null, "NoMostSpecificOverload2", Failure, new OverloadResolution.ArgumentDetector(OverloadResolution.DetectUnspecificity), new OverloadResolution.CandidateProperty(OverloadResolution.CandidateIsUnspecific));
		}

		internal static Symbols.Method ResolveOverloadedCall(string MethodName, List<Symbols.Method> Candidates, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, BindingFlags LookupFlags, bool ReportErrors, ref OverloadResolution.ResolutionFailure Failure)
		{
			Failure = OverloadResolution.ResolutionFailure.None;
			int num = Candidates.Count;
			bool flag = false;
			Symbols.Method method = OverloadResolution.RejectUncallableProcedures(Candidates, Arguments, ArgumentNames, TypeArguments, ref num, ref flag);
			if (num == 1)
			{
				return method;
			}
			checked
			{
				if (num == 0)
				{
					Failure = OverloadResolution.ResolutionFailure.InvalidArgument;
					if (ReportErrors)
					{
						throw OverloadResolution.ReportUncallableProcedures(MethodName, Candidates, Arguments, ArgumentNames, TypeArguments, Failure);
					}
					return null;
				}
				else
				{
					if (flag)
					{
						method = OverloadResolution.RemoveRedundantGenericProcedures(Candidates, ref num, Arguments, ArgumentNames);
						if (num == 1)
						{
							return method;
						}
					}
					int num2 = 0;
					Symbols.Method method2 = null;
					try
					{
						foreach (Symbols.Method method3 in Candidates)
						{
							if (!method3.NotCallable)
							{
								if (method3.RequiresNarrowingConversion)
								{
									num--;
									if (method3.AllNarrowingIsFromObject)
									{
										num2++;
										method2 = method3;
									}
								}
								else
								{
									method = method3;
								}
							}
						}
					}
					finally
					{
						List<Symbols.Method>.Enumerator enumerator;
						((IDisposable)enumerator).Dispose();
					}
					if (num == 1)
					{
						return method;
					}
					if (num == 0)
					{
						if (num2 == 1)
						{
							return method2;
						}
						Failure = OverloadResolution.ResolutionFailure.AmbiguousMatch;
						if (ReportErrors)
						{
							throw OverloadResolution.ReportNarrowingProcedures(MethodName, Candidates, Arguments, ArgumentNames, TypeArguments, Failure);
						}
						return null;
					}
					else
					{
						method = OverloadResolution.MostSpecificProcedure(Candidates, ref num, Arguments, ArgumentNames);
						if (method != null)
						{
							return method;
						}
						Failure = OverloadResolution.ResolutionFailure.AmbiguousMatch;
						if (ReportErrors)
						{
							throw OverloadResolution.ReportUnspecificProcedures(MethodName, Candidates, Failure);
						}
						return null;
					}
				}
			}
		}

		internal static Symbols.Method ResolveOverloadedCall(string MethodName, MemberInfo[] Members, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, BindingFlags LookupFlags, bool ReportErrors, ref OverloadResolution.ResolutionFailure Failure)
		{
			int num = 0;
			int num2 = 0;
			List<Symbols.Method> list = OverloadResolution.CollectOverloadCandidates(Members, Arguments, Arguments.Length, ArgumentNames, TypeArguments, false, null, ref num, ref num2);
			if (list.Count == 1 && !list[0].NotCallable)
			{
				return list[0];
			}
			if (list.Count != 0)
			{
				return OverloadResolution.ResolveOverloadedCall(MethodName, list, Arguments, ArgumentNames, TypeArguments, LookupFlags, ReportErrors, ref Failure);
			}
			Failure = OverloadResolution.ResolutionFailure.MissingMember;
			if (ReportErrors)
			{
				string text = "NoViableOverloadCandidates1";
				if (num > 0)
				{
					text = "NoArgumentCountOverloadCandidates1";
				}
				else if (num2 > 0)
				{
					text = "NoTypeArgumentCountOverloadCandidates1";
				}
				throw new MissingMemberException(Utils.GetResourceString(text, new string[] { MethodName }));
			}
			return null;
		}

		internal enum ResolutionFailure
		{
			None,
			MissingMember,
			InvalidArgument,
			AmbiguousMatch,
			InvalidTarget
		}

		private enum ComparisonType
		{
			ParameterSpecificty,
			GenericSpecificityBasedOnMethodGenericParams,
			GenericSpecificityBasedOnTypeGenericParams
		}

		private delegate bool ArgumentDetector(Symbols.Method TargetProcedure, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, List<string> Errors);

		private delegate bool CandidateProperty(Symbols.Method Candidate);
	}
}

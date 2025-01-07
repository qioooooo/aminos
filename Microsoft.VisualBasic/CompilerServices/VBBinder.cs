using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security.Permissions;

namespace Microsoft.VisualBasic.CompilerServices
{
	internal sealed class VBBinder : Binder
	{
		private void ThrowInvalidCast(Type ArgType, Type ParmType, int ParmIndex)
		{
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromToArg4", new string[]
			{
				this.CalledMethodName(),
				Conversions.ToString(checked(ParmIndex + 1)),
				Utils.VBFriendlyName(ArgType),
				Utils.VBFriendlyName(ParmType)
			}));
		}

		public VBBinder(bool[] CopyBack)
		{
			this.m_ByRefFlags = CopyBack;
		}

		public override void ReorderArgumentArray(ref object[] args, object objState)
		{
			VBBinder.VBBinderState vbbinderState = (VBBinder.VBBinderState)objState;
			checked
			{
				if (args != null)
				{
					if (vbbinderState != null)
					{
						if (vbbinderState.m_OriginalParamOrder != null)
						{
							if (this.m_ByRefFlags != null)
							{
								if (vbbinderState.m_ByRefFlags == null)
								{
									int num = 0;
									int upperBound = this.m_ByRefFlags.GetUpperBound(0);
									for (int i = num; i <= upperBound; i++)
									{
										this.m_ByRefFlags[i] = false;
									}
								}
								else
								{
									int num2 = 0;
									int upperBound2 = vbbinderState.m_OriginalParamOrder.GetUpperBound(0);
									for (int i = num2; i <= upperBound2; i++)
									{
										int num3 = vbbinderState.m_OriginalParamOrder[i];
										if (num3 >= 0 && num3 <= args.GetUpperBound(0))
										{
											this.m_ByRefFlags[num3] = vbbinderState.m_ByRefFlags[num3];
											vbbinderState.m_OriginalArgs[num3] = args[i];
										}
									}
								}
							}
						}
						else if (this.m_ByRefFlags != null)
						{
							if (vbbinderState.m_ByRefFlags == null)
							{
								int num4 = 0;
								int upperBound3 = this.m_ByRefFlags.GetUpperBound(0);
								for (int i = num4; i <= upperBound3; i++)
								{
									this.m_ByRefFlags[i] = false;
								}
							}
							else
							{
								int num5 = 0;
								int upperBound4 = this.m_ByRefFlags.GetUpperBound(0);
								for (int i = num5; i <= upperBound4; i++)
								{
									if (this.m_ByRefFlags[i])
									{
										bool flag = vbbinderState.m_ByRefFlags[i];
										this.m_ByRefFlags[i] = flag;
										if (flag)
										{
											vbbinderState.m_OriginalArgs[i] = args[i];
										}
									}
								}
							}
						}
					}
				}
				if (vbbinderState != null)
				{
					vbbinderState.m_OriginalParamOrder = null;
					vbbinderState.m_ByRefFlags = null;
				}
			}
		}

		public override MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] names, ref object ObjState)
		{
			Type type = null;
			Type type2 = null;
			Type type3 = null;
			if (match == null || match.Length == 0)
			{
				throw ExceptionUtils.VbMakeException(438);
			}
			if (this.m_CachedMember != null && this.m_CachedMember.MemberType == MemberTypes.Method && match[0] != null && Operators.CompareString(match[0].Name, this.m_CachedMember.Name, false) == 0)
			{
				return (MethodBase)this.m_CachedMember;
			}
			bool flag = (bindingAttr & BindingFlags.SetProperty) != BindingFlags.Default;
			if (names != null && names.Length == 0)
			{
				names = null;
			}
			int num = match.Length;
			checked
			{
				if (num > 1)
				{
					int num2 = 0;
					int upperBound = match.GetUpperBound(0);
					for (int i = num2; i <= upperBound; i++)
					{
						MethodBase methodBase = match[i];
						if (methodBase != null)
						{
							if (!methodBase.IsHideBySig)
							{
								if (methodBase.IsVirtual)
								{
									if ((methodBase.Attributes & MethodAttributes.VtableLayoutMask) != MethodAttributes.PrivateScope)
									{
										int num3 = 0;
										int upperBound2 = match.GetUpperBound(0);
										for (int j = num3; j <= upperBound2; j++)
										{
											if (i != j && match[j] != null && methodBase.DeclaringType.IsSubclassOf(match[j].DeclaringType))
											{
												match[j] = null;
												num--;
											}
										}
									}
								}
								else
								{
									int num4 = 0;
									int upperBound3 = match.GetUpperBound(0);
									for (int k = num4; k <= upperBound3; k++)
									{
										if (i != k && match[k] != null && methodBase.DeclaringType.IsSubclassOf(match[k].DeclaringType))
										{
											match[k] = null;
											num--;
										}
									}
								}
							}
						}
					}
				}
				int num5 = num;
				int num8;
				int m;
				if (names != null)
				{
					int num6 = 0;
					int upperBound4 = match.GetUpperBound(0);
					for (int i = num6; i <= upperBound4; i++)
					{
						MethodBase methodBase = match[i];
						if (methodBase != null)
						{
							ParameterInfo[] array = methodBase.GetParameters();
							int num7 = array.GetUpperBound(0);
							if (flag)
							{
								num7--;
							}
							if (num7 >= 0)
							{
								ParameterInfo parameterInfo = array[num7];
								num8 = -1;
								if (parameterInfo.ParameterType.IsArray)
								{
									object[] customAttributes = parameterInfo.GetCustomAttributes(typeof(ParamArrayAttribute), false);
									if (customAttributes != null && customAttributes.Length > 0)
									{
										num8 = num7;
									}
									else
									{
										num8 = -1;
									}
								}
							}
							int num9 = 0;
							int upperBound5 = names.GetUpperBound(0);
							int l = num9;
							while (l <= upperBound5)
							{
								int num10 = 0;
								int num11 = num7;
								m = num10;
								while (m <= num11)
								{
									if (Strings.StrComp(names[l], array[m].Name, CompareMethod.Text) == 0)
									{
										if (m == num8 && num == 1)
										{
											throw ExceptionUtils.VbMakeExceptionEx(446, Utils.GetResourceString("NamedArgumentOnParamArray"));
										}
										if (m == num8)
										{
											m = num7 + 1;
											break;
										}
										break;
									}
									else
									{
										m++;
									}
								}
								if (m > num7)
								{
									if (num == 1)
									{
										throw new MissingMemberException(Utils.GetResourceString("Argument_InvalidNamedArg2", new string[]
										{
											names[l],
											this.CalledMethodName()
										}));
									}
									match[i] = null;
									num--;
									break;
								}
								else
								{
									l++;
								}
							}
						}
					}
				}
				int[] array2 = new int[match.Length - 1 + 1];
				int num12 = 0;
				int upperBound6 = match.GetUpperBound(0);
				for (int i = num12; i <= upperBound6; i++)
				{
					MethodBase methodBase = match[i];
					if (methodBase != null)
					{
						num8 = -1;
						ParameterInfo[] array = methodBase.GetParameters();
						int num7 = array.GetUpperBound(0);
						if (flag)
						{
							num7--;
						}
						if (num7 >= 0)
						{
							ParameterInfo parameterInfo = array[num7];
							if (parameterInfo.ParameterType.IsArray)
							{
								object[] customAttributes2 = parameterInfo.GetCustomAttributes(typeof(ParamArrayAttribute), false);
								if (customAttributes2 != null && customAttributes2.Length > 0)
								{
									num8 = num7;
								}
							}
						}
						array2[i] = num8;
						if (num8 == -1 && args.Length > array.Length)
						{
							if (num == 1)
							{
								throw new MissingMemberException(Utils.GetResourceString("NoMethodTakingXArguments2", new string[]
								{
									this.CalledMethodName(),
									Conversions.ToString(this.GetPropArgCount(args, flag))
								}));
							}
							match[i] = null;
							num--;
						}
						int num13 = num7;
						if (num8 != -1)
						{
							num13--;
						}
						if (args.Length < num13)
						{
							int num14 = args.Length;
							int num15 = num13 - 1;
							int n;
							for (n = num14; n <= num15; n++)
							{
								if (array[n].DefaultValue == DBNull.Value)
								{
									break;
								}
							}
							if (n != num13)
							{
								if (num == 1)
								{
									throw new MissingMemberException(Utils.GetResourceString("NoMethodTakingXArguments2", new string[]
									{
										this.CalledMethodName(),
										Conversions.ToString(this.GetPropArgCount(args, flag))
									}));
								}
								match[i] = null;
								num--;
							}
						}
					}
				}
				object[] array3 = new object[match.Length - 1 + 1];
				int num16 = 0;
				int upperBound7 = match.GetUpperBound(0);
				int[] array4;
				for (int i = num16; i <= upperBound7; i++)
				{
					MethodBase methodBase = match[i];
					if (methodBase != null)
					{
						ParameterInfo[] array = methodBase.GetParameters();
						if (args.Length > array.Length)
						{
							array4 = new int[args.Length - 1 + 1];
						}
						else
						{
							array4 = new int[array.Length - 1 + 1];
						}
						array3[i] = array4;
						if (names == null)
						{
							int num17 = args.GetUpperBound(0);
							if (flag)
							{
								num17--;
							}
							int num18 = 0;
							int num19 = num17;
							int num20;
							for (num20 = num18; num20 <= num19; num20++)
							{
								if (args[num20] is Missing && (num20 > array.GetUpperBound(0) || array[num20].IsOptional))
								{
									array4[num20] = -1;
								}
								else
								{
									array4[num20] = num20;
								}
							}
							num17 = array4.GetUpperBound(0);
							int num21 = num20;
							int num22 = num17;
							for (num20 = num21; num20 <= num22; num20++)
							{
								array4[num20] = -1;
							}
							if (flag)
							{
								array4[num17] = args.GetUpperBound(0);
							}
						}
						else
						{
							Exception ex = this.CreateParamOrder(flag, array4, methodBase.GetParameters(), args, names);
							if (ex != null)
							{
								if (num == 1)
								{
									throw ex;
								}
								match[i] = null;
								num--;
							}
						}
					}
				}
				Type[] array5 = new Type[args.Length - 1 + 1];
				int num23 = 0;
				int upperBound8 = args.GetUpperBound(0);
				for (int num20 = num23; num20 <= upperBound8; num20++)
				{
					if (args[num20] != null)
					{
						array5[num20] = args[num20].GetType();
					}
				}
				int num24 = 0;
				int upperBound9 = match.GetUpperBound(0);
				for (int i = num24; i <= upperBound9; i++)
				{
					MethodBase methodBase = match[i];
					if (methodBase != null)
					{
						ParameterInfo[] array = methodBase.GetParameters();
						array4 = (int[])array3[i];
						int num7 = array4.GetUpperBound(0);
						if (flag)
						{
							num7--;
						}
						num8 = array2[i];
						if (num8 != -1)
						{
							type3 = array[num8].ParameterType.GetElementType();
						}
						else if (array4.Length > array.Length)
						{
							goto IL_08D3;
						}
						int num25 = 0;
						int num26 = num7;
						for (m = num25; m <= num26; m++)
						{
							int num20 = array4[m];
							if (num20 == -1)
							{
								if (!array[m].IsOptional && m != array2[i])
								{
									if (num == 1)
									{
										throw new MissingMemberException(Utils.GetResourceString("NoMethodTakingXArguments2", new string[]
										{
											this.CalledMethodName(),
											Conversions.ToString(this.GetPropArgCount(args, flag))
										}));
									}
									goto IL_08D3;
								}
							}
							else
							{
								type = array5[num20];
								if (type != null)
								{
									if (num8 != -1 && m > num8)
									{
										type2 = array[num8].ParameterType.GetElementType();
									}
									else
									{
										type2 = array[m].ParameterType;
										if (type2.IsByRef)
										{
											type2 = type2.GetElementType();
										}
										if (m == num8)
										{
											if (type2.IsInstanceOfType(args[num20]) && m == num7)
											{
												goto IL_08C2;
											}
											type2 = type3;
										}
									}
									if (type2 != type)
									{
										if (type != Type.Missing || !array[m].IsOptional)
										{
											if (args[num20] == Missing.Value)
											{
												goto IL_08D3;
											}
											if (type2 != typeof(object))
											{
												if (!type2.IsInstanceOfType(args[num20]))
												{
													TypeCode typeCode = Type.GetTypeCode(type2);
													TypeCode typeCode2;
													if (type == null)
													{
														typeCode2 = TypeCode.Empty;
													}
													else
													{
														typeCode2 = Type.GetTypeCode(type);
													}
													switch (typeCode)
													{
													case TypeCode.Boolean:
													case TypeCode.Byte:
													case TypeCode.Int16:
													case TypeCode.Int32:
													case TypeCode.Int64:
													case TypeCode.Single:
													case TypeCode.Double:
													case TypeCode.Decimal:
														switch (typeCode2)
														{
														case TypeCode.Boolean:
														case TypeCode.Byte:
														case TypeCode.Int16:
														case TypeCode.Int32:
														case TypeCode.Int64:
														case TypeCode.Single:
														case TypeCode.Double:
														case TypeCode.Decimal:
														case TypeCode.String:
															goto IL_08C2;
														case TypeCode.Char:
														case TypeCode.SByte:
														case TypeCode.UInt16:
														case TypeCode.UInt32:
														case TypeCode.UInt64:
														case TypeCode.DateTime:
														case (TypeCode)17:
															goto IL_08D3;
														default:
															goto IL_08D3;
														}
														break;
													case TypeCode.Char:
													{
														TypeCode typeCode3 = typeCode2;
														if (typeCode3 == TypeCode.String)
														{
															goto IL_08C2;
														}
														goto IL_08D3;
													}
													case TypeCode.DateTime:
													{
														TypeCode typeCode4 = typeCode2;
														if (typeCode4 == TypeCode.String)
														{
															goto IL_08C2;
														}
														goto IL_08D3;
													}
													case TypeCode.String:
														switch (typeCode2)
														{
														case TypeCode.Empty:
														case TypeCode.Boolean:
														case TypeCode.Char:
														case TypeCode.Byte:
														case TypeCode.Int16:
														case TypeCode.Int32:
														case TypeCode.Int64:
														case TypeCode.Single:
														case TypeCode.Double:
														case TypeCode.Decimal:
														case TypeCode.String:
															goto IL_08C2;
														}
														if (type == typeof(char[]))
														{
															goto IL_08C2;
														}
														goto IL_08D3;
													}
													if (type2 != typeof(char[]))
													{
														goto IL_08D3;
													}
													TypeCode typeCode5 = typeCode2;
													if (typeCode5 != TypeCode.String)
													{
														if (typeCode5 != TypeCode.Object)
														{
															goto IL_08D3;
														}
														if (type != typeof(char[]))
														{
															goto IL_08D3;
														}
													}
												}
											}
										}
									}
								}
							}
							IL_08C2:;
						}
						goto IL_0919;
						IL_08D3:
						if (num == 1)
						{
							if (num5 != 1)
							{
								throw new AmbiguousMatchException(Utils.GetResourceString("AmbiguousMatch_NarrowingConversion1", new string[] { this.CalledMethodName() }));
							}
							this.ThrowInvalidCast(type, type2, m);
						}
						match[i] = null;
						num--;
					}
					IL_0919:;
				}
				num = 0;
				int num27 = 0;
				int upperBound10 = match.GetUpperBound(0);
				for (int i = num27; i <= upperBound10; i++)
				{
					MethodBase methodBase = match[i];
					if (methodBase != null)
					{
						array4 = (int[])array3[i];
						ParameterInfo[] array = methodBase.GetParameters();
						bool flag2 = false;
						int num7 = array.GetUpperBound(0);
						if (flag)
						{
							num7--;
						}
						int num28 = args.GetUpperBound(0);
						if (flag)
						{
							num28--;
						}
						num8 = array2[i];
						if (num8 != -1)
						{
							type3 = array[num7].ParameterType.GetElementType();
						}
						int num29 = 0;
						int num30 = num7;
						for (m = num29; m <= num30; m++)
						{
							if (m == num8)
							{
								type2 = type3;
							}
							else
							{
								type2 = array[m].ParameterType;
							}
							if (type2.IsByRef)
							{
								flag2 = true;
								type2 = type2.GetElementType();
							}
							int num20 = array4[m];
							if (num20 != -1 || !array[m].IsOptional)
							{
								if (m != array2[i])
								{
									type = array5[num20];
									if (type != null)
									{
										if (type != Type.Missing || !array[m].IsOptional)
										{
											if (type2 != type)
											{
												if (type2 != typeof(object))
												{
													TypeCode typeCode6 = Type.GetTypeCode(type2);
													TypeCode typeCode7;
													if (type == null)
													{
														typeCode7 = TypeCode.Empty;
													}
													else
													{
														typeCode7 = Type.GetTypeCode(type);
													}
													switch (typeCode6)
													{
													case TypeCode.Boolean:
													case TypeCode.Byte:
													case TypeCode.Int16:
													case TypeCode.Int32:
													case TypeCode.Int64:
													case TypeCode.Single:
													case TypeCode.Double:
													case TypeCode.Decimal:
														switch (typeCode7)
														{
														case TypeCode.Boolean:
														case TypeCode.Byte:
														case TypeCode.Int16:
														case TypeCode.Int32:
														case TypeCode.Int64:
														case TypeCode.Single:
														case TypeCode.Double:
														case TypeCode.Decimal:
														case TypeCode.String:
															goto IL_0B0F;
														}
														if (num == 0)
														{
															this.ThrowInvalidCast(type, type2, m);
														}
														break;
													}
												}
											}
										}
									}
								}
							}
							IL_0B0F:;
						}
						if (m > num7)
						{
							if (i != num)
							{
								match[num] = match[i];
								array3[num] = array3[i];
								array2[num] = array2[i];
								match[i] = null;
							}
							num++;
							if (flag2)
							{
							}
						}
						else
						{
							match[i] = null;
						}
					}
				}
				if (num == 0)
				{
					throw new MissingMemberException(Utils.GetResourceString("NoMethodTakingXArguments2", new string[]
					{
						this.CalledMethodName(),
						Conversions.ToString(this.GetPropArgCount(args, flag))
					}));
				}
				VBBinder.VBBinderState vbbinderState = new VBBinder.VBBinderState();
				this.m_state = vbbinderState;
				ObjState = vbbinderState;
				vbbinderState.m_OriginalArgs = args;
				int num31;
				if (num == 1)
				{
					num31 = 0;
				}
				else
				{
					num31 = 0;
					VBBinder.BindScore bindScore = VBBinder.BindScore.Unknown;
					int num32 = 0;
					int num33 = 0;
					int num34 = num - 1;
					for (int i = num33; i <= num34; i++)
					{
						MethodBase methodBase = match[i];
						if (methodBase != null)
						{
							array4 = (int[])array3[i];
							VBBinder.BindScore bindScore2 = this.BindingScore(methodBase.GetParameters(), array4, array5, flag, array2[i]);
							if (bindScore2 < bindScore)
							{
								if (i != 0)
								{
									match[0] = match[i];
									array3[0] = array3[i];
									array2[0] = array2[i];
									match[i] = null;
								}
								num32 = 1;
								bindScore = bindScore2;
							}
							else if (bindScore2 == bindScore)
							{
								if (bindScore2 == VBBinder.BindScore.Exact || bindScore2 == VBBinder.BindScore.Widening1)
								{
									int mostSpecific = this.GetMostSpecific(match[0], methodBase, array4, array3, flag, array2[0], array2[i], args);
									if (mostSpecific == -1)
									{
										if (num32 != i)
										{
											match[num32] = match[i];
											array3[num32] = array3[i];
											array2[num32] = array2[i];
											match[i] = null;
										}
										num32++;
									}
									else if (mostSpecific != 0)
									{
										bool flag3 = true;
										int num35 = 1;
										int num36 = num32 - 1;
										for (int num37 = num35; num37 <= num36; num37++)
										{
											if (this.GetMostSpecific(match[num37], methodBase, array4, array3, flag, array2[num37], array2[i], args) != 1)
											{
												flag3 = false;
												break;
											}
										}
										if (flag3)
										{
											num32 = 0;
										}
										if (i != num32)
										{
											match[num32] = match[i];
											array3[num32] = array3[i];
											array2[num32] = array2[i];
											match[i] = null;
										}
										num32++;
									}
								}
								else
								{
									if (num32 != i)
									{
										match[num32] = match[i];
										array3[num32] = array3[i];
										array2[num32] = array2[i];
										match[i] = null;
									}
									num32++;
								}
							}
							else
							{
								match[i] = null;
							}
						}
					}
					if (num32 > 1)
					{
						int num38 = 0;
						int upperBound11 = match.GetUpperBound(0);
						for (int i = num38; i <= upperBound11; i++)
						{
							MethodBase methodBase = match[i];
							if (methodBase != null)
							{
								int num39 = 0;
								int upperBound12 = match.GetUpperBound(0);
								for (int num40 = num39; num40 <= upperBound12; num40++)
								{
									if (i != num40 && match[num40] != null && (methodBase == match[num40] || (methodBase.DeclaringType.IsSubclassOf(match[num40].DeclaringType) && this.MethodsDifferOnlyByReturnType(methodBase, match[num40]))))
									{
										match[num40] = null;
										num32--;
									}
								}
							}
						}
						int num41 = 0;
						int upperBound13 = match.GetUpperBound(0);
						for (int i = num41; i <= upperBound13; i++)
						{
							if (match[i] == null)
							{
								int num42 = i + 1;
								int upperBound14 = match.GetUpperBound(0);
								for (int num43 = num42; num43 <= upperBound14; num43++)
								{
									MethodBase methodBase2 = match[num43];
									if (methodBase2 != null)
									{
										match[i] = methodBase2;
										array3[i] = array3[num43];
										array2[i] = array2[num43];
										match[num43] = null;
									}
								}
							}
						}
					}
					if (num32 > 1)
					{
						string text = "\r\n    " + Utils.MethodToString(match[0]);
						int num44 = 1;
						int num45 = num32 - 1;
						for (int i = num44; i <= num45; i++)
						{
							text = text + "\r\n    " + Utils.MethodToString(match[i]);
						}
						switch (bindScore)
						{
						case VBBinder.BindScore.Exact:
							throw new AmbiguousMatchException(Utils.GetResourceString("AmbiguousCall_ExactMatch2", new string[]
							{
								this.CalledMethodName(),
								text
							}));
						case VBBinder.BindScore.Widening0:
						case VBBinder.BindScore.Widening1:
							throw new AmbiguousMatchException(Utils.GetResourceString("AmbiguousCall_WideningConversion2", new string[]
							{
								this.CalledMethodName(),
								text
							}));
						default:
							throw new AmbiguousMatchException(Utils.GetResourceString("AmbiguousCall2", new string[]
							{
								this.CalledMethodName(),
								text
							}));
						}
					}
				}
				MethodBase methodBase3 = match[num31];
				array4 = (int[])array3[num31];
				if (names != null)
				{
					this.ReorderParams(array4, args, vbbinderState);
				}
				ParameterInfo[] parameters = methodBase3.GetParameters();
				if (args.Length > 0)
				{
					vbbinderState.m_ByRefFlags = new bool[args.GetUpperBound(0) + 1];
					bool flag4 = false;
					int num46 = 0;
					int upperBound15 = parameters.GetUpperBound(0);
					for (m = num46; m <= upperBound15; m++)
					{
						if (parameters[m].ParameterType.IsByRef)
						{
							if (vbbinderState.m_OriginalParamOrder == null)
							{
								if (m < vbbinderState.m_ByRefFlags.Length)
								{
									vbbinderState.m_ByRefFlags[m] = true;
								}
							}
							else if (m < vbbinderState.m_OriginalParamOrder.Length)
							{
								int num47 = vbbinderState.m_OriginalParamOrder[m];
								if (num47 >= 0)
								{
									vbbinderState.m_ByRefFlags[num47] = true;
								}
							}
							flag4 = true;
						}
					}
					if (!flag4)
					{
						vbbinderState.m_ByRefFlags = null;
					}
				}
				else
				{
					vbbinderState.m_ByRefFlags = null;
				}
				num8 = array2[num31];
				if (num8 != -1)
				{
					int num7 = parameters.GetUpperBound(0);
					if (flag)
					{
						num7--;
					}
					int num28 = args.GetUpperBound(0);
					if (flag)
					{
						num28--;
					}
					object[] array6 = new object[parameters.Length - 1 + 1];
					int num48 = 0;
					int num49 = Math.Min(num28, num8) - 1;
					for (m = num48; m <= num49; m++)
					{
						array6[m] = ObjectType.CTypeHelper(args[m], parameters[m].ParameterType);
					}
					if (num28 < num8)
					{
						int num50 = num28 + 1;
						int num51 = num8 - 1;
						for (m = num50; m <= num51; m++)
						{
							array6[m] = ObjectType.CTypeHelper(parameters[m].DefaultValue, parameters[m].ParameterType);
						}
					}
					if (flag)
					{
						int upperBound16 = array6.GetUpperBound(0);
						array6[upperBound16] = ObjectType.CTypeHelper(args[args.GetUpperBound(0)], parameters[upperBound16].ParameterType);
					}
					if (num28 == -1)
					{
						array6[num8] = Array.CreateInstance(type3, 0);
					}
					else
					{
						type3 = parameters[num7].ParameterType.GetElementType();
						int num52 = args.Length - parameters.Length + 1;
						type2 = parameters[num7].ParameterType;
						if (num52 == 1 && type2.IsArray && (args[num8] == null || type2.IsInstanceOfType(args[num8])))
						{
							array6[num8] = args[num8];
						}
						else if (type3 == typeof(object))
						{
							object[] array7 = new object[num52 - 1 + 1];
							int num53 = 0;
							int num54 = num52 - 1;
							for (int num20 = num53; num20 <= num54; num20++)
							{
								array7[num20] = ObjectType.CTypeHelper(args[num20 + num8], type3);
							}
							array6[num8] = array7;
						}
						else
						{
							Array array8 = Array.CreateInstance(type3, num52);
							int num55 = 0;
							int num56 = num52 - 1;
							for (int num20 = num55; num20 <= num56; num20++)
							{
								array8.SetValue(ObjectType.CTypeHelper(args[num20 + num8], type3), num20);
							}
							array6[num8] = array8;
						}
					}
					args = array6;
				}
				else
				{
					object[] array9 = new object[parameters.Length - 1 + 1];
					int num57 = 0;
					int upperBound17 = array9.GetUpperBound(0);
					int num20;
					for (num20 = num57; num20 <= upperBound17; num20++)
					{
						int num58 = array4[num20];
						if (num58 >= 0 && num58 <= args.GetUpperBound(0))
						{
							array9[num20] = ObjectType.CTypeHelper(args[num58], parameters[num20].ParameterType);
						}
						else
						{
							array9[num20] = ObjectType.CTypeHelper(parameters[num20].DefaultValue, parameters[num20].ParameterType);
						}
					}
					int num59 = num20;
					int upperBound18 = parameters.GetUpperBound(0);
					for (m = num59; m <= upperBound18; m++)
					{
						array9[m] = ObjectType.CTypeHelper(parameters[m].DefaultValue, parameters[m].ParameterType);
					}
					args = array9;
				}
				if (methodBase3 == null)
				{
					throw new MissingMemberException(Utils.GetResourceString("NoMethodTakingXArguments2", new string[]
					{
						this.CalledMethodName(),
						Conversions.ToString(this.GetPropArgCount(args, flag))
					}));
				}
				return methodBase3;
			}
		}

		private int GetPropArgCount(object[] args, bool IsPropertySet)
		{
			if (IsPropertySet)
			{
				return checked(args.Length - 1);
			}
			return args.Length;
		}

		private int GetMostSpecific(MethodBase match0, MethodBase ThisMethod, int[] ArgIndexes, object[] ParamOrder, bool IsPropertySet, int ParamArrayIndex0, int ParamArrayIndex1, object[] args)
		{
			Type type = null;
			Type type2 = null;
			int num = args.GetUpperBound(0);
			ParameterInfo[] parameters = ThisMethod.GetParameters();
			ParameterInfo[] parameters2 = match0.GetParameters();
			int[] array = (int[])ParamOrder[0];
			int num2 = -1;
			int num3 = args.GetUpperBound(0);
			int num4 = parameters2.GetUpperBound(0);
			int num5 = parameters.GetUpperBound(0);
			checked
			{
				if (IsPropertySet)
				{
					num4--;
					num5--;
					num3--;
					num--;
				}
				bool flag;
				if (ParamArrayIndex0 == -1)
				{
					flag = false;
				}
				else
				{
					type = parameters2[ParamArrayIndex0].ParameterType.GetElementType();
					flag = true;
					if (num3 != -1 && num3 == num4)
					{
						object obj = args[num3];
						if (obj == null || parameters2[num4].ParameterType.IsInstanceOfType(obj))
						{
							flag = false;
						}
					}
				}
				bool flag2;
				if (ParamArrayIndex1 == -1)
				{
					flag2 = false;
				}
				else
				{
					type2 = parameters[ParamArrayIndex1].ParameterType.GetElementType();
					flag2 = true;
					if (num3 != -1 && num3 == num5)
					{
						object obj2 = args[num3];
						if (obj2 == null || parameters[num5].ParameterType.IsInstanceOfType(obj2))
						{
							flag2 = false;
						}
					}
				}
				int num6 = 0;
				int num7 = Math.Min(num, Math.Max(num4, num5));
				for (int i = num6; i <= num7; i++)
				{
					int num8;
					if (i <= num4)
					{
						num8 = array[i];
					}
					else
					{
						num8 = -1;
					}
					int num9;
					if (i <= num5)
					{
						num9 = ArgIndexes[i];
					}
					else
					{
						num9 = -1;
					}
					if (num8 != -1 || num9 != -1)
					{
						if (flag2 && ParamArrayIndex1 != -1 && i >= ParamArrayIndex1)
						{
							Type type3;
							if (flag && ParamArrayIndex0 != -1 && i >= ParamArrayIndex0)
							{
								type3 = type;
							}
							else
							{
								type3 = parameters2[num8].ParameterType;
								if (type3.IsByRef)
								{
									type3 = type3.GetElementType();
								}
							}
							if (type2 == type3)
							{
								if (num2 == -1 && ParamArrayIndex0 == -1 && i == num4 && args[num4] != null)
								{
									num2 = 0;
								}
							}
							else if (ObjectType.IsWideningConversion(type3, type2))
							{
								if (num2 == 1)
								{
									num2 = -1;
									break;
								}
								num2 = 0;
							}
							else if (ObjectType.IsWideningConversion(type2, type3))
							{
								if (num2 == 0)
								{
									num2 = -1;
									break;
								}
								num2 = 1;
							}
						}
						else if (flag && ParamArrayIndex0 != -1 && i >= ParamArrayIndex0)
						{
							Type type4;
							if (flag2 && ParamArrayIndex1 != -1 && i >= ParamArrayIndex1)
							{
								type4 = type2;
							}
							else
							{
								type4 = parameters[num9].ParameterType;
								if (type4.IsByRef)
								{
									type4 = type4.GetElementType();
								}
							}
							if (type == type4)
							{
								if (num2 == -1 && ParamArrayIndex1 == -1 && i == num5 && args[num5] != null)
								{
									num2 = 1;
								}
							}
							else if (ObjectType.IsWideningConversion(type, type4))
							{
								if (num2 == 1)
								{
									num2 = -1;
									break;
								}
								num2 = 0;
							}
							else if (ObjectType.IsWideningConversion(type4, type))
							{
								if (num2 == 0)
								{
									num2 = -1;
									break;
								}
								num2 = 1;
							}
						}
						else
						{
							Type type3 = parameters2[array[i]].ParameterType;
							Type type4 = parameters[ArgIndexes[i]].ParameterType;
							if (type3 != type4)
							{
								if (ObjectType.IsWideningConversion(type3, type4))
								{
									if (num2 == 1)
									{
										num2 = -1;
										break;
									}
									num2 = 0;
								}
								else if (ObjectType.IsWideningConversion(type4, type3))
								{
									if (num2 == 0)
									{
										num2 = -1;
										break;
									}
									num2 = 1;
								}
								else if (ObjectType.IsWiderNumeric(type3, type4))
								{
									if (num2 == 0)
									{
										num2 = -1;
										break;
									}
									num2 = 1;
								}
								else if (ObjectType.IsWiderNumeric(type4, type3))
								{
									if (num2 == 1)
									{
										num2 = -1;
										break;
									}
									num2 = 0;
								}
								else
								{
									num2 = -1;
								}
							}
						}
					}
				}
				if (num2 == -1)
				{
					if ((ParamArrayIndex0 == -1 || !flag) && ParamArrayIndex1 != -1)
					{
						if (flag2 && this.MatchesParamArraySignature(parameters2, parameters, ParamArrayIndex1, IsPropertySet, num))
						{
							num2 = 0;
						}
					}
					else if ((ParamArrayIndex1 == -1 || !flag2) && ParamArrayIndex0 != -1 && flag && this.MatchesParamArraySignature(parameters, parameters2, ParamArrayIndex0, IsPropertySet, num))
					{
						num2 = 1;
					}
				}
				return num2;
			}
		}

		private bool MatchesParamArraySignature(ParameterInfo[] param0, ParameterInfo[] param1, int ParamArrayIndex1, bool IsPropertySet, int ArgCountUpperBound)
		{
			int num = param0.GetUpperBound(0);
			checked
			{
				if (IsPropertySet)
				{
					num--;
				}
				num = Math.Min(num, ArgCountUpperBound);
				int num2 = 0;
				int num3 = num;
				for (int i = num2; i <= num3; i++)
				{
					Type type = param0[i].ParameterType;
					if (type.IsByRef)
					{
						type = type.GetElementType();
					}
					Type type2;
					if (i >= ParamArrayIndex1)
					{
						type2 = param1[ParamArrayIndex1].ParameterType;
						type2 = type2.GetElementType();
					}
					else
					{
						type2 = param1[i].ParameterType;
						if (type2.IsByRef)
						{
							type2 = type2.GetElementType();
						}
					}
					if (type != type2)
					{
						return false;
					}
				}
				return true;
			}
		}

		private bool MethodsDifferOnlyByReturnType(MethodBase match1, MethodBase match2)
		{
			if (match1 == match2)
			{
			}
			ParameterInfo[] parameters = match1.GetParameters();
			ParameterInfo[] parameters2 = match2.GetParameters();
			int num = Math.Min(parameters.GetUpperBound(0), parameters2.GetUpperBound(0));
			int num2 = 0;
			int num3 = num;
			checked
			{
				for (int i = num2; i <= num3; i++)
				{
					Type type = parameters[i].ParameterType;
					if (type.IsByRef)
					{
						type = type.GetElementType();
					}
					Type type2 = parameters2[i].ParameterType;
					if (type2.IsByRef)
					{
						type2 = type2.GetElementType();
					}
					if (type != type2)
					{
						return false;
					}
				}
				if (parameters.Length > parameters2.Length)
				{
					int num4 = num + 1;
					int upperBound = parameters2.GetUpperBound(0);
					for (int i = num4; i <= upperBound; i++)
					{
						if (!parameters[i].IsOptional)
						{
							return false;
						}
					}
				}
				else if (parameters2.Length > parameters.Length)
				{
					int num5 = num + 1;
					int upperBound2 = parameters.GetUpperBound(0);
					for (int i = num5; i <= upperBound2; i++)
					{
						if (!parameters2[i].IsOptional)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		public override FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo culture)
		{
			if (this.m_CachedMember != null && this.m_CachedMember.MemberType == MemberTypes.Field && match[0] != null && Operators.CompareString(match[0].Name, this.m_CachedMember.Name, false) == 0)
			{
				return (FieldInfo)this.m_CachedMember;
			}
			FieldInfo fieldInfo = match[0];
			int num = 1;
			int upperBound = match.GetUpperBound(0);
			checked
			{
				for (int i = num; i <= upperBound; i++)
				{
					if (match[i].DeclaringType.IsSubclassOf(fieldInfo.DeclaringType))
					{
						fieldInfo = match[i];
					}
				}
				return fieldInfo;
			}
		}

		public override MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers)
		{
			VBBinder.BindScore bindScore = VBBinder.BindScore.Unknown;
			int num = 0;
			int num2 = 0;
			int upperBound = match.GetUpperBound(0);
			checked
			{
				for (int i = num2; i <= upperBound; i++)
				{
					PropertyInfo propertyInfo = match[i];
					if (propertyInfo != null)
					{
						VBBinder.BindScore bindScore2 = this.BindingScore(propertyInfo.GetIndexParameters(), null, indexes, false, -1);
						if (bindScore2 < bindScore)
						{
							if (i != 0)
							{
								match[0] = match[i];
								match[i] = null;
							}
							num = 1;
							bindScore = bindScore2;
						}
						else if (bindScore2 == bindScore)
						{
							if (bindScore2 == VBBinder.BindScore.Widening1)
							{
								ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
								ParameterInfo[] indexParameters2 = match[0].GetIndexParameters();
								int num3 = -1;
								int num4 = 0;
								int upperBound2 = indexParameters.GetUpperBound(0);
								for (int j = num4; j <= upperBound2; j++)
								{
									int num5 = j;
									int num6 = j;
									if (num5 != -1 && num6 != -1)
									{
										Type parameterType = indexParameters2[num5].ParameterType;
										Type parameterType2 = indexParameters[num6].ParameterType;
										if (ObjectType.IsWideningConversion(parameterType, parameterType2))
										{
											if (num3 == 1)
											{
												num3 = -1;
												break;
											}
											num3 = 0;
										}
										else if (ObjectType.IsWideningConversion(parameterType2, parameterType))
										{
											if (num3 == 0)
											{
												num3 = -1;
												break;
											}
											num3 = 1;
										}
									}
								}
								if (num3 == -1)
								{
									if (num != i)
									{
										match[num] = match[i];
										match[i] = null;
									}
									num++;
								}
								else if (num3 == 0)
								{
									num = 1;
								}
								else
								{
									if (i != 0)
									{
										match[0] = match[i];
										match[i] = null;
									}
									num = 1;
								}
							}
							else if (bindScore2 == VBBinder.BindScore.Exact)
							{
								if (propertyInfo.DeclaringType.IsSubclassOf(match[0].DeclaringType))
								{
									if (i != 0)
									{
										match[0] = match[i];
										match[i] = null;
									}
									num = 1;
								}
								else if (!match[0].DeclaringType.IsSubclassOf(propertyInfo.DeclaringType))
								{
									if (num != i)
									{
										match[num] = match[i];
										match[i] = null;
									}
									num++;
								}
							}
							else
							{
								if (num != i)
								{
									match[num] = match[i];
									match[i] = null;
								}
								num++;
							}
						}
						else
						{
							match[i] = null;
						}
					}
				}
				if (num == 1)
				{
					return match[0];
				}
				return null;
			}
		}

		public override object ChangeType(object value, Type typ, CultureInfo culture)
		{
			object obj;
			try
			{
				if (typ == typeof(object) || (typ.IsByRef && typ.GetElementType() == typeof(object)))
				{
					obj = value;
				}
				else
				{
					obj = ObjectType.CTypeHelper(value, typ);
				}
			}
			catch (Exception ex)
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
				{
					Utils.VBFriendlyName(value),
					Utils.VBFriendlyName(typ)
				}));
			}
			return obj;
		}

		private VBBinder.BindScore BindingScore(ParameterInfo[] Parameters, int[] paramOrder, Type[] ArgTypes, bool IsPropertySet, int ParamArrayIndex)
		{
			VBBinder.BindScore bindScore = VBBinder.BindScore.Exact;
			int num = ArgTypes.GetUpperBound(0);
			int num2 = Parameters.GetUpperBound(0);
			checked
			{
				if (IsPropertySet)
				{
					num2--;
					num--;
				}
				int num3 = 0;
				int num4 = Math.Max(num, num2);
				for (int i = num3; i <= num4; i++)
				{
					int num5;
					if (paramOrder == null)
					{
						num5 = i;
					}
					else
					{
						num5 = paramOrder[i];
					}
					Type type;
					if (num5 == -1)
					{
						type = null;
					}
					else
					{
						type = ArgTypes[num5];
					}
					if (type != null)
					{
						Type type2;
						if (i > num2)
						{
							type2 = Parameters[ParamArrayIndex].ParameterType;
						}
						else
						{
							type2 = Parameters[i].ParameterType;
						}
						if (i != ParamArrayIndex || !type.IsArray || type2 != type)
						{
							if (i == ParamArrayIndex && type.IsArray)
							{
								if (this.m_state.m_OriginalArgs != null)
								{
									if (this.m_state.m_OriginalArgs[num5] != null)
									{
										if (!type2.IsInstanceOfType(this.m_state.m_OriginalArgs[num5]))
										{
											goto IL_00DD;
										}
									}
								}
								if (bindScore < VBBinder.BindScore.Widening1)
								{
									bindScore = VBBinder.BindScore.Widening1;
									goto IL_015E;
								}
								goto IL_015E;
							}
							IL_00DD:
							if ((ParamArrayIndex != -1 && i >= ParamArrayIndex) || type2.IsByRef)
							{
								type2 = type2.GetElementType();
							}
							if (type != type2)
							{
								if (ObjectType.IsWideningConversion(type, type2))
								{
									if (bindScore < VBBinder.BindScore.Widening1)
									{
										bindScore = VBBinder.BindScore.Widening1;
									}
								}
								else
								{
									if (type.IsArray)
									{
										if (this.m_state.m_OriginalArgs != null)
										{
											if (this.m_state.m_OriginalArgs[num5] != null)
											{
												if (!type2.IsInstanceOfType(this.m_state.m_OriginalArgs[num5]))
												{
													goto IL_015B;
												}
											}
										}
										if (bindScore < VBBinder.BindScore.Widening1)
										{
											bindScore = VBBinder.BindScore.Widening1;
											goto IL_015E;
										}
										goto IL_015E;
									}
									IL_015B:
									bindScore = VBBinder.BindScore.Narrowing;
								}
							}
						}
					}
					IL_015E:;
				}
				return bindScore;
			}
		}

		private void ReorderParams(int[] paramOrder, object[] vars, VBBinder.VBBinderState state)
		{
			int num = Math.Max(vars.GetUpperBound(0), paramOrder.GetUpperBound(0));
			checked
			{
				state.m_OriginalParamOrder = new int[num + 1];
				int num2 = 0;
				int num3 = num;
				for (int i = num2; i <= num3; i++)
				{
					state.m_OriginalParamOrder[i] = paramOrder[i];
				}
			}
		}

		private Exception CreateParamOrder(bool SetProp, int[] paramOrder, ParameterInfo[] pars, object[] args, string[] names)
		{
			checked
			{
				bool[] array = new bool[pars.Length - 1 + 1];
				int num = args.Length - names.Length - 1;
				int num2 = pars.GetUpperBound(0);
				int num3 = 0;
				int upperBound = pars.GetUpperBound(0);
				for (int i = num3; i <= upperBound; i++)
				{
					paramOrder[i] = -1;
				}
				if (SetProp)
				{
					paramOrder[pars.GetUpperBound(0)] = args.GetUpperBound(0);
					num--;
					num2--;
				}
				int num4 = 0;
				int num5 = num;
				for (int i = num4; i <= num5; i++)
				{
					paramOrder[i] = names.Length + i;
				}
				int num6 = 0;
				int upperBound2 = names.GetUpperBound(0);
				for (int i = num6; i <= upperBound2; i++)
				{
					int num7 = 0;
					int num8 = num2;
					int j = num7;
					while (j <= num8)
					{
						if (Strings.StrComp(names[i], pars[j].Name, CompareMethod.Text) == 0)
						{
							if (paramOrder[j] != -1)
							{
								return new ArgumentException(Utils.GetResourceString("NamedArgumentAlreadyUsed1", new string[] { pars[j].Name }));
							}
							paramOrder[j] = i;
							array[i] = true;
							break;
						}
						else
						{
							j++;
						}
					}
					if (j > num2)
					{
						return new MissingMemberException(Utils.GetResourceString("Argument_InvalidNamedArg2", new string[]
						{
							names[i],
							this.CalledMethodName()
						}));
					}
				}
				return null;
			}
		}

		[DebuggerStepThrough]
		[DebuggerHidden]
		internal object InvokeMember(string name, BindingFlags invokeAttr, Type objType, IReflect objIReflect, object target, object[] args, string[] namedParameters)
		{
			object obj;
			object obj2;
			checked
			{
				if (objType.IsCOMObject)
				{
					ParameterModifier[] array = null;
					if (this.m_ByRefFlags != null && target != null && !RemotingServices.IsTransparentProxy(target))
					{
						ParameterModifier parameterModifier = new ParameterModifier(args.Length);
						array = new ParameterModifier[] { parameterModifier };
						object value = Missing.Value;
						int num = 0;
						int upperBound = args.GetUpperBound(0);
						for (int i = num; i <= upperBound; i++)
						{
							if (args[i] != value)
							{
								parameterModifier[i] = this.m_ByRefFlags[i];
							}
						}
					}
					try
					{
						new SecurityPermission(PermissionState.Unrestricted).Demand();
						return objIReflect.InvokeMember(name, invokeAttr, null, target, args, array, null, namedParameters);
					}
					catch (MissingMemberException ex)
					{
						throw new MissingMemberException(Utils.GetResourceString("MissingMember_MemberNotFoundOnType2", new string[]
						{
							name,
							Utils.VBFriendlyName(objType)
						}));
					}
				}
				this.m_BindToName = name;
				this.m_objType = objType;
				if (name.Length == 0)
				{
					if (objType == objIReflect)
					{
						name = this.GetDefaultMemberName(objType);
						if (name == null)
						{
							throw new MissingMemberException(Utils.GetResourceString("MissingMember_NoDefaultMemberFound1", new string[] { Utils.VBFriendlyName(objType) }));
						}
					}
					else
					{
						name = "";
					}
				}
				MethodBase[] methodsByName = this.GetMethodsByName(objType, objIReflect, name, invokeAttr);
				if (args == null)
				{
					args = new object[0];
				}
				obj = null;
				MethodBase methodBase = this.BindToMethod(invokeAttr, methodsByName, ref args, null, null, namedParameters, ref obj);
				if (methodBase == null)
				{
					throw new MissingMemberException(Utils.GetResourceString("NoMethodTakingXArguments2", new string[]
					{
						this.CalledMethodName(),
						Conversions.ToString(this.GetPropArgCount(args, (invokeAttr & BindingFlags.SetProperty) != BindingFlags.Default))
					}));
				}
				VBBinder.SecurityCheckForLateboundCalls(methodBase, objType, objIReflect);
				MethodInfo methodInfo = (MethodInfo)methodBase;
				if (objType != objIReflect)
				{
					if (!methodInfo.IsStatic)
					{
						if (!LateBinding.DoesTargetObjectMatch(target, methodInfo))
						{
							obj2 = LateBinding.InvokeMemberOnIReflect(objIReflect, methodInfo, BindingFlags.InvokeMethod, target, args);
							goto IL_01ED;
						}
					}
				}
				LateBinding.VerifyObjRefPresentForInstanceCall(target, methodInfo);
				obj2 = methodInfo.Invoke(target, args);
			}
			IL_01ED:
			if (obj != null)
			{
				this.ReorderArgumentArray(ref args, obj);
			}
			return obj2;
		}

		private string GetDefaultMemberName(Type typ)
		{
			object[] customAttributes;
			for (;;)
			{
				customAttributes = typ.GetCustomAttributes(typeof(DefaultMemberAttribute), false);
				if (customAttributes != null && customAttributes.Length != 0)
				{
					break;
				}
				typ = typ.BaseType;
				if (typ == null)
				{
					goto Block_2;
				}
			}
			return ((DefaultMemberAttribute)customAttributes[0]).MemberName;
			Block_2:
			return null;
		}

		private MethodBase[] GetMethodsByName(Type objType, IReflect objIReflect, string name, BindingFlags invokeAttr)
		{
			MemberInfo[] array = objIReflect.GetMember(name, invokeAttr);
			array = LateBinding.GetNonGenericMembers(array);
			if (array == null)
			{
				return null;
			}
			int num = 0;
			int upperBound = array.GetUpperBound(0);
			checked
			{
				int num3;
				for (int i = num; i <= upperBound; i++)
				{
					MemberInfo memberInfo = array[i];
					if (memberInfo != null)
					{
						if (memberInfo.MemberType == MemberTypes.Field)
						{
							Type type = memberInfo.DeclaringType;
							int num2 = 0;
							int upperBound2 = array.GetUpperBound(0);
							for (int j = num2; j <= upperBound2; j++)
							{
								if (i != j && array[j] != null && type.IsSubclassOf(array[j].DeclaringType))
								{
									array[j] = null;
									num3++;
								}
							}
						}
						else if (memberInfo.MemberType == MemberTypes.Method)
						{
							MethodInfo methodInfo = (MethodInfo)memberInfo;
							if (!methodInfo.IsHideBySig)
							{
								if (!methodInfo.IsVirtual || (methodInfo.IsVirtual && (methodInfo.Attributes & MethodAttributes.VtableLayoutMask) != MethodAttributes.PrivateScope) || (methodInfo.IsVirtual && (methodInfo.GetBaseDefinition().Attributes & MethodAttributes.VtableLayoutMask) != MethodAttributes.PrivateScope))
								{
									Type type = memberInfo.DeclaringType;
									int num4 = 0;
									int upperBound3 = array.GetUpperBound(0);
									for (int k = num4; k <= upperBound3; k++)
									{
										if (i != k && array[k] != null && type.IsSubclassOf(array[k].DeclaringType))
										{
											array[k] = null;
											num3++;
										}
									}
								}
							}
						}
						else if (memberInfo.MemberType == MemberTypes.Property)
						{
							PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
							int num5 = 1;
							MethodInfo methodInfo;
							do
							{
								if (num5 == 1)
								{
									methodInfo = propertyInfo.GetGetMethod();
								}
								else
								{
									methodInfo = propertyInfo.GetSetMethod();
								}
								if (methodInfo != null)
								{
									if (!methodInfo.IsHideBySig)
									{
										if (!methodInfo.IsVirtual || (methodInfo.IsVirtual && (methodInfo.Attributes & MethodAttributes.VtableLayoutMask) != MethodAttributes.PrivateScope))
										{
											Type type = memberInfo.DeclaringType;
											int num6 = 0;
											int upperBound4 = array.GetUpperBound(0);
											for (int l = num6; l <= upperBound4; l++)
											{
												if (i != l && array[l] != null && type.IsSubclassOf(array[l].DeclaringType))
												{
													array[l] = null;
													num3++;
												}
											}
										}
									}
								}
								num5++;
							}
							while (num5 <= 2);
							if ((invokeAttr & BindingFlags.GetProperty) != BindingFlags.Default)
							{
								methodInfo = propertyInfo.GetGetMethod();
							}
							else if ((invokeAttr & BindingFlags.SetProperty) != BindingFlags.Default)
							{
								methodInfo = propertyInfo.GetSetMethod();
							}
							else
							{
								methodInfo = null;
							}
							if (methodInfo == null)
							{
								num3++;
							}
							array[i] = methodInfo;
						}
						else if (memberInfo.MemberType == MemberTypes.NestedType)
						{
							Type type = memberInfo.DeclaringType;
							int num7 = 0;
							int upperBound5 = array.GetUpperBound(0);
							for (int m = num7; m <= upperBound5; m++)
							{
								if (i != m && array[m] != null && type.IsSubclassOf(array[m].DeclaringType))
								{
									array[m] = null;
									num3++;
								}
							}
							if (num3 == array.Length - 1)
							{
								throw new ArgumentException(Utils.GetResourceString("Argument_IllegalNestedType2", new string[]
								{
									name,
									Utils.VBFriendlyName(objType)
								}));
							}
							array[i] = null;
							num3++;
						}
					}
				}
				int num8 = array.Length - num3;
				MethodBase[] array2 = new MethodBase[num8 - 1 + 1];
				int num9 = 0;
				int num10 = 0;
				int num11 = array.Length - 1;
				for (int n = num10; n <= num11; n++)
				{
					if (array[n] != null)
					{
						array2[num9] = (MethodBase)array[n];
						num9++;
					}
				}
				return array2;
			}
		}

		internal string CalledMethodName()
		{
			return this.m_objType.Name + "." + this.m_BindToName;
		}

		internal static void SecurityCheckForLateboundCalls(MemberInfo member, Type objType, IReflect objIReflect)
		{
			if (objType != objIReflect && !VBBinder.IsMemberPublic(member))
			{
				throw new MissingMethodException();
			}
			Type declaringType = member.DeclaringType;
			if (!declaringType.IsPublic && declaringType.Assembly == Utils.VBRuntimeAssembly)
			{
				throw new MissingMethodException();
			}
		}

		private static bool IsMemberPublic(MemberInfo Member)
		{
			switch (Member.MemberType)
			{
			case MemberTypes.Constructor:
				return ((ConstructorInfo)Member).IsPublic;
			case MemberTypes.Field:
				return ((FieldInfo)Member).IsPublic;
			case MemberTypes.Method:
				return ((MethodInfo)Member).IsPublic;
			case MemberTypes.Property:
				return false;
			}
			return false;
		}

		internal void CacheMember(MemberInfo member)
		{
			this.m_CachedMember = member;
		}

		private const int PARAMARRAY_NONE = -1;

		private const int ARG_MISSING = -1;

		internal string m_BindToName;

		internal Type m_objType;

		private VBBinder.VBBinderState m_state;

		private MemberInfo m_CachedMember;

		private bool[] m_ByRefFlags;

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal sealed class VBBinderState
		{
			internal object[] m_OriginalArgs;

			internal bool[] m_ByRefFlags;

			internal bool[] m_OriginalByRefFlags;

			internal int[] m_OriginalParamOrder;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public enum BindScore
		{
			Exact,
			Widening0,
			Widening1,
			Narrowing,
			Unknown
		}
	}
}

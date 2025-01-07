using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class ObjectFlowControl
	{
		private ObjectFlowControl()
		{
		}

		public static void CheckForSyncLockOnValueType(object Expression)
		{
			if (Expression != null && Expression.GetType().IsValueType)
			{
				throw new ArgumentException(Utils.GetResourceString("SyncLockRequiresReferenceType1", new string[] { Utils.VBFriendlyName(Expression.GetType()) }));
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class ForLoopControl
		{
			private ForLoopControl()
			{
			}

			private static Type GetWidestType(Type Type1, Type Type2)
			{
				if (Type1 == null || Type2 == null)
				{
					return null;
				}
				if (!Type1.IsEnum && !Type2.IsEnum)
				{
					TypeCode typeCode = Symbols.GetTypeCode(Type1);
					TypeCode typeCode2 = Symbols.GetTypeCode(Type2);
					if (Symbols.IsNumericType(typeCode) && Symbols.IsNumericType(typeCode2))
					{
						return Symbols.MapTypeCodeToType(ConversionResolution.ForLoopWidestTypeCode[(int)typeCode][(int)typeCode2]);
					}
				}
				Symbols.Method method = null;
				ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyConversion(Type2, Type1, ref method);
				if (conversionClass == ConversionResolution.ConversionClass.Identity || conversionClass == ConversionResolution.ConversionClass.Widening)
				{
					return Type2;
				}
				method = null;
				ConversionResolution.ConversionClass conversionClass2 = ConversionResolution.ClassifyConversion(Type1, Type2, ref method);
				if (conversionClass2 == ConversionResolution.ConversionClass.Widening)
				{
					return Type1;
				}
				return null;
			}

			private static Type GetWidestType(Type Type1, Type Type2, Type Type3)
			{
				return ObjectFlowControl.ForLoopControl.GetWidestType(Type1, ObjectFlowControl.ForLoopControl.GetWidestType(Type2, Type3));
			}

			private static object ConvertLoopElement(string ElementName, object Value, Type SourceType, Type TargetType)
			{
				object obj;
				try
				{
					obj = Conversions.ChangeType(Value, TargetType);
				}
				catch (AccessViolationException ex)
				{
					throw ex;
				}
				catch (StackOverflowException ex2)
				{
					throw ex2;
				}
				catch (OutOfMemoryException ex3)
				{
					throw ex3;
				}
				catch (ThreadAbortException ex4)
				{
					throw ex4;
				}
				catch (Exception)
				{
					throw new ArgumentException(Utils.GetResourceString("ForLoop_ConvertToType3", new string[]
					{
						ElementName,
						Utils.VBFriendlyName(SourceType),
						Utils.VBFriendlyName(TargetType)
					}));
				}
				return obj;
			}

			private static Symbols.Method VerifyForLoopOperator(Symbols.UserDefinedOperator Op, object ForLoopArgument, Type ForLoopArgumentType)
			{
				Symbols.Method callableUserDefinedOperator = Operators.GetCallableUserDefinedOperator(Op, new object[] { ForLoopArgument, ForLoopArgument });
				if (callableUserDefinedOperator == null)
				{
					throw new ArgumentException(Utils.GetResourceString("ForLoop_OperatorRequired2", new string[]
					{
						Utils.VBFriendlyNameOfType(ForLoopArgumentType, true),
						Symbols.OperatorNames[(int)Op]
					}));
				}
				MethodInfo methodInfo = callableUserDefinedOperator.AsMethod() as MethodInfo;
				ParameterInfo[] parameters = methodInfo.GetParameters();
				switch (Op)
				{
				case Symbols.UserDefinedOperator.Plus:
				case Symbols.UserDefinedOperator.Minus:
					if (parameters.Length == 2)
					{
						if (parameters[0].ParameterType == ForLoopArgumentType)
						{
							if (parameters[1].ParameterType == ForLoopArgumentType)
							{
								if (methodInfo.ReturnType == ForLoopArgumentType)
								{
									break;
								}
							}
						}
					}
					throw new ArgumentException(Utils.GetResourceString("ForLoop_UnacceptableOperator2", new string[]
					{
						callableUserDefinedOperator.ToString(),
						Utils.VBFriendlyNameOfType(ForLoopArgumentType, true)
					}));
				case Symbols.UserDefinedOperator.LessEqual:
				case Symbols.UserDefinedOperator.GreaterEqual:
					if (parameters.Length == 2)
					{
						if (parameters[0].ParameterType == ForLoopArgumentType)
						{
							if (parameters[1].ParameterType == ForLoopArgumentType)
							{
								break;
							}
						}
					}
					throw new ArgumentException(Utils.GetResourceString("ForLoop_UnacceptableRelOperator2", new string[]
					{
						callableUserDefinedOperator.ToString(),
						Utils.VBFriendlyNameOfType(ForLoopArgumentType, true)
					}));
				}
				return callableUserDefinedOperator;
			}

			public static bool ForLoopInitObj(object Counter, object Start, object Limit, object StepValue, ref object LoopForResult, ref object CounterResult)
			{
				if (Start == null)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Start" }));
				}
				if (Limit == null)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Limit" }));
				}
				if (StepValue == null)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Step" }));
				}
				Type type = Start.GetType();
				Type type2 = Limit.GetType();
				Type type3 = StepValue.GetType();
				Type widestType = ObjectFlowControl.ForLoopControl.GetWidestType(type3, type, type2);
				if (widestType == null)
				{
					throw new ArgumentException(Utils.GetResourceString("ForLoop_CommonType3", new string[]
					{
						Utils.VBFriendlyName(type),
						Utils.VBFriendlyName(type2),
						Utils.VBFriendlyName(StepValue)
					}));
				}
				ObjectFlowControl.ForLoopControl forLoopControl = new ObjectFlowControl.ForLoopControl();
				TypeCode typeCode = Symbols.GetTypeCode(widestType);
				if (typeCode == TypeCode.Object)
				{
					forLoopControl.UseUserDefinedOperators = true;
				}
				if (typeCode == TypeCode.String)
				{
					typeCode = TypeCode.Double;
				}
				TypeCode typeCode2 = Type.GetTypeCode(type);
				TypeCode typeCode3 = Type.GetTypeCode(type2);
				TypeCode typeCode4 = Type.GetTypeCode(type3);
				Type type4 = null;
				if (typeCode2 == typeCode && type.IsEnum)
				{
					type4 = type;
				}
				if (typeCode3 == typeCode && type2.IsEnum)
				{
					if (type4 != null && type4 != type2)
					{
						type4 = null;
						goto IL_015E;
					}
					type4 = type2;
				}
				if (typeCode4 == typeCode && type3.IsEnum)
				{
					if (type4 != null && type4 != type3)
					{
						type4 = null;
					}
					else
					{
						type4 = type3;
					}
				}
				IL_015E:
				forLoopControl.EnumType = type4;
				if (!forLoopControl.UseUserDefinedOperators)
				{
					forLoopControl.WidestType = Symbols.MapTypeCodeToType(typeCode);
				}
				else
				{
					forLoopControl.WidestType = widestType;
				}
				forLoopControl.WidestTypeCode = typeCode;
				forLoopControl.Counter = ObjectFlowControl.ForLoopControl.ConvertLoopElement("Start", Start, type, forLoopControl.WidestType);
				forLoopControl.Limit = ObjectFlowControl.ForLoopControl.ConvertLoopElement("Limit", Limit, type2, forLoopControl.WidestType);
				forLoopControl.StepValue = ObjectFlowControl.ForLoopControl.ConvertLoopElement("Step", StepValue, type3, forLoopControl.WidestType);
				if (forLoopControl.UseUserDefinedOperators)
				{
					forLoopControl.OperatorPlus = ObjectFlowControl.ForLoopControl.VerifyForLoopOperator(Symbols.UserDefinedOperator.Plus, forLoopControl.Counter, forLoopControl.WidestType);
					ObjectFlowControl.ForLoopControl.VerifyForLoopOperator(Symbols.UserDefinedOperator.Minus, forLoopControl.Counter, forLoopControl.WidestType);
					forLoopControl.OperatorLessEqual = ObjectFlowControl.ForLoopControl.VerifyForLoopOperator(Symbols.UserDefinedOperator.LessEqual, forLoopControl.Counter, forLoopControl.WidestType);
					forLoopControl.OperatorGreaterEqual = ObjectFlowControl.ForLoopControl.VerifyForLoopOperator(Symbols.UserDefinedOperator.GreaterEqual, forLoopControl.Counter, forLoopControl.WidestType);
				}
				forLoopControl.PositiveStep = Operators.ConditionalCompareObjectGreaterEqual(forLoopControl.StepValue, Operators.SubtractObject(forLoopControl.StepValue, forLoopControl.StepValue), false);
				LoopForResult = forLoopControl;
				if (forLoopControl.EnumType != null)
				{
					CounterResult = Enum.ToObject(forLoopControl.EnumType, forLoopControl.Counter);
				}
				else
				{
					CounterResult = forLoopControl.Counter;
				}
				return ObjectFlowControl.ForLoopControl.CheckContinueLoop(forLoopControl);
			}

			public static bool ForNextCheckObj(object Counter, object LoopObj, ref object CounterResult)
			{
				if (LoopObj == null)
				{
					throw ExceptionUtils.VbMakeException(92);
				}
				if (Counter == null)
				{
					throw new NullReferenceException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Counter" }));
				}
				ObjectFlowControl.ForLoopControl forLoopControl = (ObjectFlowControl.ForLoopControl)LoopObj;
				bool flag = false;
				if (!forLoopControl.UseUserDefinedOperators)
				{
					TypeCode typeCode = ((IConvertible)Counter).GetTypeCode();
					if (typeCode != forLoopControl.WidestTypeCode || typeCode == TypeCode.String)
					{
						if (typeCode == TypeCode.Object)
						{
							throw new ArgumentException(Utils.GetResourceString("ForLoop_CommonType2", new string[]
							{
								Utils.VBFriendlyName(Symbols.MapTypeCodeToType(typeCode)),
								Utils.VBFriendlyName(forLoopControl.WidestType)
							}));
						}
						Type widestType = ObjectFlowControl.ForLoopControl.GetWidestType(Symbols.MapTypeCodeToType(typeCode), forLoopControl.WidestType);
						TypeCode typeCode2 = Symbols.GetTypeCode(widestType);
						if (typeCode2 == TypeCode.String)
						{
							typeCode2 = TypeCode.Double;
						}
						forLoopControl.WidestTypeCode = typeCode2;
						forLoopControl.WidestType = Symbols.MapTypeCodeToType(typeCode2);
						flag = true;
					}
				}
				if (flag || forLoopControl.UseUserDefinedOperators)
				{
					Counter = ObjectFlowControl.ForLoopControl.ConvertLoopElement("Start", Counter, Counter.GetType(), forLoopControl.WidestType);
					if (!forLoopControl.UseUserDefinedOperators)
					{
						forLoopControl.Limit = ObjectFlowControl.ForLoopControl.ConvertLoopElement("Limit", forLoopControl.Limit, forLoopControl.Limit.GetType(), forLoopControl.WidestType);
						forLoopControl.StepValue = ObjectFlowControl.ForLoopControl.ConvertLoopElement("Step", forLoopControl.StepValue, forLoopControl.StepValue.GetType(), forLoopControl.WidestType);
					}
				}
				if (!forLoopControl.UseUserDefinedOperators)
				{
					forLoopControl.Counter = Operators.AddObject(Counter, forLoopControl.StepValue);
					TypeCode typeCode3 = ((IConvertible)forLoopControl.Counter).GetTypeCode();
					if (forLoopControl.EnumType != null)
					{
						CounterResult = Enum.ToObject(forLoopControl.EnumType, forLoopControl.Counter);
					}
					else
					{
						CounterResult = forLoopControl.Counter;
					}
					if (typeCode3 != forLoopControl.WidestTypeCode)
					{
						forLoopControl.Limit = Conversions.ChangeType(forLoopControl.Limit, Symbols.MapTypeCodeToType(typeCode3));
						forLoopControl.StepValue = Conversions.ChangeType(forLoopControl.StepValue, Symbols.MapTypeCodeToType(typeCode3));
						return false;
					}
				}
				else
				{
					forLoopControl.Counter = Operators.InvokeUserDefinedOperator(forLoopControl.OperatorPlus, true, new object[] { Counter, forLoopControl.StepValue });
					if (forLoopControl.Counter.GetType() != forLoopControl.WidestType)
					{
						forLoopControl.Counter = ObjectFlowControl.ForLoopControl.ConvertLoopElement("Start", forLoopControl.Counter, forLoopControl.Counter.GetType(), forLoopControl.WidestType);
					}
					CounterResult = forLoopControl.Counter;
				}
				return ObjectFlowControl.ForLoopControl.CheckContinueLoop(forLoopControl);
			}

			public static bool ForNextCheckR4(float count, float limit, float StepValue)
			{
				if (StepValue >= 0f)
				{
					return count <= limit;
				}
				return count >= limit;
			}

			public static bool ForNextCheckR8(double count, double limit, double StepValue)
			{
				if (StepValue >= 0.0)
				{
					return count <= limit;
				}
				return count >= limit;
			}

			public static bool ForNextCheckDec(decimal count, decimal limit, decimal StepValue)
			{
				if (decimal.Compare(StepValue, 0m) >= 0)
				{
					return decimal.Compare(count, limit) <= 0;
				}
				return decimal.Compare(count, limit) >= 0;
			}

			private static bool CheckContinueLoop(ObjectFlowControl.ForLoopControl LoopFor)
			{
				if (!LoopFor.UseUserDefinedOperators)
				{
					try
					{
						IComparable comparable = (IComparable)LoopFor.Counter;
						int num = comparable.CompareTo(LoopFor.Limit);
						if (LoopFor.PositiveStep)
						{
							return num <= 0;
						}
						return num >= 0;
					}
					catch (InvalidCastException ex)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_IComparable2", new string[]
						{
							"loop control variable",
							Utils.VBFriendlyName(LoopFor.Counter)
						}));
					}
				}
				if (LoopFor.PositiveStep)
				{
					return Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(LoopFor.OperatorLessEqual, true, new object[] { LoopFor.Counter, LoopFor.Limit }));
				}
				return Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(LoopFor.OperatorGreaterEqual, true, new object[] { LoopFor.Counter, LoopFor.Limit }));
			}

			private object Counter;

			private object Limit;

			private object StepValue;

			private bool PositiveStep;

			private Type EnumType;

			private Type WidestType;

			private TypeCode WidestTypeCode;

			private bool UseUserDefinedOperators;

			private Symbols.Method OperatorPlus;

			private Symbols.Method OperatorGreaterEqual;

			private Symbols.Method OperatorLessEqual;
		}
	}
}

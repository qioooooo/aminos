using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class FlowControl
	{
		private FlowControl()
		{
		}

		public static bool ForNextCheckR4(float count, float limit, float StepValue)
		{
			if (StepValue > 0f)
			{
				return count <= limit;
			}
			return count >= limit;
		}

		public static bool ForNextCheckR8(double count, double limit, double StepValue)
		{
			if (StepValue > 0.0)
			{
				return count <= limit;
			}
			return count >= limit;
		}

		public static bool ForNextCheckDec(decimal count, decimal limit, decimal StepValue)
		{
			if (StepValue < 0m)
			{
				return count >= limit;
			}
			return count <= limit;
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
			TypeCode typeCode = ObjectType.GetWidestType(Start, Limit, false);
			typeCode = ObjectType.GetWidestType(StepValue, typeCode);
			if (typeCode == TypeCode.String)
			{
				typeCode = TypeCode.Double;
			}
			if (typeCode == TypeCode.Object)
			{
				throw new ArgumentException(Utils.GetResourceString("ForLoop_CommonType3", new string[]
				{
					Utils.VBFriendlyName(type),
					Utils.VBFriendlyName(type2),
					Utils.VBFriendlyName(StepValue)
				}));
			}
			FlowControl.ObjectFor objectFor = new FlowControl.ObjectFor();
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
					goto IL_0159;
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
			IL_0159:
			objectFor.EnumType = type4;
			try
			{
				objectFor.Counter = ObjectType.CTypeHelper(Start, typeCode);
			}
			catch (StackOverflowException ex)
			{
				throw ex;
			}
			catch (OutOfMemoryException ex2)
			{
				throw ex2;
			}
			catch (ThreadAbortException ex3)
			{
				throw ex3;
			}
			catch (Exception)
			{
				throw new ArgumentException(Utils.GetResourceString("ForLoop_ConvertToType3", new string[]
				{
					"Start",
					Utils.VBFriendlyName(type),
					Utils.VBFriendlyName(ObjectType.TypeFromTypeCode(typeCode))
				}));
			}
			try
			{
				objectFor.Limit = ObjectType.CTypeHelper(Limit, typeCode);
			}
			catch (StackOverflowException ex4)
			{
				throw ex4;
			}
			catch (OutOfMemoryException ex5)
			{
				throw ex5;
			}
			catch (ThreadAbortException ex6)
			{
				throw ex6;
			}
			catch (Exception)
			{
				throw new ArgumentException(Utils.GetResourceString("ForLoop_ConvertToType3", new string[]
				{
					"Limit",
					Utils.VBFriendlyName(type2),
					Utils.VBFriendlyName(ObjectType.TypeFromTypeCode(typeCode))
				}));
			}
			try
			{
				objectFor.StepValue = ObjectType.CTypeHelper(StepValue, typeCode);
			}
			catch (StackOverflowException ex7)
			{
				throw ex7;
			}
			catch (OutOfMemoryException ex8)
			{
				throw ex8;
			}
			catch (ThreadAbortException ex9)
			{
				throw ex9;
			}
			catch (Exception)
			{
				throw new ArgumentException(Utils.GetResourceString("ForLoop_ConvertToType3", new string[]
				{
					"Step",
					Utils.VBFriendlyName(type3),
					Utils.VBFriendlyName(ObjectType.TypeFromTypeCode(typeCode))
				}));
			}
			object obj = ObjectType.CTypeHelper(0, typeCode);
			IComparable comparable = (IComparable)objectFor.StepValue;
			int num = comparable.CompareTo(obj);
			if (num >= 0)
			{
				objectFor.PositiveStep = true;
			}
			else
			{
				objectFor.PositiveStep = false;
			}
			LoopForResult = objectFor;
			if (objectFor.EnumType != null)
			{
				CounterResult = Enum.ToObject(objectFor.EnumType, objectFor.Counter);
			}
			else
			{
				CounterResult = objectFor.Counter;
			}
			return FlowControl.CheckContinueLoop(objectFor);
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
			FlowControl.ObjectFor objectFor = (FlowControl.ObjectFor)LoopObj;
			TypeCode typeCode = ((IConvertible)Counter).GetTypeCode();
			TypeCode typeCode2 = ((IConvertible)objectFor.StepValue).GetTypeCode();
			TypeCode typeCode3;
			TypeCode typeCode4;
			if (typeCode == typeCode2 && typeCode != TypeCode.String)
			{
				typeCode3 = typeCode;
			}
			else
			{
				typeCode3 = ObjectType.GetWidestType(typeCode, typeCode2);
				if (typeCode3 == TypeCode.String)
				{
					typeCode3 = TypeCode.Double;
				}
				if (typeCode4 == TypeCode.Object)
				{
					throw new ArgumentException(Utils.GetResourceString("ForLoop_CommonType2", new string[]
					{
						Utils.VBFriendlyName(ObjectType.TypeFromTypeCode(typeCode)),
						Utils.VBFriendlyName(ObjectType.TypeFromTypeCode(typeCode2))
					}));
				}
				try
				{
					Counter = ObjectType.CTypeHelper(Counter, typeCode3);
				}
				catch (StackOverflowException ex)
				{
					throw ex;
				}
				catch (OutOfMemoryException ex2)
				{
					throw ex2;
				}
				catch (ThreadAbortException ex3)
				{
					throw ex3;
				}
				catch (Exception)
				{
					throw new ArgumentException(Utils.GetResourceString("ForLoop_ConvertToType3", new string[]
					{
						"Start",
						Utils.VBFriendlyName(Counter.GetType()),
						Utils.VBFriendlyName(ObjectType.TypeFromTypeCode(typeCode3))
					}));
				}
				try
				{
					objectFor.Limit = ObjectType.CTypeHelper(objectFor.Limit, typeCode3);
				}
				catch (StackOverflowException ex4)
				{
					throw ex4;
				}
				catch (OutOfMemoryException ex5)
				{
					throw ex5;
				}
				catch (ThreadAbortException ex6)
				{
					throw ex6;
				}
				catch (Exception)
				{
					throw new ArgumentException(Utils.GetResourceString("ForLoop_ConvertToType3", new string[]
					{
						"Limit",
						Utils.VBFriendlyName(objectFor.Limit.GetType()),
						Utils.VBFriendlyName(ObjectType.TypeFromTypeCode(typeCode3))
					}));
				}
				try
				{
					objectFor.StepValue = ObjectType.CTypeHelper(objectFor.StepValue, typeCode3);
				}
				catch (StackOverflowException ex7)
				{
					throw ex7;
				}
				catch (OutOfMemoryException ex8)
				{
					throw ex8;
				}
				catch (ThreadAbortException ex9)
				{
					throw ex9;
				}
				catch (Exception)
				{
					throw new ArgumentException(Utils.GetResourceString("ForLoop_ConvertToType3", new string[]
					{
						"Step",
						Utils.VBFriendlyName(objectFor.StepValue.GetType()),
						Utils.VBFriendlyName(ObjectType.TypeFromTypeCode(typeCode3))
					}));
				}
			}
			objectFor.Counter = ObjectType.AddObj(Counter, objectFor.StepValue);
			typeCode4 = ((IConvertible)objectFor.Counter).GetTypeCode();
			if (objectFor.EnumType != null)
			{
				CounterResult = Enum.ToObject(objectFor.EnumType, objectFor.Counter);
			}
			else
			{
				CounterResult = objectFor.Counter;
			}
			if (typeCode4 != typeCode3)
			{
				objectFor.Limit = ObjectType.CTypeHelper(objectFor.Limit, typeCode4);
				objectFor.StepValue = ObjectType.CTypeHelper(objectFor.StepValue, typeCode4);
				return false;
			}
			return FlowControl.CheckContinueLoop(objectFor);
		}

		public static IEnumerator ForEachInArr(Array ary)
		{
			IEnumerator enumerator = ((IEnumerable)ary).GetEnumerator();
			if (enumerator == null)
			{
				throw ExceptionUtils.VbMakeException(92);
			}
			return enumerator;
		}

		public static IEnumerator ForEachInObj(object obj)
		{
			if (obj == null)
			{
				throw ExceptionUtils.VbMakeException(91);
			}
			IEnumerable enumerable;
			try
			{
				enumerable = (IEnumerable)obj;
			}
			catch (StackOverflowException ex)
			{
				throw ex;
			}
			catch (OutOfMemoryException ex2)
			{
				throw ex2;
			}
			catch (ThreadAbortException ex3)
			{
				throw ex3;
			}
			catch (Exception)
			{
				throw ExceptionUtils.MakeException1(100, obj.GetType().ToString());
			}
			IEnumerator enumerator = enumerable.GetEnumerator();
			if (enumerator == null)
			{
				throw ExceptionUtils.MakeException1(100, obj.GetType().ToString());
			}
			return enumerator;
		}

		public static bool ForEachNextObj(ref object obj, IEnumerator enumerator)
		{
			if (enumerator.MoveNext())
			{
				obj = enumerator.Current;
				return true;
			}
			obj = null;
			return false;
		}

		private static bool CheckContinueLoop(FlowControl.ObjectFor LoopFor)
		{
			bool flag;
			try
			{
				IComparable comparable = (IComparable)LoopFor.Counter;
				int num = comparable.CompareTo(LoopFor.Limit);
				if (LoopFor.PositiveStep)
				{
					if (num <= 0)
					{
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
				else if (num >= 0)
				{
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			catch (InvalidCastException ex)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_IComparable2", new string[]
				{
					"loop control variable",
					Utils.VBFriendlyName(LoopFor.Counter)
				}));
			}
			return flag;
		}

		public static void CheckForSyncLockOnValueType(object obj)
		{
			if (obj != null && obj.GetType().IsValueType)
			{
				throw new ArgumentException(Utils.GetResourceString("SyncLockRequiresReferenceType1", new string[] { Utils.VBFriendlyName(obj.GetType()) }));
			}
		}

		private sealed class ObjectFor
		{
			public object Counter;

			public object Limit;

			public object StepValue;

			public bool PositiveStep;

			public Type EnumType;
		}
	}
}

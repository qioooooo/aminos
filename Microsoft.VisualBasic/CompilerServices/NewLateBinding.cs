using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class NewLateBinding
	{
		private NewLateBinding()
		{
		}

		[DebuggerHidden]
		[DebuggerStepThrough]
		public static bool LateCanEvaluate(object instance, Type type, string memberName, object[] arguments, bool allowFunctionEvaluation, bool allowPropertyEvaluation)
		{
			Symbols.Container container;
			if (type != null)
			{
				container = new Symbols.Container(type);
			}
			else
			{
				container = new Symbols.Container(instance);
			}
			MemberInfo[] members = container.GetMembers(ref memberName, false);
			if (members.Length == 0)
			{
				return true;
			}
			if (members[0].MemberType == MemberTypes.Field)
			{
				if (arguments.Length == 0)
				{
					return true;
				}
				object fieldValue = container.GetFieldValue((FieldInfo)members[0]);
				container = new Symbols.Container(fieldValue);
				return container.IsArray || allowPropertyEvaluation;
			}
			else
			{
				if (members[0].MemberType == MemberTypes.Method)
				{
					return allowFunctionEvaluation;
				}
				return members[0].MemberType != MemberTypes.Property || allowPropertyEvaluation;
			}
		}

		[DebuggerHidden]
		[DebuggerStepThrough]
		public static object LateCall(object Instance, Type Type, string MemberName, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, bool[] CopyBack, bool IgnoreReturn)
		{
			if (Arguments == null)
			{
				Arguments = Symbols.NoArguments;
			}
			if (ArgumentNames == null)
			{
				ArgumentNames = Symbols.NoArgumentNames;
			}
			if (TypeArguments == null)
			{
				TypeArguments = Symbols.NoTypeArguments;
			}
			Symbols.Container container;
			if (Type != null)
			{
				container = new Symbols.Container(Type);
			}
			else
			{
				container = new Symbols.Container(Instance);
			}
			if (container.IsCOMObject)
			{
				return LateBinding.InternalLateCall(Instance, Type, MemberName, Arguments, ArgumentNames, CopyBack, IgnoreReturn);
			}
			BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.GetProperty;
			if (IgnoreReturn)
			{
				bindingFlags |= BindingFlags.IgnoreReturn;
			}
			OverloadResolution.ResolutionFailure resolutionFailure;
			return NewLateBinding.CallMethod(container, MemberName, Arguments, ArgumentNames, TypeArguments, CopyBack, bindingFlags, true, ref resolutionFailure);
		}

		[DebuggerStepThrough]
		[DebuggerHidden]
		public static object LateIndexGet(object Instance, object[] Arguments, string[] ArgumentNames)
		{
			OverloadResolution.ResolutionFailure resolutionFailure;
			return NewLateBinding.InternalLateIndexGet(Instance, Arguments, ArgumentNames, true, ref resolutionFailure);
		}

		internal static object InternalLateIndexGet(object Instance, object[] Arguments, string[] ArgumentNames, bool ReportErrors, ref OverloadResolution.ResolutionFailure Failure)
		{
			Failure = OverloadResolution.ResolutionFailure.None;
			if (Arguments == null)
			{
				Arguments = Symbols.NoArguments;
			}
			if (ArgumentNames == null)
			{
				ArgumentNames = Symbols.NoArgumentNames;
			}
			Symbols.Container container = new Symbols.Container(Instance);
			if (container.IsCOMObject)
			{
				return LateBinding.LateIndexGet(Instance, Arguments, ArgumentNames);
			}
			if (!container.IsArray)
			{
				return NewLateBinding.CallMethod(container, "", Arguments, ArgumentNames, Symbols.NoTypeArguments, null, BindingFlags.InvokeMethod | BindingFlags.GetProperty, ReportErrors, ref Failure);
			}
			if (ArgumentNames.Length <= 0)
			{
				return container.GetArrayValue(Arguments);
			}
			Failure = OverloadResolution.ResolutionFailure.InvalidArgument;
			if (ReportErrors)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidNamedArgs"));
			}
			return null;
		}

		[DebuggerHidden]
		[DebuggerStepThrough]
		public static object LateGet(object Instance, Type Type, string MemberName, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, bool[] CopyBack)
		{
			if (Arguments == null)
			{
				Arguments = Symbols.NoArguments;
			}
			if (ArgumentNames == null)
			{
				ArgumentNames = Symbols.NoArgumentNames;
			}
			if (TypeArguments == null)
			{
				TypeArguments = Symbols.NoTypeArguments;
			}
			Symbols.Container container;
			if (Type != null)
			{
				container = new Symbols.Container(Type);
			}
			else
			{
				container = new Symbols.Container(Instance);
			}
			BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.GetProperty;
			if (container.IsCOMObject)
			{
				return LateBinding.LateGet(Instance, Type, MemberName, Arguments, ArgumentNames, CopyBack);
			}
			MemberInfo[] members = container.GetMembers(ref MemberName, true);
			if (members[0].MemberType == MemberTypes.Field)
			{
				if (TypeArguments.Length > 0)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
				}
				object fieldValue = container.GetFieldValue((FieldInfo)members[0]);
				if (Arguments.Length == 0)
				{
					return fieldValue;
				}
				return NewLateBinding.LateIndexGet(fieldValue, Arguments, ArgumentNames);
			}
			else
			{
				if (ArgumentNames.Length > Arguments.Length || (CopyBack != null && CopyBack.Length != Arguments.Length))
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
				}
				OverloadResolution.ResolutionFailure resolutionFailure;
				Symbols.Method method = NewLateBinding.ResolveCall(container, MemberName, members, Arguments, ArgumentNames, TypeArguments, bindingFlags, false, ref resolutionFailure);
				if (resolutionFailure == OverloadResolution.ResolutionFailure.None)
				{
					return container.InvokeMethod(method, Arguments, CopyBack, bindingFlags);
				}
				if (Arguments.Length > 0)
				{
					method = NewLateBinding.ResolveCall(container, MemberName, members, Symbols.NoArguments, Symbols.NoArgumentNames, TypeArguments, bindingFlags, false, ref resolutionFailure);
					if (resolutionFailure == OverloadResolution.ResolutionFailure.None)
					{
						object obj = container.InvokeMethod(method, Symbols.NoArguments, null, bindingFlags);
						if (obj == null)
						{
							throw new MissingMemberException(Utils.GetResourceString("IntermediateLateBoundNothingResult1", new string[]
							{
								method.ToString(),
								container.VBFriendlyName
							}));
						}
						obj = NewLateBinding.InternalLateIndexGet(obj, Arguments, ArgumentNames, false, ref resolutionFailure);
						if (resolutionFailure == OverloadResolution.ResolutionFailure.None)
						{
							return obj;
						}
					}
				}
				NewLateBinding.ResolveCall(container, MemberName, members, Arguments, ArgumentNames, TypeArguments, bindingFlags, true, ref resolutionFailure);
				throw new InternalErrorException();
			}
		}

		[DebuggerHidden]
		[DebuggerStepThrough]
		public static void LateIndexSetComplex(object Instance, object[] Arguments, string[] ArgumentNames, bool OptimisticSet, bool RValueBase)
		{
			if (Arguments == null)
			{
				Arguments = Symbols.NoArguments;
			}
			if (ArgumentNames == null)
			{
				ArgumentNames = Symbols.NoArgumentNames;
			}
			Symbols.Container container = new Symbols.Container(Instance);
			if (container.IsArray)
			{
				if (ArgumentNames.Length > 0)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidNamedArgs"));
				}
				container.SetArrayValue(Arguments);
				return;
			}
			else
			{
				if (ArgumentNames.Length > Arguments.Length)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
				}
				if (Arguments.Length < 1)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
				}
				string text = "";
				if (container.IsCOMObject)
				{
					LateBinding.LateIndexSetComplex(Instance, Arguments, ArgumentNames, OptimisticSet, RValueBase);
					return;
				}
				BindingFlags bindingFlags = BindingFlags.SetProperty;
				MemberInfo[] members = container.GetMembers(ref text, true);
				OverloadResolution.ResolutionFailure resolutionFailure;
				Symbols.Method method = NewLateBinding.ResolveCall(container, text, members, Arguments, ArgumentNames, Symbols.NoTypeArguments, bindingFlags, false, ref resolutionFailure);
				if (resolutionFailure == OverloadResolution.ResolutionFailure.None)
				{
					if (RValueBase && container.IsValueType)
					{
						throw new Exception(Utils.GetResourceString("RValueBaseForValueType", new string[] { container.VBFriendlyName, container.VBFriendlyName }));
					}
					container.InvokeMethod(method, Arguments, null, bindingFlags);
					return;
				}
				else
				{
					if (OptimisticSet)
					{
						return;
					}
					NewLateBinding.ResolveCall(container, text, members, Arguments, ArgumentNames, Symbols.NoTypeArguments, bindingFlags, true, ref resolutionFailure);
					throw new InternalErrorException();
				}
			}
		}

		[DebuggerStepThrough]
		[DebuggerHidden]
		public static void LateIndexSet(object Instance, object[] Arguments, string[] ArgumentNames)
		{
			NewLateBinding.LateIndexSetComplex(Instance, Arguments, ArgumentNames, false, false);
		}

		[DebuggerHidden]
		[DebuggerStepThrough]
		public static void LateSetComplex(object Instance, Type Type, string MemberName, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, bool OptimisticSet, bool RValueBase)
		{
			NewLateBinding.LateSet(Instance, Type, MemberName, Arguments, ArgumentNames, TypeArguments, OptimisticSet, RValueBase, (CallType)0);
		}

		[DebuggerStepThrough]
		[DebuggerHidden]
		public static void LateSet(object Instance, Type Type, string MemberName, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments)
		{
			NewLateBinding.LateSet(Instance, Type, MemberName, Arguments, ArgumentNames, TypeArguments, false, false, (CallType)0);
		}

		[DebuggerStepThrough]
		[DebuggerHidden]
		public static void LateSet(object Instance, Type Type, string MemberName, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, bool OptimisticSet, bool RValueBase, CallType CallType)
		{
			if (Arguments == null)
			{
				Arguments = Symbols.NoArguments;
			}
			if (ArgumentNames == null)
			{
				ArgumentNames = Symbols.NoArgumentNames;
			}
			if (TypeArguments == null)
			{
				TypeArguments = Symbols.NoTypeArguments;
			}
			Symbols.Container container;
			if (Type != null)
			{
				container = new Symbols.Container(Type);
			}
			else
			{
				container = new Symbols.Container(Instance);
			}
			if (container.IsCOMObject)
			{
				try
				{
					LateBinding.InternalLateSet(Instance, ref Type, MemberName, Arguments, ArgumentNames, OptimisticSet, CallType);
					if (RValueBase && Type.IsValueType)
					{
						throw new Exception(Utils.GetResourceString("RValueBaseForValueType", new string[]
						{
							Utils.VBFriendlyName(Type, Instance),
							Utils.VBFriendlyName(Type, Instance)
						}));
					}
					return;
				}
				catch (MissingMemberException obj) when (OptimisticSet)
				{
					return;
				}
			}
			MemberInfo[] members = container.GetMembers(ref MemberName, true);
			if (members[0].MemberType == MemberTypes.Field)
			{
				if (TypeArguments.Length > 0)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
				}
				if (Arguments.Length != 1)
				{
					object fieldValue = container.GetFieldValue((FieldInfo)members[0]);
					NewLateBinding.LateIndexSetComplex(fieldValue, Arguments, ArgumentNames, OptimisticSet, true);
					return;
				}
				if (RValueBase && container.IsValueType)
				{
					throw new Exception(Utils.GetResourceString("RValueBaseForValueType", new string[] { container.VBFriendlyName, container.VBFriendlyName }));
				}
				container.SetFieldValue((FieldInfo)members[0], Arguments[0]);
				return;
			}
			else
			{
				BindingFlags bindingFlags = BindingFlags.SetProperty;
				if (ArgumentNames.Length > Arguments.Length)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
				}
				OverloadResolution.ResolutionFailure resolutionFailure;
				if (TypeArguments.Length == 0)
				{
					Symbols.Method method = NewLateBinding.ResolveCall(container, MemberName, members, Arguments, ArgumentNames, Symbols.NoTypeArguments, bindingFlags, false, ref resolutionFailure);
					if (resolutionFailure == OverloadResolution.ResolutionFailure.None)
					{
						if (RValueBase && container.IsValueType)
						{
							throw new Exception(Utils.GetResourceString("RValueBaseForValueType", new string[] { container.VBFriendlyName, container.VBFriendlyName }));
						}
						container.InvokeMethod(method, Arguments, null, bindingFlags);
						return;
					}
				}
				BindingFlags bindingFlags2 = BindingFlags.InvokeMethod | BindingFlags.GetProperty;
				if (resolutionFailure == OverloadResolution.ResolutionFailure.None || resolutionFailure == OverloadResolution.ResolutionFailure.MissingMember)
				{
					Symbols.Method method = NewLateBinding.ResolveCall(container, MemberName, members, Symbols.NoArguments, Symbols.NoArgumentNames, TypeArguments, bindingFlags2, false, ref resolutionFailure);
					if (resolutionFailure == OverloadResolution.ResolutionFailure.None)
					{
						object obj2 = container.InvokeMethod(method, Symbols.NoArguments, null, bindingFlags2);
						if (obj2 == null)
						{
							throw new MissingMemberException(Utils.GetResourceString("IntermediateLateBoundNothingResult1", new string[]
							{
								method.ToString(),
								container.VBFriendlyName
							}));
						}
						NewLateBinding.LateIndexSetComplex(obj2, Arguments, ArgumentNames, OptimisticSet, true);
						return;
					}
				}
				if (OptimisticSet)
				{
					return;
				}
				if (TypeArguments.Length == 0)
				{
					NewLateBinding.ResolveCall(container, MemberName, members, Arguments, ArgumentNames, TypeArguments, bindingFlags, true, ref resolutionFailure);
				}
				else
				{
					NewLateBinding.ResolveCall(container, MemberName, members, Symbols.NoArguments, Symbols.NoArgumentNames, TypeArguments, bindingFlags2, true, ref resolutionFailure);
				}
				throw new InternalErrorException();
			}
		}

		private static object CallMethod(Symbols.Container BaseReference, string MethodName, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, bool[] CopyBack, BindingFlags InvocationFlags, bool ReportErrors, ref OverloadResolution.ResolutionFailure Failure)
		{
			Failure = OverloadResolution.ResolutionFailure.None;
			if (ArgumentNames.Length > Arguments.Length || (CopyBack != null && CopyBack.Length != Arguments.Length))
			{
				Failure = OverloadResolution.ResolutionFailure.InvalidArgument;
				if (ReportErrors)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
				}
				return null;
			}
			else if (Symbols.HasFlag(InvocationFlags, BindingFlags.SetProperty) && Arguments.Length < 1)
			{
				Failure = OverloadResolution.ResolutionFailure.InvalidArgument;
				if (ReportErrors)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
				}
				return null;
			}
			else
			{
				MemberInfo[] array = BaseReference.GetMembers(ref MethodName, ReportErrors);
				if (array == null || array.Length == 0)
				{
					Failure = OverloadResolution.ResolutionFailure.MissingMember;
					if (ReportErrors)
					{
						array = BaseReference.GetMembers(ref MethodName, true);
					}
					return null;
				}
				Symbols.Method method = NewLateBinding.ResolveCall(BaseReference, MethodName, array, Arguments, ArgumentNames, TypeArguments, InvocationFlags, ReportErrors, ref Failure);
				if (Failure == OverloadResolution.ResolutionFailure.None)
				{
					return BaseReference.InvokeMethod(method, Arguments, CopyBack, InvocationFlags);
				}
				return null;
			}
		}

		internal static MethodInfo MatchesPropertyRequirements(Symbols.Method TargetProcedure, BindingFlags Flags)
		{
			if (Symbols.HasFlag(Flags, BindingFlags.SetProperty))
			{
				return TargetProcedure.AsProperty().GetSetMethod();
			}
			return TargetProcedure.AsProperty().GetGetMethod();
		}

		internal static Exception ReportPropertyMismatch(Symbols.Method TargetProcedure, BindingFlags Flags)
		{
			if (Symbols.HasFlag(Flags, BindingFlags.SetProperty))
			{
				return new MissingMemberException(Utils.GetResourceString("NoSetProperty1", new string[] { TargetProcedure.AsProperty().Name }));
			}
			return new MissingMemberException(Utils.GetResourceString("NoGetProperty1", new string[] { TargetProcedure.AsProperty().Name }));
		}

		internal static Symbols.Method ResolveCall(Symbols.Container BaseReference, string MethodName, MemberInfo[] Members, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, BindingFlags LookupFlags, bool ReportErrors, ref OverloadResolution.ResolutionFailure Failure)
		{
			Failure = OverloadResolution.ResolutionFailure.None;
			checked
			{
				if (Members[0].MemberType != MemberTypes.Method && Members[0].MemberType != MemberTypes.Property)
				{
					Failure = OverloadResolution.ResolutionFailure.InvalidTarget;
					if (ReportErrors)
					{
						throw new ArgumentException(Utils.GetResourceString("ExpressionNotProcedure", new string[] { MethodName, BaseReference.VBFriendlyName }));
					}
					return null;
				}
				else
				{
					int num = Arguments.Length;
					object obj = null;
					if (Symbols.HasFlag(LookupFlags, BindingFlags.SetProperty))
					{
						if (Arguments.Length == 0)
						{
							Failure = OverloadResolution.ResolutionFailure.InvalidArgument;
							if (ReportErrors)
							{
								throw new InvalidCastException(Utils.GetResourceString("PropertySetMissingArgument1", new string[] { MethodName }));
							}
							return null;
						}
						else
						{
							object[] array = Arguments;
							Arguments = new object[num - 2 + 1];
							Array.Copy(array, Arguments, Arguments.Length);
							obj = array[num - 1];
						}
					}
					Symbols.Method method = OverloadResolution.ResolveOverloadedCall(MethodName, Members, Arguments, ArgumentNames, TypeArguments, LookupFlags, ReportErrors, ref Failure);
					if (Failure != OverloadResolution.ResolutionFailure.None)
					{
						return null;
					}
					if (method.ArgumentsValidated || OverloadResolution.CanMatchArguments(method, Arguments, ArgumentNames, TypeArguments, false, null))
					{
						if (method.IsProperty)
						{
							if (NewLateBinding.MatchesPropertyRequirements(method, LookupFlags) == null)
							{
								Failure = OverloadResolution.ResolutionFailure.InvalidTarget;
								if (ReportErrors)
								{
									throw NewLateBinding.ReportPropertyMismatch(method, LookupFlags);
								}
								return null;
							}
						}
						else if (Symbols.HasFlag(LookupFlags, BindingFlags.SetProperty))
						{
							Failure = OverloadResolution.ResolutionFailure.InvalidTarget;
							if (ReportErrors)
							{
								throw new MissingMemberException(Utils.GetResourceString("MethodAssignment1", new string[] { method.AsMethod().Name }));
							}
							return null;
						}
						if (Symbols.HasFlag(LookupFlags, BindingFlags.SetProperty))
						{
							ParameterInfo[] parameters = NewLateBinding.GetCallTarget(method, LookupFlags).GetParameters();
							ParameterInfo parameterInfo = parameters[parameters.Length - 1];
							Symbols.Method method2 = method;
							object obj2 = obj;
							ParameterInfo parameterInfo2 = parameterInfo;
							bool flag = false;
							bool flag2 = false;
							List<string> list = null;
							bool flag3 = false;
							bool flag4 = false;
							if (!OverloadResolution.CanPassToParameter(method2, obj2, parameterInfo2, flag, flag2, list, ref flag3, ref flag4))
							{
								Failure = OverloadResolution.ResolutionFailure.InvalidArgument;
								if (ReportErrors)
								{
									string text = "";
									List<string> list2 = new List<string>();
									Symbols.Method method3 = method;
									object obj3 = obj;
									ParameterInfo parameterInfo3 = parameterInfo;
									bool flag5 = false;
									bool flag6 = false;
									List<string> list3 = list2;
									flag4 = false;
									flag3 = false;
									bool flag7 = OverloadResolution.CanPassToParameter(method3, obj3, parameterInfo3, flag5, flag6, list3, ref flag4, ref flag3);
									try
									{
										foreach (string text2 in list2)
										{
											text = text + "\r\n    " + text2;
										}
									}
									finally
									{
										List<string>.Enumerator enumerator;
										((IDisposable)enumerator).Dispose();
									}
									text = Utils.GetResourceString("MatchArgumentFailure2", new string[]
									{
										method.ToString(),
										text
									});
									throw new InvalidCastException(text);
								}
								return null;
							}
						}
						return method;
					}
					Failure = OverloadResolution.ResolutionFailure.InvalidArgument;
					if (ReportErrors)
					{
						string text3 = "";
						List<string> list4 = new List<string>();
						bool flag8 = OverloadResolution.CanMatchArguments(method, Arguments, ArgumentNames, TypeArguments, false, list4);
						try
						{
							foreach (string text4 in list4)
							{
								text3 = text3 + "\r\n    " + text4;
							}
						}
						finally
						{
							List<string>.Enumerator enumerator2;
							((IDisposable)enumerator2).Dispose();
						}
						text3 = Utils.GetResourceString("MatchArgumentFailure2", new string[]
						{
							method.ToString(),
							text3
						});
						throw new InvalidCastException(text3);
					}
					return null;
				}
			}
		}

		internal static MethodBase GetCallTarget(Symbols.Method TargetProcedure, BindingFlags Flags)
		{
			if (TargetProcedure.IsMethod)
			{
				return TargetProcedure.AsMethod();
			}
			if (TargetProcedure.IsProperty)
			{
				return NewLateBinding.MatchesPropertyRequirements(TargetProcedure, Flags);
			}
			return null;
		}

		internal static object[] ConstructCallArguments(Symbols.Method TargetProcedure, object[] Arguments, BindingFlags LookupFlags)
		{
			ParameterInfo[] parameters = NewLateBinding.GetCallTarget(TargetProcedure, LookupFlags).GetParameters();
			checked
			{
				object[] array = new object[parameters.Length - 1 + 1];
				int num = Arguments.Length;
				object obj = null;
				if (Symbols.HasFlag(LookupFlags, BindingFlags.SetProperty))
				{
					object[] array2 = Arguments;
					Arguments = new object[num - 2 + 1];
					Array.Copy(array2, Arguments, Arguments.Length);
					obj = array2[num - 1];
				}
				OverloadResolution.MatchArguments(TargetProcedure, Arguments, array);
				if (Symbols.HasFlag(LookupFlags, BindingFlags.SetProperty))
				{
					ParameterInfo parameterInfo = parameters[parameters.Length - 1];
					array[parameters.Length - 1] = OverloadResolution.PassToParameter(obj, parameterInfo, parameterInfo.ParameterType);
				}
				return array;
			}
		}
	}
}

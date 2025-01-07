using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class LateBinding
	{
		private LateBinding()
		{
		}

		private static MemberInfo GetMostDerivedMemberInfo(IReflect objIReflect, string name, BindingFlags flags)
		{
			MemberInfo[] nonGenericMembers = LateBinding.GetNonGenericMembers(objIReflect.GetMember(name, flags));
			if (nonGenericMembers == null || nonGenericMembers.Length == 0)
			{
				return null;
			}
			MemberInfo memberInfo = nonGenericMembers[0];
			int num = 1;
			int upperBound = nonGenericMembers.GetUpperBound(0);
			checked
			{
				for (int i = num; i <= upperBound; i++)
				{
					if (nonGenericMembers[i].DeclaringType.IsSubclassOf(memberInfo.DeclaringType))
					{
						memberInfo = nonGenericMembers[i];
					}
				}
				return memberInfo;
			}
		}

		[DebuggerHidden]
		[DebuggerStepThrough]
		public static object LateGet(object o, Type objType, string name, object[] args, string[] paramnames, bool[] CopyBack)
		{
			BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.OptionalParamBinding;
			if (objType == null)
			{
				if (o == null)
				{
					throw ExceptionUtils.VbMakeException(91);
				}
				objType = o.GetType();
			}
			IReflect correctIReflect = LateBinding.GetCorrectIReflect(o, objType);
			if (name == null)
			{
				name = "";
			}
			if (objType.IsCOMObject)
			{
				LateBinding.CheckForClassExtendingCOMClass(objType);
			}
			else
			{
				MemberInfo mostDerivedMemberInfo = LateBinding.GetMostDerivedMemberInfo(correctIReflect, name, bindingFlags | BindingFlags.GetField);
				if (mostDerivedMemberInfo != null && mostDerivedMemberInfo.MemberType == MemberTypes.Field)
				{
					VBBinder.SecurityCheckForLateboundCalls(mostDerivedMemberInfo, objType, correctIReflect);
					object obj;
					if (objType != correctIReflect)
					{
						if (!((FieldInfo)mostDerivedMemberInfo).IsStatic)
						{
							if (!LateBinding.DoesTargetObjectMatch(o, mostDerivedMemberInfo))
							{
								obj = LateBinding.InvokeMemberOnIReflect(correctIReflect, mostDerivedMemberInfo, BindingFlags.GetField, o, null);
								goto IL_00B0;
							}
						}
					}
					LateBinding.VerifyObjRefPresentForInstanceCall(o, mostDerivedMemberInfo);
					obj = ((FieldInfo)mostDerivedMemberInfo).GetValue(o);
					IL_00B0:
					if (args == null || args.Length == 0)
					{
						return obj;
					}
					return LateBinding.LateIndexGet(obj, args, paramnames);
				}
			}
			VBBinder vbbinder = new VBBinder(CopyBack);
			object obj2;
			try
			{
				obj2 = vbbinder.InvokeMember(name, bindingFlags, objType, correctIReflect, o, args, paramnames);
			}
			catch (Exception ex) when (LateBinding.IsMissingMemberException(ex))
			{
				if (objType.IsCOMObject || (args != null && args.Length > 0))
				{
					bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.OptionalParamBinding;
					if (!objType.IsCOMObject)
					{
						bindingFlags |= BindingFlags.GetField;
					}
					object obj4;
					try
					{
						obj4 = vbbinder.InvokeMember(name, bindingFlags, objType, correctIReflect, o, null, null);
					}
					catch (AccessViolationException ex2)
					{
						throw ex2;
					}
					catch (StackOverflowException ex3)
					{
						throw ex3;
					}
					catch (OutOfMemoryException ex4)
					{
						throw ex4;
					}
					catch (ThreadAbortException ex5)
					{
						throw ex5;
					}
					catch (Exception)
					{
						obj4 = null;
					}
					if (obj4 == null)
					{
						throw new MissingMemberException(Utils.GetResourceString("MissingMember_MemberNotFoundOnType2", new string[]
						{
							name,
							Utils.VBFriendlyName(objType, o)
						}));
					}
					try
					{
						return LateBinding.LateIndexGet(obj4, args, paramnames);
					}
					catch (Exception ex6) when ((LateBinding.IsMissingMemberException(ex6) && ex is MissingMemberException) ? 1 : 0)
					{
						throw ex;
					}
				}
				throw new MissingMemberException(Utils.GetResourceString("MissingMember_MemberNotFoundOnType2", new string[]
				{
					name,
					Utils.VBFriendlyName(objType, o)
				}));
			}
			catch (TargetInvocationException ex7)
			{
				throw ex7.InnerException;
			}
			return obj2;
		}

		private static bool IsMissingMemberException(Exception ex)
		{
			if (ex is MissingMemberException)
			{
				return true;
			}
			if (ex is MemberAccessException)
			{
				return true;
			}
			COMException ex2 = ex as COMException;
			if (ex2 != null)
			{
				if (ex2.ErrorCode == -2147352570)
				{
					return true;
				}
				if (ex2.ErrorCode == -2146827850)
				{
					return true;
				}
			}
			else if (ex is TargetInvocationException && ex.InnerException is COMException && ((COMException)ex.InnerException).ErrorCode == -2147352559)
			{
				return true;
			}
			return false;
		}

		[DebuggerStepThrough]
		[DebuggerHidden]
		public static void LateSetComplex(object o, Type objType, string name, object[] args, string[] paramnames, bool OptimisticSet, bool RValueBase)
		{
			try
			{
				LateBinding.InternalLateSet(o, ref objType, name, args, paramnames, OptimisticSet, (CallType)0);
				if (RValueBase && objType.IsValueType)
				{
					throw new Exception(Utils.GetResourceString("RValueBaseForValueType", new string[]
					{
						Utils.VBFriendlyName(objType, o),
						Utils.VBFriendlyName(objType, o)
					}));
				}
			}
			catch (MissingMemberException obj) when (OptimisticSet)
			{
			}
		}

		[DebuggerHidden]
		[DebuggerStepThrough]
		public static void LateSet(object o, Type objType, string name, object[] args, string[] paramnames)
		{
			LateBinding.InternalLateSet(o, ref objType, name, args, paramnames, false, (CallType)0);
		}

		[DebuggerHidden]
		[DebuggerStepThrough]
		internal static void InternalLateSet(object o, ref Type objType, string name, object[] args, string[] paramnames, bool OptimisticSet, CallType UseCallType)
		{
			BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.OptionalParamBinding;
			if (objType == null)
			{
				if (o == null)
				{
					throw ExceptionUtils.VbMakeException(91);
				}
				objType = o.GetType();
			}
			IReflect correctIReflect = LateBinding.GetCorrectIReflect(o, objType);
			if (name == null)
			{
				name = "";
			}
			if (objType.IsCOMObject)
			{
				LateBinding.CheckForClassExtendingCOMClass(objType);
				if (UseCallType == CallType.Set)
				{
					bindingFlags |= BindingFlags.PutRefDispProperty;
					if (args[args.GetUpperBound(0)] == null)
					{
						args[args.GetUpperBound(0)] = new DispatchWrapper(null);
					}
				}
				else if (UseCallType == CallType.Let)
				{
					bindingFlags |= BindingFlags.PutDispProperty;
				}
				else
				{
					bindingFlags |= LateBinding.GetPropertyPutFlags(args[args.GetUpperBound(0)]);
				}
			}
			else
			{
				bindingFlags |= BindingFlags.SetProperty;
				MemberInfo mostDerivedMemberInfo = LateBinding.GetMostDerivedMemberInfo(correctIReflect, name, bindingFlags | BindingFlags.SetField);
				if (mostDerivedMemberInfo != null && mostDerivedMemberInfo.MemberType == MemberTypes.Field)
				{
					FieldInfo fieldInfo = (FieldInfo)mostDerivedMemberInfo;
					if (fieldInfo.IsInitOnly)
					{
						throw new MissingMemberException(Utils.GetResourceString("MissingMember_ReadOnlyField2", new string[]
						{
							name,
							Utils.VBFriendlyName(objType, o)
						}));
					}
					if (args == null || args.Length == 0)
					{
						throw new MissingMemberException(Utils.GetResourceString("MissingMember_MemberNotFoundOnType2", new string[]
						{
							name,
							Utils.VBFriendlyName(objType, o)
						}));
					}
					if (args.Length == 1)
					{
						object obj = args[0];
						VBBinder.SecurityCheckForLateboundCalls(fieldInfo, objType, correctIReflect);
						object obj2;
						if (obj == null)
						{
							obj2 = null;
						}
						else
						{
							obj2 = ObjectType.CTypeHelper(args[0], fieldInfo.FieldType);
						}
						if (objType != correctIReflect)
						{
							if (!fieldInfo.IsStatic)
							{
								if (!LateBinding.DoesTargetObjectMatch(o, fieldInfo))
								{
									LateBinding.InvokeMemberOnIReflect(correctIReflect, fieldInfo, BindingFlags.SetField, o, new object[] { obj2 });
									return;
								}
							}
						}
						LateBinding.VerifyObjRefPresentForInstanceCall(o, fieldInfo);
						fieldInfo.SetValue(o, obj2);
						return;
					}
					if (args.Length > 1)
					{
						VBBinder.SecurityCheckForLateboundCalls(mostDerivedMemberInfo, objType, correctIReflect);
						object obj3 = null;
						if (objType != correctIReflect)
						{
							if (!((FieldInfo)mostDerivedMemberInfo).IsStatic)
							{
								if (!LateBinding.DoesTargetObjectMatch(o, mostDerivedMemberInfo))
								{
									obj3 = LateBinding.InvokeMemberOnIReflect(correctIReflect, mostDerivedMemberInfo, BindingFlags.GetField, o, new object[] { obj3 });
									goto IL_0211;
								}
							}
						}
						LateBinding.VerifyObjRefPresentForInstanceCall(o, mostDerivedMemberInfo);
						obj3 = ((FieldInfo)mostDerivedMemberInfo).GetValue(o);
						IL_0211:
						LateBinding.LateIndexSet(obj3, args, paramnames);
						return;
					}
				}
			}
			VBBinder vbbinder = new VBBinder(null);
			checked
			{
				if (OptimisticSet && args.GetUpperBound(0) > 0)
				{
					BindingFlags bindingFlags2 = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty | BindingFlags.OptionalParamBinding;
					Type[] array = new Type[args.GetUpperBound(0) - 1 + 1];
					int num = 0;
					int upperBound = array.GetUpperBound(0);
					for (int i = num; i <= upperBound; i++)
					{
						object obj4 = args[i];
						if (obj4 == null)
						{
							array[i] = null;
						}
						else
						{
							array[i] = obj4.GetType();
						}
					}
					try
					{
						PropertyInfo property = correctIReflect.GetProperty(name, bindingFlags2, vbbinder, typeof(int), array, null);
						if (property == null || !property.CanWrite)
						{
							return;
						}
					}
					catch (MissingMemberException ex)
					{
						return;
					}
				}
				try
				{
					vbbinder.InvokeMember(name, bindingFlags, objType, correctIReflect, o, args, paramnames);
				}
				catch (Exception ex2) when (LateBinding.IsMissingMemberException(ex2))
				{
					if (args == null || args.Length <= 1)
					{
						throw new MissingMemberException(Utils.GetResourceString("MissingMember_MemberNotFoundOnType2", new string[]
						{
							name,
							Utils.VBFriendlyName(objType, o)
						}));
					}
					bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty | BindingFlags.OptionalParamBinding;
					if (!objType.IsCOMObject)
					{
						bindingFlags |= BindingFlags.GetField;
					}
					object obj6;
					try
					{
						obj6 = vbbinder.InvokeMember(name, bindingFlags, objType, correctIReflect, o, null, null);
					}
					catch (Exception ex3) when ((LateBinding.IsMissingMemberException(ex3) && ex2 is MissingMemberException) ? 1 : 0)
					{
						throw ex2;
					}
					catch (AccessViolationException ex4)
					{
						throw ex4;
					}
					catch (StackOverflowException ex5)
					{
						throw ex5;
					}
					catch (OutOfMemoryException ex6)
					{
						throw ex6;
					}
					catch (ThreadAbortException ex7)
					{
						throw ex7;
					}
					catch (Exception ex8)
					{
						obj6 = null;
					}
					if (obj6 == null)
					{
						throw new MissingMemberException(Utils.GetResourceString("MissingMember_MemberNotFoundOnType2", new string[]
						{
							name,
							Utils.VBFriendlyName(objType, o)
						}));
					}
					try
					{
						LateBinding.LateIndexSet(obj6, args, paramnames);
					}
					catch (Exception ex9) when ((LateBinding.IsMissingMemberException(ex9) && ex2 is MissingMemberException) ? 1 : 0)
					{
						throw ex2;
					}
				}
				catch (TargetInvocationException ex10)
				{
					if (ex10.InnerException == null)
					{
						throw ex10;
					}
					if (!(ex10.InnerException is TargetParameterCountException))
					{
						throw ex10.InnerException;
					}
					if ((bindingFlags & BindingFlags.PutRefDispProperty) != BindingFlags.Default)
					{
						throw new MissingMemberException(Utils.GetResourceString("MissingMember_MemberSetNotFoundOnType2", new string[]
						{
							name,
							Utils.VBFriendlyName(objType, o)
						}));
					}
					throw new MissingMemberException(Utils.GetResourceString("MissingMember_MemberLetNotFoundOnType2", new string[]
					{
						name,
						Utils.VBFriendlyName(objType, o)
					}));
				}
			}
		}

		private static void CheckForClassExtendingCOMClass(Type objType)
		{
			if (!objType.IsCOMObject)
			{
				return;
			}
			if (Operators.CompareString(objType.FullName, "System.__ComObject", false) == 0)
			{
				return;
			}
			if (Operators.CompareString(objType.BaseType.FullName, "System.__ComObject", false) == 0)
			{
				return;
			}
			throw new InvalidOperationException(Utils.GetResourceString("LateboundCallToInheritedComClass"));
		}

		[DebuggerStepThrough]
		[DebuggerHidden]
		public static object LateIndexGet(object o, object[] args, string[] paramnames)
		{
			string text = null;
			if (o == null)
			{
				throw ExceptionUtils.VbMakeException(91);
			}
			Type type = o.GetType();
			IReflect correctIReflect = LateBinding.GetCorrectIReflect(o, type);
			checked
			{
				if (!type.IsArray)
				{
					MethodBase[] array = null;
					BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.OptionalParamBinding;
					if (!type.IsCOMObject)
					{
						if (args == null || args.Length == 0)
						{
							bindingFlags |= BindingFlags.GetField;
						}
						MemberInfo[] defaultMembers = LateBinding.GetDefaultMembers(type, correctIReflect, ref text);
						int num2;
						if (defaultMembers != null)
						{
							int num = 0;
							int upperBound = defaultMembers.GetUpperBound(0);
							for (int i = num; i <= upperBound; i++)
							{
								MemberInfo memberInfo = defaultMembers[i];
								if (memberInfo.MemberType == MemberTypes.Property)
								{
									memberInfo = ((PropertyInfo)memberInfo).GetGetMethod();
								}
								if (memberInfo != null && memberInfo.MemberType != MemberTypes.Field)
								{
									defaultMembers[num2] = memberInfo;
									num2++;
								}
							}
						}
						if ((defaultMembers == null) | (num2 == 0))
						{
							throw new MissingMemberException(Utils.GetResourceString("MissingMember_NoDefaultMemberFound1", new string[] { Utils.VBFriendlyName(type, o) }));
						}
						array = new MethodBase[num2 - 1 + 1];
						int num3 = 0;
						int num4 = num2 - 1;
						for (int i = num3; i <= num4; i++)
						{
							try
							{
								array[i] = (MethodBase)defaultMembers[i];
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
							catch (Exception ex4)
							{
							}
						}
					}
					else
					{
						LateBinding.CheckForClassExtendingCOMClass(type);
					}
					VBBinder vbbinder = new VBBinder(null);
					object obj;
					try
					{
						if (type.IsCOMObject)
						{
							obj = vbbinder.InvokeMember("", bindingFlags, type, correctIReflect, o, args, paramnames);
						}
						else
						{
							object obj2 = null;
							vbbinder.m_BindToName = text;
							vbbinder.m_objType = type;
							MethodBase methodBase = vbbinder.BindToMethod(bindingFlags, array, ref args, null, null, paramnames, ref obj2);
							VBBinder.SecurityCheckForLateboundCalls(methodBase, type, correctIReflect);
							object obj3;
							if (type != correctIReflect)
							{
								if (!methodBase.IsStatic)
								{
									if (!LateBinding.DoesTargetObjectMatch(o, methodBase))
									{
										obj3 = LateBinding.InvokeMemberOnIReflect(correctIReflect, methodBase, BindingFlags.InvokeMethod, o, args);
										goto IL_027A;
									}
								}
							}
							LateBinding.VerifyObjRefPresentForInstanceCall(o, methodBase);
							obj3 = methodBase.Invoke(o, args);
							IL_027A:
							vbbinder.ReorderArgumentArray(ref args, obj2);
							obj = obj3;
						}
					}
					catch (Exception ex5) when (LateBinding.IsMissingMemberException(ex5))
					{
						throw new MissingMemberException(Utils.GetResourceString("MissingMember_NoDefaultMemberFound1", new string[] { Utils.VBFriendlyName(type, o) }));
					}
					catch (TargetInvocationException ex6)
					{
						throw ex6.InnerException;
					}
					return obj;
				}
				if (paramnames != null && paramnames.Length != 0)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidNamedArgs"));
				}
				Array array2 = (Array)o;
				int num5 = args.Length;
				if (num5 != array2.Rank)
				{
					throw new RankException();
				}
				if (num5 == 1)
				{
					return array2.GetValue(Conversions.ToInteger(args[0]));
				}
				if (num5 == 2)
				{
					return array2.GetValue(Conversions.ToInteger(args[0]), Conversions.ToInteger(args[1]));
				}
				int[] array3 = new int[num5 - 1 + 1];
				int num6 = 0;
				int num7 = num5 - 1;
				for (int j = num6; j <= num7; j++)
				{
					array3[j] = Conversions.ToInteger(args[j]);
				}
				return array2.GetValue(array3);
			}
		}

		private static MemberInfo[] GetDefaultMembers(Type typ, IReflect objIReflect, ref string DefaultName)
		{
			MemberInfo[] array;
			if (typ == objIReflect)
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
						goto Block_4;
					}
				}
				DefaultName = ((DefaultMemberAttribute)customAttributes[0]).MemberName;
				array = typ.GetMember(DefaultName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				array = LateBinding.GetNonGenericMembers(array);
				if (array == null || array.Length == 0)
				{
					DefaultName = "";
					return null;
				}
				return array;
				Block_4:
				DefaultName = "";
				return null;
			}
			array = objIReflect.GetMember("", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			array = LateBinding.GetNonGenericMembers(array);
			if (array == null || array.Length == 0)
			{
				DefaultName = "";
				return null;
			}
			DefaultName = array[0].Name;
			return array;
		}

		[DebuggerHidden]
		[DebuggerStepThrough]
		public static void LateIndexSetComplex(object o, object[] args, string[] paramnames, bool OptimisticSet, bool RValueBase)
		{
			try
			{
				LateBinding.LateIndexSet(o, args, paramnames);
				if (RValueBase && o.GetType().IsValueType)
				{
					throw new Exception(Utils.GetResourceString("RValueBaseForValueType", new string[]
					{
						o.GetType().Name,
						o.GetType().Name
					}));
				}
			}
			catch (MissingMemberException obj) when (OptimisticSet)
			{
			}
		}

		[DebuggerHidden]
		[DebuggerStepThrough]
		public static void LateIndexSet(object o, object[] args, string[] paramnames)
		{
			string text = null;
			if (o == null)
			{
				throw ExceptionUtils.VbMakeException(91);
			}
			Type type = o.GetType();
			IReflect correctIReflect = LateBinding.GetCorrectIReflect(o, type);
			checked
			{
				if (type.IsArray)
				{
					if (paramnames != null && paramnames.Length != 0)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_InvalidNamedArgs"));
					}
					Array array = (Array)o;
					int num = args.Length - 1;
					object obj = args[num];
					if (obj != null)
					{
						Type elementType = type.GetElementType();
						if (obj.GetType() != elementType)
						{
							obj = ObjectType.CTypeHelper(obj, elementType);
						}
					}
					if (num != array.Rank)
					{
						throw new RankException();
					}
					if (num == 1)
					{
						array.SetValue(obj, Conversions.ToInteger(args[0]));
					}
					else if (num == 2)
					{
						array.SetValue(obj, Conversions.ToInteger(args[0]), Conversions.ToInteger(args[1]));
					}
					else
					{
						int[] array2 = new int[num - 1 + 1];
						int num2 = 0;
						int num3 = num - 1;
						for (int i = num2; i <= num3; i++)
						{
							array2[i] = Conversions.ToInteger(args[i]);
						}
						array.SetValue(obj, array2);
					}
				}
				else
				{
					MethodBase[] array3 = null;
					BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.OptionalParamBinding;
					if (type.IsCOMObject)
					{
						LateBinding.CheckForClassExtendingCOMClass(type);
						bindingFlags |= LateBinding.GetPropertyPutFlags(args[args.GetUpperBound(0)]);
					}
					else
					{
						bindingFlags |= BindingFlags.SetProperty;
						if (args.Length == 1)
						{
							bindingFlags |= BindingFlags.SetField;
						}
						MemberInfo[] defaultMembers = LateBinding.GetDefaultMembers(type, correctIReflect, ref text);
						int num5;
						if (defaultMembers != null)
						{
							int num4 = 0;
							int upperBound = defaultMembers.GetUpperBound(0);
							for (int j = num4; j <= upperBound; j++)
							{
								MemberInfo memberInfo = defaultMembers[j];
								if (memberInfo.MemberType == MemberTypes.Property)
								{
									memberInfo = ((PropertyInfo)memberInfo).GetSetMethod();
								}
								if (memberInfo != null && memberInfo.MemberType != MemberTypes.Field)
								{
									defaultMembers[num5] = memberInfo;
									num5++;
								}
							}
						}
						if ((defaultMembers == null) | (num5 == 0))
						{
							throw new MissingMemberException(Utils.GetResourceString("MissingMember_NoDefaultMemberFound1", new string[] { Utils.VBFriendlyName(type, o) }));
						}
						array3 = new MethodBase[num5 - 1 + 1];
						int num6 = 0;
						int num7 = num5 - 1;
						for (int j = num6; j <= num7; j++)
						{
							try
							{
								array3[j] = (MethodBase)defaultMembers[j];
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
							}
						}
					}
					VBBinder vbbinder = new VBBinder(null);
					try
					{
						if (type.IsCOMObject)
						{
							vbbinder.InvokeMember("", bindingFlags, type, correctIReflect, o, args, paramnames);
						}
						else
						{
							object obj2 = null;
							vbbinder.m_BindToName = text;
							vbbinder.m_objType = type;
							MethodBase methodBase = vbbinder.BindToMethod(bindingFlags, array3, ref args, null, null, paramnames, ref obj2);
							VBBinder.SecurityCheckForLateboundCalls(methodBase, type, correctIReflect);
							if (type != correctIReflect)
							{
								if (!methodBase.IsStatic)
								{
									if (!LateBinding.DoesTargetObjectMatch(o, methodBase))
									{
										LateBinding.InvokeMemberOnIReflect(correctIReflect, methodBase, BindingFlags.InvokeMethod, o, args);
										goto IL_02BF;
									}
								}
							}
							LateBinding.VerifyObjRefPresentForInstanceCall(o, methodBase);
							methodBase.Invoke(o, args);
							IL_02BF:
							vbbinder.ReorderArgumentArray(ref args, obj2);
						}
					}
					catch (Exception ex4) when (LateBinding.IsMissingMemberException(ex4))
					{
						throw new MissingMemberException(Utils.GetResourceString("MissingMember_NoDefaultMemberFound1", new string[] { Utils.VBFriendlyName(type, o) }));
					}
					catch (TargetInvocationException ex5)
					{
						throw ex5.InnerException;
					}
				}
			}
		}

		private static BindingFlags GetPropertyPutFlags(object NewValue)
		{
			if (NewValue == null)
			{
				return BindingFlags.SetProperty;
			}
			if (!(NewValue is ValueType))
			{
				if (!(NewValue is string))
				{
					if (!(NewValue is DBNull))
					{
						if (!(NewValue is Missing))
						{
							if (!(NewValue is Array))
							{
								if (!(NewValue is CurrencyWrapper))
								{
									return BindingFlags.PutRefDispProperty;
								}
							}
						}
					}
				}
			}
			return BindingFlags.PutDispProperty;
		}

		[DebuggerStepThrough]
		[DebuggerHidden]
		public static void LateCall(object o, Type objType, string name, object[] args, string[] paramnames, bool[] CopyBack)
		{
			LateBinding.InternalLateCall(o, objType, name, args, paramnames, CopyBack, true);
		}

		[DebuggerStepThrough]
		[DebuggerHidden]
		internal static object InternalLateCall(object o, Type objType, string name, object[] args, string[] paramnames, bool[] CopyBack, bool IgnoreReturn)
		{
			BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding;
			if (IgnoreReturn)
			{
				bindingFlags |= BindingFlags.IgnoreReturn;
			}
			if (objType == null)
			{
				if (o == null)
				{
					throw ExceptionUtils.VbMakeException(91);
				}
				objType = o.GetType();
			}
			IReflect correctIReflect = LateBinding.GetCorrectIReflect(o, objType);
			if (objType.IsCOMObject)
			{
				LateBinding.CheckForClassExtendingCOMClass(objType);
			}
			if (name == null)
			{
				name = "";
			}
			VBBinder vbbinder = new VBBinder(CopyBack);
			if (!objType.IsCOMObject)
			{
				MemberInfo[] membersByName = LateBinding.GetMembersByName(correctIReflect, name, bindingFlags);
				if (membersByName == null || membersByName.Length == 0)
				{
					throw new MissingMemberException(Utils.GetResourceString("MissingMember_MemberNotFoundOnType2", new string[]
					{
						name,
						Utils.VBFriendlyName(objType, o)
					}));
				}
				if (LateBinding.MemberIsField(membersByName))
				{
					throw new ArgumentException(Utils.GetResourceString("ExpressionNotProcedure", new string[]
					{
						name,
						Utils.VBFriendlyName(objType, o)
					}));
				}
				if (membersByName.Length == 1 && (paramnames == null || paramnames.Length == 0))
				{
					MemberInfo memberInfo = membersByName[0];
					if (memberInfo.MemberType == MemberTypes.Property)
					{
						memberInfo = ((PropertyInfo)memberInfo).GetGetMethod();
						if (memberInfo == null)
						{
							throw new MissingMemberException(Utils.GetResourceString("MissingMember_MemberNotFoundOnType2", new string[]
							{
								name,
								Utils.VBFriendlyName(objType, o)
							}));
						}
					}
					MethodBase methodBase = (MethodBase)memberInfo;
					ParameterInfo[] parameters = methodBase.GetParameters();
					int num = args.Length;
					int num2 = parameters.Length;
					if (num2 == num)
					{
						if (num2 == 0)
						{
							return LateBinding.FastCall(o, methodBase, parameters, args, objType, correctIReflect);
						}
						if (CopyBack == null && LateBinding.NoByrefs(parameters))
						{
							ParameterInfo parameterInfo = parameters[checked(num2 - 1)];
							if (!parameterInfo.ParameterType.IsArray)
							{
								return LateBinding.FastCall(o, methodBase, parameters, args, objType, correctIReflect);
							}
							object[] customAttributes = parameterInfo.GetCustomAttributes(typeof(ParamArrayAttribute), false);
							if (customAttributes == null || customAttributes.Length == 0)
							{
								return LateBinding.FastCall(o, methodBase, parameters, args, objType, correctIReflect);
							}
						}
					}
				}
			}
			object obj;
			try
			{
				obj = vbbinder.InvokeMember(name, bindingFlags, objType, correctIReflect, o, args, paramnames);
			}
			catch (MissingMemberException ex)
			{
				throw;
			}
			catch (Exception ex2) when (LateBinding.IsMissingMemberException(ex2))
			{
				throw new MissingMemberException(Utils.GetResourceString("MissingMember_MemberNotFoundOnType2", new string[]
				{
					name,
					Utils.VBFriendlyName(objType, o)
				}));
			}
			catch (TargetInvocationException ex3)
			{
				throw ex3.InnerException;
			}
			return obj;
		}

		private static bool NoByrefs(ParameterInfo[] parameters)
		{
			int num = 0;
			checked
			{
				int num2 = parameters.Length - 1;
				for (int i = num; i <= num2; i++)
				{
					if (parameters[i].ParameterType.IsByRef)
					{
						return false;
					}
				}
				return true;
			}
		}

		[DebuggerHidden]
		[DebuggerStepThrough]
		private static object FastCall(object o, MethodBase method, ParameterInfo[] Parameters, object[] args, Type objType, IReflect objIReflect)
		{
			int num = 0;
			int upperBound = args.GetUpperBound(0);
			checked
			{
				for (int i = num; i <= upperBound; i++)
				{
					ParameterInfo parameterInfo = Parameters[i];
					object obj = args[i];
					if (obj is Missing && parameterInfo.IsOptional)
					{
						obj = parameterInfo.DefaultValue;
					}
					args[i] = ObjectType.CTypeHelper(obj, parameterInfo.ParameterType);
				}
				VBBinder.SecurityCheckForLateboundCalls(method, objType, objIReflect);
				if (objType != objIReflect)
				{
					if (!method.IsStatic)
					{
						if (!LateBinding.DoesTargetObjectMatch(o, method))
						{
							return LateBinding.InvokeMemberOnIReflect(objIReflect, method, BindingFlags.InvokeMethod, o, args);
						}
					}
				}
				LateBinding.VerifyObjRefPresentForInstanceCall(o, method);
				return method.Invoke(o, args);
			}
		}

		private static MemberInfo[] GetMembersByName(IReflect objIReflect, string name, BindingFlags flags)
		{
			MemberInfo[] nonGenericMembers = LateBinding.GetNonGenericMembers(objIReflect.GetMember(name, flags));
			if (nonGenericMembers != null && nonGenericMembers.Length == 0)
			{
				return null;
			}
			return nonGenericMembers;
		}

		private static bool MemberIsField(MemberInfo[] mi)
		{
			int num = 0;
			int upperBound = mi.GetUpperBound(0);
			checked
			{
				for (int i = num; i <= upperBound; i++)
				{
					MemberInfo memberInfo = mi[i];
					if (memberInfo != null)
					{
						if (memberInfo.MemberType == MemberTypes.Field)
						{
							int num2 = 0;
							int upperBound2 = mi.GetUpperBound(0);
							for (int j = num2; j <= upperBound2; j++)
							{
								if (i != j && mi[j] != null && memberInfo.DeclaringType.IsSubclassOf(mi[j].DeclaringType))
								{
									mi[j] = null;
								}
							}
						}
					}
				}
				foreach (MemberInfo memberInfo in mi)
				{
					MemberInfo memberInfo;
					if (memberInfo != null && memberInfo.MemberType != MemberTypes.Field)
					{
						return false;
					}
				}
				return true;
			}
		}

		internal static bool DoesTargetObjectMatch(object Value, MemberInfo Member)
		{
			return Value == null || Member.DeclaringType.IsAssignableFrom(Value.GetType());
		}

		internal static object InvokeMemberOnIReflect(IReflect objIReflect, MemberInfo member, BindingFlags flags, object target, object[] args)
		{
			VBBinder vbbinder = new VBBinder(null);
			vbbinder.CacheMember(member);
			return objIReflect.InvokeMember(member.Name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.OptionalParamBinding | flags, vbbinder, target, args, null, null, null);
		}

		private static IReflect GetCorrectIReflect(object o, Type objType)
		{
			if (o != null && !objType.IsCOMObject && !RemotingServices.IsTransparentProxy(o) && !(o is Type))
			{
				IReflect reflect = o as IReflect;
				if (reflect != null)
				{
					return reflect;
				}
			}
			return objType;
		}

		internal static void VerifyObjRefPresentForInstanceCall(object Value, MemberInfo Member)
		{
			if (Value == null)
			{
				bool flag = true;
				switch (Member.MemberType)
				{
				case MemberTypes.Constructor:
					flag = ((ConstructorInfo)Member).IsStatic;
					break;
				case MemberTypes.Field:
					flag = ((FieldInfo)Member).IsStatic;
					break;
				case MemberTypes.Method:
					flag = ((MethodInfo)Member).IsStatic;
					break;
				}
				if (!flag)
				{
					throw new NullReferenceException(Utils.GetResourceString("NullReference_InstanceReqToAccessMember1", new string[] { Utils.MemberToString(Member) }));
				}
			}
		}

		internal static MemberInfo[] GetNonGenericMembers(MemberInfo[] Members)
		{
			checked
			{
				if (Members != null && Members.Length > 0)
				{
					int num = 0;
					int num2 = 0;
					int upperBound = Members.GetUpperBound(0);
					for (int i = num2; i <= upperBound; i++)
					{
						if (LateBinding.LegacyIsGeneric(Members[i]))
						{
							Members[i] = null;
						}
						else
						{
							num++;
						}
					}
					if (num == Members.GetUpperBound(0) + 1)
					{
						return Members;
					}
					if (num > 0)
					{
						MemberInfo[] array = new MemberInfo[num - 1 + 1];
						int num3 = 0;
						int num4 = 0;
						int upperBound2 = Members.GetUpperBound(0);
						for (int j = num4; j <= upperBound2; j++)
						{
							if (Members[j] != null)
							{
								array[num3] = Members[j];
								num3++;
							}
						}
						return array;
					}
				}
				return null;
			}
		}

		internal static bool LegacyIsGeneric(MemberInfo Member)
		{
			MethodBase methodBase = Member as MethodBase;
			return methodBase != null && methodBase.IsGenericMethod;
		}

		private const CallType DefaultCallType = (CallType)0;

		private const BindingFlags VBLateBindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.OptionalParamBinding;
	}
}

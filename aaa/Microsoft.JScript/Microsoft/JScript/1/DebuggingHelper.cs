using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000068 RID: 104
	internal static class DebuggingHelper
	{
		// Token: 0x0600050C RID: 1292 RVA: 0x00024B40 File Offset: 0x00023B40
		public static DynamicFieldInfo[] GetHashTableFields(SimpleHashtable h)
		{
			DynamicFieldInfo[] array = null;
			try
			{
				int count = h.count;
				array = new DynamicFieldInfo[count];
				IDictionaryEnumerator enumerator = h.GetEnumerator();
				int num = 0;
				while (num < count && enumerator.MoveNext())
				{
					array[num] = new DynamicFieldInfo((string)enumerator.Key, enumerator.Value);
					num++;
				}
			}
			catch
			{
				array = new DynamicFieldInfo[0];
			}
			return array;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00024BB0 File Offset: 0x00023BB0
		public static DynamicFieldInfo[] GetExpandoObjectFields(object o, bool hideNamespaces)
		{
			IReflect reflect = o as IReflect;
			if (reflect == null)
			{
				return new DynamicFieldInfo[0];
			}
			DynamicFieldInfo[] array2;
			try
			{
				FieldInfo[] fields = reflect.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
				ArrayList arrayList = new ArrayList();
				foreach (FieldInfo fieldInfo in fields)
				{
					bool flag = false;
					foreach (object obj in arrayList)
					{
						if (fieldInfo.Name == ((DynamicFieldInfo)obj).name)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						object value = fieldInfo.GetValue(o);
						if (!hideNamespaces || !(value is Namespace))
						{
							arrayList.Add(new DynamicFieldInfo(fieldInfo.Name, value, fieldInfo.FieldType.Name));
						}
					}
				}
				array2 = (DynamicFieldInfo[])arrayList.ToArray(typeof(DynamicFieldInfo));
			}
			catch
			{
				array2 = new DynamicFieldInfo[0];
			}
			return array2;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00024CCC File Offset: 0x00023CCC
		public static object CallMethod(string name, object thisob, object[] arguments, VsaEngine engine)
		{
			if (engine == null)
			{
				engine = VsaEngine.CreateEngine();
			}
			LateBinding lateBinding = new LateBinding(name, thisob, true);
			return lateBinding.Call(arguments, false, false, engine);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00024CF8 File Offset: 0x00023CF8
		public static object CallStaticMethod(string name, string typename, object[] arguments, VsaEngine engine)
		{
			if (engine == null)
			{
				engine = VsaEngine.CreateEngine();
			}
			object type = DebuggingHelper.GetType(typename);
			LateBinding lateBinding = new LateBinding(name, type, true);
			return lateBinding.Call(arguments, false, false, engine);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00024D2C File Offset: 0x00023D2C
		public static object CallConstructor(string typename, object[] arguments, VsaEngine engine)
		{
			if (engine == null)
			{
				engine = VsaEngine.CreateEngine();
			}
			object type = DebuggingHelper.GetType(typename);
			return LateBinding.CallValue(null, type, arguments, true, false, engine);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00024D58 File Offset: 0x00023D58
		private static Type GetTypeInCurrentAppDomain(string typename)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (!(assembly is AssemblyBuilder))
				{
					Type type = assembly.GetType(typename);
					if (type != null)
					{
						return type;
					}
				}
			}
			return null;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00024DA8 File Offset: 0x00023DA8
		private static Type GetType(string typename)
		{
			string[] array = typename.Split(new char[] { '.' });
			if (array != null && array.Length > 0)
			{
				string text = array[0];
				Type type = DebuggingHelper.GetTypeInCurrentAppDomain(text);
				int num = 1;
				while (num < array.Length && type == null)
				{
					text = text + "." + array[num];
					type = DebuggingHelper.GetTypeInCurrentAppDomain(text);
					num++;
				}
				int num2 = num;
				while (num2 < array.Length && type != null)
				{
					type = type.GetNestedType(array[num2], BindingFlags.Public | BindingFlags.NonPublic);
					num2++;
				}
				return type;
			}
			return null;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00024E2C File Offset: 0x00023E2C
		public static void SetIndexedPropertyValue(string name, object thisob, object[] arguments, object value, VsaEngine engine)
		{
			LateBinding lateBinding = new LateBinding(name, thisob, true);
			lateBinding.SetIndexedPropertyValue(arguments, value);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00024E4C File Offset: 0x00023E4C
		public static void SetStaticIndexedPropertyValue(string name, string typename, object[] arguments, object value, VsaEngine engine)
		{
			object type = DebuggingHelper.GetType(typename);
			LateBinding lateBinding = new LateBinding(name, type, true);
			lateBinding.SetIndexedPropertyValue(arguments, value);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00024E74 File Offset: 0x00023E74
		public static void SetDefaultIndexedPropertyValue(object thisob, object[] arguments, VsaEngine engine, string[] namedParameters)
		{
			object obj = null;
			int num = arguments.Length;
			if (num > 0)
			{
				obj = arguments[num - 1];
			}
			int num2 = 0;
			int num3 = num - 1;
			if (namedParameters != null && namedParameters.Length > 0 && namedParameters[0] == "this")
			{
				num3--;
				num2 = 1;
			}
			object[] array = new object[num3];
			ArrayObject.Copy(arguments, num2, array, 0, num3);
			LateBinding lateBinding = new LateBinding(null, thisob, true);
			lateBinding.SetIndexedPropertyValue(array, obj);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00024EE0 File Offset: 0x00023EE0
		public static object GetDefaultIndexedPropertyValue(object thisob, object[] arguments, VsaEngine engine, string[] namedParameters)
		{
			if (engine == null)
			{
				engine = VsaEngine.CreateEngine();
			}
			int num = ((arguments == null) ? 0 : arguments.Length);
			object[] array;
			if (namedParameters != null && namedParameters.Length > 0 && namedParameters[0] == "this" && num > 0)
			{
				array = new object[num - 1];
				ArrayObject.Copy(arguments, 1, array, 0, num - 1);
			}
			else
			{
				array = arguments;
			}
			LateBinding lateBinding = new LateBinding(null, thisob, true);
			return lateBinding.Call(array, false, false, engine);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00024F4C File Offset: 0x00023F4C
		public static object InvokeCOMObject(string name, object obj, object[] arguments, BindingFlags invokeAttr)
		{
			Type type = obj.GetType();
			return type.InvokeMember(name, invokeAttr, JSBinder.ob, obj, arguments, null, null, null);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00024F72 File Offset: 0x00023F72
		public static void Print(string message, VsaEngine engine)
		{
			if (engine != null && engine.doPrint)
			{
				ScriptStream.Out.Write(message);
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00024F8C File Offset: 0x00023F8C
		public static object GetClosureInstance(VsaEngine engine)
		{
			if (engine == null)
			{
				return null;
			}
			StackFrame stackFrame = engine.ScriptObjectStackTop() as StackFrame;
			if (stackFrame != null)
			{
				return stackFrame.closureInstance;
			}
			return null;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00024FB8 File Offset: 0x00023FB8
		public static object InvokeMethodInfo(MethodInfo m, object[] arguments, bool construct, object thisob, VsaEngine engine)
		{
			if (engine == null)
			{
				engine = VsaEngine.CreateEngine();
			}
			return LateBinding.CallOneOfTheMembers(new MemberInfo[] { m }, arguments, construct, thisob, JSBinder.ob, null, null, engine);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00024FED File Offset: 0x00023FED
		public static VsaEngine CreateEngine()
		{
			return VsaEngine.CreateEngineForDebugger();
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00024FF4 File Offset: 0x00023FF4
		public static object ToNativeArray(string elementTypename, object arrayObject)
		{
			Type type = DebuggingHelper.GetType(elementTypename);
			if (type == null)
			{
				throw new JScriptException(JSError.TypeMismatch);
			}
			ArrayObject arrayObject2 = arrayObject as ArrayObject;
			if (arrayObject2 != null)
			{
				return arrayObject2.ToNativeArray(type);
			}
			throw new JScriptException(JSError.TypeMismatch);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0002502C File Offset: 0x0002402C
		public static object[] CreateArray(int length)
		{
			object[] array = new object[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = new object();
			}
			return array;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00025058 File Offset: 0x00024058
		public static string[] CreateStringArray(string s)
		{
			return new string[] { s };
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00025071 File Offset: 0x00024071
		public static object StringToObject(string s)
		{
			return s;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00025074 File Offset: 0x00024074
		public static object BooleanToObject(bool i)
		{
			return i;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0002507C File Offset: 0x0002407C
		public static object SByteToObject(sbyte i)
		{
			return i;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00025084 File Offset: 0x00024084
		public static object ByteToObject(byte i)
		{
			return i;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0002508C File Offset: 0x0002408C
		public static object Int16ToObject(short i)
		{
			return i;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00025094 File Offset: 0x00024094
		public static object UInt16ToObject(ushort i)
		{
			return i;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0002509C File Offset: 0x0002409C
		public static object Int32ToObject(int i)
		{
			return i;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000250A4 File Offset: 0x000240A4
		public static object UInt32ToObject(uint i)
		{
			return i;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x000250AC File Offset: 0x000240AC
		public static object Int64ToObject(long i)
		{
			return i;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x000250B4 File Offset: 0x000240B4
		public static object UInt64ToObject(ulong i)
		{
			return i;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x000250BC File Offset: 0x000240BC
		public static object SingleToObject(float i)
		{
			return i;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x000250C4 File Offset: 0x000240C4
		public static object DoubleToObject(double i)
		{
			return i;
		}
	}
}

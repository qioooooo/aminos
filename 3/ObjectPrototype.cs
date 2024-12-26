using System;
using System.Reflection;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000D7 RID: 215
	public class ObjectPrototype : JSObject
	{
		// Token: 0x060009B1 RID: 2481 RVA: 0x0004A634 File Offset: 0x00049634
		internal ObjectPrototype()
			: base(null)
		{
			if (Globals.contextEngine == null)
			{
				this.engine = new VsaEngine(true);
				this.engine.InitVsaEngine("JS7://Microsoft.JScript.Vsa.VsaEngine", new DefaultVsaSite());
				return;
			}
			this.engine = Globals.contextEngine;
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x0004A671 File Offset: 0x00049671
		public static ObjectConstructor constructor
		{
			get
			{
				return ObjectPrototype._constructor;
			}
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0004A678 File Offset: 0x00049678
		internal static ObjectPrototype CommonInstance()
		{
			return ObjectPrototype.ob;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0004A680 File Offset: 0x00049680
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Object_hasOwnProperty)]
		public static bool hasOwnProperty(object thisob, object name)
		{
			string text = Convert.ToString(name);
			if (thisob is ArrayObject)
			{
				long num = ArrayObject.Array_index_for(text);
				if (num >= 0L)
				{
					object valueAtIndex = ((ArrayObject)thisob).GetValueAtIndex((uint)num);
					return valueAtIndex != null && valueAtIndex != Missing.Value;
				}
			}
			if (thisob is JSObject)
			{
				MemberInfo[] member = ((JSObject)thisob).GetMember(text, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
				int num2 = member.Length;
				return num2 > 1 || (num2 >= 1 && (!(member[0] is JSPrototypeField) || !(((JSPrototypeField)member[0]).value is Missing)));
			}
			return !(LateBinding.GetMemberValue(thisob, text) is Missing);
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0004A728 File Offset: 0x00049728
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Object_isPrototypeOf)]
		public static bool isPrototypeOf(object thisob, object ob)
		{
			if (thisob is ScriptObject && ob is ScriptObject)
			{
				while (ob != null)
				{
					if (ob == thisob)
					{
						return true;
					}
					ob = ((ScriptObject)ob).GetParent();
				}
			}
			return false;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0004A754 File Offset: 0x00049754
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Object_propertyIsEnumerable)]
		public static bool propertyIsEnumerable(object thisob, object name)
		{
			string text = Convert.ToString(name);
			if (thisob is ArrayObject)
			{
				long num = ArrayObject.Array_index_for(text);
				if (num >= 0L)
				{
					object valueAtIndex = ((ArrayObject)thisob).GetValueAtIndex((uint)num);
					return valueAtIndex != null && valueAtIndex != Missing.Value;
				}
			}
			if (thisob is JSObject)
			{
				FieldInfo field = ((JSObject)thisob).GetField(text, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
				return field != null && field is JSExpandoField;
			}
			return false;
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0004A7C2 File Offset: 0x000497C2
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Object_toLocaleString)]
		public static string toLocaleString(object thisob)
		{
			return Convert.ToString(thisob);
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0004A7CA File Offset: 0x000497CA
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Object_toString)]
		public static string toString(object thisob)
		{
			if (thisob is JSObject)
			{
				return "[object " + ((JSObject)thisob).GetClassName() + "]";
			}
			return "[object " + thisob.GetType().Name + "]";
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0004A809 File Offset: 0x00049809
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Object_valueOf)]
		public static object valueOf(object thisob)
		{
			return thisob;
		}

		// Token: 0x04000619 RID: 1561
		internal static readonly ObjectPrototype ob = new ObjectPrototype();

		// Token: 0x0400061A RID: 1562
		internal static ObjectConstructor _constructor;
	}
}

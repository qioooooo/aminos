using System;
using System.Globalization;
using System.Text;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000014 RID: 20
	public class ArrayPrototype : ArrayObject
	{
		// Token: 0x060000ED RID: 237 RVA: 0x000061F7 File Offset: 0x000051F7
		internal ArrayPrototype(ObjectPrototype parent)
			: base(parent)
		{
			this.noExpando = true;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006208 File Offset: 0x00005208
		[JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs | JSFunctionAttributeEnum.HasEngine, JSBuiltin.Array_concat)]
		public static ArrayObject concat(object thisob, VsaEngine engine, params object[] args)
		{
			ArrayObject arrayObject = engine.GetOriginalArrayConstructor().Construct();
			if (thisob is ArrayObject)
			{
				arrayObject.Concat((ArrayObject)thisob);
			}
			else
			{
				arrayObject.Concat(thisob);
			}
			foreach (object obj in args)
			{
				if (obj is ArrayObject)
				{
					arrayObject.Concat((ArrayObject)obj);
				}
				else
				{
					arrayObject.Concat(obj);
				}
			}
			return arrayObject;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000EF RID: 239 RVA: 0x0000626E File Offset: 0x0000526E
		public static ArrayConstructor constructor
		{
			get
			{
				return ArrayPrototype._constructor;
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006275 File Offset: 0x00005275
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_join)]
		public static string join(object thisob, object separator)
		{
			if (separator is Missing)
			{
				return ArrayPrototype.Join(thisob, ",", false);
			}
			return ArrayPrototype.Join(thisob, Convert.ToString(separator), false);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000629C File Offset: 0x0000529C
		internal static string Join(object thisob, string separator, bool localize)
		{
			StringBuilder stringBuilder = new StringBuilder();
			uint num = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
			if (num > 2147483647U)
			{
				throw new JScriptException(JSError.OutOfMemory);
			}
			if ((ulong)num > (ulong)((long)stringBuilder.Capacity))
			{
				stringBuilder.Capacity = (int)num;
			}
			for (uint num2 = 0U; num2 < num; num2 += 1U)
			{
				object valueAtIndex = LateBinding.GetValueAtIndex(thisob, (ulong)num2);
				if (valueAtIndex != null && !(valueAtIndex is Missing))
				{
					if (localize)
					{
						stringBuilder.Append(Convert.ToLocaleString(valueAtIndex));
					}
					else
					{
						stringBuilder.Append(Convert.ToString(valueAtIndex));
					}
				}
				if (num2 < num - 1U)
				{
					stringBuilder.Append(separator);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006334 File Offset: 0x00005334
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_pop)]
		public static object pop(object thisob)
		{
			uint num = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
			if (num == 0U)
			{
				LateBinding.SetMemberValue(thisob, "length", 0);
				return null;
			}
			object valueAtIndex = LateBinding.GetValueAtIndex(thisob, (ulong)(num - 1U));
			LateBinding.DeleteValueAtIndex(thisob, (ulong)(num - 1U));
			LateBinding.SetMemberValue(thisob, "length", num - 1U);
			return valueAtIndex;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006394 File Offset: 0x00005394
		[JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.Array_push)]
		public static long push(object thisob, params object[] args)
		{
			uint num = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)args.Length))
			{
				LateBinding.SetValueAtIndex(thisob, (ulong)num2 + (ulong)num, args[(int)((UIntPtr)num2)]);
				num2 += 1U;
			}
			long num3 = (long)((ulong)num + (ulong)((long)args.Length));
			LateBinding.SetMemberValue(thisob, "length", num3);
			return num3;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000063EC File Offset: 0x000053EC
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_reverse)]
		public static object reverse(object thisob)
		{
			uint num = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
			uint num2 = num / 2U;
			uint num3 = 0U;
			uint num4 = num - 1U;
			while (num3 < num2)
			{
				LateBinding.SwapValues(thisob, num3, num4);
				num3 += 1U;
				num4 -= 1U;
			}
			return thisob;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000642B File Offset: 0x0000542B
		internal override void SetMemberValue(string name, object value)
		{
			if (this.noExpando)
			{
				throw new JScriptException(JSError.OLENoPropOrMethod);
			}
			base.SetMemberValue(name, value);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00006448 File Offset: 0x00005448
		internal override void SetValueAtIndex(uint index, object value)
		{
			if (this.noExpando)
			{
				throw new JScriptException(JSError.OLENoPropOrMethod);
			}
			base.SetValueAtIndex(index, value);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006468 File Offset: 0x00005468
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_shift)]
		public static object shift(object thisob)
		{
			object obj = null;
			if (thisob is ArrayObject)
			{
				return ((ArrayObject)thisob).Shift();
			}
			uint num = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
			if (num == 0U)
			{
				LateBinding.SetMemberValue(thisob, "length", 0);
				return obj;
			}
			obj = LateBinding.GetValueAtIndex(thisob, 0UL);
			for (uint num2 = 1U; num2 < num; num2 += 1U)
			{
				object valueAtIndex = LateBinding.GetValueAtIndex(thisob, (ulong)num2);
				if (valueAtIndex is Missing)
				{
					LateBinding.DeleteValueAtIndex(thisob, (ulong)(num2 - 1U));
				}
				else
				{
					LateBinding.SetValueAtIndex(thisob, (ulong)(num2 - 1U), valueAtIndex);
				}
			}
			LateBinding.DeleteValueAtIndex(thisob, (ulong)(num - 1U));
			LateBinding.SetMemberValue(thisob, "length", num - 1U);
			return obj;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00006510 File Offset: 0x00005510
		[JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasEngine, JSBuiltin.Array_slice)]
		public static ArrayObject slice(object thisob, VsaEngine engine, double start, object end)
		{
			ArrayObject arrayObject = engine.GetOriginalArrayConstructor().Construct();
			uint num = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
			long num2 = Runtime.DoubleToInt64(Convert.ToInteger(start));
			if (num2 < 0L)
			{
				num2 = (long)((ulong)num + (ulong)num2);
				if (num2 < 0L)
				{
					num2 = 0L;
				}
			}
			else if (num2 > (long)((ulong)num))
			{
				num2 = (long)((ulong)num);
			}
			long num3 = (long)((ulong)num);
			if (end != null && !(end is Missing))
			{
				num3 = Runtime.DoubleToInt64(Convert.ToInteger(end));
				if (num3 < 0L)
				{
					num3 = (long)((ulong)num + (ulong)num3);
					if (num3 < 0L)
					{
						num3 = 0L;
					}
				}
				else if (num3 > (long)((ulong)num))
				{
					num3 = (long)((ulong)num);
				}
			}
			if (num3 > num2)
			{
				arrayObject.length = num3 - num2;
				ulong num4 = (ulong)num2;
				ulong num5 = 0UL;
				while (num4 < (ulong)num3)
				{
					object valueAtIndex = LateBinding.GetValueAtIndex(thisob, num4);
					if (!(valueAtIndex is Missing))
					{
						LateBinding.SetValueAtIndex(arrayObject, num5, valueAtIndex);
					}
					num4 += 1UL;
					num5 += 1UL;
				}
			}
			return arrayObject;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000065E4 File Offset: 0x000055E4
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_sort)]
		public static object sort(object thisob, object function)
		{
			ScriptFunction scriptFunction = null;
			if (function is ScriptFunction)
			{
				scriptFunction = (ScriptFunction)function;
			}
			uint num = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
			if (thisob is ArrayObject)
			{
				((ArrayObject)thisob).Sort(scriptFunction);
			}
			else if (num <= 2147483647U)
			{
				QuickSort quickSort = new QuickSort(thisob, scriptFunction);
				quickSort.SortObject(0L, (long)((ulong)num));
			}
			return thisob;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006644 File Offset: 0x00005644
		[JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs | JSFunctionAttributeEnum.HasEngine, JSBuiltin.Array_splice)]
		public static ArrayObject splice(object thisob, VsaEngine engine, double start, double deleteCnt, params object[] args)
		{
			uint num = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
			long num2 = Runtime.DoubleToInt64(Convert.ToInteger(start));
			if (num2 < 0L)
			{
				num2 = (long)((ulong)num + (ulong)num2);
				if (num2 < 0L)
				{
					num2 = 0L;
				}
			}
			else if (num2 > (long)((ulong)num))
			{
				num2 = (long)((ulong)num);
			}
			long num3 = Runtime.DoubleToInt64(Convert.ToInteger(deleteCnt));
			if (num3 < 0L)
			{
				num3 = 0L;
			}
			else if (num3 > (long)((ulong)num - (ulong)num2))
			{
				num3 = (long)((ulong)num - (ulong)num2);
			}
			long num4 = (long)((ulong)num + (ulong)((long)args.Length) - (ulong)num3);
			ArrayObject arrayObject = engine.GetOriginalArrayConstructor().Construct();
			arrayObject.length = num3;
			if (thisob is ArrayObject)
			{
				((ArrayObject)thisob).Splice((uint)num2, (uint)num3, args, arrayObject, num, (uint)num4);
				return arrayObject;
			}
			for (ulong num5 = 0UL; num5 < (ulong)num3; num5 += 1UL)
			{
				arrayObject.SetValueAtIndex((uint)num5, LateBinding.GetValueAtIndex(thisob, num5 + (ulong)num2));
			}
			long num6 = (long)((ulong)num - (ulong)num2 - (ulong)num3);
			if (num4 < (long)((ulong)num))
			{
				for (long num7 = 0L; num7 < num6; num7 += 1L)
				{
					LateBinding.SetValueAtIndex(thisob, (ulong)(num7 + num2 + (long)args.Length), LateBinding.GetValueAtIndex(thisob, (ulong)(num7 + num2 + num3)));
				}
				LateBinding.SetMemberValue(thisob, "length", num4);
			}
			else
			{
				LateBinding.SetMemberValue(thisob, "length", num4);
				for (long num8 = num6 - 1L; num8 >= 0L; num8 -= 1L)
				{
					LateBinding.SetValueAtIndex(thisob, (ulong)(num8 + num2 + (long)args.Length), LateBinding.GetValueAtIndex(thisob, (ulong)(num8 + num2 + num3)));
				}
			}
			int num9 = ((args == null) ? 0 : args.Length);
			uint num10 = 0U;
			while ((ulong)num10 < (ulong)((long)num9))
			{
				LateBinding.SetValueAtIndex(thisob, (ulong)num10 + (ulong)num2, args[(int)((UIntPtr)num10)]);
				num10 += 1U;
			}
			return arrayObject;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000067E0 File Offset: 0x000057E0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_toLocaleString)]
		public static string toLocaleString(object thisob)
		{
			if (thisob is ArrayObject)
			{
				StringBuilder stringBuilder = new StringBuilder(CultureInfo.CurrentCulture.TextInfo.ListSeparator);
				if (stringBuilder[stringBuilder.Length - 1] != ' ')
				{
					stringBuilder.Append(' ');
				}
				return ArrayPrototype.Join(thisob, stringBuilder.ToString(), true);
			}
			throw new JScriptException(JSError.NeedArrayObject);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000683D File Offset: 0x0000583D
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Array_toString)]
		public static string toString(object thisob)
		{
			if (thisob is ArrayObject)
			{
				return ArrayPrototype.Join(thisob, ",", false);
			}
			throw new JScriptException(JSError.NeedArrayObject);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00006860 File Offset: 0x00005860
		[JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.Array_unshift)]
		public static object unshift(object thisob, params object[] args)
		{
			if (args == null || args.Length == 0)
			{
				return thisob;
			}
			if (thisob is ArrayObject)
			{
				return ((ArrayObject)thisob).Unshift(args);
			}
			uint num = Convert.ToUint32(LateBinding.GetMemberValue(thisob, "length"));
			long num2 = (long)((ulong)num + (ulong)((long)args.Length));
			LateBinding.SetMemberValue(thisob, "length", num2);
			for (long num3 = (long)((ulong)(num - 1U)); num3 >= 0L; num3 -= 1L)
			{
				object valueAtIndex = LateBinding.GetValueAtIndex(thisob, (ulong)num3);
				if (valueAtIndex is Missing)
				{
					LateBinding.DeleteValueAtIndex(thisob, (ulong)(num3 + (long)args.Length));
				}
				else
				{
					LateBinding.SetValueAtIndex(thisob, (ulong)(num3 + (long)args.Length), valueAtIndex);
				}
			}
			uint num4 = 0U;
			while ((ulong)num4 < (ulong)((long)args.Length))
			{
				LateBinding.SetValueAtIndex(thisob, (ulong)num4, args[(int)((UIntPtr)num4)]);
				num4 += 1U;
			}
			return thisob;
		}

		// Token: 0x04000039 RID: 57
		internal static readonly ArrayPrototype ob = new ArrayPrototype(ObjectPrototype.ob);

		// Token: 0x0400003A RID: 58
		internal static ArrayConstructor _constructor;
	}
}

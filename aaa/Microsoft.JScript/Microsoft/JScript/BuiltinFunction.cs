using System;
using System.Reflection;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000034 RID: 52
	internal sealed class BuiltinFunction : ScriptFunction
	{
		// Token: 0x06000203 RID: 515 RVA: 0x0000F1B2 File Offset: 0x0000E1B2
		internal BuiltinFunction(object obj, MethodInfo method)
			: this(method.Name, obj, method, FunctionPrototype.ob)
		{
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000F1C8 File Offset: 0x0000E1C8
		internal BuiltinFunction(string name, object obj, MethodInfo method, ScriptFunction parent)
			: base(parent, name)
		{
			this.noExpando = false;
			ParameterInfo[] parameters = method.GetParameters();
			this.ilength = parameters.Length;
			object[] customAttributes = CustomAttribute.GetCustomAttributes(method, typeof(JSFunctionAttribute), false);
			JSFunctionAttribute jsfunctionAttribute = ((customAttributes.Length > 0) ? ((JSFunctionAttribute)customAttributes[0]) : new JSFunctionAttribute(JSFunctionAttributeEnum.None));
			JSFunctionAttributeEnum attributeValue = jsfunctionAttribute.attributeValue;
			if ((attributeValue & JSFunctionAttributeEnum.HasThisObject) != JSFunctionAttributeEnum.None)
			{
				this.ilength--;
			}
			if ((attributeValue & JSFunctionAttributeEnum.HasEngine) != JSFunctionAttributeEnum.None)
			{
				this.ilength--;
			}
			if ((attributeValue & JSFunctionAttributeEnum.HasVarArgs) != JSFunctionAttributeEnum.None)
			{
				this.ilength--;
			}
			this.biFunc = jsfunctionAttribute.builtinFunction;
			if (this.biFunc == JSBuiltin.None)
			{
				this.method = new JSNativeMethod(method, obj, this.engine);
				return;
			}
			this.method = null;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000F28F File Offset: 0x0000E28F
		internal override object Call(object[] args, object thisob)
		{
			return BuiltinFunction.QuickCall(args, thisob, this.biFunc, this.method, this.engine);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000F2AC File Offset: 0x0000E2AC
		internal static object QuickCall(object[] args, object thisob, JSBuiltin biFunc, MethodInfo method, VsaEngine engine)
		{
			int num = args.Length;
			switch (biFunc)
			{
			case JSBuiltin.Array_concat:
				return ArrayPrototype.concat(thisob, engine, args);
			case JSBuiltin.Array_join:
				return ArrayPrototype.join(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Array_pop:
				return ArrayPrototype.pop(thisob);
			case JSBuiltin.Array_push:
				return ArrayPrototype.push(thisob, args);
			case JSBuiltin.Array_reverse:
				return ArrayPrototype.reverse(thisob);
			case JSBuiltin.Array_shift:
				return ArrayPrototype.shift(thisob);
			case JSBuiltin.Array_slice:
				return ArrayPrototype.slice(thisob, engine, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.Array_sort:
				return ArrayPrototype.sort(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Array_splice:
				return ArrayPrototype.splice(thisob, engine, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), Convert.ToNumber(BuiltinFunction.GetArg(args, 1, num)), BuiltinFunction.VarArgs(args, 2, num));
			case JSBuiltin.Array_toLocaleString:
				return ArrayPrototype.toLocaleString(thisob);
			case JSBuiltin.Array_toString:
				return ArrayPrototype.toString(thisob);
			case JSBuiltin.Array_unshift:
				return ArrayPrototype.unshift(thisob, args);
			case JSBuiltin.Boolean_toString:
				return BooleanPrototype.toString(thisob);
			case JSBuiltin.Boolean_valueOf:
				return BooleanPrototype.valueOf(thisob);
			case JSBuiltin.Date_getDate:
				return DatePrototype.getDate(thisob);
			case JSBuiltin.Date_getDay:
				return DatePrototype.getDay(thisob);
			case JSBuiltin.Date_getFullYear:
				return DatePrototype.getFullYear(thisob);
			case JSBuiltin.Date_getHours:
				return DatePrototype.getHours(thisob);
			case JSBuiltin.Date_getMilliseconds:
				return DatePrototype.getMilliseconds(thisob);
			case JSBuiltin.Date_getMinutes:
				return DatePrototype.getMinutes(thisob);
			case JSBuiltin.Date_getMonth:
				return DatePrototype.getMonth(thisob);
			case JSBuiltin.Date_getSeconds:
				return DatePrototype.getSeconds(thisob);
			case JSBuiltin.Date_getTime:
				return DatePrototype.getTime(thisob);
			case JSBuiltin.Date_getTimezoneOffset:
				return DatePrototype.getTimezoneOffset(thisob);
			case JSBuiltin.Date_getUTCDate:
				return DatePrototype.getUTCDate(thisob);
			case JSBuiltin.Date_getUTCDay:
				return DatePrototype.getUTCDay(thisob);
			case JSBuiltin.Date_getUTCFullYear:
				return DatePrototype.getUTCFullYear(thisob);
			case JSBuiltin.Date_getUTCHours:
				return DatePrototype.getUTCHours(thisob);
			case JSBuiltin.Date_getUTCMilliseconds:
				return DatePrototype.getUTCMilliseconds(thisob);
			case JSBuiltin.Date_getUTCMinutes:
				return DatePrototype.getUTCMinutes(thisob);
			case JSBuiltin.Date_getUTCMonth:
				return DatePrototype.getUTCMonth(thisob);
			case JSBuiltin.Date_getUTCSeconds:
				return DatePrototype.getUTCSeconds(thisob);
			case JSBuiltin.Date_getVarDate:
				return DatePrototype.getVarDate(thisob);
			case JSBuiltin.Date_getYear:
				return DatePrototype.getYear(thisob);
			case JSBuiltin.Date_parse:
				return DateConstructor.parse(Convert.ToString(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Date_setDate:
				return DatePrototype.setDate(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Date_setFullYear:
				return DatePrototype.setFullYear(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num), BuiltinFunction.GetArg(args, 2, num));
			case JSBuiltin.Date_setHours:
				return DatePrototype.setHours(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num), BuiltinFunction.GetArg(args, 2, num), BuiltinFunction.GetArg(args, 3, num));
			case JSBuiltin.Date_setMinutes:
				return DatePrototype.setMinutes(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num), BuiltinFunction.GetArg(args, 2, num));
			case JSBuiltin.Date_setMilliseconds:
				return DatePrototype.setMilliseconds(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Date_setMonth:
				return DatePrototype.setMonth(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.Date_setSeconds:
				return DatePrototype.setSeconds(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.Date_setTime:
				return DatePrototype.setTime(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Date_setUTCDate:
				return DatePrototype.setUTCDate(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Date_setUTCFullYear:
				return DatePrototype.setUTCFullYear(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num), BuiltinFunction.GetArg(args, 2, num));
			case JSBuiltin.Date_setUTCHours:
				return DatePrototype.setUTCHours(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num), BuiltinFunction.GetArg(args, 2, num), BuiltinFunction.GetArg(args, 3, num));
			case JSBuiltin.Date_setUTCMinutes:
				return DatePrototype.setUTCMinutes(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num), BuiltinFunction.GetArg(args, 2, num));
			case JSBuiltin.Date_setUTCMilliseconds:
				return DatePrototype.setUTCMilliseconds(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Date_setUTCMonth:
				return DatePrototype.setUTCMonth(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.Date_setUTCSeconds:
				return DatePrototype.setUTCSeconds(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.Date_setYear:
				return DatePrototype.setYear(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Date_toDateString:
				return DatePrototype.toDateString(thisob);
			case JSBuiltin.Date_toGMTString:
				return DatePrototype.toGMTString(thisob);
			case JSBuiltin.Date_toLocaleDateString:
				return DatePrototype.toLocaleDateString(thisob);
			case JSBuiltin.Date_toLocaleString:
				return DatePrototype.toLocaleString(thisob);
			case JSBuiltin.Date_toLocaleTimeString:
				return DatePrototype.toLocaleTimeString(thisob);
			case JSBuiltin.Date_toString:
				return DatePrototype.toString(thisob);
			case JSBuiltin.Date_toTimeString:
				return DatePrototype.toTimeString(thisob);
			case JSBuiltin.Date_toUTCString:
				return DatePrototype.toUTCString(thisob);
			case JSBuiltin.Date_UTC:
				return DateConstructor.UTC(BuiltinFunction.GetArg(args, 0, num), BuiltinFunction.GetArg(args, 1, num), BuiltinFunction.GetArg(args, 2, num), BuiltinFunction.GetArg(args, 3, num), BuiltinFunction.GetArg(args, 4, num), BuiltinFunction.GetArg(args, 5, num), BuiltinFunction.GetArg(args, 6, num));
			case JSBuiltin.Date_valueOf:
				return DatePrototype.valueOf(thisob);
			case JSBuiltin.Enumerator_atEnd:
				return EnumeratorPrototype.atEnd(thisob);
			case JSBuiltin.Enumerator_item:
				return EnumeratorPrototype.item(thisob);
			case JSBuiltin.Enumerator_moveFirst:
				EnumeratorPrototype.moveFirst(thisob);
				return null;
			case JSBuiltin.Enumerator_moveNext:
				EnumeratorPrototype.moveNext(thisob);
				return null;
			case JSBuiltin.Error_toString:
				return ErrorPrototype.toString(thisob);
			case JSBuiltin.Function_apply:
				return FunctionPrototype.apply(thisob, BuiltinFunction.GetArg(args, 0, num), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.Function_call:
				return FunctionPrototype.call(thisob, BuiltinFunction.GetArg(args, 0, num), BuiltinFunction.VarArgs(args, 1, num));
			case JSBuiltin.Function_toString:
				return FunctionPrototype.toString(thisob);
			case JSBuiltin.Global_CollectGarbage:
				GlobalObject.CollectGarbage();
				return null;
			case JSBuiltin.Global_decodeURI:
				return GlobalObject.decodeURI(BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Global_decodeURIComponent:
				return GlobalObject.decodeURIComponent(BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Global_encodeURI:
				return GlobalObject.encodeURI(BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Global_encodeURIComponent:
				return GlobalObject.encodeURIComponent(BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Global_escape:
				return GlobalObject.escape(BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Global_eval:
				return GlobalObject.eval(BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Global_GetObject:
				return GlobalObject.GetObject(BuiltinFunction.GetArg(args, 0, num), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.Global_isNaN:
				return GlobalObject.isNaN(BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Global_isFinite:
				return GlobalObject.isFinite(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Global_parseFloat:
				return GlobalObject.parseFloat(BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Global_parseInt:
				return GlobalObject.parseInt(BuiltinFunction.GetArg(args, 0, num), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.Global_ScriptEngine:
				return GlobalObject.ScriptEngine();
			case JSBuiltin.Global_ScriptEngineBuildVersion:
				return GlobalObject.ScriptEngineBuildVersion();
			case JSBuiltin.Global_ScriptEngineMajorVersion:
				return GlobalObject.ScriptEngineMajorVersion();
			case JSBuiltin.Global_ScriptEngineMinorVersion:
				return GlobalObject.ScriptEngineMinorVersion();
			case JSBuiltin.Global_unescape:
				return GlobalObject.unescape(BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Math_abs:
				return MathObject.abs(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Math_acos:
				return MathObject.acos(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Math_asin:
				return MathObject.asin(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Math_atan:
				return MathObject.atan(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Math_atan2:
				return MathObject.atan2(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), Convert.ToNumber(BuiltinFunction.GetArg(args, 1, num)));
			case JSBuiltin.Math_ceil:
				return MathObject.ceil(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Math_cos:
				return MathObject.cos(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Math_exp:
				return MathObject.exp(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Math_floor:
				return MathObject.floor(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Math_log:
				return MathObject.log(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Math_max:
				return MathObject.max(BuiltinFunction.GetArg(args, 0, num), BuiltinFunction.GetArg(args, 1, num), BuiltinFunction.VarArgs(args, 2, num));
			case JSBuiltin.Math_min:
				return MathObject.min(BuiltinFunction.GetArg(args, 0, num), BuiltinFunction.GetArg(args, 1, num), BuiltinFunction.VarArgs(args, 2, num));
			case JSBuiltin.Math_pow:
				return MathObject.pow(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), Convert.ToNumber(BuiltinFunction.GetArg(args, 1, num)));
			case JSBuiltin.Math_random:
				return MathObject.random();
			case JSBuiltin.Math_round:
				return MathObject.round(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Math_sin:
				return MathObject.sin(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Math_sqrt:
				return MathObject.sqrt(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Math_tan:
				return MathObject.tan(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Number_toExponential:
				return NumberPrototype.toExponential(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Number_toFixed:
				return NumberPrototype.toFixed(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.Number_toLocaleString:
				return NumberPrototype.toLocaleString(thisob);
			case JSBuiltin.Number_toPrecision:
				return NumberPrototype.toPrecision(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Number_toString:
				return NumberPrototype.toString(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Number_valueOf:
				return NumberPrototype.valueOf(thisob);
			case JSBuiltin.Object_hasOwnProperty:
				return ObjectPrototype.hasOwnProperty(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Object_isPrototypeOf:
				return ObjectPrototype.isPrototypeOf(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Object_propertyIsEnumerable:
				return ObjectPrototype.propertyIsEnumerable(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.Object_toLocaleString:
				return ObjectPrototype.toLocaleString(thisob);
			case JSBuiltin.Object_toString:
				return ObjectPrototype.toString(thisob);
			case JSBuiltin.Object_valueOf:
				return ObjectPrototype.valueOf(thisob);
			case JSBuiltin.RegExp_compile:
				return RegExpPrototype.compile(thisob, BuiltinFunction.GetArg(args, 0, num), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.RegExp_exec:
				return RegExpPrototype.exec(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.RegExp_test:
				return RegExpPrototype.test(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.RegExp_toString:
				return RegExpPrototype.toString(thisob);
			case JSBuiltin.String_anchor:
				return StringPrototype.anchor(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.String_big:
				return StringPrototype.big(thisob);
			case JSBuiltin.String_blink:
				return StringPrototype.blink(thisob);
			case JSBuiltin.String_bold:
				return StringPrototype.bold(thisob);
			case JSBuiltin.String_charAt:
				return StringPrototype.charAt(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.String_charCodeAt:
				return StringPrototype.charCodeAt(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)));
			case JSBuiltin.String_concat:
				return StringPrototype.concat(thisob, args);
			case JSBuiltin.String_fixed:
				return StringPrototype.@fixed(thisob);
			case JSBuiltin.String_fontcolor:
				return StringPrototype.fontcolor(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.String_fontsize:
				return StringPrototype.fontsize(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.String_fromCharCode:
				return StringConstructor.fromCharCode(args);
			case JSBuiltin.String_indexOf:
				return StringPrototype.indexOf(thisob, BuiltinFunction.GetArg(args, 0, num), Convert.ToNumber(BuiltinFunction.GetArg(args, 1, num)));
			case JSBuiltin.String_italics:
				return StringPrototype.italics(thisob);
			case JSBuiltin.String_lastIndexOf:
				return StringPrototype.lastIndexOf(thisob, BuiltinFunction.GetArg(args, 0, num), Convert.ToNumber(BuiltinFunction.GetArg(args, 1, num)));
			case JSBuiltin.String_link:
				return StringPrototype.link(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.String_localeCompare:
				return StringPrototype.localeCompare(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.String_match:
				return StringPrototype.match(thisob, engine, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.String_replace:
				return StringPrototype.replace(thisob, BuiltinFunction.GetArg(args, 0, num), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.String_search:
				return StringPrototype.search(thisob, engine, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.String_slice:
				return StringPrototype.slice(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.String_small:
				return StringPrototype.small(thisob);
			case JSBuiltin.String_split:
				return StringPrototype.split(thisob, engine, BuiltinFunction.GetArg(args, 0, num), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.String_strike:
				return StringPrototype.strike(thisob);
			case JSBuiltin.String_sub:
				return StringPrototype.sub(thisob);
			case JSBuiltin.String_substr:
				return StringPrototype.substr(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.String_substring:
				return StringPrototype.substring(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, num)), BuiltinFunction.GetArg(args, 1, num));
			case JSBuiltin.String_sup:
				return StringPrototype.sup(thisob);
			case JSBuiltin.String_toLocaleLowerCase:
				return StringPrototype.toLocaleLowerCase(thisob);
			case JSBuiltin.String_toLocaleUpperCase:
				return StringPrototype.toLocaleUpperCase(thisob);
			case JSBuiltin.String_toLowerCase:
				return StringPrototype.toLowerCase(thisob);
			case JSBuiltin.String_toString:
				return StringPrototype.toString(thisob);
			case JSBuiltin.String_toUpperCase:
				return StringPrototype.toUpperCase(thisob);
			case JSBuiltin.String_valueOf:
				return StringPrototype.valueOf(thisob);
			case JSBuiltin.VBArray_dimensions:
				return VBArrayPrototype.dimensions(thisob);
			case JSBuiltin.VBArray_getItem:
				return VBArrayPrototype.getItem(thisob, args);
			case JSBuiltin.VBArray_lbound:
				return VBArrayPrototype.lbound(thisob, BuiltinFunction.GetArg(args, 0, num));
			case JSBuiltin.VBArray_toArray:
				return VBArrayPrototype.toArray(thisob, engine);
			case JSBuiltin.VBArray_ubound:
				return VBArrayPrototype.ubound(thisob, BuiltinFunction.GetArg(args, 0, num));
			default:
				return method.Invoke(thisob, BindingFlags.Default, JSBinder.ob, args, null);
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000FFE8 File Offset: 0x0000EFE8
		private static object GetArg(object[] args, int i, int n)
		{
			if (i >= n)
			{
				return Missing.Value;
			}
			return args[i];
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000FFF8 File Offset: 0x0000EFF8
		private static object[] VarArgs(object[] args, int offset, int n)
		{
			object[] array = new object[(n >= offset) ? (n - offset) : 0];
			for (int i = offset; i < n; i++)
			{
				array[i - offset] = args[i];
			}
			return array;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0001002A File Offset: 0x0000F02A
		public override string ToString()
		{
			return "function " + this.name + "() {\n    [native code]\n}";
		}

		// Token: 0x0400013F RID: 319
		internal MethodInfo method;

		// Token: 0x04000140 RID: 320
		private JSBuiltin biFunc;
	}
}

using System;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000F1 RID: 241
	public sealed class ObjectConstructor : ScriptFunction
	{
		// Token: 0x06000A94 RID: 2708 RVA: 0x000514BD File Offset: 0x000504BD
		internal ObjectConstructor()
			: base(FunctionPrototype.ob, "Object", 1)
		{
			this.originalPrototype = ObjectPrototype.ob;
			ObjectPrototype._constructor = this;
			this.proto = ObjectPrototype.ob;
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x000514EC File Offset: 0x000504EC
		internal ObjectConstructor(LenientFunctionPrototype parent, LenientObjectPrototype prototypeProp)
			: base(parent, "Object", 1)
		{
			this.originalPrototype = prototypeProp;
			prototypeProp.constructor = this;
			this.proto = prototypeProp;
			this.noExpando = false;
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00051518 File Offset: 0x00050518
		internal override object Call(object[] args, object thisob)
		{
			if (args.Length == 0)
			{
				return this.ConstructObject();
			}
			object obj = args[0];
			if (obj == null || obj == DBNull.Value)
			{
				return this.Construct(args);
			}
			return Convert.ToObject3(obj, this.engine);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00051554 File Offset: 0x00050554
		internal override object Construct(object[] args)
		{
			if (args.Length == 0)
			{
				return this.ConstructObject();
			}
			object obj = args[0];
			switch (Convert.GetTypeCode(obj))
			{
			case TypeCode.Empty:
			case TypeCode.DBNull:
				return this.ConstructObject();
			case TypeCode.Object:
			{
				if (obj is ScriptObject)
				{
					return obj;
				}
				IReflect reflect;
				if (obj is IReflect)
				{
					reflect = (IReflect)obj;
				}
				else
				{
					reflect = obj.GetType();
				}
				return reflect.InvokeMember(string.Empty, BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.OptionalParamBinding, JSBinder.ob, obj, new object[0], null, null, null);
			}
			default:
				return Convert.ToObject3(obj, this.engine);
			}
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x000515E3 File Offset: 0x000505E3
		public JSObject ConstructObject()
		{
			return new JSObject(this.originalPrototype, false);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x000515F1 File Offset: 0x000505F1
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public new object CreateInstance(params object[] args)
		{
			return this.Construct(args);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x000515FA File Offset: 0x000505FA
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public object Invoke(params object[] args)
		{
			return this.Call(args, null);
		}

		// Token: 0x0400067F RID: 1663
		internal static readonly ObjectConstructor ob = new ObjectConstructor();

		// Token: 0x04000680 RID: 1664
		internal ObjectPrototype originalPrototype;
	}
}

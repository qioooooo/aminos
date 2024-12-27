using System;

namespace Microsoft.JScript
{
	// Token: 0x0200000F RID: 15
	public sealed class ArrayConstructor : ScriptFunction
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00004C3A File Offset: 0x00003C3A
		internal ArrayConstructor()
			: base(FunctionPrototype.ob, "Array", 1)
		{
			this.originalPrototype = ArrayPrototype.ob;
			ArrayPrototype._constructor = this;
			this.proto = ArrayPrototype.ob;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004C69 File Offset: 0x00003C69
		internal ArrayConstructor(LenientFunctionPrototype parent, LenientArrayPrototype prototypeProp)
			: base(parent, "Array", 1)
		{
			this.originalPrototype = prototypeProp;
			prototypeProp.constructor = this;
			this.proto = prototypeProp;
			this.noExpando = false;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004C94 File Offset: 0x00003C94
		internal override object Call(object[] args, object thisob)
		{
			return this.Construct(args);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004C9D File Offset: 0x00003C9D
		internal ArrayObject Construct()
		{
			return new ArrayObject(this.originalPrototype, typeof(ArrayObject));
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004CB4 File Offset: 0x00003CB4
		internal override object Construct(object[] args)
		{
			return this.CreateInstance(args);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004CC0 File Offset: 0x00003CC0
		public ArrayObject ConstructArray(object[] args)
		{
			ArrayObject arrayObject = new ArrayObject(this.originalPrototype, typeof(ArrayObject));
			arrayObject.length = args.Length;
			for (int i = 0; i < args.Length; i++)
			{
				arrayObject.SetValueAtIndex((uint)i, args[i]);
			}
			return arrayObject;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004D0A File Offset: 0x00003D0A
		internal ArrayObject ConstructWrapper()
		{
			return new ArrayWrapper(this.originalPrototype, null, false);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004D19 File Offset: 0x00003D19
		internal ArrayObject ConstructWrapper(Array arr)
		{
			return new ArrayWrapper(this.originalPrototype, arr, false);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004D28 File Offset: 0x00003D28
		internal ArrayObject ConstructImplicitWrapper(Array arr)
		{
			return new ArrayWrapper(this.originalPrototype, arr, true);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004D38 File Offset: 0x00003D38
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public new ArrayObject CreateInstance(params object[] args)
		{
			ArrayObject arrayObject = new ArrayObject(this.originalPrototype, typeof(ArrayObject));
			if (args.Length != 0)
			{
				if (args.Length == 1)
				{
					object obj = args[0];
					IConvertible iconvertible = Convert.GetIConvertible(obj);
					switch (Convert.GetTypeCode(obj, iconvertible))
					{
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
					{
						double num = Convert.ToNumber(obj, iconvertible);
						uint num2 = Convert.ToUint32(obj, iconvertible);
						if (num != num2)
						{
							throw new JScriptException(JSError.ArrayLengthConstructIncorrect);
						}
						arrayObject.length = num2;
						return arrayObject;
					}
					}
				}
				if (args.Length == 1 && args[0] is Array)
				{
					Array array = (Array)args[0];
					if (array.Rank != 1)
					{
						throw new JScriptException(JSError.TypeMismatch);
					}
					arrayObject.length = array.Length;
					for (int i = 0; i < array.Length; i++)
					{
						arrayObject.SetValueAtIndex((uint)i, array.GetValue(i));
					}
				}
				else
				{
					arrayObject.length = args.Length;
					for (int j = 0; j < args.Length; j++)
					{
						arrayObject.SetValueAtIndex((uint)j, args[j]);
					}
				}
			}
			return arrayObject;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004E81 File Offset: 0x00003E81
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public ArrayObject Invoke(params object[] args)
		{
			if (args.Length == 1 && args[0] is Array)
			{
				return this.ConstructWrapper((Array)args[0]);
			}
			return this.CreateInstance(args);
		}

		// Token: 0x0400002A RID: 42
		internal static readonly ArrayConstructor ob = new ArrayConstructor();

		// Token: 0x0400002B RID: 43
		private ArrayPrototype originalPrototype;
	}
}

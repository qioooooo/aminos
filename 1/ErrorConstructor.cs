using System;

namespace Microsoft.JScript
{
	// Token: 0x02000077 RID: 119
	public sealed class ErrorConstructor : ScriptFunction
	{
		// Token: 0x06000589 RID: 1417 RVA: 0x00026D18 File Offset: 0x00025D18
		internal ErrorConstructor()
			: base(ErrorPrototype.ob, "Error", 2)
		{
			this.originalPrototype = ErrorPrototype.ob;
			ErrorPrototype.ob._constructor = this;
			this.proto = ErrorPrototype.ob;
			this.type = ErrorType.OtherError;
			this.globalObject = GlobalObject.commonInstance;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00026D69 File Offset: 0x00025D69
		internal ErrorConstructor(LenientFunctionPrototype parent, LenientErrorPrototype prototypeProp, GlobalObject globalObject)
			: base(parent, "Error", 2)
		{
			this.originalPrototype = prototypeProp;
			prototypeProp.constructor = this;
			this.proto = prototypeProp;
			this.type = ErrorType.OtherError;
			this.globalObject = globalObject;
			this.noExpando = false;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00026DA4 File Offset: 0x00025DA4
		internal ErrorConstructor(string subtypeName, ErrorType type)
			: base(ErrorConstructor.ob.parent, subtypeName, 2)
		{
			this.originalPrototype = new ErrorPrototype(ErrorConstructor.ob.originalPrototype, subtypeName);
			this.originalPrototype._constructor = this;
			this.proto = this.originalPrototype;
			this.type = type;
			this.globalObject = GlobalObject.commonInstance;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00026E04 File Offset: 0x00025E04
		internal ErrorConstructor(string subtypeName, ErrorType type, ErrorConstructor error, GlobalObject globalObject)
			: base(error.parent, subtypeName, 2)
		{
			this.originalPrototype = new LenientErrorPrototype((LenientFunctionPrototype)error.parent, error.originalPrototype, subtypeName);
			this.noExpando = false;
			this.originalPrototype._constructor = this;
			this.proto = this.originalPrototype;
			this.type = type;
			this.globalObject = globalObject;
			this.noExpando = false;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00026E71 File Offset: 0x00025E71
		internal override object Call(object[] args, object thisob)
		{
			return this.Construct(args);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00026E7A File Offset: 0x00025E7A
		internal override object Construct(object[] args)
		{
			return this.CreateInstance(args);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00026E84 File Offset: 0x00025E84
		internal ErrorObject Construct(object e)
		{
			if (!(e is JScriptException) || this != this.globalObject.originalError)
			{
				switch (this.type)
				{
				case ErrorType.EvalError:
					return new EvalErrorObject(this.originalPrototype, e);
				case ErrorType.RangeError:
					return new RangeErrorObject(this.originalPrototype, e);
				case ErrorType.ReferenceError:
					return new ReferenceErrorObject(this.originalPrototype, e);
				case ErrorType.SyntaxError:
					return new SyntaxErrorObject(this.originalPrototype, e);
				case ErrorType.TypeError:
					return new TypeErrorObject(this.originalPrototype, e);
				case ErrorType.URIError:
					return new URIErrorObject(this.originalPrototype, e);
				default:
					return new ErrorObject(this.originalPrototype, e);
				}
			}
			else
			{
				switch (((JScriptException)e).GetErrorType())
				{
				case ErrorType.EvalError:
					return this.globalObject.originalEvalError.Construct(e);
				case ErrorType.RangeError:
					return this.globalObject.originalRangeError.Construct(e);
				case ErrorType.ReferenceError:
					return this.globalObject.originalReferenceError.Construct(e);
				case ErrorType.SyntaxError:
					return this.globalObject.originalSyntaxError.Construct(e);
				case ErrorType.TypeError:
					return this.globalObject.originalTypeError.Construct(e);
				case ErrorType.URIError:
					return this.globalObject.originalURIError.Construct(e);
				default:
					return new ErrorObject(this.originalPrototype, e);
				}
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00026FD4 File Offset: 0x00025FD4
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public new ErrorObject CreateInstance(params object[] args)
		{
			switch (this.type)
			{
			case ErrorType.EvalError:
				return new EvalErrorObject(this.originalPrototype, args);
			case ErrorType.RangeError:
				return new RangeErrorObject(this.originalPrototype, args);
			case ErrorType.ReferenceError:
				return new ReferenceErrorObject(this.originalPrototype, args);
			case ErrorType.SyntaxError:
				return new SyntaxErrorObject(this.originalPrototype, args);
			case ErrorType.TypeError:
				return new TypeErrorObject(this.originalPrototype, args);
			case ErrorType.URIError:
				return new URIErrorObject(this.originalPrototype, args);
			default:
				return new ErrorObject(this.originalPrototype, args);
			}
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00027064 File Offset: 0x00026064
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public object Invoke(params object[] args)
		{
			return this.CreateInstance(args);
		}

		// Token: 0x0400025D RID: 605
		internal static readonly ErrorConstructor ob = new ErrorConstructor();

		// Token: 0x0400025E RID: 606
		internal static readonly ErrorConstructor evalOb = new ErrorConstructor("EvalError", ErrorType.EvalError);

		// Token: 0x0400025F RID: 607
		internal static readonly ErrorConstructor rangeOb = new ErrorConstructor("RangeError", ErrorType.RangeError);

		// Token: 0x04000260 RID: 608
		internal static readonly ErrorConstructor referenceOb = new ErrorConstructor("ReferenceError", ErrorType.ReferenceError);

		// Token: 0x04000261 RID: 609
		internal static readonly ErrorConstructor syntaxOb = new ErrorConstructor("SyntaxError", ErrorType.SyntaxError);

		// Token: 0x04000262 RID: 610
		internal static readonly ErrorConstructor typeOb = new ErrorConstructor("TypeError", ErrorType.TypeError);

		// Token: 0x04000263 RID: 611
		internal static readonly ErrorConstructor uriOb = new ErrorConstructor("URIError", ErrorType.URIError);

		// Token: 0x04000264 RID: 612
		private ErrorPrototype originalPrototype;

		// Token: 0x04000265 RID: 613
		private ErrorType type;

		// Token: 0x04000266 RID: 614
		private GlobalObject globalObject;
	}
}

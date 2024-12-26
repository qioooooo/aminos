using System;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000D1 RID: 209
	public sealed class LenientGlobalObject : GlobalObject
	{
		// Token: 0x0600094A RID: 2378 RVA: 0x00048F50 File Offset: 0x00047F50
		internal LenientGlobalObject(VsaEngine engine)
		{
			this.engine = engine;
			this.Infinity = double.PositiveInfinity;
			this.NaN = double.NaN;
			this.undefined = null;
			this.ActiveXObjectField = Missing.Value;
			this.ArrayField = Missing.Value;
			this.BooleanField = Missing.Value;
			this.DateField = Missing.Value;
			this.EnumeratorField = Missing.Value;
			this.ErrorField = Missing.Value;
			this.EvalErrorField = Missing.Value;
			this.FunctionField = Missing.Value;
			this.MathField = Missing.Value;
			this.NumberField = Missing.Value;
			this.ObjectField = Missing.Value;
			this.RangeErrorField = Missing.Value;
			this.ReferenceErrorField = Missing.Value;
			this.RegExpField = Missing.Value;
			this.StringField = Missing.Value;
			this.SyntaxErrorField = Missing.Value;
			this.TypeErrorField = Missing.Value;
			this.VBArrayField = Missing.Value;
			this.URIErrorField = Missing.Value;
			Type typeFromHandle = typeof(GlobalObject);
			LenientFunctionPrototype functionPrototype = this.functionPrototype;
			this.decodeURI = new BuiltinFunction("decodeURI", this, typeFromHandle.GetMethod("decodeURI"), functionPrototype);
			this.decodeURIComponent = new BuiltinFunction("decodeURIComponent", this, typeFromHandle.GetMethod("decodeURIComponent"), functionPrototype);
			this.encodeURI = new BuiltinFunction("encodeURI", this, typeFromHandle.GetMethod("encodeURI"), functionPrototype);
			this.encodeURIComponent = new BuiltinFunction("encodeURIComponent", this, typeFromHandle.GetMethod("encodeURIComponent"), functionPrototype);
			this.escape = new BuiltinFunction("escape", this, typeFromHandle.GetMethod("escape"), functionPrototype);
			this.eval = new BuiltinFunction("eval", this, typeFromHandle.GetMethod("eval"), functionPrototype);
			this.isNaN = new BuiltinFunction("isNaN", this, typeFromHandle.GetMethod("isNaN"), functionPrototype);
			this.isFinite = new BuiltinFunction("isFinite", this, typeFromHandle.GetMethod("isFinite"), functionPrototype);
			this.parseInt = new BuiltinFunction("parseInt", this, typeFromHandle.GetMethod("parseInt"), functionPrototype);
			this.GetObject = new BuiltinFunction("GetObject", this, typeFromHandle.GetMethod("GetObject"), functionPrototype);
			this.parseFloat = new BuiltinFunction("parseFloat", this, typeFromHandle.GetMethod("parseFloat"), functionPrototype);
			this.ScriptEngine = new BuiltinFunction("ScriptEngine", this, typeFromHandle.GetMethod("ScriptEngine"), functionPrototype);
			this.ScriptEngineBuildVersion = new BuiltinFunction("ScriptEngineBuildVersion", this, typeFromHandle.GetMethod("ScriptEngineBuildVersion"), functionPrototype);
			this.ScriptEngineMajorVersion = new BuiltinFunction("ScriptEngineMajorVersion", this, typeFromHandle.GetMethod("ScriptEngineMajorVersion"), functionPrototype);
			this.ScriptEngineMinorVersion = new BuiltinFunction("ScriptEngineMinorVersion", this, typeFromHandle.GetMethod("ScriptEngineMinorVersion"), functionPrototype);
			this.unescape = new BuiltinFunction("unescape", this, typeFromHandle.GetMethod("unescape"), functionPrototype);
			this.boolean = Typeob.Boolean;
			this.@byte = Typeob.Byte;
			this.@char = Typeob.Char;
			this.@decimal = Typeob.Decimal;
			this.@double = Typeob.Double;
			this.@float = Typeob.Single;
			this.@int = Typeob.Int32;
			this.@long = Typeob.Int64;
			this.@sbyte = Typeob.SByte;
			this.@short = Typeob.Int16;
			this.@void = Typeob.Void;
			this.@uint = Typeob.UInt32;
			this.@ulong = Typeob.UInt64;
			this.@ushort = Typeob.UInt16;
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x000492E6 File Offset: 0x000482E6
		private LenientArrayPrototype arrayPrototype
		{
			get
			{
				if (this.arrayPrototypeField == null)
				{
					this.arrayPrototypeField = new LenientArrayPrototype(this.functionPrototype, this.objectPrototype);
				}
				return this.arrayPrototypeField;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0004930D File Offset: 0x0004830D
		private LenientFunctionPrototype functionPrototype
		{
			get
			{
				if (this.functionPrototypeField == null)
				{
					LenientObjectPrototype objectPrototype = this.objectPrototype;
				}
				return this.functionPrototypeField;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00049324 File Offset: 0x00048324
		private LenientObjectPrototype objectPrototype
		{
			get
			{
				if (this.objectPrototypeField == null)
				{
					LenientObjectPrototype lenientObjectPrototype = (this.objectPrototypeField = new LenientObjectPrototype(this.engine));
					LenientFunctionPrototype lenientFunctionPrototype = (this.functionPrototypeField = new LenientFunctionPrototype(lenientObjectPrototype));
					lenientObjectPrototype.Initialize(lenientFunctionPrototype);
					JSObject jsobject = new JSObject(lenientObjectPrototype, false);
					jsobject.AddField("constructor").SetValue(jsobject, lenientFunctionPrototype);
					lenientFunctionPrototype.proto = jsobject;
				}
				return this.objectPrototypeField;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x0004938E File Offset: 0x0004838E
		internal override ActiveXObjectConstructor originalActiveXObject
		{
			get
			{
				if (this.originalActiveXObjectField == null)
				{
					this.originalActiveXObjectField = new ActiveXObjectConstructor(this.functionPrototype);
				}
				return this.originalActiveXObjectField;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x000493AF File Offset: 0x000483AF
		internal override ArrayConstructor originalArray
		{
			get
			{
				if (this.originalArrayField == null)
				{
					this.originalArrayField = new ArrayConstructor(this.functionPrototype, this.arrayPrototype);
				}
				return this.originalArrayField;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x000493D6 File Offset: 0x000483D6
		internal override BooleanConstructor originalBoolean
		{
			get
			{
				if (this.originalBooleanField == null)
				{
					this.originalBooleanField = new BooleanConstructor(this.functionPrototype, new LenientBooleanPrototype(this.functionPrototype, this.objectPrototype));
				}
				return this.originalBooleanField;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00049408 File Offset: 0x00048408
		internal override DateConstructor originalDate
		{
			get
			{
				if (this.originalDateField == null)
				{
					this.originalDateField = new LenientDateConstructor(this.functionPrototype, new LenientDatePrototype(this.functionPrototype, this.objectPrototype));
				}
				return this.originalDateField;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x0004943A File Offset: 0x0004843A
		internal override ErrorConstructor originalError
		{
			get
			{
				if (this.originalErrorField == null)
				{
					this.originalErrorField = new ErrorConstructor(this.functionPrototype, new LenientErrorPrototype(this.functionPrototype, this.objectPrototype, "Error"), this);
				}
				return this.originalErrorField;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x00049472 File Offset: 0x00048472
		internal override EnumeratorConstructor originalEnumerator
		{
			get
			{
				if (this.originalEnumeratorField == null)
				{
					this.originalEnumeratorField = new EnumeratorConstructor(this.functionPrototype, new LenientEnumeratorPrototype(this.functionPrototype, this.objectPrototype));
				}
				return this.originalEnumeratorField;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x000494A4 File Offset: 0x000484A4
		internal override ErrorConstructor originalEvalError
		{
			get
			{
				if (this.originalEvalErrorField == null)
				{
					this.originalEvalErrorField = new ErrorConstructor("EvalError", ErrorType.EvalError, this.originalError, this);
				}
				return this.originalEvalErrorField;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x000494CC File Offset: 0x000484CC
		internal override FunctionConstructor originalFunction
		{
			get
			{
				if (this.originalFunctionField == null)
				{
					this.originalFunctionField = new FunctionConstructor(this.functionPrototype);
				}
				return this.originalFunctionField;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x000494ED File Offset: 0x000484ED
		internal override NumberConstructor originalNumber
		{
			get
			{
				if (this.originalNumberField == null)
				{
					this.originalNumberField = new NumberConstructor(this.functionPrototype, new LenientNumberPrototype(this.functionPrototype, this.objectPrototype));
				}
				return this.originalNumberField;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x0004951F File Offset: 0x0004851F
		internal override ObjectConstructor originalObject
		{
			get
			{
				if (this.originalObjectField == null)
				{
					this.originalObjectField = new ObjectConstructor(this.functionPrototype, this.objectPrototype);
				}
				return this.originalObjectField;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x00049546 File Offset: 0x00048546
		internal override ObjectPrototype originalObjectPrototype
		{
			get
			{
				if (this.originalObjectPrototypeField == null)
				{
					this.originalObjectPrototypeField = ObjectPrototype.ob;
				}
				return this.originalObjectPrototypeField;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x00049561 File Offset: 0x00048561
		internal override ErrorConstructor originalRangeError
		{
			get
			{
				if (this.originalRangeErrorField == null)
				{
					this.originalRangeErrorField = new ErrorConstructor("RangeError", ErrorType.RangeError, this.originalError, this);
				}
				return this.originalRangeErrorField;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x00049589 File Offset: 0x00048589
		internal override ErrorConstructor originalReferenceError
		{
			get
			{
				if (this.originalReferenceErrorField == null)
				{
					this.originalReferenceErrorField = new ErrorConstructor("ReferenceError", ErrorType.ReferenceError, this.originalError, this);
				}
				return this.originalReferenceErrorField;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x000495B1 File Offset: 0x000485B1
		internal override RegExpConstructor originalRegExp
		{
			get
			{
				if (this.originalRegExpField == null)
				{
					this.originalRegExpField = new RegExpConstructor(this.functionPrototype, new LenientRegExpPrototype(this.functionPrototype, this.objectPrototype), this.arrayPrototype);
				}
				return this.originalRegExpField;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x000495E9 File Offset: 0x000485E9
		internal override StringConstructor originalString
		{
			get
			{
				if (this.originalStringField == null)
				{
					this.originalStringField = new LenientStringConstructor(this.functionPrototype, new LenientStringPrototype(this.functionPrototype, this.objectPrototype));
				}
				return this.originalStringField;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x0004961B File Offset: 0x0004861B
		internal override ErrorConstructor originalSyntaxError
		{
			get
			{
				if (this.originalSyntaxErrorField == null)
				{
					this.originalSyntaxErrorField = new ErrorConstructor("SyntaxError", ErrorType.SyntaxError, this.originalError, this);
				}
				return this.originalSyntaxErrorField;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x00049643 File Offset: 0x00048643
		internal override ErrorConstructor originalTypeError
		{
			get
			{
				if (this.originalTypeErrorField == null)
				{
					this.originalTypeErrorField = new ErrorConstructor("TypeError", ErrorType.TypeError, this.originalError, this);
				}
				return this.originalTypeErrorField;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x0004966B File Offset: 0x0004866B
		internal override ErrorConstructor originalURIError
		{
			get
			{
				if (this.originalURIErrorField == null)
				{
					this.originalURIErrorField = new ErrorConstructor("URIError", ErrorType.URIError, this.originalError, this);
				}
				return this.originalURIErrorField;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x00049693 File Offset: 0x00048693
		internal override VBArrayConstructor originalVBArray
		{
			get
			{
				if (this.originalVBArrayField == null)
				{
					this.originalVBArrayField = new VBArrayConstructor(this.functionPrototype, new LenientVBArrayPrototype(this.functionPrototype, this.objectPrototype));
				}
				return this.originalVBArrayField;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x000496C5 File Offset: 0x000486C5
		// (set) Token: 0x06000962 RID: 2402 RVA: 0x000496E6 File Offset: 0x000486E6
		public new object ActiveXObject
		{
			get
			{
				if (this.ActiveXObjectField is Missing)
				{
					this.ActiveXObjectField = this.originalActiveXObject;
				}
				return this.ActiveXObjectField;
			}
			set
			{
				this.ActiveXObjectField = value;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x000496EF File Offset: 0x000486EF
		// (set) Token: 0x06000964 RID: 2404 RVA: 0x00049710 File Offset: 0x00048710
		public new object Array
		{
			get
			{
				if (this.ArrayField is Missing)
				{
					this.ArrayField = this.originalArray;
				}
				return this.ArrayField;
			}
			set
			{
				this.ArrayField = value;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x00049719 File Offset: 0x00048719
		// (set) Token: 0x06000966 RID: 2406 RVA: 0x0004973A File Offset: 0x0004873A
		public new object Boolean
		{
			get
			{
				if (this.BooleanField is Missing)
				{
					this.BooleanField = this.originalBoolean;
				}
				return this.BooleanField;
			}
			set
			{
				this.BooleanField = value;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x00049743 File Offset: 0x00048743
		// (set) Token: 0x06000968 RID: 2408 RVA: 0x00049764 File Offset: 0x00048764
		public new object Date
		{
			get
			{
				if (this.DateField is Missing)
				{
					this.DateField = this.originalDate;
				}
				return this.DateField;
			}
			set
			{
				this.DateField = value;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0004976D File Offset: 0x0004876D
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x0004978E File Offset: 0x0004878E
		public new object Enumerator
		{
			get
			{
				if (this.EnumeratorField is Missing)
				{
					this.EnumeratorField = this.originalEnumerator;
				}
				return this.EnumeratorField;
			}
			set
			{
				this.EnumeratorField = value;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x00049797 File Offset: 0x00048797
		// (set) Token: 0x0600096C RID: 2412 RVA: 0x000497B8 File Offset: 0x000487B8
		public new object Error
		{
			get
			{
				if (this.ErrorField is Missing)
				{
					this.ErrorField = this.originalError;
				}
				return this.ErrorField;
			}
			set
			{
				this.ErrorField = value;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x000497C1 File Offset: 0x000487C1
		// (set) Token: 0x0600096E RID: 2414 RVA: 0x000497E2 File Offset: 0x000487E2
		public new object EvalError
		{
			get
			{
				if (this.EvalErrorField is Missing)
				{
					this.EvalErrorField = this.originalEvalError;
				}
				return this.EvalErrorField;
			}
			set
			{
				this.EvalErrorField = value;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x000497EB File Offset: 0x000487EB
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x0004980C File Offset: 0x0004880C
		public new object Function
		{
			get
			{
				if (this.FunctionField is Missing)
				{
					this.FunctionField = this.originalFunction;
				}
				return this.FunctionField;
			}
			set
			{
				this.FunctionField = value;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x00049815 File Offset: 0x00048815
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x00049841 File Offset: 0x00048841
		public new object Math
		{
			get
			{
				if (this.MathField is Missing)
				{
					this.MathField = new LenientMathObject(this.objectPrototype, this.functionPrototype);
				}
				return this.MathField;
			}
			set
			{
				this.MathField = value;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x0004984A File Offset: 0x0004884A
		// (set) Token: 0x06000974 RID: 2420 RVA: 0x0004986B File Offset: 0x0004886B
		public new object Number
		{
			get
			{
				if (this.NumberField is Missing)
				{
					this.NumberField = this.originalNumber;
				}
				return this.NumberField;
			}
			set
			{
				this.NumberField = value;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x00049874 File Offset: 0x00048874
		// (set) Token: 0x06000976 RID: 2422 RVA: 0x00049895 File Offset: 0x00048895
		public new object Object
		{
			get
			{
				if (this.ObjectField is Missing)
				{
					this.ObjectField = this.originalObject;
				}
				return this.ObjectField;
			}
			set
			{
				this.ObjectField = value;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x0004989E File Offset: 0x0004889E
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x000498BF File Offset: 0x000488BF
		public new object RangeError
		{
			get
			{
				if (this.RangeErrorField is Missing)
				{
					this.RangeErrorField = this.originalRangeError;
				}
				return this.RangeErrorField;
			}
			set
			{
				this.RangeErrorField = value;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x000498C8 File Offset: 0x000488C8
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x000498E9 File Offset: 0x000488E9
		public new object ReferenceError
		{
			get
			{
				if (this.ReferenceErrorField is Missing)
				{
					this.ReferenceErrorField = this.originalReferenceError;
				}
				return this.ReferenceErrorField;
			}
			set
			{
				this.ReferenceErrorField = value;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x000498F2 File Offset: 0x000488F2
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x00049913 File Offset: 0x00048913
		public new object RegExp
		{
			get
			{
				if (this.RegExpField is Missing)
				{
					this.RegExpField = this.originalRegExp;
				}
				return this.RegExpField;
			}
			set
			{
				this.RegExpField = value;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x0004991C File Offset: 0x0004891C
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x0004993D File Offset: 0x0004893D
		public new object String
		{
			get
			{
				if (this.StringField is Missing)
				{
					this.StringField = this.originalString;
				}
				return this.StringField;
			}
			set
			{
				this.StringField = value;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x00049946 File Offset: 0x00048946
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x00049967 File Offset: 0x00048967
		public new object SyntaxError
		{
			get
			{
				if (this.SyntaxErrorField is Missing)
				{
					this.SyntaxErrorField = this.originalSyntaxError;
				}
				return this.SyntaxErrorField;
			}
			set
			{
				this.SyntaxErrorField = value;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x00049970 File Offset: 0x00048970
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x00049991 File Offset: 0x00048991
		public new object TypeError
		{
			get
			{
				if (this.TypeErrorField is Missing)
				{
					this.TypeErrorField = this.originalTypeError;
				}
				return this.TypeErrorField;
			}
			set
			{
				this.TypeErrorField = value;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x0004999A File Offset: 0x0004899A
		// (set) Token: 0x06000984 RID: 2436 RVA: 0x000499BB File Offset: 0x000489BB
		public new object URIError
		{
			get
			{
				if (this.URIErrorField is Missing)
				{
					this.URIErrorField = this.originalURIError;
				}
				return this.URIErrorField;
			}
			set
			{
				this.URIErrorField = value;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x000499C4 File Offset: 0x000489C4
		// (set) Token: 0x06000986 RID: 2438 RVA: 0x000499E5 File Offset: 0x000489E5
		public new object VBArray
		{
			get
			{
				if (this.VBArrayField is Missing)
				{
					this.VBArrayField = this.originalVBArray;
				}
				return this.VBArrayField;
			}
			set
			{
				this.VBArrayField = value;
			}
		}

		// Token: 0x040005B1 RID: 1457
		public new object Infinity;

		// Token: 0x040005B2 RID: 1458
		private object MathField;

		// Token: 0x040005B3 RID: 1459
		public new object NaN;

		// Token: 0x040005B4 RID: 1460
		public new object undefined;

		// Token: 0x040005B5 RID: 1461
		private object ActiveXObjectField;

		// Token: 0x040005B6 RID: 1462
		private object ArrayField;

		// Token: 0x040005B7 RID: 1463
		private object BooleanField;

		// Token: 0x040005B8 RID: 1464
		private object DateField;

		// Token: 0x040005B9 RID: 1465
		private object EnumeratorField;

		// Token: 0x040005BA RID: 1466
		private object ErrorField;

		// Token: 0x040005BB RID: 1467
		private object EvalErrorField;

		// Token: 0x040005BC RID: 1468
		private object FunctionField;

		// Token: 0x040005BD RID: 1469
		private object NumberField;

		// Token: 0x040005BE RID: 1470
		private object ObjectField;

		// Token: 0x040005BF RID: 1471
		private object RangeErrorField;

		// Token: 0x040005C0 RID: 1472
		private object ReferenceErrorField;

		// Token: 0x040005C1 RID: 1473
		private object RegExpField;

		// Token: 0x040005C2 RID: 1474
		private object StringField;

		// Token: 0x040005C3 RID: 1475
		private object SyntaxErrorField;

		// Token: 0x040005C4 RID: 1476
		private object TypeErrorField;

		// Token: 0x040005C5 RID: 1477
		private object VBArrayField;

		// Token: 0x040005C6 RID: 1478
		private object URIErrorField;

		// Token: 0x040005C7 RID: 1479
		public new object decodeURI;

		// Token: 0x040005C8 RID: 1480
		public new object decodeURIComponent;

		// Token: 0x040005C9 RID: 1481
		public new object encodeURI;

		// Token: 0x040005CA RID: 1482
		public new object encodeURIComponent;

		// Token: 0x040005CB RID: 1483
		[NotRecommended("escape")]
		public new object escape;

		// Token: 0x040005CC RID: 1484
		public new object eval;

		// Token: 0x040005CD RID: 1485
		public new object isNaN;

		// Token: 0x040005CE RID: 1486
		public new object isFinite;

		// Token: 0x040005CF RID: 1487
		public new object parseInt;

		// Token: 0x040005D0 RID: 1488
		public new object parseFloat;

		// Token: 0x040005D1 RID: 1489
		public new object GetObject;

		// Token: 0x040005D2 RID: 1490
		public new object ScriptEngine;

		// Token: 0x040005D3 RID: 1491
		public new object ScriptEngineBuildVersion;

		// Token: 0x040005D4 RID: 1492
		public new object ScriptEngineMajorVersion;

		// Token: 0x040005D5 RID: 1493
		public new object ScriptEngineMinorVersion;

		// Token: 0x040005D6 RID: 1494
		[NotRecommended("unescape")]
		public new object unescape;

		// Token: 0x040005D7 RID: 1495
		public new object boolean;

		// Token: 0x040005D8 RID: 1496
		public new object @byte;

		// Token: 0x040005D9 RID: 1497
		public new object @char;

		// Token: 0x040005DA RID: 1498
		public new object @decimal;

		// Token: 0x040005DB RID: 1499
		public new object @double;

		// Token: 0x040005DC RID: 1500
		public new object @float;

		// Token: 0x040005DD RID: 1501
		public new object @int;

		// Token: 0x040005DE RID: 1502
		public new object @long;

		// Token: 0x040005DF RID: 1503
		public new object @sbyte;

		// Token: 0x040005E0 RID: 1504
		public new object @short;

		// Token: 0x040005E1 RID: 1505
		public new object @void;

		// Token: 0x040005E2 RID: 1506
		public new object @uint;

		// Token: 0x040005E3 RID: 1507
		public new object @ulong;

		// Token: 0x040005E4 RID: 1508
		public new object @ushort;

		// Token: 0x040005E5 RID: 1509
		private LenientArrayPrototype arrayPrototypeField;

		// Token: 0x040005E6 RID: 1510
		private LenientFunctionPrototype functionPrototypeField;

		// Token: 0x040005E7 RID: 1511
		private LenientObjectPrototype objectPrototypeField;

		// Token: 0x040005E8 RID: 1512
		private VsaEngine engine;
	}
}

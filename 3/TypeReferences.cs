using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Expando;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x02000129 RID: 297
	internal sealed class TypeReferences
	{
		// Token: 0x06000C95 RID: 3221 RVA: 0x0005BD6F File Offset: 0x0005AD6F
		internal TypeReferences(Module jscriptReferenceModule)
		{
			this._jscriptReferenceModule = jscriptReferenceModule;
			this._typeTable = new Type[83];
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0005BD8C File Offset: 0x0005AD8C
		internal Type GetPredefinedType(string typeName)
		{
			object obj = TypeReferences._predefinedTypeTable[typeName];
			Type type = obj as Type;
			if (type == null && obj is TypeReferences.TypeReference)
			{
				type = this.GetTypeReference((TypeReferences.TypeReference)obj);
			}
			return type;
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0005BDC8 File Offset: 0x0005ADC8
		static TypeReferences()
		{
			TypeReferences._predefinedTypeTable["boolean"] = typeof(bool);
			TypeReferences._predefinedTypeTable["byte"] = typeof(byte);
			TypeReferences._predefinedTypeTable["char"] = typeof(char);
			TypeReferences._predefinedTypeTable["decimal"] = typeof(decimal);
			TypeReferences._predefinedTypeTable["double"] = typeof(double);
			TypeReferences._predefinedTypeTable["float"] = typeof(float);
			TypeReferences._predefinedTypeTable["int"] = typeof(int);
			TypeReferences._predefinedTypeTable["long"] = typeof(long);
			TypeReferences._predefinedTypeTable["sbyte"] = typeof(sbyte);
			TypeReferences._predefinedTypeTable["short"] = typeof(short);
			TypeReferences._predefinedTypeTable["void"] = typeof(void);
			TypeReferences._predefinedTypeTable["uint"] = typeof(uint);
			TypeReferences._predefinedTypeTable["ulong"] = typeof(ulong);
			TypeReferences._predefinedTypeTable["ushort"] = typeof(ushort);
			TypeReferences._predefinedTypeTable["ActiveXObject"] = typeof(object);
			TypeReferences._predefinedTypeTable["Boolean"] = typeof(bool);
			TypeReferences._predefinedTypeTable["Number"] = typeof(double);
			TypeReferences._predefinedTypeTable["Object"] = typeof(object);
			TypeReferences._predefinedTypeTable["String"] = typeof(string);
			TypeReferences._predefinedTypeTable["Type"] = typeof(Type);
			TypeReferences._predefinedTypeTable["Array"] = TypeReferences.TypeReference.ArrayObject;
			TypeReferences._predefinedTypeTable["Date"] = TypeReferences.TypeReference.DateObject;
			TypeReferences._predefinedTypeTable["Enumerator"] = TypeReferences.TypeReference.EnumeratorObject;
			TypeReferences._predefinedTypeTable["Error"] = TypeReferences.TypeReference.ErrorObject;
			TypeReferences._predefinedTypeTable["EvalError"] = TypeReferences.TypeReference.EvalErrorObject;
			TypeReferences._predefinedTypeTable["Function"] = TypeReferences.TypeReference.ScriptFunction;
			TypeReferences._predefinedTypeTable["RangeError"] = TypeReferences.TypeReference.RangeErrorObject;
			TypeReferences._predefinedTypeTable["ReferenceError"] = TypeReferences.TypeReference.ReferenceErrorObject;
			TypeReferences._predefinedTypeTable["RegExp"] = TypeReferences.TypeReference.RegExpObject;
			TypeReferences._predefinedTypeTable["SyntaxError"] = TypeReferences.TypeReference.SyntaxErrorObject;
			TypeReferences._predefinedTypeTable["TypeError"] = TypeReferences.TypeReference.TypeErrorObject;
			TypeReferences._predefinedTypeTable["URIError"] = TypeReferences.TypeReference.URIErrorObject;
			TypeReferences._predefinedTypeTable["VBArray"] = TypeReferences.TypeReference.VBArrayObject;
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0005C0F2 File Offset: 0x0005B0F2
		private Module JScriptReferenceModule
		{
			get
			{
				return this._jscriptReferenceModule;
			}
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0005C0FC File Offset: 0x0005B0FC
		private Type GetTypeReference(TypeReferences.TypeReference typeRef)
		{
			Type type = this._typeTable[(int)typeRef];
			if (type == null)
			{
				string text = "Microsoft.JScript.";
				if (typeRef >= TypeReferences.TypeReference.BaseVsaStartup)
				{
					switch (typeRef)
					{
					case TypeReferences.TypeReference.BaseVsaStartup:
						text = "Microsoft.Vsa.";
						break;
					case TypeReferences.TypeReference.VsaEngine:
						text = "Microsoft.JScript.Vsa.";
						break;
					}
				}
				type = this.JScriptReferenceModule.GetType(text + global::System.Enum.GetName(typeof(TypeReferences.TypeReference), (int)typeRef));
				this._typeTable[(int)typeRef] = type;
			}
			return type;
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x0005C174 File Offset: 0x0005B174
		internal Type ArgumentsObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ArgumentsObject);
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0005C17D File Offset: 0x0005B17D
		internal Type ArrayConstructor
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ArrayConstructor);
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x0005C186 File Offset: 0x0005B186
		internal Type ArrayObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ArrayObject);
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x0005C18F File Offset: 0x0005B18F
		internal Type ArrayWrapper
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ArrayWrapper);
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0005C198 File Offset: 0x0005B198
		internal Type BaseVsaStartup
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.BaseVsaStartup);
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x0005C1A2 File Offset: 0x0005B1A2
		internal Type Binding
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Binding);
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x0005C1AB File Offset: 0x0005B1AB
		internal Type BitwiseBinary
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.BitwiseBinary);
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x0005C1B4 File Offset: 0x0005B1B4
		internal Type BooleanObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.BooleanObject);
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x0005C1BD File Offset: 0x0005B1BD
		internal Type BreakOutOfFinally
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.BreakOutOfFinally);
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x0005C1C6 File Offset: 0x0005B1C6
		internal Type BuiltinFunction
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.BuiltinFunction);
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0005C1CF File Offset: 0x0005B1CF
		internal Type ClassScope
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ClassScope);
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x0005C1D9 File Offset: 0x0005B1D9
		internal Type Closure
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Closure);
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x0005C1E3 File Offset: 0x0005B1E3
		internal Type ContinueOutOfFinally
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ContinueOutOfFinally);
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x0005C1ED File Offset: 0x0005B1ED
		internal Type Convert
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Convert);
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x0005C1F7 File Offset: 0x0005B1F7
		internal Type DateObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.DateObject);
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x0005C201 File Offset: 0x0005B201
		internal Type Empty
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Empty);
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x0005C20B File Offset: 0x0005B20B
		internal Type EnumeratorObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.EnumeratorObject);
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x0005C215 File Offset: 0x0005B215
		internal Type Equality
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Equality);
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x0005C21F File Offset: 0x0005B21F
		internal Type ErrorObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ErrorObject);
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x0005C229 File Offset: 0x0005B229
		internal Type Eval
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Eval);
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x0005C233 File Offset: 0x0005B233
		internal Type EvalErrorObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.EvalErrorObject);
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x0005C23D File Offset: 0x0005B23D
		internal Type Expando
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Expando);
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x0005C247 File Offset: 0x0005B247
		internal Type FieldAccessor
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.FieldAccessor);
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x0005C251 File Offset: 0x0005B251
		internal Type ForIn
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ForIn);
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x0005C25B File Offset: 0x0005B25B
		internal Type FunctionDeclaration
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.FunctionDeclaration);
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x0005C265 File Offset: 0x0005B265
		internal Type FunctionExpression
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.FunctionExpression);
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x0005C26F File Offset: 0x0005B26F
		internal Type FunctionObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.FunctionObject);
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x0005C279 File Offset: 0x0005B279
		internal Type FunctionWrapper
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.FunctionWrapper);
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0005C283 File Offset: 0x0005B283
		internal Type GlobalObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.GlobalObject);
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0005C28D File Offset: 0x0005B28D
		internal Type GlobalScope
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.GlobalScope);
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x0005C297 File Offset: 0x0005B297
		internal Type Globals
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Globals);
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x0005C2A1 File Offset: 0x0005B2A1
		internal Type Hide
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Hide);
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x0005C2AB File Offset: 0x0005B2AB
		internal Type IActivationObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.IActivationObject);
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x0005C2B5 File Offset: 0x0005B2B5
		internal Type INeedEngine
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.INeedEngine);
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x0005C2BF File Offset: 0x0005B2BF
		internal Type Import
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Import);
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x0005C2C9 File Offset: 0x0005B2C9
		internal Type In
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.In);
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x0005C2D3 File Offset: 0x0005B2D3
		internal Type Instanceof
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Instanceof);
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x0005C2DD File Offset: 0x0005B2DD
		internal Type JSError
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.JSError);
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x0005C2E7 File Offset: 0x0005B2E7
		internal Type JSFunctionAttribute
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.JSFunctionAttribute);
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0005C2F1 File Offset: 0x0005B2F1
		internal Type JSFunctionAttributeEnum
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.JSFunctionAttributeEnum);
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x0005C2FB File Offset: 0x0005B2FB
		internal Type JSLocalField
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.JSLocalField);
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0005C305 File Offset: 0x0005B305
		internal Type JSObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.JSObject);
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x0005C30F File Offset: 0x0005B30F
		internal Type JScriptException
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.JScriptException);
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0005C319 File Offset: 0x0005B319
		internal Type LateBinding
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.LateBinding);
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x0005C323 File Offset: 0x0005B323
		internal Type LenientGlobalObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.LenientGlobalObject);
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0005C32D File Offset: 0x0005B32D
		internal Type MathObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.MathObject);
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x0005C337 File Offset: 0x0005B337
		internal Type MethodInvoker
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.MethodInvoker);
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x0005C341 File Offset: 0x0005B341
		internal Type Missing
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Missing);
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x0005C34B File Offset: 0x0005B34B
		internal Type Namespace
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Namespace);
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0005C355 File Offset: 0x0005B355
		internal Type NotRecommended
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.NotRecommended);
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x0005C35F File Offset: 0x0005B35F
		internal Type NumberObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.NumberObject);
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x0005C369 File Offset: 0x0005B369
		internal Type NumericBinary
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.NumericBinary);
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0005C373 File Offset: 0x0005B373
		internal Type NumericUnary
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.NumericUnary);
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x0005C37D File Offset: 0x0005B37D
		internal Type ObjectConstructor
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ObjectConstructor);
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0005C387 File Offset: 0x0005B387
		internal Type Override
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Override);
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0005C391 File Offset: 0x0005B391
		internal Type Package
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Package);
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x0005C39B File Offset: 0x0005B39B
		internal Type Plus
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Plus);
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x0005C3A5 File Offset: 0x0005B3A5
		internal Type PostOrPrefixOperator
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.PostOrPrefixOperator);
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x0005C3AF File Offset: 0x0005B3AF
		internal Type RangeErrorObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.RangeErrorObject);
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x0005C3B9 File Offset: 0x0005B3B9
		internal Type ReferenceAttribute
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ReferenceAttribute);
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x0005C3C3 File Offset: 0x0005B3C3
		internal Type ReferenceErrorObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ReferenceErrorObject);
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x0005C3CD File Offset: 0x0005B3CD
		internal Type RegExpConstructor
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.RegExpConstructor);
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x0005C3D7 File Offset: 0x0005B3D7
		internal Type RegExpObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.RegExpObject);
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x0005C3E1 File Offset: 0x0005B3E1
		internal Type Relational
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Relational);
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x0005C3EB File Offset: 0x0005B3EB
		internal Type ReturnOutOfFinally
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ReturnOutOfFinally);
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x0005C3F5 File Offset: 0x0005B3F5
		internal Type Runtime
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Runtime);
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x0005C3FF File Offset: 0x0005B3FF
		internal Type ScriptFunction
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ScriptFunction);
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x0005C409 File Offset: 0x0005B409
		internal Type ScriptObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ScriptObject);
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x0005C413 File Offset: 0x0005B413
		internal Type ScriptStream
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.ScriptStream);
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x0005C41D File Offset: 0x0005B41D
		internal Type SimpleHashtable
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.SimpleHashtable);
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x0005C427 File Offset: 0x0005B427
		internal Type StackFrame
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.StackFrame);
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x0005C431 File Offset: 0x0005B431
		internal Type StrictEquality
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.StrictEquality);
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x0005C43B File Offset: 0x0005B43B
		internal Type StringObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.StringObject);
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x0005C445 File Offset: 0x0005B445
		internal Type SyntaxErrorObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.SyntaxErrorObject);
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0005C44F File Offset: 0x0005B44F
		internal Type Throw
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Throw);
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x0005C459 File Offset: 0x0005B459
		internal Type Try
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Try);
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x0005C463 File Offset: 0x0005B463
		internal Type TypedArray
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.TypedArray);
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x0005C46D File Offset: 0x0005B46D
		internal Type TypeErrorObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.TypeErrorObject);
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x0005C477 File Offset: 0x0005B477
		internal Type Typeof
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.Typeof);
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x0005C481 File Offset: 0x0005B481
		internal Type URIErrorObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.URIErrorObject);
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x0005C48B File Offset: 0x0005B48B
		internal Type VBArrayObject
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.VBArrayObject);
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x0005C495 File Offset: 0x0005B495
		internal Type With
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.With);
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x0005C49F File Offset: 0x0005B49F
		internal Type VsaEngine
		{
			get
			{
				return this.GetTypeReference(TypeReferences.TypeReference.VsaEngine);
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x0005C4A9 File Offset: 0x0005B4A9
		internal Type Array
		{
			get
			{
				return typeof(Array);
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x0005C4B5 File Offset: 0x0005B4B5
		internal Type Attribute
		{
			get
			{
				return typeof(Attribute);
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x0005C4C1 File Offset: 0x0005B4C1
		internal Type AttributeUsageAttribute
		{
			get
			{
				return typeof(AttributeUsageAttribute);
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x0005C4CD File Offset: 0x0005B4CD
		internal Type Byte
		{
			get
			{
				return typeof(byte);
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0005C4D9 File Offset: 0x0005B4D9
		internal Type Boolean
		{
			get
			{
				return typeof(bool);
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x0005C4E5 File Offset: 0x0005B4E5
		internal Type Char
		{
			get
			{
				return typeof(char);
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x0005C4F1 File Offset: 0x0005B4F1
		internal Type CLSCompliantAttribute
		{
			get
			{
				return typeof(CLSCompliantAttribute);
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x0005C4FD File Offset: 0x0005B4FD
		internal Type ContextStaticAttribute
		{
			get
			{
				return typeof(ContextStaticAttribute);
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x0005C509 File Offset: 0x0005B509
		internal Type DateTime
		{
			get
			{
				return typeof(DateTime);
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x0005C515 File Offset: 0x0005B515
		internal Type DBNull
		{
			get
			{
				return typeof(DBNull);
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x0005C521 File Offset: 0x0005B521
		internal Type Delegate
		{
			get
			{
				return typeof(Delegate);
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x0005C52D File Offset: 0x0005B52D
		internal Type Decimal
		{
			get
			{
				return typeof(decimal);
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x0005C539 File Offset: 0x0005B539
		internal Type Double
		{
			get
			{
				return typeof(double);
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x0005C545 File Offset: 0x0005B545
		internal Type Enum
		{
			get
			{
				return typeof(Enum);
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x0005C551 File Offset: 0x0005B551
		internal Type Exception
		{
			get
			{
				return typeof(Exception);
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x0005C55D File Offset: 0x0005B55D
		internal Type IConvertible
		{
			get
			{
				return typeof(IConvertible);
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x0005C569 File Offset: 0x0005B569
		internal Type IntPtr
		{
			get
			{
				return typeof(IntPtr);
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x0005C575 File Offset: 0x0005B575
		internal Type Int16
		{
			get
			{
				return typeof(short);
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x0005C581 File Offset: 0x0005B581
		internal Type Int32
		{
			get
			{
				return typeof(int);
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0005C58D File Offset: 0x0005B58D
		internal Type Int64
		{
			get
			{
				return typeof(long);
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x0005C599 File Offset: 0x0005B599
		internal Type Object
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x0005C5A5 File Offset: 0x0005B5A5
		internal Type ObsoleteAttribute
		{
			get
			{
				return typeof(ObsoleteAttribute);
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x0005C5B1 File Offset: 0x0005B5B1
		internal Type ParamArrayAttribute
		{
			get
			{
				return typeof(ParamArrayAttribute);
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x0005C5BD File Offset: 0x0005B5BD
		internal Type RuntimeTypeHandle
		{
			get
			{
				return typeof(RuntimeTypeHandle);
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x0005C5C9 File Offset: 0x0005B5C9
		internal Type SByte
		{
			get
			{
				return typeof(sbyte);
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x0005C5D5 File Offset: 0x0005B5D5
		internal Type Single
		{
			get
			{
				return typeof(float);
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x0005C5E1 File Offset: 0x0005B5E1
		internal Type STAThreadAttribute
		{
			get
			{
				return typeof(STAThreadAttribute);
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x0005C5ED File Offset: 0x0005B5ED
		internal Type String
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x0005C5F9 File Offset: 0x0005B5F9
		internal Type Type
		{
			get
			{
				return typeof(Type);
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x0005C605 File Offset: 0x0005B605
		internal Type TypeCode
		{
			get
			{
				return typeof(TypeCode);
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x0005C611 File Offset: 0x0005B611
		internal Type UIntPtr
		{
			get
			{
				return typeof(UIntPtr);
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x0005C61D File Offset: 0x0005B61D
		internal Type UInt16
		{
			get
			{
				return typeof(ushort);
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x0005C629 File Offset: 0x0005B629
		internal Type UInt32
		{
			get
			{
				return typeof(uint);
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x0005C635 File Offset: 0x0005B635
		internal Type UInt64
		{
			get
			{
				return typeof(ulong);
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x0005C641 File Offset: 0x0005B641
		internal Type ValueType
		{
			get
			{
				return typeof(ValueType);
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x0005C64D File Offset: 0x0005B64D
		internal Type Void
		{
			get
			{
				return typeof(void);
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x0005C659 File Offset: 0x0005B659
		internal Type IEnumerable
		{
			get
			{
				return typeof(IEnumerable);
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x0005C665 File Offset: 0x0005B665
		internal Type IEnumerator
		{
			get
			{
				return typeof(IEnumerator);
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x0005C671 File Offset: 0x0005B671
		internal Type IList
		{
			get
			{
				return typeof(IList);
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x0005C67D File Offset: 0x0005B67D
		internal Type Debugger
		{
			get
			{
				return typeof(Debugger);
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x0005C689 File Offset: 0x0005B689
		internal Type DebuggableAttribute
		{
			get
			{
				return typeof(DebuggableAttribute);
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0005C695 File Offset: 0x0005B695
		internal Type DebuggerHiddenAttribute
		{
			get
			{
				return typeof(DebuggerHiddenAttribute);
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x0005C6A1 File Offset: 0x0005B6A1
		internal Type DebuggerStepThroughAttribute
		{
			get
			{
				return typeof(DebuggerStepThroughAttribute);
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x0005C6AD File Offset: 0x0005B6AD
		internal Type DefaultMemberAttribute
		{
			get
			{
				return typeof(DefaultMemberAttribute);
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x0005C6B9 File Offset: 0x0005B6B9
		internal Type EventInfo
		{
			get
			{
				return typeof(EventInfo);
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x0005C6C5 File Offset: 0x0005B6C5
		internal Type FieldInfo
		{
			get
			{
				return typeof(FieldInfo);
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x0005C6D1 File Offset: 0x0005B6D1
		internal Type CompilerGlobalScopeAttribute
		{
			get
			{
				return typeof(CompilerGlobalScopeAttribute);
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x0005C6DD File Offset: 0x0005B6DD
		internal Type RequiredAttributeAttribute
		{
			get
			{
				return typeof(RequiredAttributeAttribute);
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x0005C6E9 File Offset: 0x0005B6E9
		internal Type CoClassAttribute
		{
			get
			{
				return typeof(CoClassAttribute);
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x0005C6F5 File Offset: 0x0005B6F5
		internal Type IExpando
		{
			get
			{
				return typeof(IExpando);
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x0005C701 File Offset: 0x0005B701
		internal Type CodeAccessSecurityAttribute
		{
			get
			{
				return typeof(CodeAccessSecurityAttribute);
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000D20 RID: 3360 RVA: 0x0005C70D File Offset: 0x0005B70D
		internal Type AllowPartiallyTrustedCallersAttribute
		{
			get
			{
				return typeof(AllowPartiallyTrustedCallersAttribute);
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x0005C719 File Offset: 0x0005B719
		internal Type ArrayOfObject
		{
			get
			{
				return typeof(object[]);
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000D22 RID: 3362 RVA: 0x0005C725 File Offset: 0x0005B725
		internal Type ArrayOfString
		{
			get
			{
				return typeof(string[]);
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x0005C731 File Offset: 0x0005B731
		internal Type SystemConvert
		{
			get
			{
				return typeof(Convert);
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x0005C73D File Offset: 0x0005B73D
		internal Type ReflectionMissing
		{
			get
			{
				return typeof(Missing);
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x0005C749 File Offset: 0x0005B749
		internal MethodInfo constructArrayMethod
		{
			get
			{
				return this.ArrayConstructor.GetMethod("ConstructArray");
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000D26 RID: 3366 RVA: 0x0005C75B File Offset: 0x0005B75B
		internal MethodInfo isMissingMethod
		{
			get
			{
				return this.Binding.GetMethod("IsMissing");
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x0005C770 File Offset: 0x0005B770
		internal ConstructorInfo bitwiseBinaryConstructor
		{
			get
			{
				return this.BitwiseBinary.GetConstructor(new Type[] { this.Int32 });
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x0005C799 File Offset: 0x0005B799
		internal MethodInfo evaluateBitwiseBinaryMethod
		{
			get
			{
				return this.BitwiseBinary.GetMethod("EvaluateBitwiseBinary");
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x0005C7AC File Offset: 0x0005B7AC
		internal ConstructorInfo breakOutOfFinallyConstructor
		{
			get
			{
				return this.BreakOutOfFinally.GetConstructor(new Type[] { this.Int32 });
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0005C7D8 File Offset: 0x0005B7D8
		internal ConstructorInfo closureConstructor
		{
			get
			{
				return this.Closure.GetConstructor(new Type[] { this.FunctionObject });
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x0005C804 File Offset: 0x0005B804
		internal ConstructorInfo continueOutOfFinallyConstructor
		{
			get
			{
				return this.ContinueOutOfFinally.GetConstructor(new Type[] { this.Int32 });
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000D2C RID: 3372 RVA: 0x0005C82D File Offset: 0x0005B82D
		internal MethodInfo checkIfDoubleIsIntegerMethod
		{
			get
			{
				return this.Convert.GetMethod("CheckIfDoubleIsInteger");
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x0005C83F File Offset: 0x0005B83F
		internal MethodInfo checkIfSingleIsIntegerMethod
		{
			get
			{
				return this.Convert.GetMethod("CheckIfSingleIsInteger");
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000D2E RID: 3374 RVA: 0x0005C851 File Offset: 0x0005B851
		internal MethodInfo coerce2Method
		{
			get
			{
				return this.Convert.GetMethod("Coerce2");
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000D2F RID: 3375 RVA: 0x0005C863 File Offset: 0x0005B863
		internal MethodInfo coerceTMethod
		{
			get
			{
				return this.Convert.GetMethod("CoerceT");
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000D30 RID: 3376 RVA: 0x0005C875 File Offset: 0x0005B875
		internal MethodInfo throwTypeMismatch
		{
			get
			{
				return this.Convert.GetMethod("ThrowTypeMismatch");
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x0005C888 File Offset: 0x0005B888
		internal MethodInfo doubleToBooleanMethod
		{
			get
			{
				return this.Convert.GetMethod("ToBoolean", new Type[] { this.Double });
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000D32 RID: 3378 RVA: 0x0005C8B8 File Offset: 0x0005B8B8
		internal MethodInfo toBooleanMethod
		{
			get
			{
				return this.Convert.GetMethod("ToBoolean", new Type[] { this.Object, this.Boolean });
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x0005C8F0 File Offset: 0x0005B8F0
		internal MethodInfo toForInObjectMethod
		{
			get
			{
				return this.Convert.GetMethod("ToForInObject", new Type[] { this.Object, this.VsaEngine });
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x0005C928 File Offset: 0x0005B928
		internal MethodInfo toInt32Method
		{
			get
			{
				return this.Convert.GetMethod("ToInt32", new Type[] { this.Object });
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x0005C956 File Offset: 0x0005B956
		internal MethodInfo toNativeArrayMethod
		{
			get
			{
				return this.Convert.GetMethod("ToNativeArray");
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000D36 RID: 3382 RVA: 0x0005C968 File Offset: 0x0005B968
		internal MethodInfo toNumberMethod
		{
			get
			{
				return this.Convert.GetMethod("ToNumber", new Type[] { this.Object });
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x0005C998 File Offset: 0x0005B998
		internal MethodInfo toObjectMethod
		{
			get
			{
				return this.Convert.GetMethod("ToObject", new Type[] { this.Object, this.VsaEngine });
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000D38 RID: 3384 RVA: 0x0005C9D0 File Offset: 0x0005B9D0
		internal MethodInfo toObject2Method
		{
			get
			{
				return this.Convert.GetMethod("ToObject2", new Type[] { this.Object, this.VsaEngine });
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x0005CA08 File Offset: 0x0005BA08
		internal MethodInfo doubleToStringMethod
		{
			get
			{
				return this.Convert.GetMethod("ToString", new Type[] { this.Double });
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000D3A RID: 3386 RVA: 0x0005CA38 File Offset: 0x0005BA38
		internal MethodInfo toStringMethod
		{
			get
			{
				return this.Convert.GetMethod("ToString", new Type[] { this.Object, this.Boolean });
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x0005CA6F File Offset: 0x0005BA6F
		internal FieldInfo undefinedField
		{
			get
			{
				return this.Empty.GetField("Value");
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x0005CA84 File Offset: 0x0005BA84
		internal ConstructorInfo equalityConstructor
		{
			get
			{
				return this.Equality.GetConstructor(new Type[] { this.Int32 });
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x0005CAB0 File Offset: 0x0005BAB0
		internal MethodInfo evaluateEqualityMethod
		{
			get
			{
				return this.Equality.GetMethod("EvaluateEquality", new Type[] { this.Object, this.Object });
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x0005CAE7 File Offset: 0x0005BAE7
		internal MethodInfo jScriptEqualsMethod
		{
			get
			{
				return this.Equality.GetMethod("JScriptEquals");
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x0005CAFC File Offset: 0x0005BAFC
		internal MethodInfo jScriptEvaluateMethod1
		{
			get
			{
				return this.Eval.GetMethod("JScriptEvaluate", new Type[] { this.Object, this.VsaEngine });
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x0005CB34 File Offset: 0x0005BB34
		internal MethodInfo jScriptEvaluateMethod2
		{
			get
			{
				return this.Eval.GetMethod("JScriptEvaluate", new Type[] { this.Object, this.Object, this.VsaEngine });
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x0005CB74 File Offset: 0x0005BB74
		internal MethodInfo jScriptGetEnumeratorMethod
		{
			get
			{
				return this.ForIn.GetMethod("JScriptGetEnumerator");
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000D42 RID: 3394 RVA: 0x0005CB86 File Offset: 0x0005BB86
		internal MethodInfo jScriptFunctionDeclarationMethod
		{
			get
			{
				return this.FunctionDeclaration.GetMethod("JScriptFunctionDeclaration");
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x0005CB98 File Offset: 0x0005BB98
		internal MethodInfo jScriptFunctionExpressionMethod
		{
			get
			{
				return this.FunctionExpression.GetMethod("JScriptFunctionExpression");
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x0005CBAA File Offset: 0x0005BBAA
		internal FieldInfo contextEngineField
		{
			get
			{
				return this.Globals.GetField("contextEngine");
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x0005CBBC File Offset: 0x0005BBBC
		internal MethodInfo fastConstructArrayLiteralMethod
		{
			get
			{
				return this.Globals.GetMethod("ConstructArrayLiteral");
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x0005CBD0 File Offset: 0x0005BBD0
		internal ConstructorInfo globalScopeConstructor
		{
			get
			{
				return this.GlobalScope.GetConstructor(new Type[] { this.GlobalScope, this.VsaEngine });
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000D47 RID: 3399 RVA: 0x0005CC02 File Offset: 0x0005BC02
		internal MethodInfo getDefaultThisObjectMethod
		{
			get
			{
				return this.IActivationObject.GetMethod("GetDefaultThisObject");
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x0005CC14 File Offset: 0x0005BC14
		internal MethodInfo getFieldMethod
		{
			get
			{
				return this.IActivationObject.GetMethod("GetField", new Type[] { this.String, this.Int32 });
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x0005CC4B File Offset: 0x0005BC4B
		internal MethodInfo getGlobalScopeMethod
		{
			get
			{
				return this.IActivationObject.GetMethod("GetGlobalScope");
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x0005CC60 File Offset: 0x0005BC60
		internal MethodInfo getMemberValueMethod
		{
			get
			{
				return this.IActivationObject.GetMethod("GetMemberValue", new Type[] { this.String, this.Int32 });
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x0005CC97 File Offset: 0x0005BC97
		internal MethodInfo jScriptImportMethod
		{
			get
			{
				return this.Import.GetMethod("JScriptImport");
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000D4C RID: 3404 RVA: 0x0005CCA9 File Offset: 0x0005BCA9
		internal MethodInfo jScriptInMethod
		{
			get
			{
				return this.In.GetMethod("JScriptIn");
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x0005CCBB File Offset: 0x0005BCBB
		internal MethodInfo getEngineMethod
		{
			get
			{
				return this.INeedEngine.GetMethod("GetEngine");
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000D4E RID: 3406 RVA: 0x0005CCCD File Offset: 0x0005BCCD
		internal MethodInfo setEngineMethod
		{
			get
			{
				return this.INeedEngine.GetMethod("SetEngine");
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000D4F RID: 3407 RVA: 0x0005CCDF File Offset: 0x0005BCDF
		internal MethodInfo jScriptInstanceofMethod
		{
			get
			{
				return this.Instanceof.GetMethod("JScriptInstanceof");
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000D50 RID: 3408 RVA: 0x0005CCF4 File Offset: 0x0005BCF4
		internal ConstructorInfo scriptExceptionConstructor
		{
			get
			{
				return this.JScriptException.GetConstructor(new Type[] { this.JSError });
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000D51 RID: 3409 RVA: 0x0005CD20 File Offset: 0x0005BD20
		internal ConstructorInfo jsFunctionAttributeConstructor
		{
			get
			{
				return this.JSFunctionAttribute.GetConstructor(new Type[] { this.JSFunctionAttributeEnum });
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x0005CD4C File Offset: 0x0005BD4C
		internal ConstructorInfo jsLocalFieldConstructor
		{
			get
			{
				return this.JSLocalField.GetConstructor(new Type[] { this.String, this.RuntimeTypeHandle, this.Int32 });
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000D53 RID: 3411 RVA: 0x0005CD88 File Offset: 0x0005BD88
		internal MethodInfo setMemberValue2Method
		{
			get
			{
				return this.JSObject.GetMethod("SetMemberValue2", new Type[] { this.String, this.Object });
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x0005CDC0 File Offset: 0x0005BDC0
		internal ConstructorInfo lateBindingConstructor2
		{
			get
			{
				return this.LateBinding.GetConstructor(new Type[] { this.String, this.Object });
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000D55 RID: 3413 RVA: 0x0005CDF4 File Offset: 0x0005BDF4
		internal ConstructorInfo lateBindingConstructor
		{
			get
			{
				return this.LateBinding.GetConstructor(new Type[] { this.String });
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x0005CE1D File Offset: 0x0005BE1D
		internal FieldInfo objectField
		{
			get
			{
				return this.LateBinding.GetField("obj");
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x0005CE30 File Offset: 0x0005BE30
		internal MethodInfo callMethod
		{
			get
			{
				return this.LateBinding.GetMethod("Call", new Type[] { this.ArrayOfObject, this.Boolean, this.Boolean, this.VsaEngine });
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x0005CE7C File Offset: 0x0005BE7C
		internal MethodInfo callValueMethod
		{
			get
			{
				return this.LateBinding.GetMethod("CallValue", new Type[] { this.Object, this.Object, this.ArrayOfObject, this.Boolean, this.Boolean, this.VsaEngine });
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x0005CED8 File Offset: 0x0005BED8
		internal MethodInfo callValue2Method
		{
			get
			{
				return this.LateBinding.GetMethod("CallValue2", new Type[] { this.Object, this.Object, this.ArrayOfObject, this.Boolean, this.Boolean, this.VsaEngine });
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x0005CF33 File Offset: 0x0005BF33
		internal MethodInfo deleteMethod
		{
			get
			{
				return this.LateBinding.GetMethod("Delete");
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000D5B RID: 3419 RVA: 0x0005CF45 File Offset: 0x0005BF45
		internal MethodInfo deleteMemberMethod
		{
			get
			{
				return this.LateBinding.GetMethod("DeleteMember");
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x0005CF57 File Offset: 0x0005BF57
		internal MethodInfo getNonMissingValueMethod
		{
			get
			{
				return this.LateBinding.GetMethod("GetNonMissingValue");
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000D5D RID: 3421 RVA: 0x0005CF69 File Offset: 0x0005BF69
		internal MethodInfo getValue2Method
		{
			get
			{
				return this.LateBinding.GetMethod("GetValue2");
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x0005CF7B File Offset: 0x0005BF7B
		internal MethodInfo setIndexedPropertyValueStaticMethod
		{
			get
			{
				return this.LateBinding.GetMethod("SetIndexedPropertyValueStatic");
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000D5F RID: 3423 RVA: 0x0005CF8D File Offset: 0x0005BF8D
		internal MethodInfo setValueMethod
		{
			get
			{
				return this.LateBinding.GetMethod("SetValue");
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x0005CF9F File Offset: 0x0005BF9F
		internal FieldInfo missingField
		{
			get
			{
				return this.Missing.GetField("Value");
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000D61 RID: 3425 RVA: 0x0005CFB1 File Offset: 0x0005BFB1
		internal MethodInfo getNamespaceMethod
		{
			get
			{
				return this.Namespace.GetMethod("GetNamespace");
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x0005CFC4 File Offset: 0x0005BFC4
		internal ConstructorInfo numericBinaryConstructor
		{
			get
			{
				return this.NumericBinary.GetConstructor(new Type[] { this.Int32 });
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000D63 RID: 3427 RVA: 0x0005CFED File Offset: 0x0005BFED
		internal MethodInfo numericbinaryDoOpMethod
		{
			get
			{
				return this.NumericBinary.GetMethod("DoOp");
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x0005CFFF File Offset: 0x0005BFFF
		internal MethodInfo evaluateNumericBinaryMethod
		{
			get
			{
				return this.NumericBinary.GetMethod("EvaluateNumericBinary");
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000D65 RID: 3429 RVA: 0x0005D014 File Offset: 0x0005C014
		internal ConstructorInfo numericUnaryConstructor
		{
			get
			{
				return this.NumericUnary.GetConstructor(new Type[] { this.Int32 });
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x0005D03D File Offset: 0x0005C03D
		internal MethodInfo evaluateUnaryMethod
		{
			get
			{
				return this.NumericUnary.GetMethod("EvaluateUnary");
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000D67 RID: 3431 RVA: 0x0005D04F File Offset: 0x0005C04F
		internal MethodInfo constructObjectMethod
		{
			get
			{
				return this.ObjectConstructor.GetMethod("ConstructObject");
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x0005D061 File Offset: 0x0005C061
		internal MethodInfo jScriptPackageMethod
		{
			get
			{
				return this.Package.GetMethod("JScriptPackage");
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000D69 RID: 3433 RVA: 0x0005D073 File Offset: 0x0005C073
		internal ConstructorInfo plusConstructor
		{
			get
			{
				return this.Plus.GetConstructor(new Type[0]);
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x0005D086 File Offset: 0x0005C086
		internal MethodInfo plusDoOpMethod
		{
			get
			{
				return this.Plus.GetMethod("DoOp");
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x0005D098 File Offset: 0x0005C098
		internal MethodInfo evaluatePlusMethod
		{
			get
			{
				return this.Plus.GetMethod("EvaluatePlus");
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x0005D0AC File Offset: 0x0005C0AC
		internal ConstructorInfo postOrPrefixConstructor
		{
			get
			{
				return this.PostOrPrefixOperator.GetConstructor(new Type[] { this.Int32 });
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000D6D RID: 3437 RVA: 0x0005D0D5 File Offset: 0x0005C0D5
		internal MethodInfo evaluatePostOrPrefixOperatorMethod
		{
			get
			{
				return this.PostOrPrefixOperator.GetMethod("EvaluatePostOrPrefix");
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x0005D0E8 File Offset: 0x0005C0E8
		internal ConstructorInfo referenceAttributeConstructor
		{
			get
			{
				return this.ReferenceAttribute.GetConstructor(new Type[] { this.String });
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000D6F RID: 3439 RVA: 0x0005D114 File Offset: 0x0005C114
		internal MethodInfo regExpConstructMethod
		{
			get
			{
				return this.RegExpConstructor.GetMethod("Construct", new Type[] { this.String, this.Boolean, this.Boolean, this.Boolean });
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x0005D160 File Offset: 0x0005C160
		internal ConstructorInfo relationalConstructor
		{
			get
			{
				return this.Relational.GetConstructor(new Type[] { this.Int32 });
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x0005D189 File Offset: 0x0005C189
		internal MethodInfo evaluateRelationalMethod
		{
			get
			{
				return this.Relational.GetMethod("EvaluateRelational");
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x0005D19B File Offset: 0x0005C19B
		internal MethodInfo jScriptCompareMethod
		{
			get
			{
				return this.Relational.GetMethod("JScriptCompare");
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x0005D1AD File Offset: 0x0005C1AD
		internal ConstructorInfo returnOutOfFinallyConstructor
		{
			get
			{
				return this.ReturnOutOfFinally.GetConstructor(new Type[0]);
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x0005D1C0 File Offset: 0x0005C1C0
		internal MethodInfo doubleToInt64
		{
			get
			{
				return this.Runtime.GetMethod("DoubleToInt64");
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000D75 RID: 3445 RVA: 0x0005D1D2 File Offset: 0x0005C1D2
		internal MethodInfo uncheckedDecimalToInt64Method
		{
			get
			{
				return this.Runtime.GetMethod("UncheckedDecimalToInt64");
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x0005D1E4 File Offset: 0x0005C1E4
		internal FieldInfo engineField
		{
			get
			{
				return this.ScriptObject.GetField("engine");
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000D77 RID: 3447 RVA: 0x0005D1F6 File Offset: 0x0005C1F6
		internal MethodInfo getParentMethod
		{
			get
			{
				return this.ScriptObject.GetMethod("GetParent");
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x0005D208 File Offset: 0x0005C208
		internal MethodInfo writeMethod
		{
			get
			{
				return this.ScriptStream.GetMethod("Write");
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x0005D21A File Offset: 0x0005C21A
		internal MethodInfo writeLineMethod
		{
			get
			{
				return this.ScriptStream.GetMethod("WriteLine");
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000D7A RID: 3450 RVA: 0x0005D22C File Offset: 0x0005C22C
		internal ConstructorInfo hashtableCtor
		{
			get
			{
				return this.SimpleHashtable.GetConstructor(new Type[] { this.UInt32 });
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x0005D258 File Offset: 0x0005C258
		internal MethodInfo hashtableGetItem
		{
			get
			{
				return this.SimpleHashtable.GetMethod("get_Item", new Type[] { this.Object });
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x0005D286 File Offset: 0x0005C286
		internal MethodInfo hashTableGetEnumerator
		{
			get
			{
				return this.SimpleHashtable.GetMethod("GetEnumerator", Type.EmptyTypes);
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x0005D2A0 File Offset: 0x0005C2A0
		internal MethodInfo hashtableRemove
		{
			get
			{
				return this.SimpleHashtable.GetMethod("Remove", new Type[] { this.Object });
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x0005D2D0 File Offset: 0x0005C2D0
		internal MethodInfo hashtableSetItem
		{
			get
			{
				return this.SimpleHashtable.GetMethod("set_Item", new Type[] { this.Object, this.Object });
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x0005D307 File Offset: 0x0005C307
		internal FieldInfo closureInstanceField
		{
			get
			{
				return this.StackFrame.GetField("closureInstance");
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000D80 RID: 3456 RVA: 0x0005D319 File Offset: 0x0005C319
		internal FieldInfo localVarsField
		{
			get
			{
				return this.StackFrame.GetField("localVars");
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x0005D32B File Offset: 0x0005C32B
		internal MethodInfo pushStackFrameForMethod
		{
			get
			{
				return this.StackFrame.GetMethod("PushStackFrameForMethod");
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x0005D33D File Offset: 0x0005C33D
		internal MethodInfo pushStackFrameForStaticMethod
		{
			get
			{
				return this.StackFrame.GetMethod("PushStackFrameForStaticMethod");
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x0005D350 File Offset: 0x0005C350
		internal MethodInfo jScriptStrictEqualsMethod
		{
			get
			{
				return this.StrictEquality.GetMethod("JScriptStrictEquals", new Type[] { this.Object, this.Object });
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x0005D387 File Offset: 0x0005C387
		internal MethodInfo jScriptThrowMethod
		{
			get
			{
				return this.Throw.GetMethod("JScriptThrow");
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x0005D399 File Offset: 0x0005C399
		internal MethodInfo jScriptExceptionValueMethod
		{
			get
			{
				return this.Try.GetMethod("JScriptExceptionValue");
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x0005D3AB File Offset: 0x0005C3AB
		internal MethodInfo jScriptTypeofMethod
		{
			get
			{
				return this.Typeof.GetMethod("JScriptTypeof");
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x0005D3BD File Offset: 0x0005C3BD
		internal ConstructorInfo vsaEngineConstructor
		{
			get
			{
				return this.VsaEngine.GetConstructor(new Type[0]);
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x0005D3D0 File Offset: 0x0005C3D0
		internal MethodInfo createVsaEngine
		{
			get
			{
				return this.VsaEngine.GetMethod("CreateEngine", new Type[0]);
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x0005D3E8 File Offset: 0x0005C3E8
		internal MethodInfo createVsaEngineWithType
		{
			get
			{
				return this.VsaEngine.GetMethod("CreateEngineWithType", new Type[] { this.RuntimeTypeHandle });
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x0005D416 File Offset: 0x0005C416
		internal MethodInfo getOriginalArrayConstructorMethod
		{
			get
			{
				return this.VsaEngine.GetMethod("GetOriginalArrayConstructor");
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x0005D428 File Offset: 0x0005C428
		internal MethodInfo getOriginalObjectConstructorMethod
		{
			get
			{
				return this.VsaEngine.GetMethod("GetOriginalObjectConstructor");
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x0005D43A File Offset: 0x0005C43A
		internal MethodInfo getOriginalRegExpConstructorMethod
		{
			get
			{
				return this.VsaEngine.GetMethod("GetOriginalRegExpConstructor");
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x0005D44C File Offset: 0x0005C44C
		internal MethodInfo popScriptObjectMethod
		{
			get
			{
				return this.VsaEngine.GetMethod("PopScriptObject");
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x0005D45E File Offset: 0x0005C45E
		internal MethodInfo pushScriptObjectMethod
		{
			get
			{
				return this.VsaEngine.GetMethod("PushScriptObject");
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x0005D470 File Offset: 0x0005C470
		internal MethodInfo scriptObjectStackTopMethod
		{
			get
			{
				return this.VsaEngine.GetMethod("ScriptObjectStackTop");
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x0005D482 File Offset: 0x0005C482
		internal MethodInfo getLenientGlobalObjectMethod
		{
			get
			{
				return this.VsaEngine.GetProperty("LenientGlobalObject").GetGetMethod();
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x0005D499 File Offset: 0x0005C499
		internal MethodInfo jScriptWithMethod
		{
			get
			{
				return this.With.GetMethod("JScriptWith");
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x0005D4AC File Offset: 0x0005C4AC
		internal ConstructorInfo clsCompliantAttributeCtor
		{
			get
			{
				return this.CLSCompliantAttribute.GetConstructor(new Type[] { this.Boolean });
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x0005D4D5 File Offset: 0x0005C4D5
		internal MethodInfo getEnumeratorMethod
		{
			get
			{
				return this.IEnumerable.GetMethod("GetEnumerator", Type.EmptyTypes);
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x0005D4EC File Offset: 0x0005C4EC
		internal MethodInfo moveNextMethod
		{
			get
			{
				return this.IEnumerator.GetMethod("MoveNext", Type.EmptyTypes);
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000D95 RID: 3477 RVA: 0x0005D503 File Offset: 0x0005C503
		internal MethodInfo getCurrentMethod
		{
			get
			{
				return this.IEnumerator.GetProperty("Current", Type.EmptyTypes).GetGetMethod();
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x0005D51F File Offset: 0x0005C51F
		internal ConstructorInfo contextStaticAttributeCtor
		{
			get
			{
				return this.ContextStaticAttribute.GetConstructor(new Type[0]);
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x0005D534 File Offset: 0x0005C534
		internal MethodInfo changeTypeMethod
		{
			get
			{
				return this.SystemConvert.GetMethod("ChangeType", new Type[] { this.Object, this.TypeCode });
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x0005D56C File Offset: 0x0005C56C
		internal MethodInfo convertCharToStringMethod
		{
			get
			{
				return this.SystemConvert.GetMethod("ToString", new Type[] { this.Char });
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x0005D59C File Offset: 0x0005C59C
		internal ConstructorInfo dateTimeConstructor
		{
			get
			{
				return this.DateTime.GetConstructor(new Type[] { this.Int64 });
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x0005D5C5 File Offset: 0x0005C5C5
		internal MethodInfo dateTimeToStringMethod
		{
			get
			{
				return this.DateTime.GetMethod("ToString", new Type[0]);
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x0005D5DD File Offset: 0x0005C5DD
		internal MethodInfo dateTimeToInt64Method
		{
			get
			{
				return this.DateTime.GetProperty("Ticks").GetGetMethod();
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x0005D5F4 File Offset: 0x0005C5F4
		internal ConstructorInfo decimalConstructor
		{
			get
			{
				return this.Decimal.GetConstructor(new Type[] { this.Int32, this.Int32, this.Int32, this.Boolean, this.Byte });
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x0005D641 File Offset: 0x0005C641
		internal FieldInfo decimalZeroField
		{
			get
			{
				return this.Decimal.GetField("Zero");
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x0005D654 File Offset: 0x0005C654
		internal MethodInfo decimalCompare
		{
			get
			{
				return this.Decimal.GetMethod("Compare", new Type[] { this.Decimal, this.Decimal });
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x0005D68C File Offset: 0x0005C68C
		internal MethodInfo doubleToDecimalMethod
		{
			get
			{
				return this.Decimal.GetMethod("op_Explicit", new Type[] { this.Double });
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x0005D6BC File Offset: 0x0005C6BC
		internal MethodInfo int32ToDecimalMethod
		{
			get
			{
				return this.Decimal.GetMethod("op_Implicit", new Type[] { this.Int32 });
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x0005D6EC File Offset: 0x0005C6EC
		internal MethodInfo int64ToDecimalMethod
		{
			get
			{
				return this.Decimal.GetMethod("op_Implicit", new Type[] { this.Int64 });
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x0005D71C File Offset: 0x0005C71C
		internal MethodInfo uint32ToDecimalMethod
		{
			get
			{
				return this.Decimal.GetMethod("op_Implicit", new Type[] { this.UInt32 });
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x0005D74C File Offset: 0x0005C74C
		internal MethodInfo uint64ToDecimalMethod
		{
			get
			{
				return this.Decimal.GetMethod("op_Implicit", new Type[] { this.UInt64 });
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x0005D77C File Offset: 0x0005C77C
		internal MethodInfo decimalToDoubleMethod
		{
			get
			{
				return this.Decimal.GetMethod("ToDouble", new Type[] { this.Decimal });
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x0005D7AC File Offset: 0x0005C7AC
		internal MethodInfo decimalToInt32Method
		{
			get
			{
				return this.Decimal.GetMethod("ToInt32", new Type[] { this.Decimal });
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x0005D7DC File Offset: 0x0005C7DC
		internal MethodInfo decimalToInt64Method
		{
			get
			{
				return this.Decimal.GetMethod("ToInt64", new Type[] { this.Decimal });
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x0005D80A File Offset: 0x0005C80A
		internal MethodInfo decimalToStringMethod
		{
			get
			{
				return this.Decimal.GetMethod("ToString", new Type[0]);
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x0005D824 File Offset: 0x0005C824
		internal MethodInfo decimalToUInt32Method
		{
			get
			{
				return this.Decimal.GetMethod("ToUInt32", new Type[] { this.Decimal });
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x0005D854 File Offset: 0x0005C854
		internal MethodInfo decimalToUInt64Method
		{
			get
			{
				return this.Decimal.GetMethod("ToUInt64", new Type[] { this.Decimal });
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x0005D882 File Offset: 0x0005C882
		internal MethodInfo debugBreak
		{
			get
			{
				return this.Debugger.GetMethod("Break", new Type[0]);
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x0005D89A File Offset: 0x0005C89A
		internal ConstructorInfo debuggerHiddenAttributeCtor
		{
			get
			{
				return this.DebuggerHiddenAttribute.GetConstructor(new Type[0]);
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x0005D8AD File Offset: 0x0005C8AD
		internal ConstructorInfo debuggerStepThroughAttributeCtor
		{
			get
			{
				return this.DebuggerStepThroughAttribute.GetConstructor(new Type[0]);
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x0005D8C0 File Offset: 0x0005C8C0
		internal MethodInfo int32ToStringMethod
		{
			get
			{
				return this.Int32.GetMethod("ToString", new Type[0]);
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x0005D8D8 File Offset: 0x0005C8D8
		internal MethodInfo int64ToStringMethod
		{
			get
			{
				return this.Int64.GetMethod("ToString", new Type[0]);
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x0005D8F0 File Offset: 0x0005C8F0
		internal MethodInfo equalsMethod
		{
			get
			{
				return this.Object.GetMethod("Equals", new Type[] { this.Object });
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x0005D920 File Offset: 0x0005C920
		internal ConstructorInfo defaultMemberAttributeCtor
		{
			get
			{
				return this.DefaultMemberAttribute.GetConstructor(new Type[] { this.String });
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x0005D94C File Offset: 0x0005C94C
		internal MethodInfo getFieldValueMethod
		{
			get
			{
				return this.FieldInfo.GetMethod("GetValue", new Type[] { this.Object });
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x0005D97C File Offset: 0x0005C97C
		internal MethodInfo setFieldValueMethod
		{
			get
			{
				return this.FieldInfo.GetMethod("SetValue", new Type[] { this.Object, this.Object });
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x0005D9B3 File Offset: 0x0005C9B3
		internal FieldInfo systemReflectionMissingField
		{
			get
			{
				return this.ReflectionMissing.GetField("Value");
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x0005D9C5 File Offset: 0x0005C9C5
		internal ConstructorInfo compilerGlobalScopeAttributeCtor
		{
			get
			{
				return this.CompilerGlobalScopeAttribute.GetConstructor(new Type[0]);
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x0005D9D8 File Offset: 0x0005C9D8
		internal MethodInfo stringConcatArrMethod
		{
			get
			{
				return this.String.GetMethod("Concat", new Type[] { this.ArrayOfString });
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x0005DA08 File Offset: 0x0005CA08
		internal MethodInfo stringConcat4Method
		{
			get
			{
				return this.String.GetMethod("Concat", new Type[] { this.String, this.String, this.String, this.String });
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x0005DA54 File Offset: 0x0005CA54
		internal MethodInfo stringConcat3Method
		{
			get
			{
				return this.String.GetMethod("Concat", new Type[] { this.String, this.String, this.String });
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x0005DA94 File Offset: 0x0005CA94
		internal MethodInfo stringConcat2Method
		{
			get
			{
				return this.String.GetMethod("Concat", new Type[] { this.String, this.String });
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x0005DACC File Offset: 0x0005CACC
		internal MethodInfo stringEqualsMethod
		{
			get
			{
				return this.String.GetMethod("Equals", new Type[] { this.String, this.String });
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x0005DB03 File Offset: 0x0005CB03
		internal MethodInfo stringLengthMethod
		{
			get
			{
				return this.String.GetProperty("Length").GetGetMethod();
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x0005DB1C File Offset: 0x0005CB1C
		internal MethodInfo getMethodMethod
		{
			get
			{
				return this.Type.GetMethod("GetMethod", new Type[] { this.String });
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x0005DB4C File Offset: 0x0005CB4C
		internal MethodInfo getTypeMethod
		{
			get
			{
				return this.Type.GetMethod("GetType", new Type[] { this.String });
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x0005DB7C File Offset: 0x0005CB7C
		internal MethodInfo getTypeFromHandleMethod
		{
			get
			{
				return this.Type.GetMethod("GetTypeFromHandle", new Type[] { this.RuntimeTypeHandle });
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x0005DBAA File Offset: 0x0005CBAA
		internal MethodInfo uint32ToStringMethod
		{
			get
			{
				return this.UInt32.GetMethod("ToString", new Type[0]);
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x0005DBC2 File Offset: 0x0005CBC2
		internal MethodInfo uint64ToStringMethod
		{
			get
			{
				return this.UInt64.GetMethod("ToString", new Type[0]);
			}
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0005DBDC File Offset: 0x0005CBDC
		internal Type ToReferenceContext(Type type)
		{
			if (this.InReferenceContext(type))
			{
				return type;
			}
			if (type.IsArray)
			{
				return Microsoft.JScript.Convert.ToType(Microsoft.JScript.TypedArray.ToRankString(type.GetArrayRank()), this.ToReferenceContext(type.GetElementType()));
			}
			return this.JScriptReferenceModule.ResolveType(type.MetadataToken, null, null);
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0005DC2C File Offset: 0x0005CC2C
		internal IReflect ToReferenceContext(IReflect ireflect)
		{
			if (ireflect is Type)
			{
				return this.ToReferenceContext((Type)ireflect);
			}
			return ireflect;
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0005DC44 File Offset: 0x0005CC44
		internal MethodInfo ToReferenceContext(MethodInfo method)
		{
			if (method is JSMethod)
			{
				method = ((JSMethod)method).GetMethodInfo(null);
			}
			else if (method is JSMethodInfo)
			{
				method = ((JSMethodInfo)method).method;
			}
			return (MethodInfo)this.MapMemberInfoToReferenceContext(method);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0005DC7F File Offset: 0x0005CC7F
		internal PropertyInfo ToReferenceContext(PropertyInfo property)
		{
			return (PropertyInfo)this.MapMemberInfoToReferenceContext(property);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0005DC8D File Offset: 0x0005CC8D
		internal FieldInfo ToReferenceContext(FieldInfo field)
		{
			return (FieldInfo)this.MapMemberInfoToReferenceContext(field);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0005DC9B File Offset: 0x0005CC9B
		internal ConstructorInfo ToReferenceContext(ConstructorInfo constructor)
		{
			return (ConstructorInfo)this.MapMemberInfoToReferenceContext(constructor);
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0005DCA9 File Offset: 0x0005CCA9
		private MemberInfo MapMemberInfoToReferenceContext(MemberInfo member)
		{
			if (this.InReferenceContext(member.DeclaringType))
			{
				return member;
			}
			return this.JScriptReferenceModule.ResolveMember(member.MetadataToken);
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0005DCCC File Offset: 0x0005CCCC
		internal bool InReferenceContext(Type type)
		{
			if (type == null)
			{
				return true;
			}
			Assembly assembly = type.Assembly;
			return assembly.ReflectionOnly || assembly != typeof(TypeReferences).Assembly || !this.JScriptReferenceModule.Assembly.ReflectionOnly;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0005DD14 File Offset: 0x0005CD14
		internal bool InReferenceContext(MemberInfo member)
		{
			if (member == null)
			{
				return true;
			}
			if (member is JSMethod)
			{
				member = ((JSMethod)member).GetMethodInfo(null);
			}
			else if (member is JSMethodInfo)
			{
				member = ((JSMethodInfo)member).method;
			}
			return this.InReferenceContext(member.DeclaringType);
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0005DD54 File Offset: 0x0005CD54
		internal bool InReferenceContext(IReflect ireflect)
		{
			return ireflect == null || !(ireflect is Type) || this.InReferenceContext((Type)ireflect);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0005DD71 File Offset: 0x0005CD71
		internal static Type ToExecutionContext(Type type)
		{
			if (TypeReferences.InExecutionContext(type))
			{
				return type;
			}
			return typeof(TypeReferences).Module.ResolveType(type.MetadataToken, null, null);
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0005DD99 File Offset: 0x0005CD99
		internal static IReflect ToExecutionContext(IReflect ireflect)
		{
			if (ireflect is Type)
			{
				return TypeReferences.ToExecutionContext((Type)ireflect);
			}
			return ireflect;
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0005DDB0 File Offset: 0x0005CDB0
		internal static MethodInfo ToExecutionContext(MethodInfo method)
		{
			if (method is JSMethod)
			{
				method = ((JSMethod)method).GetMethodInfo(null);
			}
			else if (method is JSMethodInfo)
			{
				method = ((JSMethodInfo)method).method;
			}
			return (MethodInfo)TypeReferences.MapMemberInfoToExecutionContext(method);
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0005DDEA File Offset: 0x0005CDEA
		internal static PropertyInfo ToExecutionContext(PropertyInfo property)
		{
			return (PropertyInfo)TypeReferences.MapMemberInfoToExecutionContext(property);
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0005DDF7 File Offset: 0x0005CDF7
		internal static FieldInfo ToExecutionContext(FieldInfo field)
		{
			return (FieldInfo)TypeReferences.MapMemberInfoToExecutionContext(field);
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0005DE04 File Offset: 0x0005CE04
		internal static ConstructorInfo ToExecutionContext(ConstructorInfo constructor)
		{
			return (ConstructorInfo)TypeReferences.MapMemberInfoToExecutionContext(constructor);
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0005DE11 File Offset: 0x0005CE11
		private static MemberInfo MapMemberInfoToExecutionContext(MemberInfo member)
		{
			if (TypeReferences.InExecutionContext(member.DeclaringType))
			{
				return member;
			}
			return typeof(TypeReferences).Module.ResolveMember(member.MetadataToken);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0005DE3C File Offset: 0x0005CE3C
		internal static bool InExecutionContext(Type type)
		{
			if (type == null)
			{
				return true;
			}
			Assembly assembly = type.Assembly;
			return !assembly.ReflectionOnly || assembly.Location != typeof(TypeReferences).Assembly.Location;
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0005DE80 File Offset: 0x0005CE80
		internal static object GetDefaultParameterValue(ParameterInfo parameter)
		{
			if (parameter.GetType().Assembly == typeof(TypeReferences).Assembly || !parameter.Member.DeclaringType.Assembly.ReflectionOnly)
			{
				return parameter.DefaultValue;
			}
			return parameter.RawDefaultValue;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0005DED0 File Offset: 0x0005CED0
		internal static object GetConstantValue(FieldInfo field)
		{
			if (field.GetType().Assembly == typeof(TypeReferences).Assembly || !field.DeclaringType.Assembly.ReflectionOnly)
			{
				return field.GetValue(null);
			}
			Type fieldType = field.FieldType;
			object rawConstantValue = field.GetRawConstantValue();
			if (fieldType.IsEnum)
			{
				return MetadataEnumValue.GetEnumValue(fieldType, rawConstantValue);
			}
			return rawConstantValue;
		}

		// Token: 0x04000726 RID: 1830
		private const int TypeReferenceStartOfSpecialCases = 81;

		// Token: 0x04000727 RID: 1831
		private const int TypeReferenceArrayLength = 83;

		// Token: 0x04000728 RID: 1832
		private static readonly SimpleHashtable _predefinedTypeTable = new SimpleHashtable(34U);

		// Token: 0x04000729 RID: 1833
		private Type[] _typeTable;

		// Token: 0x0400072A RID: 1834
		private Module _jscriptReferenceModule;

		// Token: 0x0200012A RID: 298
		private enum TypeReference
		{
			// Token: 0x0400072C RID: 1836
			ArgumentsObject,
			// Token: 0x0400072D RID: 1837
			ArrayConstructor,
			// Token: 0x0400072E RID: 1838
			ArrayObject,
			// Token: 0x0400072F RID: 1839
			ArrayWrapper,
			// Token: 0x04000730 RID: 1840
			Binding,
			// Token: 0x04000731 RID: 1841
			BitwiseBinary,
			// Token: 0x04000732 RID: 1842
			BooleanObject,
			// Token: 0x04000733 RID: 1843
			BreakOutOfFinally,
			// Token: 0x04000734 RID: 1844
			BuiltinFunction,
			// Token: 0x04000735 RID: 1845
			ClassScope,
			// Token: 0x04000736 RID: 1846
			Closure,
			// Token: 0x04000737 RID: 1847
			ContinueOutOfFinally,
			// Token: 0x04000738 RID: 1848
			Convert,
			// Token: 0x04000739 RID: 1849
			DateObject,
			// Token: 0x0400073A RID: 1850
			Empty,
			// Token: 0x0400073B RID: 1851
			EnumeratorObject,
			// Token: 0x0400073C RID: 1852
			Equality,
			// Token: 0x0400073D RID: 1853
			ErrorObject,
			// Token: 0x0400073E RID: 1854
			Eval,
			// Token: 0x0400073F RID: 1855
			EvalErrorObject,
			// Token: 0x04000740 RID: 1856
			Expando,
			// Token: 0x04000741 RID: 1857
			FieldAccessor,
			// Token: 0x04000742 RID: 1858
			ForIn,
			// Token: 0x04000743 RID: 1859
			FunctionDeclaration,
			// Token: 0x04000744 RID: 1860
			FunctionExpression,
			// Token: 0x04000745 RID: 1861
			FunctionObject,
			// Token: 0x04000746 RID: 1862
			FunctionWrapper,
			// Token: 0x04000747 RID: 1863
			GlobalObject,
			// Token: 0x04000748 RID: 1864
			GlobalScope,
			// Token: 0x04000749 RID: 1865
			Globals,
			// Token: 0x0400074A RID: 1866
			Hide,
			// Token: 0x0400074B RID: 1867
			IActivationObject,
			// Token: 0x0400074C RID: 1868
			INeedEngine,
			// Token: 0x0400074D RID: 1869
			Import,
			// Token: 0x0400074E RID: 1870
			In,
			// Token: 0x0400074F RID: 1871
			Instanceof,
			// Token: 0x04000750 RID: 1872
			JSError,
			// Token: 0x04000751 RID: 1873
			JSFunctionAttribute,
			// Token: 0x04000752 RID: 1874
			JSFunctionAttributeEnum,
			// Token: 0x04000753 RID: 1875
			JSLocalField,
			// Token: 0x04000754 RID: 1876
			JSObject,
			// Token: 0x04000755 RID: 1877
			JScriptException,
			// Token: 0x04000756 RID: 1878
			LateBinding,
			// Token: 0x04000757 RID: 1879
			LenientGlobalObject,
			// Token: 0x04000758 RID: 1880
			MathObject,
			// Token: 0x04000759 RID: 1881
			MethodInvoker,
			// Token: 0x0400075A RID: 1882
			Missing,
			// Token: 0x0400075B RID: 1883
			Namespace,
			// Token: 0x0400075C RID: 1884
			NotRecommended,
			// Token: 0x0400075D RID: 1885
			NumberObject,
			// Token: 0x0400075E RID: 1886
			NumericBinary,
			// Token: 0x0400075F RID: 1887
			NumericUnary,
			// Token: 0x04000760 RID: 1888
			ObjectConstructor,
			// Token: 0x04000761 RID: 1889
			Override,
			// Token: 0x04000762 RID: 1890
			Package,
			// Token: 0x04000763 RID: 1891
			Plus,
			// Token: 0x04000764 RID: 1892
			PostOrPrefixOperator,
			// Token: 0x04000765 RID: 1893
			RangeErrorObject,
			// Token: 0x04000766 RID: 1894
			ReferenceAttribute,
			// Token: 0x04000767 RID: 1895
			ReferenceErrorObject,
			// Token: 0x04000768 RID: 1896
			RegExpConstructor,
			// Token: 0x04000769 RID: 1897
			RegExpObject,
			// Token: 0x0400076A RID: 1898
			Relational,
			// Token: 0x0400076B RID: 1899
			ReturnOutOfFinally,
			// Token: 0x0400076C RID: 1900
			Runtime,
			// Token: 0x0400076D RID: 1901
			ScriptFunction,
			// Token: 0x0400076E RID: 1902
			ScriptObject,
			// Token: 0x0400076F RID: 1903
			ScriptStream,
			// Token: 0x04000770 RID: 1904
			SimpleHashtable,
			// Token: 0x04000771 RID: 1905
			StackFrame,
			// Token: 0x04000772 RID: 1906
			StrictEquality,
			// Token: 0x04000773 RID: 1907
			StringObject,
			// Token: 0x04000774 RID: 1908
			SyntaxErrorObject,
			// Token: 0x04000775 RID: 1909
			Throw,
			// Token: 0x04000776 RID: 1910
			Try,
			// Token: 0x04000777 RID: 1911
			TypedArray,
			// Token: 0x04000778 RID: 1912
			TypeErrorObject,
			// Token: 0x04000779 RID: 1913
			Typeof,
			// Token: 0x0400077A RID: 1914
			URIErrorObject,
			// Token: 0x0400077B RID: 1915
			VBArrayObject,
			// Token: 0x0400077C RID: 1916
			With,
			// Token: 0x0400077D RID: 1917
			BaseVsaStartup,
			// Token: 0x0400077E RID: 1918
			VsaEngine
		}
	}
}

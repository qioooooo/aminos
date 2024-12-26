using System;
using System.Globalization;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000A4 RID: 164
	[Serializable]
	public class JScriptException : ApplicationException, IVsaFullErrorInfo, IVsaError
	{
		// Token: 0x06000798 RID: 1944 RVA: 0x0003475E File Offset: 0x0003375E
		public JScriptException()
			: this(JSError.NoError)
		{
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00034767 File Offset: 0x00033767
		public JScriptException(string m)
			: this(m, null)
		{
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00034771 File Offset: 0x00033771
		public JScriptException(string m, Exception e)
			: this(m, e, null)
		{
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0003477C File Offset: 0x0003377C
		public JScriptException(JSError errorNumber)
			: this(errorNumber, null)
		{
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00034788 File Offset: 0x00033788
		internal JScriptException(JSError errorNumber, Context context)
		{
			this.value = Missing.Value;
			this.context = context;
			this.code = (base.HResult = (int)((ulong)(-2146828288) + (ulong)((long)errorNumber)));
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x000347C8 File Offset: 0x000337C8
		internal JScriptException(object value, Context context)
		{
			this.value = value;
			this.context = context;
			this.code = (base.HResult = -2146823266);
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x000347FD File Offset: 0x000337FD
		internal JScriptException(Exception e, Context context)
			: this(null, e, context)
		{
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00034808 File Offset: 0x00033808
		internal JScriptException(string m, Exception e, Context context)
			: base(m, e)
		{
			this.value = e;
			this.context = context;
			if (e is StackOverflowException)
			{
				this.code = (base.HResult = -2146828260);
				this.value = Missing.Value;
				return;
			}
			if (e is OutOfMemoryException)
			{
				this.code = (base.HResult = -2146828281);
				this.value = Missing.Value;
				return;
			}
			if (e is ExternalException)
			{
				this.code = (base.HResult = ((ExternalException)e).ErrorCode);
				if (((long)base.HResult & (long)((ulong)(-65536))) == (long)((ulong)(-2146828288)) && Enum.IsDefined(typeof(JSError), base.HResult & 65535))
				{
					this.value = Missing.Value;
					return;
				}
			}
			else
			{
				int hrforException = Marshal.GetHRForException(e);
				if (((long)hrforException & (long)((ulong)(-65536))) == (long)((ulong)(-2146828288)) && Enum.IsDefined(typeof(JSError), hrforException & 65535))
				{
					this.code = (base.HResult = hrforException);
					this.value = Missing.Value;
					return;
				}
				this.code = (base.HResult = -2146823266);
			}
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00034950 File Offset: 0x00033950
		protected JScriptException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.code = (base.HResult = info.GetInt32("Code"));
			this.value = Missing.Value;
			this.isError = info.GetBoolean("IsError");
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x0003499B File Offset: 0x0003399B
		public string SourceMoniker
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				if (this.context != null)
				{
					return this.context.document.documentName;
				}
				return "no source";
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x000349BB File Offset: 0x000339BB
		public int StartColumn
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				return this.Column;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x000349C3 File Offset: 0x000339C3
		public int Column
		{
			get
			{
				if (this.context != null)
				{
					return this.context.StartColumn + this.context.document.startCol + 1;
				}
				return 0;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x000349ED File Offset: 0x000339ED
		string IVsaError.Description
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				return this.Description;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x000349F5 File Offset: 0x000339F5
		public string Description
		{
			get
			{
				return this.Message;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x000349FD File Offset: 0x000339FD
		public int EndLine
		{
			get
			{
				if (this.context != null)
				{
					return this.context.EndLine + this.context.document.startLine - this.context.document.lastLineInSource;
				}
				return 0;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x00034A36 File Offset: 0x00033A36
		public int EndColumn
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				if (this.context != null)
				{
					return this.context.EndColumn + this.context.document.startCol + 1;
				}
				return 0;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00034A60 File Offset: 0x00033A60
		int IVsaError.Number
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				return this.Number;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00034A68 File Offset: 0x00033A68
		public int Number
		{
			get
			{
				return this.ErrorNumber;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x00034A70 File Offset: 0x00033A70
		public int ErrorNumber
		{
			get
			{
				return base.HResult;
			}
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00034A78 File Offset: 0x00033A78
		internal ErrorType GetErrorType()
		{
			int hresult = base.HResult;
			if (((long)hresult & (long)((ulong)(-65536))) != (long)((ulong)(-2146828288)))
			{
				return ErrorType.OtherError;
			}
			JSError jserror = (JSError)(hresult & 65535);
			if (jserror <= JSError.InvalidElse)
			{
				if (jserror <= JSError.TypeMismatch)
				{
					if (jserror == JSError.InvalidCall)
					{
						return ErrorType.TypeError;
					}
					if (jserror == JSError.TypeMismatch)
					{
						return ErrorType.TypeError;
					}
				}
				else
				{
					if (jserror == JSError.OLENoPropOrMethod)
					{
						return ErrorType.TypeError;
					}
					if (jserror == JSError.NotCollection)
					{
						return ErrorType.TypeError;
					}
					switch (jserror)
					{
					case JSError.SyntaxError:
						return ErrorType.SyntaxError;
					case JSError.NoColon:
						return ErrorType.SyntaxError;
					case JSError.NoSemicolon:
						return ErrorType.SyntaxError;
					case JSError.NoLeftParen:
						return ErrorType.SyntaxError;
					case JSError.NoRightParen:
						return ErrorType.SyntaxError;
					case JSError.NoRightBracket:
						return ErrorType.SyntaxError;
					case JSError.NoLeftCurly:
						return ErrorType.SyntaxError;
					case JSError.NoRightCurly:
						return ErrorType.SyntaxError;
					case JSError.NoIdentifier:
						return ErrorType.SyntaxError;
					case JSError.NoEqual:
						return ErrorType.SyntaxError;
					case JSError.IllegalChar:
						return ErrorType.SyntaxError;
					case JSError.UnterminatedString:
						return ErrorType.SyntaxError;
					case JSError.NoCommentEnd:
						return ErrorType.SyntaxError;
					case JSError.BadReturn:
						return ErrorType.SyntaxError;
					case JSError.BadBreak:
						return ErrorType.SyntaxError;
					case JSError.BadContinue:
						return ErrorType.SyntaxError;
					case JSError.BadHexDigit:
						return ErrorType.SyntaxError;
					case JSError.NoWhile:
						return ErrorType.SyntaxError;
					case JSError.BadLabel:
						return ErrorType.SyntaxError;
					case JSError.NoLabel:
						return ErrorType.SyntaxError;
					case JSError.DupDefault:
						return ErrorType.SyntaxError;
					case JSError.NoMemberIdentifier:
						return ErrorType.SyntaxError;
					case JSError.NoCcEnd:
						return ErrorType.SyntaxError;
					case JSError.CcOff:
						return ErrorType.SyntaxError;
					case JSError.NotConst:
						return ErrorType.SyntaxError;
					case JSError.NoAt:
						return ErrorType.SyntaxError;
					case JSError.NoCatch:
						return ErrorType.SyntaxError;
					case JSError.InvalidElse:
						return ErrorType.SyntaxError;
					}
				}
			}
			else if (jserror <= JSError.PackageExpected)
			{
				switch (jserror)
				{
				case JSError.NoComma:
					return ErrorType.SyntaxError;
				case JSError.DupVisibility:
				case JSError.IllegalVisibility:
				case JSError.IncompatibleVisibility:
				case JSError.DuplicateName:
				case (JSError)1116:
				case (JSError)1117:
				case JSError.Deprecated:
				case JSError.IllegalUseOfThis:
				case JSError.CannotUseNameOfClass:
				case (JSError)1125:
				case (JSError)1126:
				case (JSError)1127:
				case JSError.MustImplementMethod:
				case (JSError)1130:
				case (JSError)1131:
				case (JSError)1132:
				case JSError.VariableLeftUninitialized:
				case (JSError)1138:
				case (JSError)1139:
				case JSError.NotAllowedInSuperConstructorCall:
				case JSError.NotMeantToBeCalledDirectly:
				case JSError.GetAndSetAreInconsistent:
				case (JSError)1145:
					break;
				case JSError.BadSwitch:
					return ErrorType.SyntaxError;
				case JSError.CcInvalidEnd:
					return ErrorType.SyntaxError;
				case JSError.CcInvalidElse:
					return ErrorType.SyntaxError;
				case JSError.CcInvalidElif:
					return ErrorType.SyntaxError;
				case JSError.ErrEOF:
					return ErrorType.SyntaxError;
				case JSError.ClassNotAllowed:
					return ErrorType.SyntaxError;
				case JSError.NeedCompileTimeConstant:
					return ErrorType.ReferenceError;
				case JSError.NeedType:
					return ErrorType.TypeError;
				case JSError.NotInsideClass:
					return ErrorType.SyntaxError;
				case JSError.InvalidPositionDirective:
					return ErrorType.SyntaxError;
				case JSError.MustBeEOL:
					return ErrorType.SyntaxError;
				case JSError.WrongDirective:
					return ErrorType.SyntaxError;
				case JSError.CannotNestPositionDirective:
					return ErrorType.SyntaxError;
				case JSError.CircularDefinition:
					return ErrorType.SyntaxError;
				case JSError.NotAccessible:
					return ErrorType.ReferenceError;
				case JSError.NeedInterface:
					return ErrorType.TypeError;
				case JSError.UnreachableCatch:
					return ErrorType.SyntaxError;
				case JSError.TypeCannotBeExtended:
					return ErrorType.ReferenceError;
				case JSError.UndeclaredVariable:
					return ErrorType.ReferenceError;
				case JSError.KeywordUsedAsIdentifier:
					return ErrorType.SyntaxError;
				case JSError.InvalidCustomAttribute:
					return ErrorType.TypeError;
				case JSError.InvalidCustomAttributeArgument:
					return ErrorType.TypeError;
				case JSError.InvalidCustomAttributeClassOrCtor:
					return ErrorType.TypeError;
				default:
					switch (jserror)
					{
					case JSError.NoSuchMember:
						return ErrorType.ReferenceError;
					case JSError.ItemNotAllowedOnExpandoClass:
						return ErrorType.SyntaxError;
					case JSError.MethodNotAllowedOnExpandoClass:
					case (JSError)1154:
					case JSError.MethodClashOnExpandoSuperClass:
					case JSError.BaseClassIsExpandoAlready:
					case JSError.AbstractCannotBePrivate:
						break;
					case JSError.NotIndexable:
						return ErrorType.TypeError;
					case JSError.StaticMissingInStaticInit:
						return ErrorType.SyntaxError;
					case JSError.MissingConstructForAttributes:
						return ErrorType.SyntaxError;
					case JSError.OnlyClassesAllowed:
						return ErrorType.SyntaxError;
					default:
						if (jserror == JSError.PackageExpected)
						{
							return ErrorType.SyntaxError;
						}
						break;
					}
					break;
				}
			}
			else
			{
				switch (jserror)
				{
				case JSError.DifferentReturnTypeFromBase:
					return ErrorType.TypeError;
				case JSError.ClashWithProperty:
					return ErrorType.SyntaxError;
				case JSError.OverrideAndHideUsedTogether:
				case JSError.InvalidLanguageOption:
				case JSError.NoMethodInBaseToOverride:
				case JSError.NotValidForConstructor:
				case JSError.OctalLiteralsAreDeprecated:
				case JSError.VariableMightBeUnitialized:
				case JSError.NotOKToCallSuper:
				case JSError.IllegalUseOfSuper:
				case JSError.BadWayToLeaveFinally:
				case (JSError)1201:
				case (JSError)1202:
				case JSError.UselessAssignment:
				case JSError.SuspectAssignment:
				case JSError.SuspectSemicolon:
				case JSError.FinalPrecludesAbstract:
				case (JSError)1211:
				case JSError.CannotBeAbstract:
				case JSError.ArrayMayBeCopied:
				case JSError.AbstractCannotBeStatic:
				case JSError.StaticIsAlreadyFinal:
				case JSError.StaticMethodsCannotOverride:
				case JSError.StaticMethodsCannotHide:
				case JSError.ExpandoPrecludesOverride:
				case JSError.IllegalParamArrayAttribute:
				case JSError.ExpandoPrecludesAbstract:
				case (JSError)1225:
				case JSError.InvalidImport:
				case JSError.InvalidCustomAttributeTarget:
				case JSError.CustomAttributeUsedMoreThanOnce:
				case JSError.BadThrow:
				case JSError.NoSuchType:
				case JSError.BadOctalLiteral:
				case JSError.SuspectLoopCondition:
				case JSError.ExpandoPrecludesStatic:
				case JSError.NotValidVersionString:
				case JSError.ExecutablesCannotBeLocalized:
				case JSError.StringConcatIsSlow:
					break;
				case JSError.CannotReturnValueFromVoidFunction:
					return ErrorType.TypeError;
				case JSError.AmbiguousMatch:
					return ErrorType.ReferenceError;
				case JSError.AmbiguousConstructorCall:
					return ErrorType.ReferenceError;
				case JSError.SuperClassConstructorNotAccessible:
					return ErrorType.ReferenceError;
				case JSError.NoCommaOrTypeDefinitionError:
					return ErrorType.SyntaxError;
				case JSError.AbstractWithBody:
					return ErrorType.SyntaxError;
				case JSError.NoRightParenOrComma:
					return ErrorType.SyntaxError;
				case JSError.NoRightBracketOrComma:
					return ErrorType.SyntaxError;
				case JSError.ExpressionExpected:
					return ErrorType.SyntaxError;
				case JSError.UnexpectedSemicolon:
					return ErrorType.SyntaxError;
				case JSError.TooManyTokensSkipped:
					return ErrorType.SyntaxError;
				case JSError.BadVariableDeclaration:
					return ErrorType.SyntaxError;
				case JSError.BadFunctionDeclaration:
					return ErrorType.SyntaxError;
				case JSError.BadPropertyDeclaration:
					return ErrorType.SyntaxError;
				case JSError.DoesNotHaveAnAddress:
					return ErrorType.ReferenceError;
				case JSError.TooFewParameters:
					return ErrorType.TypeError;
				case JSError.ImpossibleConversion:
					return ErrorType.TypeError;
				case JSError.NeedInstance:
					return ErrorType.ReferenceError;
				case JSError.InvalidBaseTypeForEnum:
					return ErrorType.TypeError;
				case JSError.CannotInstantiateAbstractClass:
					return ErrorType.TypeError;
				case JSError.ShouldBeAbstract:
					return ErrorType.SyntaxError;
				case JSError.BadModifierInInterface:
					return ErrorType.SyntaxError;
				case JSError.VarIllegalInInterface:
					return ErrorType.SyntaxError;
				case JSError.InterfaceIllegalInInterface:
					return ErrorType.SyntaxError;
				case JSError.NoVarInEnum:
					return ErrorType.SyntaxError;
				case JSError.EnumNotAllowed:
					return ErrorType.SyntaxError;
				case JSError.PackageInWrongContext:
					return ErrorType.SyntaxError;
				case JSError.ConstructorMayNotHaveReturnType:
					return ErrorType.SyntaxError;
				case JSError.OnlyClassesAndPackagesAllowed:
					return ErrorType.SyntaxError;
				case JSError.InvalidDebugDirective:
					return ErrorType.SyntaxError;
				case JSError.NestedInstanceTypeCannotBeExtendedByStatic:
					return ErrorType.ReferenceError;
				case JSError.PropertyLevelAttributesMustBeOnGetter:
					return ErrorType.ReferenceError;
				case JSError.ParamListNotLast:
					return ErrorType.SyntaxError;
				case JSError.InstanceNotAccessibleFromStatic:
					return ErrorType.ReferenceError;
				case JSError.StaticRequiresTypeName:
					return ErrorType.ReferenceError;
				case JSError.NonStaticWithTypeName:
					return ErrorType.ReferenceError;
				case JSError.NoSuchStaticMember:
					return ErrorType.ReferenceError;
				case JSError.ExpectedAssembly:
					return ErrorType.SyntaxError;
				case JSError.AssemblyAttributesMustBeGlobal:
					return ErrorType.SyntaxError;
				case JSError.DuplicateMethod:
					return ErrorType.TypeError;
				case JSError.NotAnExpandoFunction:
					return ErrorType.ReferenceError;
				case JSError.CcInvalidInDebugger:
					return ErrorType.SyntaxError;
				default:
					switch (jserror)
					{
					case JSError.TypeNameTooLong:
						return ErrorType.SyntaxError;
					case JSError.MemberInitializerCannotContainFuncExpr:
						return ErrorType.SyntaxError;
					default:
						switch (jserror)
						{
						case JSError.CantAssignThis:
							return ErrorType.ReferenceError;
						case JSError.NumberExpected:
							return ErrorType.TypeError;
						case JSError.FunctionExpected:
							return ErrorType.TypeError;
						case JSError.StringExpected:
							return ErrorType.TypeError;
						case JSError.DateExpected:
							return ErrorType.TypeError;
						case JSError.ObjectExpected:
							return ErrorType.TypeError;
						case JSError.IllegalAssignment:
							return ErrorType.ReferenceError;
						case JSError.UndefinedIdentifier:
							return ErrorType.ReferenceError;
						case JSError.BooleanExpected:
							return ErrorType.TypeError;
						case JSError.VBArrayExpected:
							return ErrorType.TypeError;
						case JSError.EnumeratorExpected:
							return ErrorType.TypeError;
						case JSError.RegExpExpected:
							return ErrorType.TypeError;
						case JSError.RegExpSyntax:
							return ErrorType.SyntaxError;
						case JSError.InvalidPrototype:
							return ErrorType.TypeError;
						case JSError.URIEncodeError:
							return ErrorType.URIError;
						case JSError.URIDecodeError:
							return ErrorType.URIError;
						case JSError.FractionOutOfRange:
							return ErrorType.RangeError;
						case JSError.PrecisionOutOfRange:
							return ErrorType.RangeError;
						case JSError.ArrayLengthConstructIncorrect:
							return ErrorType.RangeError;
						case JSError.ArrayLengthAssignIncorrect:
							return ErrorType.RangeError;
						case JSError.NeedArrayObject:
							return ErrorType.TypeError;
						case JSError.NoConstructor:
							return ErrorType.TypeError;
						case JSError.IllegalEval:
							return ErrorType.EvalError;
						case JSError.MustProvideNameForNamedParameter:
							return ErrorType.ReferenceError;
						case JSError.DuplicateNamedParameter:
							return ErrorType.ReferenceError;
						case JSError.MissingNameParameter:
							return ErrorType.ReferenceError;
						case JSError.MoreNamedParametersThanArguments:
							return ErrorType.ReferenceError;
						case JSError.AssignmentToReadOnly:
							return ErrorType.ReferenceError;
						case JSError.WriteOnlyProperty:
							return ErrorType.ReferenceError;
						case JSError.IncorrectNumberOfIndices:
							return ErrorType.ReferenceError;
						}
						break;
					}
					break;
				}
			}
			return ErrorType.OtherError;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00034FC9 File Offset: 0x00033FC9
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("IsError", this.isError);
			info.AddValue("Code", this.code);
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x00035003 File Offset: 0x00034003
		public int Line
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				if (this.context != null)
				{
					return this.context.StartLine + this.context.document.startLine - this.context.document.lastLineInSource;
				}
				return 0;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x0003503C File Offset: 0x0003403C
		public string LineText
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				if (this.context != null)
				{
					return this.context.source_string;
				}
				return "";
			}
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00035057 File Offset: 0x00034057
		internal static string Localize(string key, CultureInfo culture)
		{
			return JScriptException.Localize(key, null, culture);
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00035064 File Offset: 0x00034064
		internal static string Localize(string key, string context, CultureInfo culture)
		{
			try
			{
				ResourceManager resourceManager = new ResourceManager("Microsoft.JScript", typeof(JScriptException).Module.Assembly);
				string @string = resourceManager.GetString(key, culture);
				if (@string != null)
				{
					int num = @string.IndexOf(JScriptException.ContextStringDelimiter);
					if (num == -1)
					{
						return @string;
					}
					if (context == null)
					{
						return @string.Substring(0, num);
					}
					return string.Format(culture, @string.Substring(num + 2), new object[] { context });
				}
			}
			catch (MissingManifestResourceException)
			{
			}
			return key;
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x000350F8 File Offset: 0x000340F8
		public override string Message
		{
			get
			{
				if (this.value is Exception)
				{
					Exception ex = (Exception)this.value;
					string message = ex.Message;
					if (message != null && message.Length > 0)
					{
						return message;
					}
					return ex.ToString();
				}
				else
				{
					string text = (base.HResult & 65535).ToString(CultureInfo.InvariantCulture);
					CultureInfo cultureInfo = null;
					if (this.context != null && this.context.document != null)
					{
						VsaEngine engine = this.context.document.engine;
						if (engine != null)
						{
							cultureInfo = engine.ErrorCultureInfo;
						}
					}
					if (this.value is ErrorObject)
					{
						string message2 = ((ErrorObject)this.value).Message;
						if (message2 != null && message2.Length > 0)
						{
							return message2;
						}
						return JScriptException.Localize("No description available", cultureInfo) + ": " + text;
					}
					else
					{
						if (this.value is string)
						{
							JSError jserror = (JSError)(base.HResult & 65535);
							if (jserror <= JSError.HidesAbstractInBase)
							{
								if (jserror <= JSError.MustImplementMethod)
								{
									if (jserror <= JSError.DuplicateName)
									{
										if (jserror != JSError.TypeMismatch && jserror != JSError.DuplicateName)
										{
											goto IL_021D;
										}
									}
									else if (jserror != JSError.Deprecated && jserror != JSError.MustImplementMethod)
									{
										goto IL_021D;
									}
								}
								else if (jserror <= JSError.NoSuchMember)
								{
									if (jserror != JSError.TypeCannotBeExtended && jserror != JSError.NoSuchMember)
									{
										goto IL_021D;
									}
								}
								else if (jserror != JSError.NotIndexable)
								{
									switch (jserror)
									{
									case JSError.HidesParentMember:
									case JSError.HidesAbstractInBase:
										break;
									case JSError.CannotChangeVisibility:
										goto IL_021D;
									default:
										goto IL_021D;
									}
								}
							}
							else if (jserror <= JSError.NoSuchType)
							{
								if (jserror <= JSError.CannotBeAbstract)
								{
									if (jserror != JSError.DifferentReturnTypeFromBase && jserror != JSError.CannotBeAbstract)
									{
										goto IL_021D;
									}
								}
								else if (jserror != JSError.InvalidCustomAttributeTarget && jserror != JSError.NoSuchType)
								{
									goto IL_021D;
								}
							}
							else if (jserror <= JSError.ImplicitlyReferencedAssemblyNotFound)
							{
								if (jserror != JSError.NoSuchStaticMember && jserror != JSError.ImplicitlyReferencedAssemblyNotFound)
								{
									goto IL_021D;
								}
							}
							else if (jserror != JSError.InvalidResource)
							{
								switch (jserror)
								{
								case JSError.IncompatibleAssemblyReference:
								case JSError.InvalidAssemblyKeyFile:
								case JSError.TypeNameTooLong:
									break;
								default:
									goto IL_021D;
								}
							}
							return JScriptException.Localize(text, (string)this.value, cultureInfo);
							IL_021D:
							return (string)this.value;
						}
						if (this.context != null)
						{
							JSError jserror2 = (JSError)(base.HResult & 65535);
							if (jserror2 <= JSError.NotDeletable)
							{
								if (jserror2 <= JSError.NotAccessible)
								{
									if (jserror2 != JSError.DuplicateName && jserror2 != JSError.NotAccessible)
									{
										goto IL_0325;
									}
								}
								else
								{
									switch (jserror2)
									{
									case JSError.UndeclaredVariable:
									case JSError.VariableLeftUninitialized:
									case JSError.KeywordUsedAsIdentifier:
									case JSError.NotMeantToBeCalledDirectly:
										break;
									case (JSError)1138:
									case (JSError)1139:
									case JSError.NotAllowedInSuperConstructorCall:
										goto IL_0325;
									default:
										switch (jserror2)
										{
										case JSError.AmbiguousBindingBecauseOfWith:
										case JSError.AmbiguousBindingBecauseOfEval:
											break;
										default:
											if (jserror2 != JSError.NotDeletable)
											{
												goto IL_0325;
											}
											break;
										}
										break;
									}
								}
							}
							else if (jserror2 <= JSError.NeedInstance)
							{
								if (jserror2 != JSError.VariableMightBeUnitialized && jserror2 != JSError.NeedInstance)
								{
									goto IL_0325;
								}
							}
							else
							{
								switch (jserror2)
								{
								case JSError.InstanceNotAccessibleFromStatic:
								case JSError.StaticRequiresTypeName:
								case JSError.NonStaticWithTypeName:
									break;
								default:
									switch (jserror2)
									{
									case JSError.ObjectExpected:
									case JSError.UndefinedIdentifier:
										break;
									case JSError.IllegalAssignment:
										goto IL_0325;
									default:
										if (jserror2 != JSError.AssignmentToReadOnly)
										{
											goto IL_0325;
										}
										break;
									}
									break;
								}
							}
							return JScriptException.Localize(text, this.context.GetCode(), cultureInfo);
						}
						IL_0325:
						return JScriptException.Localize((base.HResult & 65535).ToString(CultureInfo.InvariantCulture), cultureInfo);
					}
				}
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0003544C File Offset: 0x0003444C
		public int Severity
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				int hresult = base.HResult;
				if (((long)hresult & (long)((ulong)(-65536))) != (long)((ulong)(-2146828288)))
				{
					return 0;
				}
				if (!this.isError)
				{
					JSError jserror = (JSError)(hresult & 65535);
					if (jserror <= JSError.DifferentReturnTypeFromBase)
					{
						if (jserror <= JSError.GetAndSetAreInconsistent)
						{
							if (jserror <= JSError.IncompatibleVisibility)
							{
								if (jserror == JSError.DupVisibility)
								{
									return 1;
								}
								if (jserror == JSError.IncompatibleVisibility)
								{
									return 1;
								}
							}
							else
							{
								if (jserror == JSError.DuplicateName)
								{
									return 1;
								}
								if (jserror == JSError.Deprecated)
								{
									return 2;
								}
								switch (jserror)
								{
								case JSError.UndeclaredVariable:
									return 3;
								case JSError.VariableLeftUninitialized:
									return 3;
								case JSError.KeywordUsedAsIdentifier:
									return 2;
								case JSError.NotMeantToBeCalledDirectly:
									return 1;
								case JSError.GetAndSetAreInconsistent:
									return 1;
								}
							}
						}
						else if (jserror <= JSError.BaseClassIsExpandoAlready)
						{
							switch (jserror)
							{
							case JSError.TooManyParameters:
								return 1;
							case JSError.AmbiguousBindingBecauseOfWith:
								return 4;
							case JSError.AmbiguousBindingBecauseOfEval:
								return 4;
							default:
								if (jserror == JSError.BaseClassIsExpandoAlready)
								{
									return 1;
								}
								break;
							}
						}
						else
						{
							if (jserror == JSError.NotDeletable)
							{
								return 1;
							}
							switch (jserror)
							{
							case JSError.UselessExpression:
								return 1;
							case JSError.HidesParentMember:
								return 1;
							case JSError.CannotChangeVisibility:
							case JSError.HidesAbstractInBase:
								break;
							case JSError.NewNotSpecifiedInMethodDeclaration:
								return 1;
							default:
								if (jserror == JSError.DifferentReturnTypeFromBase)
								{
									return 1;
								}
								break;
							}
						}
					}
					else if (jserror <= JSError.BadOctalLiteral)
					{
						if (jserror <= JSError.SuspectSemicolon)
						{
							switch (jserror)
							{
							case JSError.OctalLiteralsAreDeprecated:
								return 2;
							case JSError.VariableMightBeUnitialized:
								return 3;
							case JSError.NotOKToCallSuper:
							case JSError.IllegalUseOfSuper:
								break;
							case JSError.BadWayToLeaveFinally:
								return 3;
							default:
								switch (jserror)
								{
								case JSError.TooFewParameters:
									return 1;
								case JSError.UselessAssignment:
									return 1;
								case JSError.SuspectAssignment:
									return 1;
								case JSError.SuspectSemicolon:
									return 1;
								}
								break;
							}
						}
						else
						{
							if (jserror == JSError.ArrayMayBeCopied)
							{
								return 1;
							}
							if (jserror == JSError.ShouldBeAbstract)
							{
								return 1;
							}
							if (jserror == JSError.BadOctalLiteral)
							{
								return 1;
							}
						}
					}
					else if (jserror <= JSError.StringConcatIsSlow)
					{
						if (jserror == JSError.SuspectLoopCondition)
						{
							return 1;
						}
						if (jserror == JSError.StringConcatIsSlow)
						{
							return 3;
						}
					}
					else
					{
						switch (jserror)
						{
						case JSError.PossibleBadConversion:
							return 1;
						case JSError.PossibleBadConversionFromString:
							return 4;
						default:
							if (jserror == JSError.IncompatibleAssemblyReference)
							{
								return 1;
							}
							if (jserror == JSError.AssignmentToReadOnly)
							{
								return 1;
							}
							break;
						}
					}
				}
				return 0;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00035661 File Offset: 0x00034661
		public IVsaItem SourceItem
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				if (this.context != null)
				{
					return this.context.document.sourceItem;
				}
				throw new NoContextException();
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00035684 File Offset: 0x00034684
		public override string StackTrace
		{
			get
			{
				if (this.context == null)
				{
					return this.Message + Environment.NewLine + base.StackTrace;
				}
				StringBuilder stringBuilder = new StringBuilder();
				Context context = this.context;
				string documentName = context.document.documentName;
				if (documentName != null && documentName.Length > 0)
				{
					stringBuilder.Append(documentName + ": ");
				}
				CultureInfo cultureInfo = null;
				if (this.context != null && this.context.document != null)
				{
					VsaEngine engine = this.context.document.engine;
					if (engine != null)
					{
						cultureInfo = engine.ErrorCultureInfo;
					}
				}
				stringBuilder.Append(JScriptException.Localize("Line", cultureInfo));
				stringBuilder.Append(' ');
				stringBuilder.Append(context.StartLine);
				stringBuilder.Append(" - ");
				stringBuilder.Append(JScriptException.Localize("Error", cultureInfo));
				stringBuilder.Append(": ");
				stringBuilder.Append(this.Message);
				stringBuilder.Append(Environment.NewLine);
				if (context.document.engine != null)
				{
					Stack callContextStack = context.document.engine.Globals.CallContextStack;
					int i = 0;
					int num = callContextStack.Size();
					while (i < num)
					{
						CallContext callContext = (CallContext)callContextStack.Peek(i);
						stringBuilder.Append("    ");
						stringBuilder.Append(JScriptException.Localize("at call to", cultureInfo));
						stringBuilder.Append(callContext.FunctionName());
						stringBuilder.Append(' ');
						stringBuilder.Append(JScriptException.Localize("in line", cultureInfo));
						stringBuilder.Append(": ");
						stringBuilder.Append(callContext.sourceContext.EndLine);
						i++;
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0400032D RID: 813
		internal object value;

		// Token: 0x0400032E RID: 814
		[NonSerialized]
		internal Context context;

		// Token: 0x0400032F RID: 815
		internal bool isError;

		// Token: 0x04000330 RID: 816
		internal static readonly string ContextStringDelimiter = ";;";

		// Token: 0x04000331 RID: 817
		private int code;
	}
}

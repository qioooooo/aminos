using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x0200003D RID: 61
	[Serializable]
	public class CmdLineException : Exception
	{
		// Token: 0x0600028A RID: 650 RVA: 0x00015292 File Offset: 0x00014292
		public CmdLineException(CmdLineError errorCode, CultureInfo culture)
		{
			this.culture = culture;
			this.errorCode = errorCode;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000152A8 File Offset: 0x000142A8
		public CmdLineException(CmdLineError errorCode, string context, CultureInfo culture)
		{
			this.culture = culture;
			this.errorCode = errorCode;
			if (context != "")
			{
				this.context = context;
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x000152D2 File Offset: 0x000142D2
		public CmdLineException()
		{
		}

		// Token: 0x0600028D RID: 653 RVA: 0x000152DA File Offset: 0x000142DA
		public CmdLineException(string m)
			: base(m)
		{
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000152E3 File Offset: 0x000142E3
		public CmdLineException(string m, Exception e)
			: base(m, e)
		{
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000152F0 File Offset: 0x000142F0
		protected CmdLineException(SerializationInfo s, StreamingContext c)
			: base(s, c)
		{
			this.errorCode = (CmdLineError)s.GetInt32("ErrorCode");
			this.context = s.GetString("Context");
			int @int = s.GetInt32("LCID");
			if (@int != 1024)
			{
				this.culture = new CultureInfo(@int);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00015348 File Offset: 0x00014348
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo s, StreamingContext c)
		{
			base.GetObjectData(s, c);
			s.AddValue("ErrorCode", (int)this.errorCode);
			s.AddValue("Context", this.context);
			int num = 1024;
			if (this.culture != null)
			{
				num = this.culture.LCID;
			}
			s.AddValue("LCID", num);
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000291 RID: 657 RVA: 0x000153A8 File Offset: 0x000143A8
		public override string Message
		{
			get
			{
				string text = this.ResourceKey(this.errorCode);
				string text2 = JScriptException.Localize(text, this.context, this.culture);
				string text3 = ((int)(10000 + this.errorCode)).ToString(CultureInfo.InvariantCulture).Substring(1);
				return "fatal error JS" + text3 + ": " + text2;
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00015408 File Offset: 0x00014408
		public string ResourceKey(CmdLineError errorCode)
		{
			switch (errorCode)
			{
			case CmdLineError.AssemblyNotFound:
				return "Assembly not found";
			case CmdLineError.CannotCreateEngine:
				return "Cannot create JScript engine";
			case CmdLineError.CompilerConstant:
				return "Compiler constant";
			case CmdLineError.DuplicateFileAsSourceAndAssembly:
				return "Duplicate file as source and assembly";
			case CmdLineError.DuplicateResourceFile:
				return "Duplicate resource file";
			case CmdLineError.DuplicateResourceName:
				return "Duplicate resource name";
			case CmdLineError.DuplicateSourceFile:
				return "Duplicate source file";
			case CmdLineError.ErrorSavingCompiledState:
				return "Error saving compiled state";
			case CmdLineError.InvalidAssembly:
				return "Invalid assembly";
			case CmdLineError.InvalidCodePage:
				return "Invalid code page";
			case CmdLineError.InvalidDefinition:
				return "Invalid definition";
			case CmdLineError.InvalidLocaleID:
				return "Invalid Locale ID";
			case CmdLineError.InvalidTarget:
				return "Invalid target";
			case CmdLineError.InvalidSourceFile:
				return "Invalid source file";
			case CmdLineError.InvalidWarningLevel:
				return "Invalid warning level";
			case CmdLineError.MultipleOutputNames:
				return "Multiple output filenames";
			case CmdLineError.MultipleTargets:
				return "Multiple targets";
			case CmdLineError.MissingDefineArgument:
				return "Missing define argument";
			case CmdLineError.MissingExtension:
				return "Missing extension";
			case CmdLineError.MissingLibArgument:
				return "Missing lib argument";
			case CmdLineError.ManagedResourceNotFound:
				return "Managed resource not found";
			case CmdLineError.NestedResponseFiles:
				return "Nested response files";
			case CmdLineError.NoCodePage:
				return "No code page";
			case CmdLineError.NoFileName:
				return "No filename";
			case CmdLineError.NoInputSourcesSpecified:
				return "No input sources specified";
			case CmdLineError.NoLocaleID:
				return "No Locale ID";
			case CmdLineError.NoWarningLevel:
				return "No warning level";
			case CmdLineError.ResourceNotFound:
				return "Resource not found";
			case CmdLineError.UnknownOption:
				return "Unknown option";
			case CmdLineError.InvalidVersion:
				return "Invalid version";
			case CmdLineError.SourceFileTooBig:
				return "Source file too big";
			case CmdLineError.MultipleWin32Resources:
				return "Multiple win32resources";
			case CmdLineError.MissingReference:
				return "Missing reference";
			case CmdLineError.SourceNotFound:
				return "Source not found";
			case CmdLineError.InvalidCharacters:
				return "Invalid characters";
			case CmdLineError.InvalidForCompilerOptions:
				return "Invalid for CompilerOptions";
			case CmdLineError.IncompatibleTargets:
				return "Incompatible targets";
			case CmdLineError.InvalidPlatform:
				return "Invalid platform";
			}
			return "No description available";
		}

		// Token: 0x040001B1 RID: 433
		private const int LOCALE_USER_DEFAULT = 1024;

		// Token: 0x040001B2 RID: 434
		private CmdLineError errorCode;

		// Token: 0x040001B3 RID: 435
		private string context;

		// Token: 0x040001B4 RID: 436
		private CultureInfo culture;
	}
}

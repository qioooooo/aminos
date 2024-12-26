using System;

namespace Microsoft.JScript
{
	// Token: 0x0200003C RID: 60
	public enum CmdLineError
	{
		// Token: 0x04000187 RID: 391
		NoError,
		// Token: 0x04000188 RID: 392
		AssemblyNotFound = 2001,
		// Token: 0x04000189 RID: 393
		CannotCreateEngine,
		// Token: 0x0400018A RID: 394
		CompilerConstant,
		// Token: 0x0400018B RID: 395
		DuplicateFileAsSourceAndAssembly,
		// Token: 0x0400018C RID: 396
		DuplicateResourceFile,
		// Token: 0x0400018D RID: 397
		DuplicateResourceName,
		// Token: 0x0400018E RID: 398
		DuplicateSourceFile,
		// Token: 0x0400018F RID: 399
		ErrorSavingCompiledState,
		// Token: 0x04000190 RID: 400
		IncompatibleTargets = 2038,
		// Token: 0x04000191 RID: 401
		InvalidAssembly = 2009,
		// Token: 0x04000192 RID: 402
		InvalidCharacters = 2036,
		// Token: 0x04000193 RID: 403
		InvalidCodePage = 2010,
		// Token: 0x04000194 RID: 404
		InvalidDefinition,
		// Token: 0x04000195 RID: 405
		InvalidForCompilerOptions = 2037,
		// Token: 0x04000196 RID: 406
		InvalidLocaleID = 2012,
		// Token: 0x04000197 RID: 407
		InvalidPlatform = 2039,
		// Token: 0x04000198 RID: 408
		InvalidTarget = 2013,
		// Token: 0x04000199 RID: 409
		InvalidSourceFile,
		// Token: 0x0400019A RID: 410
		InvalidVersion = 2031,
		// Token: 0x0400019B RID: 411
		InvalidWarningLevel = 2015,
		// Token: 0x0400019C RID: 412
		ManagedResourceNotFound = 2022,
		// Token: 0x0400019D RID: 413
		MissingDefineArgument = 2018,
		// Token: 0x0400019E RID: 414
		MissingExtension,
		// Token: 0x0400019F RID: 415
		MissingLibArgument,
		// Token: 0x040001A0 RID: 416
		MissingReference = 2034,
		// Token: 0x040001A1 RID: 417
		MissingVersionInfo = 2021,
		// Token: 0x040001A2 RID: 418
		MultipleOutputNames = 2016,
		// Token: 0x040001A3 RID: 419
		MultipleTargets,
		// Token: 0x040001A4 RID: 420
		MultipleWin32Resources = 2033,
		// Token: 0x040001A5 RID: 421
		NestedResponseFiles = 2023,
		// Token: 0x040001A6 RID: 422
		NoCodePage,
		// Token: 0x040001A7 RID: 423
		NoFileName,
		// Token: 0x040001A8 RID: 424
		NoInputSourcesSpecified,
		// Token: 0x040001A9 RID: 425
		NoLocaleID,
		// Token: 0x040001AA RID: 426
		NoWarningLevel,
		// Token: 0x040001AB RID: 427
		ResourceNotFound,
		// Token: 0x040001AC RID: 428
		SourceFileTooBig = 2032,
		// Token: 0x040001AD RID: 429
		SourceNotFound = 2035,
		// Token: 0x040001AE RID: 430
		UnknownOption = 2030,
		// Token: 0x040001AF RID: 431
		Unspecified = 2999,
		// Token: 0x040001B0 RID: 432
		LAST = 2039
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Configuration
{
	// Token: 0x02000096 RID: 150
	[DebuggerDisplay("SectionInput {_sectionXmlInfo.ConfigKey}")]
	internal class SectionInput
	{
		// Token: 0x060005B3 RID: 1459 RVA: 0x0001B9E1 File Offset: 0x0001A9E1
		internal SectionInput(SectionXmlInfo sectionXmlInfo, List<ConfigurationException> errors)
		{
			this._sectionXmlInfo = sectionXmlInfo;
			this._errors = errors;
			this._result = SectionInput.s_unevaluated;
			this._resultRuntimeObject = SectionInput.s_unevaluated;
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x0001BA0D File Offset: 0x0001AA0D
		internal SectionXmlInfo SectionXmlInfo
		{
			get
			{
				return this._sectionXmlInfo;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x0001BA15 File Offset: 0x0001AA15
		internal bool HasResult
		{
			get
			{
				return this._result != SectionInput.s_unevaluated;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0001BA27 File Offset: 0x0001AA27
		internal bool HasResultRuntimeObject
		{
			get
			{
				return this._resultRuntimeObject != SectionInput.s_unevaluated;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x0001BA39 File Offset: 0x0001AA39
		// (set) Token: 0x060005B8 RID: 1464 RVA: 0x0001BA41 File Offset: 0x0001AA41
		internal object Result
		{
			get
			{
				return this._result;
			}
			set
			{
				this._result = value;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0001BA4A File Offset: 0x0001AA4A
		// (set) Token: 0x060005BA RID: 1466 RVA: 0x0001BA52 File Offset: 0x0001AA52
		internal object ResultRuntimeObject
		{
			get
			{
				return this._resultRuntimeObject;
			}
			set
			{
				this._resultRuntimeObject = value;
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001BA5B File Offset: 0x0001AA5B
		internal void ClearResult()
		{
			this._result = SectionInput.s_unevaluated;
			this._resultRuntimeObject = SectionInput.s_unevaluated;
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x0001BA73 File Offset: 0x0001AA73
		internal bool IsProtectionProviderDetermined
		{
			get
			{
				return this._isProtectionProviderDetermined;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x0001BA7B File Offset: 0x0001AA7B
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x0001BA83 File Offset: 0x0001AA83
		internal ProtectedConfigurationProvider ProtectionProvider
		{
			get
			{
				return this._protectionProvider;
			}
			set
			{
				this._protectionProvider = value;
				this._isProtectionProviderDetermined = true;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x0001BA93 File Offset: 0x0001AA93
		internal ICollection<ConfigurationException> Errors
		{
			get
			{
				return this._errors;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0001BA9B File Offset: 0x0001AA9B
		internal bool HasErrors
		{
			get
			{
				return ErrorsHelper.GetHasErrors(this._errors);
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001BAA8 File Offset: 0x0001AAA8
		internal void ThrowOnErrors()
		{
			ErrorsHelper.ThrowOnErrors(this._errors);
		}

		// Token: 0x040003AE RID: 942
		private static object s_unevaluated = new object();

		// Token: 0x040003AF RID: 943
		private SectionXmlInfo _sectionXmlInfo;

		// Token: 0x040003B0 RID: 944
		private ProtectedConfigurationProvider _protectionProvider;

		// Token: 0x040003B1 RID: 945
		private bool _isProtectionProviderDetermined;

		// Token: 0x040003B2 RID: 946
		private object _result;

		// Token: 0x040003B3 RID: 947
		private object _resultRuntimeObject;

		// Token: 0x040003B4 RID: 948
		private List<ConfigurationException> _errors;
	}
}

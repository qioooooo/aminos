using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Configuration
{
	// Token: 0x02000097 RID: 151
	[DebuggerDisplay("SectionRecord {ConfigKey}")]
	internal class SectionRecord
	{
		// Token: 0x060005C3 RID: 1475 RVA: 0x0001BAC1 File Offset: 0x0001AAC1
		internal SectionRecord(string configKey)
		{
			this._configKey = configKey;
			this._result = SectionRecord.s_unevaluated;
			this._resultRuntimeObject = SectionRecord.s_unevaluated;
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0001BAE6 File Offset: 0x0001AAE6
		internal string ConfigKey
		{
			get
			{
				return this._configKey;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0001BAEE File Offset: 0x0001AAEE
		internal bool Locked
		{
			get
			{
				return this._flags[1];
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x0001BAFC File Offset: 0x0001AAFC
		internal bool LockChildren
		{
			get
			{
				return this._flags[2];
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0001BB0C File Offset: 0x0001AB0C
		internal bool LockChildrenWithoutFileInput
		{
			get
			{
				bool flag = this.LockChildren;
				if (this.HasFileInput)
				{
					flag = this._flags[64];
				}
				return flag;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0001BB37 File Offset: 0x0001AB37
		// (set) Token: 0x060005C9 RID: 1481 RVA: 0x0001BB45 File Offset: 0x0001AB45
		internal bool IsResultTrustedWithoutAptca
		{
			get
			{
				return this._flags[4];
			}
			set
			{
				this._flags[4] = value;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x0001BB54 File Offset: 0x0001AB54
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x0001BB62 File Offset: 0x0001AB62
		internal bool RequirePermission
		{
			get
			{
				return this._flags[8];
			}
			set
			{
				this._flags[8] = value;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x0001BB71 File Offset: 0x0001AB71
		// (set) Token: 0x060005CD RID: 1485 RVA: 0x0001BB83 File Offset: 0x0001AB83
		internal bool AddUpdate
		{
			get
			{
				return this._flags[65536];
			}
			set
			{
				this._flags[65536] = value;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0001BB96 File Offset: 0x0001AB96
		internal bool HasLocationInputs
		{
			get
			{
				return this._locationInputs != null && this._locationInputs.Count > 0;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x0001BBB0 File Offset: 0x0001ABB0
		internal List<SectionInput> LocationInputs
		{
			get
			{
				return this._locationInputs;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0001BBB8 File Offset: 0x0001ABB8
		internal SectionInput LastLocationInput
		{
			get
			{
				if (this.HasLocationInputs)
				{
					return this._locationInputs[this._locationInputs.Count - 1];
				}
				return null;
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001BBDC File Offset: 0x0001ABDC
		internal void AddLocationInput(SectionInput sectionInput)
		{
			this.AddLocationInputImpl(sectionInput, false);
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001BBE6 File Offset: 0x0001ABE6
		internal bool HasFileInput
		{
			get
			{
				return this._fileInput != null;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x0001BBF4 File Offset: 0x0001ABF4
		internal SectionInput FileInput
		{
			get
			{
				return this._fileInput;
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001BBFC File Offset: 0x0001ABFC
		internal void ChangeLockSettings(OverrideMode forSelf, OverrideMode forChildren)
		{
			if (forSelf != OverrideMode.Inherit)
			{
				this._flags[1] = forSelf == OverrideMode.Deny;
				this._flags[2] = forSelf == OverrideMode.Deny;
			}
			if (forChildren != OverrideMode.Inherit)
			{
				this._flags[2] = forSelf == OverrideMode.Deny || forChildren == OverrideMode.Deny;
			}
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001BC3C File Offset: 0x0001AC3C
		internal void AddFileInput(SectionInput sectionInput)
		{
			this._fileInput = sectionInput;
			if (!sectionInput.HasErrors && sectionInput.SectionXmlInfo.OverrideModeSetting.OverrideMode != OverrideMode.Inherit)
			{
				this._flags[64] = this.LockChildren;
				this.ChangeLockSettings(OverrideMode.Inherit, sectionInput.SectionXmlInfo.OverrideModeSetting.OverrideMode);
			}
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001BC9A File Offset: 0x0001AC9A
		internal void RemoveFileInput()
		{
			if (this._fileInput != null)
			{
				this._fileInput = null;
				this._flags[2] = this.Locked;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0001BCBD File Offset: 0x0001ACBD
		internal bool HasIndirectLocationInputs
		{
			get
			{
				return this._indirectLocationInputs != null && this._indirectLocationInputs.Count > 0;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001BCD7 File Offset: 0x0001ACD7
		internal List<SectionInput> IndirectLocationInputs
		{
			get
			{
				return this._indirectLocationInputs;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0001BCDF File Offset: 0x0001ACDF
		internal SectionInput LastIndirectLocationInput
		{
			get
			{
				if (this.HasIndirectLocationInputs)
				{
					return this._indirectLocationInputs[this._indirectLocationInputs.Count - 1];
				}
				return null;
			}
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001BD03 File Offset: 0x0001AD03
		internal void AddIndirectLocationInput(SectionInput sectionInput)
		{
			this.AddLocationInputImpl(sectionInput, true);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001BD10 File Offset: 0x0001AD10
		private void AddLocationInputImpl(SectionInput sectionInput, bool isIndirectLocation)
		{
			List<SectionInput> list = (isIndirectLocation ? this._indirectLocationInputs : this._locationInputs);
			int num = (isIndirectLocation ? 32 : 16);
			if (list == null)
			{
				list = new List<SectionInput>(1);
				if (isIndirectLocation)
				{
					this._indirectLocationInputs = list;
				}
				else
				{
					this._locationInputs = list;
				}
			}
			list.Insert(0, sectionInput);
			if (!sectionInput.HasErrors && !this._flags[num])
			{
				OverrideMode overrideMode = sectionInput.SectionXmlInfo.OverrideModeSetting.OverrideMode;
				if (overrideMode != OverrideMode.Inherit)
				{
					this.ChangeLockSettings(overrideMode, overrideMode);
					this._flags[num] = true;
				}
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001BDA0 File Offset: 0x0001ADA0
		internal bool HasInput
		{
			get
			{
				return this.HasLocationInputs || this.HasFileInput || this.HasIndirectLocationInputs;
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001BDBC File Offset: 0x0001ADBC
		internal void ClearRawXml()
		{
			if (this.HasLocationInputs)
			{
				foreach (SectionInput sectionInput in this.LocationInputs)
				{
					sectionInput.SectionXmlInfo.RawXml = null;
				}
			}
			if (this.HasIndirectLocationInputs)
			{
				foreach (SectionInput sectionInput2 in this.IndirectLocationInputs)
				{
					sectionInput2.SectionXmlInfo.RawXml = null;
				}
			}
			if (this.HasFileInput)
			{
				this.FileInput.SectionXmlInfo.RawXml = null;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x0001BE84 File Offset: 0x0001AE84
		internal bool HasResult
		{
			get
			{
				return this._result != SectionRecord.s_unevaluated;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x0001BE96 File Offset: 0x0001AE96
		internal bool HasResultRuntimeObject
		{
			get
			{
				return this._resultRuntimeObject != SectionRecord.s_unevaluated;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0001BEA8 File Offset: 0x0001AEA8
		// (set) Token: 0x060005E1 RID: 1505 RVA: 0x0001BEB0 File Offset: 0x0001AEB0
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

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0001BEB9 File Offset: 0x0001AEB9
		// (set) Token: 0x060005E3 RID: 1507 RVA: 0x0001BEC1 File Offset: 0x0001AEC1
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

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001BECC File Offset: 0x0001AECC
		internal void ClearResult()
		{
			if (this._fileInput != null)
			{
				this._fileInput.ClearResult();
			}
			if (this._locationInputs != null)
			{
				foreach (SectionInput sectionInput in this._locationInputs)
				{
					sectionInput.ClearResult();
				}
			}
			this._result = SectionRecord.s_unevaluated;
			this._resultRuntimeObject = SectionRecord.s_unevaluated;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001BF50 File Offset: 0x0001AF50
		private List<ConfigurationException> GetAllErrors()
		{
			List<ConfigurationException> list = null;
			ErrorsHelper.AddErrors(ref list, this._errors);
			if (this.HasLocationInputs)
			{
				foreach (SectionInput sectionInput in this.LocationInputs)
				{
					ErrorsHelper.AddErrors(ref list, sectionInput.Errors);
				}
			}
			if (this.HasIndirectLocationInputs)
			{
				foreach (SectionInput sectionInput2 in this.IndirectLocationInputs)
				{
					ErrorsHelper.AddErrors(ref list, sectionInput2.Errors);
				}
			}
			if (this.HasFileInput)
			{
				ErrorsHelper.AddErrors(ref list, this.FileInput.Errors);
			}
			return list;
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x0001C02C File Offset: 0x0001B02C
		internal bool HasErrors
		{
			get
			{
				if (ErrorsHelper.GetHasErrors(this._errors))
				{
					return true;
				}
				if (this.HasLocationInputs)
				{
					foreach (SectionInput sectionInput in this.LocationInputs)
					{
						if (sectionInput.HasErrors)
						{
							return true;
						}
					}
				}
				if (this.HasIndirectLocationInputs)
				{
					foreach (SectionInput sectionInput2 in this.IndirectLocationInputs)
					{
						if (sectionInput2.HasErrors)
						{
							return true;
						}
					}
				}
				return this.HasFileInput && this.FileInput.HasErrors;
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001C108 File Offset: 0x0001B108
		internal void ThrowOnErrors()
		{
			if (this.HasErrors)
			{
				throw new ConfigurationErrorsException(this.GetAllErrors());
			}
		}

		// Token: 0x040003B5 RID: 949
		private const int Flag_Locked = 1;

		// Token: 0x040003B6 RID: 950
		private const int Flag_LockChildren = 2;

		// Token: 0x040003B7 RID: 951
		private const int Flag_IsResultTrustedWithoutAptca = 4;

		// Token: 0x040003B8 RID: 952
		private const int Flag_RequirePermission = 8;

		// Token: 0x040003B9 RID: 953
		private const int Flag_LocationInputLockApplied = 16;

		// Token: 0x040003BA RID: 954
		private const int Flag_IndirectLocationInputLockApplied = 32;

		// Token: 0x040003BB RID: 955
		private const int Flag_ChildrenLockWithoutFileInput = 64;

		// Token: 0x040003BC RID: 956
		private const int Flag_AddUpdate = 65536;

		// Token: 0x040003BD RID: 957
		private static object s_unevaluated = new object();

		// Token: 0x040003BE RID: 958
		private SafeBitVector32 _flags;

		// Token: 0x040003BF RID: 959
		private string _configKey;

		// Token: 0x040003C0 RID: 960
		private List<SectionInput> _locationInputs;

		// Token: 0x040003C1 RID: 961
		private SectionInput _fileInput;

		// Token: 0x040003C2 RID: 962
		private List<SectionInput> _indirectLocationInputs;

		// Token: 0x040003C3 RID: 963
		private object _result;

		// Token: 0x040003C4 RID: 964
		private object _resultRuntimeObject;

		// Token: 0x040003C5 RID: 965
		private List<ConfigurationException> _errors;
	}
}

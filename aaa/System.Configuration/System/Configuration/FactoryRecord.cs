using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Diagnostics;

namespace System.Configuration
{
	// Token: 0x02000069 RID: 105
	[DebuggerDisplay("FactoryRecord {ConfigKey}")]
	internal class FactoryRecord : IConfigErrorInfo
	{
		// Token: 0x060003E7 RID: 999 RVA: 0x000137EC File Offset: 0x000127EC
		private FactoryRecord(string configKey, string group, string name, object factory, string factoryTypeName, SimpleBitVector32 flags, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition, OverrideModeSetting overrideModeDefault, string filename, int lineNumber, ICollection<ConfigurationException> errors)
		{
			this._configKey = configKey;
			this._group = group;
			this._name = name;
			this._factory = factory;
			this._factoryTypeName = factoryTypeName;
			this._flags = flags;
			this._allowDefinition = allowDefinition;
			this._allowExeDefinition = allowExeDefinition;
			this._overrideModeDefault = overrideModeDefault;
			this._filename = filename;
			this._lineNumber = lineNumber;
			this.AddErrors(errors);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001385C File Offset: 0x0001285C
		internal FactoryRecord(string configKey, string group, string name, string factoryTypeName, string filename, int lineNumber)
		{
			this._configKey = configKey;
			this._group = group;
			this._name = name;
			this._factoryTypeName = factoryTypeName;
			this.IsGroup = true;
			this._filename = filename;
			this._lineNumber = lineNumber;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00013898 File Offset: 0x00012898
		internal FactoryRecord(string configKey, string group, string name, string factoryTypeName, bool allowLocation, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition, OverrideModeSetting overrideModeDefault, bool restartOnExternalChanges, bool requirePermission, bool isFromTrustedConfigRecord, bool isUndeclared, string filename, int lineNumber)
		{
			this._configKey = configKey;
			this._group = group;
			this._name = name;
			this._factoryTypeName = factoryTypeName;
			this._allowDefinition = allowDefinition;
			this._allowExeDefinition = allowExeDefinition;
			this._overrideModeDefault = overrideModeDefault;
			this.AllowLocation = allowLocation;
			this.RestartOnExternalChanges = restartOnExternalChanges;
			this.RequirePermission = requirePermission;
			this.IsFromTrustedConfigRecord = isFromTrustedConfigRecord;
			this.IsUndeclared = isUndeclared;
			this._filename = filename;
			this._lineNumber = lineNumber;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00013918 File Offset: 0x00012918
		internal FactoryRecord CloneSection(string filename, int lineNumber)
		{
			return new FactoryRecord(this._configKey, this._group, this._name, this._factory, this._factoryTypeName, this._flags, this._allowDefinition, this._allowExeDefinition, this._overrideModeDefault, filename, lineNumber, this.Errors);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00013968 File Offset: 0x00012968
		internal FactoryRecord CloneSectionGroup(string factoryTypeName, string filename, int lineNumber)
		{
			if (this._factoryTypeName != null)
			{
				factoryTypeName = this._factoryTypeName;
			}
			return new FactoryRecord(this._configKey, this._group, this._name, this._factory, factoryTypeName, this._flags, this._allowDefinition, this._allowExeDefinition, this._overrideModeDefault, filename, lineNumber, this.Errors);
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x000139C3 File Offset: 0x000129C3
		internal string ConfigKey
		{
			get
			{
				return this._configKey;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x000139CB File Offset: 0x000129CB
		internal string Group
		{
			get
			{
				return this._group;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x000139D3 File Offset: 0x000129D3
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x000139DB File Offset: 0x000129DB
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x000139E3 File Offset: 0x000129E3
		internal object Factory
		{
			get
			{
				return this._factory;
			}
			set
			{
				this._factory = value;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000139EC File Offset: 0x000129EC
		// (set) Token: 0x060003F2 RID: 1010 RVA: 0x000139F4 File Offset: 0x000129F4
		internal string FactoryTypeName
		{
			get
			{
				return this._factoryTypeName;
			}
			set
			{
				this._factoryTypeName = value;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x000139FD File Offset: 0x000129FD
		// (set) Token: 0x060003F4 RID: 1012 RVA: 0x00013A05 File Offset: 0x00012A05
		internal ConfigurationAllowDefinition AllowDefinition
		{
			get
			{
				return this._allowDefinition;
			}
			set
			{
				this._allowDefinition = value;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00013A0E File Offset: 0x00012A0E
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x00013A16 File Offset: 0x00012A16
		internal ConfigurationAllowExeDefinition AllowExeDefinition
		{
			get
			{
				return this._allowExeDefinition;
			}
			set
			{
				this._allowExeDefinition = value;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00013A1F File Offset: 0x00012A1F
		internal OverrideModeSetting OverrideModeDefault
		{
			get
			{
				return this._overrideModeDefault;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00013A27 File Offset: 0x00012A27
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x00013A35 File Offset: 0x00012A35
		internal bool AllowLocation
		{
			get
			{
				return this._flags[1];
			}
			set
			{
				this._flags[1] = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00013A44 File Offset: 0x00012A44
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x00013A52 File Offset: 0x00012A52
		internal bool RestartOnExternalChanges
		{
			get
			{
				return this._flags[2];
			}
			set
			{
				this._flags[2] = value;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x00013A61 File Offset: 0x00012A61
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x00013A6F File Offset: 0x00012A6F
		internal bool RequirePermission
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

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x00013A7E File Offset: 0x00012A7E
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x00013A8C File Offset: 0x00012A8C
		internal bool IsGroup
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

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00013A9B File Offset: 0x00012A9B
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x00013AAA File Offset: 0x00012AAA
		internal bool IsFromTrustedConfigRecord
		{
			get
			{
				return this._flags[16];
			}
			set
			{
				this._flags[16] = value;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00013ABA File Offset: 0x00012ABA
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x00013AC9 File Offset: 0x00012AC9
		internal bool IsUndeclared
		{
			get
			{
				return this._flags[64];
			}
			set
			{
				this._flags[64] = value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00013AD9 File Offset: 0x00012AD9
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x00013AE8 File Offset: 0x00012AE8
		internal bool IsFactoryTrustedWithoutAptca
		{
			get
			{
				return this._flags[32];
			}
			set
			{
				this._flags[32] = value;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x00013AF8 File Offset: 0x00012AF8
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x00013B00 File Offset: 0x00012B00
		public string Filename
		{
			get
			{
				return this._filename;
			}
			set
			{
				this._filename = value;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00013B09 File Offset: 0x00012B09
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x00013B11 File Offset: 0x00012B11
		public int LineNumber
		{
			get
			{
				return this._lineNumber;
			}
			set
			{
				this._lineNumber = value;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00013B1A File Offset: 0x00012B1A
		internal bool HasFile
		{
			get
			{
				return this._lineNumber >= 0;
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00013B28 File Offset: 0x00012B28
		internal bool IsEquivalentType(IInternalConfigHost host, string typeName)
		{
			try
			{
				if (this._factoryTypeName == typeName)
				{
					return true;
				}
				Type type;
				Type type2;
				if (host != null)
				{
					type = TypeUtil.GetTypeWithReflectionPermission(host, typeName, false);
					type2 = TypeUtil.GetTypeWithReflectionPermission(host, this._factoryTypeName, false);
				}
				else
				{
					type = TypeUtil.GetTypeWithReflectionPermission(typeName, false);
					type2 = TypeUtil.GetTypeWithReflectionPermission(this._factoryTypeName, false);
				}
				return type != null && type == type2;
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00013B9C File Offset: 0x00012B9C
		internal bool IsEquivalentSectionGroupFactory(IInternalConfigHost host, string typeName)
		{
			return typeName == null || this._factoryTypeName == null || this.IsEquivalentType(host, typeName);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00013BB3 File Offset: 0x00012BB3
		internal bool IsEquivalentSectionFactory(IInternalConfigHost host, string typeName, bool allowLocation, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition, bool restartOnExternalChanges, bool requirePermission)
		{
			return allowLocation == this.AllowLocation && allowDefinition == this.AllowDefinition && allowExeDefinition == this.AllowExeDefinition && restartOnExternalChanges == this.RestartOnExternalChanges && requirePermission == this.RequirePermission && this.IsEquivalentType(host, typeName);
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x00013BF0 File Offset: 0x00012BF0
		internal List<ConfigurationException> Errors
		{
			get
			{
				return this._errors;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00013BF8 File Offset: 0x00012BF8
		internal bool HasErrors
		{
			get
			{
				return ErrorsHelper.GetHasErrors(this._errors);
			}
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00013C05 File Offset: 0x00012C05
		internal void AddErrors(ICollection<ConfigurationException> coll)
		{
			ErrorsHelper.AddErrors(ref this._errors, coll);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00013C13 File Offset: 0x00012C13
		internal void ThrowOnErrors()
		{
			ErrorsHelper.ThrowOnErrors(this._errors);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00013C20 File Offset: 0x00012C20
		internal bool IsIgnorable()
		{
			if (this._factory != null)
			{
				return this._factory is IgnoreSectionHandler;
			}
			return this._factoryTypeName != null && this._factoryTypeName.Contains("System.Configuration.IgnoreSection");
		}

		// Token: 0x04000301 RID: 769
		private const int Flag_AllowLocation = 1;

		// Token: 0x04000302 RID: 770
		private const int Flag_RestartOnExternalChanges = 2;

		// Token: 0x04000303 RID: 771
		private const int Flag_RequirePermission = 4;

		// Token: 0x04000304 RID: 772
		private const int Flag_IsGroup = 8;

		// Token: 0x04000305 RID: 773
		private const int Flag_IsFromTrustedConfigRecord = 16;

		// Token: 0x04000306 RID: 774
		private const int Flag_IsFactoryTrustedWithoutAptca = 32;

		// Token: 0x04000307 RID: 775
		private const int Flag_IsUndeclared = 64;

		// Token: 0x04000308 RID: 776
		private string _configKey;

		// Token: 0x04000309 RID: 777
		private string _group;

		// Token: 0x0400030A RID: 778
		private string _name;

		// Token: 0x0400030B RID: 779
		private SimpleBitVector32 _flags;

		// Token: 0x0400030C RID: 780
		private string _factoryTypeName;

		// Token: 0x0400030D RID: 781
		private ConfigurationAllowDefinition _allowDefinition;

		// Token: 0x0400030E RID: 782
		private ConfigurationAllowExeDefinition _allowExeDefinition;

		// Token: 0x0400030F RID: 783
		private OverrideModeSetting _overrideModeDefault;

		// Token: 0x04000310 RID: 784
		private string _filename;

		// Token: 0x04000311 RID: 785
		private int _lineNumber;

		// Token: 0x04000312 RID: 786
		private object _factory;

		// Token: 0x04000313 RID: 787
		private List<ConfigurationException> _errors;
	}
}

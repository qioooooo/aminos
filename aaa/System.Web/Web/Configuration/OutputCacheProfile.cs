using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Web.UI;

namespace System.Web.Configuration
{
	// Token: 0x02000218 RID: 536
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class OutputCacheProfile : ConfigurationElement
	{
		// Token: 0x06001CB4 RID: 7348 RVA: 0x0008346C File Offset: 0x0008246C
		static OutputCacheProfile()
		{
			OutputCacheProfile._properties.Add(OutputCacheProfile._propName);
			OutputCacheProfile._properties.Add(OutputCacheProfile._propEnabled);
			OutputCacheProfile._properties.Add(OutputCacheProfile._propDuration);
			OutputCacheProfile._properties.Add(OutputCacheProfile._propLocation);
			OutputCacheProfile._properties.Add(OutputCacheProfile._propSqlDependency);
			OutputCacheProfile._properties.Add(OutputCacheProfile._propVaryByCustom);
			OutputCacheProfile._properties.Add(OutputCacheProfile._propVaryByControl);
			OutputCacheProfile._properties.Add(OutputCacheProfile._propVaryByContentEncoding);
			OutputCacheProfile._properties.Add(OutputCacheProfile._propVaryByHeader);
			OutputCacheProfile._properties.Add(OutputCacheProfile._propVaryByParam);
			OutputCacheProfile._properties.Add(OutputCacheProfile._propNoStore);
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x0008366F File Offset: 0x0008266F
		internal OutputCacheProfile()
		{
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x00083677 File Offset: 0x00082677
		public OutputCacheProfile(string name)
		{
			this.Name = name;
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001CB7 RID: 7351 RVA: 0x00083686 File Offset: 0x00082686
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return OutputCacheProfile._properties;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x0008368D File Offset: 0x0008268D
		// (set) Token: 0x06001CB9 RID: 7353 RVA: 0x0008369F File Offset: 0x0008269F
		[ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
		[TypeConverter(typeof(WhiteSpaceTrimStringConverter))]
		[StringValidator(MinLength = 1)]
		public string Name
		{
			get
			{
				return (string)base[OutputCacheProfile._propName];
			}
			set
			{
				base[OutputCacheProfile._propName] = value;
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001CBA RID: 7354 RVA: 0x000836AD File Offset: 0x000826AD
		// (set) Token: 0x06001CBB RID: 7355 RVA: 0x000836BF File Offset: 0x000826BF
		[ConfigurationProperty("enabled", DefaultValue = true)]
		public bool Enabled
		{
			get
			{
				return (bool)base[OutputCacheProfile._propEnabled];
			}
			set
			{
				base[OutputCacheProfile._propEnabled] = value;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001CBC RID: 7356 RVA: 0x000836D2 File Offset: 0x000826D2
		// (set) Token: 0x06001CBD RID: 7357 RVA: 0x000836E4 File Offset: 0x000826E4
		[ConfigurationProperty("duration", DefaultValue = -1)]
		public int Duration
		{
			get
			{
				return (int)base[OutputCacheProfile._propDuration];
			}
			set
			{
				base[OutputCacheProfile._propDuration] = value;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001CBE RID: 7358 RVA: 0x000836F7 File Offset: 0x000826F7
		// (set) Token: 0x06001CBF RID: 7359 RVA: 0x00083709 File Offset: 0x00082709
		[ConfigurationProperty("location")]
		public OutputCacheLocation Location
		{
			get
			{
				return (OutputCacheLocation)base[OutputCacheProfile._propLocation];
			}
			set
			{
				base[OutputCacheProfile._propLocation] = value;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001CC0 RID: 7360 RVA: 0x0008371C File Offset: 0x0008271C
		// (set) Token: 0x06001CC1 RID: 7361 RVA: 0x0008372E File Offset: 0x0008272E
		[ConfigurationProperty("sqlDependency")]
		public string SqlDependency
		{
			get
			{
				return (string)base[OutputCacheProfile._propSqlDependency];
			}
			set
			{
				base[OutputCacheProfile._propSqlDependency] = value;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x0008373C File Offset: 0x0008273C
		// (set) Token: 0x06001CC3 RID: 7363 RVA: 0x0008374E File Offset: 0x0008274E
		[ConfigurationProperty("varyByCustom")]
		public string VaryByCustom
		{
			get
			{
				return (string)base[OutputCacheProfile._propVaryByCustom];
			}
			set
			{
				base[OutputCacheProfile._propVaryByCustom] = value;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001CC4 RID: 7364 RVA: 0x0008375C File Offset: 0x0008275C
		// (set) Token: 0x06001CC5 RID: 7365 RVA: 0x0008376E File Offset: 0x0008276E
		[ConfigurationProperty("varyByControl")]
		public string VaryByControl
		{
			get
			{
				return (string)base[OutputCacheProfile._propVaryByControl];
			}
			set
			{
				base[OutputCacheProfile._propVaryByControl] = value;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001CC6 RID: 7366 RVA: 0x0008377C File Offset: 0x0008277C
		// (set) Token: 0x06001CC7 RID: 7367 RVA: 0x0008378E File Offset: 0x0008278E
		[ConfigurationProperty("varyByContentEncoding")]
		public string VaryByContentEncoding
		{
			get
			{
				return (string)base[OutputCacheProfile._propVaryByContentEncoding];
			}
			set
			{
				base[OutputCacheProfile._propVaryByContentEncoding] = value;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001CC8 RID: 7368 RVA: 0x0008379C File Offset: 0x0008279C
		// (set) Token: 0x06001CC9 RID: 7369 RVA: 0x000837AE File Offset: 0x000827AE
		[ConfigurationProperty("varyByHeader")]
		public string VaryByHeader
		{
			get
			{
				return (string)base[OutputCacheProfile._propVaryByHeader];
			}
			set
			{
				base[OutputCacheProfile._propVaryByHeader] = value;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001CCA RID: 7370 RVA: 0x000837BC File Offset: 0x000827BC
		// (set) Token: 0x06001CCB RID: 7371 RVA: 0x000837CE File Offset: 0x000827CE
		[ConfigurationProperty("varyByParam")]
		public string VaryByParam
		{
			get
			{
				return (string)base[OutputCacheProfile._propVaryByParam];
			}
			set
			{
				base[OutputCacheProfile._propVaryByParam] = value;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001CCC RID: 7372 RVA: 0x000837DC File Offset: 0x000827DC
		// (set) Token: 0x06001CCD RID: 7373 RVA: 0x000837EE File Offset: 0x000827EE
		[ConfigurationProperty("noStore", DefaultValue = false)]
		public bool NoStore
		{
			get
			{
				return (bool)base[OutputCacheProfile._propNoStore];
			}
			set
			{
				base[OutputCacheProfile._propNoStore] = value;
			}
		}

		// Token: 0x040018FF RID: 6399
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001900 RID: 6400
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, StdValidatorsAndConverters.WhiteSpaceTrimStringConverter, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04001901 RID: 6401
		private static readonly ConfigurationProperty _propEnabled = new ConfigurationProperty("enabled", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001902 RID: 6402
		private static readonly ConfigurationProperty _propDuration = new ConfigurationProperty("duration", typeof(int), -1, ConfigurationPropertyOptions.None);

		// Token: 0x04001903 RID: 6403
		private static readonly ConfigurationProperty _propLocation = new ConfigurationProperty("location", typeof(OutputCacheLocation), (OutputCacheLocation)(-1), ConfigurationPropertyOptions.None);

		// Token: 0x04001904 RID: 6404
		private static readonly ConfigurationProperty _propSqlDependency = new ConfigurationProperty("sqlDependency", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001905 RID: 6405
		private static readonly ConfigurationProperty _propVaryByCustom = new ConfigurationProperty("varyByCustom", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001906 RID: 6406
		private static readonly ConfigurationProperty _propVaryByContentEncoding = new ConfigurationProperty("varyByContentEncoding", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001907 RID: 6407
		private static readonly ConfigurationProperty _propVaryByHeader = new ConfigurationProperty("varyByHeader", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001908 RID: 6408
		private static readonly ConfigurationProperty _propVaryByParam = new ConfigurationProperty("varyByParam", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001909 RID: 6409
		private static readonly ConfigurationProperty _propNoStore = new ConfigurationProperty("noStore", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x0400190A RID: 6410
		private static readonly ConfigurationProperty _propVaryByControl = new ConfigurationProperty("varyByControl", typeof(string), null, ConfigurationPropertyOptions.None);
	}
}

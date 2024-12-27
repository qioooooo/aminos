using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000454 RID: 1108
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class StaticPartialCachingControl : BasePartialCachingControl
	{
		// Token: 0x060034B7 RID: 13495 RVA: 0x000E47E0 File Offset: 0x000E37E0
		public StaticPartialCachingControl(string ctrlID, string guid, int duration, string varyByParams, string varyByControls, string varyByCustom, BuildMethod buildMethod)
			: this(ctrlID, guid, duration, varyByParams, varyByControls, varyByCustom, null, buildMethod)
		{
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x000E4800 File Offset: 0x000E3800
		public StaticPartialCachingControl(string ctrlID, string guid, int duration, string varyByParams, string varyByControls, string varyByCustom, string sqlDependency, BuildMethod buildMethod)
		{
			this._ctrlID = ctrlID;
			base.Duration = new TimeSpan(0, 0, duration);
			base.SetVaryByParamsCollectionFromString(varyByParams);
			if (varyByControls != null)
			{
				this._varyByControlsCollection = varyByControls.Split(new char[] { ';' });
			}
			this._varyByCustom = varyByCustom;
			this._guid = guid;
			this._buildMethod = buildMethod;
			this._sqlDependency = sqlDependency;
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x000E486D File Offset: 0x000E386D
		internal override Control CreateCachedControl()
		{
			return this._buildMethod();
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x000E487C File Offset: 0x000E387C
		public static void BuildCachedControl(Control parent, string ctrlID, string guid, int duration, string varyByParams, string varyByControls, string varyByCustom, BuildMethod buildMethod)
		{
			StaticPartialCachingControl.BuildCachedControl(parent, ctrlID, guid, duration, varyByParams, varyByControls, varyByCustom, null, buildMethod);
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x000E489C File Offset: 0x000E389C
		public static void BuildCachedControl(Control parent, string ctrlID, string guid, int duration, string varyByParams, string varyByControls, string varyByCustom, string sqlDependency, BuildMethod buildMethod)
		{
			StaticPartialCachingControl staticPartialCachingControl = new StaticPartialCachingControl(ctrlID, guid, duration, varyByParams, varyByControls, varyByCustom, sqlDependency, buildMethod);
			((IParserAccessor)parent).AddParsedSubObject(staticPartialCachingControl);
		}

		// Token: 0x040024E5 RID: 9445
		private BuildMethod _buildMethod;
	}
}

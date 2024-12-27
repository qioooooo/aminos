using System;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000455 RID: 1109
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class PartialCachingControl : BasePartialCachingControl
	{
		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x060034BC RID: 13500 RVA: 0x000E48C3 File Offset: 0x000E38C3
		public Control CachedControl
		{
			get
			{
				return this._cachedCtrl;
			}
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x000E48CC File Offset: 0x000E38CC
		internal PartialCachingControl(IWebObjectFactory objectFactory, Type createCachedControlType, PartialCachingAttribute cacheAttrib, string cacheKey, object[] args)
		{
			this._ctrlID = cacheKey;
			base.Duration = new TimeSpan(0, 0, cacheAttrib.Duration);
			base.SetVaryByParamsCollectionFromString(cacheAttrib.VaryByParams);
			if (cacheAttrib.VaryByControls != null)
			{
				this._varyByControlsCollection = cacheAttrib.VaryByControls.Split(new char[] { ';' });
			}
			this._varyByCustom = cacheAttrib.VaryByCustom;
			this._sqlDependency = cacheAttrib.SqlDependency;
			this._guid = cacheKey;
			this._objectFactory = objectFactory;
			this._createCachedControlType = createCachedControlType;
			this._args = args;
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x000E4964 File Offset: 0x000E3964
		internal override Control CreateCachedControl()
		{
			Control control;
			if (this._objectFactory != null)
			{
				control = (Control)this._objectFactory.CreateInstance();
			}
			else
			{
				control = (Control)HttpRuntime.CreatePublicInstance(this._createCachedControlType, this._args);
			}
			UserControl userControl = control as UserControl;
			if (userControl != null)
			{
				userControl.InitializeAsUserControl(this.Page);
			}
			control.ID = this._ctrlID;
			return control;
		}

		// Token: 0x040024E6 RID: 9446
		private IWebObjectFactory _objectFactory;

		// Token: 0x040024E7 RID: 9447
		private Type _createCachedControlType;

		// Token: 0x040024E8 RID: 9448
		private object[] _args;
	}
}

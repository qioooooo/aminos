using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006C4 RID: 1732
	[ToolboxItem(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class ProxyWebPart : WebPart
	{
		// Token: 0x06005545 RID: 21829 RVA: 0x00159C2C File Offset: 0x00158C2C
		protected ProxyWebPart(WebPart webPart)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			GenericWebPart genericWebPart = webPart as GenericWebPart;
			if (genericWebPart != null)
			{
				Control childControl = genericWebPart.ChildControl;
				if (childControl == null)
				{
					throw new ArgumentException(SR.GetString("PropertyCannotBeNull", new object[] { "ChildControl" }), "webPart");
				}
				this._originalID = childControl.ID;
				if (string.IsNullOrEmpty(this._originalID))
				{
					throw new ArgumentException(SR.GetString("PropertyCannotBeNullOrEmptyString", new object[] { "ChildControl.ID" }), "webPart");
				}
				UserControl userControl = childControl as UserControl;
				Type type;
				if (userControl != null)
				{
					type = typeof(UserControl);
					this._originalPath = userControl.AppRelativeVirtualPath;
				}
				else
				{
					type = childControl.GetType();
				}
				this._originalTypeName = WebPartUtil.SerializeType(type);
				this._genericWebPartID = genericWebPart.ID;
				if (string.IsNullOrEmpty(this._genericWebPartID))
				{
					throw new ArgumentException(SR.GetString("PropertyCannotBeNullOrEmptyString", new object[] { "ID" }), "webPart");
				}
				this.ID = this._genericWebPartID;
				return;
			}
			else
			{
				this._originalID = webPart.ID;
				if (string.IsNullOrEmpty(this._originalID))
				{
					throw new ArgumentException(SR.GetString("PropertyCannotBeNullOrEmptyString", new object[] { "ID" }), "webPart");
				}
				this._originalTypeName = WebPartUtil.SerializeType(webPart.GetType());
				this.ID = this._originalID;
				return;
			}
		}

		// Token: 0x06005546 RID: 21830 RVA: 0x00159DB0 File Offset: 0x00158DB0
		protected ProxyWebPart(string originalID, string originalTypeName, string originalPath, string genericWebPartID)
		{
			if (string.IsNullOrEmpty(originalID))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("originalID");
			}
			if (string.IsNullOrEmpty(originalTypeName))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("originalTypeName");
			}
			if (!string.IsNullOrEmpty(originalPath) && string.IsNullOrEmpty(genericWebPartID))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("genericWebPartID");
			}
			this._originalID = originalID;
			this._originalTypeName = originalTypeName;
			this._originalPath = originalPath;
			this._genericWebPartID = genericWebPartID;
			if (!string.IsNullOrEmpty(genericWebPartID))
			{
				this.ID = this._genericWebPartID;
				return;
			}
			this.ID = this._originalID;
		}

		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x06005547 RID: 21831 RVA: 0x00159E44 File Offset: 0x00158E44
		public string GenericWebPartID
		{
			get
			{
				if (this._genericWebPartID == null)
				{
					return string.Empty;
				}
				return this._genericWebPartID;
			}
		}

		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x06005548 RID: 21832 RVA: 0x00159E5A File Offset: 0x00158E5A
		// (set) Token: 0x06005549 RID: 21833 RVA: 0x00159E62 File Offset: 0x00158E62
		public sealed override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
				base.ID = value;
			}
		}

		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x0600554A RID: 21834 RVA: 0x00159E6B File Offset: 0x00158E6B
		public string OriginalID
		{
			get
			{
				if (this._originalID == null)
				{
					return string.Empty;
				}
				return this._originalID;
			}
		}

		// Token: 0x170015EF RID: 5615
		// (get) Token: 0x0600554B RID: 21835 RVA: 0x00159E81 File Offset: 0x00158E81
		public string OriginalTypeName
		{
			get
			{
				if (this._originalTypeName == null)
				{
					return string.Empty;
				}
				return this._originalTypeName;
			}
		}

		// Token: 0x170015F0 RID: 5616
		// (get) Token: 0x0600554C RID: 21836 RVA: 0x00159E97 File Offset: 0x00158E97
		public string OriginalPath
		{
			get
			{
				if (this._originalPath == null)
				{
					return string.Empty;
				}
				return this._originalPath;
			}
		}

		// Token: 0x0600554D RID: 21837 RVA: 0x00159EAD File Offset: 0x00158EAD
		protected internal override void LoadControlState(object savedState)
		{
		}

		// Token: 0x0600554E RID: 21838 RVA: 0x00159EAF File Offset: 0x00158EAF
		protected override void LoadViewState(object savedState)
		{
		}

		// Token: 0x0600554F RID: 21839 RVA: 0x00159EB1 File Offset: 0x00158EB1
		protected internal override object SaveControlState()
		{
			base.SaveControlState();
			return null;
		}

		// Token: 0x06005550 RID: 21840 RVA: 0x00159EBB File Offset: 0x00158EBB
		protected override object SaveViewState()
		{
			base.SaveViewState();
			return null;
		}

		// Token: 0x04002F16 RID: 12054
		private string _originalID;

		// Token: 0x04002F17 RID: 12055
		private string _originalTypeName;

		// Token: 0x04002F18 RID: 12056
		private string _originalPath;

		// Token: 0x04002F19 RID: 12057
		private string _genericWebPartID;
	}
}

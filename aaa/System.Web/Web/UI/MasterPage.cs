using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x0200042A RID: 1066
	[ControlBuilder(typeof(MasterPageControlBuilder))]
	[Designer("Microsoft.VisualStudio.Web.WebForms.MasterPageWebFormDesigner, Microsoft.VisualStudio.Web, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
	[ParseChildren(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class MasterPage : UserControl
	{
		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x0600332D RID: 13101 RVA: 0x000DE1F8 File Offset: 0x000DD1F8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal IDictionary ContentTemplates
		{
			get
			{
				return this._contentTemplates;
			}
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x0600332E RID: 13102 RVA: 0x000DE200 File Offset: 0x000DD200
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		protected internal IList ContentPlaceHolders
		{
			get
			{
				if (this._contentPlaceHolders == null)
				{
					this._contentPlaceHolders = new ArrayList();
				}
				return this._contentPlaceHolders;
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x0600332F RID: 13103 RVA: 0x000DE21B File Offset: 0x000DD21B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebSysDescription("MasterPage_MasterPage")]
		[Browsable(false)]
		public MasterPage Master
		{
			get
			{
				if (this._master == null && !this._masterPageApplied)
				{
					this._master = MasterPage.CreateMaster(this, this.Context, this._masterPageFile, this._contentTemplateCollection);
				}
				return this._master;
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06003330 RID: 13104 RVA: 0x000DE251 File Offset: 0x000DD251
		// (set) Token: 0x06003331 RID: 13105 RVA: 0x000DE260 File Offset: 0x000DD260
		[WebSysDescription("MasterPage_MasterPageFile")]
		[WebCategory("Behavior")]
		[DefaultValue("")]
		public string MasterPageFile
		{
			get
			{
				return VirtualPath.GetVirtualPathString(this._masterPageFile);
			}
			set
			{
				if (this._masterPageApplied)
				{
					throw new InvalidOperationException(SR.GetString("PropertySetBeforePageEvent", new object[] { "MasterPageFile", "Page_PreInit" }));
				}
				if (value != VirtualPath.GetVirtualPathString(this._masterPageFile))
				{
					this._masterPageFile = VirtualPath.CreateAllowNull(value);
					if (this._master != null && this.Controls.Contains(this._master))
					{
						this.Controls.Remove(this._master);
					}
					this._master = null;
				}
			}
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x000DE2F0 File Offset: 0x000DD2F0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal void AddContentTemplate(string templateName, ITemplate template)
		{
			if (this._contentTemplateCollection == null)
			{
				this._contentTemplateCollection = new Hashtable(10, StringComparer.OrdinalIgnoreCase);
			}
			try
			{
				this._contentTemplateCollection.Add(templateName, template);
			}
			catch (ArgumentException)
			{
				throw new HttpException(SR.GetString("MasterPage_Multiple_content", new object[] { templateName }));
			}
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x000DE354 File Offset: 0x000DD354
		internal static MasterPage CreateMaster(TemplateControl owner, HttpContext context, VirtualPath masterPageFile, IDictionary contentTemplateCollection)
		{
			MasterPage masterPage = null;
			if (masterPageFile == null)
			{
				if (contentTemplateCollection != null && contentTemplateCollection.Count > 0)
				{
					throw new HttpException(SR.GetString("Content_only_allowed_in_content_page"));
				}
				return null;
			}
			else
			{
				VirtualPath virtualPath = VirtualPathProvider.CombineVirtualPathsInternal(owner.TemplateControlVirtualPath, masterPageFile);
				ITypedWebObjectFactory typedWebObjectFactory = (ITypedWebObjectFactory)BuildManager.GetVPathBuildResult(context, virtualPath);
				if (!typeof(MasterPage).IsAssignableFrom(typedWebObjectFactory.InstantiatedType))
				{
					throw new HttpException(SR.GetString("Invalid_master_base", new object[] { masterPageFile }));
				}
				masterPage = (MasterPage)typedWebObjectFactory.CreateInstance();
				masterPage.TemplateControlVirtualPath = virtualPath;
				if (owner.HasControls())
				{
					foreach (object obj in owner.Controls)
					{
						Control control = (Control)obj;
						LiteralControl literalControl = control as LiteralControl;
						if (literalControl == null || Util.FirstNonWhiteSpaceIndex(literalControl.Text) >= 0)
						{
							throw new HttpException(SR.GetString("Content_allowed_in_top_level_only"));
						}
					}
					owner.Controls.Clear();
				}
				if (owner.Controls.IsReadOnly)
				{
					throw new HttpException(SR.GetString("MasterPage_Cannot_ApplyTo_ReadOnly_Collection"));
				}
				if (contentTemplateCollection != null)
				{
					foreach (object obj2 in contentTemplateCollection.Keys)
					{
						string text = (string)obj2;
						if (!masterPage.ContentPlaceHolders.Contains(text.ToLower(CultureInfo.InvariantCulture)))
						{
							throw new HttpException(SR.GetString("MasterPage_doesnt_have_contentplaceholder", new object[] { text, masterPageFile }));
						}
					}
					masterPage._contentTemplates = contentTemplateCollection;
				}
				masterPage._ownerControl = owner;
				masterPage.InitializeAsUserControl(owner.Page);
				owner.Controls.Add(masterPage);
				return masterPage;
			}
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x000DE548 File Offset: 0x000DD548
		internal static void ApplyMasterRecursive(MasterPage master, IList appliedMasterFilePaths)
		{
			if (master.Master != null)
			{
				string text = master._masterPageFile.VirtualPathString.ToLower(CultureInfo.InvariantCulture);
				if (appliedMasterFilePaths.Contains(text))
				{
					throw new InvalidOperationException(SR.GetString("MasterPage_Circular_Master_Not_Allowed", new object[] { master._masterPageFile }));
				}
				appliedMasterFilePaths.Add(text);
				MasterPage.ApplyMasterRecursive(master.Master, appliedMasterFilePaths);
			}
			master._masterPageApplied = true;
		}

		// Token: 0x040023EA RID: 9194
		private VirtualPath _masterPageFile;

		// Token: 0x040023EB RID: 9195
		private MasterPage _master;

		// Token: 0x040023EC RID: 9196
		private IDictionary _contentTemplates;

		// Token: 0x040023ED RID: 9197
		private IDictionary _contentTemplateCollection;

		// Token: 0x040023EE RID: 9198
		private IList _contentPlaceHolders;

		// Token: 0x040023EF RID: 9199
		private bool _masterPageApplied;

		// Token: 0x040023F0 RID: 9200
		internal TemplateControl _ownerControl;
	}
}

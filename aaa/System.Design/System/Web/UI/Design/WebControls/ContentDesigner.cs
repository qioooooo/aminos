using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200040D RID: 1037
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ContentDesigner : ControlDesigner
	{
		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060025E0 RID: 9696 RVA: 0x000CBF5C File Offset: 0x000CAF5C
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new ContentDesigner.ContentDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x060025E1 RID: 9697 RVA: 0x000CBF89 File Offset: 0x000CAF89
		public override bool AllowResize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x060025E2 RID: 9698 RVA: 0x000CBF8C File Offset: 0x000CAF8C
		private IContentResolutionService ContentResolutionService
		{
			get
			{
				if (this._contentResolutionService == null)
				{
					this._contentResolutionService = (IContentResolutionService)this.GetService(typeof(IContentResolutionService));
				}
				return this._contentResolutionService;
			}
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x000CBFB7 File Offset: 0x000CAFB7
		private void ClearRegion()
		{
			if (this.ContentResolutionService != null && this.GetContentDefinition() != null)
			{
				this.ContentResolutionService.SetContentDesignerState(this.GetContentDefinition().ContentPlaceHolderID, ContentDesignerState.ShowDefaultContent);
			}
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x000CBFE0 File Offset: 0x000CAFE0
		private void CreateBlankContent()
		{
			if (this.ContentResolutionService != null && this.GetContentDefinition() != null)
			{
				this.ContentResolutionService.SetContentDesignerState(this.GetContentDefinition().ContentPlaceHolderID, ContentDesignerState.ShowUserContent);
			}
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000CC00C File Offset: 0x000CB00C
		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			EditableDesignerRegion editableDesignerRegion = new EditableDesignerRegion(this, "Content");
			regions.Add(editableDesignerRegion);
			Font captionFont = SystemFonts.CaptionFont;
			Color controlText = SystemColors.ControlText;
			Color control = SystemColors.Control;
			string text = base.Component.GetType().Name + " - " + base.Component.Site.Name;
			return string.Format(CultureInfo.InvariantCulture, "<table cellspacing=0 cellpadding=0 style=\"border:1px solid black; width:100%; height:200px\">\r\n            <tr>\r\n              <td style=\"width:100%; height:25px; font-family:Tahoma; font-size:{2}pt; color:{3}; background-color:{4}; padding:5px; border-bottom:1px solid black;\">\r\n                &nbsp;{0}\r\n              </td>\r\n            </tr>\r\n            <tr>\r\n              <td style=\"width:100%; height:175px; vertical-align:top;\" {1}=\"0\">\r\n              </td>\r\n            </tr>\r\n          </table>", new object[]
			{
				text,
				DesignerRegion.DesignerRegionAttributeName,
				captionFont.SizeInPoints,
				ColorTranslator.ToHtml(controlText),
				ColorTranslator.ToHtml(control)
			});
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000CC0B6 File Offset: 0x000CB0B6
		public override string GetPersistenceContent()
		{
			return this._content;
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000CC0C0 File Offset: 0x000CB0C0
		private ContentDefinition GetContentDefinition()
		{
			if (this._contentDefinition == null)
			{
				try
				{
					ContentDefinition contentDefinition = (ContentDefinition)this.ContentResolutionService.ContentDefinitions[((Content)base.Component).ContentPlaceHolderID];
					this._contentDefinition = new ContentDefinition(contentDefinition.ContentPlaceHolderID, contentDefinition.DefaultContent, contentDefinition.DefaultDesignTimeHtml);
				}
				catch
				{
				}
			}
			return this._contentDefinition;
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x000CC134 File Offset: 0x000CB134
		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			if (this._content == null)
			{
				this._content = base.Tag.GetContent();
			}
			if (this._content == null)
			{
				return string.Empty;
			}
			return this._content;
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x000CC163 File Offset: 0x000CB163
		protected override void PreFilterEvents(IDictionary events)
		{
			events.Clear();
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000CC16C File Offset: 0x000CB16C
		protected override void PostFilterProperties(IDictionary properties)
		{
			base.PostFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["ID"];
			PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)properties["ContentPlaceHolderID"];
			properties.Clear();
			ContentDesignerState contentDesignerState = ContentDesignerState.ShowDefaultContent;
			ContentDefinition contentDefinition = this.GetContentDefinition();
			if (this.ContentResolutionService != null && contentDefinition != null)
			{
				contentDesignerState = this.ContentResolutionService.GetContentDesignerState(contentDefinition.ContentPlaceHolderID);
			}
			propertyDescriptor = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { (contentDesignerState == ContentDesignerState.ShowDefaultContent) ? ReadOnlyAttribute.Yes : ReadOnlyAttribute.No });
			properties.Add("ID", propertyDescriptor);
			propertyDescriptor2 = TypeDescriptor.CreateProperty(propertyDescriptor2.ComponentType, propertyDescriptor2, new Attribute[] { ReadOnlyAttribute.Yes });
			properties.Add("ContentPlaceHolderID", propertyDescriptor2);
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000CC230 File Offset: 0x000CB230
		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
			if (string.Compare(this._content, content, StringComparison.Ordinal) != 0)
			{
				this._content = content;
				base.Tag.SetDirty(true);
			}
		}

		// Token: 0x040019F2 RID: 6642
		private const string _designtimeHTML = "<table cellspacing=0 cellpadding=0 style=\"border:1px solid black; width:100%; height:200px\">\r\n            <tr>\r\n              <td style=\"width:100%; height:25px; font-family:Tahoma; font-size:{2}pt; color:{3}; background-color:{4}; padding:5px; border-bottom:1px solid black;\">\r\n                &nbsp;{0}\r\n              </td>\r\n            </tr>\r\n            <tr>\r\n              <td style=\"width:100%; height:175px; vertical-align:top;\" {1}=\"0\">\r\n              </td>\r\n            </tr>\r\n          </table>";

		// Token: 0x040019F3 RID: 6643
		private const string _idProperty = "ID";

		// Token: 0x040019F4 RID: 6644
		private const string _contentPlaceHolderIDProperty = "ContentPlaceHolderID";

		// Token: 0x040019F5 RID: 6645
		private string _content;

		// Token: 0x040019F6 RID: 6646
		private ContentDefinition _contentDefinition;

		// Token: 0x040019F7 RID: 6647
		private IContentResolutionService _contentResolutionService;

		// Token: 0x0200040E RID: 1038
		private class ContentDesignerActionList : DesignerActionList
		{
			// Token: 0x060025ED RID: 9709 RVA: 0x000CC25C File Offset: 0x000CB25C
			public ContentDesignerActionList(ContentDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
			}

			// Token: 0x1700070F RID: 1807
			// (get) Token: 0x060025EE RID: 9710 RVA: 0x000CC271 File Offset: 0x000CB271
			// (set) Token: 0x060025EF RID: 9711 RVA: 0x000CC274 File Offset: 0x000CB274
			public override bool AutoShow
			{
				get
				{
					return true;
				}
				set
				{
				}
			}

			// Token: 0x060025F0 RID: 9712 RVA: 0x000CC276 File Offset: 0x000CB276
			public void ClearRegion()
			{
				this._parent.ClearRegion();
			}

			// Token: 0x060025F1 RID: 9713 RVA: 0x000CC283 File Offset: 0x000CB283
			public void CreateBlankContent()
			{
				this._parent.CreateBlankContent();
			}

			// Token: 0x060025F2 RID: 9714 RVA: 0x000CC290 File Offset: 0x000CB290
			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				ContentDesignerState contentDesignerState = ContentDesignerState.ShowDefaultContent;
				if (this._parent.ContentResolutionService != null && this._parent.GetContentDefinition() != null)
				{
					contentDesignerState = this._parent.ContentResolutionService.GetContentDesignerState(this._parent.GetContentDefinition().ContentPlaceHolderID);
				}
				if (contentDesignerState == ContentDesignerState.ShowDefaultContent)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "CreateBlankContent", SR.GetString("Content_CreateBlankContent"), string.Empty, string.Empty, true));
				}
				else if (ContentDesignerState.ShowUserContent == contentDesignerState)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ClearRegion", SR.GetString("Content_ClearRegion"), string.Empty, string.Empty, true));
				}
				return designerActionItemCollection;
			}

			// Token: 0x040019F8 RID: 6648
			private ContentDesigner _parent;
		}
	}
}

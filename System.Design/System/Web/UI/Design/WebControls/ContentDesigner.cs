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
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ContentDesigner : ControlDesigner
	{
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

		public override bool AllowResize
		{
			get
			{
				return true;
			}
		}

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

		private void ClearRegion()
		{
			if (this.ContentResolutionService != null && this.GetContentDefinition() != null)
			{
				this.ContentResolutionService.SetContentDesignerState(this.GetContentDefinition().ContentPlaceHolderID, ContentDesignerState.ShowDefaultContent);
			}
		}

		private void CreateBlankContent()
		{
			if (this.ContentResolutionService != null && this.GetContentDefinition() != null)
			{
				this.ContentResolutionService.SetContentDesignerState(this.GetContentDefinition().ContentPlaceHolderID, ContentDesignerState.ShowUserContent);
			}
		}

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

		public override string GetPersistenceContent()
		{
			return this._content;
		}

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

		protected override void PreFilterEvents(IDictionary events)
		{
			events.Clear();
		}

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

		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
			if (string.Compare(this._content, content, StringComparison.Ordinal) != 0)
			{
				this._content = content;
				base.Tag.SetDirty(true);
			}
		}

		private const string _designtimeHTML = "<table cellspacing=0 cellpadding=0 style=\"border:1px solid black; width:100%; height:200px\">\r\n            <tr>\r\n              <td style=\"width:100%; height:25px; font-family:Tahoma; font-size:{2}pt; color:{3}; background-color:{4}; padding:5px; border-bottom:1px solid black;\">\r\n                &nbsp;{0}\r\n              </td>\r\n            </tr>\r\n            <tr>\r\n              <td style=\"width:100%; height:175px; vertical-align:top;\" {1}=\"0\">\r\n              </td>\r\n            </tr>\r\n          </table>";

		private const string _idProperty = "ID";

		private const string _contentPlaceHolderIDProperty = "ContentPlaceHolderID";

		private string _content;

		private ContentDefinition _contentDefinition;

		private IContentResolutionService _contentResolutionService;

		private class ContentDesignerActionList : DesignerActionList
		{
			public ContentDesignerActionList(ContentDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
			}

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

			public void ClearRegion()
			{
				this._parent.ClearRegion();
			}

			public void CreateBlankContent()
			{
				this._parent.CreateBlankContent();
			}

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

			private ContentDesigner _parent;
		}
	}
}

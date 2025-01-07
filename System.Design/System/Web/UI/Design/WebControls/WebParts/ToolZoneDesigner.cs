using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ToolZoneDesigner : WebZoneDesigner
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new ToolZoneDesigner.ToolZoneDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		private protected bool ViewInBrowseMode
		{
			protected get
			{
				object obj = base.DesignerState["ViewInBrowseMode"];
				return obj != null && (bool)obj;
			}
			private set
			{
				if (value != this.ViewInBrowseMode)
				{
					base.DesignerState["ViewInBrowseMode"] = value;
					this.UpdateDesignTimeHtml();
				}
			}
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(ToolZone));
			base.Initialize(component);
		}

		private class ToolZoneDesignerActionList : DesignerActionList
		{
			public ToolZoneDesignerActionList(ToolZoneDesigner parent)
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

			public bool ViewInBrowseMode
			{
				get
				{
					return this._parent.ViewInBrowseMode;
				}
				set
				{
					this._parent.ViewInBrowseMode = value;
				}
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				return new DesignerActionItemCollection
				{
					new DesignerActionPropertyItem("ViewInBrowseMode", SR.GetString("ToolZoneDesigner_ViewInBrowseMode"), string.Empty, SR.GetString("ToolZoneDesigner_ViewInBrowseModeDesc"))
				};
			}

			private ToolZoneDesigner _parent;
		}
	}
}

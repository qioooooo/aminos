using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004AF RID: 1199
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SiteMapPathDesigner : ControlDesigner
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06002B6A RID: 11114 RVA: 0x000EF8D6 File Offset: 0x000EE8D6
		public override DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				if (this._autoFormats == null)
				{
					this._autoFormats = ControlDesigner.CreateAutoFormats("<Schemes>\r\n        <xsd:schema id=\"Schemes\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n          <xsd:element name=\"Scheme\">\r\n            <xsd:complexType>\r\n              <xsd:all>\r\n                <xsd:element name=\"SchemeName\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FontName\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PathSeparator\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"NodeStyleFontBold\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"NodeStyleForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"RootNodeStyleFontBold\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"RootNodeStyleForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"CurrentNodeStyleForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PathSeparatorStyleForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PathSeparatorStyleFontBold\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n              </xsd:all>\r\n            </xsd:complexType>\r\n          </xsd:element>\r\n          <xsd:element name=\"Schemes\" msdata:IsDataSet=\"true\">\r\n            <xsd:complexType>\r\n              <xsd:choice maxOccurs=\"unbounded\">\r\n                <xsd:element ref=\"Scheme\"/>\r\n              </xsd:choice>\r\n            </xsd:complexType>\r\n          </xsd:element>\r\n        </xsd:schema>\r\n        <Scheme>\r\n          <SchemeName>SiteMapPathAFmt_Scheme_Default</SchemeName>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>SiteMapPathAFmt_Scheme_Colorful</SchemeName>\r\n          <FontName>Verdana</FontName>\r\n          <FontSize>0.8em</FontSize>\r\n          <PathSeparator> : </PathSeparator>\r\n          <NodeStyleFontBold>True</NodeStyleFontBold>\r\n          <NodeStyleForeColor>#990000</NodeStyleForeColor>\r\n          <RootNodeStyleFontBold>True</RootNodeStyleFontBold>\r\n          <RootNodeStyleForeColor>#FF8000</RootNodeStyleForeColor>\r\n          <CurrentNodeStyleForeColor>#333333</CurrentNodeStyleForeColor>\r\n          <PathSeparatorStyleFontBold>True</PathSeparatorStyleFontBold>\r\n          <PathSeparatorStyleForeColor>#990000</PathSeparatorStyleForeColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>SiteMapPathAFmt_Scheme_Simple</SchemeName>\r\n          <FontName>Verdana</FontName>\r\n          <FontSize>0.8em</FontSize>\r\n          <PathSeparator> : </PathSeparator>\r\n          <NodeStyleFontBold>True</NodeStyleFontBold>\r\n          <NodeStyleForeColor>#666666</NodeStyleForeColor>\r\n          <RootNodeStyleFontBold>True</RootNodeStyleFontBold>\r\n          <RootNodeStyleForeColor>#1C5E55</RootNodeStyleForeColor>\r\n          <CurrentNodeStyleForeColor>#333333</CurrentNodeStyleForeColor>\r\n          <PathSeparatorStyleFontBold>True</PathSeparatorStyleFontBold>\r\n          <PathSeparatorStyleForeColor>#1C5E55</PathSeparatorStyleForeColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>SiteMapPathAFmt_Scheme_Professional</SchemeName>\r\n          <FontName>Verdana</FontName>\r\n          <FontSize>0.8em</FontSize>\r\n          <PathSeparator> : </PathSeparator>\r\n          <NodeStyleFontBold>True</NodeStyleFontBold>\r\n          <NodeStyleForeColor>#7C6F57</NodeStyleForeColor>\r\n          <RootNodeStyleFontBold>True</RootNodeStyleFontBold>\r\n          <RootNodeStyleForeColor>#5D7B9D</RootNodeStyleForeColor>\r\n          <CurrentNodeStyleForeColor>#333333</CurrentNodeStyleForeColor>\r\n          <PathSeparatorStyleFontBold>True</PathSeparatorStyleFontBold>\r\n          <PathSeparatorStyleForeColor>#5D7B9D</PathSeparatorStyleForeColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>SiteMapPathAFmt_Scheme_Classic</SchemeName>\r\n          <FontName>Verdana</FontName>\r\n          <FontSize>0.8em</FontSize>\r\n          <PathSeparator> : </PathSeparator>\r\n          <NodeStyleFontBold>True</NodeStyleFontBold>\r\n          <NodeStyleForeColor>#284E98</NodeStyleForeColor>\r\n          <RootNodeStyleFontBold>True</RootNodeStyleFontBold>\r\n          <RootNodeStyleForeColor>#507CD1</RootNodeStyleForeColor>\r\n          <CurrentNodeStyleForeColor>#333333</CurrentNodeStyleForeColor>\r\n          <PathSeparatorStyleFontBold>True</PathSeparatorStyleFontBold>\r\n          <PathSeparatorStyleForeColor>#507CD1</PathSeparatorStyleForeColor>\r\n        </Scheme>\r\n      </Schemes>", (DataRow schemeData) => new SiteMapPathAutoFormat(schemeData));
				}
				return this._autoFormats;
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06002B6B RID: 11115 RVA: 0x000EF914 File Offset: 0x000EE914
		private SiteMapProvider DesignTimeSiteMapProvider
		{
			get
			{
				if (this._siteMapProvider == null)
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					this._siteMapProvider = new DesignTimeSiteMapProvider(designerHost);
				}
				return this._siteMapProvider;
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002B6C RID: 11116 RVA: 0x000EF954 File Offset: 0x000EE954
		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection templateGroups = base.TemplateGroups;
				for (int i = 0; i < SiteMapPathDesigner._controlTemplateNames.Length; i++)
				{
					string text = SiteMapPathDesigner._controlTemplateNames[i];
					TemplateGroup templateGroup = new TemplateGroup(text);
					templateGroup.AddTemplateDefinition(new TemplateDefinition(this, text, base.Component, text, this.TemplateStyleArray[i]));
					templateGroups.Add(templateGroup);
				}
				return templateGroups;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002B6D RID: 11117 RVA: 0x000EF9B0 File Offset: 0x000EE9B0
		private Style[] TemplateStyleArray
		{
			get
			{
				if (SiteMapPathDesigner._templateStyleArray == null)
				{
					SiteMapPathDesigner._templateStyleArray = new Style[]
					{
						((SiteMapPath)base.ViewControl).NodeStyle,
						((SiteMapPath)base.ViewControl).CurrentNodeStyle,
						((SiteMapPath)base.ViewControl).RootNodeStyle,
						((SiteMapPath)base.ViewControl).PathSeparatorStyle
					};
				}
				return SiteMapPathDesigner._templateStyleArray;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06002B6E RID: 11118 RVA: 0x000EFA22 File Offset: 0x000EEA22
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x000EFA28 File Offset: 0x000EEA28
		public override string GetDesignTimeHtml()
		{
			string text = null;
			SiteMapPath siteMapPath = (SiteMapPath)base.ViewControl;
			try
			{
				siteMapPath.Provider = this.DesignTimeSiteMapProvider;
				ICompositeControlDesignerAccessor compositeControlDesignerAccessor = siteMapPath;
				compositeControlDesignerAccessor.RecreateChildControls();
				text = base.GetDesignTimeHtml();
			}
			catch (Exception ex)
			{
				text = this.GetErrorDesignTimeHtml(ex);
			}
			return text;
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x000EFA7C File Offset: 0x000EEA7C
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRendering") + e.Message);
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x000EFA99 File Offset: 0x000EEA99
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(SiteMapPath));
			base.Initialize(component);
			this._navigationPath = (SiteMapPath)component;
			if (base.View != null)
			{
				base.View.SetFlags(ViewFlags.TemplateEditing, true);
			}
		}

		// Token: 0x04001D7E RID: 7550
		private SiteMapPath _navigationPath;

		// Token: 0x04001D7F RID: 7551
		private SiteMapProvider _siteMapProvider;

		// Token: 0x04001D80 RID: 7552
		private DesignerAutoFormatCollection _autoFormats;

		// Token: 0x04001D81 RID: 7553
		private static string[] _controlTemplateNames = new string[] { "NodeTemplate", "CurrentNodeTemplate", "RootNodeTemplate", "PathSeparatorTemplate" };

		// Token: 0x04001D82 RID: 7554
		private static Style[] _templateStyleArray;

		// Token: 0x04001D83 RID: 7555
		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;
	}
}

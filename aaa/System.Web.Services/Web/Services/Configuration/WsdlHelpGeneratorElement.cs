using System;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Xml;

namespace System.Web.Services.Configuration
{
	// Token: 0x0200013E RID: 318
	public sealed class WsdlHelpGeneratorElement : ConfigurationElement
	{
		// Token: 0x060009F1 RID: 2545 RVA: 0x0004732D File Offset: 0x0004632D
		public WsdlHelpGeneratorElement()
		{
			this.properties.Add(this.href);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0004736D File Offset: 0x0004636D
		[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
		private string GetConfigurationDirectory()
		{
			return HttpRuntime.MachineConfigurationDirectory;
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x00047374 File Offset: 0x00046374
		internal string HelpGeneratorVirtualPath
		{
			get
			{
				return this.virtualPath + this.Href;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x00047387 File Offset: 0x00046387
		internal string HelpGeneratorPath
		{
			get
			{
				return Path.Combine(this.actualPath, this.Href);
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x0004739A File Offset: 0x0004639A
		// (set) Token: 0x060009F6 RID: 2550 RVA: 0x000473AD File Offset: 0x000463AD
		[ConfigurationProperty("href", IsRequired = true)]
		public string Href
		{
			get
			{
				return (string)base[this.href];
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (this.needToValidateHref && value.Length > 0)
				{
					WsdlHelpGeneratorElement.CheckIOReadPermission(this.actualPath, value);
				}
				base[this.href] = value;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060009F7 RID: 2551 RVA: 0x000473E3 File Offset: 0x000463E3
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x000473EC File Offset: 0x000463EC
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			base.DeserializeElement(reader, serializeCollectionKey);
			ContextInformation evaluationContext = base.EvaluationContext;
			WebContext webContext = evaluationContext.HostingContext as WebContext;
			if (webContext == null)
			{
				return;
			}
			if (this.Href.Length == 0)
			{
				return;
			}
			string text = webContext.Path;
			string text2;
			if (text == null)
			{
				text = HostingEnvironment.ApplicationVirtualPath;
				if (text == null)
				{
					text = "";
				}
				text2 = this.GetConfigurationDirectory();
			}
			else
			{
				text2 = HostingEnvironment.MapPath(text);
			}
			if (!text.EndsWith("/", StringComparison.Ordinal))
			{
				text += "/";
			}
			WsdlHelpGeneratorElement.CheckIOReadPermission(text2, this.Href);
			this.actualPath = text2;
			this.virtualPath = text;
			this.needToValidateHref = true;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0004748C File Offset: 0x0004648C
		protected override void Reset(ConfigurationElement parentElement)
		{
			WsdlHelpGeneratorElement wsdlHelpGeneratorElement = (WsdlHelpGeneratorElement)parentElement;
			ContextInformation evaluationContext = base.EvaluationContext;
			WebContext webContext = evaluationContext.HostingContext as WebContext;
			if (webContext != null)
			{
				string text = webContext.Path;
				bool flag = text == null;
				this.actualPath = wsdlHelpGeneratorElement.actualPath;
				if (flag)
				{
					text = HostingEnvironment.ApplicationVirtualPath;
				}
				if (text != null && !text.EndsWith("/", StringComparison.Ordinal))
				{
					text += "/";
				}
				if (text == null && parentElement != null)
				{
					this.virtualPath = wsdlHelpGeneratorElement.virtualPath;
				}
				else if (text != null)
				{
					this.virtualPath = text;
				}
			}
			base.Reset(parentElement);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0004751C File Offset: 0x0004651C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void SetDefaults()
		{
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				this.virtualPath = HostingEnvironment.ApplicationVirtualPath;
			}
			this.actualPath = this.GetConfigurationDirectory();
			if (this.virtualPath != null && !this.virtualPath.EndsWith("/", StringComparison.Ordinal))
			{
				this.virtualPath += "/";
			}
			if (this.actualPath != null && !this.actualPath.EndsWith("\\", StringComparison.Ordinal))
			{
				this.actualPath += "\\";
			}
			this.Href = "DefaultWsdlHelpGenerator.aspx";
			WsdlHelpGeneratorElement.CheckIOReadPermission(this.actualPath, this.Href);
			this.needToValidateHref = true;
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x000475D0 File Offset: 0x000465D0
		private static void CheckIOReadPermission(string path, string file)
		{
			if (path == null)
			{
				return;
			}
			string fullPath = Path.GetFullPath(Path.Combine(path, file));
			new FileIOPermission(FileIOPermissionAccess.Read, fullPath).Demand();
		}

		// Token: 0x04000632 RID: 1586
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04000633 RID: 1587
		private readonly ConfigurationProperty href = new ConfigurationProperty("href", typeof(string), null, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04000634 RID: 1588
		private string virtualPath;

		// Token: 0x04000635 RID: 1589
		private string actualPath;

		// Token: 0x04000636 RID: 1590
		private bool needToValidateHref;
	}
}

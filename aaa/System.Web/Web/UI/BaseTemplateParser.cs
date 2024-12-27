using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Web.Compilation;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000387 RID: 903
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class BaseTemplateParser : TemplateParser
	{
		// Token: 0x06002C32 RID: 11314 RVA: 0x000C5894 File Offset: 0x000C4894
		internal Type GetDesignTimeUserControlType(string tagPrefix, string tagName)
		{
			Type type = typeof(UserControl);
			IDesignerHost designerHost = base.DesignerHost;
			if (designerHost != null)
			{
				IUserControlTypeResolutionService userControlTypeResolutionService = (IUserControlTypeResolutionService)designerHost.GetService(typeof(IUserControlTypeResolutionService));
				if (userControlTypeResolutionService != null)
				{
					try
					{
						type = userControlTypeResolutionService.GetType(tagPrefix, tagName);
					}
					catch
					{
					}
				}
			}
			return type;
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x000C58F0 File Offset: 0x000C48F0
		protected internal Type GetUserControlType(string virtualPath)
		{
			return this.GetUserControlType(VirtualPath.Create(virtualPath));
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x000C5900 File Offset: 0x000C4900
		internal Type GetUserControlType(VirtualPath virtualPath)
		{
			Type type = this.GetReferencedType(virtualPath, false);
			if (type == null)
			{
				if (this._pageParserFilter != null)
				{
					type = this._pageParserFilter.GetNoCompileUserControlType();
				}
				if (type == null)
				{
					base.ProcessError(SR.GetString("Cant_use_nocompile_uc", new object[] { virtualPath }));
				}
			}
			else
			{
				Util.CheckAssignableType(typeof(UserControl), type);
			}
			return type;
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x000C595F File Offset: 0x000C495F
		protected Type GetReferencedType(string virtualPath)
		{
			return this.GetReferencedType(VirtualPath.Create(virtualPath));
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x000C596D File Offset: 0x000C496D
		internal Type GetReferencedType(VirtualPath virtualPath)
		{
			return this.GetReferencedType(virtualPath, true);
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x000C5978 File Offset: 0x000C4978
		internal Type GetReferencedType(VirtualPath virtualPath, bool allowNoCompile)
		{
			virtualPath = base.ResolveVirtualPath(virtualPath);
			if (this._pageParserFilter != null && !this._pageParserFilter.AllowVirtualReference(base.CompConfig, virtualPath))
			{
				base.ProcessError(SR.GetString("Reference_not_allowed", new object[] { virtualPath }));
			}
			BuildResult buildResult = null;
			try
			{
				buildResult = BuildManager.GetVPathBuildResult(virtualPath);
			}
			catch (HttpCompileException ex)
			{
				if (ex.VirtualPathDependencies != null)
				{
					foreach (object obj in ex.VirtualPathDependencies)
					{
						string text = (string)obj;
						base.AddSourceDependency(VirtualPath.Create(text));
					}
				}
				throw;
			}
			catch
			{
				if (this.IgnoreParseErrors)
				{
					base.AddSourceDependency(virtualPath);
				}
				throw;
			}
			BuildResultNoCompileTemplateControl buildResultNoCompileTemplateControl = buildResult as BuildResultNoCompileTemplateControl;
			Type type;
			if (buildResultNoCompileTemplateControl != null)
			{
				if (!allowNoCompile)
				{
					return null;
				}
				type = buildResultNoCompileTemplateControl.BaseType;
			}
			else
			{
				if (!(buildResult is BuildResultCompiledType))
				{
					throw new HttpException(SR.GetString("Invalid_typeless_reference", new object[] { "src" }));
				}
				BuildResultCompiledType buildResultCompiledType = (BuildResultCompiledType)buildResult;
				type = buildResultCompiledType.ResultType;
			}
			base.AddTypeDependency(type);
			base.AddBuildResultDependency(buildResult);
			return type;
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x000C5ACC File Offset: 0x000C4ACC
		internal override void ProcessDirective(string directiveName, IDictionary directive)
		{
			if (StringUtil.EqualsIgnoreCase(directiveName, "register"))
			{
				string andRemoveNonEmptyIdentifierAttribute = Util.GetAndRemoveNonEmptyIdentifierAttribute(directive, "tagprefix", true);
				string andRemoveNonEmptyIdentifierAttribute2 = Util.GetAndRemoveNonEmptyIdentifierAttribute(directive, "tagname", false);
				VirtualPath andRemoveVirtualPathAttribute = Util.GetAndRemoveVirtualPathAttribute(directive, "src", false);
				string andRemoveNonEmptyNoSpaceAttribute = Util.GetAndRemoveNonEmptyNoSpaceAttribute(directive, "namespace", false);
				string andRemoveNonEmptyAttribute = Util.GetAndRemoveNonEmptyAttribute(directive, "assembly", false);
				RegisterDirectiveEntry registerDirectiveEntry;
				if (andRemoveNonEmptyIdentifierAttribute2 != null)
				{
					if (andRemoveVirtualPathAttribute == null)
					{
						throw new HttpException(SR.GetString("Missing_attr", new object[] { "src" }));
					}
					if (andRemoveNonEmptyNoSpaceAttribute != null)
					{
						throw new HttpException(SR.GetString("Invalid_attr", new object[] { "namespace", "tagname" }));
					}
					if (andRemoveNonEmptyAttribute != null)
					{
						throw new HttpException(SR.GetString("Invalid_attr", new object[] { "assembly", "tagname" }));
					}
					UserControlRegisterEntry userControlRegisterEntry = new UserControlRegisterEntry(andRemoveNonEmptyIdentifierAttribute, andRemoveNonEmptyIdentifierAttribute2);
					userControlRegisterEntry.UserControlSource = andRemoveVirtualPathAttribute;
					registerDirectiveEntry = userControlRegisterEntry;
					base.TypeMapper.ProcessUserControlRegistration(userControlRegisterEntry);
				}
				else
				{
					if (andRemoveVirtualPathAttribute != null)
					{
						throw new HttpException(SR.GetString("Missing_attr", new object[] { "tagname" }));
					}
					if (andRemoveNonEmptyNoSpaceAttribute == null)
					{
						throw new HttpException(SR.GetString("Missing_attr", new object[] { "namespace" }));
					}
					TagNamespaceRegisterEntry tagNamespaceRegisterEntry = new TagNamespaceRegisterEntry(andRemoveNonEmptyIdentifierAttribute, andRemoveNonEmptyNoSpaceAttribute, andRemoveNonEmptyAttribute);
					registerDirectiveEntry = tagNamespaceRegisterEntry;
					base.TypeMapper.ProcessTagNamespaceRegistration(tagNamespaceRegisterEntry);
				}
				registerDirectiveEntry.Line = this._lineNumber;
				registerDirectiveEntry.VirtualPath = base.CurrentVirtualPathString;
				Util.CheckUnknownDirectiveAttributes(directiveName, directive);
				return;
			}
			base.ProcessDirective(directiveName, directive);
		}

		// Token: 0x04002080 RID: 8320
		private const string _sourceString = "src";

		// Token: 0x04002081 RID: 8321
		private const string _namespaceString = "namespace";

		// Token: 0x04002082 RID: 8322
		private const string _tagnameString = "tagname";
	}
}

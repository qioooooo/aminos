using System;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Web.RegularExpressions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000382 RID: 898
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class BaseParser
	{
		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06002B90 RID: 11152 RVA: 0x000C2007 File Offset: 0x000C1007
		internal VirtualPath BaseVirtualDir
		{
			get
			{
				return this._baseVirtualDir;
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06002B91 RID: 11153 RVA: 0x000C200F File Offset: 0x000C100F
		// (set) Token: 0x06002B92 RID: 11154 RVA: 0x000C2017 File Offset: 0x000C1017
		internal VirtualPath CurrentVirtualPath
		{
			get
			{
				return this._currentVirtualPath;
			}
			set
			{
				this._currentVirtualPath = value;
				if (value == null)
				{
					return;
				}
				this._baseVirtualDir = value.Parent;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06002B93 RID: 11155 RVA: 0x000C2036 File Offset: 0x000C1036
		internal string CurrentVirtualPathString
		{
			get
			{
				return VirtualPath.GetVirtualPathString(this.CurrentVirtualPath);
			}
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x000C2043 File Offset: 0x000C1043
		internal VirtualPath ResolveVirtualPath(VirtualPath virtualPath)
		{
			return VirtualPathProvider.CombineVirtualPathsInternal(this.CurrentVirtualPath, virtualPath);
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06002B95 RID: 11157 RVA: 0x000C2051 File Offset: 0x000C1051
		internal static Regex TagRegex
		{
			get
			{
				if (BaseParser._tagRegex == null)
				{
					if (AppSettings.UseStrictParserRegex)
					{
						BaseParser._tagRegex = (Regex)BaseParser.CreateInstanceNoException("System.Web.RegularExpressions.TagRegex40");
					}
					if (BaseParser._tagRegex == null)
					{
						BaseParser._tagRegex = new TagRegex();
					}
				}
				return BaseParser._tagRegex;
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06002B96 RID: 11158 RVA: 0x000C208B File Offset: 0x000C108B
		internal static Regex DirectiveRegex
		{
			get
			{
				if (BaseParser._directiveRegex == null)
				{
					if (AppSettings.UseStrictParserRegex)
					{
						BaseParser._directiveRegex = (Regex)BaseParser.CreateInstanceNoException("System.Web.RegularExpressions.DirectiveRegex40");
					}
					if (BaseParser._directiveRegex == null)
					{
						BaseParser._directiveRegex = new DirectiveRegex();
					}
				}
				return BaseParser._directiveRegex;
			}
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000C20C8 File Offset: 0x000C10C8
		internal static object CreateInstanceNoException(string typeName)
		{
			object obj = null;
			try
			{
				Type type = typeof(TagRegex).Assembly.GetType(typeName);
				if (type != null)
				{
					obj = Activator.CreateInstance(type);
				}
			}
			catch
			{
			}
			return obj;
		}

		// Token: 0x04002025 RID: 8229
		private VirtualPath _baseVirtualDir;

		// Token: 0x04002026 RID: 8230
		private VirtualPath _currentVirtualPath;

		// Token: 0x04002027 RID: 8231
		private static Regex _tagRegex;

		// Token: 0x04002028 RID: 8232
		private static Regex _directiveRegex;

		// Token: 0x04002029 RID: 8233
		internal static readonly Regex endtagRegex = new EndTagRegex();

		// Token: 0x0400202A RID: 8234
		internal static readonly Regex aspCodeRegex = new AspCodeRegex();

		// Token: 0x0400202B RID: 8235
		internal static readonly Regex aspExprRegex = new AspExprRegex();

		// Token: 0x0400202C RID: 8236
		internal static readonly Regex databindExprRegex = new DatabindExprRegex();

		// Token: 0x0400202D RID: 8237
		internal static readonly Regex commentRegex = new CommentRegex();

		// Token: 0x0400202E RID: 8238
		internal static readonly Regex includeRegex = new IncludeRegex();

		// Token: 0x0400202F RID: 8239
		internal static readonly Regex textRegex = new TextRegex();

		// Token: 0x04002030 RID: 8240
		internal static readonly Regex gtRegex = new GTRegex();

		// Token: 0x04002031 RID: 8241
		internal static readonly Regex ltRegex = new LTRegex();

		// Token: 0x04002032 RID: 8242
		internal static readonly Regex serverTagsRegex = new ServerTagsRegex();

		// Token: 0x04002033 RID: 8243
		internal static readonly Regex runatServerRegex = new RunatServerRegex();
	}
}

using System;
using System.Collections;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x0200042D RID: 1069
	internal sealed class MasterPageParser : UserControlParser
	{
		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06003350 RID: 13136 RVA: 0x000DED3C File Offset: 0x000DDD3C
		internal override Type DefaultBaseType
		{
			get
			{
				return typeof(MasterPage);
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06003351 RID: 13137 RVA: 0x000DED48 File Offset: 0x000DDD48
		internal override string DefaultDirectiveName
		{
			get
			{
				return "master";
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06003352 RID: 13138 RVA: 0x000DED4F File Offset: 0x000DDD4F
		internal override Type DefaultFileLevelBuilderType
		{
			get
			{
				return typeof(FileLevelMasterPageControlBuilder);
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06003353 RID: 13139 RVA: 0x000DED5B File Offset: 0x000DDD5B
		internal Type MasterPageType
		{
			get
			{
				return this._masterPageType;
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06003354 RID: 13140 RVA: 0x000DED63 File Offset: 0x000DDD63
		internal CaseInsensitiveStringSet PlaceHolderList
		{
			get
			{
				if (this._placeHolderList == null)
				{
					this._placeHolderList = new CaseInsensitiveStringSet();
				}
				return this._placeHolderList;
			}
		}

		// Token: 0x06003355 RID: 13141 RVA: 0x000DED7E File Offset: 0x000DDD7E
		internal override void ApplyBaseType()
		{
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x000DED80 File Offset: 0x000DDD80
		internal override RootBuilder CreateDefaultFileLevelBuilder()
		{
			return new FileLevelMasterPageControlBuilder();
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x000DED88 File Offset: 0x000DDD88
		internal override void ProcessDirective(string directiveName, IDictionary directive)
		{
			if (StringUtil.EqualsIgnoreCase(directiveName, "masterType"))
			{
				if (this._masterPageType != null)
				{
					base.ProcessError(SR.GetString("Only_one_directive_allowed", new object[] { directiveName }));
					return;
				}
				this._masterPageType = base.GetDirectiveType(directive, directiveName);
				Util.CheckAssignableType(typeof(MasterPage), this._masterPageType);
				return;
			}
			else
			{
				if (StringUtil.EqualsIgnoreCase(directiveName, "outputcache"))
				{
					base.ProcessError(SR.GetString("Directive_not_allowed", new object[] { directiveName }));
					return;
				}
				base.ProcessDirective(directiveName, directive);
				return;
			}
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x000DEE20 File Offset: 0x000DDE20
		internal override bool ProcessMainDirectiveAttribute(string deviceName, string name, string value, IDictionary parseData)
		{
			if (name != null)
			{
				if (!(name == "masterpagefile"))
				{
					if (name == "outputcaching")
					{
						base.ProcessError(SR.GetString("Attr_not_supported_in_directive", new object[] { name, this.DefaultDirectiveName }));
						return false;
					}
				}
				else
				{
					if (base.IsExpressionBuilderValue(value))
					{
						return false;
					}
					if (value.Length > 0)
					{
						Type referencedType = base.GetReferencedType(value);
						Util.CheckAssignableType(typeof(MasterPage), referencedType);
					}
					return false;
				}
			}
			return base.ProcessMainDirectiveAttribute(deviceName, name, value, parseData);
		}

		// Token: 0x040023F5 RID: 9205
		internal new const string defaultDirectiveName = "master";

		// Token: 0x040023F6 RID: 9206
		private Type _masterPageType;

		// Token: 0x040023F7 RID: 9207
		private CaseInsensitiveStringSet _placeHolderList;
	}
}

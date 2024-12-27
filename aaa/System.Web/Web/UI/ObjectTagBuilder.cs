using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000436 RID: 1078
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ObjectTagBuilder : ControlBuilder
	{
		// Token: 0x0600339E RID: 13214 RVA: 0x000E0B2C File Offset: 0x000DFB2C
		public override void Init(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string id, IDictionary attribs)
		{
			if (id == null)
			{
				throw new HttpException(SR.GetString("Object_tag_must_have_id"));
			}
			base.ID = id;
			string text = (string)attribs["scope"];
			if (text == null)
			{
				this._scope = ObjectTagScope.Default;
			}
			else if (StringUtil.EqualsIgnoreCase(text, "page"))
			{
				this._scope = ObjectTagScope.Page;
			}
			else if (StringUtil.EqualsIgnoreCase(text, "session"))
			{
				this._scope = ObjectTagScope.Session;
			}
			else if (StringUtil.EqualsIgnoreCase(text, "application"))
			{
				this._scope = ObjectTagScope.Application;
			}
			else
			{
				if (!StringUtil.EqualsIgnoreCase(text, "appinstance"))
				{
					throw new HttpException(SR.GetString("Invalid_scope", new object[] { text }));
				}
				this._scope = ObjectTagScope.AppInstance;
			}
			Util.GetAndRemoveBooleanAttribute(attribs, "latebinding", ref this._fLateBinding);
			string text2 = (string)attribs["class"];
			if (text2 != null)
			{
				this._type = parser.GetType(text2);
			}
			if (this._type == null)
			{
				text2 = (string)attribs["classid"];
				if (text2 != null)
				{
					Guid guid = new Guid(text2);
					this._type = Type.GetTypeFromCLSID(guid);
					if (this._type == null)
					{
						throw new HttpException(SR.GetString("Invalid_clsid", new object[] { text2 }));
					}
					if (this._fLateBinding || Util.IsLateBoundComClassicType(this._type))
					{
						this._lateBound = true;
						this._clsid = text2;
					}
					else
					{
						parser.AddTypeDependency(this._type);
					}
				}
			}
			if (this._type == null)
			{
				text2 = (string)attribs["progid"];
				if (text2 != null)
				{
					this._type = Type.GetTypeFromProgID(text2);
					if (this._type == null)
					{
						throw new HttpException(SR.GetString("Invalid_progid", new object[] { text2 }));
					}
					if (this._fLateBinding || Util.IsLateBoundComClassicType(this._type))
					{
						this._lateBound = true;
						this._progid = text2;
					}
					else
					{
						parser.AddTypeDependency(this._type);
					}
				}
			}
			if (this._type == null)
			{
				throw new HttpException(SR.GetString("Object_tag_must_have_class_classid_or_progid"));
			}
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x000E0D41 File Offset: 0x000DFD41
		public override void AppendSubBuilder(ControlBuilder subBuilder)
		{
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x000E0D43 File Offset: 0x000DFD43
		public override void AppendLiteralString(string s)
		{
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x060033A1 RID: 13217 RVA: 0x000E0D45 File Offset: 0x000DFD45
		internal ObjectTagScope Scope
		{
			get
			{
				return this._scope;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x060033A2 RID: 13218 RVA: 0x000E0D4D File Offset: 0x000DFD4D
		internal Type ObjectType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x060033A3 RID: 13219 RVA: 0x000E0D55 File Offset: 0x000DFD55
		internal bool LateBound
		{
			get
			{
				return this._lateBound;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x060033A4 RID: 13220 RVA: 0x000E0D5D File Offset: 0x000DFD5D
		internal Type DeclaredType
		{
			get
			{
				if (!this._lateBound)
				{
					return this.ObjectType;
				}
				return typeof(object);
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x060033A5 RID: 13221 RVA: 0x000E0D78 File Offset: 0x000DFD78
		internal string Progid
		{
			get
			{
				return this._progid;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x060033A6 RID: 13222 RVA: 0x000E0D80 File Offset: 0x000DFD80
		internal string Clsid
		{
			get
			{
				return this._clsid;
			}
		}

		// Token: 0x04002437 RID: 9271
		private ObjectTagScope _scope;

		// Token: 0x04002438 RID: 9272
		private Type _type;

		// Token: 0x04002439 RID: 9273
		private bool _lateBound;

		// Token: 0x0400243A RID: 9274
		private string _progid;

		// Token: 0x0400243B RID: 9275
		private string _clsid;

		// Token: 0x0400243C RID: 9276
		private bool _fLateBinding;
	}
}

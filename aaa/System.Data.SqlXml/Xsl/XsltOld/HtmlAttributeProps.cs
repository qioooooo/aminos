using System;
using System.Collections;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200017B RID: 379
	internal class HtmlAttributeProps
	{
		// Token: 0x06000F88 RID: 3976 RVA: 0x0004DD24 File Offset: 0x0004CD24
		public static HtmlAttributeProps Create(bool abr, bool uri, bool name)
		{
			return new HtmlAttributeProps
			{
				abr = abr,
				uri = uri,
				name = name
			};
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0004DD4D File Offset: 0x0004CD4D
		public bool Abr
		{
			get
			{
				return this.abr;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x0004DD55 File Offset: 0x0004CD55
		public bool Uri
		{
			get
			{
				return this.uri;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0004DD5D File Offset: 0x0004CD5D
		public bool Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0004DD68 File Offset: 0x0004CD68
		public static HtmlAttributeProps GetProps(string name)
		{
			return (HtmlAttributeProps)HtmlAttributeProps.s_table[name];
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0004DD88 File Offset: 0x0004CD88
		private static Hashtable CreatePropsTable()
		{
			bool flag = false;
			bool flag2 = true;
			return new Hashtable(26, StringComparer.OrdinalIgnoreCase)
			{
				{
					"action",
					HtmlAttributeProps.Create(flag, flag2, flag)
				},
				{
					"checked",
					HtmlAttributeProps.Create(flag2, flag, flag)
				},
				{
					"cite",
					HtmlAttributeProps.Create(flag, flag2, flag)
				},
				{
					"classid",
					HtmlAttributeProps.Create(flag, flag2, flag)
				},
				{
					"codebase",
					HtmlAttributeProps.Create(flag, flag2, flag)
				},
				{
					"compact",
					HtmlAttributeProps.Create(flag2, flag, flag)
				},
				{
					"data",
					HtmlAttributeProps.Create(flag, flag2, flag)
				},
				{
					"datasrc",
					HtmlAttributeProps.Create(flag, flag2, flag)
				},
				{
					"declare",
					HtmlAttributeProps.Create(flag2, flag, flag)
				},
				{
					"defer",
					HtmlAttributeProps.Create(flag2, flag, flag)
				},
				{
					"disabled",
					HtmlAttributeProps.Create(flag2, flag, flag)
				},
				{
					"for",
					HtmlAttributeProps.Create(flag, flag2, flag)
				},
				{
					"href",
					HtmlAttributeProps.Create(flag, flag2, flag)
				},
				{
					"ismap",
					HtmlAttributeProps.Create(flag2, flag, flag)
				},
				{
					"longdesc",
					HtmlAttributeProps.Create(flag, flag2, flag)
				},
				{
					"multiple",
					HtmlAttributeProps.Create(flag2, flag, flag)
				},
				{
					"name",
					HtmlAttributeProps.Create(flag, flag, flag2)
				},
				{
					"nohref",
					HtmlAttributeProps.Create(flag2, flag, flag)
				},
				{
					"noresize",
					HtmlAttributeProps.Create(flag2, flag, flag)
				},
				{
					"noshade",
					HtmlAttributeProps.Create(flag2, flag, flag)
				},
				{
					"nowrap",
					HtmlAttributeProps.Create(flag2, flag, flag)
				},
				{
					"profile",
					HtmlAttributeProps.Create(flag, flag2, flag)
				},
				{
					"readonly",
					HtmlAttributeProps.Create(flag2, flag, flag)
				},
				{
					"selected",
					HtmlAttributeProps.Create(flag2, flag, flag)
				},
				{
					"src",
					HtmlAttributeProps.Create(flag, flag2, flag)
				},
				{
					"usemap",
					HtmlAttributeProps.Create(flag, flag2, flag)
				}
			};
		}

		// Token: 0x040009FE RID: 2558
		private bool abr;

		// Token: 0x040009FF RID: 2559
		private bool uri;

		// Token: 0x04000A00 RID: 2560
		private bool name;

		// Token: 0x04000A01 RID: 2561
		private static Hashtable s_table = HtmlAttributeProps.CreatePropsTable();
	}
}

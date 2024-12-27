using System;
using System.Collections;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200017A RID: 378
	internal class HtmlElementProps
	{
		// Token: 0x06000F7C RID: 3964 RVA: 0x0004D5F0 File Offset: 0x0004C5F0
		public static HtmlElementProps Create(bool empty, bool abrParent, bool uriParent, bool noEntities, bool blockWS, bool head, bool nameParent)
		{
			return new HtmlElementProps
			{
				empty = empty,
				abrParent = abrParent,
				uriParent = uriParent,
				noEntities = noEntities,
				blockWS = blockWS,
				head = head,
				nameParent = nameParent
			};
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000F7D RID: 3965 RVA: 0x0004D638 File Offset: 0x0004C638
		public bool Empty
		{
			get
			{
				return this.empty;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000F7E RID: 3966 RVA: 0x0004D640 File Offset: 0x0004C640
		public bool AbrParent
		{
			get
			{
				return this.abrParent;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x0004D648 File Offset: 0x0004C648
		public bool UriParent
		{
			get
			{
				return this.uriParent;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000F80 RID: 3968 RVA: 0x0004D650 File Offset: 0x0004C650
		public bool NoEntities
		{
			get
			{
				return this.noEntities;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x0004D658 File Offset: 0x0004C658
		public bool BlockWS
		{
			get
			{
				return this.blockWS;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000F82 RID: 3970 RVA: 0x0004D660 File Offset: 0x0004C660
		public bool Head
		{
			get
			{
				return this.head;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000F83 RID: 3971 RVA: 0x0004D668 File Offset: 0x0004C668
		public bool NameParent
		{
			get
			{
				return this.nameParent;
			}
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0004D670 File Offset: 0x0004C670
		public static HtmlElementProps GetProps(string name)
		{
			return (HtmlElementProps)HtmlElementProps.s_table[name];
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0004D690 File Offset: 0x0004C690
		private static Hashtable CreatePropsTable()
		{
			bool flag = false;
			bool flag2 = true;
			return new Hashtable(71, StringComparer.OrdinalIgnoreCase)
			{
				{
					"a",
					HtmlElementProps.Create(flag, flag, flag2, flag, flag, flag, flag2)
				},
				{
					"address",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"applet",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"area",
					HtmlElementProps.Create(flag2, flag2, flag2, flag, flag2, flag, flag)
				},
				{
					"base",
					HtmlElementProps.Create(flag2, flag, flag2, flag, flag2, flag, flag)
				},
				{
					"basefont",
					HtmlElementProps.Create(flag2, flag, flag, flag, flag2, flag, flag)
				},
				{
					"blockquote",
					HtmlElementProps.Create(flag, flag, flag2, flag, flag2, flag, flag)
				},
				{
					"body",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"br",
					HtmlElementProps.Create(flag2, flag, flag, flag, flag, flag, flag)
				},
				{
					"button",
					HtmlElementProps.Create(flag, flag2, flag, flag, flag, flag, flag)
				},
				{
					"caption",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"center",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"col",
					HtmlElementProps.Create(flag2, flag, flag, flag, flag2, flag, flag)
				},
				{
					"colgroup",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"dd",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"del",
					HtmlElementProps.Create(flag, flag, flag2, flag, flag2, flag, flag)
				},
				{
					"dir",
					HtmlElementProps.Create(flag, flag2, flag, flag, flag2, flag, flag)
				},
				{
					"div",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"dl",
					HtmlElementProps.Create(flag, flag2, flag, flag, flag2, flag, flag)
				},
				{
					"dt",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"fieldset",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"font",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"form",
					HtmlElementProps.Create(flag, flag, flag2, flag, flag2, flag, flag)
				},
				{
					"frame",
					HtmlElementProps.Create(flag2, flag2, flag, flag, flag2, flag, flag)
				},
				{
					"frameset",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"h1",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"h2",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"h3",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"h4",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"h5",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"h6",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"head",
					HtmlElementProps.Create(flag, flag, flag2, flag, flag2, flag2, flag)
				},
				{
					"hr",
					HtmlElementProps.Create(flag2, flag2, flag, flag, flag2, flag, flag)
				},
				{
					"html",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"iframe",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"img",
					HtmlElementProps.Create(flag2, flag2, flag2, flag, flag, flag, flag)
				},
				{
					"input",
					HtmlElementProps.Create(flag2, flag2, flag2, flag, flag, flag, flag)
				},
				{
					"ins",
					HtmlElementProps.Create(flag, flag, flag2, flag, flag2, flag, flag)
				},
				{
					"isindex",
					HtmlElementProps.Create(flag2, flag, flag, flag, flag2, flag, flag)
				},
				{
					"legend",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"li",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"link",
					HtmlElementProps.Create(flag2, flag, flag2, flag, flag2, flag, flag)
				},
				{
					"map",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"menu",
					HtmlElementProps.Create(flag, flag2, flag, flag, flag2, flag, flag)
				},
				{
					"meta",
					HtmlElementProps.Create(flag2, flag, flag, flag, flag2, flag, flag)
				},
				{
					"noframes",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"noscript",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"object",
					HtmlElementProps.Create(flag, flag2, flag2, flag, flag, flag, flag)
				},
				{
					"ol",
					HtmlElementProps.Create(flag, flag2, flag, flag, flag2, flag, flag)
				},
				{
					"optgroup",
					HtmlElementProps.Create(flag, flag2, flag, flag, flag2, flag, flag)
				},
				{
					"option",
					HtmlElementProps.Create(flag, flag2, flag, flag, flag2, flag, flag)
				},
				{
					"p",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"param",
					HtmlElementProps.Create(flag2, flag, flag, flag, flag2, flag, flag)
				},
				{
					"pre",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"q",
					HtmlElementProps.Create(flag, flag, flag2, flag, flag, flag, flag)
				},
				{
					"s",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"script",
					HtmlElementProps.Create(flag, flag2, flag2, flag2, flag, flag, flag)
				},
				{
					"select",
					HtmlElementProps.Create(flag, flag2, flag, flag, flag, flag, flag)
				},
				{
					"strike",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"style",
					HtmlElementProps.Create(flag, flag, flag, flag2, flag2, flag, flag)
				},
				{
					"table",
					HtmlElementProps.Create(flag, flag, flag2, flag, flag2, flag, flag)
				},
				{
					"tbody",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"td",
					HtmlElementProps.Create(flag, flag2, flag, flag, flag2, flag, flag)
				},
				{
					"textarea",
					HtmlElementProps.Create(flag, flag2, flag, flag, flag, flag, flag)
				},
				{
					"tfoot",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"th",
					HtmlElementProps.Create(flag, flag2, flag, flag, flag2, flag, flag)
				},
				{
					"thead",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"title",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"tr",
					HtmlElementProps.Create(flag, flag, flag, flag, flag2, flag, flag)
				},
				{
					"ul",
					HtmlElementProps.Create(flag, flag2, flag, flag, flag2, flag, flag)
				},
				{
					"xmp",
					HtmlElementProps.Create(flag, flag, flag, flag, flag, flag, flag)
				}
			};
		}

		// Token: 0x040009F6 RID: 2550
		private bool empty;

		// Token: 0x040009F7 RID: 2551
		private bool abrParent;

		// Token: 0x040009F8 RID: 2552
		private bool uriParent;

		// Token: 0x040009F9 RID: 2553
		private bool noEntities;

		// Token: 0x040009FA RID: 2554
		private bool blockWS;

		// Token: 0x040009FB RID: 2555
		private bool head;

		// Token: 0x040009FC RID: 2556
		private bool nameParent;

		// Token: 0x040009FD RID: 2557
		private static Hashtable s_table = HtmlElementProps.CreatePropsTable();
	}
}

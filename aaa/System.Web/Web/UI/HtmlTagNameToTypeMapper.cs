using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000402 RID: 1026
	internal class HtmlTagNameToTypeMapper : ITagNameToTypeMapper
	{
		// Token: 0x06003295 RID: 12949 RVA: 0x000DD361 File Offset: 0x000DC361
		internal HtmlTagNameToTypeMapper()
		{
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x000DD36C File Offset: 0x000DC36C
		Type ITagNameToTypeMapper.GetControlType(string tagName, IDictionary attributeBag)
		{
			if (HtmlTagNameToTypeMapper._tagMap == null)
			{
				HtmlTagNameToTypeMapper._tagMap = new Hashtable(10, StringComparer.OrdinalIgnoreCase)
				{
					{
						"a",
						typeof(HtmlAnchor)
					},
					{
						"button",
						typeof(HtmlButton)
					},
					{
						"form",
						typeof(HtmlForm)
					},
					{
						"head",
						typeof(HtmlHead)
					},
					{
						"img",
						typeof(HtmlImage)
					},
					{
						"textarea",
						typeof(HtmlTextArea)
					},
					{
						"select",
						typeof(HtmlSelect)
					},
					{
						"table",
						typeof(HtmlTable)
					},
					{
						"tr",
						typeof(HtmlTableRow)
					},
					{
						"td",
						typeof(HtmlTableCell)
					},
					{
						"th",
						typeof(HtmlTableCell)
					}
				};
			}
			if (HtmlTagNameToTypeMapper._inputTypes == null)
			{
				HtmlTagNameToTypeMapper._inputTypes = new Hashtable(10, StringComparer.OrdinalIgnoreCase)
				{
					{
						"text",
						typeof(HtmlInputText)
					},
					{
						"password",
						typeof(HtmlInputPassword)
					},
					{
						"button",
						typeof(HtmlInputButton)
					},
					{
						"submit",
						typeof(HtmlInputSubmit)
					},
					{
						"reset",
						typeof(HtmlInputReset)
					},
					{
						"image",
						typeof(HtmlInputImage)
					},
					{
						"checkbox",
						typeof(HtmlInputCheckBox)
					},
					{
						"radio",
						typeof(HtmlInputRadioButton)
					},
					{
						"hidden",
						typeof(HtmlInputHidden)
					},
					{
						"file",
						typeof(HtmlInputFile)
					}
				};
			}
			Type type;
			if (StringUtil.EqualsIgnoreCase("input", tagName))
			{
				string text = (string)attributeBag["type"];
				if (text == null)
				{
					text = "text";
				}
				type = (Type)HtmlTagNameToTypeMapper._inputTypes[text];
				if (type == null)
				{
					throw new HttpException(SR.GetString("Invalid_type_for_input_tag", new object[] { text }));
				}
			}
			else
			{
				type = (Type)HtmlTagNameToTypeMapper._tagMap[tagName];
				if (type == null)
				{
					type = typeof(HtmlGenericControl);
				}
			}
			return type;
		}

		// Token: 0x04002307 RID: 8967
		private static Hashtable _tagMap;

		// Token: 0x04002308 RID: 8968
		private static Hashtable _inputTypes;
	}
}

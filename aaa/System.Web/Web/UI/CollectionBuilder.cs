using System;
using System.Collections;
using System.Reflection;

namespace System.Web.UI
{
	// Token: 0x020003B0 RID: 944
	internal sealed class CollectionBuilder : ControlBuilder
	{
		// Token: 0x06002E30 RID: 11824 RVA: 0x000CF3C9 File Offset: 0x000CE3C9
		internal CollectionBuilder(bool ignoreUnknownContent)
		{
			this._ignoreUnknownContent = ignoreUnknownContent;
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x000CF3D8 File Offset: 0x000CE3D8
		public override void Init(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string ID, IDictionary attribs)
		{
			base.Init(parser, parentBuilder, type, tagName, ID, attribs);
			PropertyInfo propertyInfo = parentBuilder.ControlType.GetProperty(tagName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
			base.SetControlType(propertyInfo.PropertyType);
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
			propertyInfo = base.ControlType.GetProperty("Item", bindingFlags, null, null, new Type[] { typeof(int) }, null);
			if (propertyInfo == null)
			{
				propertyInfo = base.ControlType.GetProperty("Item", bindingFlags);
			}
			if (propertyInfo != null)
			{
				this._itemType = propertyInfo.PropertyType;
			}
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x000CF461 File Offset: 0x000CE461
		public override object BuildObject()
		{
			return this;
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x000CF464 File Offset: 0x000CE464
		public override Type GetChildControlType(string tagName, IDictionary attribs)
		{
			Type type = base.Parser.MapStringToType(tagName, attribs);
			if (this._itemType == null || this._itemType.IsAssignableFrom(type))
			{
				return type;
			}
			if (this._ignoreUnknownContent)
			{
				return null;
			}
			string text = string.Empty;
			if (base.ControlType != null)
			{
				text = base.ControlType.FullName;
			}
			else
			{
				text = base.TagName;
			}
			throw new HttpException(SR.GetString("Invalid_collection_item_type", new string[]
			{
				text,
				this._itemType.FullName,
				tagName,
				type.FullName
			}));
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x000CF4FC File Offset: 0x000CE4FC
		public override void AppendLiteralString(string s)
		{
			if (this._ignoreUnknownContent)
			{
				return;
			}
			if (!Util.IsWhiteSpaceString(s))
			{
				throw new HttpException(SR.GetString("Literal_content_not_allowed", new object[]
				{
					base.ControlType.FullName,
					s.Trim()
				}));
			}
		}

		// Token: 0x04002174 RID: 8564
		private Type _itemType;

		// Token: 0x04002175 RID: 8565
		private bool _ignoreUnknownContent;
	}
}

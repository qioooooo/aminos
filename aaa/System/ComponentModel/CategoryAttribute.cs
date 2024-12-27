using System;

namespace System.ComponentModel
{
	// Token: 0x02000007 RID: 7
	[AttributeUsage(AttributeTargets.All)]
	public class CategoryAttribute : Attribute
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000218F File Offset: 0x0000118F
		public static CategoryAttribute Action
		{
			get
			{
				if (CategoryAttribute.action == null)
				{
					CategoryAttribute.action = new CategoryAttribute("Action");
				}
				return CategoryAttribute.action;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000021AC File Offset: 0x000011AC
		public static CategoryAttribute Appearance
		{
			get
			{
				if (CategoryAttribute.appearance == null)
				{
					CategoryAttribute.appearance = new CategoryAttribute("Appearance");
				}
				return CategoryAttribute.appearance;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000021C9 File Offset: 0x000011C9
		public static CategoryAttribute Asynchronous
		{
			get
			{
				if (CategoryAttribute.asynchronous == null)
				{
					CategoryAttribute.asynchronous = new CategoryAttribute("Asynchronous");
				}
				return CategoryAttribute.asynchronous;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000021E6 File Offset: 0x000011E6
		public static CategoryAttribute Behavior
		{
			get
			{
				if (CategoryAttribute.behavior == null)
				{
					CategoryAttribute.behavior = new CategoryAttribute("Behavior");
				}
				return CategoryAttribute.behavior;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002203 File Offset: 0x00001203
		public static CategoryAttribute Data
		{
			get
			{
				if (CategoryAttribute.data == null)
				{
					CategoryAttribute.data = new CategoryAttribute("Data");
				}
				return CategoryAttribute.data;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002220 File Offset: 0x00001220
		public static CategoryAttribute Default
		{
			get
			{
				if (CategoryAttribute.defAttr == null)
				{
					CategoryAttribute.defAttr = new CategoryAttribute();
				}
				return CategoryAttribute.defAttr;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002238 File Offset: 0x00001238
		public static CategoryAttribute Design
		{
			get
			{
				if (CategoryAttribute.design == null)
				{
					CategoryAttribute.design = new CategoryAttribute("Design");
				}
				return CategoryAttribute.design;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002255 File Offset: 0x00001255
		public static CategoryAttribute DragDrop
		{
			get
			{
				if (CategoryAttribute.dragDrop == null)
				{
					CategoryAttribute.dragDrop = new CategoryAttribute("DragDrop");
				}
				return CategoryAttribute.dragDrop;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002272 File Offset: 0x00001272
		public static CategoryAttribute Focus
		{
			get
			{
				if (CategoryAttribute.focus == null)
				{
					CategoryAttribute.focus = new CategoryAttribute("Focus");
				}
				return CategoryAttribute.focus;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000228F File Offset: 0x0000128F
		public static CategoryAttribute Format
		{
			get
			{
				if (CategoryAttribute.format == null)
				{
					CategoryAttribute.format = new CategoryAttribute("Format");
				}
				return CategoryAttribute.format;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000022AC File Offset: 0x000012AC
		public static CategoryAttribute Key
		{
			get
			{
				if (CategoryAttribute.key == null)
				{
					CategoryAttribute.key = new CategoryAttribute("Key");
				}
				return CategoryAttribute.key;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000022C9 File Offset: 0x000012C9
		public static CategoryAttribute Layout
		{
			get
			{
				if (CategoryAttribute.layout == null)
				{
					CategoryAttribute.layout = new CategoryAttribute("Layout");
				}
				return CategoryAttribute.layout;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000022E6 File Offset: 0x000012E6
		public static CategoryAttribute Mouse
		{
			get
			{
				if (CategoryAttribute.mouse == null)
				{
					CategoryAttribute.mouse = new CategoryAttribute("Mouse");
				}
				return CategoryAttribute.mouse;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002303 File Offset: 0x00001303
		public static CategoryAttribute WindowStyle
		{
			get
			{
				if (CategoryAttribute.windowStyle == null)
				{
					CategoryAttribute.windowStyle = new CategoryAttribute("WindowStyle");
				}
				return CategoryAttribute.windowStyle;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002320 File Offset: 0x00001320
		public CategoryAttribute()
			: this("Default")
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000232D File Offset: 0x0000132D
		public CategoryAttribute(string category)
		{
			this.categoryValue = category;
			this.localized = false;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002344 File Offset: 0x00001344
		public string Category
		{
			get
			{
				if (!this.localized)
				{
					this.localized = true;
					string localizedString = this.GetLocalizedString(this.categoryValue);
					if (localizedString != null)
					{
						this.categoryValue = localizedString;
					}
				}
				return this.categoryValue;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000237D File Offset: 0x0000137D
		public override bool Equals(object obj)
		{
			return obj == this || (obj is CategoryAttribute && this.Category.Equals(((CategoryAttribute)obj).Category));
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000023A5 File Offset: 0x000013A5
		public override int GetHashCode()
		{
			return this.Category.GetHashCode();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000023B2 File Offset: 0x000013B2
		protected virtual string GetLocalizedString(string value)
		{
			return (string)SR.GetObject("PropertyCategory" + value);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000023C9 File Offset: 0x000013C9
		public override bool IsDefaultAttribute()
		{
			return this.Category.Equals(CategoryAttribute.Default.Category);
		}

		// Token: 0x04000038 RID: 56
		private static CategoryAttribute appearance;

		// Token: 0x04000039 RID: 57
		private static CategoryAttribute asynchronous;

		// Token: 0x0400003A RID: 58
		private static CategoryAttribute behavior;

		// Token: 0x0400003B RID: 59
		private static CategoryAttribute data;

		// Token: 0x0400003C RID: 60
		private static CategoryAttribute design;

		// Token: 0x0400003D RID: 61
		private static CategoryAttribute action;

		// Token: 0x0400003E RID: 62
		private static CategoryAttribute format;

		// Token: 0x0400003F RID: 63
		private static CategoryAttribute layout;

		// Token: 0x04000040 RID: 64
		private static CategoryAttribute mouse;

		// Token: 0x04000041 RID: 65
		private static CategoryAttribute key;

		// Token: 0x04000042 RID: 66
		private static CategoryAttribute focus;

		// Token: 0x04000043 RID: 67
		private static CategoryAttribute windowStyle;

		// Token: 0x04000044 RID: 68
		private static CategoryAttribute dragDrop;

		// Token: 0x04000045 RID: 69
		private static CategoryAttribute defAttr;

		// Token: 0x04000046 RID: 70
		private bool localized;

		// Token: 0x04000047 RID: 71
		private string categoryValue;
	}
}

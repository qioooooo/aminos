using System;

namespace System.Windows.Forms
{
	// Token: 0x02000422 RID: 1058
	internal class HelpInfo
	{
		// Token: 0x06003ED7 RID: 16087 RVA: 0x000E4D54 File Offset: 0x000E3D54
		public HelpInfo(string helpfilepath)
		{
			this.helpFilePath = helpfilepath;
			this.keyword = "";
			this.navigator = HelpNavigator.TableOfContents;
			this.param = null;
			this.option = 1;
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x000E4D87 File Offset: 0x000E3D87
		public HelpInfo(string helpfilepath, string keyword)
		{
			this.helpFilePath = helpfilepath;
			this.keyword = keyword;
			this.navigator = HelpNavigator.TableOfContents;
			this.param = null;
			this.option = 2;
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x000E4DB6 File Offset: 0x000E3DB6
		public HelpInfo(string helpfilepath, HelpNavigator navigator)
		{
			this.helpFilePath = helpfilepath;
			this.keyword = "";
			this.navigator = navigator;
			this.param = null;
			this.option = 3;
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x000E4DE5 File Offset: 0x000E3DE5
		public HelpInfo(string helpfilepath, HelpNavigator navigator, object param)
		{
			this.helpFilePath = helpfilepath;
			this.keyword = "";
			this.navigator = navigator;
			this.param = param;
			this.option = 4;
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06003EDB RID: 16091 RVA: 0x000E4E14 File Offset: 0x000E3E14
		public int Option
		{
			get
			{
				return this.option;
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x06003EDC RID: 16092 RVA: 0x000E4E1C File Offset: 0x000E3E1C
		public string HelpFilePath
		{
			get
			{
				return this.helpFilePath;
			}
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06003EDD RID: 16093 RVA: 0x000E4E24 File Offset: 0x000E3E24
		public string Keyword
		{
			get
			{
				return this.keyword;
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06003EDE RID: 16094 RVA: 0x000E4E2C File Offset: 0x000E3E2C
		public HelpNavigator Navigator
		{
			get
			{
				return this.navigator;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06003EDF RID: 16095 RVA: 0x000E4E34 File Offset: 0x000E3E34
		public object Param
		{
			get
			{
				return this.param;
			}
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x000E4E3C File Offset: 0x000E3E3C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{HelpFilePath=",
				this.helpFilePath,
				", keyword =",
				this.keyword,
				", navigator=",
				this.navigator.ToString(),
				"}"
			});
		}

		// Token: 0x04001EF8 RID: 7928
		private string helpFilePath;

		// Token: 0x04001EF9 RID: 7929
		private string keyword;

		// Token: 0x04001EFA RID: 7930
		private HelpNavigator navigator;

		// Token: 0x04001EFB RID: 7931
		private object param;

		// Token: 0x04001EFC RID: 7932
		private int option;
	}
}

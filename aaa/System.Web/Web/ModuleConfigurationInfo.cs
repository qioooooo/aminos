using System;

namespace System.Web
{
	// Token: 0x020000AB RID: 171
	internal class ModuleConfigurationInfo
	{
		// Token: 0x06000873 RID: 2163 RVA: 0x00025E8A File Offset: 0x00024E8A
		internal ModuleConfigurationInfo(string name, string type, string condition)
		{
			this._type = type;
			this._name = name;
			this._precondition = condition;
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x00025EA7 File Offset: 0x00024EA7
		internal string Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x00025EAF File Offset: 0x00024EAF
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x00025EB7 File Offset: 0x00024EB7
		internal string Precondition
		{
			get
			{
				return this._precondition;
			}
		}

		// Token: 0x040011A9 RID: 4521
		private string _type;

		// Token: 0x040011AA RID: 4522
		private string _name;

		// Token: 0x040011AB RID: 4523
		private string _precondition;
	}
}

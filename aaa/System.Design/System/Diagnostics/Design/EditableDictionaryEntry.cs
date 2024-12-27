using System;

namespace System.Diagnostics.Design
{
	// Token: 0x020000E5 RID: 229
	internal class EditableDictionaryEntry
	{
		// Token: 0x0600095F RID: 2399 RVA: 0x00022E28 File Offset: 0x00021E28
		public EditableDictionaryEntry(string name, string value)
		{
			this._name = name;
			this._value = value;
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x00022E3E File Offset: 0x00021E3E
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x00022E46 File Offset: 0x00021E46
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x00022E4F File Offset: 0x00021E4F
		// (set) Token: 0x06000963 RID: 2403 RVA: 0x00022E57 File Offset: 0x00021E57
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x04000D13 RID: 3347
		public string _name;

		// Token: 0x04000D14 RID: 3348
		public string _value;
	}
}

using System;

namespace System.Configuration
{
	// Token: 0x02000072 RID: 114
	internal sealed class InvalidPropValue
	{
		// Token: 0x06000433 RID: 1075 RVA: 0x000140E9 File Offset: 0x000130E9
		internal InvalidPropValue(string value, ConfigurationException error)
		{
			this._value = value;
			this._error = error;
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x000140FF File Offset: 0x000130FF
		internal ConfigurationException Error
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x00014107 File Offset: 0x00013107
		internal string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x04000325 RID: 805
		private string _value;

		// Token: 0x04000326 RID: 806
		private ConfigurationException _error;
	}
}

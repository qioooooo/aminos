using System;
using System.Globalization;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000236 RID: 566
	internal class Com2Enum
	{
		// Token: 0x06001AF5 RID: 6901 RVA: 0x00033C8E File Offset: 0x00032C8E
		public Com2Enum(string[] names, object[] values, bool allowUnknownValues)
		{
			this.allowUnknownValues = allowUnknownValues;
			if (names == null || values == null || names.Length != values.Length)
			{
				throw new ArgumentException(SR.GetString("COM2NamesAndValuesNotEqual"));
			}
			this.PopulateArrays(names, values);
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x00033CC3 File Offset: 0x00032CC3
		public bool IsStrictEnum
		{
			get
			{
				return !this.allowUnknownValues;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x00033CCE File Offset: 0x00032CCE
		public virtual object[] Values
		{
			get
			{
				return (object[])this.values.Clone();
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x00033CE0 File Offset: 0x00032CE0
		public virtual string[] Names
		{
			get
			{
				return (string[])this.names.Clone();
			}
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x00033CF4 File Offset: 0x00032CF4
		public virtual object FromString(string s)
		{
			int num = -1;
			for (int i = 0; i < this.stringValues.Length; i++)
			{
				if (string.Compare(this.names[i], s, true, CultureInfo.InvariantCulture) == 0 || string.Compare(this.stringValues[i], s, true, CultureInfo.InvariantCulture) == 0)
				{
					return this.values[i];
				}
				if (num == -1 && string.Compare(this.names[i], s, true, CultureInfo.InvariantCulture) == 0)
				{
					num = i;
				}
			}
			if (num != -1)
			{
				return this.values[num];
			}
			if (!this.allowUnknownValues)
			{
				return null;
			}
			return s;
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x00033D80 File Offset: 0x00032D80
		protected virtual void PopulateArrays(string[] names, object[] values)
		{
			this.names = new string[names.Length];
			this.stringValues = new string[names.Length];
			this.values = new object[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				this.names[i] = names[i];
				this.values[i] = values[i];
				if (values[i] != null)
				{
					this.stringValues[i] = values[i].ToString();
				}
			}
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x00033DF0 File Offset: 0x00032DF0
		public virtual string ToString(object v)
		{
			if (v != null)
			{
				if (this.values.Length > 0 && v.GetType() != this.values[0].GetType())
				{
					try
					{
						v = Convert.ChangeType(v, this.values[0].GetType(), CultureInfo.InvariantCulture);
					}
					catch
					{
					}
				}
				string text = v.ToString();
				for (int i = 0; i < this.values.Length; i++)
				{
					if (string.Compare(this.stringValues[i], text, true, CultureInfo.InvariantCulture) == 0)
					{
						return this.names[i];
					}
				}
				if (this.allowUnknownValues)
				{
					return text;
				}
			}
			return "";
		}

		// Token: 0x040012EE RID: 4846
		private string[] names;

		// Token: 0x040012EF RID: 4847
		private object[] values;

		// Token: 0x040012F0 RID: 4848
		private string[] stringValues;

		// Token: 0x040012F1 RID: 4849
		private bool allowUnknownValues;
	}
}

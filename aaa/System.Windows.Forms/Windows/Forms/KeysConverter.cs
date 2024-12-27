using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x0200045F RID: 1119
	public class KeysConverter : TypeConverter, IComparer
	{
		// Token: 0x060041D6 RID: 16854 RVA: 0x000EB33A File Offset: 0x000EA33A
		private void AddKey(string key, Keys value)
		{
			this.keyNames[key] = value;
			this.displayOrder.Add(key);
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x000EB35C File Offset: 0x000EA35C
		private void Initialize()
		{
			this.keyNames = new Hashtable(34);
			this.displayOrder = new List<string>(34);
			this.AddKey(SR.GetString("toStringEnter"), Keys.Return);
			this.AddKey("F12", Keys.F12);
			this.AddKey("F11", Keys.F11);
			this.AddKey("F10", Keys.F10);
			this.AddKey(SR.GetString("toStringEnd"), Keys.End);
			this.AddKey(SR.GetString("toStringControl"), Keys.Control);
			this.AddKey("F8", Keys.F8);
			this.AddKey("F9", Keys.F9);
			this.AddKey(SR.GetString("toStringAlt"), Keys.Alt);
			this.AddKey("F4", Keys.F4);
			this.AddKey("F5", Keys.F5);
			this.AddKey("F6", Keys.F6);
			this.AddKey("F7", Keys.F7);
			this.AddKey("F1", Keys.F1);
			this.AddKey("F2", Keys.F2);
			this.AddKey("F3", Keys.F3);
			this.AddKey(SR.GetString("toStringPageDown"), Keys.Next);
			this.AddKey(SR.GetString("toStringInsert"), Keys.Insert);
			this.AddKey(SR.GetString("toStringHome"), Keys.Home);
			this.AddKey(SR.GetString("toStringDelete"), Keys.Delete);
			this.AddKey(SR.GetString("toStringShift"), Keys.Shift);
			this.AddKey(SR.GetString("toStringPageUp"), Keys.Prior);
			this.AddKey(SR.GetString("toStringBack"), Keys.Back);
			this.AddKey("0", Keys.D0);
			this.AddKey("1", Keys.D1);
			this.AddKey("2", Keys.D2);
			this.AddKey("3", Keys.D3);
			this.AddKey("4", Keys.D4);
			this.AddKey("5", Keys.D5);
			this.AddKey("6", Keys.D6);
			this.AddKey("7", Keys.D7);
			this.AddKey("8", Keys.D8);
			this.AddKey("9", Keys.D9);
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x060041D8 RID: 16856 RVA: 0x000EB56F File Offset: 0x000EA56F
		private IDictionary KeyNames
		{
			get
			{
				if (this.keyNames == null)
				{
					this.Initialize();
				}
				return this.keyNames;
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x060041D9 RID: 16857 RVA: 0x000EB585 File Offset: 0x000EA585
		private List<string> DisplayOrder
		{
			get
			{
				if (this.displayOrder == null)
				{
					this.Initialize();
				}
				return this.displayOrder;
			}
		}

		// Token: 0x060041DA RID: 16858 RVA: 0x000EB59B File Offset: 0x000EA59B
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(Enum[]) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x000EB5C1 File Offset: 0x000EA5C1
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(Enum[]) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060041DC RID: 16860 RVA: 0x000EB5DA File Offset: 0x000EA5DA
		public int Compare(object a, object b)
		{
			return string.Compare(base.ConvertToString(a), base.ConvertToString(b), false, CultureInfo.InvariantCulture);
		}

		// Token: 0x060041DD RID: 16861 RVA: 0x000EB5F8 File Offset: 0x000EA5F8
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				if (text.Length == 0)
				{
					return null;
				}
				string[] array = text.Split(new char[] { '+' });
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = array[i].Trim();
				}
				Keys keys = Keys.None;
				bool flag = false;
				for (int j = 0; j < array.Length; j++)
				{
					object obj = this.KeyNames[array[j]];
					if (obj == null)
					{
						obj = Enum.Parse(typeof(Keys), array[j]);
					}
					if (obj == null)
					{
						throw new FormatException(SR.GetString("KeysConverterInvalidKeyName", new object[] { array[j] }));
					}
					Keys keys2 = (Keys)obj;
					if ((keys2 & Keys.KeyCode) != Keys.None)
					{
						if (flag)
						{
							throw new FormatException(SR.GetString("KeysConverterInvalidKeyCombination"));
						}
						flag = true;
					}
					keys |= keys2;
				}
				return keys;
			}
			else
			{
				if (value is Enum[])
				{
					long num = 0L;
					foreach (Enum @enum in (Enum[])value)
					{
						num |= Convert.ToInt64(@enum, CultureInfo.InvariantCulture);
					}
					return Enum.ToObject(typeof(Keys), num);
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		// Token: 0x060041DE RID: 16862 RVA: 0x000EB758 File Offset: 0x000EA758
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is Keys || value is int)
			{
				bool flag = destinationType == typeof(string);
				bool flag2 = false;
				if (!flag)
				{
					flag2 = destinationType == typeof(Enum[]);
				}
				if (flag || flag2)
				{
					Keys keys = (Keys)value;
					bool flag3 = false;
					ArrayList arrayList = new ArrayList();
					Keys keys2 = keys & Keys.Modifiers;
					for (int i = 0; i < this.DisplayOrder.Count; i++)
					{
						string text = this.DisplayOrder[i];
						Keys keys3 = (Keys)this.keyNames[text];
						if ((keys3 & keys2) != Keys.None)
						{
							if (flag)
							{
								if (flag3)
								{
									arrayList.Add("+");
								}
								arrayList.Add(text);
							}
							else
							{
								arrayList.Add(keys3);
							}
							flag3 = true;
						}
					}
					Keys keys4 = keys & Keys.KeyCode;
					bool flag4 = false;
					if (flag3 && flag)
					{
						arrayList.Add("+");
					}
					for (int j = 0; j < this.DisplayOrder.Count; j++)
					{
						string text2 = this.DisplayOrder[j];
						Keys keys5 = (Keys)this.keyNames[text2];
						if (keys5.Equals(keys4))
						{
							if (flag)
							{
								arrayList.Add(text2);
							}
							else
							{
								arrayList.Add(keys5);
							}
							flag4 = true;
							break;
						}
					}
					if (!flag4 && Enum.IsDefined(typeof(Keys), (int)keys4))
					{
						if (flag)
						{
							arrayList.Add(keys4.ToString());
						}
						else
						{
							arrayList.Add(keys4);
						}
					}
					if (flag)
					{
						StringBuilder stringBuilder = new StringBuilder(32);
						foreach (object obj in arrayList)
						{
							string text3 = (string)obj;
							stringBuilder.Append(text3);
						}
						return stringBuilder.ToString();
					}
					return (Enum[])arrayList.ToArray(typeof(Enum));
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x060041DF RID: 16863 RVA: 0x000EB9A4 File Offset: 0x000EA9A4
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.values == null)
			{
				ArrayList arrayList = new ArrayList();
				ICollection collection = this.KeyNames.Values;
				foreach (object obj in collection)
				{
					arrayList.Add(obj);
				}
				arrayList.Sort(this);
				this.values = new TypeConverter.StandardValuesCollection(arrayList.ToArray());
			}
			return this.values;
		}

		// Token: 0x060041E0 RID: 16864 RVA: 0x000EBA30 File Offset: 0x000EAA30
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x000EBA33 File Offset: 0x000EAA33
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04002076 RID: 8310
		private const Keys FirstDigit = Keys.D0;

		// Token: 0x04002077 RID: 8311
		private const Keys LastDigit = Keys.D9;

		// Token: 0x04002078 RID: 8312
		private const Keys FirstAscii = Keys.A;

		// Token: 0x04002079 RID: 8313
		private const Keys LastAscii = Keys.Z;

		// Token: 0x0400207A RID: 8314
		private const Keys FirstNumpadDigit = Keys.NumPad0;

		// Token: 0x0400207B RID: 8315
		private const Keys LastNumpadDigit = Keys.NumPad9;

		// Token: 0x0400207C RID: 8316
		private IDictionary keyNames;

		// Token: 0x0400207D RID: 8317
		private List<string> displayOrder;

		// Token: 0x0400207E RID: 8318
		private TypeConverter.StandardValuesCollection values;
	}
}

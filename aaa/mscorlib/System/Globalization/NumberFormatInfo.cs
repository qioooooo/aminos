using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Globalization
{
	// Token: 0x020003B8 RID: 952
	[ComVisible(true)]
	[Serializable]
	public sealed class NumberFormatInfo : ICloneable, IFormatProvider
	{
		// Token: 0x06002775 RID: 10101 RVA: 0x00076D29 File Offset: 0x00075D29
		public NumberFormatInfo()
			: this(null)
		{
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x00076D34 File Offset: 0x00075D34
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			if (this.numberDecimalSeparator != this.numberGroupSeparator)
			{
				this.validForParseAsNumber = true;
			}
			else
			{
				this.validForParseAsNumber = false;
			}
			if (this.numberDecimalSeparator != this.numberGroupSeparator && this.numberDecimalSeparator != this.currencyGroupSeparator && this.currencyDecimalSeparator != this.numberGroupSeparator && this.currencyDecimalSeparator != this.currencyGroupSeparator)
			{
				this.validForParseAsCurrency = true;
				return;
			}
			this.validForParseAsCurrency = false;
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x00076DBF File Offset: 0x00075DBF
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.nativeDigits = null;
			this.digitSubstitution = -1;
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x00076DD0 File Offset: 0x00075DD0
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			if (this.nativeDigits == null)
			{
				this.nativeDigits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
			}
			if (this.digitSubstitution < 0)
			{
				this.digitSubstitution = 1;
			}
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x00076E55 File Offset: 0x00075E55
		private void VerifyDecimalSeparator(string decSep, string propertyName)
		{
			if (decSep == null)
			{
				throw new ArgumentNullException(propertyName, Environment.GetResourceString("ArgumentNull_String"));
			}
			if (decSep.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyDecString"));
			}
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x00076E83 File Offset: 0x00075E83
		private void VerifyGroupSeparator(string groupSep, string propertyName)
		{
			if (groupSep == null)
			{
				throw new ArgumentNullException(propertyName, Environment.GetResourceString("ArgumentNull_String"));
			}
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x00076E9C File Offset: 0x00075E9C
		private void VerifyNativeDigits(string[] nativeDig, string propertyName)
		{
			if (nativeDig == null)
			{
				throw new ArgumentNullException(propertyName, Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (nativeDig.Length != 10)
			{
				throw new ArgumentException(propertyName, Environment.GetResourceString("Argument_InvalidNativeDigitCount"));
			}
			for (int i = 0; i < nativeDig.Length; i++)
			{
				if (nativeDig[i] == null)
				{
					throw new ArgumentNullException(propertyName, Environment.GetResourceString("ArgumentNull_ArrayValue"));
				}
				if (nativeDig[i].Length != 1)
				{
					if (nativeDig[i].Length != 2)
					{
						throw new ArgumentException(propertyName, Environment.GetResourceString("Argument_InvalidNativeDigitValue"));
					}
					if (!char.IsSurrogatePair(nativeDig[i][0], nativeDig[i][1]))
					{
						throw new ArgumentException(propertyName, Environment.GetResourceString("Argument_InvalidNativeDigitValue"));
					}
				}
				if (CharUnicodeInfo.GetDecimalDigitValue(nativeDig[i], 0) != i && CharUnicodeInfo.GetUnicodeCategory(nativeDig[i], 0) != UnicodeCategory.PrivateUse)
				{
					throw new ArgumentException(propertyName, Environment.GetResourceString("Argument_InvalidNativeDigitValue"));
				}
			}
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x00076F7C File Offset: 0x00075F7C
		private void VerifyDigitSubstitution(DigitShapes digitSub, string propertyName)
		{
			switch (digitSub)
			{
			case DigitShapes.Context:
			case DigitShapes.None:
			case DigitShapes.NativeNational:
				return;
			default:
				throw new ArgumentException(propertyName, Environment.GetResourceString("Argument_InvalidDigitSubstitution"));
			}
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x00076FB0 File Offset: 0x00075FB0
		internal NumberFormatInfo(CultureTableRecord cultureTableRecord)
		{
			if (cultureTableRecord != null)
			{
				cultureTableRecord.GetNFIOverrideValues(this);
				if (932 == cultureTableRecord.IDEFAULTANSICODEPAGE || 949 == cultureTableRecord.IDEFAULTANSICODEPAGE)
				{
					this.ansiCurrencySymbol = "\\";
				}
				this.negativeInfinitySymbol = cultureTableRecord.SNEGINFINITY;
				this.positiveInfinitySymbol = cultureTableRecord.SPOSINFINITY;
				this.nanSymbol = cultureTableRecord.SNAN;
			}
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x00077177 File Offset: 0x00076177
		private void VerifyWritable()
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x0600277F RID: 10111 RVA: 0x00077191 File Offset: 0x00076191
		public static NumberFormatInfo InvariantInfo
		{
			get
			{
				if (NumberFormatInfo.invariantInfo == null)
				{
					NumberFormatInfo.invariantInfo = NumberFormatInfo.ReadOnly(new NumberFormatInfo());
				}
				return NumberFormatInfo.invariantInfo;
			}
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000771B0 File Offset: 0x000761B0
		public static NumberFormatInfo GetInstance(IFormatProvider formatProvider)
		{
			CultureInfo cultureInfo = formatProvider as CultureInfo;
			if (cultureInfo != null && !cultureInfo.m_isInherited)
			{
				NumberFormatInfo numberFormatInfo = cultureInfo.numInfo;
				if (numberFormatInfo != null)
				{
					return numberFormatInfo;
				}
				return cultureInfo.NumberFormat;
			}
			else
			{
				NumberFormatInfo numberFormatInfo = formatProvider as NumberFormatInfo;
				if (numberFormatInfo != null)
				{
					return numberFormatInfo;
				}
				if (formatProvider != null)
				{
					numberFormatInfo = formatProvider.GetFormat(typeof(NumberFormatInfo)) as NumberFormatInfo;
					if (numberFormatInfo != null)
					{
						return numberFormatInfo;
					}
				}
				return NumberFormatInfo.CurrentInfo;
			}
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x00077214 File Offset: 0x00076214
		public object Clone()
		{
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)base.MemberwiseClone();
			numberFormatInfo.isReadOnly = false;
			return numberFormatInfo;
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06002782 RID: 10114 RVA: 0x00077235 File Offset: 0x00076235
		// (set) Token: 0x06002783 RID: 10115 RVA: 0x00077240 File Offset: 0x00076240
		public int CurrencyDecimalDigits
		{
			get
			{
				return this.currencyDecimalDigits;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("CurrencyDecimalDigits", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 0, 99 }));
				}
				this.currencyDecimalDigits = value;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002784 RID: 10116 RVA: 0x0007729D File Offset: 0x0007629D
		// (set) Token: 0x06002785 RID: 10117 RVA: 0x000772A5 File Offset: 0x000762A5
		public string CurrencyDecimalSeparator
		{
			get
			{
				return this.currencyDecimalSeparator;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyDecimalSeparator(value, "CurrencyDecimalSeparator");
				this.currencyDecimalSeparator = value;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06002786 RID: 10118 RVA: 0x000772C0 File Offset: 0x000762C0
		public bool IsReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x000772C8 File Offset: 0x000762C8
		internal void CheckGroupSize(string propName, int[] groupSize)
		{
			int i = 0;
			while (i < groupSize.Length)
			{
				if (groupSize[i] < 1)
				{
					if (i == groupSize.Length - 1 && groupSize[i] == 0)
					{
						return;
					}
					throw new ArgumentException(propName, Environment.GetResourceString("Argument_InvalidGroupSize"));
				}
				else
				{
					if (groupSize[i] > 9)
					{
						throw new ArgumentException(propName, Environment.GetResourceString("Argument_InvalidGroupSize"));
					}
					i++;
				}
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002788 RID: 10120 RVA: 0x00077320 File Offset: 0x00076320
		// (set) Token: 0x06002789 RID: 10121 RVA: 0x00077334 File Offset: 0x00076334
		public int[] CurrencyGroupSizes
		{
			get
			{
				return (int[])this.currencyGroupSizes.Clone();
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("CurrencyGroupSizes", Environment.GetResourceString("ArgumentNull_Obj"));
				}
				int[] array = (int[])value.Clone();
				this.CheckGroupSize("CurrencyGroupSizes", array);
				this.currencyGroupSizes = array;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x0600278A RID: 10122 RVA: 0x0007737E File Offset: 0x0007637E
		// (set) Token: 0x0600278B RID: 10123 RVA: 0x00077390 File Offset: 0x00076390
		public int[] NumberGroupSizes
		{
			get
			{
				return (int[])this.numberGroupSizes.Clone();
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("NumberGroupSizes", Environment.GetResourceString("ArgumentNull_Obj"));
				}
				int[] array = (int[])value.Clone();
				this.CheckGroupSize("NumberGroupSizes", array);
				this.numberGroupSizes = array;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x0600278C RID: 10124 RVA: 0x000773DA File Offset: 0x000763DA
		// (set) Token: 0x0600278D RID: 10125 RVA: 0x000773EC File Offset: 0x000763EC
		public int[] PercentGroupSizes
		{
			get
			{
				return (int[])this.percentGroupSizes.Clone();
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("PercentGroupSizes", Environment.GetResourceString("ArgumentNull_Obj"));
				}
				int[] array = (int[])value.Clone();
				this.CheckGroupSize("PercentGroupSizes", array);
				this.percentGroupSizes = array;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x0600278E RID: 10126 RVA: 0x00077436 File Offset: 0x00076436
		// (set) Token: 0x0600278F RID: 10127 RVA: 0x0007743E File Offset: 0x0007643E
		public string CurrencyGroupSeparator
		{
			get
			{
				return this.currencyGroupSeparator;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyGroupSeparator(value, "CurrencyGroupSeparator");
				this.currencyGroupSeparator = value;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06002790 RID: 10128 RVA: 0x00077459 File Offset: 0x00076459
		// (set) Token: 0x06002791 RID: 10129 RVA: 0x00077461 File Offset: 0x00076461
		public string CurrencySymbol
		{
			get
			{
				return this.currencySymbol;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("CurrencySymbol", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.currencySymbol = value;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06002792 RID: 10130 RVA: 0x00077488 File Offset: 0x00076488
		public static NumberFormatInfo CurrentInfo
		{
			get
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				if (!currentCulture.m_isInherited)
				{
					NumberFormatInfo numInfo = currentCulture.numInfo;
					if (numInfo != null)
					{
						return numInfo;
					}
				}
				return (NumberFormatInfo)currentCulture.GetFormat(typeof(NumberFormatInfo));
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06002793 RID: 10131 RVA: 0x000774C9 File Offset: 0x000764C9
		// (set) Token: 0x06002794 RID: 10132 RVA: 0x000774D1 File Offset: 0x000764D1
		public string NaNSymbol
		{
			get
			{
				return this.nanSymbol;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("NaNSymbol", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.nanSymbol = value;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06002795 RID: 10133 RVA: 0x000774F8 File Offset: 0x000764F8
		// (set) Token: 0x06002796 RID: 10134 RVA: 0x00077500 File Offset: 0x00076500
		public int CurrencyNegativePattern
		{
			get
			{
				return this.currencyNegativePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 15)
				{
					throw new ArgumentOutOfRangeException("CurrencyNegativePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 0, 15 }));
				}
				this.currencyNegativePattern = value;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06002797 RID: 10135 RVA: 0x0007755D File Offset: 0x0007655D
		// (set) Token: 0x06002798 RID: 10136 RVA: 0x00077568 File Offset: 0x00076568
		public int NumberNegativePattern
		{
			get
			{
				return this.numberNegativePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 4)
				{
					throw new ArgumentOutOfRangeException("NumberNegativePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 0, 4 }));
				}
				this.numberNegativePattern = value;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06002799 RID: 10137 RVA: 0x000775C3 File Offset: 0x000765C3
		// (set) Token: 0x0600279A RID: 10138 RVA: 0x000775CC File Offset: 0x000765CC
		public int PercentPositivePattern
		{
			get
			{
				return this.percentPositivePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 3)
				{
					throw new ArgumentOutOfRangeException("PercentPositivePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 0, 3 }));
				}
				this.percentPositivePattern = value;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x0600279B RID: 10139 RVA: 0x00077627 File Offset: 0x00076627
		// (set) Token: 0x0600279C RID: 10140 RVA: 0x00077630 File Offset: 0x00076630
		public int PercentNegativePattern
		{
			get
			{
				return this.percentNegativePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 11)
				{
					throw new ArgumentOutOfRangeException("PercentNegativePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 0, 11 }));
				}
				this.percentNegativePattern = value;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x0600279D RID: 10141 RVA: 0x0007768D File Offset: 0x0007668D
		// (set) Token: 0x0600279E RID: 10142 RVA: 0x00077695 File Offset: 0x00076695
		public string NegativeInfinitySymbol
		{
			get
			{
				return this.negativeInfinitySymbol;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("NegativeInfinitySymbol", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.negativeInfinitySymbol = value;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x0600279F RID: 10143 RVA: 0x000776BC File Offset: 0x000766BC
		// (set) Token: 0x060027A0 RID: 10144 RVA: 0x000776C4 File Offset: 0x000766C4
		public string NegativeSign
		{
			get
			{
				return this.negativeSign;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("NegativeSign", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.negativeSign = value;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060027A1 RID: 10145 RVA: 0x000776EB File Offset: 0x000766EB
		// (set) Token: 0x060027A2 RID: 10146 RVA: 0x000776F4 File Offset: 0x000766F4
		public int NumberDecimalDigits
		{
			get
			{
				return this.numberDecimalDigits;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("NumberDecimalDigits", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 0, 99 }));
				}
				this.numberDecimalDigits = value;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060027A3 RID: 10147 RVA: 0x00077751 File Offset: 0x00076751
		// (set) Token: 0x060027A4 RID: 10148 RVA: 0x00077759 File Offset: 0x00076759
		public string NumberDecimalSeparator
		{
			get
			{
				return this.numberDecimalSeparator;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyDecimalSeparator(value, "NumberDecimalSeparator");
				this.numberDecimalSeparator = value;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x060027A5 RID: 10149 RVA: 0x00077774 File Offset: 0x00076774
		// (set) Token: 0x060027A6 RID: 10150 RVA: 0x0007777C File Offset: 0x0007677C
		public string NumberGroupSeparator
		{
			get
			{
				return this.numberGroupSeparator;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyGroupSeparator(value, "NumberGroupSeparator");
				this.numberGroupSeparator = value;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060027A7 RID: 10151 RVA: 0x00077797 File Offset: 0x00076797
		// (set) Token: 0x060027A8 RID: 10152 RVA: 0x000777A0 File Offset: 0x000767A0
		public int CurrencyPositivePattern
		{
			get
			{
				return this.currencyPositivePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 3)
				{
					throw new ArgumentOutOfRangeException("CurrencyPositivePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 0, 3 }));
				}
				this.currencyPositivePattern = value;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060027A9 RID: 10153 RVA: 0x000777FB File Offset: 0x000767FB
		// (set) Token: 0x060027AA RID: 10154 RVA: 0x00077803 File Offset: 0x00076803
		public string PositiveInfinitySymbol
		{
			get
			{
				return this.positiveInfinitySymbol;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("PositiveInfinitySymbol", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.positiveInfinitySymbol = value;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x060027AB RID: 10155 RVA: 0x0007782A File Offset: 0x0007682A
		// (set) Token: 0x060027AC RID: 10156 RVA: 0x00077832 File Offset: 0x00076832
		public string PositiveSign
		{
			get
			{
				return this.positiveSign;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("PositiveSign", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.positiveSign = value;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x060027AD RID: 10157 RVA: 0x00077859 File Offset: 0x00076859
		// (set) Token: 0x060027AE RID: 10158 RVA: 0x00077864 File Offset: 0x00076864
		public int PercentDecimalDigits
		{
			get
			{
				return this.percentDecimalDigits;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("PercentDecimalDigits", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 0, 99 }));
				}
				this.percentDecimalDigits = value;
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x060027AF RID: 10159 RVA: 0x000778C1 File Offset: 0x000768C1
		// (set) Token: 0x060027B0 RID: 10160 RVA: 0x000778C9 File Offset: 0x000768C9
		public string PercentDecimalSeparator
		{
			get
			{
				return this.percentDecimalSeparator;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyDecimalSeparator(value, "PercentDecimalSeparator");
				this.percentDecimalSeparator = value;
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x060027B1 RID: 10161 RVA: 0x000778E4 File Offset: 0x000768E4
		// (set) Token: 0x060027B2 RID: 10162 RVA: 0x000778EC File Offset: 0x000768EC
		public string PercentGroupSeparator
		{
			get
			{
				return this.percentGroupSeparator;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyGroupSeparator(value, "PercentGroupSeparator");
				this.percentGroupSeparator = value;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x060027B3 RID: 10163 RVA: 0x00077907 File Offset: 0x00076907
		// (set) Token: 0x060027B4 RID: 10164 RVA: 0x0007790F File Offset: 0x0007690F
		public string PercentSymbol
		{
			get
			{
				return this.percentSymbol;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("PercentSymbol", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.percentSymbol = value;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x060027B5 RID: 10165 RVA: 0x00077936 File Offset: 0x00076936
		// (set) Token: 0x060027B6 RID: 10166 RVA: 0x0007793E File Offset: 0x0007693E
		public string PerMilleSymbol
		{
			get
			{
				return this.perMilleSymbol;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("PerMilleSymbol", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.perMilleSymbol = value;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x060027B7 RID: 10167 RVA: 0x00077965 File Offset: 0x00076965
		// (set) Token: 0x060027B8 RID: 10168 RVA: 0x0007796D File Offset: 0x0007696D
		[ComVisible(false)]
		public string[] NativeDigits
		{
			get
			{
				return this.nativeDigits;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyNativeDigits(value, "NativeDigits");
				this.nativeDigits = value;
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060027B9 RID: 10169 RVA: 0x00077988 File Offset: 0x00076988
		// (set) Token: 0x060027BA RID: 10170 RVA: 0x00077990 File Offset: 0x00076990
		[ComVisible(false)]
		public DigitShapes DigitSubstitution
		{
			get
			{
				return (DigitShapes)this.digitSubstitution;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyDigitSubstitution(value, "DigitSubstitution");
				this.digitSubstitution = (int)value;
			}
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x000779AB File Offset: 0x000769AB
		public object GetFormat(Type formatType)
		{
			if (formatType != typeof(NumberFormatInfo))
			{
				return null;
			}
			return this;
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x000779C0 File Offset: 0x000769C0
		public static NumberFormatInfo ReadOnly(NumberFormatInfo nfi)
		{
			if (nfi == null)
			{
				throw new ArgumentNullException("nfi");
			}
			if (nfi.IsReadOnly)
			{
				return nfi;
			}
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)nfi.MemberwiseClone();
			numberFormatInfo.isReadOnly = true;
			return numberFormatInfo;
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000779FC File Offset: 0x000769FC
		internal static void ValidateParseStyleInteger(NumberStyles style)
		{
			if ((style & ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier)) != NumberStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNumberStyles"), "style");
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None && (style & ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowHexSpecifier)) != NumberStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidHexStyle"));
			}
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x00077A49 File Offset: 0x00076A49
		internal static void ValidateParseStyleFloatingPoint(NumberStyles style)
		{
			if ((style & ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier)) != NumberStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNumberStyles"), "style");
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_HexStyleNotSupported"));
			}
		}

		// Token: 0x040011CE RID: 4558
		private const NumberStyles InvalidNumberStyles = ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier);

		// Token: 0x040011CF RID: 4559
		private static NumberFormatInfo invariantInfo;

		// Token: 0x040011D0 RID: 4560
		internal int[] numberGroupSizes = new int[] { 3 };

		// Token: 0x040011D1 RID: 4561
		internal int[] currencyGroupSizes = new int[] { 3 };

		// Token: 0x040011D2 RID: 4562
		internal int[] percentGroupSizes = new int[] { 3 };

		// Token: 0x040011D3 RID: 4563
		internal string positiveSign = "+";

		// Token: 0x040011D4 RID: 4564
		internal string negativeSign = "-";

		// Token: 0x040011D5 RID: 4565
		internal string numberDecimalSeparator = ".";

		// Token: 0x040011D6 RID: 4566
		internal string numberGroupSeparator = ",";

		// Token: 0x040011D7 RID: 4567
		internal string currencyGroupSeparator = ",";

		// Token: 0x040011D8 RID: 4568
		internal string currencyDecimalSeparator = ".";

		// Token: 0x040011D9 RID: 4569
		internal string currencySymbol = "¤";

		// Token: 0x040011DA RID: 4570
		internal string ansiCurrencySymbol;

		// Token: 0x040011DB RID: 4571
		internal string nanSymbol = "NaN";

		// Token: 0x040011DC RID: 4572
		internal string positiveInfinitySymbol = "Infinity";

		// Token: 0x040011DD RID: 4573
		internal string negativeInfinitySymbol = "-Infinity";

		// Token: 0x040011DE RID: 4574
		internal string percentDecimalSeparator = ".";

		// Token: 0x040011DF RID: 4575
		internal string percentGroupSeparator = ",";

		// Token: 0x040011E0 RID: 4576
		internal string percentSymbol = "%";

		// Token: 0x040011E1 RID: 4577
		internal string perMilleSymbol = "‰";

		// Token: 0x040011E2 RID: 4578
		[OptionalField(VersionAdded = 2)]
		internal string[] nativeDigits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

		// Token: 0x040011E3 RID: 4579
		internal int m_dataItem;

		// Token: 0x040011E4 RID: 4580
		internal int numberDecimalDigits = 2;

		// Token: 0x040011E5 RID: 4581
		internal int currencyDecimalDigits = 2;

		// Token: 0x040011E6 RID: 4582
		internal int currencyPositivePattern;

		// Token: 0x040011E7 RID: 4583
		internal int currencyNegativePattern;

		// Token: 0x040011E8 RID: 4584
		internal int numberNegativePattern = 1;

		// Token: 0x040011E9 RID: 4585
		internal int percentPositivePattern;

		// Token: 0x040011EA RID: 4586
		internal int percentNegativePattern;

		// Token: 0x040011EB RID: 4587
		internal int percentDecimalDigits = 2;

		// Token: 0x040011EC RID: 4588
		[OptionalField(VersionAdded = 2)]
		internal int digitSubstitution = 1;

		// Token: 0x040011ED RID: 4589
		internal bool isReadOnly;

		// Token: 0x040011EE RID: 4590
		internal bool m_useUserOverride;

		// Token: 0x040011EF RID: 4591
		internal bool validForParseAsNumber = true;

		// Token: 0x040011F0 RID: 4592
		internal bool validForParseAsCurrency = true;
	}
}

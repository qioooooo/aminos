using System;

namespace System.Xml.Schema
{
	// Token: 0x0200028A RID: 650
	internal abstract class XmlValueConverter
	{
		// Token: 0x06001DE5 RID: 7653
		public abstract bool ToBoolean(bool value);

		// Token: 0x06001DE6 RID: 7654
		public abstract bool ToBoolean(long value);

		// Token: 0x06001DE7 RID: 7655
		public abstract bool ToBoolean(int value);

		// Token: 0x06001DE8 RID: 7656
		public abstract bool ToBoolean(decimal value);

		// Token: 0x06001DE9 RID: 7657
		public abstract bool ToBoolean(float value);

		// Token: 0x06001DEA RID: 7658
		public abstract bool ToBoolean(double value);

		// Token: 0x06001DEB RID: 7659
		public abstract bool ToBoolean(DateTime value);

		// Token: 0x06001DEC RID: 7660
		public abstract bool ToBoolean(DateTimeOffset value);

		// Token: 0x06001DED RID: 7661
		public abstract bool ToBoolean(string value);

		// Token: 0x06001DEE RID: 7662
		public abstract bool ToBoolean(object value);

		// Token: 0x06001DEF RID: 7663
		public abstract int ToInt32(bool value);

		// Token: 0x06001DF0 RID: 7664
		public abstract int ToInt32(int value);

		// Token: 0x06001DF1 RID: 7665
		public abstract int ToInt32(long value);

		// Token: 0x06001DF2 RID: 7666
		public abstract int ToInt32(decimal value);

		// Token: 0x06001DF3 RID: 7667
		public abstract int ToInt32(float value);

		// Token: 0x06001DF4 RID: 7668
		public abstract int ToInt32(double value);

		// Token: 0x06001DF5 RID: 7669
		public abstract int ToInt32(DateTime value);

		// Token: 0x06001DF6 RID: 7670
		public abstract int ToInt32(DateTimeOffset value);

		// Token: 0x06001DF7 RID: 7671
		public abstract int ToInt32(string value);

		// Token: 0x06001DF8 RID: 7672
		public abstract int ToInt32(object value);

		// Token: 0x06001DF9 RID: 7673
		public abstract long ToInt64(bool value);

		// Token: 0x06001DFA RID: 7674
		public abstract long ToInt64(int value);

		// Token: 0x06001DFB RID: 7675
		public abstract long ToInt64(long value);

		// Token: 0x06001DFC RID: 7676
		public abstract long ToInt64(decimal value);

		// Token: 0x06001DFD RID: 7677
		public abstract long ToInt64(float value);

		// Token: 0x06001DFE RID: 7678
		public abstract long ToInt64(double value);

		// Token: 0x06001DFF RID: 7679
		public abstract long ToInt64(DateTime value);

		// Token: 0x06001E00 RID: 7680
		public abstract long ToInt64(DateTimeOffset value);

		// Token: 0x06001E01 RID: 7681
		public abstract long ToInt64(string value);

		// Token: 0x06001E02 RID: 7682
		public abstract long ToInt64(object value);

		// Token: 0x06001E03 RID: 7683
		public abstract decimal ToDecimal(bool value);

		// Token: 0x06001E04 RID: 7684
		public abstract decimal ToDecimal(int value);

		// Token: 0x06001E05 RID: 7685
		public abstract decimal ToDecimal(long value);

		// Token: 0x06001E06 RID: 7686
		public abstract decimal ToDecimal(decimal value);

		// Token: 0x06001E07 RID: 7687
		public abstract decimal ToDecimal(float value);

		// Token: 0x06001E08 RID: 7688
		public abstract decimal ToDecimal(double value);

		// Token: 0x06001E09 RID: 7689
		public abstract decimal ToDecimal(DateTime value);

		// Token: 0x06001E0A RID: 7690
		public abstract decimal ToDecimal(DateTimeOffset value);

		// Token: 0x06001E0B RID: 7691
		public abstract decimal ToDecimal(string value);

		// Token: 0x06001E0C RID: 7692
		public abstract decimal ToDecimal(object value);

		// Token: 0x06001E0D RID: 7693
		public abstract double ToDouble(bool value);

		// Token: 0x06001E0E RID: 7694
		public abstract double ToDouble(int value);

		// Token: 0x06001E0F RID: 7695
		public abstract double ToDouble(long value);

		// Token: 0x06001E10 RID: 7696
		public abstract double ToDouble(decimal value);

		// Token: 0x06001E11 RID: 7697
		public abstract double ToDouble(float value);

		// Token: 0x06001E12 RID: 7698
		public abstract double ToDouble(double value);

		// Token: 0x06001E13 RID: 7699
		public abstract double ToDouble(DateTime value);

		// Token: 0x06001E14 RID: 7700
		public abstract double ToDouble(DateTimeOffset value);

		// Token: 0x06001E15 RID: 7701
		public abstract double ToDouble(string value);

		// Token: 0x06001E16 RID: 7702
		public abstract double ToDouble(object value);

		// Token: 0x06001E17 RID: 7703
		public abstract float ToSingle(bool value);

		// Token: 0x06001E18 RID: 7704
		public abstract float ToSingle(int value);

		// Token: 0x06001E19 RID: 7705
		public abstract float ToSingle(long value);

		// Token: 0x06001E1A RID: 7706
		public abstract float ToSingle(decimal value);

		// Token: 0x06001E1B RID: 7707
		public abstract float ToSingle(float value);

		// Token: 0x06001E1C RID: 7708
		public abstract float ToSingle(double value);

		// Token: 0x06001E1D RID: 7709
		public abstract float ToSingle(DateTime value);

		// Token: 0x06001E1E RID: 7710
		public abstract float ToSingle(DateTimeOffset value);

		// Token: 0x06001E1F RID: 7711
		public abstract float ToSingle(string value);

		// Token: 0x06001E20 RID: 7712
		public abstract float ToSingle(object value);

		// Token: 0x06001E21 RID: 7713
		public abstract DateTime ToDateTime(bool value);

		// Token: 0x06001E22 RID: 7714
		public abstract DateTime ToDateTime(int value);

		// Token: 0x06001E23 RID: 7715
		public abstract DateTime ToDateTime(long value);

		// Token: 0x06001E24 RID: 7716
		public abstract DateTime ToDateTime(decimal value);

		// Token: 0x06001E25 RID: 7717
		public abstract DateTime ToDateTime(float value);

		// Token: 0x06001E26 RID: 7718
		public abstract DateTime ToDateTime(double value);

		// Token: 0x06001E27 RID: 7719
		public abstract DateTime ToDateTime(DateTime value);

		// Token: 0x06001E28 RID: 7720
		public abstract DateTime ToDateTime(DateTimeOffset value);

		// Token: 0x06001E29 RID: 7721
		public abstract DateTime ToDateTime(string value);

		// Token: 0x06001E2A RID: 7722
		public abstract DateTime ToDateTime(object value);

		// Token: 0x06001E2B RID: 7723
		public abstract DateTimeOffset ToDateTimeOffset(bool value);

		// Token: 0x06001E2C RID: 7724
		public abstract DateTimeOffset ToDateTimeOffset(int value);

		// Token: 0x06001E2D RID: 7725
		public abstract DateTimeOffset ToDateTimeOffset(long value);

		// Token: 0x06001E2E RID: 7726
		public abstract DateTimeOffset ToDateTimeOffset(decimal value);

		// Token: 0x06001E2F RID: 7727
		public abstract DateTimeOffset ToDateTimeOffset(float value);

		// Token: 0x06001E30 RID: 7728
		public abstract DateTimeOffset ToDateTimeOffset(double value);

		// Token: 0x06001E31 RID: 7729
		public abstract DateTimeOffset ToDateTimeOffset(DateTime value);

		// Token: 0x06001E32 RID: 7730
		public abstract DateTimeOffset ToDateTimeOffset(DateTimeOffset value);

		// Token: 0x06001E33 RID: 7731
		public abstract DateTimeOffset ToDateTimeOffset(string value);

		// Token: 0x06001E34 RID: 7732
		public abstract DateTimeOffset ToDateTimeOffset(object value);

		// Token: 0x06001E35 RID: 7733
		public abstract string ToString(bool value);

		// Token: 0x06001E36 RID: 7734
		public abstract string ToString(int value);

		// Token: 0x06001E37 RID: 7735
		public abstract string ToString(long value);

		// Token: 0x06001E38 RID: 7736
		public abstract string ToString(decimal value);

		// Token: 0x06001E39 RID: 7737
		public abstract string ToString(float value);

		// Token: 0x06001E3A RID: 7738
		public abstract string ToString(double value);

		// Token: 0x06001E3B RID: 7739
		public abstract string ToString(DateTime value);

		// Token: 0x06001E3C RID: 7740
		public abstract string ToString(DateTimeOffset value);

		// Token: 0x06001E3D RID: 7741
		public abstract string ToString(string value);

		// Token: 0x06001E3E RID: 7742
		public abstract string ToString(string value, IXmlNamespaceResolver nsResolver);

		// Token: 0x06001E3F RID: 7743
		public abstract string ToString(object value);

		// Token: 0x06001E40 RID: 7744
		public abstract string ToString(object value, IXmlNamespaceResolver nsResolver);

		// Token: 0x06001E41 RID: 7745
		public abstract object ChangeType(bool value, Type destinationType);

		// Token: 0x06001E42 RID: 7746
		public abstract object ChangeType(int value, Type destinationType);

		// Token: 0x06001E43 RID: 7747
		public abstract object ChangeType(long value, Type destinationType);

		// Token: 0x06001E44 RID: 7748
		public abstract object ChangeType(decimal value, Type destinationType);

		// Token: 0x06001E45 RID: 7749
		public abstract object ChangeType(float value, Type destinationType);

		// Token: 0x06001E46 RID: 7750
		public abstract object ChangeType(double value, Type destinationType);

		// Token: 0x06001E47 RID: 7751
		public abstract object ChangeType(DateTime value, Type destinationType);

		// Token: 0x06001E48 RID: 7752
		public abstract object ChangeType(DateTimeOffset value, Type destinationType);

		// Token: 0x06001E49 RID: 7753
		public abstract object ChangeType(string value, Type destinationType);

		// Token: 0x06001E4A RID: 7754
		public abstract object ChangeType(string value, Type destinationType, IXmlNamespaceResolver nsResolver);

		// Token: 0x06001E4B RID: 7755
		public abstract object ChangeType(object value, Type destinationType);

		// Token: 0x06001E4C RID: 7756
		public abstract object ChangeType(object value, Type destinationType, IXmlNamespaceResolver nsResolver);
	}
}

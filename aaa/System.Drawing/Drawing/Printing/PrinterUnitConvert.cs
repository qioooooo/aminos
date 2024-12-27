using System;

namespace System.Drawing.Printing
{
	// Token: 0x02000120 RID: 288
	public sealed class PrinterUnitConvert
	{
		// Token: 0x06000F2E RID: 3886 RVA: 0x0002D769 File Offset: 0x0002C769
		private PrinterUnitConvert()
		{
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0002D774 File Offset: 0x0002C774
		public static double Convert(double value, PrinterUnit fromUnit, PrinterUnit toUnit)
		{
			double num = PrinterUnitConvert.UnitsPerDisplay(fromUnit);
			double num2 = PrinterUnitConvert.UnitsPerDisplay(toUnit);
			return value * num2 / num;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0002D794 File Offset: 0x0002C794
		public static int Convert(int value, PrinterUnit fromUnit, PrinterUnit toUnit)
		{
			return (int)Math.Round(PrinterUnitConvert.Convert((double)value, fromUnit, toUnit));
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0002D7A5 File Offset: 0x0002C7A5
		public static Point Convert(Point value, PrinterUnit fromUnit, PrinterUnit toUnit)
		{
			return new Point(PrinterUnitConvert.Convert(value.X, fromUnit, toUnit), PrinterUnitConvert.Convert(value.Y, fromUnit, toUnit));
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0002D7C8 File Offset: 0x0002C7C8
		public static Size Convert(Size value, PrinterUnit fromUnit, PrinterUnit toUnit)
		{
			return new Size(PrinterUnitConvert.Convert(value.Width, fromUnit, toUnit), PrinterUnitConvert.Convert(value.Height, fromUnit, toUnit));
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x0002D7EB File Offset: 0x0002C7EB
		public static Rectangle Convert(Rectangle value, PrinterUnit fromUnit, PrinterUnit toUnit)
		{
			return new Rectangle(PrinterUnitConvert.Convert(value.X, fromUnit, toUnit), PrinterUnitConvert.Convert(value.Y, fromUnit, toUnit), PrinterUnitConvert.Convert(value.Width, fromUnit, toUnit), PrinterUnitConvert.Convert(value.Height, fromUnit, toUnit));
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x0002D82C File Offset: 0x0002C82C
		public static Margins Convert(Margins value, PrinterUnit fromUnit, PrinterUnit toUnit)
		{
			return new Margins
			{
				Left = PrinterUnitConvert.Convert(value.Left, fromUnit, toUnit),
				Right = PrinterUnitConvert.Convert(value.Right, fromUnit, toUnit),
				Top = PrinterUnitConvert.Convert(value.Top, fromUnit, toUnit),
				Bottom = PrinterUnitConvert.Convert(value.Bottom, fromUnit, toUnit)
			};
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0002D88C File Offset: 0x0002C88C
		private static double UnitsPerDisplay(PrinterUnit unit)
		{
			double num;
			switch (unit)
			{
			case PrinterUnit.Display:
				num = 1.0;
				break;
			case PrinterUnit.ThousandthsOfAnInch:
				num = 10.0;
				break;
			case PrinterUnit.HundredthsOfAMillimeter:
				num = 25.4;
				break;
			case PrinterUnit.TenthsOfAMillimeter:
				num = 2.54;
				break;
			default:
				num = 1.0;
				break;
			}
			return num;
		}
	}
}

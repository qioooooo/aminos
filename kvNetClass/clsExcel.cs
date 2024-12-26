using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace kvNetClass
{
	// Token: 0x02000004 RID: 4
	public class clsExcel
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002770 File Offset: 0x00000970
		public string funExcelNextCell(object oWorkSheet, string sReportCell, clsExcel.Direction Direction = clsExcel.Direction.Down)
		{
			checked
			{
				string text2;
				try
				{
					object[] array;
					bool[] array2;
					object obj = NewLateBinding.LateGet(oWorkSheet, null, "Range", array = new object[] { sReportCell }, null, null, array2 = new bool[] { true });
					if (array2[0])
					{
						sReportCell = (string)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array[0]), typeof(string));
					}
					int num = Conversions.ToInteger(NewLateBinding.LateGet(obj, null, "Row", new object[0], null, null, null));
					obj = NewLateBinding.LateGet(oWorkSheet, null, "Range", array = new object[] { sReportCell }, null, null, array2 = new bool[] { true });
					if (array2[0])
					{
						sReportCell = (string)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array[0]), typeof(string));
					}
					int num2 = Conversions.ToInteger(NewLateBinding.LateGet(obj, null, "Column", new object[0], null, null, null));
					switch (Direction)
					{
					case clsExcel.Direction.Down:
						num++;
						break;
					case clsExcel.Direction.Up:
					{
						bool flag = num < 2;
						if (flag)
						{
							num = 1;
						}
						else
						{
							num--;
						}
						break;
					}
					case clsExcel.Direction.Right:
						num2++;
						break;
					case clsExcel.Direction.Left:
					{
						bool flag2 = num2 < 2;
						if (flag2)
						{
							num2 = 1;
						}
						else
						{
							num2--;
						}
						break;
					}
					}
					obj = NewLateBinding.LateGet(oWorkSheet, null, "Cells", array = new object[] { num, num2 }, null, null, array2 = new bool[] { true, true });
					if (array2[0])
					{
						num = (int)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array[0]), typeof(int));
					}
					if (array2[1])
					{
						num2 = (int)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array[1]), typeof(int));
					}
					string text = Conversions.ToString(NewLateBinding.LateGet(obj, null, "Address", new object[0], null, null, null));
					text2 = text.Replace("$", "");
				}
				catch (Exception ex)
				{
					throw new Exception();
				}
				return text2;
			}
		}

		// Token: 0x0200000C RID: 12
		public enum Direction
		{
			// Token: 0x04000018 RID: 24
			Down,
			// Token: 0x04000019 RID: 25
			Up,
			// Token: 0x0400001A RID: 26
			Right,
			// Token: 0x0400001B RID: 27
			Left
		}
	}
}

using System;
using System.Diagnostics;
using GeFanuc.iFixToolkit.Adapter;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace kvNetClass
{
	// Token: 0x02000005 RID: 5
	public class clsFixHDA
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002998 File Offset: 0x00000B98
		public int subFixHDARead(string sTag, DateTime dDate, string sTime, string sMode, string sDuration, string sInterval, ref float[] afValues, ref int[] aiTimes, ref int[] aiStats, ref int[] aiAlarms, string sHtrPath = "Default")
		{
			int num = 0;
			int[] array = new int[9];
			string text = dDate.ToShortDateString();
			checked
			{
				int num3;
				try
				{
					int num2 = Hda.DefineGroup(out num);
					bool flag = num2 != 11000;
					if (flag)
					{
						num3 = num2;
					}
					else
					{
						bool flag2 = Operators.CompareString(sHtrPath.ToUpper(), "Default".ToUpper(), false) != 0;
						if (flag2)
						{
							num2 = Hda.SetPath(num, sHtrPath);
							bool flag3 = num2 != 11000;
							if (flag3)
							{
								return num2;
							}
						}
						num2 = Hda.SetStart(num, text, sTime);
						bool flag4 = num2 != 11000;
						if (flag4)
						{
							num3 = num2;
						}
						else
						{
							num2 = Hda.SetDuration(num, sDuration);
							bool flag5 = num2 != 11000;
							if (flag5)
							{
								num3 = num2;
							}
							else
							{
								num2 = Hda.SetInterval(num, sInterval);
								bool flag6 = num2 != 11000;
								if (flag6)
								{
									num3 = num2;
								}
								else
								{
									int num4;
									num2 = Hda.NtfCount(num, out num4);
									num2 = Hda.AddNtf(num, out array[num4], sTag);
									bool flag7 = num2 != 11000;
									if (flag7)
									{
										num3 = num2;
									}
									else
									{
										num2 = Hda.SetMode(num, array[0], this.ModeStrToInt(sMode));
										bool flag8 = num2 != 11000;
										if (flag8)
										{
											num3 = num2;
										}
										else
										{
											num2 = Hda.Read(num, 0);
											bool flag9 = num2 != 11000;
											if (flag9)
											{
												num3 = num2;
											}
											else
											{
												int num5;
												num2 = Hda.GetNumSamples(num, array[0], out num5);
												bool flag10 = num2 != 11000;
												if (flag10)
												{
													num3 = num2;
												}
												else
												{
													afValues = new float[num5 - 1 + 1];
													aiTimes = new int[num5 - 1 + 1];
													aiStats = new int[num5 - 1 + 1];
													aiAlarms = new int[num5 - 1 + 1];
													num2 = Hda.GetData(num, array[0], 0, num5, afValues, aiTimes, aiStats, aiAlarms);
													bool flag11 = num2 != 11000;
													if (flag11)
													{
														num3 = num2;
													}
													else
													{
														num2 = Hda.DeleteGroup(num);
														num3 = 11000;
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Debug.Write(ex.Message);
					num3 = Information.Err().Number;
				}
				return num3;
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002BE8 File Offset: 0x00000DE8
		private int ModeStrToInt(string m)
		{
			string text = Strings.UCase(m);
			bool flag = Operators.CompareString(text, Strings.UCase("Sample"), false) == 0;
			int num;
			if (flag)
			{
				num = 4;
			}
			else
			{
				flag = Operators.CompareString(text, Strings.UCase("Average"), false) == 0;
				if (flag)
				{
					num = 1;
				}
				else
				{
					flag = Operators.CompareString(text, Strings.UCase("High"), false) == 0;
					if (flag)
					{
						num = 2;
					}
					else
					{
						flag = Operators.CompareString(text, Strings.UCase("Low"), false) == 0;
						if (flag)
						{
							num = 3;
						}
						else
						{
							flag = Operators.CompareString(text, Strings.UCase("Raw"), false) == 0;
							if (flag)
							{
								num = 5;
							}
							else
							{
								num = -1;
							}
						}
					}
				}
			}
			return num;
		}
	}
}

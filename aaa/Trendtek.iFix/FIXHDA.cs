using System;
using System.Diagnostics;
using GeFanuc.iFixToolkit.Adapter;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Trendtek.iFix
{
	// Token: 0x02000007 RID: 7
	public class FIXHDA
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002288 File Offset: 0x00000688
		[DebuggerNonUserCode]
		public FIXHDA()
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002290 File Offset: 0x00000690
		public int FixHDARead(string sTag, DateTime dDate, string sTime, string sMode, string sDuration, string sInterval, ref float[] afValues, ref int[] aiTimes, ref int[] aiStats, ref int[] aiAlarms, string sHtrPath = "Default")
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
					if (num2 != 11000)
					{
						num3 = num2;
					}
					else
					{
						if (Operators.CompareString(sHtrPath.ToUpper(), "Default".ToUpper(), false) != 0)
						{
							num2 = Hda.SetPath(num, sHtrPath);
							if (num2 != 11000)
							{
								return num2;
							}
						}
						num2 = Hda.SetStart(num, text, sTime);
						if (num2 != 11000)
						{
							num3 = num2;
						}
						else
						{
							num2 = Hda.SetDuration(num, sDuration);
							if (num2 != 11000)
							{
								num3 = num2;
							}
							else
							{
								num2 = Hda.SetInterval(num, sInterval);
								if (num2 != 11000)
								{
									num3 = num2;
								}
								else
								{
									int num4;
									num2 = Hda.NtfCount(num, out num4);
									num2 = Hda.AddNtf(num, out array[num4], sTag);
									if (num2 != 11000)
									{
										num3 = num2;
									}
									else
									{
										num2 = Hda.SetMode(num, array[0], this.ModeStrToInt(sMode));
										if (num2 != 11000)
										{
											num3 = num2;
										}
										else
										{
											num2 = Hda.Read(num, 0);
											if (num2 != 11000)
											{
												num3 = num2;
											}
											else
											{
												int num5;
												num2 = Hda.GetNumSamples(num, array[0], out num5);
												if (num2 != 11000)
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
													if (num2 != 11000)
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
					num3 = Information.Err().Number;
				}
				return num3;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000248C File Offset: 0x0000088C
		private int ModeStrToInt(string m)
		{
			string text = Strings.UCase(m);
			int num;
			if (Operators.CompareString(text, Strings.UCase("Sample"), false) == 0)
			{
				num = 4;
			}
			else if (Operators.CompareString(text, Strings.UCase("Average"), false) == 0)
			{
				num = 1;
			}
			else if (Operators.CompareString(text, Strings.UCase("High"), false) == 0)
			{
				num = 2;
			}
			else if (Operators.CompareString(text, Strings.UCase("Low"), false) == 0)
			{
				num = 3;
			}
			else if (Operators.CompareString(text, Strings.UCase("Raw"), false) == 0)
			{
				num = 5;
			}
			else
			{
				num = -1;
			}
			return num;
		}
	}
}

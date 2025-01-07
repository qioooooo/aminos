using System;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic
{
	[StandardModule]
	public sealed class Financial
	{
		public static double DDB(double Cost, double Salvage, double Life, double Period, double Factor = 2.0)
		{
			if (Factor > 0.0)
			{
				if (Salvage >= 0.0)
				{
					if (Period > 0.0)
					{
						if (Period <= Life)
						{
							if (Cost <= 0.0)
							{
								return 0.0;
							}
							if (Life < 2.0)
							{
								return Cost - Salvage;
							}
							if (Life == 2.0 && Period > 1.0)
							{
								return 0.0;
							}
							if (Life == 2.0 && Period <= 1.0)
							{
								return Cost - Salvage;
							}
							double num;
							double num3;
							if (Period > 1.0)
							{
								num = (Life - Factor) / Life;
								double num2 = Period - 1.0;
								num3 = Factor * Cost / Life * Math.Pow(num, num2);
								double num4 = Cost * (1.0 - Math.Pow(num, Period));
								double num5 = num4 - Cost + Salvage;
								if (num5 > 0.0)
								{
									num3 -= num5;
								}
								double num6;
								if (num3 >= 0.0)
								{
									num6 = num3;
								}
								else
								{
									num6 = 0.0;
								}
								return num6;
							}
							num3 = Cost * Factor / Life;
							num = Cost - Salvage;
							if (num3 > num)
							{
								return num;
							}
							return num3;
						}
					}
				}
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Factor" }));
		}

		public static double FV(double Rate, double NPer, double Pmt, double PV = 0.0, DueDate Due = DueDate.EndOfPeriod)
		{
			return Financial.FV_Internal(Rate, NPer, Pmt, PV, Due);
		}

		private static double FV_Internal(double Rate, double NPer, double Pmt, double PV = 0.0, DueDate Due = DueDate.EndOfPeriod)
		{
			if (Rate == 0.0)
			{
				return -PV - Pmt * NPer;
			}
			double num;
			if (Due != DueDate.EndOfPeriod)
			{
				num = 1.0 + Rate;
			}
			else
			{
				num = 1.0;
			}
			double num2 = 1.0 + Rate;
			double num3 = Math.Pow(num2, NPer);
			return -PV * num3 - Pmt / Rate * num * (num3 - 1.0);
		}

		public static double IPmt(double Rate, double Per, double NPer, double PV, double FV = 0.0, DueDate Due = DueDate.EndOfPeriod)
		{
			double num;
			if (Due != DueDate.EndOfPeriod)
			{
				num = 2.0;
			}
			else
			{
				num = 1.0;
			}
			if (Per <= 0.0 || Per >= NPer + 1.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Per" }));
			}
			if (Due != DueDate.EndOfPeriod && Per == 1.0)
			{
				return 0.0;
			}
			double num2 = Financial.PMT_Internal(Rate, NPer, PV, FV, Due);
			if (Due != DueDate.EndOfPeriod)
			{
				PV += num2;
			}
			double num3 = Financial.FV_Internal(Rate, Per - num, num2, PV, DueDate.EndOfPeriod);
			return num3 * Rate;
		}

		public static double IRR(ref double[] ValueArray, double Guess = 0.1)
		{
			int upperBound;
			try
			{
				upperBound = ValueArray.GetUpperBound(0);
			}
			catch (StackOverflowException ex)
			{
				throw ex;
			}
			catch (OutOfMemoryException ex2)
			{
				throw ex2;
			}
			catch (ThreadAbortException ex3)
			{
				throw ex3;
			}
			catch (Exception)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "ValueArray" }));
			}
			int num = checked(upperBound + 1);
			if (Guess <= -1.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Guess" }));
			}
			if (num <= 1)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "ValueArray" }));
			}
			double num2;
			if (ValueArray[0] > 0.0)
			{
				num2 = ValueArray[0];
			}
			else
			{
				num2 = -ValueArray[0];
			}
			int num3 = 0;
			int num4 = upperBound;
			int i;
			for (i = num3; i <= num4; i = checked(i + 1))
			{
				if (ValueArray[i] > num2)
				{
					num2 = ValueArray[i];
				}
				else if (-ValueArray[i] > num2)
				{
					num2 = -ValueArray[i];
				}
			}
			double num5 = num2 * 1E-07 * 0.01;
			double num6 = Guess;
			double num7 = Financial.OptPV2(ref ValueArray, num6);
			double num8;
			if (num7 > 0.0)
			{
				num8 = num6 + 1E-05;
			}
			else
			{
				num8 = num6 - 1E-05;
			}
			if (num8 <= -1.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Rate" }));
			}
			double num9 = Financial.OptPV2(ref ValueArray, num8);
			i = 0;
			for (;;)
			{
				if (num9 == num7)
				{
					if (num8 > num6)
					{
						num6 -= 1E-05;
					}
					else
					{
						num6 += 1E-05;
					}
					num7 = Financial.OptPV2(ref ValueArray, num6);
					if (num9 == num7)
					{
						break;
					}
				}
				num6 = num8 - (num8 - num6) * num9 / (num9 - num7);
				if (num6 <= -1.0)
				{
					num6 = (num8 - 1.0) * 0.5;
				}
				num7 = Financial.OptPV2(ref ValueArray, num6);
				if (num6 > num8)
				{
					num2 = num6 - num8;
				}
				else
				{
					num2 = num8 - num6;
				}
				double num10;
				if (num7 > 0.0)
				{
					num10 = num7;
				}
				else
				{
					num10 = -num7;
				}
				if (num10 < num5 && num2 < 1E-07)
				{
					return num6;
				}
				num2 = num7;
				num7 = num9;
				num9 = num2;
				num2 = num6;
				num6 = num8;
				num8 = num2;
				checked
				{
					i++;
					if (i > 39)
					{
						goto Block_17;
					}
				}
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
			Block_17:
			throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
		}

		public static double MIRR(ref double[] ValueArray, double FinanceRate, double ReinvestRate)
		{
			if (ValueArray.Rank != 1)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_RankEQOne1", new string[] { "ValueArray" }));
			}
			int num = 0;
			int upperBound = ValueArray.GetUpperBound(0);
			int num2 = checked(upperBound - num + 1);
			if (FinanceRate == -1.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "FinanceRate" }));
			}
			if (ReinvestRate == -1.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "ReinvestRate" }));
			}
			if (num2 <= 1)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "ValueArray" }));
			}
			double num3 = Financial.LDoNPV(FinanceRate, ref ValueArray, -1);
			if (num3 == 0.0)
			{
				throw new DivideByZeroException(Utils.GetResourceString("Financial_CalcDivByZero"));
			}
			double num4 = Financial.LDoNPV(ReinvestRate, ref ValueArray, 1);
			double num5 = ReinvestRate + 1.0;
			double num6 = (double)num2;
			double num7 = -num4 * Math.Pow(num5, num6) / (num3 * (FinanceRate + 1.0));
			if (num7 < 0.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
			}
			num5 = 1.0 / ((double)num2 - 1.0);
			return Math.Pow(num7, num5) - 1.0;
		}

		public static double NPer(double Rate, double Pmt, double PV, double FV = 0.0, DueDate Due = DueDate.EndOfPeriod)
		{
			if (Rate <= -1.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Rate" }));
			}
			if (Rate != 0.0)
			{
				double num;
				if (Due != DueDate.EndOfPeriod)
				{
					num = Pmt * (1.0 + Rate) / Rate;
				}
				else
				{
					num = Pmt / Rate;
				}
				double num2 = -FV + num;
				double num3 = PV + num;
				if (num2 < 0.0 && num3 < 0.0)
				{
					num2 = -1.0 * num2;
					num3 = -1.0 * num3;
				}
				else if (num2 <= 0.0 || num3 <= 0.0)
				{
					throw new ArgumentException(Utils.GetResourceString("Financial_CannotCalculateNPer"));
				}
				double num4 = Rate + 1.0;
				return (Math.Log(num2) - Math.Log(num3)) / Math.Log(num4);
			}
			if (Pmt == 0.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Pmt" }));
			}
			return -(PV + FV) / Pmt;
		}

		public static double NPV(double Rate, ref double[] ValueArray)
		{
			if (ValueArray == null)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "ValueArray" }));
			}
			if (ValueArray.Rank != 1)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_RankEQOne1", new string[] { "ValueArray" }));
			}
			int num = 0;
			int upperBound = ValueArray.GetUpperBound(0);
			int num2 = checked(upperBound - num + 1);
			if (Rate == -1.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Rate" }));
			}
			if (num2 < 1)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "ValueArray" }));
			}
			return Financial.LDoNPV(Rate, ref ValueArray, 0);
		}

		public static double Pmt(double Rate, double NPer, double PV, double FV = 0.0, DueDate Due = DueDate.EndOfPeriod)
		{
			return Financial.PMT_Internal(Rate, NPer, PV, FV, Due);
		}

		private static double PMT_Internal(double Rate, double NPer, double PV, double FV = 0.0, DueDate Due = DueDate.EndOfPeriod)
		{
			if (NPer == 0.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "NPer" }));
			}
			if (Rate == 0.0)
			{
				return (-FV - PV) / NPer;
			}
			double num;
			if (Due != DueDate.EndOfPeriod)
			{
				num = 1.0 + Rate;
			}
			else
			{
				num = 1.0;
			}
			double num2 = Rate + 1.0;
			double num3 = Math.Pow(num2, NPer);
			return (-FV - PV * num3) / (num * (num3 - 1.0)) * Rate;
		}

		public static double PPmt(double Rate, double Per, double NPer, double PV, double FV = 0.0, DueDate Due = DueDate.EndOfPeriod)
		{
			if (Per <= 0.0 || Per >= NPer + 1.0)
			{
				throw new ArgumentException(Utils.GetResourceString("PPMT_PerGT0AndLTNPer", new string[] { "Per" }));
			}
			double num = Financial.PMT_Internal(Rate, NPer, PV, FV, Due);
			double num2 = Financial.IPmt(Rate, Per, NPer, PV, FV, Due);
			return num - num2;
		}

		public static double PV(double Rate, double NPer, double Pmt, double FV = 0.0, DueDate Due = DueDate.EndOfPeriod)
		{
			if (Rate == 0.0)
			{
				return -FV - Pmt * NPer;
			}
			double num;
			if (Due != DueDate.EndOfPeriod)
			{
				num = 1.0 + Rate;
			}
			else
			{
				num = 1.0;
			}
			double num2 = 1.0 + Rate;
			double num3 = Math.Pow(num2, NPer);
			return -(FV + Pmt * num * ((num3 - 1.0) / Rate)) / num3;
		}

		public static double Rate(double NPer, double Pmt, double PV, double FV = 0.0, DueDate Due = DueDate.EndOfPeriod, double Guess = 0.1)
		{
			if (NPer <= 0.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Rate_NPerMustBeGTZero"));
			}
			double num = Guess;
			double num2 = Financial.LEvalRate(num, NPer, Pmt, PV, FV, Due);
			double num3;
			if (num2 > 0.0)
			{
				num3 = num / 2.0;
			}
			else
			{
				num3 = num * 2.0;
			}
			double num4 = Financial.LEvalRate(num3, NPer, Pmt, PV, FV, Due);
			int num5 = 0;
			for (;;)
			{
				if (num4 == num2)
				{
					if (num3 > num)
					{
						num -= 1E-05;
					}
					else
					{
						num -= -1E-05;
					}
					num2 = Financial.LEvalRate(num, NPer, Pmt, PV, FV, Due);
					if (num4 == num2)
					{
						break;
					}
				}
				num = num3 - (num3 - num) * num4 / (num4 - num2);
				num2 = Financial.LEvalRate(num, NPer, Pmt, PV, FV, Due);
				if (Math.Abs(num2) < 1E-07)
				{
					return num;
				}
				double num6 = num2;
				num2 = num4;
				num4 = num6;
				num6 = num;
				num = num3;
				num3 = num6;
				checked
				{
					num5++;
					if (num5 > 39)
					{
						goto Block_7;
					}
				}
			}
			throw new ArgumentException(Utils.GetResourceString("Financial_CalcDivByZero"));
			Block_7:
			throw new ArgumentException(Utils.GetResourceString("Financial_CannotCalculateRate"));
		}

		public static double SLN(double Cost, double Salvage, double Life)
		{
			if (Life == 0.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Financial_LifeNEZero"));
			}
			return (Cost - Salvage) / Life;
		}

		public static double SYD(double Cost, double Salvage, double Life, double Period)
		{
			if (Salvage < 0.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Financial_ArgGEZero1", new string[] { "Salvage" }));
			}
			if (Period > Life)
			{
				throw new ArgumentException(Utils.GetResourceString("Financial_PeriodLELife"));
			}
			if (Period <= 0.0)
			{
				throw new ArgumentException(Utils.GetResourceString("Financial_ArgGTZero1", new string[] { "Period" }));
			}
			double num = (Cost - Salvage) / (Life * (Life + 1.0));
			return num * (Life + 1.0 - Period) * 2.0;
		}

		private static double LEvalRate(double Rate, double NPer, double Pmt, double PV, double dFv, DueDate Due)
		{
			if (Rate == 0.0)
			{
				return PV + Pmt * NPer + dFv;
			}
			double num = Rate + 1.0;
			double num2 = Math.Pow(num, NPer);
			double num3;
			if (Due != DueDate.EndOfPeriod)
			{
				num3 = 1.0 + Rate;
			}
			else
			{
				num3 = 1.0;
			}
			return PV * num2 + Pmt * num3 * (num2 - 1.0) / Rate + dFv;
		}

		private static double LDoNPV(double Rate, ref double[] ValueArray, int iWNType)
		{
			bool flag = iWNType < 0;
			bool flag2 = iWNType > 0;
			double num = 1.0;
			double num2 = 0.0;
			int num3 = 0;
			int upperBound = ValueArray.GetUpperBound(0);
			int num4 = num3;
			int num5 = upperBound;
			for (int i = num4; i <= num5; i = checked(i + 1))
			{
				double num6 = ValueArray[i];
				num += num * Rate;
				if ((!flag || num6 <= 0.0) && (!flag2 || num6 >= 0.0))
				{
					num2 += num6 / num;
				}
			}
			return num2;
		}

		private static double OptPV2(ref double[] ValueArray, double Guess = 0.1)
		{
			int num = 0;
			int upperBound = ValueArray.GetUpperBound(0);
			double num2 = 0.0;
			double num3 = 1.0 + Guess;
			checked
			{
				while (num <= upperBound && ValueArray[num] == 0.0)
				{
					num++;
				}
				int num4 = upperBound;
				int num5 = num;
				for (int i = num4; i >= num5; i += -1)
				{
					num2 /= num3;
					unchecked
					{
						num2 += ValueArray[i];
					}
				}
				return num2;
			}
		}

		private const double cnL_IT_STEP = 1E-05;

		private const double cnL_IT_EPSILON = 1E-07;
	}
}

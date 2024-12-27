using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000765 RID: 1893
	[ComVisible(true)]
	public sealed class SoapDuration
	{
		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x060043E9 RID: 17385 RVA: 0x000E9AB0 File Offset: 0x000E8AB0
		public static string XsdType
		{
			get
			{
				return "duration";
			}
		}

		// Token: 0x060043EA RID: 17386 RVA: 0x000E9AB8 File Offset: 0x000E8AB8
		private static void CarryOver(int inDays, out int years, out int months, out int days)
		{
			years = inDays / 360;
			int num = years * 360;
			months = Math.Max(0, inDays - num) / 30;
			int num2 = months * 30;
			days = Math.Max(0, inDays - (num + num2));
			days = inDays % 30;
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x000E9B00 File Offset: 0x000E8B00
		public static string ToString(TimeSpan timeSpan)
		{
			StringBuilder stringBuilder = new StringBuilder(10);
			stringBuilder.Length = 0;
			if (TimeSpan.Compare(timeSpan, TimeSpan.Zero) < 1)
			{
				stringBuilder.Append('-');
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			SoapDuration.CarryOver(Math.Abs(timeSpan.Days), out num, out num2, out num3);
			stringBuilder.Append('P');
			stringBuilder.Append(num);
			stringBuilder.Append('Y');
			stringBuilder.Append(num2);
			stringBuilder.Append('M');
			stringBuilder.Append(num3);
			stringBuilder.Append("DT");
			stringBuilder.Append(Math.Abs(timeSpan.Hours));
			stringBuilder.Append('H');
			stringBuilder.Append(Math.Abs(timeSpan.Minutes));
			stringBuilder.Append('M');
			stringBuilder.Append(Math.Abs(timeSpan.Seconds));
			long num4 = Math.Abs(timeSpan.Ticks % 864000000000L);
			int num5 = (int)(num4 % 10000000L);
			if (num5 != 0)
			{
				string text = ParseNumbers.IntToString(num5, 10, 7, '0', 0);
				stringBuilder.Append('.');
				stringBuilder.Append(text);
			}
			stringBuilder.Append('S');
			return stringBuilder.ToString();
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x000E9C34 File Offset: 0x000E8C34
		public static TimeSpan Parse(string value)
		{
			int num = 1;
			TimeSpan timeSpan;
			try
			{
				if (value == null)
				{
					timeSpan = TimeSpan.Zero;
				}
				else
				{
					if (value[0] == '-')
					{
						num = -1;
					}
					char[] array = value.ToCharArray();
					string text = "0";
					string text2 = "0";
					string text3 = "0";
					string text4 = "0";
					string text5 = "0";
					string text6 = "0";
					string text7 = "0";
					bool flag = false;
					bool flag2 = false;
					int num2 = 0;
					for (int i = 0; i < array.Length; i++)
					{
						char c = array[i];
						if (c <= 'H')
						{
							if (c != '.')
							{
								if (c != 'D')
								{
									if (c == 'H')
									{
										text4 = new string(array, num2, i - num2);
										num2 = i + 1;
									}
								}
								else
								{
									text3 = new string(array, num2, i - num2);
									num2 = i + 1;
								}
							}
							else
							{
								flag2 = true;
								text6 = new string(array, num2, i - num2);
								num2 = i + 1;
							}
						}
						else if (c != 'M')
						{
							switch (c)
							{
							case 'P':
								num2 = i + 1;
								break;
							case 'Q':
							case 'R':
								break;
							case 'S':
								if (!flag2)
								{
									text6 = new string(array, num2, i - num2);
								}
								else
								{
									text7 = new string(array, num2, i - num2);
								}
								break;
							case 'T':
								flag = true;
								num2 = i + 1;
								break;
							default:
								switch (c)
								{
								case 'Y':
									text = new string(array, num2, i - num2);
									num2 = i + 1;
									break;
								}
								break;
							}
						}
						else
						{
							if (flag)
							{
								text5 = new string(array, num2, i - num2);
							}
							else
							{
								text2 = new string(array, num2, i - num2);
							}
							num2 = i + 1;
						}
					}
					long num3 = (long)num * ((long.Parse(text, CultureInfo.InvariantCulture) * 360L + long.Parse(text2, CultureInfo.InvariantCulture) * 30L + long.Parse(text3, CultureInfo.InvariantCulture)) * 864000000000L + long.Parse(text4, CultureInfo.InvariantCulture) * 36000000000L + long.Parse(text5, CultureInfo.InvariantCulture) * 600000000L + Convert.ToInt64(double.Parse(text6 + "." + text7, CultureInfo.InvariantCulture) * 10000000.0));
					timeSpan = new TimeSpan(num3);
				}
			}
			catch (Exception)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:duration", value }));
			}
			return timeSpan;
		}
	}
}

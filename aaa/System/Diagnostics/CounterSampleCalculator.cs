using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000745 RID: 1861
	public static class CounterSampleCalculator
	{
		// Token: 0x060038C7 RID: 14535 RVA: 0x000EF59C File Offset: 0x000EE59C
		private static float GetElapsedTime(CounterSample oldSample, CounterSample newSample)
		{
			if (newSample.RawValue == 0L)
			{
				return 0f;
			}
			float num = oldSample.CounterFrequency;
			if (oldSample.UnsignedRawValue >= (ulong)newSample.CounterTimeStamp || num <= 0f)
			{
				return 0f;
			}
			float num2 = newSample.CounterTimeStamp - (long)oldSample.UnsignedRawValue;
			return num2 / num;
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x000EF5F9 File Offset: 0x000EE5F9
		public static float ComputeCounterValue(CounterSample newSample)
		{
			return CounterSampleCalculator.ComputeCounterValue(CounterSample.Empty, newSample);
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x000EF608 File Offset: 0x000EE608
		public static float ComputeCounterValue(CounterSample oldSample, CounterSample newSample)
		{
			int counterType = (int)newSample.CounterType;
			if (oldSample.SystemFrequency == 0L)
			{
				if (counterType != 537003008 && counterType != 65536 && counterType != 0 && counterType != 65792 && counterType != 256 && counterType != 1107494144)
				{
					return 0f;
				}
			}
			else if (oldSample.CounterType != newSample.CounterType)
			{
				throw new InvalidOperationException(SR.GetString("MismatchedCounterTypes"));
			}
			if (counterType == 807666944)
			{
				return CounterSampleCalculator.GetElapsedTime(oldSample, newSample);
			}
			NativeMethods.PDH_RAW_COUNTER pdh_RAW_COUNTER = new NativeMethods.PDH_RAW_COUNTER();
			NativeMethods.PDH_RAW_COUNTER pdh_RAW_COUNTER2 = new NativeMethods.PDH_RAW_COUNTER();
			CounterSampleCalculator.FillInValues(oldSample, newSample, pdh_RAW_COUNTER2, pdh_RAW_COUNTER);
			CounterSampleCalculator.LoadPerfCounterDll();
			NativeMethods.PDH_FMT_COUNTERVALUE pdh_FMT_COUNTERVALUE = new NativeMethods.PDH_FMT_COUNTERVALUE();
			long systemFrequency = newSample.SystemFrequency;
			int num = SafeNativeMethods.FormatFromRawValue((uint)counterType, 37376U, ref systemFrequency, pdh_RAW_COUNTER, pdh_RAW_COUNTER2, pdh_FMT_COUNTERVALUE);
			if (num == 0)
			{
				return (float)pdh_FMT_COUNTERVALUE.data;
			}
			if (num == -2147481640 || num == -2147481642 || num == -2147481643)
			{
				return 0f;
			}
			throw new Win32Exception(num, SR.GetString("PerfCounterPdhError", new object[] { num.ToString("x", CultureInfo.InvariantCulture) }));
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x000EF724 File Offset: 0x000EE724
		private static void FillInValues(CounterSample oldSample, CounterSample newSample, NativeMethods.PDH_RAW_COUNTER oldPdhValue, NativeMethods.PDH_RAW_COUNTER newPdhValue)
		{
			int counterType = (int)newSample.CounterType;
			int num = counterType;
			if (num <= 537003264)
			{
				if (num <= 4260864)
				{
					if (num <= 65536)
					{
						if (num != 0 && num != 256 && num != 65536)
						{
							goto IL_03D3;
						}
					}
					else if (num <= 4195328)
					{
						if (num != 65792 && num != 4195328)
						{
							goto IL_03D3;
						}
					}
					else if (num != 4195584)
					{
						if (num != 4260864)
						{
							goto IL_03D3;
						}
						goto IL_0201;
					}
					newPdhValue.FirstValue = newSample.RawValue;
					newPdhValue.SecondValue = 0L;
					oldPdhValue.FirstValue = oldSample.RawValue;
					oldPdhValue.SecondValue = 0L;
					return;
				}
				if (num <= 6620416)
				{
					if (num <= 4523264)
					{
						if (num != 4523008)
						{
							if (num != 4523264)
							{
								goto IL_03D3;
							}
							goto IL_026B;
						}
					}
					else
					{
						if (num == 5571840)
						{
							newPdhValue.FirstValue = newSample.RawValue;
							newPdhValue.SecondValue = newSample.TimeStamp100nSec;
							oldPdhValue.FirstValue = oldSample.RawValue;
							oldPdhValue.SecondValue = oldSample.TimeStamp100nSec;
							return;
						}
						if (num != 6620416)
						{
							goto IL_03D3;
						}
					}
				}
				else if (num <= 272696576)
				{
					if (num != 272696320)
					{
						if (num != 272696576)
						{
							goto IL_03D3;
						}
						goto IL_026B;
					}
				}
				else
				{
					if (num != 537003008 && num != 537003264)
					{
						goto IL_03D3;
					}
					goto IL_039E;
				}
			}
			else
			{
				if (num <= 549585920)
				{
					if (num <= 542180608)
					{
						if (num == 541132032)
						{
							goto IL_026B;
						}
						if (num == 541525248)
						{
							goto IL_039E;
						}
						if (num != 542180608)
						{
							goto IL_03D3;
						}
					}
					else if (num <= 543229184)
					{
						if (num == 542573824)
						{
							goto IL_039E;
						}
						if (num != 543229184)
						{
							goto IL_03D3;
						}
						goto IL_0201;
					}
					else
					{
						if (num != 543622400 && num != 549585920)
						{
							goto IL_03D3;
						}
						goto IL_039E;
					}
				}
				else if (num <= 575735040)
				{
					if (num <= 558957824)
					{
						if (num == 557909248)
						{
							goto IL_026B;
						}
						if (num != 558957824)
						{
							goto IL_03D3;
						}
					}
					else
					{
						if (num == 574686464)
						{
							goto IL_026B;
						}
						if (num != 575735040)
						{
							goto IL_03D3;
						}
					}
				}
				else if (num <= 592512256)
				{
					if (num == 591463680)
					{
						goto IL_026B;
					}
					if (num != 592512256)
					{
						goto IL_03D3;
					}
				}
				else
				{
					if (num != 805438464 && num != 1073874176)
					{
						goto IL_03D3;
					}
					goto IL_039E;
				}
				newPdhValue.FirstValue = newSample.RawValue;
				newPdhValue.SecondValue = newSample.TimeStamp100nSec;
				oldPdhValue.FirstValue = oldSample.RawValue;
				oldPdhValue.SecondValue = oldSample.TimeStamp100nSec;
				if ((counterType & 33554432) == 33554432)
				{
					newPdhValue.MultiCount = (int)newSample.BaseValue;
					oldPdhValue.MultiCount = (int)oldSample.BaseValue;
					return;
				}
				return;
			}
			IL_0201:
			newPdhValue.FirstValue = newSample.RawValue;
			newPdhValue.SecondValue = newSample.TimeStamp;
			oldPdhValue.FirstValue = oldSample.RawValue;
			oldPdhValue.SecondValue = oldSample.TimeStamp;
			return;
			IL_026B:
			newPdhValue.FirstValue = newSample.RawValue;
			newPdhValue.SecondValue = newSample.TimeStamp;
			oldPdhValue.FirstValue = oldSample.RawValue;
			oldPdhValue.SecondValue = oldSample.TimeStamp;
			if (counterType == 574686464 || counterType == 591463680)
			{
				newPdhValue.FirstValue *= (long)((ulong)((uint)newSample.CounterFrequency));
				if (oldSample.CounterFrequency != 0L)
				{
					oldPdhValue.FirstValue *= (long)((ulong)((uint)oldSample.CounterFrequency));
				}
			}
			if ((counterType & 33554432) == 33554432)
			{
				newPdhValue.MultiCount = (int)newSample.BaseValue;
				oldPdhValue.MultiCount = (int)oldSample.BaseValue;
				return;
			}
			return;
			IL_039E:
			newPdhValue.FirstValue = newSample.RawValue;
			newPdhValue.SecondValue = newSample.BaseValue;
			oldPdhValue.FirstValue = oldSample.RawValue;
			oldPdhValue.SecondValue = oldSample.BaseValue;
			return;
			IL_03D3:
			newPdhValue.FirstValue = 0L;
			newPdhValue.SecondValue = 0L;
			oldPdhValue.FirstValue = 0L;
			oldPdhValue.SecondValue = 0L;
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x000EFB24 File Offset: 0x000EEB24
		private static void LoadPerfCounterDll()
		{
			if (CounterSampleCalculator.perfCounterDllLoaded)
			{
				return;
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			IntPtr moduleHandle = NativeMethods.GetModuleHandle("mscorwks.dll");
			if (moduleHandle == IntPtr.Zero)
			{
				throw new Win32Exception();
			}
			int num = 132;
			HandleRef handleRef = new HandleRef(null, moduleHandle);
			StringBuilder stringBuilder;
			for (;;)
			{
				num *= 2;
				stringBuilder = new StringBuilder(num);
				num = UnsafeNativeMethods.GetModuleFileName(handleRef, stringBuilder, num);
				if (num == 0)
				{
					break;
				}
				if (num != stringBuilder.Capacity)
				{
					goto Block_4;
				}
			}
			throw new Win32Exception();
			Block_4:
			string directoryName = Path.GetDirectoryName(stringBuilder.ToString());
			string text = Path.Combine(directoryName, "perfcounter.dll");
			if (SafeNativeMethods.LoadLibrary(text) == IntPtr.Zero)
			{
				throw new Win32Exception();
			}
			CounterSampleCalculator.perfCounterDllLoaded = true;
		}

		// Token: 0x04003266 RID: 12902
		private static bool perfCounterDllLoaded;
	}
}

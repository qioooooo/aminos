using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x0200076C RID: 1900
	internal class CounterDefinitionSample
	{
		// Token: 0x06003A7C RID: 14972 RVA: 0x000F8EC4 File Offset: 0x000F7EC4
		internal CounterDefinitionSample(NativeMethods.PERF_COUNTER_DEFINITION perfCounter, CategorySample categorySample, int instanceNumber)
		{
			this.NameIndex = perfCounter.CounterNameTitleIndex;
			this.CounterType = perfCounter.CounterType;
			this.offset = perfCounter.CounterOffset;
			this.size = perfCounter.CounterSize;
			if (instanceNumber == -1)
			{
				this.instanceValues = new long[1];
			}
			else
			{
				this.instanceValues = new long[instanceNumber];
			}
			this.categorySample = categorySample;
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x000F8F2C File Offset: 0x000F7F2C
		private long ReadValue(IntPtr pointer)
		{
			if (this.size == 4)
			{
				return (long)((ulong)Marshal.ReadInt32((IntPtr)((long)pointer + (long)this.offset)));
			}
			if (this.size == 8)
			{
				return Marshal.ReadInt64((IntPtr)((long)pointer + (long)this.offset));
			}
			return -1L;
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x000F8F80 File Offset: 0x000F7F80
		internal CounterSample GetInstanceValue(string instanceName)
		{
			if (!this.categorySample.InstanceNameTable.ContainsKey(instanceName))
			{
				if (instanceName.Length > 127)
				{
					instanceName = instanceName.Substring(0, 127);
				}
				if (!this.categorySample.InstanceNameTable.ContainsKey(instanceName))
				{
					throw new InvalidOperationException(SR.GetString("CantReadInstance", new object[] { instanceName }));
				}
			}
			int num = (int)this.categorySample.InstanceNameTable[instanceName];
			long num2 = this.instanceValues[num];
			long num3 = 0L;
			if (this.BaseCounterDefinitionSample != null)
			{
				CategorySample categorySample = this.BaseCounterDefinitionSample.categorySample;
				int num4 = (int)categorySample.InstanceNameTable[instanceName];
				num3 = this.BaseCounterDefinitionSample.instanceValues[num4];
			}
			return new CounterSample(num2, num3, this.categorySample.CounterFrequency, this.categorySample.SystemFrequency, this.categorySample.TimeStamp, this.categorySample.TimeStamp100nSec, (PerformanceCounterType)this.CounterType, this.categorySample.CounterTimeStamp);
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x000F9084 File Offset: 0x000F8084
		internal InstanceDataCollection ReadInstanceData(string counterName)
		{
			InstanceDataCollection instanceDataCollection = new InstanceDataCollection(counterName);
			string[] array = new string[this.categorySample.InstanceNameTable.Count];
			this.categorySample.InstanceNameTable.Keys.CopyTo(array, 0);
			int[] array2 = new int[this.categorySample.InstanceNameTable.Count];
			this.categorySample.InstanceNameTable.Values.CopyTo(array2, 0);
			for (int i = 0; i < array.Length; i++)
			{
				long num = 0L;
				if (this.BaseCounterDefinitionSample != null)
				{
					CategorySample categorySample = this.BaseCounterDefinitionSample.categorySample;
					int num2 = (int)categorySample.InstanceNameTable[array[i]];
					num = this.BaseCounterDefinitionSample.instanceValues[num2];
				}
				CounterSample counterSample = new CounterSample(this.instanceValues[array2[i]], num, this.categorySample.CounterFrequency, this.categorySample.SystemFrequency, this.categorySample.TimeStamp, this.categorySample.TimeStamp100nSec, (PerformanceCounterType)this.CounterType, this.categorySample.CounterTimeStamp);
				instanceDataCollection.Add(array[i], new InstanceData(array[i], counterSample));
			}
			return instanceDataCollection;
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x000F91AC File Offset: 0x000F81AC
		internal CounterSample GetSingleValue()
		{
			long num = this.instanceValues[0];
			long num2 = 0L;
			if (this.BaseCounterDefinitionSample != null)
			{
				num2 = this.BaseCounterDefinitionSample.instanceValues[0];
			}
			return new CounterSample(num, num2, this.categorySample.CounterFrequency, this.categorySample.SystemFrequency, this.categorySample.TimeStamp, this.categorySample.TimeStamp100nSec, (PerformanceCounterType)this.CounterType, this.categorySample.CounterTimeStamp);
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x000F9220 File Offset: 0x000F8220
		internal void SetInstanceValue(int index, IntPtr dataRef)
		{
			long num = this.ReadValue(dataRef);
			this.instanceValues[index] = num;
		}

		// Token: 0x0400333C RID: 13116
		internal readonly int NameIndex;

		// Token: 0x0400333D RID: 13117
		internal readonly int CounterType;

		// Token: 0x0400333E RID: 13118
		internal CounterDefinitionSample BaseCounterDefinitionSample;

		// Token: 0x0400333F RID: 13119
		private readonly int size;

		// Token: 0x04003340 RID: 13120
		private readonly int offset;

		// Token: 0x04003341 RID: 13121
		private long[] instanceValues;

		// Token: 0x04003342 RID: 13122
		private CategorySample categorySample;
	}
}

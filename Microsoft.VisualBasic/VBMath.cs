using System;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic
{
	[StandardModule]
	public sealed class VBMath
	{
		public static float Rnd()
		{
			return VBMath.Rnd(1f);
		}

		public static float Rnd(float Number)
		{
			ProjectData projectData = ProjectData.GetProjectData();
			int num = projectData.m_rndSeed;
			if ((double)Number != 0.0)
			{
				if ((double)Number < 0.0)
				{
					num = BitConverter.ToInt32(BitConverter.GetBytes(Number), 0);
					long num2 = (long)num;
					num2 &= (long)((ulong)(-1));
					num = checked((int)((num2 + (num2 >> 24)) & 16777215L));
				}
				checked
				{
					num = (int)((unchecked((long)num) * 1140671485L + 12820163L) & 16777215L);
				}
			}
			projectData.m_rndSeed = num;
			return (float)num / 16777216f;
		}

		public static void Randomize()
		{
			ProjectData projectData = ProjectData.GetProjectData();
			float timer = VBMath.GetTimer();
			int num = projectData.m_rndSeed;
			int num2 = BitConverter.ToInt32(BitConverter.GetBytes(timer), 0);
			num2 = ((num2 & 65535) ^ (num2 >> 16)) << 8;
			num = (num & -16776961) | num2;
			projectData.m_rndSeed = num;
		}

		public static void Randomize(double Number)
		{
			ProjectData projectData = ProjectData.GetProjectData();
			int num = projectData.m_rndSeed;
			int num2;
			if (BitConverter.IsLittleEndian)
			{
				num2 = BitConverter.ToInt32(BitConverter.GetBytes(Number), 4);
			}
			else
			{
				num2 = BitConverter.ToInt32(BitConverter.GetBytes(Number), 0);
			}
			num2 = ((num2 & 65535) ^ (num2 >> 16)) << 8;
			num = (num & -16776961) | num2;
			projectData.m_rndSeed = num;
		}

		private static float GetTimer()
		{
			DateTime now = DateTime.Now;
			return (float)((double)(checked((60 * now.Hour + now.Minute) * 60 + now.Second)) + (double)now.Millisecond / 1000.0);
		}
	}
}

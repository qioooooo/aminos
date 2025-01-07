using System;
using System.Reflection;
using System.Security.Cryptography;

namespace System
{
	internal static class MarvinHash
	{
		private static bool IsItanium()
		{
			PortableExecutableKinds portableExecutableKinds;
			ImageFileMachine imageFileMachine;
			typeof(object).Module.GetPEKind(out portableExecutableKinds, out imageFileMachine);
			return imageFileMachine == ImageFileMachine.IA64;
		}

		public unsafe static int ComputeHash32(string key, ulong seed)
		{
			int num;
			fixed (char* ptr = key)
			{
				num = MarvinHash.ComputeHash32((byte*)ptr, 2 * key.Length, seed);
			}
			return num;
		}

		public unsafe static int ComputeHash32(char[] key, int start, int len, ulong seed)
		{
			int num;
			fixed (char* ptr = &key[start])
			{
				num = MarvinHash.ComputeHash32((byte*)ptr, 2 * len, seed);
			}
			return num;
		}

		private unsafe static int ComputeHash32(byte* data, int count, ulong seed)
		{
			long num = MarvinHash.ComputeHash(data, count, seed);
			return (int)(num >> 32) ^ (int)num;
		}

		private unsafe static long ComputeHashNonAligned(byte* data, int count, ulong seed)
		{
			uint num = (uint)count;
			uint num2 = (uint)seed;
			uint num3 = (uint)(seed >> 32);
			int num4 = 0;
			while (num >= 8U)
			{
				num2 += *(uint*)(data + num4);
				MarvinHash.Block(ref num2, ref num3);
				num2 += *(uint*)(data + num4 + 4);
				MarvinHash.Block(ref num2, ref num3);
				num4 += 8;
				num -= 8U;
			}
			switch (num)
			{
			case 0U:
				break;
			case 1U:
				goto IL_009B;
			case 2U:
				goto IL_00BE;
			case 3U:
				goto IL_00E1;
			case 4U:
				num2 += *(uint*)(data + num4);
				MarvinHash.Block(ref num2, ref num3);
				break;
			case 5U:
				num2 += *(uint*)(data + num4);
				num4 += 4;
				MarvinHash.Block(ref num2, ref num3);
				goto IL_009B;
			case 6U:
				num2 += *(uint*)(data + num4);
				num4 += 4;
				MarvinHash.Block(ref num2, ref num3);
				goto IL_00BE;
			case 7U:
				num2 += *(uint*)(data + num4);
				num4 += 4;
				MarvinHash.Block(ref num2, ref num3);
				goto IL_00E1;
			default:
				goto IL_00F9;
			}
			num2 += 128U;
			goto IL_00F9;
			IL_009B:
			num2 += 32768U | (uint)data[num4];
			goto IL_00F9;
			IL_00BE:
			num2 += 8388608U | (uint)(*(ushort*)(data + num4));
			goto IL_00F9;
			IL_00E1:
			num2 += (uint)(int.MinValue | ((int)(data + num4)[2] << 16) | (int)(*(ushort*)(data + num4)));
			IL_00F9:
			MarvinHash.Block(ref num2, ref num3);
			MarvinHash.Block(ref num2, ref num3);
			return (long)(((ulong)num3 << 32) | (ulong)num2);
		}

		private unsafe static long ComputeHashIA64(byte* data, int count, ulong seed)
		{
			uint num = (uint)count;
			uint num2 = (uint)seed;
			uint num3 = (uint)(seed >> 32);
			byte* ptr = data;
			while (num >= 4U)
			{
				num2 += (uint)((int)(*ptr) | ((int)ptr[1] << 8) | ((int)ptr[2] << 16) | ((int)ptr[3] << 24));
				MarvinHash.Block(ref num2, ref num3);
				ptr += 4;
				num -= 4U;
			}
			switch (num)
			{
			case 0U:
				num2 += 128U;
				break;
			case 1U:
				num2 += 32768U | (uint)(*ptr);
				break;
			case 2U:
				num2 += (uint)(8388608 | (int)(*ptr) | ((int)ptr[1] << 8));
				break;
			case 3U:
				num2 += (uint)(int.MinValue | (int)(*ptr) | ((int)ptr[1] << 8) | ((int)ptr[2] << 16));
				break;
			}
			MarvinHash.Block(ref num2, ref num3);
			MarvinHash.Block(ref num2, ref num3);
			return (long)(((ulong)num3 << 32) | (ulong)num2);
		}

		private static void Block(ref uint rp0, ref uint rp1)
		{
			uint num = rp0;
			uint num2 = rp1;
			num2 ^= num;
			num = MarvinHash._rotl(num, 20);
			num += num2;
			num2 = MarvinHash._rotl(num2, 9);
			num2 ^= num;
			num = MarvinHash._rotl(num, 27);
			num += num2;
			num2 = MarvinHash._rotl(num2, 19);
			rp0 = num;
			rp1 = num2;
		}

		private static uint _rotl(uint value, int shift)
		{
			return (value << shift) | (value >> 32 - shift);
		}

		public unsafe static ulong GenerateSeed()
		{
			byte[] array = new byte[8];
			ulong num;
			using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
			{
				randomNumberGenerator.GetBytes(array);
				try
				{
					fixed (byte* ptr = array)
					{
						num = (ulong)(*(long*)ptr);
					}
				}
				finally
				{
					byte* ptr = null;
				}
			}
			return num;
		}

		private static readonly MarvinHash.ComputeHashImpl ComputeHash = (MarvinHash.IsItanium() ? new MarvinHash.ComputeHashImpl(MarvinHash.ComputeHashIA64) : new MarvinHash.ComputeHashImpl(MarvinHash.ComputeHashNonAligned));

		public static readonly ulong DefaultSeed = MarvinHash.GenerateSeed();

		private unsafe delegate long ComputeHashImpl(byte* data, int count, ulong seed);
	}
}

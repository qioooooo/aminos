using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	[StructLayout(2)]
	public struct almUnion
	{
		[FieldOffset(0)]
		public short bValue;

		[FieldOffset(0)]
		public byte ucValue;

		[FieldOffset(0)]
		public char cValue;

		[FieldOffset(0)]
		public short iValue;

		[FieldOffset(0)]
		public short uiValue;

		[FieldOffset(0)]
		public int lValue;

		[FieldOffset(0)]
		public int ulValue;

		[FieldOffset(0)]
		public float fValue;

		[FieldOffset(0)]
		public double dValue;
	}
}

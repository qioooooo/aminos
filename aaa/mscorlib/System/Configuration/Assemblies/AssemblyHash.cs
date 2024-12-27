using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Assemblies
{
	// Token: 0x02000847 RID: 2119
	[ComVisible(true)]
	[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
	[Serializable]
	public struct AssemblyHash : ICloneable
	{
		// Token: 0x06004E0B RID: 19979 RVA: 0x0010F9EC File Offset: 0x0010E9EC
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public AssemblyHash(byte[] value)
		{
			this._Algorithm = AssemblyHashAlgorithm.SHA1;
			this._Value = null;
			if (value != null)
			{
				int num = value.Length;
				this._Value = new byte[num];
				Array.Copy(value, this._Value, num);
			}
		}

		// Token: 0x06004E0C RID: 19980 RVA: 0x0010FA2C File Offset: 0x0010EA2C
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public AssemblyHash(AssemblyHashAlgorithm algorithm, byte[] value)
		{
			this._Algorithm = algorithm;
			this._Value = null;
			if (value != null)
			{
				int num = value.Length;
				this._Value = new byte[num];
				Array.Copy(value, this._Value, num);
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06004E0D RID: 19981 RVA: 0x0010FA67 File Offset: 0x0010EA67
		// (set) Token: 0x06004E0E RID: 19982 RVA: 0x0010FA6F File Offset: 0x0010EA6F
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public AssemblyHashAlgorithm Algorithm
		{
			get
			{
				return this._Algorithm;
			}
			set
			{
				this._Algorithm = value;
			}
		}

		// Token: 0x06004E0F RID: 19983 RVA: 0x0010FA78 File Offset: 0x0010EA78
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public byte[] GetValue()
		{
			return this._Value;
		}

		// Token: 0x06004E10 RID: 19984 RVA: 0x0010FA80 File Offset: 0x0010EA80
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public void SetValue(byte[] value)
		{
			this._Value = value;
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x0010FA89 File Offset: 0x0010EA89
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public object Clone()
		{
			return new AssemblyHash(this._Algorithm, this._Value);
		}

		// Token: 0x04002828 RID: 10280
		private AssemblyHashAlgorithm _Algorithm;

		// Token: 0x04002829 RID: 10281
		private byte[] _Value;

		// Token: 0x0400282A RID: 10282
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static readonly AssemblyHash Empty = new AssemblyHash(AssemblyHashAlgorithm.None, null);
	}
}

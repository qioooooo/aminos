using System;

namespace System.Data
{
	// Token: 0x0200004F RID: 79
	public interface IDataRecord
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060002FE RID: 766
		int FieldCount { get; }

		// Token: 0x1700005B RID: 91
		object this[int i] { get; }

		// Token: 0x1700005C RID: 92
		object this[string name] { get; }

		// Token: 0x06000301 RID: 769
		string GetName(int i);

		// Token: 0x06000302 RID: 770
		string GetDataTypeName(int i);

		// Token: 0x06000303 RID: 771
		Type GetFieldType(int i);

		// Token: 0x06000304 RID: 772
		object GetValue(int i);

		// Token: 0x06000305 RID: 773
		int GetValues(object[] values);

		// Token: 0x06000306 RID: 774
		int GetOrdinal(string name);

		// Token: 0x06000307 RID: 775
		bool GetBoolean(int i);

		// Token: 0x06000308 RID: 776
		byte GetByte(int i);

		// Token: 0x06000309 RID: 777
		long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length);

		// Token: 0x0600030A RID: 778
		char GetChar(int i);

		// Token: 0x0600030B RID: 779
		long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length);

		// Token: 0x0600030C RID: 780
		Guid GetGuid(int i);

		// Token: 0x0600030D RID: 781
		short GetInt16(int i);

		// Token: 0x0600030E RID: 782
		int GetInt32(int i);

		// Token: 0x0600030F RID: 783
		long GetInt64(int i);

		// Token: 0x06000310 RID: 784
		float GetFloat(int i);

		// Token: 0x06000311 RID: 785
		double GetDouble(int i);

		// Token: 0x06000312 RID: 786
		string GetString(int i);

		// Token: 0x06000313 RID: 787
		decimal GetDecimal(int i);

		// Token: 0x06000314 RID: 788
		DateTime GetDateTime(int i);

		// Token: 0x06000315 RID: 789
		IDataReader GetData(int i);

		// Token: 0x06000316 RID: 790
		bool IsDBNull(int i);
	}
}

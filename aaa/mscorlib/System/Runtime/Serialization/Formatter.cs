using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000346 RID: 838
	[ComVisible(true)]
	[CLSCompliant(false)]
	[Serializable]
	public abstract class Formatter : IFormatter
	{
		// Token: 0x0600215D RID: 8541 RVA: 0x00053F37 File Offset: 0x00052F37
		protected Formatter()
		{
			this.m_objectQueue = new Queue();
			this.m_idGenerator = new ObjectIDGenerator();
		}

		// Token: 0x0600215E RID: 8542
		public abstract object Deserialize(Stream serializationStream);

		// Token: 0x0600215F RID: 8543 RVA: 0x00053F58 File Offset: 0x00052F58
		protected virtual object GetNext(out long objID)
		{
			if (this.m_objectQueue.Count == 0)
			{
				objID = 0L;
				return null;
			}
			object obj = this.m_objectQueue.Dequeue();
			bool flag;
			objID = this.m_idGenerator.HasId(obj, out flag);
			if (flag)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_NoID"));
			}
			return obj;
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x00053FA8 File Offset: 0x00052FA8
		protected virtual long Schedule(object obj)
		{
			if (obj == null)
			{
				return 0L;
			}
			bool flag;
			long id = this.m_idGenerator.GetId(obj, out flag);
			if (flag)
			{
				this.m_objectQueue.Enqueue(obj);
			}
			return id;
		}

		// Token: 0x06002161 RID: 8545
		public abstract void Serialize(Stream serializationStream, object graph);

		// Token: 0x06002162 RID: 8546
		protected abstract void WriteArray(object obj, string name, Type memberType);

		// Token: 0x06002163 RID: 8547
		protected abstract void WriteBoolean(bool val, string name);

		// Token: 0x06002164 RID: 8548
		protected abstract void WriteByte(byte val, string name);

		// Token: 0x06002165 RID: 8549
		protected abstract void WriteChar(char val, string name);

		// Token: 0x06002166 RID: 8550
		protected abstract void WriteDateTime(DateTime val, string name);

		// Token: 0x06002167 RID: 8551
		protected abstract void WriteDecimal(decimal val, string name);

		// Token: 0x06002168 RID: 8552
		protected abstract void WriteDouble(double val, string name);

		// Token: 0x06002169 RID: 8553
		protected abstract void WriteInt16(short val, string name);

		// Token: 0x0600216A RID: 8554
		protected abstract void WriteInt32(int val, string name);

		// Token: 0x0600216B RID: 8555
		protected abstract void WriteInt64(long val, string name);

		// Token: 0x0600216C RID: 8556
		protected abstract void WriteObjectRef(object obj, string name, Type memberType);

		// Token: 0x0600216D RID: 8557 RVA: 0x00053FDC File Offset: 0x00052FDC
		protected virtual void WriteMember(string memberName, object data)
		{
			if (data == null)
			{
				this.WriteObjectRef(data, memberName, typeof(object));
				return;
			}
			Type type = data.GetType();
			if (type == typeof(bool))
			{
				this.WriteBoolean(Convert.ToBoolean(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(char))
			{
				this.WriteChar(Convert.ToChar(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(sbyte))
			{
				this.WriteSByte(Convert.ToSByte(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(byte))
			{
				this.WriteByte(Convert.ToByte(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(short))
			{
				this.WriteInt16(Convert.ToInt16(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(int))
			{
				this.WriteInt32(Convert.ToInt32(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(long))
			{
				this.WriteInt64(Convert.ToInt64(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(float))
			{
				this.WriteSingle(Convert.ToSingle(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(double))
			{
				this.WriteDouble(Convert.ToDouble(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(DateTime))
			{
				this.WriteDateTime(Convert.ToDateTime(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(decimal))
			{
				this.WriteDecimal(Convert.ToDecimal(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(ushort))
			{
				this.WriteUInt16(Convert.ToUInt16(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(uint))
			{
				this.WriteUInt32(Convert.ToUInt32(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(ulong))
			{
				this.WriteUInt64(Convert.ToUInt64(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type.IsArray)
			{
				this.WriteArray(data, memberName, type);
				return;
			}
			if (type.IsValueType)
			{
				this.WriteValueType(data, memberName, type);
				return;
			}
			this.WriteObjectRef(data, memberName, type);
		}

		// Token: 0x0600216E RID: 8558
		[CLSCompliant(false)]
		protected abstract void WriteSByte(sbyte val, string name);

		// Token: 0x0600216F RID: 8559
		protected abstract void WriteSingle(float val, string name);

		// Token: 0x06002170 RID: 8560
		protected abstract void WriteTimeSpan(TimeSpan val, string name);

		// Token: 0x06002171 RID: 8561
		[CLSCompliant(false)]
		protected abstract void WriteUInt16(ushort val, string name);

		// Token: 0x06002172 RID: 8562
		[CLSCompliant(false)]
		protected abstract void WriteUInt32(uint val, string name);

		// Token: 0x06002173 RID: 8563
		[CLSCompliant(false)]
		protected abstract void WriteUInt64(ulong val, string name);

		// Token: 0x06002174 RID: 8564
		protected abstract void WriteValueType(object obj, string name, Type memberType);

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06002175 RID: 8565
		// (set) Token: 0x06002176 RID: 8566
		public abstract ISurrogateSelector SurrogateSelector { get; set; }

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06002177 RID: 8567
		// (set) Token: 0x06002178 RID: 8568
		public abstract SerializationBinder Binder { get; set; }

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06002179 RID: 8569
		// (set) Token: 0x0600217A RID: 8570
		public abstract StreamingContext Context { get; set; }

		// Token: 0x04000DEC RID: 3564
		protected ObjectIDGenerator m_idGenerator;

		// Token: 0x04000DED RID: 3565
		protected Queue m_objectQueue;
	}
}

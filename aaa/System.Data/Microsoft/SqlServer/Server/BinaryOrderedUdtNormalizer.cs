using System;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200028A RID: 650
	internal sealed class BinaryOrderedUdtNormalizer : Normalizer
	{
		// Token: 0x06002230 RID: 8752 RVA: 0x0026CC84 File Offset: 0x0026C084
		[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
		private FieldInfo[] GetFields(Type t)
		{
			return t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x0026CC9C File Offset: 0x0026C09C
		internal BinaryOrderedUdtNormalizer(Type t, bool isTopLevelUdt)
		{
			this.m_skipNormalize = false;
			if (this.m_skipNormalize)
			{
				this.m_isTopLevelUdt = true;
			}
			this.m_isTopLevelUdt = true;
			FieldInfo[] fields = this.GetFields(t);
			this.m_fieldsToNormalize = new FieldInfoEx[fields.Length];
			int num = 0;
			foreach (FieldInfo fieldInfo in fields)
			{
				int num2 = Marshal.OffsetOf(fieldInfo.DeclaringType, fieldInfo.Name).ToInt32();
				this.m_fieldsToNormalize[num++] = new FieldInfoEx(fieldInfo, num2, Normalizer.GetNormalizer(fieldInfo.FieldType));
			}
			Array.Sort<FieldInfoEx>(this.m_fieldsToNormalize);
			if (!this.m_isTopLevelUdt && typeof(INullable).IsAssignableFrom(t))
			{
				PropertyInfo property = t.GetProperty("Null", BindingFlags.Static | BindingFlags.Public);
				if (property == null || property.PropertyType != t)
				{
					FieldInfo field = t.GetField("Null", BindingFlags.Static | BindingFlags.Public);
					if (field == null || field.FieldType != t)
					{
						throw new Exception("could not find Null field/property in nullable type " + t);
					}
					this.NullInstance = field.GetValue(null);
				}
				else
				{
					this.NullInstance = property.GetValue(null, null);
				}
				this.m_PadBuffer = new byte[this.Size - 1];
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06002232 RID: 8754 RVA: 0x0026CDDC File Offset: 0x0026C1DC
		internal bool IsNullable
		{
			get
			{
				return this.NullInstance != null;
			}
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x0026CDF8 File Offset: 0x0026C1F8
		internal void NormalizeTopObject(object udt, Stream s)
		{
			this.Normalize(null, udt, s);
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x0026CE10 File Offset: 0x0026C210
		internal object DeNormalizeTopObject(Type t, Stream s)
		{
			return this.DeNormalizeInternal(t, s);
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x0026CE28 File Offset: 0x0026C228
		private object DeNormalizeInternal(Type t, Stream s)
		{
			object obj = null;
			if (!this.m_isTopLevelUdt && typeof(INullable).IsAssignableFrom(t) && (byte)s.ReadByte() == 0)
			{
				obj = this.NullInstance;
				s.Read(this.m_PadBuffer, 0, this.m_PadBuffer.Length);
				return obj;
			}
			if (obj == null)
			{
				obj = Activator.CreateInstance(t);
			}
			foreach (FieldInfoEx fieldInfoEx in this.m_fieldsToNormalize)
			{
				fieldInfoEx.normalizer.DeNormalize(fieldInfoEx.fieldInfo, obj, s);
			}
			return obj;
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x0026CEB4 File Offset: 0x0026C2B4
		internal override void Normalize(FieldInfo fi, object obj, Stream s)
		{
			object obj2;
			if (fi == null)
			{
				obj2 = obj;
			}
			else
			{
				obj2 = base.GetValue(fi, obj);
			}
			INullable nullable = obj2 as INullable;
			if (nullable != null && !this.m_isTopLevelUdt)
			{
				if (nullable.IsNull)
				{
					s.WriteByte(0);
					s.Write(this.m_PadBuffer, 0, this.m_PadBuffer.Length);
					return;
				}
				s.WriteByte(1);
			}
			foreach (FieldInfoEx fieldInfoEx in this.m_fieldsToNormalize)
			{
				fieldInfoEx.normalizer.Normalize(fieldInfoEx.fieldInfo, obj2, s);
			}
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x0026CF40 File Offset: 0x0026C340
		internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
		{
			base.SetValue(fi, recvr, this.DeNormalizeInternal(fi.FieldType, s));
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06002238 RID: 8760 RVA: 0x0026CF64 File Offset: 0x0026C364
		internal override int Size
		{
			get
			{
				if (this.m_size != 0)
				{
					return this.m_size;
				}
				if (this.IsNullable && !this.m_isTopLevelUdt)
				{
					this.m_size = 1;
				}
				foreach (FieldInfoEx fieldInfoEx in this.m_fieldsToNormalize)
				{
					this.m_size += fieldInfoEx.normalizer.Size;
				}
				return this.m_size;
			}
		}

		// Token: 0x0400164B RID: 5707
		internal readonly FieldInfoEx[] m_fieldsToNormalize;

		// Token: 0x0400164C RID: 5708
		private int m_size;

		// Token: 0x0400164D RID: 5709
		private byte[] m_PadBuffer;

		// Token: 0x0400164E RID: 5710
		internal readonly object NullInstance;

		// Token: 0x0400164F RID: 5711
		private bool m_isTopLevelUdt;
	}
}

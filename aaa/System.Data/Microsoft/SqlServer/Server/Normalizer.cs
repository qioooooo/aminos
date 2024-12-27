using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Security.Permissions;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000289 RID: 649
	internal abstract class Normalizer
	{
		// Token: 0x06002228 RID: 8744 RVA: 0x0026CAD0 File Offset: 0x0026BED0
		internal static Normalizer GetNormalizer(Type t)
		{
			Normalizer normalizer = null;
			if (t.IsPrimitive)
			{
				if (t == typeof(byte))
				{
					normalizer = new ByteNormalizer();
				}
				else if (t == typeof(sbyte))
				{
					normalizer = new SByteNormalizer();
				}
				else if (t == typeof(bool))
				{
					normalizer = new BooleanNormalizer();
				}
				else if (t == typeof(short))
				{
					normalizer = new ShortNormalizer();
				}
				else if (t == typeof(ushort))
				{
					normalizer = new UShortNormalizer();
				}
				else if (t == typeof(int))
				{
					normalizer = new IntNormalizer();
				}
				else if (t == typeof(uint))
				{
					normalizer = new UIntNormalizer();
				}
				else if (t == typeof(float))
				{
					normalizer = new FloatNormalizer();
				}
				else if (t == typeof(double))
				{
					normalizer = new DoubleNormalizer();
				}
				else if (t == typeof(long))
				{
					normalizer = new LongNormalizer();
				}
				else if (t == typeof(ulong))
				{
					normalizer = new ULongNormalizer();
				}
			}
			else if (t.IsValueType)
			{
				normalizer = new BinaryOrderedUdtNormalizer(t, false);
			}
			if (normalizer == null)
			{
				throw new Exception(Res.GetString("Sql_CanotCreateNormalizer", new object[] { t.FullName }));
			}
			normalizer.m_skipNormalize = false;
			return normalizer;
		}

		// Token: 0x06002229 RID: 8745
		internal abstract void Normalize(FieldInfo fi, object recvr, Stream s);

		// Token: 0x0600222A RID: 8746
		internal abstract void DeNormalize(FieldInfo fi, object recvr, Stream s);

		// Token: 0x0600222B RID: 8747 RVA: 0x0026CC20 File Offset: 0x0026C020
		protected void FlipAllBits(byte[] b)
		{
			for (int i = 0; i < b.Length; i++)
			{
				b[i] = ~b[i];
			}
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x0026CC44 File Offset: 0x0026C044
		[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
		protected object GetValue(FieldInfo fi, object obj)
		{
			return fi.GetValue(obj);
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x0026CC58 File Offset: 0x0026C058
		[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
		protected void SetValue(FieldInfo fi, object recvr, object value)
		{
			fi.SetValue(recvr, value);
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x0600222E RID: 8750
		internal abstract int Size { get; }

		// Token: 0x0400164A RID: 5706
		protected bool m_skipNormalize;
	}
}

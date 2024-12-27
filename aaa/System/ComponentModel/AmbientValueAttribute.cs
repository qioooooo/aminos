using System;

namespace System.ComponentModel
{
	// Token: 0x0200008D RID: 141
	[AttributeUsage(AttributeTargets.All)]
	public sealed class AmbientValueAttribute : Attribute
	{
		// Token: 0x060004E4 RID: 1252 RVA: 0x0001593C File Offset: 0x0001493C
		public AmbientValueAttribute(Type type, string value)
		{
			try
			{
				this.value = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
			}
			catch
			{
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00015978 File Offset: 0x00014978
		public AmbientValueAttribute(char value)
		{
			this.value = value;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001598C File Offset: 0x0001498C
		public AmbientValueAttribute(byte value)
		{
			this.value = value;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000159A0 File Offset: 0x000149A0
		public AmbientValueAttribute(short value)
		{
			this.value = value;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000159B4 File Offset: 0x000149B4
		public AmbientValueAttribute(int value)
		{
			this.value = value;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000159C8 File Offset: 0x000149C8
		public AmbientValueAttribute(long value)
		{
			this.value = value;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000159DC File Offset: 0x000149DC
		public AmbientValueAttribute(float value)
		{
			this.value = value;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x000159F0 File Offset: 0x000149F0
		public AmbientValueAttribute(double value)
		{
			this.value = value;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00015A04 File Offset: 0x00014A04
		public AmbientValueAttribute(bool value)
		{
			this.value = value;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00015A18 File Offset: 0x00014A18
		public AmbientValueAttribute(string value)
		{
			this.value = value;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00015A27 File Offset: 0x00014A27
		public AmbientValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00015A36 File Offset: 0x00014A36
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00015A40 File Offset: 0x00014A40
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			AmbientValueAttribute ambientValueAttribute = obj as AmbientValueAttribute;
			if (ambientValueAttribute == null)
			{
				return false;
			}
			if (this.value != null)
			{
				return this.value.Equals(ambientValueAttribute.Value);
			}
			return ambientValueAttribute.Value == null;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00015A82 File Offset: 0x00014A82
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040008B5 RID: 2229
		private readonly object value;
	}
}

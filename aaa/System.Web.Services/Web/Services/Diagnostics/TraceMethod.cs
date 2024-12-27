using System;
using System.Globalization;
using System.Text;

namespace System.Web.Services.Diagnostics
{
	// Token: 0x02000132 RID: 306
	internal class TraceMethod
	{
		// Token: 0x0600097B RID: 2427 RVA: 0x0004567D File Offset: 0x0004467D
		internal TraceMethod(object target, string name, params object[] args)
		{
			this.target = target;
			this.name = name;
			this.args = args;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0004569A File Offset: 0x0004469A
		public override string ToString()
		{
			if (this.call == null)
			{
				this.call = TraceMethod.CallString(this.target, this.name, this.args);
			}
			return this.call;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x000456C8 File Offset: 0x000446C8
		internal static string CallString(object target, string method, params object[] args)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TraceMethod.WriteObjectId(stringBuilder, target);
			stringBuilder.Append(':');
			stringBuilder.Append(':');
			stringBuilder.Append(method);
			stringBuilder.Append('(');
			for (int i = 0; i < args.Length; i++)
			{
				object obj = args[i];
				TraceMethod.WriteObjectId(stringBuilder, obj);
				if (obj != null)
				{
					stringBuilder.Append('=');
					TraceMethod.WriteValue(stringBuilder, obj);
				}
				if (i + 1 < args.Length)
				{
					stringBuilder.Append(',');
					stringBuilder.Append(' ');
				}
			}
			stringBuilder.Append(')');
			return stringBuilder.ToString();
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0004575C File Offset: 0x0004475C
		internal static string MethodId(object target, string method)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TraceMethod.WriteObjectId(stringBuilder, target);
			stringBuilder.Append(':');
			stringBuilder.Append(':');
			stringBuilder.Append(method);
			return stringBuilder.ToString();
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00045798 File Offset: 0x00044798
		private static void WriteObjectId(StringBuilder sb, object o)
		{
			if (o == null)
			{
				sb.Append("(null)");
				return;
			}
			if (o is Type)
			{
				Type type = (Type)o;
				sb.Append(type.FullName);
				if (!type.IsAbstract || !type.IsSealed)
				{
					sb.Append('#');
					sb.Append(TraceMethod.HashString(o));
					return;
				}
			}
			else
			{
				sb.Append(o.GetType().FullName);
				sb.Append('#');
				sb.Append(TraceMethod.HashString(o));
			}
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00045820 File Offset: 0x00044820
		private static void WriteValue(StringBuilder sb, object o)
		{
			if (o == null)
			{
				return;
			}
			if (o is string)
			{
				sb.Append('"');
				sb.Append(o);
				sb.Append('"');
				return;
			}
			Type type = o.GetType();
			if (type.IsArray)
			{
				sb.Append('[');
				sb.Append(((Array)o).Length);
				sb.Append(']');
				return;
			}
			string text = o.ToString();
			if (type.FullName == text)
			{
				sb.Append('.');
				sb.Append('.');
				return;
			}
			sb.Append(text);
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x000458B8 File Offset: 0x000448B8
		private static string HashString(object objectValue)
		{
			if (objectValue == null)
			{
				return "(null)";
			}
			return objectValue.GetHashCode().ToString(NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x04000604 RID: 1540
		private object target;

		// Token: 0x04000605 RID: 1541
		private string name;

		// Token: 0x04000606 RID: 1542
		private object[] args;

		// Token: 0x04000607 RID: 1543
		private string call;
	}
}

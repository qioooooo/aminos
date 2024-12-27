using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x020007A3 RID: 1955
	[ComVisible(true)]
	[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0x002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293", Name = "System.Runtime.Serialization.Formatters.Soap")]
	public sealed class InternalST
	{
		// Token: 0x06004624 RID: 17956 RVA: 0x000F03B2 File Offset: 0x000EF3B2
		private InternalST()
		{
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x000F03BA File Offset: 0x000EF3BA
		[Conditional("_LOGGING")]
		public static void InfoSoap(params object[] messages)
		{
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x000F03BC File Offset: 0x000EF3BC
		public static bool SoapCheckEnabled()
		{
			return BCLDebug.CheckEnabled("Soap");
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x000F03C8 File Offset: 0x000EF3C8
		[Conditional("SER_LOGGING")]
		public static void Soap(params object[] messages)
		{
			if (!(messages[0] is string))
			{
				messages[0] = messages[0].GetType().Name + " ";
				return;
			}
			messages[0] = messages[0] + " ";
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x000F03FF File Offset: 0x000EF3FF
		[Conditional("_DEBUG")]
		public static void SoapAssert(bool condition, string message)
		{
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x000F0401 File Offset: 0x000EF401
		public static void SerializationSetValue(FieldInfo fi, object target, object value)
		{
			if (fi == null)
			{
				throw new ArgumentNullException("fi");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			FormatterServices.SerializationSetValue(fi, target, value);
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x000F0435 File Offset: 0x000EF435
		public static Assembly LoadAssemblyFromString(string assemblyString)
		{
			return FormatterServices.LoadAssemblyFromString(assemblyString);
		}
	}
}

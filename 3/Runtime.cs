using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace Microsoft.JScript
{
	// Token: 0x0200010F RID: 271
	public static class Runtime
	{
		// Token: 0x06000B58 RID: 2904 RVA: 0x00056BBC File Offset: 0x00055BBC
		public new static bool Equals(object v1, object v2)
		{
			Equality equality = new Equality(53);
			return equality.EvaluateEquality(v1, v2);
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00056BDC File Offset: 0x00055BDC
		public static long DoubleToInt64(double val)
		{
			if (double.IsNaN(val))
			{
				return 0L;
			}
			if (-9.223372036854776E+18 <= val && val <= 9.223372036854776E+18)
			{
				return (long)val;
			}
			if (double.IsInfinity(val))
			{
				return 0L;
			}
			double num = Math.IEEERemainder((double)Math.Sign(val) * Math.Floor(Math.Abs(val)), 1.8446744073709552E+19);
			if (num == 9.223372036854776E+18)
			{
				return long.MinValue;
			}
			return (long)num;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00056C54 File Offset: 0x00055C54
		public static long UncheckedDecimalToInt64(decimal val)
		{
			val = decimal.Truncate(val);
			if (val < -9223372036854775808m || 9223372036854775807m < val)
			{
				val = decimal.Remainder(val, 18446744073709551616m);
				if (val < -9223372036854775808m)
				{
					val += 18446744073709551616m;
				}
				else if (val > 9223372036854775807m)
				{
					val -= 18446744073709551616m;
				}
			}
			return (long)val;
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000B5B RID: 2907 RVA: 0x00056D00 File Offset: 0x00055D00
		internal static TypeReferences TypeRefs
		{
			get
			{
				TypeReferences typeReferences = Runtime._typeRefs;
				if (typeReferences == null)
				{
					typeReferences = (Runtime._typeRefs = new TypeReferences(typeof(Runtime).Module));
				}
				return typeReferences;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x00056D34 File Offset: 0x00055D34
		internal static ModuleBuilder ThunkModuleBuilder
		{
			get
			{
				ModuleBuilder moduleBuilder = Runtime._thunkModuleBuilder;
				if (moduleBuilder == null)
				{
					moduleBuilder = (Runtime._thunkModuleBuilder = Runtime.CreateThunkModuleBuilder());
				}
				return moduleBuilder;
			}
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00056D58 File Offset: 0x00055D58
		[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
		[ReflectionPermission(SecurityAction.Assert, ReflectionEmit = true)]
		private static ModuleBuilder CreateThunkModuleBuilder()
		{
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = "JScript Thunk Assembly";
			AssemblyBuilder assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
			ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("JScript Thunk Module");
			moduleBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(SecurityTransparentAttribute).GetConstructor(new Type[0]), new object[0]));
			return moduleBuilder;
		}

		// Token: 0x040006D8 RID: 1752
		private const decimal DecimalTwoToThe64 = 18446744073709551616m;

		// Token: 0x040006D9 RID: 1753
		private static TypeReferences _typeRefs;

		// Token: 0x040006DA RID: 1754
		private static ModuleBuilder _thunkModuleBuilder;
	}
}

using System;

namespace Microsoft.JScript
{
	// Token: 0x02000057 RID: 87
	public class DateConstructor : ScriptFunction
	{
		// Token: 0x0600045E RID: 1118 RVA: 0x00021850 File Offset: 0x00020850
		internal DateConstructor()
			: base(FunctionPrototype.ob, "Date", 7)
		{
			this.originalPrototype = DatePrototype.ob;
			DatePrototype._constructor = this;
			this.proto = DatePrototype.ob;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0002187F File Offset: 0x0002087F
		internal DateConstructor(LenientFunctionPrototype parent, LenientDatePrototype prototypeProp)
			: base(parent, "Date", 7)
		{
			this.originalPrototype = prototypeProp;
			prototypeProp.constructor = this;
			this.proto = prototypeProp;
			this.noExpando = false;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000218AA File Offset: 0x000208AA
		internal override object Call(object[] args, object thisob)
		{
			return this.Invoke();
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000218B4 File Offset: 0x000208B4
		internal DateObject Construct(DateTime dt)
		{
			return new DateObject(this.originalPrototype, (double)dt.ToUniversalTime().Ticks / 10000.0 - 62135596800000.0);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000218F0 File Offset: 0x000208F0
		internal override object Construct(object[] args)
		{
			return this.CreateInstance(args);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000218FC File Offset: 0x000208FC
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public new DateObject CreateInstance(params object[] args)
		{
			if (args.Length == 0)
			{
				return new DateObject(this.originalPrototype, (double)DateTime.Now.ToUniversalTime().Ticks / 10000.0 - 62135596800000.0);
			}
			if (args.Length != 1)
			{
				double num = Convert.ToNumber(args[0]);
				double num2 = Convert.ToNumber(args[1]);
				double num3 = ((args.Length > 2) ? Convert.ToNumber(args[2]) : 1.0);
				double num4 = ((args.Length > 3) ? Convert.ToNumber(args[3]) : 0.0);
				double num5 = ((args.Length > 4) ? Convert.ToNumber(args[4]) : 0.0);
				double num6 = ((args.Length > 5) ? Convert.ToNumber(args[5]) : 0.0);
				double num7 = ((args.Length > 6) ? Convert.ToNumber(args[6]) : 0.0);
				int num8 = (int)Runtime.DoubleToInt64(num);
				if (!double.IsNaN(num) && 0 <= num8 && num8 <= 99)
				{
					num = (double)(num8 + 1900);
				}
				double num9 = DatePrototype.MakeDay(num, num2, num3);
				double num10 = DatePrototype.MakeTime(num4, num5, num6, num7);
				return new DateObject(this.originalPrototype, DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(num9, num10))));
			}
			object obj = args[0];
			IConvertible iconvertible = Convert.GetIConvertible(obj);
			TypeCode typeCode = Convert.GetTypeCode(obj, iconvertible);
			if (typeCode == TypeCode.DateTime)
			{
				return new DateObject(this.originalPrototype, (double)iconvertible.ToDateTime(null).ToUniversalTime().Ticks / 10000.0 - 62135596800000.0);
			}
			object obj2 = Convert.ToPrimitive(obj, PreferredType.Either, ref iconvertible);
			if (Convert.GetTypeCode(obj2, iconvertible) == TypeCode.String)
			{
				return new DateObject(this.originalPrototype, DateConstructor.parse(iconvertible.ToString(null)));
			}
			double num11 = Convert.ToNumber(obj2, iconvertible);
			if (-8640000000000000.0 <= num11 && num11 <= 8640000000000000.0)
			{
				return new DateObject(this.originalPrototype, num11);
			}
			return new DateObject(this.originalPrototype, double.NaN);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00021B14 File Offset: 0x00020B14
		public string Invoke()
		{
			return DatePrototype.DateToString((double)DateTime.Now.ToUniversalTime().Ticks / 10000.0 - 62135596800000.0);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00021B50 File Offset: 0x00020B50
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Date_parse)]
		public static double parse(string str)
		{
			return DatePrototype.ParseDate(str);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00021B58 File Offset: 0x00020B58
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Date_UTC)]
		public static double UTC(object year, object month, object date, object hours, object minutes, object seconds, object ms)
		{
			if (year is Missing)
			{
				return (double)DateTime.Now.ToUniversalTime().Ticks / 10000.0 - 62135596800000.0;
			}
			double num = Convert.ToNumber(year);
			double num2 = ((month is Missing) ? 0.0 : Convert.ToNumber(month));
			double num3 = ((date is Missing) ? 1.0 : Convert.ToNumber(date));
			double num4 = ((hours is Missing) ? 0.0 : Convert.ToNumber(hours));
			double num5 = ((minutes is Missing) ? 0.0 : Convert.ToNumber(minutes));
			double num6 = ((seconds is Missing) ? 0.0 : Convert.ToNumber(seconds));
			double num7 = ((ms is Missing) ? 0.0 : Convert.ToNumber(ms));
			int num8 = (int)Runtime.DoubleToInt64(num);
			if (!double.IsNaN(num) && 0 <= num8 && num8 <= 99)
			{
				num = (double)(num8 + 1900);
			}
			double num9 = DatePrototype.MakeDay(num, num2, num3);
			double num10 = DatePrototype.MakeTime(num4, num5, num6, num7);
			return DatePrototype.TimeClip(DatePrototype.MakeDate(num9, num10));
		}

		// Token: 0x04000203 RID: 515
		internal static readonly DateConstructor ob = new DateConstructor();

		// Token: 0x04000204 RID: 516
		private DatePrototype originalPrototype;
	}
}

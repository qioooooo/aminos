using System;
using System.Collections;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x0200012E RID: 302
	public class VBArrayObject : JSObject
	{
		// Token: 0x06000DE3 RID: 3555 RVA: 0x0005E4E1 File Offset: 0x0005D4E1
		public VBArrayObject(VBArrayPrototype parent, Array array)
			: base(parent)
		{
			this.array = array;
			this.noExpando = false;
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0005E4F8 File Offset: 0x0005D4F8
		internal virtual int dimensions()
		{
			return this.array.Rank;
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0005E508 File Offset: 0x0005D508
		internal virtual object getItem(object[] args)
		{
			if (args == null || args.Length == 0)
			{
				throw new JScriptException(JSError.TooFewParameters);
			}
			if (args.Length == 1)
			{
				return this.array.GetValue(Convert.ToInt32(args[0]));
			}
			if (args.Length == 2)
			{
				return this.array.GetValue(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]));
			}
			if (args.Length == 3)
			{
				return this.array.GetValue(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt32(args[2]));
			}
			int num = args.Length;
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = Convert.ToInt32(args[i]);
			}
			return this.array.GetValue(array);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0005E5BC File Offset: 0x0005D5BC
		internal virtual int lbound(object dimension)
		{
			int num = Convert.ToInt32(dimension);
			return this.array.GetLowerBound(num);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0005E5DC File Offset: 0x0005D5DC
		internal virtual ArrayObject toArray(VsaEngine engine)
		{
			IList list = this.array;
			ArrayObject arrayObject = engine.GetOriginalArrayConstructor().Construct();
			uint num = 0U;
			int count = list.Count;
			IEnumerator enumerator = list.GetEnumerator();
			arrayObject.length = count;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				arrayObject.SetValueAtIndex(num++, obj);
			}
			return arrayObject;
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0005E638 File Offset: 0x0005D638
		internal virtual int ubound(object dimension)
		{
			int num = Convert.ToInt32(dimension);
			return this.array.GetUpperBound(num);
		}

		// Token: 0x04000786 RID: 1926
		private Array array;
	}
}

using System;
using System.Collections;

namespace Microsoft.JScript
{
	// Token: 0x02000061 RID: 97
	internal class DebugArrayFieldEnumerator
	{
		// Token: 0x060004D3 RID: 1235 RVA: 0x0002466E File Offset: 0x0002366E
		internal DebugArrayFieldEnumerator(ScriptObjectPropertyEnumerator enumerator, ArrayObject arrayObject)
		{
			this.enumerator = enumerator;
			this.arrayObject = arrayObject;
			this.EnsureCount();
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0002468C File Offset: 0x0002368C
		internal DynamicFieldInfo[] Next(int count)
		{
			DynamicFieldInfo[] array2;
			try
			{
				ArrayList arrayList = new ArrayList();
				while (count > 0 && this.enumerator.MoveNext())
				{
					string text = (string)this.enumerator.Current;
					arrayList.Add(new DynamicFieldInfo(text, this.arrayObject.GetMemberValue(text)));
					count--;
				}
				DynamicFieldInfo[] array = new DynamicFieldInfo[arrayList.Count];
				arrayList.CopyTo(array);
				array2 = array;
			}
			catch
			{
				array2 = new DynamicFieldInfo[0];
			}
			return array2;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00024714 File Offset: 0x00023714
		internal int GetCount()
		{
			return this.count;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0002471C File Offset: 0x0002371C
		internal void Skip(int count)
		{
			while (count > 0 && this.enumerator.MoveNext())
			{
				count--;
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00024736 File Offset: 0x00023736
		internal void Reset()
		{
			this.enumerator.Reset();
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00024743 File Offset: 0x00023743
		internal void EnsureCount()
		{
			this.enumerator.Reset();
			this.count = 0;
			while (this.enumerator.MoveNext())
			{
				this.count++;
			}
			this.enumerator.Reset();
		}

		// Token: 0x0400022C RID: 556
		private ScriptObjectPropertyEnumerator enumerator;

		// Token: 0x0400022D RID: 557
		private int count;

		// Token: 0x0400022E RID: 558
		private ArrayObject arrayObject;
	}
}

using System;
using System.Collections;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000056 RID: 86
	internal sealed class CustomAttributeList : AST
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x00021510 File Offset: 0x00020510
		internal CustomAttributeList(Context context)
			: base(context)
		{
			this.list = new ArrayList();
			this.customAttributes = null;
			this.alreadyPartiallyEvaluated = false;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00021532 File Offset: 0x00020532
		internal void Append(CustomAttribute elem)
		{
			this.list.Add(elem);
			this.context.UpdateWith(elem.context);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00021554 File Offset: 0x00020554
		internal bool ContainsExpandoAttribute()
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				CustomAttribute customAttribute = (CustomAttribute)this.list[i];
				if (customAttribute != null && customAttribute.IsExpandoAttribute())
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0002159C File Offset: 0x0002059C
		internal CustomAttribute GetAttribute(Type attributeClass)
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				CustomAttribute customAttribute = (CustomAttribute)this.list[i];
				if (customAttribute != null)
				{
					object type = customAttribute.type;
					if (type is Type && type == attributeClass)
					{
						return (CustomAttribute)this.list[i];
					}
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x000215FC File Offset: 0x000205FC
		internal override object Evaluate()
		{
			return this.Evaluate(false);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00021608 File Offset: 0x00020608
		internal object Evaluate(bool getForProperty)
		{
			int count = this.list.Count;
			ArrayList arrayList = new ArrayList(count);
			for (int i = 0; i < count; i++)
			{
				CustomAttribute customAttribute = (CustomAttribute)this.list[i];
				if (customAttribute != null)
				{
					if (customAttribute.raiseToPropertyLevel)
					{
						if (!getForProperty)
						{
							goto IL_0049;
						}
					}
					else if (getForProperty)
					{
						goto IL_0049;
					}
					arrayList.Add(customAttribute.Evaluate());
				}
				IL_0049:;
			}
			object[] array = new object[arrayList.Count];
			arrayList.CopyTo(array);
			return array;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00021680 File Offset: 0x00020680
		internal CustomAttributeBuilder[] GetCustomAttributeBuilders(bool getForProperty)
		{
			this.customAttributes = new ArrayList(this.list.Count);
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				CustomAttribute customAttribute = (CustomAttribute)this.list[i];
				if (customAttribute != null)
				{
					if (customAttribute.raiseToPropertyLevel)
					{
						if (!getForProperty)
						{
							goto IL_0062;
						}
					}
					else if (getForProperty)
					{
						goto IL_0062;
					}
					CustomAttributeBuilder customAttribute2 = customAttribute.GetCustomAttribute();
					if (customAttribute2 != null)
					{
						this.customAttributes.Add(customAttribute2);
					}
				}
				IL_0062:
				i++;
			}
			CustomAttributeBuilder[] array = new CustomAttributeBuilder[this.customAttributes.Count];
			this.customAttributes.CopyTo(array);
			return array;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00021718 File Offset: 0x00020718
		internal override AST PartiallyEvaluate()
		{
			if (this.alreadyPartiallyEvaluated)
			{
				return this;
			}
			this.alreadyPartiallyEvaluated = true;
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				this.list[i] = ((CustomAttribute)this.list[i]).PartiallyEvaluate();
				i++;
			}
			int j = 0;
			int count2 = this.list.Count;
			while (j < count2)
			{
				CustomAttribute customAttribute = (CustomAttribute)this.list[j];
				if (customAttribute != null)
				{
					object typeIfAttributeHasToBeUnique = customAttribute.GetTypeIfAttributeHasToBeUnique();
					if (typeIfAttributeHasToBeUnique != null)
					{
						for (int k = j + 1; k < count2; k++)
						{
							CustomAttribute customAttribute2 = (CustomAttribute)this.list[k];
							if (customAttribute2 != null && typeIfAttributeHasToBeUnique == customAttribute2.type)
							{
								customAttribute2.context.HandleError(JSError.CustomAttributeUsedMoreThanOnce);
								this.list[k] = null;
							}
						}
					}
				}
				j++;
			}
			return this;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00021801 File Offset: 0x00020801
		internal void Remove(CustomAttribute elem)
		{
			this.list.Remove(elem);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00021810 File Offset: 0x00020810
		internal void SetTarget(AST target)
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				((CustomAttribute)this.list[i]).SetTarget(target);
				i++;
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0002184C File Offset: 0x0002084C
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0002184E File Offset: 0x0002084E
		internal override void TranslateToILInitializer(ILGenerator il)
		{
		}

		// Token: 0x04000200 RID: 512
		private ArrayList list;

		// Token: 0x04000201 RID: 513
		private ArrayList customAttributes;

		// Token: 0x04000202 RID: 514
		private bool alreadyPartiallyEvaluated;
	}
}

using System;
using System.CodeDom;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200024A RID: 586
	public class ImageListCodeDomSerializer : CodeDomSerializer
	{
		// Token: 0x06001652 RID: 5714 RVA: 0x00074760 File Offset: 0x00073760
		public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
		{
			if (manager == null || codeObject == null)
			{
				throw new ArgumentNullException((manager == null) ? "manager" : "codeObject");
			}
			CodeDomSerializer codeDomSerializer = (CodeDomSerializer)manager.GetSerializer(typeof(Component), typeof(CodeDomSerializer));
			if (codeDomSerializer == null)
			{
				return null;
			}
			return codeDomSerializer.Deserialize(manager, codeObject);
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x000747B8 File Offset: 0x000737B8
		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			CodeDomSerializer codeDomSerializer = (CodeDomSerializer)manager.GetSerializer(typeof(ImageList).BaseType, typeof(CodeDomSerializer));
			object obj = codeDomSerializer.Serialize(manager, value);
			ImageList imageList = value as ImageList;
			if (imageList != null)
			{
				StringCollection keys = imageList.Images.Keys;
				if (obj is CodeStatementCollection)
				{
					CodeExpression expression = base.GetExpression(manager, value);
					if (expression != null)
					{
						CodeExpression codeExpression = new CodePropertyReferenceExpression(expression, "Images");
						if (codeExpression != null)
						{
							for (int i = 0; i < keys.Count; i++)
							{
								if (keys[i] != null || keys[i].Length != 0)
								{
									CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(codeExpression, "SetKeyName", new CodeExpression[]
									{
										new CodePrimitiveExpression(i),
										new CodePrimitiveExpression(keys[i])
									});
									((CodeStatementCollection)obj).Add(codeMethodInvokeExpression);
								}
							}
						}
					}
				}
			}
			return obj;
		}
	}
}

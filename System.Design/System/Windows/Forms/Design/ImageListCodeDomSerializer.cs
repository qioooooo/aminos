using System;
using System.CodeDom;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace System.Windows.Forms.Design
{
	public class ImageListCodeDomSerializer : CodeDomSerializer
	{
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

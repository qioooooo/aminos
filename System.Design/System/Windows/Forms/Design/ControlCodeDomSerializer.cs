using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms.Design
{
	internal class ControlCodeDomSerializer : CodeDomSerializer
	{
		public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
		{
			if (manager == null || codeObject == null)
			{
				throw new ArgumentNullException((manager == null) ? "manager" : "codeObject");
			}
			IContainer container = (IContainer)manager.GetService(typeof(IContainer));
			ArrayList arrayList = null;
			if (container != null)
			{
				arrayList = new ArrayList(container.Components.Count);
				foreach (object obj in container.Components)
				{
					IComponent component = (IComponent)obj;
					Control control = component as Control;
					if (control != null)
					{
						control.SuspendLayout();
						arrayList.Add(control);
					}
				}
			}
			object obj2 = null;
			try
			{
				CodeDomSerializer codeDomSerializer = (CodeDomSerializer)manager.GetSerializer(typeof(Component), typeof(CodeDomSerializer));
				if (codeDomSerializer == null)
				{
					return null;
				}
				obj2 = codeDomSerializer.Deserialize(manager, codeObject);
			}
			finally
			{
				if (arrayList != null)
				{
					foreach (object obj3 in arrayList)
					{
						Control control2 = (Control)obj3;
						control2.ResumeLayout(true);
					}
				}
			}
			return obj2;
		}

		private bool HasAutoSizedChildren(Control parent)
		{
			foreach (object obj in parent.Controls)
			{
				Control control = (Control)obj;
				if (control.AutoSize)
				{
					return true;
				}
			}
			return false;
		}

		private bool HasMixedInheritedChildren(Control parent)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (object obj in parent.Controls)
			{
				Control control = (Control)obj;
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(control)[typeof(InheritanceAttribute)];
				if (inheritanceAttribute != null && inheritanceAttribute.InheritanceLevel != InheritanceLevel.NotInherited)
				{
					flag = true;
				}
				else
				{
					flag2 = true;
				}
				if (flag && flag2)
				{
					return true;
				}
			}
			return false;
		}

		protected virtual bool HasSitedNonReadonlyChildren(Control parent)
		{
			if (!parent.HasChildren)
			{
				return false;
			}
			foreach (object obj in parent.Controls)
			{
				Control control = (Control)obj;
				if (control.Site != null && control.Site.DesignMode)
				{
					InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(control)[typeof(InheritanceAttribute)];
					if (inheritanceAttribute != null && inheritanceAttribute.InheritanceLevel != InheritanceLevel.InheritedReadOnly)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			if (manager == null || value == null)
			{
				throw new ArgumentNullException((manager == null) ? "manager" : "value");
			}
			CodeDomSerializer codeDomSerializer = (CodeDomSerializer)manager.GetSerializer(typeof(Component), typeof(CodeDomSerializer));
			if (codeDomSerializer == null)
			{
				return null;
			}
			object obj = codeDomSerializer.Serialize(manager, value);
			InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(value)[typeof(InheritanceAttribute)];
			InheritanceLevel inheritanceLevel = InheritanceLevel.NotInherited;
			if (inheritanceAttribute != null)
			{
				inheritanceLevel = inheritanceAttribute.InheritanceLevel;
			}
			if (inheritanceLevel != InheritanceLevel.InheritedReadOnly)
			{
				IDesignerHost designerHost = (IDesignerHost)manager.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(designerHost.RootComponent)["Localizable"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool) && (bool)propertyDescriptor.GetValue(designerHost.RootComponent))
					{
						this.SerializeControlHierarchy(manager, designerHost, value);
					}
				}
				CodeStatementCollection codeStatementCollection = obj as CodeStatementCollection;
				if (codeStatementCollection != null)
				{
					Control control = (Control)value;
					if ((designerHost != null && control == designerHost.RootComponent) || this.HasSitedNonReadonlyChildren(control))
					{
						this.SerializeSuspendLayout(manager, codeStatementCollection, value);
						this.SerializeResumeLayout(manager, codeStatementCollection, value);
						ControlDesigner controlDesigner = designerHost.GetDesigner(control) as ControlDesigner;
						if (this.HasAutoSizedChildren(control) || (controlDesigner != null && controlDesigner.SerializePerformLayout))
						{
							this.SerializePerformLayout(manager, codeStatementCollection, value);
						}
					}
					if (this.HasMixedInheritedChildren(control))
					{
						this.SerializeZOrder(manager, codeStatementCollection, control);
					}
				}
			}
			return obj;
		}

		private void SerializeControlHierarchy(IDesignerSerializationManager manager, IDesignerHost host, object value)
		{
			Control control = value as Control;
			if (control != null)
			{
				string text;
				if (control == host.RootComponent)
				{
					text = "$this";
					using (IEnumerator enumerator = host.Container.Components.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							IComponent component = (IComponent)obj;
							if (!(component is Control) && !TypeDescriptor.GetAttributes(component).Contains(InheritanceAttribute.InheritedReadOnly))
							{
								string name = manager.GetName(component);
								string assemblyQualifiedName = component.GetType().AssemblyQualifiedName;
								if (name != null)
								{
									base.SerializeResourceInvariant(manager, ">>" + name + ".Name", name);
									base.SerializeResourceInvariant(manager, ">>" + name + ".Type", assemblyQualifiedName);
								}
							}
						}
						goto IL_00CF;
					}
				}
				text = manager.GetName(value);
				if (text == null)
				{
					return;
				}
				IL_00CF:
				base.SerializeResourceInvariant(manager, ">>" + text + ".Name", manager.GetName(value));
				base.SerializeResourceInvariant(manager, ">>" + text + ".Type", control.GetType().AssemblyQualifiedName);
				Control parent = control.Parent;
				if (parent != null && parent.Site != null)
				{
					string text2;
					if (parent == host.RootComponent)
					{
						text2 = "$this";
					}
					else
					{
						text2 = manager.GetName(parent);
					}
					if (text2 != null)
					{
						base.SerializeResourceInvariant(manager, ">>" + text + ".Parent", text2);
					}
					for (int i = 0; i < parent.Controls.Count; i++)
					{
						if (parent.Controls[i] == control)
						{
							base.SerializeResourceInvariant(manager, ">>" + text + ".ZOrder", i.ToString(CultureInfo.InvariantCulture));
							return;
						}
					}
				}
			}
		}

		private void SerializeMethodInvocation(IDesignerSerializationManager manager, CodeStatementCollection statements, object control, string methodName, CodeExpressionCollection parameters, Type[] paramTypes, ControlCodeDomSerializer.StatementOrdering ordering)
		{
			using (CodeDomSerializerBase.TraceScope("ControlCodeDomSerializer::SerializeMethodInvocation(" + methodName + ")"))
			{
				manager.GetName(control);
				MethodInfo method = TypeDescriptor.GetReflectionType(control).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public, null, paramTypes, null);
				if (method != null)
				{
					CodeExpression codeExpression = base.SerializeToExpression(manager, control);
					CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(codeExpression, methodName);
					CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
					codeMethodInvokeExpression.Method = codeMethodReferenceExpression;
					if (parameters != null)
					{
						codeMethodInvokeExpression.Parameters.AddRange(parameters);
					}
					CodeExpressionStatement codeExpressionStatement = new CodeExpressionStatement(codeMethodInvokeExpression);
					switch (ordering)
					{
					case ControlCodeDomSerializer.StatementOrdering.Prepend:
						codeExpressionStatement.UserData["statement-ordering"] = "begin";
						break;
					case ControlCodeDomSerializer.StatementOrdering.Append:
						codeExpressionStatement.UserData["statement-ordering"] = "end";
						break;
					}
					statements.Add(codeExpressionStatement);
				}
			}
		}

		private void SerializePerformLayout(IDesignerSerializationManager manager, CodeStatementCollection statements, object control)
		{
			this.SerializeMethodInvocation(manager, statements, control, "PerformLayout", null, new Type[0], ControlCodeDomSerializer.StatementOrdering.Append);
		}

		private void SerializeResumeLayout(IDesignerSerializationManager manager, CodeStatementCollection statements, object control)
		{
			CodeExpressionCollection codeExpressionCollection = new CodeExpressionCollection();
			codeExpressionCollection.Add(new CodePrimitiveExpression(false));
			Type[] array = new Type[] { typeof(bool) };
			this.SerializeMethodInvocation(manager, statements, control, "ResumeLayout", codeExpressionCollection, array, ControlCodeDomSerializer.StatementOrdering.Append);
		}

		private void SerializeSuspendLayout(IDesignerSerializationManager manager, CodeStatementCollection statements, object control)
		{
			this.SerializeMethodInvocation(manager, statements, control, "SuspendLayout", null, new Type[0], ControlCodeDomSerializer.StatementOrdering.Prepend);
		}

		private void SerializeZOrder(IDesignerSerializationManager manager, CodeStatementCollection statements, Control control)
		{
			using (CodeDomSerializerBase.TraceScope("ControlCodeDomSerializer::SerializeZOrder()"))
			{
				for (int i = control.Controls.Count - 1; i >= 0; i--)
				{
					Control control2 = control.Controls[i];
					if (control2.Site != null && control2.Site.Container == control.Site.Container)
					{
						InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(control2)[typeof(InheritanceAttribute)];
						if (inheritanceAttribute.InheritanceLevel != InheritanceLevel.InheritedReadOnly)
						{
							CodeExpression codeExpression = new CodePropertyReferenceExpression(base.SerializeToExpression(manager, control), "Controls");
							CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(codeExpression, "SetChildIndex");
							CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
							codeMethodInvokeExpression.Method = codeMethodReferenceExpression;
							CodeExpression codeExpression2 = base.SerializeToExpression(manager, control2);
							codeMethodInvokeExpression.Parameters.Add(codeExpression2);
							codeMethodInvokeExpression.Parameters.Add(base.SerializeToExpression(manager, 0));
							CodeExpressionStatement codeExpressionStatement = new CodeExpressionStatement(codeMethodInvokeExpression);
							statements.Add(codeExpressionStatement);
						}
					}
				}
			}
		}

		private enum StatementOrdering
		{
			Prepend,
			Append
		}
	}
}

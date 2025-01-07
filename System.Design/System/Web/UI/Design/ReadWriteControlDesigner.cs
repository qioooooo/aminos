using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	[Obsolete("The recommended alternative is ContainerControlDesigner because it uses an EditableDesignerRegion for editing the content. Designer regions allow for better control of the content being edited. http://go.microsoft.com/fwlink/?linkid=14202")]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ReadWriteControlDesigner : ControlDesigner
	{
		public ReadWriteControlDesigner()
		{
			base.ReadOnlyInternal = false;
		}

		public override string GetDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml();
		}

		public override void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
		{
			base.OnComponentChanged(sender, ce);
			if (base.IsIgnoringComponentChanges)
			{
				return;
			}
			if (!base.IsWebControl || base.DesignTimeElementInternal == null)
			{
				return;
			}
			MemberDescriptor member = ce.Member;
			object obj = ce.NewValue;
			Type type = Type.GetType("System.ComponentModel.ReflectPropertyDescriptor, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			if (member != null && member.GetType() == type)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)member;
				if (member.Name.Equals("Font"))
				{
					WebControl webControl = (WebControl)base.Component;
					obj = webControl.Font.Name;
					this.MapPropertyToStyle("Font.Name", obj);
					obj = webControl.Font.Size;
					this.MapPropertyToStyle("Font.Size", obj);
					obj = webControl.Font.Bold;
					this.MapPropertyToStyle("Font.Bold", obj);
					obj = webControl.Font.Italic;
					this.MapPropertyToStyle("Font.Italic", obj);
					obj = webControl.Font.Underline;
					this.MapPropertyToStyle("Font.Underline", obj);
					obj = webControl.Font.Strikeout;
					this.MapPropertyToStyle("Font.Strikeout", obj);
					obj = webControl.Font.Overline;
					this.MapPropertyToStyle("Font.Overline", obj);
					return;
				}
				if (obj != null)
				{
					if (propertyDescriptor.PropertyType == typeof(Color))
					{
						obj = ColorTranslator.ToHtml((Color)obj);
					}
					this.MapPropertyToStyle(propertyDescriptor.Name, obj);
				}
			}
		}

		protected virtual void MapPropertyToStyle(string propName, object varPropValue)
		{
			if (this.BehaviorInternal == null)
			{
				return;
			}
			if (propName == null || varPropValue == null)
			{
				return;
			}
			try
			{
				if (propName.Equals("BackColor"))
				{
					this.BehaviorInternal.SetStyleAttribute("backgroundColor", true, varPropValue, true);
				}
				else if (propName.Equals("ForeColor"))
				{
					this.BehaviorInternal.SetStyleAttribute("color", true, varPropValue, true);
				}
				else if (propName.Equals("BorderWidth"))
				{
					string text = Convert.ToString(varPropValue, CultureInfo.InvariantCulture);
					this.BehaviorInternal.SetStyleAttribute("borderWidth", true, text, true);
				}
				else if (propName.Equals("BorderStyle"))
				{
					string text2;
					if ((BorderStyle)varPropValue == BorderStyle.NotSet)
					{
						text2 = string.Empty;
					}
					else
					{
						text2 = Enum.Format(typeof(BorderStyle), (BorderStyle)varPropValue, "G");
					}
					this.BehaviorInternal.SetStyleAttribute("borderStyle", true, text2, true);
				}
				else if (propName.Equals("BorderColor"))
				{
					this.BehaviorInternal.SetStyleAttribute("borderColor", true, Convert.ToString(varPropValue, CultureInfo.InvariantCulture), true);
				}
				else if (propName.Equals("Height"))
				{
					this.BehaviorInternal.SetStyleAttribute("height", true, Convert.ToString(varPropValue, CultureInfo.InvariantCulture), true);
				}
				else if (propName.Equals("Width"))
				{
					this.BehaviorInternal.SetStyleAttribute("width", true, Convert.ToString(varPropValue, CultureInfo.InvariantCulture), true);
				}
				else if (propName.Equals("Font.Name"))
				{
					this.BehaviorInternal.SetStyleAttribute("fontFamily", true, Convert.ToString(varPropValue, CultureInfo.InvariantCulture), true);
				}
				else if (propName.Equals("Font.Size"))
				{
					this.BehaviorInternal.SetStyleAttribute("fontSize", true, Convert.ToString(varPropValue, CultureInfo.InvariantCulture), true);
				}
				else if (propName.Equals("Font.Bold"))
				{
					string text3;
					if ((bool)varPropValue)
					{
						text3 = "bold";
					}
					else
					{
						text3 = "normal";
					}
					this.BehaviorInternal.SetStyleAttribute("fontWeight", true, text3, true);
				}
				else if (propName.Equals("Font.Italic"))
				{
					string text4;
					if ((bool)varPropValue)
					{
						text4 = "italic";
					}
					else
					{
						text4 = "normal";
					}
					this.BehaviorInternal.SetStyleAttribute("fontStyle", true, text4, true);
				}
				else if (propName.Equals("Font.Underline"))
				{
					string text5 = (string)this.BehaviorInternal.GetStyleAttribute("textDecoration", true, true);
					if ((bool)varPropValue)
					{
						if (text5 == null)
						{
							text5 = "underline";
						}
						else if (text5.ToLower(CultureInfo.InvariantCulture).IndexOf("underline", StringComparison.Ordinal) < 0)
						{
							text5 += " underline";
						}
						this.BehaviorInternal.SetStyleAttribute("textDecoration", true, text5, true);
					}
					else if (text5 != null)
					{
						int num = text5.ToLower(CultureInfo.InvariantCulture).IndexOf("underline", StringComparison.Ordinal);
						if (num >= 0)
						{
							string text6 = text5.Substring(0, num);
							if (num + 9 < text5.Length)
							{
								text6 = " " + text5.Substring(num + 9);
							}
							this.BehaviorInternal.SetStyleAttribute("textDecoration", true, text6, true);
						}
					}
				}
				else if (propName.Equals("Font.Strikeout"))
				{
					string text7 = (string)this.BehaviorInternal.GetStyleAttribute("textDecoration", true, true);
					if ((bool)varPropValue)
					{
						if (text7 == null)
						{
							text7 = "line-through";
						}
						else if (text7.ToLower(CultureInfo.InvariantCulture).IndexOf("line-through", StringComparison.Ordinal) < 0)
						{
							text7 += " line-through";
						}
						this.BehaviorInternal.SetStyleAttribute("textDecoration", true, text7, true);
					}
					else if (text7 != null)
					{
						int num2 = text7.ToLower(CultureInfo.InvariantCulture).IndexOf("line-through", StringComparison.Ordinal);
						if (num2 >= 0)
						{
							string text8 = text7.Substring(0, num2);
							if (num2 + 12 < text7.Length)
							{
								text8 = " " + text7.Substring(num2 + 12);
							}
							this.BehaviorInternal.SetStyleAttribute("textDecoration", true, text8, true);
						}
					}
				}
				else if (propName.Equals("Font.Overline"))
				{
					string text9 = (string)this.BehaviorInternal.GetStyleAttribute("textDecoration", true, true);
					if ((bool)varPropValue)
					{
						if (text9 == null)
						{
							text9 = "overline";
						}
						else if (text9.ToLower(CultureInfo.InvariantCulture).IndexOf("overline", StringComparison.Ordinal) < 0)
						{
							text9 += " overline";
						}
						this.BehaviorInternal.SetStyleAttribute("textDecoration", true, text9, true);
					}
					else if (text9 != null)
					{
						int num3 = text9.ToLower(CultureInfo.InvariantCulture).IndexOf("overline", StringComparison.Ordinal);
						if (num3 >= 0)
						{
							string text10 = text9.Substring(0, num3);
							if (num3 + 8 < text9.Length)
							{
								text10 = " " + text9.Substring(num3 + 8);
							}
							this.BehaviorInternal.SetStyleAttribute("textDecoration", true, text10, true);
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		[Obsolete("The recommended alternative is ControlDesigner.Tag. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected override void OnBehaviorAttached()
		{
			base.OnBehaviorAttached();
			if (!base.IsWebControl)
			{
				return;
			}
			WebControl webControl = (WebControl)base.Component;
			string text = ColorTranslator.ToHtml(webControl.BackColor);
			if (text.Length > 0)
			{
				this.MapPropertyToStyle("BackColor", text);
			}
			text = ColorTranslator.ToHtml(webControl.ForeColor);
			if (text.Length > 0)
			{
				this.MapPropertyToStyle("ForeColor", text);
			}
			text = ColorTranslator.ToHtml(webControl.BorderColor);
			if (text.Length > 0)
			{
				this.MapPropertyToStyle("BorderColor", text);
			}
			BorderStyle borderStyle = webControl.BorderStyle;
			if (borderStyle != BorderStyle.NotSet)
			{
				this.MapPropertyToStyle("BorderStyle", borderStyle);
			}
			Unit borderWidth = webControl.BorderWidth;
			if (!borderWidth.IsEmpty && borderWidth.Value != 0.0)
			{
				this.MapPropertyToStyle("BorderWidth", borderWidth.ToString(CultureInfo.InvariantCulture));
			}
			Unit width = webControl.Width;
			if (!width.IsEmpty && width.Value != 0.0)
			{
				this.MapPropertyToStyle("Width", width.ToString(CultureInfo.InvariantCulture));
			}
			Unit height = webControl.Height;
			if (!height.IsEmpty && height.Value != 0.0)
			{
				this.MapPropertyToStyle("Height", height.ToString(CultureInfo.InvariantCulture));
			}
			string name = webControl.Font.Name;
			if (name.Length != 0)
			{
				this.MapPropertyToStyle("Font.Name", name);
			}
			FontUnit size = webControl.Font.Size;
			if (size != FontUnit.Empty)
			{
				this.MapPropertyToStyle("Font.Size", size.ToString(CultureInfo.InvariantCulture));
			}
			bool flag = webControl.Font.Bold;
			if (flag)
			{
				this.MapPropertyToStyle("Font.Bold", flag);
			}
			flag = webControl.Font.Italic;
			if (flag)
			{
				this.MapPropertyToStyle("Font.Italic", flag);
			}
			flag = webControl.Font.Underline;
			if (flag)
			{
				this.MapPropertyToStyle("Font.Underline", flag);
			}
			flag = webControl.Font.Strikeout;
			if (flag)
			{
				this.MapPropertyToStyle("Font.Strikeout", flag);
			}
			flag = webControl.Font.Overline;
			if (flag)
			{
				this.MapPropertyToStyle("Font.Overline", flag);
			}
		}

		public override void UpdateDesignTimeHtml()
		{
		}
	}
}

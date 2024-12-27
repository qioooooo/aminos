using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace System.Diagnostics
{
	// Token: 0x02000748 RID: 1864
	[Obsolete("This class has been deprecated.  http://go.microsoft.com/fwlink/?linkid=14202")]
	public class DiagnosticsConfigurationHandler : IConfigurationSectionHandler
	{
		// Token: 0x060038D2 RID: 14546 RVA: 0x000EFBE8 File Offset: 0x000EEBE8
		public virtual object Create(object parent, object configContext, XmlNode section)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			HandlerBase.CheckForUnrecognizedAttributes(section);
			Hashtable hashtable = (Hashtable)parent;
			Hashtable hashtable2;
			if (hashtable == null)
			{
				hashtable2 = new Hashtable();
			}
			else
			{
				hashtable2 = (Hashtable)hashtable.Clone();
			}
			foreach (object obj in section.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(xmlNode))
				{
					string name;
					if ((name = xmlNode.Name) == null)
					{
						goto IL_0188;
					}
					if (!(name == "switches"))
					{
						if (!(name == "assert"))
						{
							if (!(name == "trace"))
							{
								if (!(name == "performanceCounters"))
								{
									goto IL_0188;
								}
								if (flag4)
								{
									throw new ConfigurationErrorsException(SR.GetString("ConfigSectionsUnique", new object[] { "performanceCounters" }));
								}
								flag4 = true;
								DiagnosticsConfigurationHandler.HandleCounters((Hashtable)parent, hashtable2, xmlNode, configContext);
							}
							else
							{
								if (flag3)
								{
									throw new ConfigurationErrorsException(SR.GetString("ConfigSectionsUnique", new object[] { "trace" }));
								}
								flag3 = true;
								DiagnosticsConfigurationHandler.HandleTrace(hashtable2, xmlNode, configContext);
							}
						}
						else
						{
							if (flag2)
							{
								throw new ConfigurationErrorsException(SR.GetString("ConfigSectionsUnique", new object[] { "assert" }));
							}
							flag2 = true;
							DiagnosticsConfigurationHandler.HandleAssert(hashtable2, xmlNode, configContext);
						}
					}
					else
					{
						if (flag)
						{
							throw new ConfigurationErrorsException(SR.GetString("ConfigSectionsUnique", new object[] { "switches" }));
						}
						flag = true;
						DiagnosticsConfigurationHandler.HandleSwitches(hashtable2, xmlNode, configContext);
					}
					IL_018F:
					HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
					continue;
					IL_0188:
					HandlerBase.ThrowUnrecognizedElement(xmlNode);
					goto IL_018F;
				}
			}
			return hashtable2;
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x000EFDCC File Offset: 0x000EEDCC
		private static void HandleSwitches(Hashtable config, XmlNode switchesNode, object context)
		{
			Hashtable hashtable = (Hashtable)new SwitchesDictionarySectionHandler().Create(config["switches"], context, switchesNode);
			IDictionaryEnumerator enumerator = hashtable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				try
				{
					int.Parse((string)enumerator.Value, CultureInfo.InvariantCulture);
				}
				catch
				{
					throw new ConfigurationErrorsException(SR.GetString("Value_must_be_numeric", new object[] { enumerator.Key }));
				}
			}
			config["switches"] = hashtable;
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x000EFE60 File Offset: 0x000EEE60
		private static void HandleAssert(Hashtable config, XmlNode assertNode, object context)
		{
			bool flag = false;
			if (HandlerBase.GetAndRemoveBooleanAttribute(assertNode, "assertuienabled", ref flag) != null)
			{
				config["assertuienabled"] = flag;
			}
			string text = null;
			if (HandlerBase.GetAndRemoveStringAttribute(assertNode, "logfilename", ref text) != null)
			{
				config["logfilename"] = text;
			}
			HandlerBase.CheckForChildNodes(assertNode);
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x000EFEB4 File Offset: 0x000EEEB4
		private static void HandleCounters(Hashtable parent, Hashtable config, XmlNode countersNode, object context)
		{
			int num = 0;
			if (HandlerBase.GetAndRemoveIntegerAttribute(countersNode, "filemappingsize", ref num) != null && parent == null)
			{
				config["filemappingsize"] = num;
			}
			HandlerBase.CheckForChildNodes(countersNode);
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x000EFEEC File Offset: 0x000EEEEC
		private static void HandleTrace(Hashtable config, XmlNode traceNode, object context)
		{
			bool flag = false;
			bool flag2 = false;
			if (HandlerBase.GetAndRemoveBooleanAttribute(traceNode, "autoflush", ref flag2) != null)
			{
				config["autoflush"] = flag2;
			}
			int num = 0;
			if (HandlerBase.GetAndRemoveIntegerAttribute(traceNode, "indentsize", ref num) != null)
			{
				config["indentsize"] = num;
			}
			foreach (object obj in traceNode.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(xmlNode))
				{
					if (xmlNode.Name == "listeners")
					{
						if (flag)
						{
							throw new ConfigurationErrorsException(SR.GetString("ConfigSectionsUnique", new object[] { "listeners" }));
						}
						flag = true;
						DiagnosticsConfigurationHandler.HandleListeners(config, xmlNode, context);
					}
					else
					{
						HandlerBase.ThrowUnrecognizedElement(xmlNode);
					}
				}
			}
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x000EFFE0 File Offset: 0x000EEFE0
		private static void HandleListeners(Hashtable config, XmlNode listenersNode, object context)
		{
			HandlerBase.CheckForUnrecognizedAttributes(listenersNode);
			foreach (object obj in listenersNode.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(xmlNode))
				{
					string text = null;
					string text2 = null;
					string text3 = null;
					string name = xmlNode.Name;
					string text4;
					if ((text4 = name) == null || (!(text4 == "add") && !(text4 == "remove") && !(text4 == "clear")))
					{
						HandlerBase.ThrowUnrecognizedElement(xmlNode);
					}
					HandlerBase.GetAndRemoveStringAttribute(xmlNode, "name", ref text);
					HandlerBase.GetAndRemoveStringAttribute(xmlNode, "type", ref text2);
					HandlerBase.GetAndRemoveStringAttribute(xmlNode, "initializeData", ref text3);
					HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
					HandlerBase.CheckForChildNodes(xmlNode);
					TraceListener traceListener = null;
					if (text2 != null)
					{
						Type type = Type.GetType(text2);
						if (type == null)
						{
							throw new ConfigurationErrorsException(SR.GetString("Could_not_find_type", new object[] { text2 }));
						}
						if (!typeof(TraceListener).IsAssignableFrom(type))
						{
							throw new ConfigurationErrorsException(SR.GetString("Type_isnt_tracelistener", new object[] { text2 }));
						}
						if (text3 == null)
						{
							ConstructorInfo constructor = type.GetConstructor(new Type[0]);
							if (constructor == null)
							{
								throw new ConfigurationErrorsException(SR.GetString("Could_not_get_constructor", new object[] { text2 }));
							}
							traceListener = (TraceListener)constructor.Invoke(new object[0]);
						}
						else
						{
							ConstructorInfo constructor2 = type.GetConstructor(new Type[] { typeof(string) });
							if (constructor2 == null)
							{
								throw new ConfigurationErrorsException(SR.GetString("Could_not_get_constructor", new object[] { text2 }));
							}
							traceListener = (TraceListener)constructor2.Invoke(new object[] { text3 });
						}
						if (text != null)
						{
							traceListener.Name = text;
						}
					}
					char c = name[0];
					switch (c)
					{
					case 'a':
						if (traceListener == null)
						{
							throw new ConfigurationErrorsException(SR.GetString("Could_not_create_listener", new object[] { text }));
						}
						Trace.Listeners.Add(traceListener);
						continue;
					case 'b':
						break;
					case 'c':
						Trace.Listeners.Clear();
						continue;
					default:
						if (c == 'r')
						{
							if (traceListener != null)
							{
								Trace.Listeners.Remove(traceListener);
								continue;
							}
							if (text == null)
							{
								throw new ConfigurationErrorsException(SR.GetString("Cannot_remove_with_null"));
							}
							Trace.Listeners.Remove(text);
							continue;
						}
						break;
					}
					HandlerBase.ThrowUnrecognizedElement(xmlNode);
				}
			}
		}
	}
}

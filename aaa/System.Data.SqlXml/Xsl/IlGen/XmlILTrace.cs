using System;
using System.Globalization;
using System.IO;
using System.Security;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x02000042 RID: 66
	internal static class XmlILTrace
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0001E760 File Offset: 0x0001D760
		public static bool IsEnabled
		{
			get
			{
				if (!XmlILTrace.alreadyCheckedEnabled)
				{
					try
					{
						XmlILTrace.dirName = Environment.GetEnvironmentVariable("XmlILTrace");
					}
					catch (SecurityException)
					{
					}
					XmlILTrace.alreadyCheckedEnabled = true;
				}
				return XmlILTrace.dirName != null;
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0001E7AC File Offset: 0x0001D7AC
		public static void PrepareTraceWriter(string fileName)
		{
			if (!XmlILTrace.IsEnabled)
			{
				return;
			}
			File.Delete(XmlILTrace.dirName + "\\" + fileName);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0001E7CB File Offset: 0x0001D7CB
		public static TextWriter GetTraceWriter(string fileName)
		{
			if (!XmlILTrace.IsEnabled)
			{
				return null;
			}
			return new StreamWriter(XmlILTrace.dirName + "\\" + fileName, true);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001E7EC File Offset: 0x0001D7EC
		public static void WriteQil(QilExpression qil, string fileName)
		{
			if (!XmlILTrace.IsEnabled)
			{
				return;
			}
			XmlWriter xmlWriter = XmlWriter.Create(XmlILTrace.dirName + "\\" + fileName);
			try
			{
				XmlILTrace.WriteQil(qil, xmlWriter);
			}
			finally
			{
				xmlWriter.Close();
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001E838 File Offset: 0x0001D838
		public static void TraceOptimizations(QilExpression qil, string fileName)
		{
			if (!XmlILTrace.IsEnabled)
			{
				return;
			}
			XmlWriter xmlWriter = XmlWriter.Create(XmlILTrace.dirName + "\\" + fileName);
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteProcessingInstruction("xml-stylesheet", "href='qilo.xslt' type='text/xsl'");
			xmlWriter.WriteStartElement("QilOptimizer");
			xmlWriter.WriteAttributeString("timestamp", DateTime.Now.ToString(CultureInfo.InvariantCulture));
			XmlILTrace.WriteQilRewrite(qil, xmlWriter, null);
			try
			{
				for (int i = 1; i < 200; i++)
				{
					QilExpression qilExpression = (QilExpression)new QilCloneVisitor(qil.Factory).Clone(qil);
					XmlILOptimizerVisitor xmlILOptimizerVisitor = new XmlILOptimizerVisitor(qilExpression, !qilExpression.IsDebug);
					xmlILOptimizerVisitor.Threshold = i;
					qilExpression = xmlILOptimizerVisitor.Optimize();
					XmlILTrace.WriteQilRewrite(qilExpression, xmlWriter, XmlILTrace.OptimizationToString(xmlILOptimizerVisitor.LastReplacement));
					if (xmlILOptimizerVisitor.ReplacementCount < i)
					{
						break;
					}
				}
			}
			catch (Exception ex)
			{
				if (!XmlException.IsCatchableException(ex))
				{
					throw;
				}
				xmlWriter.WriteElementString("Exception", null, ex.ToString());
				throw;
			}
			finally
			{
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndDocument();
				xmlWriter.Flush();
				xmlWriter.Close();
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001E968 File Offset: 0x0001D968
		private static void WriteQil(QilExpression qil, XmlWriter w)
		{
			QilXmlWriter qilXmlWriter = new QilXmlWriter(w);
			qilXmlWriter.ToXml(qil);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001E983 File Offset: 0x0001D983
		private static void WriteQilRewrite(QilExpression qil, XmlWriter w, string rewriteName)
		{
			w.WriteStartElement("Diff");
			if (rewriteName != null)
			{
				w.WriteAttributeString("rewrite", rewriteName);
			}
			XmlILTrace.WriteQil(qil, w);
			w.WriteEndElement();
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001E9AC File Offset: 0x0001D9AC
		private static string OptimizationToString(int opt)
		{
			string name = Enum.GetName(typeof(XmlILOptimization), opt);
			if (name.StartsWith("Introduce", StringComparison.Ordinal))
			{
				return name.Substring(9) + " introduction";
			}
			if (name.StartsWith("Eliminate", StringComparison.Ordinal))
			{
				return name.Substring(9) + " elimination";
			}
			if (name.StartsWith("Commute", StringComparison.Ordinal))
			{
				return name.Substring(7) + " commutation";
			}
			if (name.StartsWith("Fold", StringComparison.Ordinal))
			{
				return name.Substring(4) + " folding";
			}
			if (name.StartsWith("Misc", StringComparison.Ordinal))
			{
				return name.Substring(4);
			}
			return name;
		}

		// Token: 0x04000376 RID: 886
		private const int MAX_REWRITES = 200;

		// Token: 0x04000377 RID: 887
		private static string dirName;

		// Token: 0x04000378 RID: 888
		private static bool alreadyCheckedEnabled;
	}
}

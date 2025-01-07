using System;

namespace System.Data.Design
{
	internal sealed class DSGeneratorProblem
	{
		internal string Message
		{
			get
			{
				return this.message;
			}
		}

		internal ProblemSeverity Severity
		{
			get
			{
				return this.severity;
			}
		}

		internal DataSourceComponent ProblemSource
		{
			get
			{
				return this.problemSource;
			}
		}

		internal DSGeneratorProblem(string message, ProblemSeverity severity, DataSourceComponent problemSource)
		{
			this.message = message;
			this.severity = severity;
			this.problemSource = problemSource;
		}

		private string message;

		private ProblemSeverity severity;

		private DataSourceComponent problemSource;
	}
}

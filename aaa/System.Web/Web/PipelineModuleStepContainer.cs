using System;
using System.Collections.Generic;

namespace System.Web
{
	// Token: 0x020000BC RID: 188
	internal sealed class PipelineModuleStepContainer
	{
		// Token: 0x060008C6 RID: 2246 RVA: 0x00027E3A File Offset: 0x00026E3A
		internal PipelineModuleStepContainer()
		{
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00027E44 File Offset: 0x00026E44
		private List<HttpApplication.IExecutionStep> GetStepArray(RequestNotification notification, bool isPostEvent)
		{
			List<HttpApplication.IExecutionStep>[] array = this._moduleSteps;
			if (isPostEvent)
			{
				array = this._modulePostSteps;
			}
			int num = PipelineModuleStepContainer.EventToIndex(notification);
			return array[num];
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00027E70 File Offset: 0x00026E70
		internal int GetEventCount(RequestNotification notification, bool isPostEvent)
		{
			List<HttpApplication.IExecutionStep> stepArray = this.GetStepArray(notification, isPostEvent);
			if (stepArray == null)
			{
				return 0;
			}
			return stepArray.Count;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00027E94 File Offset: 0x00026E94
		internal HttpApplication.IExecutionStep GetNextEvent(RequestNotification notification, bool isPostEvent, int eventIndex)
		{
			List<HttpApplication.IExecutionStep> stepArray = this.GetStepArray(notification, isPostEvent);
			return stepArray[eventIndex];
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00027EB4 File Offset: 0x00026EB4
		internal void RemoveEvent(RequestNotification notification, bool isPostEvent, Delegate handler)
		{
			List<HttpApplication.IExecutionStep>[] array = this._moduleSteps;
			if (isPostEvent)
			{
				array = this._modulePostSteps;
			}
			if (array == null)
			{
				return;
			}
			int num = PipelineModuleStepContainer.EventToIndex(notification);
			List<HttpApplication.IExecutionStep> list = array[num];
			if (list == null)
			{
				return;
			}
			int num2 = -1;
			for (int i = 0; i < list.Count; i++)
			{
				HttpApplication.SyncEventExecutionStep syncEventExecutionStep = list[i] as HttpApplication.SyncEventExecutionStep;
				if (syncEventExecutionStep != null && syncEventExecutionStep.Handler == (EventHandler)handler)
				{
					num2 = i;
					break;
				}
			}
			if (num2 != -1)
			{
				list.RemoveAt(num2);
			}
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00027F34 File Offset: 0x00026F34
		internal void AddEvent(RequestNotification notification, bool isPostEvent, HttpApplication.IExecutionStep step)
		{
			int num = PipelineModuleStepContainer.EventToIndex(notification);
			List<HttpApplication.IExecutionStep>[] array;
			if (isPostEvent)
			{
				if (this._modulePostSteps == null)
				{
					this._modulePostSteps = new List<HttpApplication.IExecutionStep>[32];
				}
				array = this._modulePostSteps;
			}
			else
			{
				if (this._moduleSteps == null)
				{
					this._moduleSteps = new List<HttpApplication.IExecutionStep>[32];
				}
				array = this._moduleSteps;
			}
			List<HttpApplication.IExecutionStep> list = array[num];
			if (list == null)
			{
				list = new List<HttpApplication.IExecutionStep>();
				array[num] = list;
			}
			list.Add(step);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00027FA0 File Offset: 0x00026FA0
		private static int EventToIndex(RequestNotification notification)
		{
			int num = -1;
			if (notification <= RequestNotification.PreExecuteRequestHandler)
			{
				if (notification <= RequestNotification.ResolveRequestCache)
				{
					switch (notification)
					{
					case RequestNotification.BeginRequest:
						return 0;
					case RequestNotification.AuthenticateRequest:
						return 1;
					case RequestNotification.BeginRequest | RequestNotification.AuthenticateRequest:
						break;
					case RequestNotification.AuthorizeRequest:
						return 2;
					default:
						if (notification == RequestNotification.ResolveRequestCache)
						{
							return 3;
						}
						break;
					}
				}
				else
				{
					if (notification == RequestNotification.MapRequestHandler)
					{
						return 4;
					}
					if (notification == RequestNotification.AcquireRequestState)
					{
						return 5;
					}
					if (notification == RequestNotification.PreExecuteRequestHandler)
					{
						return 6;
					}
				}
			}
			else if (notification <= RequestNotification.UpdateRequestCache)
			{
				if (notification == RequestNotification.ExecuteRequestHandler)
				{
					return 7;
				}
				if (notification == RequestNotification.ReleaseRequestState)
				{
					return 8;
				}
				if (notification == RequestNotification.UpdateRequestCache)
				{
					return 9;
				}
			}
			else
			{
				if (notification == RequestNotification.LogRequest)
				{
					return 10;
				}
				if (notification == RequestNotification.EndRequest)
				{
					return 11;
				}
				if (notification == RequestNotification.SendResponse)
				{
					return 12;
				}
			}
			return num;
		}

		// Token: 0x040011E8 RID: 4584
		private List<HttpApplication.IExecutionStep>[] _moduleSteps;

		// Token: 0x040011E9 RID: 4585
		private List<HttpApplication.IExecutionStep>[] _modulePostSteps;
	}
}

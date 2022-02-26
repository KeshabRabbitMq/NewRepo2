using System;
using Microservice.Framework.Domain.ExecutionResults;

namespace ServiceMonitoringTool.Api.Domain
{
    public static class IExecutionResultExtensions
    {
        public static TResult As<TResult>(this IExecutionResult executionResult)
        {
            if (executionResult != null
                && executionResult.IsSuccess
                && executionResult.GetType().IsGenericType
                && executionResult.GetType().GetGenericTypeDefinition() == typeof(SuccessExecutionResult<>))
            {
                return ((SuccessExecutionResult<TResult>)executionResult).Result;
            }

            if (executionResult != null
                && !executionResult.IsSuccess)
            {
                throw new Exception(((FailedExecutionResult)executionResult).Errors);
            }

            throw new Exception($"As<> method is only for type SuccessExecutionResult<> and must not be null");
        }
    }
}
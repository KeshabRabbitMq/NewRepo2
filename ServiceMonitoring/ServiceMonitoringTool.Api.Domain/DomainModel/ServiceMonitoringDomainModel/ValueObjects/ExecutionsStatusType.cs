using Microservice.Framework.Common;

namespace ServiceMonitoring.Api.Domain
{
    [ValueObjectResourcePath("ServiceMonitoring.Api.Domain.DomainModel.ServiceMonitoringDomainModel.ValueObjects.Mappings.ExecutionsStatusType.xml")]
    public class ExecutionsStatusType : XmlValueObject
    {
    }

    public class ExecutionsStatusTypes : XmlValueObjectLookup<ExecutionsStatusType, ExecutionsStatusTypes>
    {
        public ExecutionsStatusType ExecutedSuccessfully { get { return FindValueObject("Ex_Suc"); } }

        public ExecutionsStatusType FailedExecution { get { return FindValueObject("Ex_Fal"); } }
    }
}
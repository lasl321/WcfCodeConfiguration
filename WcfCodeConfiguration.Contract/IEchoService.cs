using System.ServiceModel;

namespace WcfCodeConfiguration.Contract
{
    [ServiceContract]
    public interface IEchoService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }
}
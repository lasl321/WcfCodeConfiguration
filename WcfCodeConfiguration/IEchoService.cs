using System.ServiceModel;

namespace WcfCodeConfiguration
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
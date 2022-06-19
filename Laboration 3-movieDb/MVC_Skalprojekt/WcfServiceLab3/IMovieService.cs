using System.ServiceModel;
using System.Xml.Linq;

namespace WcfServiceLab3
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IMovieService
    {

        [OperationContract]
        XElement GetAllMovies();

        [OperationContract]
        XElement GetTopTenMovies();


        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
}

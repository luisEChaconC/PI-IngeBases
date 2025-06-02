using backend.Domain;

namespace backend.Repositories
{
    public interface IAPIRepository
    {
        List<ApiModel> GetAPIs();
        List<ApiParameterModel> GetParametersByAPI(Guid apiId);
        bool AddParameterValue(ParameterValueModel value);
        List<ParameterValueModel> GetParameterValues(Guid parameterId); 
    }
}
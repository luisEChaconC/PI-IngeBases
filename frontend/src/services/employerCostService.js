import axios from 'axios'
import API_CONFIG from '@/config/api'
import { buildApiUrl, handleApiError } from '@/utils/apiUtils'

class EmployerCostService {
 
  async getByPayrollId(payrollId) {
    try {
      const url = buildApiUrl(API_CONFIG.ENDPOINTS.EMPLOYER_COST.GET_BY_PAYROLL(payrollId))
      const { data } = await axios.get(url)
      return data
    } catch (err) {
      console.error('Error fetching employer cost:', err)
      throw handleApiError(err, 'No se pudo obtener el costo patronal')
    }
  }

  async getEmployerById(id) {
  try {
    const endpoint = API_CONFIG.ENDPOINTS.EMPLOYER.GET_BY_ID(id);
    const response = await axios.get(buildApiUrl(endpoint));
    return response.data;
  } catch (error) {
    console.error('Error fetching employer:', error);
    throw handleApiError(error, 'Error al obtener el empleador');
  }
}
}
export default new EmployerCostService()

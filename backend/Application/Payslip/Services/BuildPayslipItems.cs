using backend.Domain;

namespace backend.Application.Payslip.Services
{
    public class BuildPayslipItems : IBuildPayslipItems
    {
        public List<PayslipItem> Build(List<DeductionDetailModel> rawDeductions)
        {
            var voluntaryItems = new List<PayslipItem>();
            var mandatoryItems = new List<PayslipItem>();
            decimal voluntaryTotal = 0;
            decimal mandatoryTotal = 0;

            foreach (var d in rawDeductions)
            {
                var item = new PayslipItem
                {
                    Label = d.Name,
                    Amount = -d.AmountDeduced,
                    IsBold = false
                };

                if (d.DeductionType.ToLower() == "voluntary")
                {
                    voluntaryItems.Add(item);
                    voluntaryTotal += d.AmountDeduced;
                }
                else if (d.DeductionType.ToLower() == "mandatory")
                {
                    mandatoryItems.Add(item);
                    mandatoryTotal += d.AmountDeduced;
                }
            }

            var result = new List<PayslipItem>();
            result.AddRange(voluntaryItems);
            result.Add(new PayslipItem
            {
                Label = "Total Deducciones Voluntarias",
                Amount = -voluntaryTotal,
                IsBold = true
            });
            result.AddRange(mandatoryItems);
            result.Add(new PayslipItem
            {
                Label = "Total Deducciones Obligatorias",
                Amount = -mandatoryTotal,
                IsBold = true
            });

            return result;
        }
    }
}


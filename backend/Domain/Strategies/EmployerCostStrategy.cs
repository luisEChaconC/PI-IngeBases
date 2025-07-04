using backend.Domain;

public class EmployerCostStrategy : IEmployerCostStrategy
{
    public EmployerCostModel Calculate(Guid payrollId, decimal grossSalary)
    {
        var model = new EmployerCostModel
        {
            Id = Guid.NewGuid(),
            PayrollId = payrollId,

            // CCSS
            SEM = grossSalary * 0.0925m,
            IVM = grossSalary * 0.0542m,

            // Otras instituciones
            BPOP_OtherInstitutions = grossSalary * 0.0025m,
            FamilyAllocations = grossSalary * 0.05m,
            IMAS = grossSalary * 0.005m,
            INA = grossSalary * 0.015m,

            // LPT
            BPOP_LPT = grossSalary * 0.0025m,
            FCL = grossSalary * 0.015m,
            OPC = grossSalary * 0.02m,
            INS = grossSalary * 0.01m,
        };

        model.LegalDeductionsTotal =
            model.SEM + model.IVM +
            model.BPOP_OtherInstitutions + model.FamilyAllocations + model.IMAS + model.INA +
            model.BPOP_LPT + model.FCL + model.OPC + model.INS;

        model.BenefitsTotal = model.PrivateInsurance + model.SolidarityAssociation;
        model.TotalEmployerCost = model.LegalDeductionsTotal + model.BenefitsTotal;

        return model;
    }
}

using backend.Domain;
public class EmployerCostModel
{
    public Guid Id { get; set; }
    public Guid PayrollId { get; set; }

    public decimal SEM { get; set; }
    public decimal IVM { get; set; }

    public decimal BPOP_OtherInstitutions { get; set; }
    public decimal FamilyAllocations { get; set; }
    public decimal IMAS { get; set; }
    public decimal INA { get; set; }

    public decimal BPOP_LPT { get; set; }
    public decimal FCL { get; set; }
    public decimal OPC { get; set; }
    public decimal INS { get; set; }

    public decimal PrivateInsurance { get; set; }
    public decimal SolidarityAssociation { get; set; }

    public decimal LegalDeductionsTotal { get; set; }
    public decimal BenefitsTotal { get; set; }
    public decimal TotalEmployerCost { get; set; }
}

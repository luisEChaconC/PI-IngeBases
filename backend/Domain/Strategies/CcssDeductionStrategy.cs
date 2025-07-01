using System;
using backend.Domain;

namespace backend.Domain.Strategies
{
    public class CcssDeductionStrategy : IDeductionCalculationStrategy
    {
        private const decimal WorkerSemRate = 0.0550m;
        private const decimal WorkerIvmRate = 0.0417m;
        private const decimal EmployerSemRate = 0.0925m;
        private const decimal EmployerIvmRate = 0.0542m;

        private const decimal BpopLptRate = 0.0025m;
        private const decimal BpopWorkerRate = 0.01m;
        private const decimal BpopEmployerRate = 0.0025m;
        private const decimal FamilyAllocationsRate = 0.05m;
        private const decimal ImasRate = 0.005m;
        private const decimal InaRate = 0.015m;
        private const decimal FclRate = 0.015m;
        private const decimal OpcRate = 0.02m;
        private const decimal InsRate = 0.01m;

        private const decimal MinBaseSem = 341227m;
        private const decimal MinBaseIvm = 319384m;

        public decimal CalculateDeduction(
            decimal grossSalary,
            string contractType,
            string gender,
            Benefit? benefit = null,
            Guid? employeeId = null)
        {
            return CalculateFullDeduction(grossSalary, contractType, gender, benefit, employeeId).WorkerDeduction;
        }

        public (decimal WorkerDeduction, decimal EmployerDeduction) CalculateFullDeduction(
            decimal grossSalary,
            string contractType,
            string gender,
            Benefit? benefit = null,
            Guid? employeeId = null)
        {
            if (contractType == "Professional Services")
                return (0m, 0m);

            var semBase = Math.Max(grossSalary, MinBaseSem);
            var ivmBase = Math.Max(grossSalary, MinBaseIvm);

            decimal semWorker = Math.Round(semBase * WorkerSemRate, 2);
            decimal ivmWorker = Math.Round(ivmBase * WorkerIvmRate, 2);
            decimal bpopWorker = Math.Round(grossSalary * BpopWorkerRate, 2);
            decimal totalWorker = semWorker + ivmWorker + bpopWorker;

            decimal semEmployer = Math.Round(semBase * EmployerSemRate, 2);
            decimal ivmEmployer = Math.Round(ivmBase * EmployerIvmRate, 2);
            decimal bpopEmployer = Math.Round(grossSalary * BpopEmployerRate, 2);
            decimal bpopLpt = Math.Round(grossSalary * BpopLptRate, 2);
            decimal familyAllocations = Math.Round(grossSalary * FamilyAllocationsRate, 2);
            decimal imas = Math.Round(grossSalary * ImasRate, 2);
            decimal ina = Math.Round(grossSalary * InaRate, 2);
            decimal fcl = Math.Round(grossSalary * FclRate, 2);
            decimal opc = Math.Round(grossSalary * OpcRate, 2);
            decimal ins = Math.Round(grossSalary * InsRate, 2);

            decimal totalEmployer = semEmployer + ivmEmployer + bpopEmployer + bpopLpt + familyAllocations + imas + ina + fcl + opc + ins;

            return (totalWorker, totalEmployer);
        }
    }
}
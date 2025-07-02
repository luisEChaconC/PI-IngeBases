CREATE TABLE EmployerCost (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    PayrollId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Payrolls(Id),

  
    SEM DECIMAL(10, 2),                 
    IVM DECIMAL(10, 2),                

   
    BPOP_OtherInstitutions DECIMAL(10, 2),   
    FamilyAllocations DECIMAL(10, 2),         
    IMAS DECIMAL(10, 2),                     
    INA DECIMAL(10, 2),                    

 
    BPOP_LPT DECIMAL(10, 2),           
    FCL DECIMAL(10, 2),                
    OPC DECIMAL(10, 2),                 
    INS DECIMAL(10, 2),             

   
    PrivateInsurance DECIMAL(10, 2),         
    SolidarityAssociation DECIMAL(10, 2),    

    
    LegalDeductionsTotal DECIMAL(10, 2),    
    BenefitsTotal DECIMAL(10, 2),             
    TotalEmployerCost DECIMAL(10, 2),         

);
USE PayrollSystem

CREATE TABLE DeductionDetails (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    AmountDeduced DECIMAL(11,3) NOT NULL,
    PaymentDetailsId UNIQUEIDENTIFIER NOT NULL,
    DeductionType VARCHAR(10) NOT NULL CHECK (DeductionType IN ('mandatory', 'voluntary')),
    CONSTRAINT FK_DeductionDetails_PaymentDetails FOREIGN KEY (PaymentDetailsId)
        REFERENCES PaymentDetails(Id)
);
﻿CREATE PROCEDURE FI_SP_VerificaCPFBeneficiario
    @CPF VARCHAR(14),
    @IdAtual BIGINT = NULL
AS
BEGIN
    SELECT 1 
    FROM BENEFICIARIOS 
    WHERE CPF = @CPF 
    AND (@IdAtual IS NULL OR Id != @IdAtual)
END
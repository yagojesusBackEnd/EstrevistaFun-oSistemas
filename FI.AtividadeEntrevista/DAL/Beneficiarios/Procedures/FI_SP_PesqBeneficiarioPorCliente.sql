CREATE PROCEDURE FI_SP_PesqBeneficiarioPorCliente
    @idCliente INT,
    @iniciarEm INT,
    @quantidade INT,
    @campoOrdenacao NVARCHAR(50),
    @crescente BIT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @fetchQuantity INT = CASE WHEN @quantidade > 0 THEN @quantidade ELSE 1 END;

    SELECT *
    FROM BENEFICIARIOS
    WHERE IdCliente = @idCliente
    ORDER BY 
        CASE 
            WHEN @campoOrdenacao = 'Nome' AND @crescente = 1 THEN Nome
        END ASC,
        CASE 
            WHEN @campoOrdenacao = 'Nome' AND @crescente = 0 THEN Nome
        END DESC
    OFFSET @iniciarEm ROWS 
    FETCH NEXT @fetchQuantity ROWS ONLY;
END;

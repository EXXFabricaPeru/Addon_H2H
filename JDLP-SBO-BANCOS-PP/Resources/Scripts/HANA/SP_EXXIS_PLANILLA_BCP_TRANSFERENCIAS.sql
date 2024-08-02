CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_TRANSFERENCIAS"
LANGUAGE SQLSCRIPT
AS
BEGIN
	SELECT 
		'' AS "1.01_TipoRegistro",
		'' AS "1.02_SecuencialFila",
		'' AS "1.03_CantidadCuentasArchivo",
		'' AS "1.04_IdentificadorCabecera",
		'' AS "1.05_Filler"
		from dummy;
	--[dbo].[PM_RESUMEN_PAGOS]
END;

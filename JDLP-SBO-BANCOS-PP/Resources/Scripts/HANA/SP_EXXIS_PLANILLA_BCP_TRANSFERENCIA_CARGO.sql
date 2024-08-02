CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_TRANSFERENCIA_CARGO"
LANGUAGE SQLSCRIPT
AS
BEGIN
	SELECT 
		'' AS "2.01_TipoRegistro",
		'' AS "2.02_SecuencialFila",
		'' AS "2.03_CantidadAbonosPlanilla",
		'' AS "2.04_TipoCuentaCargo",
		'' AS "2.05_MonedaCuentaCargo",
		'' AS "2.06_NumeroCuentaCargo",
		'' AS "2.07_MontoTotalSoles",
		'' AS "2.08_MontoTotalDolares",
		'' AS "2.09_TotalControl",
		'' AS "2.1_IdentificadorCargo",
		'' AS "2.11_Filler"
		from dummy;
	--[dbo].[PM_RESUMEN_PAGOS]
END;

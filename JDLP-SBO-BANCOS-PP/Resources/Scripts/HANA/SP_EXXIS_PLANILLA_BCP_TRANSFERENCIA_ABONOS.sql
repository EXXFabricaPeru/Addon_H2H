CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_TRANSFERENCIA_ABONOS"
LANGUAGE SQLSCRIPT
AS
BEGIN
	SELECT 
		'' AS "3.01_TipoRegistro",
		'' AS "3.02_SecuencialFila",
		'' AS "3.03_TipoCuenta",
		'' AS "3.04_CuentaPropia",
		'' AS "3.05_NumeroCuentaAbono",
		'' AS "3.06_TipoDocumentoBeneficiario",
		'' AS "3.07_NumeroDocumentoBeneficiario",
		'' AS "3.08_CorrelativoDocumento",
		'' AS "3.09_NombreBeneficiario",
		'' AS "3.1_MonedaMontoTransferir",
		'' AS "3.11_MontoOperación",
		'' AS "3.12_Referencia",
		'' AS "3.13_IdentificadorAbono",
		'' AS "3.14_TitularCuenta",
		'' AS "3.15_EmailClienteBCP",
		'' AS "3.16_Filler"
		FRom dummy;
	--[dbo].[PM_RESUMEN_PAGOS]
END;

CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_U_PLANILLA_CORREOENVIADO"
(
    IN vPLANILLA NVARCHAR(50),
    IN vENVIADO NVARCHAR(10)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    UPDATE "@EXX_BCP_HOST" SET "U_HTH_COR" = :vENVIADO WHERE "U_HTH_PL" = :vPLANILLA;
END;
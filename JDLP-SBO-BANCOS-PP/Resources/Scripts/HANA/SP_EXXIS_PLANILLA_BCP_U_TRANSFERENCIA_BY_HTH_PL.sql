CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_U_TRANSFERENCIA_BY_HTH_PL"
(
    IN vHTH_PL NVARCHAR(50),
    IN vHTH_NAE NVARCHAR(100),
    IN vHTH_PLH NVARCHAR(100),
    IN vHTH_PLG NVARCHAR(5)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    UPDATE "@EXX_BCP_HOST"
    SET "U_HTH_NAE" = :vHTH_NAE,
        "U_HTH_PLH" = :vHTH_PLH,
        "U_HTH_PLG" = :vHTH_PLG
    WHERE "U_HTH_PL" = :vHTH_PL;
END;

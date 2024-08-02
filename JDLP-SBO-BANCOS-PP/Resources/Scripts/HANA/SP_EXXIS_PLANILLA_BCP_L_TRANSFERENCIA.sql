CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_L_TRANSFERENCIA"
(
    IN vPlanilla NVARCHAR(50)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    SELECT  
        T0.*,
        LPAD(
            CAST(
                (
                    (SELECT COUNT(*) FROM "@EXX_BCP_TRAN_CAR" WHERE "U_HTH_PL" = :vPlanilla)
                    +
                    (SELECT COUNT(*) FROM "@EXX_BCP_TRAN_ABO" WHERE "U_HTH_PL" = :vPlanilla)
                ) AS NVARCHAR(10)
            ), 
            6, 
            '0'
        ) AS "103_CCA" -- REEMPLAZA AL CAMPO U_103_CCA
    FROM "@EXX_BCP_TRAN" T0 
    WHERE T0."U_HTH_PL" = :vPlanilla;
END;

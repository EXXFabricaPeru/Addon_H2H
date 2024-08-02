CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_L_TRANSFERENCIA_CARGO"
(
    IN vPlanilla NVARCHAR(50)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    SELECT  
        T0.*,
        (
            SELECT SUM(CAST(IFNULL(TX."U_ACHK", 0) AS BIGINT)) 
            FROM "@EXX_BCP_TRAN_ABO" TX
            WHERE TX."U_HTT_CA" = T0."U_HTT_CA"
        ) AS "U_ACHK", -- Suma de cuentas abono

        RIGHT(
            LPAD(
                CAST(
                    (
                        CAST(IFNULL(T0."U_CCHK", 0) AS BIGINT) +
                        (SELECT SUM(CAST(IFNULL(TX."U_ACHK", 0) AS BIGINT)) 
                        FROM "@EXX_BCP_TRAN_ABO" TX
                        WHERE TX."U_HTT_CA" = T0."U_HTT_CA")
                    ) AS NVARCHAR(50)
                ), 
                15, 
                '0'
            ),
            15
        ) AS "TotalControCHK" -- TotalControl (CheckSum)

    FROM "@EXX_BCP_TRAN_CAR" T0 WHERE T0."U_HTH_PL" = :vPlanilla;
END;
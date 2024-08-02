CREATE PROCEDURE "SP_EXX_BCP_A_TRANSFERENCIA"
(
IN vCode NVARCHAR(50),
IN vName NVARCHAR(50),
IN vHTT_TF NVARCHAR(254),
IN vHTH_PL NVARCHAR(254),
IN v101_TR NVARCHAR(1),
IN v102_SF NVARCHAR(6),
IN v103_CCA NVARCHAR(6),
IN v104_IC NVARCHAR(32),
IN v105_FI NVARCHAR(305)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    INSERT INTO "@EXX_BCP_TRAN"
    (
        "Code",
        "Name",
        "U_HTT_TF",
        "U_HTH_PL",
        "U_101_TR",
        "U_102_SF",
        "U_103_CCA",
        "U_104_IC",
        "U_105_FI"
    )
    VALUES
    (
        vCode,
        vName,
        vHTT_TF,
        vHTH_PL,
        v101_TR,
        v102_SF,
        v103_CCA,
        v104_IC,
        v105_FI
    );
END;

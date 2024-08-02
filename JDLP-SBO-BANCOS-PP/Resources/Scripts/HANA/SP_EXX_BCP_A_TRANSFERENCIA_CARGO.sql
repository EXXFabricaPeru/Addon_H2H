CREATE PROCEDURE "SP_EXX_BCP_A_TRANSFERENCIA_CARGO"
(
IN vCode NVARCHAR(50),
IN vName NVARCHAR(50),
IN vHTT_CA NVARCHAR(254),
IN vHTH_PL NVARCHAR(254),
IN vHTT_TF NVARCHAR(254),
IN v201_TR NVARCHAR(1),
IN v202_SF NVARCHAR(6),
IN v203_CAP NVARCHAR(6),
IN v204_TCC NVARCHAR(1),
IN v205_MCC NVARCHAR(4),
IN v206_NCC NVARCHAR(20),
IN v207_MTS NVARCHAR(17),
IN v208_MTD NVARCHAR(17),
IN v209_TC NVARCHAR(15),
IN v210_IC NVARCHAR(32),
IN v211_FI NVARCHAR(231),
IN vCCHK NVARCHAR(30)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    INSERT INTO "@EXX_BCP_TRAN_CAR"
    (
        "Code",
        "Name",
        "U_HTT_CA",
        "U_HTH_PL",
        "U_HTT_TF",
        "U_201_TR",
        "U_202_SF",
        "U_203_CAP",
        "U_204_TCC",
        "U_205_MCC",
        "U_206_NCC",
        "U_207_MTS",
        "U_208_MTD",
        "U_209_TC",
        "U_210_IC",
        "U_211_FI",
        "U_CCHK"
    )
    VALUES
    (
        vCode,
        vName,
        vHTT_CA,
        vHTH_PL,
        vHTT_TF,
        v201_TR,
        v202_SF,
        v203_CAP,
        v204_TCC,
        v205_MCC,
        v206_NCC,
        v207_MTS,
        v208_MTD,
        v209_TC,
        v210_IC,
        v211_FI,
        vCCHK
    );
END;

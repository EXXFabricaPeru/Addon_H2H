CREATE PROCEDURE "SP_EXX_BCP_A_TRANSFERENCIA_ABONO"
(
IN vCode NVARCHAR(50),
IN vName NVARCHAR(50),
IN vHTT_AB NVARCHAR(254),
IN vHTT_CA NVARCHAR(254),
IN vHTH_PL NVARCHAR(254),
IN vHTT_TF NVARCHAR(254),
IN v301_TR NVARCHAR(1),
IN v302_SF NVARCHAR(6),
IN v303_TC NVARCHAR(1),
IN v304_CP NVARCHAR(1),
IN v305_NCA NVARCHAR(20),
IN v306_TDB NVARCHAR(3),
IN v307_NDB NVARCHAR(12),
IN v308_CD NVARCHAR(3),
IN v309_NB NVARCHAR(75),
IN v310_MMT NVARCHAR(4),
IN v311_MO NVARCHAR(17),
IN v312_RE NVARCHAR(40),
IN v313_IA NVARCHAR(32),
IN v314_TC NVARCHAR(1),
IN v315_ECBCP NVARCHAR(100),
IN v316_FI NVARCHAR(34),
IN vACHK NVARCHAR(30)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    INSERT INTO "@EXX_BCP_TRAN_ABO"
    (
        "Code",
        "Name",
        "U_HTT_AB",
        "U_HTT_CA",
        "U_HTH_PL",
        "U_HTT_TF",
        "U_301_TR",
        "U_302_SF",
        "U_303_TC",
        "U_304_CP",
        "U_305_NCA",
        "U_306_TDB",
        "U_307_NDB",
        "U_308_CD",
        "U_309_NB",
        "U_310_MMT",
        "U_311_MO",
        "U_312_RE",
        "U_313_IA",
        "U_314_TC",
        "U_315_ECBCP",
        "U_316_FI",
        "U_ACHK"
    )
    VALUES
    (
        vCode,
        vName,
        vHTT_AB,
        vHTT_CA,
        vHTH_PL,
        vHTT_TF,
        v301_TR,
        v302_SF,
        v303_TC,
        v304_CP,
        v305_NCA,
        v306_TDB,
        v307_NDB,
        v308_CD,
        v309_NB,
        v310_MMT,
        v311_MO,
        v312_RE,
        v313_IA,
        v314_TC,
        v315_ECBCP,
        v316_FI,
        vACHK
    );
END;

ALTER PROCEDURE dbo.sp_GetCustomerSchemeDetailsByMobile
(
    @Mobile VARCHAR(15)
)
AS
BEGIN
    SET NOCOUNT ON;

    /* =========================
       1. Scheme Transaction Details
       ========================= */
    SELECT
        PI.PNAME,
        PI.DOORNO,
        PI.ADDRESS1,
        PI.ADDRESS2,
        PI.AREA,
        PI.CITY,
        PI.STATE,
        PI.COUNTRY,
        PI.PINCODE,
        PI.EMAIL,
        SM.GROUPCODE,
        SM.REGNO,
        SM.JOINDATE,
		DATEADD(MONTH,S.INSTALMENT,SM.JOINDATE)MATURITYDATE,
        ST.AMOUNT,
        ST.WEIGHT,
        ST.RECEIPTNO,
        ST.RDATE,
        ST.INSTALLMENT
    FROM PERSONALINFO PI
    INNER JOIN SCHEMEMAST SM 
        ON SM.SNO = PI.PERSONALID
	INNER JOIN SCHEME S
		ON SM.SCHEMEID = S.SchemeId 
    INNER JOIN BMPSH0708..SCHEMETRAN ST 
        ON ST.GROUPCODE = SM.GROUPCODE
       AND ST.REGNO = SM.REGNO
    WHERE PI.MOBILE = @Mobile
      AND SM.DOCLOSE IS NULL
      AND ISNULL(ST.CANCEL, '') = ''
    ORDER BY ST.GROUPCODE, ST.REGNO, ST.INSTALLMENT;


    /* =========================
       2. Scheme Collection Details
       ========================= */
    SELECT
        SC.GROUPCODE,
        SC.REGNO,
        SC.RECEIPTNO,
        SC.RDATE,
        SC.AMOUNT,
        SC.CHQ_CARDNO,
        SC.CHQDATE,
        SC.CHQBANK,
        SC.CHQRTNREASON,
        SC.CHQBRANCH
    FROM PERSONALINFO PI
    INNER JOIN SCHEMEMAST SM 
        ON SM.SNO = PI.PERSONALID
    INNER JOIN BMPSH0708..SCHEMECOLLECT SC 
        ON SC.GROUPCODE = SM.GROUPCODE
       AND SC.REGNO = SM.REGNO
    WHERE PI.MOBILE = @Mobile
      AND SM.DOCLOSE IS NULL
      AND ISNULL(SC.CANCEL, '') = ''
    ORDER BY SC.GROUPCODE, SC.REGNO;


    /* =========================
       3. Total Collection Amount (Group-wise)
       ========================= */
    SELECT
        SC.GROUPCODE,
        SC.REGNO,
        SUM(SC.AMOUNT) AS AMOUNT
    FROM PERSONALINFO PI
    INNER JOIN SCHEMEMAST SM 
        ON SM.SNO = PI.PERSONALID
    INNER JOIN BMPSH0708..SCHEMECOLLECT SC 
        ON SC.GROUPCODE = SM.GROUPCODE
       AND SC.REGNO = SM.REGNO
    WHERE PI.MOBILE = @Mobile
      AND SM.DOCLOSE IS NULL
      AND ISNULL(SC.CANCEL, '') = ''
    GROUP BY SC.GROUPCODE, SC.REGNO
    ORDER BY SC.GROUPCODE, SC.REGNO;

END;
GO

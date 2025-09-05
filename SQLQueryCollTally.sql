declare @date as date = '2024-05-08'
;with sal as(
select batchno,sum(AMOUNT)samt,sum(TAX)stax,sum(AMOUNT)+sum(TAX) sGAMT from ISSUE where TRANDATE = @date and CANCEL = '' and TRANTYPE = 'SA'
group by batchno
),
pur as(
select batchno,sum(AMOUNT)pamt from RECEIPT where TRANDATE = @date and CANCEL = '' and TRANTYPE = 'PU'
group by batchno
),
sp as(
select s.BATCHNO,s.samt,s.stax,s.sGAMT,p.pamt,(s.sGAMT-isnull(p.pamt,0))fAmt from sal s
left join pur p on s.BATCHNO = p.BATCHNO 
),
col as(
select BATCHNO,sum(AMOUNT) purAMOUNT from ACCTRAN where TRANDATE = @date and CANCEL = '' and PAYMODE = 'PU'
group by batchno
),
sv as(
select BATCHNO,sum(AMOUNT) svAMOUNT from ACCTRAN where TRANDATE = @date and CANCEL = '' and PAYMODE = 'SV'
group by batchno
),
Hc as(
select BATCHNO,sum(AMOUNT) HcAMOUNT from ACCTRAN where TRANDATE = @date and CANCEL = '' and PAYMODE = 'HC'
group by batchno
),
sa as(
select BATCHNO,sum(AMOUNT) saAMOUNT from ACCTRAN where TRANDATE = @date and CANCEL = '' and PAYMODE = 'SA'
group by batchno
),
cash as(
select BATCHNO,sum(AMOUNT) cashAMOUNT from ACCTRAN where TRANDATE = @date and CANCEL = '' and PAYMODE = 'CA'
group by batchno
),
sr as(
select BATCHNO,sum(AMOUNT) srAMOUNT from ACCTRAN where TRANDATE = @date and CANCEL = '' and PAYMODE = 'SR'
group by batchno
),
ch as(
select BATCHNO,sum(AMOUNT) chAMOUNT from ACCTRAN where TRANDATE = @date and CANCEL = '' and PAYMODE = 'CH'
group by batchno
),
aa as(
select BATCHNO,sum(AMOUNT) aaAMOUNT from ACCTRAN where TRANDATE = @date and CANCEL = '' and PAYMODE = 'AA'
group by batchno
),
du as(
select BATCHNO,sum(AMOUNT) duAMOUNT from ACCTRAN where TRANDATE = @date and CANCEL = '' and PAYMODE = 'DU'
group by batchno
),
cc as(
select BATCHNO,sum(AMOUNT) ccAMOUNT from ACCTRAN where TRANDATE = @date and CANCEL = '' and PAYMODE = 'CC'
group by batchno
),
ar as(
select BATCHNO,sum(AMOUNT) arAMOUNT from ACCTRAN where TRANDATE = @date and CANCEL = '' and PAYMODE = 'AR'
group by batchno
)
select sp.*,col.purAMOUNT,(sp.pamt-col.purAMOUNT)purT,sv.svAMOUNT,(sv.svAMOUNT-sp.stax) sv,sa.saAMOUNT,(sa.saAMOUNT-sp.samt)salAccDiff,(pamt-col.purAMOUNT)purDiff,
cashAMOUNT,srAMOUNT,chAMOUNT,aaAMOUNT,duAMOUNT,(ccAMOUNT-HcAMOUNT) cc,arAMOUNT,
fAmt - (isnull(cashAMOUNT,0)+isnull(srAMOUNT,0)+isnull(chAMOUNT,0)+isnull(aaAMOUNT,0)+isnull(duAMOUNT,0)+isnull(ccAMOUNT,0)+isnull(arAMOUNT,0)-isnull(HcAMOUNT,0))finTally from sp 
left join col on sp.BATCHNO = col.BATCHNO 
left join sv on sp.BATCHNO = sv.BATCHNO 
left join sa on sp.BATCHNO = sa.BATCHNO 
left join cash on sp.BATCHNO = cash.BATCHNO 
left join sr on sp.BATCHNO = sr.BATCHNO
left join ch on sp.BATCHNO = ch.BATCHNO 
left join aa on sp.BATCHNO = aa.BATCHNO 
left join du on sp.BATCHNO = du.BATCHNO 
left join cc on sp.BATCHNO = cc.BATCHNO 
left join ar on sp.BATCHNO = ar.BATCHNO 
left join hc on sp.BATCHNO = Hc.BATCHNO 

--select distinct PAYMODE  from ACCTRAN where TRANDATE = '2024-05-03' and CANCEL = '' 
--select distinct PAYMODE  from ACCTRAN where TRANMODE = 'D' order by PAYMODE 

--select BATCHNO,* from ACCTRAN where BATCHNO = '00SFH251362'

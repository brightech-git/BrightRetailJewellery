declare @HOCOST as varchar(2) = (select COSTID from BMGADMINDB..SYNCCOSTCENTRE where 1=1 and ACTIVE = 'Y' and MAIN = 'Y')
;with cte as(
select ACCODE,ACNAME from BMGADMINDB..ACHEAD where 1=1
and ACTIVE = 'Y'
),
cte1 as(
select cte.accode,
SUM(taxtran.amount)RAMOUNT,
SUM(CASE WHEN taxid = 'CG' THEN TAXAMOUNT ELSE 0 END) AS RCGST,
SUM(CASE WHEN taxid = 'SG' THEN TAXAMOUNT ELSE 0 END) AS RSGST,
SUM(CASE WHEN taxid = 'IG' THEN TAXAMOUNT ELSE 0 END) AS RIGST,
SUM(taxtran.amount) + SUM(TAXAMOUNT) AS RGROSSAMT
from BMGT2526..RECEIPT 
join BMGT2526..TAXTRAN on RECEIPT.BATCHNO = TAXTRAN.BATCHNO 
join cte on RECEIPT.ACCODE = cte.ACCODE 
where 1=1
and isnull(receipt.cancel,'')=''
and RECEIPT.TRANTYPE = 'RPU'
and RECEIPT.COSTID = @HOCOST
group by cte.accode
),
cte2 as(
select cte.accode,sum(grswt)RWT
from BMGT2526..RECEIPT 
join cte on RECEIPT.ACCODE = cte.ACCODE 
where 1=1
and isnull(receipt.cancel,'')=''
and RECEIPT.TRANTYPE = 'RPU'
and RECEIPT.COSTID = @HOCOST
group by cte.accode
),
cte3 as(
select ACCODE,sum(TWT) TWT from(
select cte.ACCODE, sum(itemtag.grswt)TWT from BMGADMINDB..ITEMTAG 
join BMGADMINDB..DESIGNER on ITEMTAG.DESIGNERID = DESIGNER.DESIGNERID
join cte on DESIGNER.ACCODE = cte.ACCODE 
where 1=1
and ITEMTAG.COSTID = @HOCOST
group by cte.ACCODE
UNION
select cte.ACCODE, sum(CITEMTAG.grswt)TWT from BMGADMINDB..CITEMTAG 
join BMGADMINDB..DESIGNER on CITEMTAG.DESIGNERID = DESIGNER.DESIGNERID
join cte on DESIGNER.ACCODE = cte.ACCODE 
where 1=1
and CITEMTAG.COSTID = @HOCOST
group by cte.ACCODE)as x
group by ACCODE 
),
cte4 as(
select cte.accode,sum(grswt)PRWT
from BMGT2526..ISSUE 
join cte on ISSUE.ACCODE = cte.ACCODE 
where 1=1
and isnull(ISSUE.cancel,'')=''
and ISSUE.TRANTYPE = 'IPU'
and ISSUE.COSTID = @HOCOST
group by cte.accode
),
cte5 as(
select cte.accode,
SUM(taxtran.amount)PRAMOUNT,
SUM(CASE WHEN taxid = 'CG' THEN TAXAMOUNT ELSE 0 END) AS PRCGST,
SUM(CASE WHEN taxid = 'SG' THEN TAXAMOUNT ELSE 0 END) AS PRSGST,
SUM(CASE WHEN taxid = 'IG' THEN TAXAMOUNT ELSE 0 END) AS PRIGST,
SUM(taxtran.amount) + SUM(TAXAMOUNT) AS PRGROSSAMT
from BMGT2526..ISSUE 
join BMGT2526..TAXTRAN on ISSUE.BATCHNO = TAXTRAN.BATCHNO 
join cte on ISSUE.ACCODE = cte.ACCODE 
where 1=1
and isnull(ISSUE.cancel,'')=''
and ISSUE.TRANTYPE = 'IPU'
and ISSUE.COSTID = @HOCOST
group by cte.accode
)
select cte.ACCODE,cte.ACNAME,cte1.RAMOUNT,cte1.RCGST,cte1.RSGST,cte1.RIGST,cte1.RGROSSAMT,cte2.RWT,cte3.TWT,(isnull(cte3.TWT,0)-isnull(cte2.RWT,0))BALWT,
cte4.PRWT,cte5.PRAMOUNT,cte5.PRCGST,cte5.PRSGST,cte5.PRIGST,cte5.PRGROSSAMT,
(isnull(cte2.RWT,0)-isnull(cte4.PRWT,0))NETWT,
(isnull(cte3.TWT,0)-isnull(cte4.PRWT,0))ACTWT,
(isnull(cte3.TWT,0)-isnull(cte4.PRWT,0))-(isnull(cte2.RWT,0)-isnull(cte4.PRWT,0)) ACTWTDIFF
from cte
left join cte1 on cte.accode = cte1.accode
left join cte2 on cte.accode = cte2.accode
left join cte3 on cte.accode = cte3.accode
left join cte4 on cte.accode = cte4.accode
left join cte5 on cte.accode = cte5.accode


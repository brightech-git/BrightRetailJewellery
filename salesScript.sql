select METALMAST.METALNAME,sum(PCS)PCS,sum(itemtag.GRSWT)GRSWT,sum(NETWT)NETWT,sum(LESSWT)LESSWT,sum(isnull(ITEMTAGSTONE.stnwt,0))StnWt from ITEMTAG 
join ITEMMAST on itemtag.itemid = itemmast.itemid
join METALMAST on ITEMMAST.METALID = METALMAST.METALID 
left join ITEMTAGSTONE on ITEMTAG.tagno = ITEMTAGSTONE.tagno
where 1=1
and ITEMTAG.issdate is null
and ITEMTAG.costid = 'FL'
group by METALMAST.METALNAME,metalmast.displayorder
order by metalmast.displayorder


select METALMAST.METALNAME,sum(PCS)PCS,sum(issue.GRSWT)GRSWT,sum(NETWT)NETWT,sum(LESSWT)LESSWT from sflt2526..issue 
join ITEMMAST on issue.itemid = itemmast.itemid
join METALMAST on ITEMMAST.METALID = METALMAST.METALID 
where 1=1
and costid = 'FL'
and cancel <> 'Y'
and ISSUE.TRANTYPE = 'SA'
and issue.TRANDATE between '2025-10-18' and '2025-10-18'
group by METALMAST.METALNAME,metalmast.displayorder
order by metalmast.displayorder

select METALMAST.METALNAME,sum(PCS)PCS,sum(RECEIPT.GRSWT)GRSWT,sum(NETWT)NETWT,sum(LESSWT)LESSWT from sflt2526..RECEIPT  
join ITEMMAST on RECEIPT.itemid = itemmast.itemid
join METALMAST on ITEMMAST.METALID = METALMAST.METALID 
where 1=1
and costid = 'FL'
and cancel <> 'Y'
and RECEIPT.TRANTYPE in ('PU','SR')
and RECEIPT.TRANDATE between '2025-10-02' and '2025-10-18'
group by METALMAST.METALNAME,metalmast.displayorder
order by metalmast.displayorder

select case when PAYMODE = 'CC' then 'CARD' when PAYMODE = 'CA' then 'CASH' when PAYMODE = 'CH' then 'CHEQUE\UPI' when PAYMODE in ('SS','CB') then 'SCHEME ADJUSTED' else 'OTHERS' end PAYMODE,
sum(AMOUNT)AMOUNT from sflt2526..acctran
where 1=1
and cancel <> 'Y'
and tranmode = 'D'
and acctran.TRANDATE between '2025-10-01' and '2025-10-01'
group by PAYMODE

select PAYMODE,sum(AMOUNT)AMOUNT from sflt2526..acctran
where 1=1
and cancel <> 'Y'
and tranmode = 'D'
and acctran.TRANDATE between '2025-10-01' and '2025-10-01'
group by PAYMODE

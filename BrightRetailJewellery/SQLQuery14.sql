select * from DESIGNER where ACCODE = '0000027'

select * from ITEMTAG where DESIGNERID = 25
select * from BMPADMINDB..ITEMTAG where DESIGNERID = 25

select a.TAGNO,a.GRSWT, * from ITEMTAG A
left join BMPADMINDB..ITEMTAG B on a.TAGNO = b.TAGNO 
where A.DESIGNERID = 25
--and b.TAGNO is null
order by b.tagno



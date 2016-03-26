# AdService
WCF Ad Data Service Test

#All Data
select * from addata order by brandname asc;

#Cover Ads with >0.5 pages
select * from addata 
where numpages > 0.5
and position='Cover'
order by brandname asc;

#Top 5 Ads
select * from addata 
group by brandid
order by numpages desc, brandname asc
limit 5;

#Top 5 Brands
select brandname, sum(numpages)
from addata
group by brandid
order by sum(numpages) desc, brandname asc
limit 5;

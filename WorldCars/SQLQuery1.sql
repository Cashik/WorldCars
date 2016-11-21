GO
DROP TRIGGER RatingUpdater;
go
Create TRIGGER RatingUpdater  
ON Comment
AFTER DELETE,UPDATE,INSERT
AS 

UPDATE CarInfo 
Set CarInfo.rating= (
select AVG(C.rating) 
from Comment as C
where C.car_info_id = (select car_info_id from inserted,deleted)
)
where Id = (select car_info_id from inserted,deleted);
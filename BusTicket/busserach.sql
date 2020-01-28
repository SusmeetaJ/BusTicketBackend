select b.BusCapacity,b.BusType,j.ArrivalTime,j.DepartureTime,j.JourneyFare,r.ToId,r.FromId
from T_BusDetails b
inner join T_JourneyDetails j on b.BusId=j.BusId
inner join T_RouteDetail r on j.RouteId=r.RouteId
where r.RouteId = (select r1.RouteId
from T_RouteDetail r1
where r1.FromId='Karad' and r1.ToId='Pune'
)
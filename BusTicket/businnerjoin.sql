select b.BusCapacity, b.BusCapacity, j.ArrivalTime, r.FromId, r.ToId
from T_BusDetails b
inner join T_JourneyDetails j on b.BusId = j.BusId
inner join T_RouteDetail r on j.RouteId = r.RouteId
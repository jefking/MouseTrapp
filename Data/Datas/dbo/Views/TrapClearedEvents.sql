Create View TrapClearedEvents as
 select CaughtEvent.TrapId, CaughtEvent.building, CaughtEvent.location, CaughtEvent.eventDate as CaughtDate, ClearedEvent.eventDate as ClearedDate,
 DATEDIFF(minute, CaughtEvent.eventDate, ClearedEvent.eventDate) as MinutesToClear
 from Trapevents CaughtEvent
	 Left Outer Join Trapevents ClearedEvent
	 On CaughtEvent.TrapId = ClearedEvent.TrapId
	and ClearedEvent.EventType = 1
	and ClearedEvent.eventDate = (select min(eventDate) 
									from Trapevents ClearedEvents
									where ClearedEvents.TrapId = CaughtEvent.TrapId
										and ClearedEvents.EventType = 1
										and ClearedEvents.eventDate > CaughtEvent.eventDate
									)
where CaughtEvent.EventType = 2

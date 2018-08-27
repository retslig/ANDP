

CREATE PROCEDURE [gdo].[sp_UpdateOrderStatus]
@OrderId int,
@StatusTypeId int
AS

--update equipment
UPDATE [gdo].[equipment]
SET StatusTypeId = @StatusTypeId
WHERE ItemId IN (
	SELECT i.Id
	FROM [gdo].[item] AS i
	JOIN [gdo].[service] AS s on s.id = i.ServiceId
	JOIN gdo.[Order] as o on o.id = s.OrderId
);

--update items
UPDATE [gdo].[item]
SET StatusTypeId = @StatusTypeId
WHERE ServiceId IN (
	SELECT s.Id
	FROM [gdo].[service] AS s
	JOIN gdo.[Order] as o on o.id = s.OrderId
);

--update service
UPDATE [gdo].[service]
SET StatusTypeId = @StatusTypeId
WHERE OrderId = @OrderId;

--update order
UPDATE [gdo].[Order] 
SET StatusTypeId = @StatusTypeId 
WHERE id = @OrderId;



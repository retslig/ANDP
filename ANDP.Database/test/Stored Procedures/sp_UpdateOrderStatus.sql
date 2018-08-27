

CREATE PROCEDURE [test].[sp_UpdateOrderStatus]
@OrderId int,
@StatusTypeId int
AS

--update equipment
UPDATE [test].[equipment]
SET StatusTypeId = @StatusTypeId
WHERE ItemId IN (
	SELECT i.Id
	FROM [test].[item] AS i
	JOIN [test].[service] AS s on s.id = i.ServiceId
	JOIN test.[Order] as o on o.id = s.OrderId
);

--update items
UPDATE [test].[item]
SET StatusTypeId = @StatusTypeId
WHERE ServiceId IN (
	SELECT s.Id
	FROM [test].[service] AS s
	JOIN test.[Order] as o on o.id = s.OrderId
);

--update service
UPDATE [test].[service]
SET StatusTypeId = @StatusTypeId
WHERE OrderId = @OrderId;

--update order
UPDATE [test].[Order] 
SET StatusTypeId = @StatusTypeId 
WHERE id = @OrderId;





CREATE PROCEDURE [ral].[sp_UpdateOrderStatus]
@OrderId int,
@StatusTypeId int
AS

--update equipment
UPDATE [ral].[equipment]
SET StatusTypeId = @StatusTypeId
WHERE ItemId IN (
	SELECT i.Id
	FROM [ral].[item] AS i
	JOIN [ral].[service] AS s on s.id = i.ServiceId
	JOIN ral.[Order] as o on o.id = s.OrderId
);

--update items
UPDATE [ral].[item]
SET StatusTypeId = @StatusTypeId
WHERE ServiceId IN (
	SELECT s.Id
	FROM [ral].[service] AS s
	JOIN ral.[Order] as o on o.id = s.OrderId
);

--update service
UPDATE [ral].[service]
SET StatusTypeId = @StatusTypeId
WHERE OrderId = @OrderId;

--update order
UPDATE [ral].[Order] 
SET StatusTypeId = @StatusTypeId 
WHERE id = @OrderId;





CREATE PROCEDURE [emp].[sp_UpdateOrderStatus]
@OrderId int,
@StatusTypeId int
AS

--update equipment
UPDATE [emp].[equipment]
SET StatusTypeId = @StatusTypeId
WHERE ItemId IN (
	SELECT i.Id
	FROM [emp].[item] AS i
	JOIN [emp].[service] AS s on s.id = i.ServiceId
	JOIN emp.[Order] as o on o.id = s.OrderId
);

--update items
UPDATE [emp].[item]
SET StatusTypeId = @StatusTypeId
WHERE ServiceId IN (
	SELECT s.Id
	FROM [emp].[service] AS s
	JOIN emp.[Order] as o on o.id = s.OrderId
);

--update service
UPDATE [emp].[service]
SET StatusTypeId = @StatusTypeId
WHERE OrderId = @OrderId;

--update order
UPDATE [emp].[Order] 
SET StatusTypeId = @StatusTypeId 
WHERE id = @OrderId;



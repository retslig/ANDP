

CREATE PROCEDURE [ctc].[sp_UpdateOrderStatus]
@OrderId int,
@StatusTypeId int
AS

--update equipment
UPDATE [ctc].[equipment]
SET StatusTypeId = @StatusTypeId
WHERE ItemId IN (
	SELECT i.Id
	FROM [ctc].[item] AS i
	JOIN [ctc].[service] AS s on s.id = i.ServiceId
	JOIN ctc.[Order] as o on o.id = s.OrderId
);

--update items
UPDATE [ctc].[item]
SET StatusTypeId = @StatusTypeId
WHERE ServiceId IN (
	SELECT s.Id
	FROM [ctc].[service] AS s
	JOIN ctc.[Order] as o on o.id = s.OrderId
);

--update service
UPDATE [ctc].[service]
SET StatusTypeId = @StatusTypeId
WHERE OrderId = @OrderId;

--update order
UPDATE [ctc].[Order] 
SET StatusTypeId = @StatusTypeId 
WHERE id = @OrderId;





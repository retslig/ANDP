

CREATE PROCEDURE [srtc].[sp_UpdateOrderStatus]
@OrderId int,
@StatusTypeId int
AS

--update equipment
UPDATE [srtc].[equipment]
SET StatusTypeId = @StatusTypeId
WHERE ItemId IN (
	SELECT i.Id
	FROM [srtc].[item] AS i
	JOIN [srtc].[service] AS s on s.id = i.ServiceId
	JOIN srtc.[Order] as o on o.id = s.OrderId
);

--update items
UPDATE [srtc].[item]
SET StatusTypeId = @StatusTypeId
WHERE ServiceId IN (
	SELECT s.Id
	FROM [srtc].[service] AS s
	JOIN srtc.[Order] as o on o.id = s.OrderId
);

--update service
UPDATE [srtc].[service]
SET StatusTypeId = @StatusTypeId
WHERE OrderId = @OrderId;

--update order
UPDATE [srtc].[Order] 
SET StatusTypeId = @StatusTypeId 
WHERE id = @OrderId;






CREATE PROCEDURE [ctc].[sp_DeleteOrder]
@OrderId int
AS



	WITH equipmentCTE AS
	(
		select e.Id from  [ctc].[service] as s
		join [ctc].[item] as i on i.ServiceId = s.Id
		join [ctc].[equipment] as e on e.ItemId = i.Id
		where s.OrderId = @OrderId
	)
	delete [ctc].[equipment]
	FROM [ctc].[equipment] Z
	INNER JOIN equipmentCTE ON equipmentCTE.Id = Z.Id;


	WITH itemCTE AS
	(
		select i.Id from  [ctc].[service] as s
		join [ctc].[item] as i on i.ServiceId = s.Id
		where s.OrderId = @OrderId
	)
	delete [ctc].[item]
	FROM [ctc].[item] Z
	INNER JOIN itemCTE ON itemCTE.Id = Z.Id;

	WITH serviceCTE AS
	(
		select s.Id from  [ctc].[service] as s
		where s.OrderId = @OrderId
	)
	delete [ctc].[service]
	FROM [ctc].[service] Z
	INNER JOIN serviceCTE ON serviceCTE.Id = Z.Id;


	delete [ctc].[Order]  where id = @OrderId;






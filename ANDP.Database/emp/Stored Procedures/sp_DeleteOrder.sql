
CREATE PROCEDURE [emp].[sp_DeleteOrder]
@OrderId int
AS



	WITH equipmentCTE AS
	(
		select e.Id from  [emp].[service] as s
		join [emp].[item] as i on i.ServiceId = s.Id
		join [emp].[equipment] as e on e.ItemId = i.Id
		where s.OrderId = @OrderId
	)
	delete [emp].[equipment]
	FROM [emp].[equipment] Z
	INNER JOIN equipmentCTE ON equipmentCTE.Id = Z.Id;


	WITH itemCTE AS
	(
		select i.Id from  [emp].[service] as s
		join [emp].[item] as i on i.ServiceId = s.Id
		where s.OrderId = @OrderId
	)
	delete [emp].[item]
	FROM [emp].[item] Z
	INNER JOIN itemCTE ON itemCTE.Id = Z.Id;

	WITH serviceCTE AS
	(
		select s.Id from  [emp].[service] as s
		where s.OrderId = @OrderId
	)
	delete [emp].[service]
	FROM [emp].[service] Z
	INNER JOIN serviceCTE ON serviceCTE.Id = Z.Id;


	delete [emp].[Order]  where id = @OrderId;







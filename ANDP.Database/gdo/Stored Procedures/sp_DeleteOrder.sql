
CREATE PROCEDURE [gdo].[sp_DeleteOrder]
@OrderId int
AS



	WITH equipmentCTE AS
	(
		select e.Id from  [gdo].[service] as s
		join [gdo].[item] as i on i.ServiceId = s.Id
		join [gdo].[equipment] as e on e.ItemId = i.Id
		where s.OrderId = @OrderId
	)
	delete [gdo].[equipment]
	FROM [gdo].[equipment] Z
	INNER JOIN equipmentCTE ON equipmentCTE.Id = Z.Id;


	WITH itemCTE AS
	(
		select i.Id from  [gdo].[service] as s
		join [gdo].[item] as i on i.ServiceId = s.Id
		where s.OrderId = @OrderId
	)
	delete [gdo].[item]
	FROM [gdo].[item] Z
	INNER JOIN itemCTE ON itemCTE.Id = Z.Id;

	WITH serviceCTE AS
	(
		select s.Id from  [gdo].[service] as s
		where s.OrderId = @OrderId
	)
	delete [gdo].[service]
	FROM [gdo].[service] Z
	INNER JOIN serviceCTE ON serviceCTE.Id = Z.Id;


	delete [gdo].[Order]  where id = @OrderId;







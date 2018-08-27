
CREATE PROCEDURE [ral].[sp_DeleteOrder]
@OrderId int
AS



	WITH equipmentCTE AS
	(
		select e.Id from  [ral].[service] as s
		join [ral].[item] as i on i.ServiceId = s.Id
		join [ral].[equipment] as e on e.ItemId = i.Id
		where s.OrderId = @OrderId
	)
	delete [ral].[equipment]
	FROM [ral].[equipment] Z
	INNER JOIN equipmentCTE ON equipmentCTE.Id = Z.Id;


	WITH itemCTE AS
	(
		select i.Id from  [ral].[service] as s
		join [ral].[item] as i on i.ServiceId = s.Id
		where s.OrderId = @OrderId
	)
	delete [ral].[item]
	FROM [ral].[item] Z
	INNER JOIN itemCTE ON itemCTE.Id = Z.Id;

	WITH serviceCTE AS
	(
		select s.Id from  [ral].[service] as s
		where s.OrderId = @OrderId
	)
	delete [ral].[service]
	FROM [ral].[service] Z
	INNER JOIN serviceCTE ON serviceCTE.Id = Z.Id;


	delete [ral].[Order]  where id = @OrderId;







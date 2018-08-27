
CREATE PROCEDURE [test].[sp_DeleteOrder]
@OrderId int
AS



	WITH equipmentCTE AS
	(
		select e.Id from  [test].[service] as s
		join [test].[item] as i on i.ServiceId = s.Id
		join [test].[equipment] as e on e.ItemId = i.Id
		where s.OrderId = @OrderId
	)
	delete [test].[equipment]
	FROM [test].[equipment] Z
	INNER JOIN equipmentCTE ON equipmentCTE.Id = Z.Id;


	WITH itemCTE AS
	(
		select i.Id from  [test].[service] as s
		join [test].[item] as i on i.ServiceId = s.Id
		where s.OrderId = @OrderId
	)
	delete [test].[item]
	FROM [test].[item] Z
	INNER JOIN itemCTE ON itemCTE.Id = Z.Id;

	WITH serviceCTE AS
	(
		select s.Id from  [test].[service] as s
		where s.OrderId = @OrderId
	)
	delete [test].[service]
	FROM [test].[service] Z
	INNER JOIN serviceCTE ON serviceCTE.Id = Z.Id;


	delete [test].[Order]  where id = @OrderId;







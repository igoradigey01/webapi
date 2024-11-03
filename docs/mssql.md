### create function  :
```  create fuction  last_id() :

Use OrderDB;
CREATE  FUNCTION  [dbo].last_id_order_detail()
RETURNS int

BEGIN
DECLARE  @LIMIT INT ;
SELECT    @LIMIT= MAX(id) 
     
  FROM   [OrderDB].[dbo].[OrderDetail];
if(@LIMIT is NULL)
   RETURN  0 ;

    RETURN @LIMIT ;
END;


```

```
!!!  не используется  реализвовано в  на клиенте :   private void   GetOrdrNO(string owner_id, out int no_order) !!!
-- Active: 1729270188056@@127.0.0.1@1433@OrderDB@dbo
DROP FUNCTION max_order_no;
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[max_order_no]( @owner_id  nvarchar(50) )

RETURNS int 
AS
BEGIN
	-- Declare the return variable here
    
     declare   @order_no  int;

     
     

SELECT    @order_no =   MAX(no_order) 
FROM  [dbo].[Sequence]
 where    [dbo].[Sequence].[owner_id] =  @owner_id


	RETURN  @order_no;

END
```

```create function new_order_no() :  !!! EXEC  [dbo].new_order_no    @owner_id=' x-01' !!! перед  х-01 должен быть пробел !!!!   
!!!  не используется  реализвовано в  на клиенте :   private void   GetOrdrNO(string owner_id, out int no_order) !!!

CREATE  FUNCTION  [dbo].[new_order_no]( @owner_id  nvarchar(50) )
 RETURNS INT
 
BEGIN
    declare  @order_new int;
     declare   @order_no  int;

     if(@owner_id is null)
     set @owner_id='' ;
     
     set @order_new=0;
     set @order_no =0;

SELECT    @order_no =   MAX(no_order) 
FROM [OrderDB].[dbo].[Sequence]
 where    [dbo].[Sequence].owner_id=@owner_id

 set  @order_new = @order_no+1;

 DECLARE @sql nvarchar(max);
 
 
 

SET @sql = N' INSERT INTO [dbo].[Sequence] ([owner_id],[no_order]) VALUES ( ' + ' '+ @owner_id +' ' + ','   + CAST( @order_new as NVARCHAR) +' )';


EXEC sys.sp_executesql @sql;

 


if( @order_no is NULL)
   RETURN  0 ;

RETURN  @order_no;
END



```

```
DECLARE @RowTo int;
SET @RowTo = 5;

DECLARE @sql nvarchar(max);
SET @sql = N'SELECT ' + CONVERT(varchar(12), @RowTo) + ' * 5';
EXEC sys.sp_executesql @sql;

```

```
GO
DECLARE @ColumnName VARCHAR(100),
        @SQL NVARCHAR(MAX);
SET @ColumnName = 'FirstName';
SET @SQL = 'SELECT ' + @ColumnName + ' FROM Person.Person';
EXEC sys.sp_executesql @SQL  
GO
```

```
EXEC  [dbo].new_order_no    @owner_id='x-01'
```
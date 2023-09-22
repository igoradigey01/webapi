## Story_procekdure
- https://oracleplsql.ru/procedures-mysql.html
- https://www.mysqltutorial.org/mysql-stored-procedure-tutorial.aspx
- https://dev.mysql.com/doc/connector-net/en/connector-net-programming-stored-proc.html

### create function  :

```  create function new_order_no() :
CREATE DEFINER=`root`@`%` FUNCTION `new_order_no`(
  owner_id  VARCHAR(50)
) RETURNS int
    DETERMINISTIC
BEGIN

declare  order_new int;
declare   order_no  int;
SELECT     `Sequence`.`no_order`   
FROM `OrderDB`.`Sequence`
 where    `Sequence`.`owner_id`=owner_id
  ORDER BY id DESC LIMIT 1
  INTO order_no  ;
  set  order_new = order_no+1;
  
  INSERT INTO `OrderDB`.`Sequence`
(`id`,
`owner_id`,
`no_order`)
VALUES
(0,
owner_id,
order_new);

RETURN  order_no;
END
```

```
CREATE DEFINER=`root`@`%` FUNCTION `last_id_order_detail`() RETURNS int
    DETERMINISTIC
BEGIN
declare  last_id int;
SELECT id FROM OrderDB.OrderDetail ORDER BY id DESC LIMIT 1
  INTO  last_id  ;
RETURN last_id;
END
```
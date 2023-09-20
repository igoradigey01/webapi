## Story_procekdure
- https://oracleplsql.ru/procedures-mysql.html
- https://www.mysqltutorial.org/mysql-stored-procedure-tutorial.aspx

```
USE `OrderDB`;
DROP procedure IF EXISTS `next_order_no`;

DELIMITER $$
USE `OrderDB`$$
CREATE PROCEDURE `next_order_no` (
in  owner_id  VARCHAR(50),
out order_no int
)
BEGIN
SELECT  order_no =   `Sequence`.`no_order`   
FROM `OrderDB`.`Sequence`
 where    `Sequence`.`owner_id`=owner_id
  ORDER BY id DESC LIMIT 1;
END$$

DELIMITER ;
```
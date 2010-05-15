SELECT pb.productname, SUM(co.qty) WipQty
FROM container co INNER JOIN currentstatus cs ON co.currentstatusid = cs.currentstatusid
INNER JOIN spec ON spec.specid = cs.specid
INNER JOIN operation op ON spec.operationid = op.operationid
INNER JOIN product pt ON co.productid = pt.productid
INNER JOIN productbase pb ON pt.productbaseid = pb.productbaseid
WHERE op.operationname = &OPERATION_NAME
GROUP BY pb.productname
ORDER BY pb.productname
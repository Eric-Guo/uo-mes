SELECT wlh.productname, sum(wlh.moveinqty) InQty, sum(wlh.moveoutqty) OutQty, 
round(AVG(wlh.moveouttimestamp-wlh.moveintimestamp)*24,2) CycleTime, sum(wlh.totalrejectqty) RejectQty
FROM a_wiplothistory wlh INNER JOIN specbase sb ON wlh.specname = sb.specname
INNER JOIN spec ON sb.specbaseid = spec.specbaseid
INNER JOIN operation op ON op.operationid = spec.operationid
WHERE op.operationname = :OPERATION_NAME
  AND wlh.moveintimestamp > :START_DATE
  AND wlh.moveouttimestamp < :END_DATE
GROUP BY wlh.productname
ORDER BY wlh.productname

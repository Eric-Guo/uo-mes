SELECT DISTINCT objecttype, operationname
FROM (SELECT op.objecttype, op.operationname
FROM operation op INNER JOIN spec ON spec.operationid=op.operationid
INNER JOIN specbase sb ON sb.specbaseid = spec.specbaseid
INNER JOIN Workflowstep ws ON ws.specbaseid = sb.specbaseid
WHERE spec.resourcegroupid IS NOT NULL
  AND op.objectcategory = 'WIP'
ORDER BY op.objecttype, ws.sequence) seq_op
ORDER BY objecttype

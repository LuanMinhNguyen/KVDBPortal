--------------------------------------------------------
--  File created - Tuesday-July-14-2020   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Procedure AA_KPIPLANINMONTHOFPLATFORM
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "EAMDB"."AA_KPIPLANINMONTHOFPLATFORM" (platform varchar2, returnvalue OUT NUMBER)
IS
BEGIN
 select 
CASE total
   WHEN 0 THEN 0
   ELSE round(completed/total*100)
END into returnvalue
from
(select count(case when (evt_type='PPM' and evt_status ='C' and to_char(evt_completed,'mm/yyyy') = to_char(SYSDATE,'mm/yyyy') and evt_org = platform) then 1 end) as completed, 
            count(case when (evt_type='PPM' and evt_status not in ('CANC') and to_char(evt_target,'mm/yyyy') = to_char(SYSDATE,'mm/yyyy') and evt_org =platform) then 1 end) as total
from r5events)
;
END AA_KPIPlanInMonthOfPlatform;

/

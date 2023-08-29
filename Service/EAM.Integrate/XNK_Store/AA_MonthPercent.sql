--------------------------------------------------------
--  File created - Tuesday-July-14-2020   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Procedure AA_MONTHPERCENT
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "EAMDB"."AA_MONTHPERCENT" (platform varchar2, discipline varchar2, returnvalue OUT NUMBER)
IS
BEGIN
select CASE total
   WHEN 0 THEN 0
   ELSE round(completed/total*100)
END into returnvalue
from (select count(case when (evt_type='PPM' and evt_status ='C' and to_char(evt_COMPLETED,'mm/yyyy') = to_char(add_months(SYSDATE,-1),'mm/yyyy') and evt_org =platform and evt_mrc = discipline) then 1 end) as completed, 
count(case when (evt_type='PPM' and evt_status not in ('CANC') and to_char(evt_target,'mm/yyyy') = to_char(add_months(SYSDATE,-1),'mm/yyyy') and evt_org =platform and evt_mrc = discipline) then 1 end) as total
from r5events);
END AA_MonthPercent;

/

--------------------------------------------------------
--  File created - Tuesday-July-14-2020   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Procedure AA_PLANMONTHCOMPLETED
--------------------------------------------------------
set define off;

  CREATE OR REPLACE EDITIONABLE PROCEDURE "EAMDB"."AA_PLANMONTHCOMPLETED" (platform varchar2, discipline varchar2, returnvalue OUT NUMBER)
IS
BEGIN
select count(*) into returnvalue from r5events
where evt_type = 'PPM' and evt_status = 'C' and to_char(evt_target,'mm/yyyy') = to_char(SYSDATE,'mm/yyyy')
and evt_org = platform and evt_mrc = discipline;

END AA_PlanMonthCompleted;

/

CREATE OR REPLACE PROCEDURE DeleteObject(objType varchar2, objName varchar2)
IS
    v_counter number := 0;
    begin
          if objType = 'TABLE' then
            select count(*) into v_counter from user_tables where table_name = upper(objName);
            if v_counter > 0 then          
                  execute immediate 'drop table ' || objName || ' cascade constraints';
            end if;
        end if;
          if objType = 'PROCEDURE' then
            select count(*) into v_counter from User_Objects where object_type = 'PROCEDURE' and OBJECT_NAME = upper(objName);
            if v_counter > 0 then          
                    execute immediate 'DROP PROCEDURE ' || objName;
            end if;
        end if;
          if objType = 'FUNCTION' then
            select count(*) into v_counter from User_Objects where object_type = 'FUNCTION' and OBJECT_NAME = upper(objName);
            if v_counter > 0 then          
                    execute immediate 'DROP FUNCTION ' || objName;
            end if;
        end if;
          if objType = 'TRIGGER' then
            select count(*) into v_counter from User_Triggers where TRIGGER_NAME = upper(objName);
            if v_counter > 0 then          
                    execute immediate 'DROP TRIGGER ' || objName;
            end if;
        end if;
          if objType = 'VIEW' then
            select count(*) into v_counter from User_Views where VIEW_NAME = upper(objName);
            if v_counter > 0 then          
                    execute immediate 'DROP VIEW ' || objName;
            end if;
        end if;
          if objType = 'SEQUENCE' then
            select count(*) into v_counter from user_sequences where sequence_name = upper(objName);
            if v_counter > 0 then          
                    execute immediate 'DROP SEQUENCE ' || objName;
            end if;
        end if;
    end;
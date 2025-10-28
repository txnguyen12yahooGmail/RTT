
-- server = FTFA-DB-OM15V ;User Id=d_pims3_main_user1;Password={THx'60jju4ox8d  
Use D_eglin_pims
Go

ALTER LOGIN [d_pims3_main_user1] WITH CHECK_POLICY = OFF

ALTER LOGIN [d_pims3_main_user1] WITH CHECK_EXPIRATION = OFF
 
ALTER LOGIN [d_pims3_main_user1] WITH PASSWORD = '{THx''60jju4ox8d'  -- escape '\'  -- {THx\'60jju4ox8d  -- or {THx''60jju4ox8d
 
ALTER LOGIN [d_pims3_main_user1] WITH CHECK_POLICY = ON

ALTER LOGIN [d_pims3_main_user1] WITH CHECK_EXPIRATION = ON




SELECT @@SERVERNAME AS ServerName, SL.name AS LoginName ,LOGINPROPERTY(SL.name, 'PasswordLastSetTime') AS PasswordLastSetTime 
,ISNULL(CONVERT(varchar(100),LOGINPROPERTY(SL.name, 'DaysUntilExpiration')),'Never Expire') AS DaysUntilExpiration 
,ISNULL(CONVERT(varchar(100),DATEADD(dd, CONVERT(int, LOGINPROPERTY(SL.name, 'DaysUntilExpiration')),convert(int, getdate())),120),'Never Expire') AS PasswordExpirationDate 
, CASE WHEN is_expiration_checked = 1 THEN 'TRUE' ELSE 'FALSE'
  END AS PasswordExpireChecked  
FROM master.sys.sql_logins AS SL  
WHERE SL.name NOT LIKE '##%' AND SL.name NOT LIKE 'endPointUser' and is_disabled = 0  
--ORDER BY  (LOGINPROPERTY(SL.name, 'PasswordLastSetTime')) DESC 
order by SL.name  


--select * from Products p where MFGPartNumber like 'SHACKLE\_R25%' escape '\'  --{THx'60jju4ox8d


--* Needs SA permissions to do this: Reset sqlUser's Pass Here *-- 
ALTER LOGIN [t_pims2_warehouse_user1] WITH CHECK_POLICY = OFF

ALTER LOGIN [t_pims2_warehouse_user1] WITH CHECK_EXPIRATION = OFF
 
ALTER LOGIN [t_pims2_warehouse_user1] WITH PASSWORD =  '{THx''60jju4ox8d'  -- escape '\'  --{THx'60jju4ox8d
 
ALTER LOGIN [t_pims2_warehouse_user1] WITH CHECK_POLICY = ON

ALTER LOGIN [t_pims2_warehouse_user1] WITH CHECK_EXPIRATION = ON
 



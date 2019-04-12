use endar
select r.ID, r.USER_ID, r.EMAIL,m.MESSAGE_DATE, 
 m.MESSAGE_CATEGORY, m.MESSAGE_SUBJECT, m.MESSAGE_BODY, m.FROM_ADDRESS,  
m.SITE_ID,  
 r.SENT_DATE, r.PROCESS_RESULT,m.BATCH_ID, m.TRANSMIT_PRIORITY  from NOTIF_MESSAGE_RECIPIENT r 
  left join USERS u on r.USER_ID = u.USER_ID  
  inner join NOTIF_MESSAGE m on r.MESSAGE_ID = m.ID  
  where (u.ISACTIVE = 'true' or u.ISACTIVE is null)  
  and (r.SENT_DATE is null or PROCESS_RESULT is null or PROCESS_RESULT =0) 
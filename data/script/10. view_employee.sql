ALTER VIEW view_employee
AS

	SELECT e.emp_id,emp_name,date_of_birth,joining_date,salary,designation,email_id, 
	ISNULL( STUFF(
			 (SELECT ', ' + name
			  FROM skills s
			  left join emp_X_skill es on es.skill_id = s.id
			  where  es.emp_id = e.emp_id
			  FOR XML PATH (''))
			  , 1, 1, '') ,'') AS skills
		from employee e
		group by e.emp_id,emp_name,date_of_birth,joining_date,salary,designation,email_id


CREATE PROCEDURE dbo.sp_employee_ins_upd_bulk
@emp tt_employee readonly
AS
BEGIN

	merge into employee dest
	using (select emp_name,date_of_birth,joining_date,salary,designation,email_id from @emp) sou on sou.emp_name = dest.emp_name
	WHEN MATCHED THEN
		UPDATE SET
			dest.[emp_name]		= sou.[emp_name],
			dest.[date_of_birth]= sou.[date_of_birth],
			dest.[joining_date] = sou.[joining_date],
			dest.[salary]	 	= sou.[salary],
			dest.[designation] 	= sou.[designation],
			dest.[email_id] 	= sou.[email_id]
		
	WHEN NOT MATCHED BY TARGET THEN
			INSERT (emp_name,date_of_birth,joining_date,salary,designation,email_id)
			VALUES (sou.emp_name, sou.date_of_birth,sou.joining_date,sou.salary,sou.designation,sou.email_id);

--declare @emp tt_employee 

--insert into @emp (emp_name, skills) values ('Mukesh', 'SQL,.NET,Node,React,MVC'), ('Raj','.Net,JQuery,MySQL,MVC')

--select a.emp_name, (select item from SplitString(b.skills,','))sd
--from @emp a
--join @emp b on a.emp_name = b.emp_name
	
END
GO

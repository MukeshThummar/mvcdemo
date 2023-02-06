SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE dbo.sp_employee_get
@emp_id int = 0
AS
BEGIN

	SET NOCOUNT ON;

	SELECT emp_id,emp_name,date_of_birth,joining_date,salary,designation,email_id, s.name skills
	from employee e
	left join emp_X_skill es on es.emp_id = e.emp_id
	left join skills s on es.skill_id = s.ID
	where ISNULL(@emp_id,0) = 0 or emp_id = @emp_id
END
GO

ALTER PROCEDURE dbo.sp_employee_ins_upd
@emp_id int,
@emp_name VARCHAR(128),
@date_of_birth DATETIME,
@joining_date DATETIME,
@designation VARCHAR(64),
@salary decimal(18,2),
@email_id VARCHAR(128),
@skills varchar(max)
AS
BEGIN

	IF EXISTS (SELECT 1 FROM employee where emp_id != @emp_id and emp_name = @emp_name)
	BEGIN
		RAISERROR('Employee Name already exists',18,1);
		return;
	END
	
	IF EXISTS(SELECT 1 FROM employee where emp_id = @emp_id)
	BEGIN
		UPDATE employee set
			[emp_name] =@emp_name,
			[date_of_birth] =@date_of_birth,
			[joining_date] =@joining_date,
			[salary] = @salary, 
			[designation] =@designation,
			[email_id] = @email_id
		WHERE [emp_id] = @emp_id
	END
	ELSE
	BEGIN
		INSERT INTO employee (emp_name,date_of_birth,joining_date,salary,designation,email_id)
		VALUES (@emp_name, @date_of_birth,@joining_date,@salary,@designation,@email_id)
		SET @emp_id = SCOPE_IDENTITY()
	END

	/*Set Skills*/
	select RTRIM(LTRIM(item)) skills into #skill from SplitString(@skills,',') 

	IF EXISTS (select 1 from #skill p left join skills s with(nolock) on p.skills = s.name where s.ID is null)
	BEGIN
		INSERT INTO skills (name)
		select p.skills from #skill p
		left join skills s with(nolock) on p.skills = s.name
		where s.ID is null
	END
	
	INSERT INTO emp_X_skill (emp_id, skill_id)
	select @emp_id, s.ID 
	from #skill p
	left join skills s with(nolock) on p.skills = s.name
	left join emp_X_skill es on es.emp_id = @emp_id and es.skill_id = s.ID
	where s.ID is not null  and es.id is null


	delete from emp_X_skill where emp_id = @emp_id and skill_id not in (select s.ID 
	from #skill p
	left join skills s with(nolock) on p.skills = s.name
	where s.ID is not null 
	)

	drop table #skill

END
GO

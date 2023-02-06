-- Create the data type
CREATE TYPE tt_employee AS TABLE 
(
	emp_id int null,
	emp_name varchar(128) null,
	date_of_birth datetime null,
	joining_date datetime null,
	skills varchar(max) null,
	salary numeric(18,2) null,
	designation varchar(128) null,
	email_id varchar(128) null
)
GO


- Create `appsettings.json`.

    ```json
    {
        "SqlConnectionString": "<sql-connection-string>"
    }
    ```

- Create table and enable change tracking.

    ```sql
    CREATE TABLE [SqlExtension].[Employees] (
        EmployeeId int PRIMARY KEY,
        FirstName varchar(255),
        LastName varchar(255),
        Company varchar(255),
        Team varchar(255)
    );

    ALTER DATABASE [jasanghv-db]
    SET CHANGE_TRACKING = ON  
    (CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON);

    ALTER TABLE [SqlExtension].[Employees]
    ENABLE CHANGE_TRACKING
    WITH (TRACK_COLUMNS_UPDATED = ON);
    ```

- Add rows to the table to test SQL trigger.

    ```sql
    INSERT INTO [SqlExtension].[Employees] VALUES
        (1, 'Hello1', 'World1', 'Microsoft1', 'Functions1'),
        (2, 'Hello2', 'World2', 'Microsoft2', 'Functions2'),
        (3, 'Hello3', 'World3', 'Microsoft3', 'Functions3');
    ```

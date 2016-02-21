How Migrations work in Performance Labs : 

We use the Code First approach provided by Entity Framework. All the database schema is defined in the Models folder. Configurations for the Model and relationships among different objects is defined in the Configurations folder. Since all the schema is defined in C# code, we make use of the Migrations feature in EF to make incremental changes to our schema. 

The db Migrations feature in Entity Framework is powered by the Nuget Package Manager Console. 

Please follow the following steps to add a migration - 

1. Open Visual Studio -> Tools -> Library Package Manager -> Package Manager Console (assuming Nuget Package Manager is installed). This should open the Command Line Interface Package Manager Console.

2. Say you want to add a column X to one of your tables T. Go to the POCO class for T. Add a property for X.

3. Now you would want to add the .cs file that lists this change. This is going to be our Migration file in the Migrations folder. This is a simple matter of typing the following command on the Package Manager Console - 
  > Add-Migration AddColumnX		(press Enter\Return)
	where AddColumnX is any meaningful name you want to give to your Migration(generally reflecting what you meant to change in the schema).

Once this is done, you will see the file xyztimestamp_AddColumnX.cs(and a corresponding designer.cs file) file in the Migrations folder. The 'Up' method will list the exact change to be applied to the db in order for the schema change to take effect. There is a 'Down' method too. This will be the exact opposite of the 'Up' method and will come into play when you want to migrate down.

4. Another command to be executed on the Package Manager Console
   > Update-Database -verbose
   This will migrate the db to the most recent migration. 
PerformanceLabs is not configured to automatically update its db to the most recent migration. Note that in order to avoid any chance of data loss it would be better to take backup of the database first.

Some other useful commands
1. Update-Database –TargetMigration:"XYZ" -Verbose -> used to migrate(up or down) to whatever migration is mentioned as TargetMigration.
2. Enable-Migrations -> general EF command to enable migrations in any EF enabled project. Not to be used with PerformanceLabs. This command is used just once when migrations are enabled for any project.
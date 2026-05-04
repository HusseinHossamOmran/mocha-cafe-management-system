# Mocha Cafe Management System

A multi-branch cafe management system built using C#, SQL Server, and Windows Forms.

## Features
- Cashier POS system
- Real-time kitchen display system
- Multi-branch restaurant management
- Order lifecycle tracking
- Automated inventory deduction using SQL triggers
- Relational database with 11 tables and strong data integrity constraints

## Technologies Used
- C#
- Windows Forms
- SQL Server
- Visual Studio

## My Contribution
- Designed core database tables and relationships
- Implemented order and order details workflow
- Built POS and kitchen display logic
- Added backend financial calculations
- Implemented SQL triggers for automated inventory deduction

## How to Run
1. Open `DBapplication.sln` in Visual Studio.
2. Run `SQL/Mocha.sql` to create the database schema.
3. Run `SQL/Mocha_Data.sql` to insert sample data.
4. Update the SQL Server connection string in `DBapplication/app.config` if your local SQL Server instance uses a different name.
5. Run the Windows Forms application.

## Notes
- The project uses Windows Authentication with SQL Server.
- If your SQL Server instance is named `SQLEXPRESS`, update the connection string accordingly.

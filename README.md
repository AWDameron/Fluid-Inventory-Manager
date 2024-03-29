Fluid Inventory Management System

This is a dual table inventory manager designed to allow a technician to maintain an inventory of available fluids and allocate those fluids to a project number. 

Required Features: 
1. CRUD API
2. Asynchronous Programming
3. A Database with 2 or more tables and interacting with a JOIN
4. Create a Dictionary/List and pull from that list


Program Start Up:
NOTE: You will need to download .Net 8.0 before running the software, https://dotnet.microsoft.com/en-us/download, once completed you will need to install the tool in your IDE using the Terminal 
 and typing in the following, <dotnet tool insall --global dotnet-ef>
1. Clone in from the Git Hub
2. Open your IDE of choice, if you're using Visual Studio Code you should open the folder
3. Open the Terminal
4. Set up the database with the following command, dotnet ef database update
5. Press the Run button (If you're running in Chrome and its giving you a privacy issue, you can click the drop down box and click run it as http instead of https)

Here are some Sample Fluids and Fluid Allocations, you may also use your own values:

Lot Number: LOT-110B, LOT-120B, LOT-130B
Fluid Name: Isopropyl Alcohol, Acetone, Oil
Total Quantity: 5, 5, 10
Notes: Flammable, Flammable, 5 year shelf life
Date: <Date of your choosing>

Customer Number: 12345, 53443, 17532
Quantity Used: 2, 3, 5
Lot Number: LOT-110B, LOT-120B, LOT-130B





Special Thanks to Beta Testers: Monkies, Julia, Mrs. Donut, Chiyo
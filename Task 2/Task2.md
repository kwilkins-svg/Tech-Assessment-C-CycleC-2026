1. Open SQL Server Management Studio (or an equivalent SQL Management Software)
2. Connect to Northwind DB (Same DB as you used in Geek Week)
3. Write a query that shows each employee’s first name and the number of orders they have placed. Group the results by employee’s first name.

Please add your SQL query below AND add explanation of the query: 

```
[Insert Code Here]
SELECT
    e.FirstName,
    COUNT(o.OrderID) AS NumberOfOrders
FROM Employees AS e
JOIN Orders AS o
    ON e.EmployeeID = o.EmployeeID
GROUP BY e.FirstName
ORDER BY NumberOfOrders DESC;
[Insert Explanation Here]
This query joins the Employees and Orders tables on EmployeeID so each order is matched to the employee who placed it, then groups the rows by first name and uses COUNT to total each employee’s orders. The results are sorted from most orders to fewest, showing 9 employees with 830 orders total, led by Margaret with 15. 

4. For submission commit and push this .md file to Github

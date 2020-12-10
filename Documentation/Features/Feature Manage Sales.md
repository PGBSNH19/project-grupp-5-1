**Feature: Manage Sales**
As an Admin, i want to be able to place different products
on sale for a limited time.

**Scenario: Place product on sale**
Given I currently are on the adminpage have listed all products
Then I can specify discount amount in precentage into a input field next to the product
And click a button to Save the new changes to place the product on sale

**Scenario: Take a product off sale**
Given I currently are on the adminpage have listed all products
Then I erase the text in the sale input field
And click a button to Save the new changes to take the product off sale
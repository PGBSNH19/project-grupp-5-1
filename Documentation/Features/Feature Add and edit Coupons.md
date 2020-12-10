**Feature: Add and Inactivate Coupons**
As an Admin, I want to add and inactivate a Coupons
so that customers can use them on special occations to buy products
or be able to inactivate Coupons so they are no longer valid

**Scenario: Adding a Coupon**
Given I currently are on the adminpage
And I click a button that creates a new coupon
When I have typed in a Coupon name
And specified startdate,  enddate, discount amount
And clicked a button to save
Then a coupon with specified details is created

**Scenario: Inactivate a Coupon**
Given I currently are on the adminpage
And have listed all active coupons
And have clicked a checkbox next to the coupon name
And clicked a button to save the changes
Then the coupon will be deactivated and are no longer valid
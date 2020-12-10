**Feature: Add and edit Coupons**

As an Admin, I can add and edit a Coupons
so that customers can use them on special occations to buy products

**Scenario: Adding a Coupon**
Given I currently are on the adminpage
When I click a button that creates a coupon
Then I can type in Coupon Name
And specify startdate
And specify enddate
And specify discount amount
Then a coupon with specified details is created

**Scenario: Editing a Coupon**
Given I currently are on the adminpage
And have listed all active coupons
Then I can choose a coupon on the list and click a button to Edit the coupon
And there I will be able to set an active coupon to inactive
And be able to change dates
And be  able to change name
Then I click a button to Save the new changes
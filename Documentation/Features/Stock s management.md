# Stocks Management

| Sell products                                                |
| ------------------------------------------------------------ |
| As a **admin**<br />I want to **reduct the bought products quantity from its stocks balance in the central store**<br />So that i **can control always the stock balance** |
| **Scenario 1: Customer buy a product  from the website**<br />- Given that the customer is presented with a list of available products<br />- When the customer sees an interesting product<br />- Then the customer chooses the product and buy it.<br />- Then the system deduct the item amount from stocks balance in database. |



| Return products                                              |
| ------------------------------------------------------------ |
| As a **admin**<br />I want to **add the returned products quantity to its stocks balance in the central store**<br />So that **i can sell again** |
| **Scenario 1: Customer returns unwanted bought product**<br />- Given that the customer does not want this bought product from website<br />- When he does not like it,  small or big size..  <br />- Then the customer returns the product to our store.<br />- Then the store workers scan the product back to the system.<br />-Then the product added automatically and increase the stock balance in the system. |



| Buy products                                                 |
| ------------------------------------------------------------ |
| As a **admin**<br />I want to **add the bought products from the supplier to its stocks balance in the central store**<br />So that **i can sell on my store.** |
| **Scenario 1: Seller buy new products from supplier**<br />- Given that the seller is authorized to buy new products<br />- When the seller clicks "Buy or register products"-button<br />- Then the database stocks balance will update automatically by adding the new item to the balance. |



| Wasted products                                              |
| ------------------------------------------------------------ |
| As a **admin**<br />I want to **have option of waste items**<br />So that **i can deduct this wasted items from my store balance.** |
| **Scenario 1: Seller registers a new product**<br />- Given that the seller is authorized to waste some products<br />- When the seller find any product in the physical store is damaged or returned from customer of damage reason<br />- Then the seller can waste item by send it to the waste table and deduct from database automatically. |



| Out of stock alert                                           |
| ------------------------------------------------------------ |
| As a **admin**<br />I want to **get alert when the product is going to be out of stock**<br />So that **i can do another order from the supplier.** |
| **Scenario 1: Seller get an system alert **<br />- Given that the seller is getting an system alert<br />- When the any product's amount being equal to specific amount<br />- Then the seller can order new products from the supplier again. |


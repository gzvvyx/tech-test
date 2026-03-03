# Solution

## Validation

Validation is either done by route constrains or using Fluent Validation validators.

## DTOs

Created `CreateOrderDto` and `CreateOrderItemDto` containing necessary fields to create new Order.

## Endpoints

### GET /orders
Already Implemented.

---

### GET /orders/{orderId}
Already Implemented, added guid constraint.

**Validation**
- `orderId` must be a valid GUID — enforced by the `:guid` route constraint
- non-GUIDs are rejected before reaching the controller

---

### GET /orders/{status}
Returns all orders matching given status.

**Validation**
- `status` must be a valid `OrderStatus` enum value
- Invalid values are rejected automatically

---

### PATCH /orders/{orderId}/{status}
Updates the status of an existing order.

**Validation**
- `orderId` must be a valid GUID — enforced by the `:guid` route constraint
- `status` must be a valid `OrderStatus` enum value

**Logic**
- Returns `404NotFound` if no order exists with the given ID
- If the order already has the requested status, no update is performed and the current order is returned
- Shouldn't happen, but if new status doesn't exist throw an exception

---

### POST /orders
Creates a new order.

**Validation**
- `resellerId` — required, must be a non-empty GUID
- `customerId` — required, must be a non-empty GUID
- `orderItems` — must contain at least one item
- Each item requires a non-empty `productId`, non-empty `serviceId`, and `quantity` greater than 0

**Logic**
- Order ID is generated server-side
- Status is automatically set to `Created` on creation — callers cannot set the initial status
- `createdDate` is set to UTC now server-side

---

### GET /orders/profit
Returns profit grouped by month, ordered by year and month ascending.

**Logic**
- Only `Completed` orders are included
- Profit is calculated as `totalPrice - totalCost` per order, then aggregated by month
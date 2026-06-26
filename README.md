# Project Description

An online pharmacy shop system where users can search for medicines and logged in customers can buy them. The staff can do crud operations on medicines but cannot buy medicines how users buy with carts.

Users must be able to enter the website and look for their specific medicines. Staff members must be able to manage medicines.

# Requirements

1. Razor pages
2. MVC Controller for API
3. Entity Framework Core
4. LINQ
5. Signal R
6. Graphic interface layout

# ER Diagram

```mermaid
erDiagram

Company || -- o{ Medicine : "Manufactures"
Cart || -- o{ CartItem : "Contains"
Medicine || -- o{ CartItem : "Is in"
Supplier }o -- o{ Medicine : "Supplies"
Payment o| -- || Cart : "Pays"

Medicine{
    id Integer PK
    company_id int FK
    name varchar(50)
    retail_price decimal
    image_src varchar "nullable"
}
Company{
    id Integer PK
    name varchar(50)
}
Supplier{
    id Integer PK
    name varchar(50)
}
Cart{
    id Integer PK
    total_cost decimal
    total_quantity int
}
CartItem{
    id Integer PK
    cart_id int FK
    medicine_id int FK
    quantity int
    unit_price decimal
}
Payment{
    id Integer PK
    cart_id int FK
    amount decimal
}
Customer{
    id Integer PK
    user_id string FK
    first_name varchar(50)
    last_name varchar(50)
    age int
    address varchar(200)
    phone_number varchar(20) "nullable"
}
NonCustomer{
    id Integer PK
    user_id string FK
    first_name varchar(50)
    last_name varchar(50)
    age int
}

```

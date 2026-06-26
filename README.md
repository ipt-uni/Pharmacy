# Project Description

An online pharmacy shop system where users can search for medicines and logged in customers can buy them. The staff can buy medicines, stock them and talk with customer complaints. So there will be three roles for staff members: Stockers, Purchasers, CustomerSupport. The CEO do all this plus having access to every staff info and can hire or fire a staff member.

Users must be able to enter the website and look for their specific medicines. Staff members must be able to mo

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

```

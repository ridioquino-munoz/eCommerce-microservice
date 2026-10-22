# E-Commerce Api
REST API build with Asp.Net Core

## Feature
- JWT Authentication
- Product Management
- Order Processing
 
<h2>Technologies</h2>
<table>
  <tr>
    <td>Asp.Net Core</td>
    <td>Sql</td>
    <td>Entity Framework</td>
  </tr>
  <tr>
    <td>RabbitMQ</td>
    <td>Redis</td>
    <td>Docker</td>
  </tr>
   <tr>
    <td>Microservices</td>
    <td>RestApi</td>
    <td>C#</td>
  </tr>
</table>

<h2>Project Setup</h2>

1. clone the repository 

     git clone https://github.com/ridioquino-munoz/eCommerce-microservice.git

2. setup docker services

    docker compose up -d

3. Setting up databases

   <p>update-database --context identitydbcontext</p>
   <p>update-database --context productdbcontext</p>
   <p>update-database --context inventorydbcontext</p>

4. Clean and Build

5. Setup Start up project. In solution > Setup startup project > Common properties > Configure Startup Project > Multiple startup project

Select start for all api projects (API.Identity, API.ProductCatalog, API.Inventory, API.Cart)

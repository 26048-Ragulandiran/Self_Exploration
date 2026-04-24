## Steps to run the application,


#### Create the .env file and add these,

 - DB_SQL_CONNECTION=sql_connection_uri

#### Execute these commands before running,

##### For migration,
 - dotnet ef migrations add ItineraryMigration
 - dotnet ef database update

	
##### Add or install these packages or just build to install automatically through nuget, 

 - Serilog
 - FluentValidation
 - EntityFramework
 - Pomelo [MySQL]
 - Swashbuckle [Swagger]


### gRPC Testing with Postman

## 1. Prerequisites Check

Before testing, ensure the following:

### Application is running
You should see: Now listening on: [https://localhost:7118](https://localhost:7118)


## 2. Open Postman (gRPC Mode)

1. Open Postman
2. Click **New**
3. Select **gRPC Request**

## 3. Set Server URL

Use the HTTPS endpoint from your application:

```
https://localhost:7118
```

Click **Connect**

## 4. Import .proto File

1. Click **Import a .proto file**
2. Select:

```
itinerary.proto
```

After import, Postman will display:

```
ItineraryGrpc
 ├── GetAll
 ├── GetById
 ├── Create
 ├── Update
 ├── Delete
```

## 5. Testing Each Method

### GetById

Select:
`GetById`

Message:

```json
{
  "id": 1
}
```

Click **Invoke**

### Create

Select:
`Create`

Message:

```json
{
  "destination": "Chennai",
  "travelDate": "2026-04-24",
  "durationDays": 5
}
```

Click **Invoke**

### Update

Select:
`Update`

Message:

```json
{
  "id": 1,
  "destination": "Bangalore",
  "travelDate": "2026-05-01",
  "durationDays": 4
}
```

Click **Invoke**

### Delete

Select:
`Delete`

Message:

```json
{
  "id": 1
}
```

Click **Invoke**

### GetAll

Select:
`GetAll`

Message:

```json
{
  "destination": "",
  "pageNumber": 1,
  "pageSize": 10,
  "travelDate": ""
}
```

Click **Invoke**

## 6. Expected Behavior

| Operation | Result                   |
| --------- | ------------------------ |
| Create    | New record inserted      |
| GetById   | Returns single itinerary |
| Update    | Updates existing record  |
| Delete    | Removes record           |
| GetAll    | Returns paginated list   |

```
```

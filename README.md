The focus of this project was a simple implementation of Sagas using MassTransit and RabbitMQ.

![#](https://github.com/HenrySaldanha/AspNet.MassTransit.Sagas/blob/main/Images/api.png?raw=true)
![#](https://github.com/HenrySaldanha/AspNet.MassTransit.Sagas/blob/main/Images/Microsservices.png?raw=true)

## Run

* To run RabbitMQ, use the command **docker-compose up** in the directory of the file docker-compose.yaml.
* Run the projects **PurchaseSaga**,**PurchaseSaga**,**ProductWorker** and **Api**. 
* Run the **/purchase** end point to start the saga
* If you fill the **expirationDate** field with a value in the past, the saga will perform rollback during execution, if the value is in the future, the saga will end perfectly.

## Give a Star 
If you found this Implementation helpful or used it in your Projects, do give it a star. Thanks!

## This project was built with
* [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [MassTransit](https://masstransit-project.com/)
* [Swagger](https://swagger.io/)
* [Serilog](https://serilog.net/)
* [RabbitMQ](https://www.rabbitmq.com/)

## My contacts
* [LinkedIn](https://www.linkedin.com/in/henry-saldanha-3b930b98/)
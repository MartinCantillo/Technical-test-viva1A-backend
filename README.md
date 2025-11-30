# PatientsBackend API

## Descripción del proyecto

Este proyecto es una API REST para la gestión de pacientes, desarrollada en **.NET** y utilizando **SQL Server 2022** como base de datos.  
Se implementó la arquitectura **limpia**, usando el **patrón Repository** para separar la lógica de acceso a datos de la lógica de negocio, lo que permite:

- Mejor mantenimiento y escalabilidad.
- Facilitar la realización de pruebas unitarias.
- Abstracción del acceso a la base de datos, desacoplando la implementación concreta de SQL Server de la lógica de la aplicación.

La API expone endpoints para manejar pacientes y sus datos asociados, y está lista para ser consumida por aplicaciones front-end o integraciones externas.

---

## Arquitectura

- **Controllers**: Exponen los endpoints HTTP de la API.  
- **Repository / Business Logic**: Contienen la lógica de negocio.  
- **IRepository**: Encapsula el acceso a la base de datos.  
- **Entities / Models**: Representan las tablas y objetos del dominio.  
- **DbContext**: Configuración de Entity Framework Core (si se usa EF).  

Esta separación facilita **pruebas unitarias, mantenimiento y escalabilidad**.

---

## Base de datos

- Se utiliza **SQL Server 2022**.
- La base de datos principal se llama `bdPatient`.
- Se incluye un **script de inicialización** `init.sql` que:
  - Crea la base de datos `bdPatient` si no existe.
  - Crea las tablas necesarias.
  - Crea procedimientos almacenados para operaciones comunes.

### Inicializar la base de datos con Docker

1. Levantar los contenedores:


docker-compose up -d
Esto creará:

sqlserver-patients: Contenedor de SQL Server 2022 en el puerto 1433.

api-patients: Contenedor de la API en el puerto 8080.

Copiar el script de inicialización al contenedor:

por ejemplo : 
cd C:\Users\marti\Desktop\backend-viva1A\sql
docker cp .\init.sql sqlserver-patients:/init.sql

Ejecutar el script desde Windows usando sqlcmd:

sqlcmd -S 127.0.0.1,1433 -U sa -P "Container123!" -i C:\Users\marti\Desktop\backend-viva1A\sql\init.sql


Nota: Debes tener instalado sqlcmd en Windows. Para instalarlo:

winget install Microsoft.MicrosoftSQLServer.SqlCmd


Esto creará la base de datos bdPatient y sus tablas/procedimientos almacenados.

Verificar la base de datos
sqlcmd -S 127.0.0.1,1433 -U sa -P "Container123!" -Q "SELECT name FROM sys.databases;"

Ejecutar la API

La API se expone en http://localhost:8080.

Para acceder a Swagger y probar los endpoints:

http://localhost:8080/swagger


Ejemplo de consulta directa a la tabla Patients:

sqlcmd -S 127.0.0.1,1433 -U sa -P "Container123!" -d bdPatient -Q "SELECT * FROM Patients;"

Reiniciar la aplicación

Si quieres reiniciar la aplicación y la base de datos:

docker-compose down -v
docker-compose up -d


Luego, repite los pasos de inicialización para poblar la base de datos con init.sql.


Notas

La API depende de la base de datos bdPatient y de que los procedimientos almacenados hayan sido ejecutados.

La arquitectura con Repository Pattern permite un código más limpio, mantenible y escalable.


Ademas se adjunta proyecto con pruebas unitarias para su ejecucion

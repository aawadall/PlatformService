# Platform Service
Follow along with [Les Jackson's Course](https://www.youtube.com/watch?v=DgVjEo3OGBI) 

## Getting Started
1. Clone the repo
2. Build images:
    - `docker build -t <your_docker_user>/platform_service ./PlatformService 
    - `docker build -t <your_docker_user>/command_service ./CommandService`
3. Push images to Docker Hub:
    - `docker push <your_docker_user>/platform_service`
    - `docker push <your_docker_user>/command_service`
4. Deploy SQL Server:
    - Create Secret with name `mssql` and key `SA_PASSWORD` 
    - `kubectl apply -f ./K8S/mssql-plat-depl.yaml`
5. Deploy Command Service:
    - `kubectl apply -f ./K8S/commands-depl.yaml`
6. Deploy Platform Service:
    - `kubectl apply -f ./K8S/platform-depl.yaml`
7. Deploy Ingress:
    - `kubectl apply -f ./K8S/ingress.yaml`

**Note:** MS SQL Password is stored in `appsettings.Production.json` and `appsettings.Development.json` files. These files are not included in the repo. You will need to create them and add the password to them.



# CORE 6 SERVICE DEPLOYMENT

## Creates a new Service
    sc.exe create "EET4250Service" binpath="C:\Program Files\dotnet.exe C:\Users\eecsm\OneDrive\Documents\Deployments\EET4250Service\VelocityService.dll"

## Provide a Description for a Service
    sc.exe description EET4250Service "Service to Add Beer Pong Ball Data"

## Start A Service
    sc.exe start EET4250Service

## Stop A Service
    sc.exe stop EET4250Service

## Restart a Service
    sc.exe restart EET4250Service
